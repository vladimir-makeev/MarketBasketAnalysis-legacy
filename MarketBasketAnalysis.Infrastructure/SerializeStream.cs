using System.IO;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Collections.Generic;
using CodeContracts;
using CsvHelper;
using MarketBasketAnalysis.Services;

namespace MarketBasketAnalysis.Infrastructure
{
    /// <summary>
    /// Сохранение, загрузка данных в формате CSV-файла
    /// </summary>
    public class SerializeStream : ISerializeStream
    {
        /// <summary>
        /// Разделитель значений в CSV-файле
        /// </summary>
        private const string DELIMITER = ";";

        /// <summary>
        /// Сериализация объектов, запись полученных данных в файл
        /// </summary>
        /// <typeparam name="T">Тип записываемых объектов</typeparam>
        /// <param name="path">Путь к файлу, в который будут записаны данные</param>
        /// <param name="data">Список объектов для записи</param>
        public void Write<T>(string path, IReadOnlyList<T> data)
        {
            Requires.NotNullOrEmpty(path, nameof(path));
            Requires.NotNull(data, nameof(data));

            using (var streamWriter = new StreamWriter(path, false, Encoding.UTF8))
            using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
            {
                csvWriter.Configuration.Delimiter = DELIMITER;
                csvWriter.WriteRecords(data);
            }
        }

        /// <summary>
        /// Загрузка данных из файла, десериализация объектов
        /// </summary>
        /// <typeparam name="T">Тип читаемых объектов</typeparam>
        /// <param name="path">Путь к файлу с данными</param>
        /// <returns>Прочитанные десериализованные объекты</returns>
        public List<T> Read<T>(string path)
        {
            Requires.NotNullOrEmpty(path, nameof(path));
            Requires.True(File.Exists(path), nameof(path), $"Не найден файл {path}");

            using (var streamReader = new StreamReader(path, Encoding.UTF8))
            using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
            {
                csvReader.Configuration.Delimiter = DELIMITER;
                csvReader.Configuration.IncludePrivateMembers = true;
                csvReader.Configuration.ShouldUseConstructorParameters += _ => false;
                return csvReader.GetRecords<T>().ToList();
            }
        }
    }
}