using System;

namespace DirectoryService.Domain.PositionContext;

public class Position
{
    public PositionId Id { get; private set; }
    public PositionName Name { get; private set; }
    public PositionDescription Description { get; private set; }
    public EntityLifeTime LifeTime { get; private set; }

    public Position(
        PositionId id,
        PositionName name,
        PositionDescription description,
        EntityLifeTime lifeTime
    )
    {
        Id = id;
        Name = name;
        Description = description;
        LifeTime = lifeTime;
    }

    public static Position Create(
        PositionId id,
        PositionName name,
        PositionDescription description,
        DateTime createdAt
    )
    {
        var lifeTime = EntityLifeTime.Create(createdAt, createdAt, isActive: true);
        return new Position(id, name, description, lifeTime);
    }

    public void UpdateName(
        DateTime currentUtcTime,
        PositionName? newPosName = null
    )
    {
        if (!LifeTime.IsActive)
        {
            throw new InvalidOperationException("Редактирование архивированных должностей запрещено.");
        }

        bool isChanged = false;

        if (newPosName != null && Name != newPosName)
        {
            Name = newPosName;
            isChanged = true;
        }

        if (isChanged)
        {
            LifeTime = LifeTime.Update(currentUtcTime);
        }
    }

    public void Archive(DateTime currentUtcTime)
    {
        if (!LifeTime.IsActive) return;

        LifeTime = EntityLifeTime.Create(LifeTime.CreatedAt, currentUtcTime, isActive: false);
    }
}