using System;
using System.Collections.Generic;
using MarketBasketAnalysis.Presentation.Common;
using MarketBasketAnalysis.DomainModel.AssociationRules.Mining;
using MarketBasketAnalysis.Presentation.Views.EventArguments;

namespace MarketBasketAnalysis.Presentation.Views
{
    /// <summary>
    /// Описание представления настроек приложения
    /// </summary>
    public interface ISettingsView : IView
    {
        /// <summary>
        /// Событие запроса сохранения настроек
        /// </summary>
        event EventHandler<SaveSettingsArgs> SettingsSaveRequested;

        /// <summary>
        /// Показ правил удаления элементов транзакций
        /// </summary>
        /// <param name="rules">Правила удаления элементов транзакций</param>
        void ShowItemDeletionRules(IReadOnlyList<ItemDeletionRule> rules);

        /// <summary>
        /// Показ правил замены СКЮ на группы товаров
        /// </summary>
        /// <param name="rules">Правила замены СКЮ группами товаров</param>
        void ShowItemReplacementRules(IReadOnlyList<ItemReplacementRule> rules);

        /// <summary>
        /// Показ групп товаров
        /// </summary>
        /// <param name="groups">Группы товаров</param>
        void ShowArticleGroups(IReadOnlyList<string> groups);
    }
}