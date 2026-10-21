using System;
using DirectoryService.Domain.PositionContext;

namespace DirectoryService.Domain.DepartmentContext;

public sealed record DepartmentPositionRank(PositionId PositionId, int Rank);