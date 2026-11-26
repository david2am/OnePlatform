
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OnePlatform.Clearing.Shared.Domain.Entities;

namespace OnePlatform.Clearing.Shared.Infrastructure.Persistence.SqlServer.Converters;

public sealed class GuidIdConverter<TId> : ValueConverter<TId, Guid>
    where TId : IGuidId<TId>
{
    public GuidIdConverter()
        : base(
            id => id.Value,
            BuildFromGuid())
    {
    }

    private static Expression<Func<Guid, TId>> BuildFromGuid()
    {
        var method = typeof(TId).GetMethod(
            nameof(IGuidId<TId>.FromGuid),
            BindingFlags.Public | BindingFlags.Static,
            [typeof(Guid)]
        ) ?? throw new InvalidOperationException(
            $"{typeof(TId).Name} must implement FromGuid(Guid).");

        var parameter = Expression.Parameter(typeof(Guid), "value");

        return Expression.Lambda<Func<Guid, TId>>(
            Expression.Call(method, parameter),
            parameter);
    }
}