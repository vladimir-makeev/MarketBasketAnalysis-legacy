using System.Linq;
using System.Collections;
using System.Collections.Generic;
using CodeContracts;

namespace MarketBasketAnalysis.DomainModel.AssociationRules
{
    /// <summary>
    /// Транзакция, представляющая список приобретенных покупателем товаров
    /// </summary>
    public class Transaction : IEnumerable<string>
    {
        /// <summary>
        /// Элементы транзакции
        /// </summary>
        public IReadOnlyList<string> Items { get; }

        /// <summary>
        /// Создание транзакции
        /// </summary>
        /// <param name="checkItems">Элементы транзакции - список приобретенных товаров</param>
        public Transaction(IReadOnlyList<string> checkItems)
        {
            Requires.NotNull(checkItems, nameof(checkItems));
            Items = checkItems.Distinct().ToList();
        }

        /// <summary>
        /// Получение перечислителя для перебора элементов транзакции
        /// </summary>
        /// <returns>Перечислиель элементов транзакции</returns>
        public IEnumerator<string> GetEnumerator() =>
            Items.GetEnumerator();

        /// <summary>
        /// Получение перечислителя для перебора элементов транзакции
        /// </summary>
        /// <returns>Перечислитель элементов транзакции</returns>
        IEnumerator IEnumerable.GetEnumerator() =>
            Items.GetEnumerator();
    }
}