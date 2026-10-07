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
        DateTime createdAt,
        Func<PositionName, bool> isNameUnique
    )
    {
        if (!isNameUnique(name))
        {
            throw new InvalidOperationException("Должность с таким названием уже существует в системе.");
        }

        var lifeTime = EntityLifeTime.Create(createdAt, createdAt, isActive: true);
        return new Position(id, name, description, lifeTime);
    }

    public void UpdateName(
        DateTime currentUtcTime,
        Func<PositionName, bool> isNameUnique,
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
            if (!isNameUnique(newPosName))
            {
                throw new InvalidOperationException("Должность с таким названием уже существует в системе.");
            }

            Name = newPosName;
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
        LifeTime = LifeTime with { UpdatedAt = currentUtcTime, IsActive = false };
    }
}