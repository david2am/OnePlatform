using OnePlatform.Clearing.Domain.Enums;

namespace OnePlatform.Api.Payments;

public sealed record ParticipantRequest(
    Guid InstitutionId,
    Guid AccountId,
    ParticipantRole Role
);