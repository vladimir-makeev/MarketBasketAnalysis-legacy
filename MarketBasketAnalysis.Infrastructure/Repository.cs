using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using CodeContracts;
using MarketBasketAnalysis.Services;

namespace MarketBasketAnalysis.Infrastructure
{
    /// <summary>
    /// Репозиторий, взаимодействующий с базой данных
    /// </summary>
    /// <typeparam name="T">Класс хранимой сущности</typeparam>
    public class Repository<T> : IRepository<T> where T : class
    {
        /// <summary>
        /// Контекст базы данных
        /// </summary>
        private readonly DbContext _dbContext;

        /// <summary>
        /// Набор экземпляров сущности, хранящихся в базе данных
        /// </summary>
        private readonly DbSet<T> _dbSet;

        /// <summary>
        /// Создание репозитория
        /// </summary>
        /// <param name="dbContext">Контекст базы данных</param>
        public Repository(DbContext dbContext)
        {
            Requires.NotNull(dbContext, nameof(dbContext));

            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
        }

        /// <summary>
        /// Добавление экземпляров сущности в базу даннных
        /// </summary>
        /// <param name="items">Добавляемые экземпляры сущности</param>
        public void Add(IReadOnlyList<T> items)
        {
            Requires.NotNull(items, nameof(items));

            _dbSet.AddRange(items.ToArray());
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Удаления экземпляров сущности из базы данных
        /// </summary>
        /// <param name="items">Экземпляры сущности для удаления</param>
        public void Delete(IReadOnlyList<T> items)
        {
            Requires.NotNull(items, nameof(items));

            _dbSet.RemoveRange(items);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Получение всех экземпляров сущности, хранящихся в базе данных
        /// </summary>
        /// <returns>Экземпляры сущности</returns>
        public List<T> Get() =>
            _dbSet.AsNoTracking().ToList();

        /// <summary>
        /// Получение экземпляров сущности, удовлетворяющих заданному условию выборки
        /// </summary>
        /// <param name="predicate">Условие, по которому производится отбор элементов</param>
        /// <returns>Экземпляры сущности, для которых условие истинно</returns>
        public List<T> Get(Predicate<T> predicate)
        {
            Requires.NotNull(predicate, nameof(predicate));

            return _dbSet.Where(item => predicate(item)).AsNoTracking().ToList();
        }

        /// <summary>
        /// Удаление всех экземпляров сущности из базы данных
        /// </summary>
        public void Clear()
        {
            _dbSet.RemoveRange(_dbSet);
            _dbContext.SaveChanges();
        }
    }
}