using System;
using DirectoryService.Domain.LocationsContext;
using Xunit;

namespace Test;

public class LocationValueObjectsTests
{
    //Тут у меня тесты на название локации (LocationName)

    [Theory]
    [InlineData("Офис")]
    [InlineData("Главный Склад")]
    [InlineData("Коворкинг А")]
    public void LocationName_Create_WithValidValue_ShouldSucceed(string validName)
    {
        var result = LocationName.Create(validName);

        Assert.NotNull(result);
        Assert.Equal(validName, result.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("Оф")]
    [InlineData("123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789")]
    public void LocationName_Create_WithInvalidValue_ShouldThrowArgumentException(string invalidName)
    {
        Assert.Throws<ArgumentException>(() => LocationName.Create(invalidName));
    }

    //Тут тесты на временную зону (IanaTimeZone)

    [Theory]
    [InlineData("Europe/Moscow")]
    [InlineData("Asia/Krasnoyarsk")]
    [InlineData("America/New_York")]
    public void IanaTimeZone_Create_WithValidFormat_ShouldSucceed(string validTimeZone)
    {
        var result = IanaTimeZone.Create(validTimeZone);

        Assert.Equal(validTimeZone, result.Value);
    }

    [Theory]
    [InlineData("Moscow")]
    [InlineData("Europe/")]
    [InlineData("/Moscow")]
    [InlineData("Europe/Moscow/Gmt")]
    [InlineData(null)]
    public void IanaTimeZone_Create_WithInvalidFormat_ShouldThrowArgumentException(string invalidTimeZone)
    {
        Assert.Throws<ArgumentException>(() => IanaTimeZone.Create(invalidTimeZone!));
    }
}