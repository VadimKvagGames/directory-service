using System;
using DirectoryService.Domain.LocationsContext;
using Xunit;

namespace Test;

public class LocationBusinessTests
{
    private Location CreateTestLocation()
    {
        return Location.Create(
            LocationId.Create(Guid.NewGuid()),
            LocationAddress.Create("г. Москва, ул. Ленина, д. 1"),
            LocationName.Create("Старый Офис"),
            IanaTimeZone.Create("Europe/Moscow"),
            DateTime.UtcNow
        );
    }

    [Fact]
    public void Update_ShouldModifyOnlyProvidedFields_AndChangeUpdatedAt()
    {
        var location = CreateTestLocation();
        var newName = LocationName.Create("Контора ОООтмыв Денег");
        var updateTime = DateTime.UtcNow.AddMinutes(5);

        location.Update(updateTime, newName: newName);

        Assert.Equal("Контора ОООтмыв Денег", location.Name.Value);
        Assert.Equal("г. Москва, ул. Ленина, д. 1", location.Address.Value);
        Assert.Equal(updateTime, location.LifeTime.UpdatedAt);
    }

    [Fact]
    public void Update_ShouldThrowException_WhenLocationIsArchived()
    {
        var location = CreateTestLocation();
        location.Archive(DateTime.UtcNow);

        var newName = LocationName.Create("Попытка взлома архива");

        var exception = Assert.Throws<InvalidOperationException>(() =>
            location.Update(DateTime.UtcNow, newName: newName)
        );

        Assert.Equal("Редактирование архивированных локаций запрещено.", exception.Message);
    }
}