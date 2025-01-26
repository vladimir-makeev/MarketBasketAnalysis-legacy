using System.Collections.Generic;
using QuickGraph;
using CodeContracts;
using MarketBasketAnalysis.Presentation.Common;
using MarketBasketAnalysis.DomainModel.AssociationRules.Graph;
using MarketBasketAnalysis.Presentation.Views;

namespace MarketBasketAnalysis.Presentation.Presenters
{
    /// <summary>
    /// Представитель, взаимодействующий с представлением показа
    /// максимальных подграфов ассоциативных правил
    /// </summary>
    public class ShowMaximalSubgraphsPresenter : BasePresenter<IShowMaximalSubgraphsView, IReadOnlyList<BidirectionalGraph<Vertex, Edge>>>
    {
        /// <summary>
        /// Создание представителя
        /// </summary>
        /// <param name="controller">Контроллер приложения</param>
        /// <param name="navigator">Средство показа, закрытия представлений</param>
        /// <param name="view">Представление показа максимальных подграфов</param>
        public ShowMaximalSubgraphsPresenter
        (
            ApplicationController controller,
            INavigator navigator,
            IShowMaximalSubgraphsView view
        ) : base(controller, navigator, view)
        {

        }

        /// <summary>
        /// Показ представления, максимальных подграфов
        /// </summary>
        /// <param name="cliques">Максимальные подграфы графа ассоциативных правил</param>
        public override void Run(IReadOnlyList<BidirectionalGraph<Vertex, Edge>> cliques)
        {
            Requires.NotNull(cliques, nameof(cliques));

            view.ShowMaximalSubgraphs(cliques);
            base.Run(cliques);
        }
    }
}