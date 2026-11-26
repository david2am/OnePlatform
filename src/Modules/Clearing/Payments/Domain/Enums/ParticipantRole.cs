namespace OnePlatform.Clearing.Payments.Domain.Enums;

public enum ParticipantRole
{
  DebtorAgent = 1,
  CreditorAgent = 2,
  IntermediaryAgent = 3
}

public enum ParticipantStatus
{
  Pending = 0,
  Accepted = 1,
  Processing = 2,
  Completed = 3,
  Rejected = 4,
  Failed = 5,
  Cancelled = 6
}