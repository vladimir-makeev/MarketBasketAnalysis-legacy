using System;
using System.Globalization;
using System.Windows.Data;

namespace MarketBasketAnalysis.UI.Converters
{
    /// <summary>
    /// Конвертер количества вершин графа в название закладки
    /// </summary>
    public class VertexCountToTabHeaderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var vertexCount = (int)value;
            if (vertexCount >= 11 && vertexCount <= 14 ||
                vertexCount % 10 == 0 || vertexCount % 10 > 4)
                return $"{vertexCount} вершин";
            return $"{vertexCount} вершины";  
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}