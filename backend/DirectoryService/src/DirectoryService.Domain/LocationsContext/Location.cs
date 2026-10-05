using System;

namespace DirectoryService.Domain.LocationsContext;

public class Location
{
    public LocationId Id { get; private set; }
    public LocationName Name { get; private set; }
    public LocationAddress Address { get; private set; }
    public IanaTimeZone TimeZone { get; private set; }
    public EntityLifeTime LifeTime { get; private set; }

    public Location(
        LocationId id,
        LocationAddress address,
        LocationName name,
        IanaTimeZone timeZone,
        EntityLifeTime lifeTime
    )
    {
        Id = id;
        Address = address;
        Name = name;
        TimeZone = timeZone;
        LifeTime = lifeTime;
    }

    public static Location Create(
        LocationId id,
        LocationAddress address,
        LocationName name,
        IanaTimeZone timeZone,
        DateTime createdAt
    )
    {
        var lifeTime = EntityLifeTime.Create(createdAt, createdAt, isActive: true);

        return new Location(id, address, name, timeZone, lifeTime);
    }

    public void Update(
        DateTime currentUtcTime,
        LocationName? newName = null,
        LocationAddress? newAddress = null,
        IanaTimeZone? newTimeZone = null
    )
    {
        if (!LifeTime.IsActive)
        {
            throw new InvalidOperationException("Редактирование архивированных локаций запрещено.");
        }

        bool isChanged = false;

        if (newName != null)
        {
            Name = newName;
            isChanged = true;
        }

        if (newAddress != null)
        {
            Address = newAddress;
            isChanged = true;
        }

        if (newTimeZone != null)
        {
            TimeZone = newTimeZone;
            isChanged = true;
        }

        if (isChanged)
        {
            LifeTime = LifeTime with { UpdatedAt = currentUtcTime };
        }
    }

    public void Archive(DateTime currentUtcTime)
    {
        if (!LifeTime.IsActive) return;

        LifeTime = EntityLifeTime.Create(LifeTime.CreatedAt, currentUtcTime, isActive: false);
    }
}