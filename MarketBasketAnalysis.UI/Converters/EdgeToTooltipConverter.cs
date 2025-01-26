using System;
using System.Globalization;
using System.Windows.Data;
using MarketBasketAnalysis.DomainModel.AssociationRules.Graph;

namespace MarketBasketAnalysis.UI.Converters
{
    /// <summary>
    /// Конвертер ребра графа в текстовую подсказку
    /// </summary>
    public class EdgeToTooltipConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var rule = ((Edge)value).AssociationRule;
            return $"{rule.LeftHandSide} - {rule.RightHandSide}\n" +
                $"Количество: {rule.Count}\n" +
                $"Поддержка: {Math.Round(rule.Support, 6)}\n" +
                $"Достоверность: {Math.Round(rule.Confidence, 5)}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}