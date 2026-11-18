using OnePlatform.Clearing.Domain.Enums;

public sealed record ParticipantRequest(
    Guid InstitutionId,
    Guid AccountId,
    ParticipantRole Role
);