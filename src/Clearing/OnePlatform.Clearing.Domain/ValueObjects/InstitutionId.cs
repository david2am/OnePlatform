using OnePlatform.Clearing.Domain.Shared;

namespace OnePlatform.Clearing.Domain.ValueObjects;

public sealed record InstitutionId
{
  public Guid Value { get; }

  private InstitutionId() {}
  private InstitutionId(Guid value)
  {
    Value = value;
  }

  public static Result<InstitutionId> Create(Guid value)
  {
    if (value == Guid.Empty)
      return Result.Failure<InstitutionId>(InstitutionIdErrors.Required);

    return Result.Success(new InstitutionId(value));
  }

  public static InstitutionId New() => new(Guid.CreateVersion7());
}

public static class InstitutionIdErrors
{
    public static readonly Error Required = new(
        "InstitutionId.Required",
        "InstitutionId is required.");
}