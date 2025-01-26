using System.Linq;
using QuickGraph;
using System.Windows.Controls;
using System.Collections.Generic;
using MarketBasketAnalysis.DomainModel.AssociationRules.Graph;
using MarketBasketAnalysis.Presentation.Views;

namespace MarketBasketAnalysis.UI
{
    /// <summary>
    /// Элемент управления показа максимальных подграфов
    /// </summary>
    public partial class ShowMaximalSubgraphsControl : ContentControl, IShowMaximalSubgraphsView
    {
        /// <summary>
        /// Инициализация элемента управления
        /// </summary>
        public ShowMaximalSubgraphsControl() =>
            InitializeComponent();

        /// <summary>
        /// Обработчик выбора элемента из списка подграфов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MaximalSubgraphSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var subgraph = (IBidirectionalGraph<Vertex, Edge>)((ListView)sender).SelectedItem;
            if (subgraph != null)
                AssociationRuleGraphLayout.Graph = subgraph;
        }

        #region IShowMaximalSubgraphs

        /// <summary>
        /// Показ максимальных подграфов
        /// </summary>
        /// <param name="subgraphs">Максимальные подграфы</param>
        public void ShowMaximalSubgraphs(IReadOnlyList<BidirectionalGraph<Vertex, Edge>> subgraphs) =>
            Dispatcher.Invoke(() =>
            {
                MaximalSubgraphsTabControl.ItemsSource = subgraphs
                    .GroupBy(subgraph => subgraph.VertexCount, subgraph => subgraph)
                    .OrderBy(subgraph => subgraph.Key);
                MaximalSubgraphsTabControl.SelectedIndex = subgraphs.Count > 0 ? 0 : -1;
                MaximalSubgraphsCountLabel.Content = $"Количество максимальных подграфов: {subgraphs.Count}";
            });

        #endregion
    }
}