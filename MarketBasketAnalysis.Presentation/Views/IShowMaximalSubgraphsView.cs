using MarketBasketAnalysis.Presentation.Common;
using System.Collections.Generic;
using QuickGraph;
using MarketBasketAnalysis.DomainModel.AssociationRules.Graph;

namespace MarketBasketAnalysis.Presentation.Views
{
    /// <summary>
    /// Описания представления отображения максимальных подграфов
    /// </summary>
    public interface IShowMaximalSubgraphsView : IView
    {
        /// <summary>
        /// Показ максимальных подграфов
        /// </summary>
        /// <param name="subgraphs">Максимальные подграфы</param>
        void ShowMaximalSubgraphs(IReadOnlyList<BidirectionalGraph<Vertex, Edge>> subgraphs);
    }
}