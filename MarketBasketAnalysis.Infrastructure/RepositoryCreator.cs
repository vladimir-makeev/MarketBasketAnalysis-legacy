using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity;
using MarketBasketAnalysis.Services;

namespace MarketBasketAnalysis.Infrastructure
{
    /// <summary>
    /// Создатель репозиториев, инкапсулирующих работу с базой данных
    /// </summary>
    public class RepositoryCreator : IRepositoryCreator
    {
        /// <summary>
        /// Контекст базы данных
        /// </summary>
        private static readonly AppDbContext _dbContext;

        /// <summary>
        /// Инициализация подключения к базе данных
        /// </summary>
        static RepositoryCreator() =>
            _dbContext = new AppDbContext();

        public static void LoadData()
        {
            _dbContext.HierarchyItems.Load();
            _dbContext.ItemDeletionRules.Load();
            _dbContext.ItemReplacementRules.Load();
        }

        /// <summary>
        /// Получение репозитория для сущности указанного класса
        /// </summary>
        /// <typeparam name="T">Класс сущности</typeparam>
        /// <returns>Репозиторий сущности</returns>
        public IRepository<T> GetRepository<T>() where T : class =>
            new Repository<T>(_dbContext);
    }
}