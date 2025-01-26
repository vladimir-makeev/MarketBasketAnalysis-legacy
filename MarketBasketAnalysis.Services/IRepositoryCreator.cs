namespace MarketBasketAnalysis.Services
{
    /// <summary>
    /// Описание функционала создателя репозиториев
    /// </summary>
    public interface IRepositoryCreator
    {
        /// <summary>
        /// Получение хранилища указанного класса объектов
        /// </summary>
        /// <typeparam name="T">Класс хранимых объектов</typeparam>
        /// <returns>Хранилище объектов</returns>
        IRepository<T> GetRepository<T>() where T : class;
    }
}