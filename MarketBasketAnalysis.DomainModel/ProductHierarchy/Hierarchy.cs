using System.Collections.Generic;
using CodeContracts;
using MarketBasketAnalysis.DataAccess;

namespace MarketBasketAnalysis.DomainModel.ProductHierarchy
{
    /// <summary>
    /// Товарная иерархия
    /// </summary>
    public class Hierarchy
    {
        /// <summary>
        /// Путь к файлу с описанием товарной ирерахии
        /// </summary>
        private const string HIERARCHY_FILE_PATH = "ProductHierarchy/hierarchy.csv";

        /// <summary>
        /// Элементы иерархии товаров
        /// </summary>
        public IReadOnlyList<HierarchyItem> HierarchyItems { get; }

        /// <summary>
        /// Создание экземпляра класса: получение данных иерархии из файла
        /// </summary>
        /// <param name="hierarchyItemsFile">Средство чтения данных из файла</param>
        public Hierarchy(ObjectStream<HierarchyItem> hierarchyItemsFile)
        {
            Requires.NotNull(hierarchyItemsFile, nameof(hierarchyItemsFile));
            HierarchyItems = hierarchyItemsFile.Read(HIERARCHY_FILE_PATH).Result;
        }
    }
}