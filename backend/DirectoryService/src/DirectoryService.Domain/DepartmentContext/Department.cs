using System;
using System.Collections.Generic;
using System.Linq;
using DirectoryService.Domain.DepartmentContext.ValueObjects;

namespace DirectoryService.Domain.DepartmentContext;

public class Department
{
    private readonly List<LocationId> _locations = [];
    private readonly List<PositionId> _positions = [];

    public DepartmentId Id { get; private set; }
    public DepartmentName Name { get; private set; }
    public DepartmentSlug Slug { get; private set; }
    public DepartmentId? ParentId { get; private set; }
    public DepartmentPath Path { get; private set; }
    public EntityLifeTime LifeTime { get; private set; }

    public IReadOnlyList<LocationId> Locations => _locations.AsReadOnly();
    public IReadOnlyList<PositionId> Positions => _positions.AsReadOnly();

    public Department(
        DepartmentId id,
        DepartmentName name,
        DepartmentSlug slug,
        DepartmentId? parentId,
        IEnumerable<LocationId> locations,
        EntityLifeTime lifeTime
    )
    {
        Id = id;
        Name = name;
        Slug = slug;
        ParentId = parentId;
        _locations = [.. locations];
        LifeTime = lifeTime;
        Path = DepartmentPath.Create(slug);
    }

    public Department(
        DepartmentId id,
        DepartmentName name,
        DepartmentSlug slug,
        DepartmentId? parentId,
        IEnumerable<LocationId> locations,
        IEnumerable<PositionId> positions,
        EntityLifeTime lifeTime,
        DepartmentPath path
    )
    {
        Id = id;
        Name = name;
        Slug = slug;
        ParentId = parentId;
        _locations = [.. locations];
        _positions = [.. positions];
        LifeTime = lifeTime;
        Path = path;
    }

    public static Department Create(
        DepartmentId id,
        DepartmentName name,
        DepartmentSlug slug,
        DepartmentId? parentId,
        IEnumerable<LocationId> locations,
        DateTime createdAt,
        Func<DepartmentName, bool> isNameUnique
    )
    {
        if (!isNameUnique(name))
        {
            throw new InvalidOperationException("Подразделение с таким названием уже существует.");
        }

        var lifeTime = EntityLifeTime.Create(createdAt, createdAt, isActive: true);
        return new Department(id, name, slug, parentId, locations, lifeTime);
    }

    public void Update(
        DateTime currentUtcTime,
        Func<DepartmentName, bool> isNameUnique,
        DepartmentName? newName = null,
        DepartmentSlug? newSlug = null
    )
    {
        if (!LifeTime.IsActive)
        {
            throw new InvalidOperationException("Редактирование архивированных подразделений запрещено.");
        }

        bool isChanged = false;

        if (newName != null && Name != newName)
        {
            if (!isNameUnique(newName))
            {
                throw new InvalidOperationException("Подразделение с таким названием уже существует.");
            }
            Name = newName;
            isChanged = true;
        }

        if (newSlug != null && Slug != newSlug)
        {
            Slug = newSlug;
            isChanged = true;

            if (ParentId == null)
            {
                Path = DepartmentPath.Create(newSlug);
            }
        }

        if (isChanged)
        {
            LifeTime = LifeTime with { UpdatedAt = currentUtcTime };
        }
    }

    public void MoveToParent(DepartmentId? newParentId, DepartmentPath? parentPath, DateTime currentUtcTime)
    {
        if (!LifeTime.IsActive)
        {
            throw new InvalidOperationException("Редактирование архивированных подразделений запрещено.");
        }

        if (newParentId == null)
        {
            ParentId = null;
            Path = DepartmentPath.Create(Slug);
            LifeTime = LifeTime with { UpdatedAt = currentUtcTime };
            return;
        }

        if (newParentId == Id)
        {
            throw new InvalidOperationException("Подразделение не может быть присоединено к самому себе.");
        }

        if (ParentId == newParentId)
        {
            throw new InvalidOperationException("Подразделение уже присоединено к этому родителю.");
        }

        if (parentPath == null)
        {
            throw new ArgumentNullException(nameof(parentPath), "Для изменения родителя необходимо передать его Path.");
        }

        ParentId = newParentId;

        Path = DepartmentPath.CreateForChild(parentPath, Slug);
        LifeTime = LifeTime with { UpdatedAt = currentUtcTime };
    }

    public void AddLocation(LocationId locationId, DateTime currentUtcTime)
    {
        if (!LifeTime.IsActive)
        {
            throw new InvalidOperationException("Редактирование архивированных подразделений запрещено.");
        }

        if (_locations.Contains(locationId))
        {
            throw new InvalidOperationException("Эта локация уже добавлена в подразделение.");
        }

        _locations.Add(locationId);
        LifeTime = LifeTime with { UpdatedAt = currentUtcTime };
    }

    public void AddPosition(PositionId positionId, DateTime currentUtcTime)
    {
        if (!LifeTime.IsActive)
        {
            throw new InvalidOperationException("Редактирование архивированных подразделений запрещено.");
        }

        if (_positions.Contains(positionId))
        {
            throw new InvalidOperationException("Эта должность уже закреплена за подразделением.");
        }

        _positions.Add(positionId);
        LifeTime = LifeTime with { UpdatedAt = currentUtcTime };
    }

    public void Archive(DateTime currentUtcTime)
    {
        if (!LifeTime.IsActive) return;

        LifeTime = LifeTime with { UpdatedAt = currentUtcTime, IsActive = false };
    }
}