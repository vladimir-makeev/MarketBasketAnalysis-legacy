using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using CodeContracts;

namespace MarketBasketAnalysis.DomainModel.AssociationRules.Operations
{
    /// <summary>
    /// Бинарные операции над списками ассоциативных правил
    /// </summary>
    public class Operations
    {
        /// <summary>
        /// Поиск общих элементов списков ассоциативных правил: два правила являются общими,
        /// если множества, содержащие их левую и правую части совпадают
        /// </summary>
        /// <param name="first">Первый список ассоциативных правил</param>
        /// <param name="second">Второй список ассоциативных правил</param>
        /// <param name="considerDirection">
        /// Следует ли учитывать направление связи в ассоцитивных правилах при их сравнении
        /// </param>
        /// <returns>
        /// Два списка ассоциативных правил:
        /// первый список формируется на основе элементов первого списка-аргумента, также принадлежащих и второму
        /// второй список - на основе правил второго списка-аргумента, принадлежащих первому списку-аргументу
        /// </returns>
        public OperationResult Intersection
            (IReadOnlyList<AssociationRule> first,
            IReadOnlyList<AssociationRule> second,
            bool considerDirection)
        {
            Requires.NotNull(first, nameof(first));
            Requires.NotNull(second, nameof(second));

            var firstResultSet = new ConcurrentBag<AssociationRule>();
            var secondResultSet = new ConcurrentBag<AssociationRule>();

            first.AsParallel().Join
            (
                inner: second.AsParallel(),
                outerKeySelector: rule => GetKey(rule, considerDirection),
                innerKeySelector: rule => GetKey(rule, considerDirection),
                resultSelector: (firstRule, secondRule) => (firstRule, secondRule)
            ).ForAll(result =>
            {
                firstResultSet.Add(result.firstRule);
                secondResultSet.Add(result.secondRule);
            });

            return new OperationResult
            (
                firstResultSet: firstResultSet.Distinct().ToList(),
                secondResultSet: secondResultSet.Distinct().ToList()
            );
        }

        /// <summary>
        /// Выделение уникальных элементов списков ассоциативных правил: выполняется поиск правил,
        /// множества, состоящие из левой и правой части которых представлены только в первом
        /// или втором списке
        /// </summary>
        /// <param name="first">Первый список ассоциативных правил</param>
        /// <param name="second">Второй список ассоциативных правил</param>
        /// <param name="considerDirection">
        /// Следует ли учитывать направление связи в ассоциативных правилах при их сравнении
        /// </param>
        /// <returns>
        /// Два списка ассоциативных правил с парами товаров:
        /// - первый список включает правила первого списка-аргумента, не принадлежащие второму списку-аргументу
        /// - второй список состоит из правил второго списка-аргумента, не входящих в первый список-аргумент
        /// </returns>
        public OperationResult SymmetricDifference
            (IReadOnlyList<AssociationRule> first,
            IReadOnlyList<AssociationRule> second,
            bool considerDirection)
        {
            Requires.NotNull(first, nameof(first));
            Requires.NotNull(second, nameof(second));

            var intersection = Intersection(first, second, considerDirection);

            return new OperationResult
            (
                firstResultSet: first.Except(intersection.FirstResultSet)
                    .ToList(),
                secondResultSet: second.Except(intersection.SecondResultSet)
                    .ToList()
            );
        }

        /// <summary>
        /// Создание ключа для соединения списков ассоциативных правил
        /// </summary>
        /// <param name="rule">Ассоциативное правило, на основе которого создается ключ</param>
        /// <param name="considerDirection">
        /// Учитывать ли направление связи ассоциативного правила при создании ключа
        /// </param>
        /// <returns>Сформированный на основе ассоциативного правила ключ</returns>
        public (string, string) GetKey(AssociationRule rule, bool considerDirection)
        {
            if (!considerDirection && rule.LeftHandSide.CompareTo(rule.RightHandSide) < 0)
                return (rule.RightHandSide, rule.LeftHandSide);
            return (rule.LeftHandSide, rule.RightHandSide);
        }
    }
}