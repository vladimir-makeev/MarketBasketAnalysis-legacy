using System.ComponentModel.DataAnnotations;
using CodeContracts;

namespace MarketBasketAnalysis.DomainModel.ProductHierarchy
{
    /// <summary>
    /// Элемент товарной иерархии
    /// </summary>
    public class HierarchyItem
    {
        /// <summary>
        /// Название товара
        /// </summary>
        [Key]
        public string Article { get; set; }

        /// <summary>
        /// Наименование группы товаров
        /// </summary>
        public string ArticleGroup { get; set; }

        /// <summary>
        /// Создание элемента иерархии
        /// </summary>
        /// <param name="article">Название товара</param>
        /// <param name="articleGroup">Наименование группы товаров</param>
        public HierarchyItem(string article, string articleGroup)
        {
            Requires.NotNull(article, nameof(article));
            Requires.NotNull(articleGroup, nameof(articleGroup));

            Article = article;
            ArticleGroup = articleGroup;
        }

        /// <summary>
        /// Конструктор для десериализации объекта
        /// </summary>
        private HierarchyItem()
        {
        
        }
    }
}