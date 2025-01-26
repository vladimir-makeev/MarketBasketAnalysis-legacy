using System.Windows;
using QuickGraph;
using GraphSharp.Controls;
using MarketBasketAnalysis.DomainModel.AssociationRules.Graph;

namespace MarketBasketAnalysis.UI
{
    /// <summary>
    /// Элемент управления отображения графа ассоциативных правил
    /// </summary>
    public class AssociationRuleGraphLayout : GraphLayout<Vertex, Edge, IBidirectionalGraph<Vertex, Edge>>
    {
        /// <summary>
        /// Создание элемента управления: загрузка его стиля,
        /// стилей элементов управления вершин, ребер
        /// </summary>
        public AssociationRuleGraphLayout()
        {
            Resources = new ResourceDictionary
            {
                Source = new System.Uri("pack://application:,,,/AssociationRuleGraphLayoutStyles.xaml")
            };
        }
    }
}