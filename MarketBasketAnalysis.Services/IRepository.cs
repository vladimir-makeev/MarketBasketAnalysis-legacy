using System;
using System.Collections.Generic;

namespace MarketBasketAnalysis.Services
{
    /// <summary>
    /// Описание функционала хранилища объектов
    /// </summary>
    /// <typeparam name="T">Класс хранимых объектов</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Получение всех объектов
        /// </summary>
        /// <returns>Все объекты, содержащиеся в хранилище</returns>
        List<T> Get();

        /// <summary>
        /// Получение объектов, удовлетворяющих заданному условию
        /// </summary>
        /// <param name="predicate">Условие выбора объектов</param>
        /// <returns>Объекты, условие для которых истинно</returns>
        List<T> Get(Predicate<T> predicate);

        /// <summary>
        /// Добавление объектов в хранилище
        /// </summary>
        /// <param name="items">Добавляемые объекты</param>
        void Add(IReadOnlyList<T> items);

        /// <summary>
        /// Удаление объектов из хранилища
        /// </summary>
        /// <param name="items">Объекты для удаления</param>
        void Delete(IReadOnlyList<T> items);

        /// <summary>
        /// Очистка хранилища: удаление всех объектов
        /// </summary>
        void Clear();
    }
}