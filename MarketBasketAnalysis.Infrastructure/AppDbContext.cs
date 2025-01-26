using System.Data.Entity;
using MarketBasketAnalysis.DomainModel.ProductHierarchy;
using MarketBasketAnalysis.DomainModel.AssociationRules.Mining;

namespace MarketBasketAnalysis.Infrastructure
{
    /// <summary>
    /// Контекст базы данных приложения
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Элементы товарной иерархии
        /// </summary>
        public DbSet<HierarchyItem> HierarchyItems { get; set; }

        /// <summary>
        /// Правила удаления элементов транзакций
        /// </summary>
        public DbSet<ItemDeletionRule> ItemDeletionRules { get; set; }

        /// <summary>
        /// Правила замены СКЮ группами товаров в ассоциативных правилах
        /// </summary>
        public DbSet<ItemReplacementRule> ItemReplacementRules { get; set; }

        /// <summary>
        /// Создание подключения к базе данных
        /// </summary>
        public AppDbContext() : base("AppDbConnection")
        {
            
        }
    }
}