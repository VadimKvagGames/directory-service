using System;
using System.Collections.Generic;
using DirectoryService.Domain.DepartmentContext;
using DirectoryService.Domain.DepartmentContext.ValueObjects;
using DirectoryService.Domain.LocationsContext;
using DirectoryService.Domain.PositionContext;
using Xunit;

namespace Test;

public class DepartmentRankTests
{
    private Department CreateTestDepartment()
    {
        return Department.Create(
            DepartmentId.Create(Guid.NewGuid()),
            DepartmentName.Create("Тестовый отдел"),
            DepartmentSlug.Create("test-dept"),
            null,
            new List<LocationId>(),
            DateTime.UtcNow
        );
    }

    [Fact]
    public void AddPositionWithRank_ShouldSucceed_WhenRankIsUnique()
    {
        var department = CreateTestDepartment();
        var pos1 = PositionId.Create(Guid.NewGuid());
        var pos2 = PositionId.Create(Guid.NewGuid());

        department.AddPositionWithRank(pos1, rank: 1, DateTime.UtcNow);
        department.AddPositionWithRank(pos2, rank: 2, DateTime.UtcNow);

        Assert.Equal(2, department.PositionRanks.Count);
    }

    [Fact]
    public void AddPositionWithRank_ShouldThrowException_WhenRankAlreadyExists()
    {
        var department = CreateTestDepartment();
        var director = PositionId.Create(Guid.NewGuid());
        var secondDirector = PositionId.Create(Guid.NewGuid());

        department.AddPositionWithRank(director, rank: 1, DateTime.UtcNow);

        var exception = Assert.Throws<InvalidOperationException>(() =>
            department.AddPositionWithRank(secondDirector, rank: 1, DateTime.UtcNow)
        );

        Assert.Equal("Должность с таким рангом уже зарегистрирована в данном подразделении.", exception.Message);
    }

    [Fact]
    public void ChangePositionRank_ShouldMoveRankSuccessfully()
    {
        var department = CreateTestDepartment();
        var employee = PositionId.Create(Guid.NewGuid());

        department.AddPositionWithRank(employee, rank: 3, DateTime.UtcNow);


        department.ChangePositionRank(employee, newRank: 2, DateTime.UtcNow);

        var boundRank = Assert.Single(department.PositionRanks);
        Assert.Equal(2, boundRank.Rank);
    }
}