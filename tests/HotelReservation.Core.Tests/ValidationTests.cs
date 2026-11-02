using HotelReservation.Core.Models;
using HotelReservation.Core.Security;
using HotelReservation.Core.Services;
using HotelReservation.Core.Validation;

namespace HotelReservation.Core.Tests;

public class ValidationTests
{
    [Theory]
    [InlineData("4111 1111 1111 1111", true)]
    [InlineData("4242-4242-4242-4242", true)]
    [InlineData("4111 1111 1111 1112", false)]
    [InlineData("4111abc111111111", false)]
    [InlineData("1234", false)]
    [InlineData("", false)]
    public void CardNumberIsValidatedWithLuhnCheck(string number, bool expected)
    {
        Assert.Equal(expected, CardValidator.IsValidNumber(number));
    }

    [Theory]
    [InlineData("12/30", true)]
    [InlineData("06/26", true)]
    [InlineData("05/26", false)]
    [InlineData("13/30", false)]
    [InlineData("1230", false)]
    public void CardExpiryMustBeInTheFuture(string expiry, bool expected)
    {
        Assert.Equal(expected, CardValidator.IsValidExpiry(expiry, new DateOnly(2026, 6, 15)));
    }

    [Fact]
    public void OnlyLastFourCardDigitsAreKept()
    {
        Assert.Equal("1111", CardValidator.LastFourDigits("4111 1111 1111 1111"));
    }

    [Theory]
    [InlineData("guest@example.com", true)]
    [InlineData("guest@example", false)]
    [InlineData("guest example.com", false)]
    public void EmailFormatIsChecked(string email, bool expected)
    {
        Assert.Equal(expected, InputValidator.IsValidEmail(email));
    }

    [Theory]
    [InlineData("Secure123", true)]
    [InlineData("short1", false)]
    [InlineData("nodigitshere", false)]
    [InlineData("12345678", false)]
    public void PasswordsMustHaveLettersDigitsAndEightCharacters(string password, bool expected)
    {
        Assert.Equal(expected, InputValidator.IsStrongPassword(password));
    }

    [Fact]
    public void PasswordHashVerifiesOnlyTheOriginalPassword()
    {
        var hash = PasswordHasher.Hash("Secure123");

        Assert.True(PasswordHasher.Verify("Secure123", hash));
        Assert.False(PasswordHasher.Verify("secure123", hash));
        Assert.False(hash.Contains("Secure123"));
    }

    [Fact]
    public void SamePasswordProducesDifferentHashes()
    {
        Assert.NotEqual(PasswordHasher.Hash("Secure123"), PasswordHasher.Hash("Secure123"));
    }

    [Fact]
    public void TotalPriceMultipliesRoomAndBoardByNights()
    {
        var total = PriceCalculator.Total(Catalog.GetRoom("DOUBLE"), Catalog.GetBoard("BB"), 3);

        Assert.Equal(405m, total);
    }
}
