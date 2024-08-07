﻿using Safari.Net.Data.Repositories;
using Safari.Net.Data.Test.TestUtilities;
using Safari.Net.Data.Test.TestUtilities.Models;
using Safari.Net.TestTools;
using Safari.Net.TestTools.Data;
using Safari.Net.TestTools.Data.Factories;

namespace Safari.Net.Data.Test.Entities;

public class EntityServiceTest : UnitTest
{
    private readonly DataFactory<FakeUser> _userFactory;
    private readonly Repository<FakeUser, TestContext> _userRepository;
    private readonly FakeUserService _userService;

    public EntityServiceTest()
    {
        var context = new SqliteMemoryDb<TestContext>().Context;
        _userRepository = new Repository<FakeUser, TestContext>(context);
        _userFactory = new DataFactory<FakeUser>(_userRepository);
        _userService = new FakeUserService(_userRepository);
    }

    [Fact]
    public void Get_ReturnsMappedModelWithCorrectPagination_WhenQueryIsValid()
    {
        const int total = 10;
        const int index = 1;
        const int size = 5;
        _userFactory.CreateMany(total);
        var query = new FakeUserQuery { Index = index, Size = size };
        var result = _userService.Get<FakeUserModel>(query);
        Assert.NotNull(result);
        Assert.Equal(total, result.Total);
        Assert.Equal(size, result.Size);
        Assert.Equal(index, result.Index);
        Assert.Equal(size, result.Count);
    }

    [Fact]
    public void Get_ReturnsCorrectItems_WhenIndexInSecondPage()
    {
        const int total = 10;
        const int index = 2;
        const int size = 5;
        var users = _userFactory.CreateMany(total);
        var query = new FakeUserQuery { Index = index, Size = size, OrderBy = "CreatedAt" };
        foreach (var user in users)
        {
            var i = users.IndexOf(user) + 1;
            user.Username = $"User{i}";
            user.CreatedAt = DateTime.Now.AddDays(-total + i);
            _userRepository.Save();
        }

        var result = _userService.Get<FakeUserModel>(query);
        Assert.Equal("User6", result.Value.First().Username);
    }

    [Fact]
    public void Get_ReturnsError_WhenInvalidOrder()
    {
        const int total = 10;
        var query = new FakeUserQuery { OrderBy = "Lol" };
        _userFactory.CreateMany(total);
        var result = _userService.Get<FakeUserModel>(query);
        Assert.True(result.HasError);
        Assert.NotEmpty(result.Errors);
    }
}