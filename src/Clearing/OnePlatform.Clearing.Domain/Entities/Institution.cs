using OnePlatform.Clearing.Domain.Shared;

namespace OnePlatform.Clearing.Domain.Entities;

public sealed class Institution : AggregateRoot
{
  public string Name { get; private set; } = string.Empty;

  // ABA number or RTN, routing transit number is a 9-digit ID that identifies a financial institution in the US
  public string RoutingNumber { get; private set; } = string.Empty;
  public bool IsMember { get; private set; }
  public DateTimeOffset? MembershipSince { get; private set; }

  // Used only by EF Core when it loads data from the database.
  // Entity has no parameterless constructor, so we pass Guid.Empty;
  // EF then overwrites Id with the real value.
  private Institution() : base(Guid.Empty)
  {
    
  }
  
  private Institution(
    Guid id,
    string name,
    string routingNumber,
    bool isMember,
    DateTimeOffset? membershipSince) : base(id)
  {
    Name = name;
    RoutingNumber = routingNumber;
    IsMember = isMember;
    MembershipSince = membershipSince;
  }

  public static Result<Institution> Create(
    string name,
    string routingNumber,
    bool isMember,
    DateTimeOffset? membershipSince)
  {
    // validations
    if (string.IsNullOrWhiteSpace(name))
      return Result.Failure<Institution>(InstitutionErrors.NameRequired);

    if (!IsValidRoutingNumber(routingNumber))
      return Result.Failure<Institution>(InstitutionErrors.InvalidRoutingNumber);

    if (isMember && membershipSince is null)
      return Result.Failure<Institution>(InstitutionErrors.MembershipDateRequired);
    
    if (isMember && membershipSince is not null)
      return Result.Failure<Institution>(InstitutionErrors.MembershipDateNotAllowed);

    return Result.Success(
      new Institution(
        Guid.NewGuid(),
        name,
        routingNumber,
        isMember,
        membershipSince
      )
    );
  }

  public void EnrollAsMember(DateTimeOffset since)
  {
    if (IsMember)
      throw new InvalidOperationException("Institution is already a member.");

    IsMember = true;
    MembershipSince = since;
  }

  // ABA checksum: 3*(d1+d4+d7) + 7*(d2+d5+d8) + 1*(d3+d6+d9) must be divisible by 10
  private static bool IsValidRoutingNumber(string? value)
  {
    if (value is null || value.Length != 9 || !value.All(char.IsAsciiDigit))
      return false;

    var d = value.Select(c => c - '0').ToArray();

    var sum = 3 * (d[0] + d[3] + d[6])
            + 7 * (d[1] + d[4] + d[7])
            + 1 * (d[2] + d[5] + d[8]);

    return sum % 10 == 0;
  }
}

public static class InstitutionErrors
{
  public static readonly Error NameRequired = new(
    "Institution.NameRequired", "Name is required.");

  public static readonly Error InvalidRoutingNumber = new(
    "Institution.InvalidRoutingNumber",
    "Routing number must be 9 digits and pass the ABA checksum.");

  public static readonly Error MembershipDateRequired = new(
    "Institution.MembershipDateRequired",
    "A member institution must have a membership date.");

  public static readonly Error MembershipDateNotAllowed = new(
    "Institution.MembershipDateNotAllowed",
    "A non-member institution cannot have a membership date.");

  public static readonly Error AlreadyMember = new(
    "Institution.AlreadyMember",
    "Institution is already a member.");
}