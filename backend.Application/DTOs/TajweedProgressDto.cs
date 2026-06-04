namespace backend.Application.DTOs;

public record SaveTajweedProgressRequest(
    string Mode,
    double Score,
    double AccuracyScore,
    bool   IsCorrect
);

public record TajweedModeStat(double BestScore, int Attempts, bool LastCorrect);

public record TajweedLetterProgress(
    Guid             LetterId,
    string           Letter,
    string           NameBangla,
    TajweedModeStat? Harakat,
    TajweedModeStat? Tanween,
    TajweedModeStat? SukoonShaddah,
    TajweedModeStat? WordBuilding
);
