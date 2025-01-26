using CodeContracts;

namespace MarketBasketAnalysis.Presentation.Views.EventArguments
{
    /// <summary>
    /// Параметры запроса поиска ассоциативных правил
    /// </summary>
    public class MiningArgs
    {
        /// <summary>
        /// Путь к файлу с транзакциями
        /// </summary>
        public string TransactionFilePath { get; }

        /// <summary>
        /// Порог поддержки
        /// </summary>
        public double Support { get; }

        /// <summary>
        /// Порог достоверности
        /// </summary>
        public double Confidence { get; }

        /// <summary>
        /// Следует ли применять правила удаления элементов транзакций
        /// </summary>
        public bool ApplyItemDeletionRules { get; }

        /// <summary>
        /// Следует ли использовать правила замены СКЮ группами товаров
        /// </summary>
        public bool ReplaceArticlesWithGroups { get; }

        /// <summary>
        /// Задание параметров поиска ассоциативных правил
        /// </summary>
        /// <param name="transactionFilePath">Путь к файлу с транзакциями</param>
        /// <param name="support">Порог поддержки</param>
        /// <param name="confidence">Порог достоверности</param>
        /// <param name="applyItemDeletionRules">Следует ли применять правила удаления элементов транзакций</param>
        /// <param name="applyItemReplacementRules">Следует ли использовать правила замены СКЮ группами товаров</param>
        public MiningArgs
        (
            string transactionFilePath,
            double support,
            double confidence,
            bool applyItemDeletionRules,
            bool applyItemReplacementRules
        )
        {
            Requires.NotNull(transactionFilePath, nameof(transactionFilePath));

            this.TransactionFilePath = transactionFilePath;
            this.Support = support;
            this.Confidence = confidence;
            this.ApplyItemDeletionRules = applyItemDeletionRules;
            this.ReplaceArticlesWithGroups = applyItemReplacementRules;
        }
    }
}