using OnePlatform.Clearing.Shared.Domain.Entities;
using OnePlatform.Clearing.Shared.Domain.Exceptions;

namespace OnePlatform.Clearing.Institutions.Domain.ValueObjects;

public sealed record InstitutionId : IGuidId<InstitutionId>
{
  public Guid Value { get; }

  private InstitutionId(Guid value) => Value = value;

  public static InstitutionId FromGuid(Guid value) => new(value);

  public static Result<InstitutionId> Create(Guid value) =>
    value == Guid.Empty
      ? Result.Failure<InstitutionId>(InstitutionIdErrors.Required)
      : Result.Success(new InstitutionId(value));

  public static InstitutionId New() => new(Guid.CreateVersion7());
}

public static class InstitutionIdErrors
{
    public static readonly Error Required = new(
        "InstitutionId.Required",
        "InstitutionId is required.");
}