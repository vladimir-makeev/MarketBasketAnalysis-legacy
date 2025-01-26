using System.ComponentModel.DataAnnotations;
using CodeContracts;

namespace MarketBasketAnalysis.DomainModel.AssociationRules.Mining
{
    /// <summary>
    /// Правило, описывающее объединение товаров в товарную группу
    /// в ассоциативных правилах
    /// </summary>
    public class ItemReplacementRule
    {
        /// <summary>
        /// Название группы товаров, до которой требуется провести замену
        /// </summary>
        [Key]
        public string ArticleGroup { get; set; }

        /// <summary>
        /// Область действия замены
        /// </summary>
        public ReplacementScope Scope { get; set; }

        /// <summary>
        /// Создание правила замены
        /// </summary>
        /// <param name="articleGroup">Название группы товаров</param>
        /// <param name="scope">Область действия замены</param>
        public ItemReplacementRule(string articleGroup, ReplacementScope scope)
        {
            Requires.NotNullOrEmpty(articleGroup, nameof(articleGroup));

            this.ArticleGroup = articleGroup;
            this.Scope = scope;
        }

        /// <summary>
        /// Конструктор для десериализации объекта
        /// </summary>
        private ItemReplacementRule()
        {

        }
    }
}