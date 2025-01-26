using System.Collections.Generic;
using CodeContracts;

namespace MarketBasketAnalysis.DomainModel.AssociationRules.Operations
{
    /// <summary>
    /// Результат операции над двумя наборами ассоциативных правил
    /// </summary>
    public class OperationResult
    {
        /// <summary>
        /// Первый результирующий набор ассоциативных правил
        /// </summary>
        public IReadOnlyCollection<AssociationRule> FirstResultSet { get; }

        /// <summary>
        /// Второй результирующий набор ассоциативных правил
        /// </summary>
        public IReadOnlyCollection<AssociationRule> SecondResultSet { get; }

        /// <summary>
        /// Создание результата операции над ассоциативными правилами
        /// </summary>
        /// <param name="firstResultSet">Первый результирующий набор ассоциативных правил</param>
        /// <param name="secondResultSet">Второй результирующий набор ассоциативных правил</param>
        public OperationResult
            (IReadOnlyCollection<AssociationRule> firstResultSet,
            IReadOnlyCollection<AssociationRule> secondResultSet)
        {
            Requires.NotNull(firstResultSet, nameof(firstResultSet));
            Requires.NotNull(secondResultSet, nameof(secondResultSet));

            FirstResultSet = firstResultSet;
            SecondResultSet = secondResultSet;
        }
    }
}