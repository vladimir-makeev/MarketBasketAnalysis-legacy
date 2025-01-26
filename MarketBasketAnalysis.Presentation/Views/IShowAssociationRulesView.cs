using System;
using System.Collections.Generic;
using QuickGraph;
using MarketBasketAnalysis.DomainModel.AssociationRules;
using MarketBasketAnalysis.DomainModel.AssociationRules.Graph;
using MarketBasketAnalysis.Presentation.Common;
using MarketBasketAnalysis.Presentation.Views.EventArguments;

namespace MarketBasketAnalysis.Presentation.Views
{
    /// <summary>
    /// Описание представления отображения ассоциативных правил
    /// </summary>
    public interface IShowAssociationRulesView : IView
    {
        /// <summary>
        /// Событие запроса фильтрации ассоциативных правил
        /// </summary>
        event EventHandler<FilteringArgs> FilterRequested;

        /// <summary>
        /// Событие запроса сброса примененных фильтров
        /// </summary>
        event EventHandler FilterResetRequested;
        
        /// <summary>
        /// Событие запроса поиска максимальных клик
        /// </summary>
        event EventHandler<CliquesFindingArgs> MaximalSubgraphsFindingRequested;
        
        /// <summary>
        /// Событие запроса сохранения ассоциативных правил
        /// </summary>
        event EventHandler<string> SaveRequested;

        /// <summary>
        /// Показ ассоциативных правил
        /// </summary>
        /// <param name="rules">Ассоциативные правила</param>
        /// <param name="graph">Граф ассоциативных правил</param>
        void ShowAssociationRules(IReadOnlyList<AssociationRule> rules, IBidirectionalGraph<Vertex, Edge> graph);
    }
}