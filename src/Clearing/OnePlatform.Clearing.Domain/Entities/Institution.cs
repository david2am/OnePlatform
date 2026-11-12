using OnePlatform.Clearing.Domain.Shared;
using OnePlatform.Clearing.Domain.ValueObjects;

namespace OnePlatform.Clearing.Domain.Entities;

public sealed class Institution : AggregateRoot
{
  public string Name { get; private set; } = string.Empty;

  public RoutingNumber RoutingNumber { get; private set; }
  public bool IsMember { get; private set; }
  public DateTimeOffset? MembershipSince { get; private set; }


  private Institution() : base(Guid.Empty)
  {
    RoutingNumber = null!;
  }
  
  private Institution(
    InstitutionId id,
    string name,
    RoutingNumber routingNumber,
    bool isMember,
    DateTimeOffset? membershipSince) : base(id.Value)
  {
    Name = name;
    RoutingNumber = routingNumber;
    IsMember = isMember;
    MembershipSince = membershipSince;
  }

  public static Result<Institution> Create(
    string name,
    RoutingNumber routingNumber,
    bool isMember,
    DateTimeOffset? membershipSince)
  {
    // validations
    if (string.IsNullOrWhiteSpace(name))
      return Result.Failure<Institution>(InstitutionErrors.NameRequired);

    if (isMember && membershipSince is null)
      return Result.Failure<Institution>(InstitutionErrors.MembershipDateRequired);
    
    if (!isMember && membershipSince is not null)
      return Result.Failure<Institution>(InstitutionErrors.MembershipDateNotAllowed);

    return Result.Success(
      new Institution(
        InstitutionId.New(),
        name,
        routingNumber,
        isMember,
        membershipSince
      )
    );
  }

  public Result EnrollAsMember(DateTimeOffset since)
  {
    if (IsMember)
      return Result.Failure(InstitutionErrors.AlreadyMember);

    IsMember = true;
    MembershipSince = since;

    return Result.Success();
  }
}

public static class InstitutionErrors
{
  public static readonly Error NameRequired = new(
    "Institution.NameRequired", "Name is required.");

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