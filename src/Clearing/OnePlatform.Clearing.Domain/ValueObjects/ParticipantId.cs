using OnePlatform.Clearing.Domain.Shared;

namespace OnePlatform.Clearing.Domain.ValueObjects;

public sealed record ParticipantId : IGuidId<ParticipantId>
{
  public Guid Value { get; }

  private ParticipantId(Guid value) => Value = value;

  public static ParticipantId FromGuid(Guid value) => new(value);

  public static Result<ParticipantId> Create(Guid value) =>
    value == Guid.Empty
      ? Result.Failure<ParticipantId>(ParticipantIdErrors.Required)
      : Result.Success(new ParticipantId(value));

  public static ParticipantId New() => new(Guid.CreateVersion7());
}

public static class ParticipantIdErrors
{
    public static readonly Error Required = new(
        "ParticipantId.Required",
        "ParticipantId is required.");
}