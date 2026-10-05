using System;

namespace DirectoryService.Domain.LocationsContext;

public class Location
{
    public LocationId Id { get; private set; }
    public LocationName Name { get; private set; }
    public LocationAddress Address { get; private set; }
    public IanaTimeZone TimeZone { get; private set; }
    public EntityLifeTime LifeTime { get; private set; }

    private Location(
        LocationId id,
        LocationAddress address,
        LocationName name,
        IanaTimeZone timeZone,
        EntityLifeTime lifeTime
    )
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        Address = address ?? throw new ArgumentNullException(nameof(address));
        Name = name ?? throw new ArgumentNullException(nameof(name));
        TimeZone = timeZone ?? throw new ArgumentNullException(nameof(timeZone));
        LifeTime = lifeTime ?? throw new ArgumentNullException(nameof(lifeTime));
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
        LocationName newName,
        LocationAddress newAddress,
        IanaTimeZone newTimeZone,
        DateTime currentUtcTime
    )
    {
        if (!LifeTime.IsActive)
        {
            throw new InvalidOperationException("Редактирование архивированных локаций запрещено.");
        }

        Name = newName ?? throw new ArgumentNullException(nameof(newName));
        Address = newAddress ?? throw new ArgumentNullException(nameof(newAddress));
        TimeZone = newTimeZone ?? throw new ArgumentNullException(nameof(newTimeZone));

        LifeTime = EntityLifeTime.Create(LifeTime.CreatedAt, currentUtcTime, LifeTime.IsActive);
    }

    public void Archive(DateTime currentUtcTime)
    {
        if (!LifeTime.IsActive) return;

        LifeTime = EntityLifeTime.Create(LifeTime.CreatedAt, currentUtcTime, isActive: false);
    }
}