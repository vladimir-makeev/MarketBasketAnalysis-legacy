using System;
using MarketBasketAnalysis.Presentation.Common;
using MarketBasketAnalysis.Presentation.Views.EventArguments;

namespace MarketBasketAnalysis.Presentation.Views
{
    /// <summary>
    /// Описание представления поиска ассоциативных правил
    /// </summary>
    public interface IMineAssociationRulesView : IView
    {
        /// <summary>
        /// Событие запроса поиска ассоциативных правил
        /// </summary>
        event EventHandler<MiningArgs> AssociationRuleMiningRequested;

        /// <summary>
        /// Событие запроса загрузки ассоциативных правил
        /// </summary>
        event EventHandler<string> AssociationRuleLoadingRequested;

        /// <summary>
        /// Показ прогресса поиска правил
        /// </summary>
        void ShowMiningProgress();

        /// <summary>
        /// Скрытие прогресса поиска правил
        /// </summary>
        void HideMiningProgress();

        /// <summary>
        /// Обновление прогресса поиска правил
        /// </summary>
        /// <param name="progress">Новое значение прогресса</param>
        void UpdateMiningProgress(double progress);
    }
}