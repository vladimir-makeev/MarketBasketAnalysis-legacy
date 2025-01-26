using System;
using System.Collections.Generic;
using CodeContracts;
using MarketBasketAnalysis.DomainModel.AssociationRules.Mining;

namespace MarketBasketAnalysis.Presentation.Views.EventArguments
{
    /// <summary>
    /// Параметры запроса сохранения настроек приложения
    /// </summary>
    public class SaveSettingsArgs
    {
        /// <summary>
        /// Правила удаления элементов транзакций
        /// </summary>
        public IReadOnlyList<Tuple<string, bool>> ItemDeletionRules { get; }

        /// <summary>
        /// Правила замены СКЮ группами товаров
        /// </summary>
        public IReadOnlyList<Tuple<string, ReplacementScope>> ItemReplacementRules { get; }

        /// <summary>
        /// Задание параметров настроек приложения
        /// </summary>
        /// <param name="itemDeletionRules">Правила удаления элементов транзакций</param>
        /// <param name="itemReplacementRules">Правила замены СКЮ группами товаров</param>
        public SaveSettingsArgs
            (
                IReadOnlyList<Tuple<string, bool>> itemDeletionRules,
                IReadOnlyList<Tuple<string, ReplacementScope>> itemReplacementRules
            )
        {
            Requires.NotNull(itemDeletionRules, nameof(itemDeletionRules));
            Requires.NotNull(itemReplacementRules, nameof(itemReplacementRules));

            this.ItemDeletionRules = itemDeletionRules;
            this.ItemReplacementRules = itemReplacementRules;
        }
    }
}