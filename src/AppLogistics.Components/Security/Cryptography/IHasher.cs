namespace AppLogistics.Components.Security;

public interface IHasher
{
    string Hash(string value);

    string HashPassword(string value);

    bool Verify(string value, string hash);

    bool VerifyPassword(string value, string passhash);
}
