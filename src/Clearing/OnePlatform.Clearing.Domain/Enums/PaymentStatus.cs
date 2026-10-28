namespace OnePlatform.Clearing.Domain.Enums;

public enum PaymentStatus
{
  Pending = 1,
  Validated = 2,
  Rejected = 3,
  Settled = 4,
  Final = 5,
  Returned = 6
}
public enum PaymentType
{
  CreditTransfer = 1,
  DirectDebit = 2
}
public enum PaymentEventType
{
  Received = 1,
  Validated = 2,
  Rejected = 3,
  Settled = 4,
  Final = 5,
  Delivered = 6,
  Returned = 7
}

public enum ValidationCheckType
{
  Schema = 1,
  Sanctions = 2,
  Duplicate = 3,
  Membership = 4
}

public enum ValidationOutcome
{
  Pass = 1,
  Fail = 2
}