using System.Collections.Generic;

namespace MarketBasketAnalysis.Services
{
    /// <summary>
    /// Описание сохранение объектов в файлы, их загрузки из файлов
    /// </summary>
    public interface ISerializeStream
    {
        /// <summary>
        /// Загрузка данных из файла, десериализация объектов
        /// </summary>
        /// <typeparam name="T">Тип читаемых объектов</typeparam>
        /// <param name="path">Путь к файлу с данными</param>
        /// <returns>Прочитанные десериализованные объекты</returns>
        List<T> Read<T>(string path);

        /// <summary>
        /// Сериализация объектов, запись полученных данных в файл
        /// </summary>
        /// <typeparam name="T">Тип записываемых объектов</typeparam>
        /// <param name="path">Путь к файлу, в который будут записаны данные</param>
        /// <param name="data">Список объектов для записи</param>
        void Write<T>(string path, IReadOnlyList<T> data);
    }
}