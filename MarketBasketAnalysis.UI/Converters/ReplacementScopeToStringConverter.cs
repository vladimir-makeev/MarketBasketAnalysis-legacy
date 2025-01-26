using MarketBasketAnalysis.DomainModel.AssociationRules.Mining;
using System;
using System.Globalization;
using System.Windows.Data;

namespace MarketBasketAnalysis.UI.Converters
{
    /// <summary>
    /// Конвертер области действия замены СКЮ группой товаров в строку
    /// </summary>
    public class ReplacementScopeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((ReplacementScope)value)
            {
                case ReplacementScope.LeftHandSide:
                    return "Левая часть правила";
                case ReplacementScope.RightHandSide:
                    return "Правая часть правила";
                default:
                    return "Обе части правила";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((string)value)
            {
                case "Левая часть правила":
                    return ReplacementScope.LeftHandSide;
                case "Правая часть правила":
                    return ReplacementScope.RightHandSide;
                default:
                    return ReplacementScope.BothHandSides;
            }
        }
    }
}
