using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OnePlatform.Clearing.Shared.Domain.ValueObjects;

namespace OnePlatform.Clearing.Shared.Infrastructure.Persistence.SqlServer.Converters;

public sealed class CurrencyConverter : ValueConverter<Currency, string>
{
    public CurrencyConverter()
        : base(
            currency => currency.Code,
            value => Currency.Create(value).Value)
    {
    }
}