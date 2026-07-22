using BCryptor = BCrypt.Net.BCrypt;

namespace AppLogistics.Components.Security;

public class Hasher : IHasher
{
    public string Hash(string value)
    {
        return BCryptor.HashString(value, 6);
    }

    public string HashPassword(string value)
    {
        return BCryptor.HashPassword(value, 13);
    }

    public bool Verify(string value, string hash)
    {
        if (value == null)
        {
            return false;
        }

        if (hash == null)
        {
            BCryptor.Verify("TakeSameTime", "$2a$06$L01HfIu56AJsQWhsvzbByujj9XtGht5qJ/rxjA4bsKEJzu7fxQxqu");

            return false;
        }

        return BCryptor.Verify(value, hash);
    }

    public bool VerifyPassword(string value, string passhash)
    {
        if (value == null)
        {
            return false;
        }

        if (passhash == null)
        {
            BCryptor.Verify("TakeSameTime", "$2a$13$06DpsSNHCcSaVJ4cdSfLEeWXs2PYVXQ0bVXvShTt/g0I4t1pTwgTu");

            return false;
        }

        return BCryptor.Verify(value, passhash);
    }
}
