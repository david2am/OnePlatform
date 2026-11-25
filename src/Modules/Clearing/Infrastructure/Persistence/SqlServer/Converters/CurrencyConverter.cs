using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OnePlatform.Clearing.Domain.ValueObjects;

namespace OnePlatform.Clearing.Infrastructure.Persistence.SqlServer.Converters;

public sealed class CurrencyConverter : ValueConverter<Currency, string>
{
    public CurrencyConverter()
        : base(
            currency => currency.Code,
            value => Currency.Create(value).Value)
    {
    }
}