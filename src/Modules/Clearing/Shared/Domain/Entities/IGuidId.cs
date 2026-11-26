namespace OnePlatform.Clearing.Shared.Domain.Entities;

public interface IGuidId<TSelf> where TSelf : IGuidId<TSelf>
{
  Guid Value { get; }
  static abstract TSelf FromGuid(Guid value);
}