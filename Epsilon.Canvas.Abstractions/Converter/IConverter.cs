namespace Epsilon.Canvas.Abstractions.Converter;

public interface IConverter<TTo, TFrom>
{
    TTo ConvertFrom(TFrom from);

    TFrom ConvertTo(TTo to);
}