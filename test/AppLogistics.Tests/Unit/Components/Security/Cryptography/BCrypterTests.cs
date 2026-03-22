using Xunit;

namespace AppLogistics.Components.Security.Tests;

public class BCrypterTests
{
    private Hasher crypter;

    public BCrypterTests()
    {
        crypter = new Hasher();
    }

    #region Hash(String value)

    [Fact]
    public void Hash_Value()
    {
        string value = "Test";
        string hash = crypter.Hash(value);

        Assert.True(BCrypt.Net.BCrypt.Verify(value, hash));
    }

    #endregion Hash(String value)

    #region HashPassword(String value)

    [Fact]
    public void HashPassword_Value()
    {
        string value = "Test";
        string hash = crypter.HashPassword(value);

        Assert.True(BCrypt.Net.BCrypt.Verify(value, hash));
    }

    #endregion HashPassword(String value)

    #region Verify(String value, String hash)

    [Fact]
    public void Verify_NullValue_ReturnsFalse()
    {
        Assert.False(crypter.Verify(null, ""));
    }

    [Fact]
    public void Verify_NullHash_ReturnsFalse()
    {
        Assert.False(crypter.Verify("", null));
    }

    [Fact]
    public void Verify_Hash()
    {
        Assert.True(crypter.Verify("Test", "$2a$04$tXfDH9cZGOqFbCV8CF1ik.kW8R7.UKpEi5G7P4K842As1DI1bwDxm"));
    }

    #endregion Verify(String value, String hash)

    #region VerifyPassword(String value, String passhash)

    [Fact]
    public void VerifyPassword_NullValue_ReturnsFalse()
    {
        Assert.False(crypter.VerifyPassword(null, ""));
    }

    [Fact]
    public void VerifyPassword_NullHash_ReturnsFalse()
    {
        Assert.False(crypter.VerifyPassword("", null));
    }

    [Fact]
    public void VerifyPassword_Passhash()
    {
        Assert.True(crypter.VerifyPassword("Test", "$2a$13$g7QgmyFicKkyI4kiHM8XQ.LfBdpdcLUbw1tkr9.owCN5qY9eCj8Lq"));
    }

    #endregion VerifyPassword(String value, String passhash)
}
