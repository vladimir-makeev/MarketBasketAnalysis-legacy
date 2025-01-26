using System.Collections.Generic;
using MarketBasketAnalysis.DomainModel.AssociationRules;

namespace MarketBasketAnalysis.Services
{
    /// <summary>
    /// Описание функционала средства чтения транзакций
    /// </summary>
    public interface ITransactionReader
    {
        /// <summary>
        /// Чтение транзакций из указанного файла
        /// </summary>
        /// <param name="path">Путь к файлу с транзакциями</param>
        /// <returns>Прочитанные транзакции</returns>
        IEnumerable<Transaction> ReadTransactions(string path);
    }
}