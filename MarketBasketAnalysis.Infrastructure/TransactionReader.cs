using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using CodeContracts;
using MarketBasketAnalysis.DomainModel.AssociationRules;
using MarketBasketAnalysis.Services;

namespace MarketBasketAnalysis.Infrastructure
{
    /// <summary>
    /// Средство чтения файла транзакций
    /// </summary>
    public class TransactionReader : ITransactionReader
    {
        /// <summary>
        /// Чтение файла транзакций
        /// </summary>
        /// <param name="path">Путь к CSV-файлу с транзакциями</param>
        /// <returns>Прочитанные транзакции</returns>
        public IEnumerable<Transaction> ReadTransactions(string path)
        {
            Requires.NotNull(path, nameof(path));
            Requires.True(File.Exists(path), $"Не найден файл {path}");

            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (var bufferedStream = new BufferedStream(fileStream, 1024 * 1024 * 200))
            using (var streamReader = new StreamReader(bufferedStream, Encoding.UTF8))
            {
                string line = null;
                while ((line = streamReader.ReadLine()) != null)
                    yield return new Transaction
                    (
                       checkItems: line.Trim('\"')
                            .Split('|')
                            .Distinct()
                            .Select(item => item.Trim('\t', ' '))
                            .ToArray()
                    );
            }
        }
    }
}