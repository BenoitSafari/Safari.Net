using Safari.Net.Data.Entities;

namespace Safari.Net.Data.Test.TestUtilities.Models;

public class FakeUserQuery : Query
{
    public string? Username { get; set; }
    public string? Email { get; set; }
    public virtual FakeRole? Role { get; set; }
}