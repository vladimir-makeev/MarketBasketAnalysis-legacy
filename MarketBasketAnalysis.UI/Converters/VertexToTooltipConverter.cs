using System;
using System.Windows.Data;
using System.Globalization;
using MarketBasketAnalysis.DomainModel.AssociationRules.Graph;

namespace MarketBasketAnalysis.UI.Converters
{
    /// <summary>
    /// Конвертер вершины графа ассоциативных правил в текстовую подсказку
    /// </summary>
    public class VertexToTooltipConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var vertex = (Vertex)value;
            return vertex.Item + '\n' +
                $"Количество: {vertex.Count}\n" +
                $"Поддержка: {Math.Round(vertex.Support, 6)}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}