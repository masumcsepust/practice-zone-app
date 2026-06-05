namespace backend.Application.DTOs;

public record LeaderboardEntryDto(
    int    Rank,
    string DisplayName,
    string AvatarUrl,
    int    Xp
);

public record LeaderboardMyPositionDto(
    int    Rank,
    string DisplayName,
    string AvatarUrl,
    int    Xp,
    int    XpToNextRank
);

public record LeaderboardResponseDto(
    IReadOnlyList<LeaderboardEntryDto> Entries,
    LeaderboardMyPositionDto?          MyPosition
);
