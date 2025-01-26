using System;
using System.ComponentModel.DataAnnotations;
using CodeContracts;

namespace MarketBasketAnalysis.DomainModel.AssociationRules.Mining
{
    /// <summary>
    /// Правило исключения элемента транзакций из поиска ассоциативных правил
    /// </summary>
    public class ItemDeletionRule
    {
        /// <summary>
        /// Наименование элемента для исключения
        /// </summary>
        [Key]
        public string Item { get; private set; }

        /// <summary>
        /// Требуется ли точное соответствие названия элемента для исключения
        /// наименованиям элементов транзакций при текстовом поиске
        /// </summary>
        public bool ExactMatch { get; private set; }

        /// <summary>
        /// Создание правила исключения элемента из поиска ассоциативных правил
        /// </summary>
        /// <param name="item">Наименование элемента для удаления</param>
        /// <param name="exactMatch">
        /// Требуется ли точное соответствие названия элемента для исключения
        /// наименованиям элементов транзакций
        /// </param>
        public ItemDeletionRule(string item, bool exactMatch)
        {
            Requires.NotNullOrEmpty(item, nameof(item));

            this.Item = item;
            this.ExactMatch = exactMatch;
        }

        /// <summary>
        /// Конструктор для десериализации объекта
        /// </summary>
        private ItemDeletionRule()
        {

        }

        /// <summary>
        /// Проверка, удовлетворяет ли элемент транзакций правилу удаления
        /// </summary>
        /// <param name="item">Элемент транзакций</param>
        /// <returns>Истина, если правило применимо к элементу, иначе - ложь</returns>
        public bool ShouldDelete(string item)
        {
            Requires.NotNullOrEmpty(item, nameof(item));

            if (this.ExactMatch)
                return this.Item == item;
            return item.IndexOf(this.Item, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}