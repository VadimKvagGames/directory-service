public class Position
{
    public PositionId Id { get; }
    public PositionName Name { get; }
    public PositionDescription Description { get; }
    public EntityLifeTime LifeTime { get; }

    public Position(
        PositionId id,
        PositionName name,
        PositionDescription description,
        EntityLifeTime lifeTime)
    {
        Id = id;
        Name = name;
        Description = description;
        LifeTime = lifeTime;
    }
}