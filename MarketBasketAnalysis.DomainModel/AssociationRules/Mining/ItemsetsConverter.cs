using System.Linq;
using System.Collections.Generic;
using CodeContracts;
using MarketBasketAnalysis.DomainModel.ProductHierarchy;

namespace MarketBasketAnalysis.DomainModel.AssociationRules.Mining
{
    /// <summary>
    /// Конвертер одноэлементных и двуэлементных наборов
    /// </summary>
    public class ItemsetsConverter
    {
        /// <summary>
        /// Двухуровневая иерархия: ключ - товар, значение - группа товаров
        /// </summary>
        private IReadOnlyDictionary<string, string> _twoLevelHierarchy;

        /// <summary>
        /// Правила для замены частей наборов, являющихся товарами, группами товаров
        /// </summary>
        private IReadOnlyDictionary<string, ItemReplacementRule> _replacement;

        /// <summary>
        /// Создание экземпляра класса
        /// </summary>
        /// <param name="items">Элементы иерархии</param>
        /// <param name="rules">Правила замены товаров группами товаров</param>
        public ItemsetsConverter(IReadOnlyList<HierarchyItem> items, IReadOnlyList<ItemReplacementRule> rules = null)
        {
            Requires.NotNull(items, nameof(items));

            _twoLevelHierarchy = items.ToDictionary(item => item.Article, item => item.ArticleGroup);
            _replacement = rules?.ToDictionary(item => item.ArticleGroup, item => item);
        }

        /// <summary>
        /// Попытка получения группы товаров для заданного товара
        /// </summary>
        /// <param name="item">Наименование товара</param>
        /// <param name="articleGroup">
        /// Выходная переменная, имеет значение null, если группа товаров не была найдена,
        /// иначе содержит наименование группы, соответствующей заданному товару</param>
        /// <returns>Истина, если группа товаров для указанного товара была найдена, иначе - ложь</returns>
        public bool TryGetArticleGroup(string item, out string articleGroup)
        {
            Requires.NotNull(item, nameof(item));

            if (_twoLevelHierarchy.ContainsKey(item))
            {
                articleGroup = _twoLevelHierarchy[item];
                return true;
            }

            articleGroup = null;
            return false;
        }

        /// <summary>
        /// Попытка замены частей двуэлементного набора, содержащего товары, группами товаров
        /// в соответствии с правилами замены
        /// </summary>
        /// <param name="itemset">Набор, в котором требуется провести замену</param>
        /// <param name="convertedItemset">
        /// Выходная переменная, содержащая преобразованный набор в случае успешной замены, иначе - значение null
        /// </param>
        /// <returns>
        /// Истина, если замена выполнена успешна, иначе - ложь
        /// </returns>
        public bool TryConvertItemset((string, string) itemset, out (string, string)? convertedItemset)
        {
            _twoLevelHierarchy.TryGetValue(itemset.Item1, out string leftArticleGroup);
            _twoLevelHierarchy.TryGetValue(itemset.Item2, out string rightArticleGroup);

            if (leftArticleGroup == null || rightArticleGroup == null)
            {
                convertedItemset = null;
                return false;
            }

            ItemReplacementRule leftReplacementItem = null;
            ItemReplacementRule rightReplacementItem = null;

            _replacement?.TryGetValue(leftArticleGroup, out leftReplacementItem);
            _replacement?.TryGetValue(rightArticleGroup, out rightReplacementItem);

            if (leftReplacementItem == null && rightReplacementItem == null)
            {
                convertedItemset = itemset;
                return true;
            }
            else if (rightReplacementItem == null)
            {
                if (leftReplacementItem.Scope != ReplacementScope.RightHandSide)
                    convertedItemset = (leftReplacementItem.ArticleGroup, itemset.Item2);
                else
                    convertedItemset = itemset;
                return true;
            }
            else if (leftReplacementItem == null)
            {
                if (rightReplacementItem.Scope != ReplacementScope.LeftHandSide)
                    convertedItemset = (itemset.Item1, rightReplacementItem.ArticleGroup);
                else
                    convertedItemset = itemset;
                return true;
            }
            else if (leftReplacementItem.ArticleGroup != rightReplacementItem.ArticleGroup)
            {
                convertedItemset =
                (
                    item1: leftReplacementItem.Scope != ReplacementScope.RightHandSide ?
                        leftReplacementItem.ArticleGroup :
                        itemset.Item1,
                    item2: rightReplacementItem.Scope != ReplacementScope.LeftHandSide ?
                        rightReplacementItem.ArticleGroup :
                        itemset.Item2
                );
                return true;
            }

            convertedItemset = null;
            return false;
        }
    }
}