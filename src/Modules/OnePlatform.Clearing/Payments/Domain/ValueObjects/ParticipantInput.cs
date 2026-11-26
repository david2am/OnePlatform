using OnePlatform.Clearing.Accounts.Domain.ValueObjects;
using OnePlatform.Clearing.Institutions.Domain.ValueObjects;
using OnePlatform.Clearing.Payments.Domain.Enums;

namespace OnePlatform.Clearing.Payments.Domain.ValueObjects;

public sealed record ParticipantInput(
    InstitutionId InstitutionId,
    AccountId AccountId,
    ParticipantRole Role
);
