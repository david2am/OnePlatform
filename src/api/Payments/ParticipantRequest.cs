using OnePlatform.Clearing.Payments.Domain.Enums;

namespace OnePlatform.Api.Payments;

public sealed record ParticipantRequest(
    Guid InstitutionId,
    Guid AccountId,
    ParticipantRole Role
);