using OnePlatform.Clearing.Institutions.Domain.ValueObjects;
using OnePlatform.Clearing.Shared.Domain.Entities;
using OnePlatform.Clearing.Shared.Domain.Exceptions;
using OnePlatform.Clearing.Shared.Domain.ValueObjects;

namespace OnePlatform.Clearing.Institutions.Domain.Entities;

public sealed class Institution : AggregateRoot<InstitutionId>
{
  public Name Name { get; private set; }

  public RoutingNumber RoutingNumber { get; private set; }
  public bool IsMember { get; private set; }
  public DateTimeOffset? MembershipSince { get; private set; }


  private Institution() : base(null!)
  {
    Name = null!;
    RoutingNumber = null!;
  }
  
  private Institution(
    InstitutionId id,
    Name name,
    RoutingNumber routingNumber,
    bool isMember,
    DateTimeOffset? membershipSince) : base(id)
  {
    Name = name;
    RoutingNumber = routingNumber;
    IsMember = isMember;
    MembershipSince = membershipSince;
  }

  public static Result<Institution> Create(
    Name name,
    RoutingNumber routingNumber,
    bool isMember,
    DateTimeOffset? membershipSince)
  {
    if (name is null)
      return Result.Failure<Institution>(InstitutionErrors.NameRequired);

    if (routingNumber is null)
      return Result.Failure<Institution>(InstitutionErrors.RoutingNumberRequired);

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
    MembershipSince = since; // TODO: validate date constraints

    return Result.Success();
  }
}

public static class InstitutionErrors
{
  public static readonly Error NameRequired = new(
    "Institution.NameRequired", "Name is required.");
  
  public static readonly Error RoutingNumberRequired = new(
    "Institution.RoutingNumberRequired", "Routing Number is required.");

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