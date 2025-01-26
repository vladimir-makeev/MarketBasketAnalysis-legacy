using CodeContracts;

namespace MarketBasketAnalysis.DomainModel.AssociationRules.Graph
{
    /// <summary>
    /// Вершина графа ассоциативных правил
    /// </summary>
    public class Vertex
    {
        /// <summary>
        /// Наименование вершины - название товара
        /// </summary>
        public string Item { get; }

        /// <summary>
        /// Количество транзакций, содержащих товар
        /// </summary>
        public int Count { get; }

        /// <summary>
        /// Поддержка - частота встречаемости товара в транзакциях
        /// </summary>
        public double Support { get; }

        /// <summary>
        /// Хэш-значение, вычисляемое по значениям свойств класса
        /// </summary>
        private int _hashcode;

        /// <summary>
        /// Создание вершины графа
        /// </summary>
        /// <param name="item">Название товара</param>
        /// <param name="count">Количество транзакций, содержащих товар</param>
        /// <param name="support">Поддержка - частота встречаемости товара в транзакциях</param>
        internal Vertex(string item, int count, double support)
        {
            Requires.NotNullOrEmpty(item, nameof(item));
            Requires.InRange(count > 0, nameof(count));
            Requires.InRange(support > 0 && support <= 1, nameof(support));

            this.Item = item;
            this.Count = count;
            this.Support = support;
            _hashcode = (item, count, support).GetHashCode();
        }

        /// <summary>
        /// Преобразование экземпляра класса в строку
        /// </summary>
        /// <returns>Наименование вершины</returns>
        public override string ToString()
        {
            return Item;
        }

        /// <summary>
        /// Сравнение переданного объекта с текущим экземпляром класса
        /// </summary>
        /// <param name="obj">Объект для сравнения</param>
        /// <returns>
        /// Если параметр объекта имеет нулевое значение, возвращается ложь
        /// Если переданный объект является экземпляром данного класса,
        /// выполняется сравнение с текущим объектом по значению,
        /// иначе объект передается аналогичному методу родительского класса
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj is Vertex)
            {
                var vertex = (Vertex)obj;
                return vertex.Item == this.Item &&
                    vertex.Count == this.Count &&
                    vertex.Support == this.Support;
            }
            return base.Equals(obj);
        }

        /// <summary>
        /// Получение хеш-значения класса
        /// </summary>
        /// <returns>Хеш-значение, вычисленное по значениям свойств класса</returns>
        public override int GetHashCode() =>
            _hashcode;
    }
}