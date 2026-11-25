using OnePlatform.Clearing.Domain.Enums;

namespace OnePlatform.Clearing.Domain.ValueObjects;

public sealed record ParticipantInput(
    InstitutionId InstitutionId,
    AccountId AccountId,
    ParticipantRole Role
);
