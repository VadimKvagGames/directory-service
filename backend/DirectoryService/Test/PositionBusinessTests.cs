using System;
using DirectoryService.Domain.PositionContext;
using Xunit;

namespace Test;

public class PositionBusinessTests
{
    private Position CreateTestPosition()
    {
        return Position.Create(
            PositionId.Create(Guid.NewGuid()),
            PositionName.Create("Программист"),
            PositionDescription.Create("Пишет отличный код на C#"),
            DateTime.UtcNow
        );
    }

    [Fact]
    public void UpdateName_ShouldModifyName_AndChangeUpdatedAt_WhenPositionIsActive()
    {
        var position = CreateTestPosition();
        var newName = PositionName.Create("Senior C# Разработчик");
        var updateTime = DateTime.UtcNow.AddMinutes(5);

        position.UpdateName(updateTime, newPosName: newName);

        Assert.Equal("Senior C# Разработчик", position.Name.Value);
        Assert.Equal("Пишет отличный код на C#", position.Description.Value);
        Assert.Equal(updateTime, position.LifeTime.UpdatedAt);
    }

    [Fact]
    public void UpdateName_ShouldThrowException_WhenPositionIsArchived()
    {
        var position = CreateTestPosition();
        position.Archive(DateTime.UtcNow);

        var newName = PositionName.Create("Попытка изменения архива");

        var exception = Assert.Throws<InvalidOperationException>(() =>
            position.UpdateName(DateTime.UtcNow, newPosName: newName)
        );

        Assert.Equal("Редактирование архивированных должностей запрещено.", exception.Message);
    }
}