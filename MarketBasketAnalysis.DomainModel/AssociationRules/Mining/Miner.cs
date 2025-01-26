using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;
using CodeContracts;

namespace MarketBasketAnalysis.DomainModel.AssociationRules.Mining
{
    /// <summary>
    /// Релизация алгоритма поиска ассоциативных правил
    /// </summary>
    public class Miner
    {
        /// <summary>
        /// Источник токена отмены операции поиска ассоциативных правил
        /// </summary>
        private CancellationTokenSource _cancelTokenSource;

        /// <summary>
        /// Событие, уведомляющее о ходе процесса поиска правил
        /// </summary>
        public event EventHandler<double> MiningProgressChanged;

        /// <summary>
        /// Поиск ассоциативных правил в транзакциях
        /// </summary>
        /// <param name="transactions">Последовательность транзакций</param>
        /// <param name="parameters">Параметры поиска правил</param>
        /// <param name="converter">Конвертер одноэлементных, двуэлементных наборов</param>
        /// <param name="itemDeletionRules">Правила исключения элементов транзакций из поиска ассоциативных правил</param>
        /// <returns>Найденные ассоциативные правила</returns>
        /// <exception cref="OperationCanceledException">
        /// Выброс исключения OperationCanceledException, если операция поиска правил была отменена клиентом
        /// </exception>
        public List<AssociationRule> Mine
            (IEnumerable<Transaction> transactions,
            MiningParameters parameters,
            ItemsetsConverter converter,
            IReadOnlyList<ItemDeletionRule> itemDeletionRules = null)
        {
            Requires.NotNull(transactions, nameof(transactions));
            Requires.NotNull(parameters, nameof(parameters));

                _cancelTokenSource = new CancellationTokenSource();

                var itemFrequencies = new ConcurrentDictionary<string, int>();

                var transactionCount = transactions
                    .AsParallel()
                    .WithCancellation(_cancelTokenSource.Token)
                    .Count(transaction =>
                    {
                        var set = new HashSet<string>();

                        foreach (var item in transaction)
                            if (converter.TryGetArticleGroup(item, out string group))
                            {
                                set.Add(group);
                                set.Add(item);
                            }

                        foreach (var item in set)
                            itemFrequencies.AddOrUpdate(item, 1, (_, value) => value + 1);

                        return true;
                    });

                if (transactionCount == 0)
                    return new List<AssociationRule>();

                var frequencyThreshold = (int)(transactionCount * parameters.Support);
                var frequentItems = itemFrequencies.Select(pair => pair.Key)
                    .Where(item => itemFrequencies[item] > frequencyThreshold)
                    .ToHashSet();

                var pairFrequencies = new ConcurrentDictionary<(string, string), int>();
                var processedTransactionCount = 0;

                var timer = new Timer
                (
                    callback: _ => MiningProgressChanged?.Invoke(this, processedTransactionCount / (double)transactionCount * 100),
                    state: null,
                    dueTime: 0,
                    period: 100
                );

                void UpdateFrequency((string, string) itemset)
                {
                    if (converter.TryConvertItemset(itemset, out (string, string)? convertedItemset) &&
                        frequentItems.Contains(convertedItemset.Value.Item1) &&
                        frequentItems.Contains(convertedItemset.Value.Item2))
                        pairFrequencies.AddOrUpdate(convertedItemset.Value, 1, (_, value) => value + 1);
                }

                transactions.AsParallel()
                    .WithCancellation(_cancelTokenSource.Token)
                    .ForAll(transaction =>
                    {
                        var set = new HashSet<(string, string)>();

                        for (var i = 0; i < transaction.Items.Count; i++)
                            for (var j = i + 1; j < transaction.Items.Count; j++)
                            {
                                UpdateFrequency((transaction.Items[i], transaction.Items[j]));
                                UpdateFrequency((transaction.Items[j], transaction.Items[i]));
                            }

                        processedTransactionCount++;
                    });

                timer.Dispose();

                return pairFrequencies
                    .Where(pair =>
                    {
                        if (pair.Value / (double)transactionCount < parameters.Support)
                            return false;
                        if (itemDeletionRules == null)
                            return true;
                        return !itemDeletionRules.Any
                        (
                            rule => rule.ShouldDelete(pair.Key.Item1) || rule.ShouldDelete(pair.Key.Item2)
                        );
                    })
                    .AsParallel()
                    .WithCancellation(_cancelTokenSource.Token)
                    .Select
                    (
                        pair => new AssociationRule
                        (
                            lhs: pair.Key.Item1,
                            rhs: pair.Key.Item2,
                            transactionCount: transactionCount,
                            pairCount: pair.Value,
                            lhsCount: itemFrequencies[pair.Key.Item1],
                            rhsCount: itemFrequencies[pair.Key.Item2]
                        )
                    )
                    .Where(rule => rule.Confidence >= parameters.Confidence)
                    .ToList();
        }

        /// <summary>
        /// Отмена поиска ассоциативных правил
        /// </summary>
        public void CancelMining() =>
            _cancelTokenSource?.Cancel();
    }
}