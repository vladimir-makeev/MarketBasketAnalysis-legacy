using System;

namespace MarketBasketAnalysis.Presentation.Views.EventArguments
{
    /// <summary>
    /// Интервал значений типа
    /// </summary>
    /// <typeparam name="T">Тип, поддерживающий упорядочение экземпляров</typeparam>
    public class Interval<T> where T: IComparable<T>
    {
        /// <summary>
        /// Левая граница интервала
        /// </summary>
        public T Min { get; }

        /// <summary>
        /// Правая граница интервала
        /// </summary>
        public T Max { get; }


        /// <summary>
        /// Создание интервала, задаваемого границами значений
        /// </summary>
        /// <param name="min">Левая граница интервала</param>
        /// <param name="max">Правая граница интервала</param>
        public Interval(T min, T max)
        {
            this.Min = min;
            this.Max = max;
        }
    }
}