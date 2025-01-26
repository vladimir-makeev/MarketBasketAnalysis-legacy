using System.Linq;
using System.Collections.Generic;
using CodeContracts;
using QuickGraph;

namespace MarketBasketAnalysis.DomainModel.AssociationRules.Graph
{
    /// <summary>
    /// Средство создания графа ассоциативных правил
    /// </summary>
    public class Graph : BidirectionalGraph<Vertex, Edge>
    {
        /// <summary>
        /// Создание графа на основе ассоциативных правил:
        /// - вершины графа - левые и правые части ассоциативных правил
        /// - ребра графа есть ассоциативные правила, отражающие связь между товарами
        /// </summary>
        /// <param name="rules">Ассоциативные правила</param>
        /// <returns>Граф ассоциативных правил</returns>
        public Graph(IReadOnlyList<AssociationRule> rules)
        {
            Requires.NotNull(rules, nameof(rules));

            var graph = new BidirectionalGraph<Vertex, Edge>();
            this.AddVerticesAndEdgeRange(rules.AsParallel().Select(rule => new Edge(rule)));
        }

        /// <summary>
        /// Поиск максимальных подграфов в графе,
        /// где каждая пара вершин соединена хотя бы одним ребром
        /// </summary>
        /// <param name="minCliqueSize">Минимальное число вершин подграфе</param>
        /// <param name="maxCliqueSize">Максимальное число вершин в подграфе</param>
        /// <returns>Список найденных максимальных подграфов</returns>
        public List<Graph> FindMaxCliques(int minCliqueSize, int maxCliqueSize)
        {
            Requires.InRange(minCliqueSize > 1, nameof(minCliqueSize));
            Requires.InRange(maxCliqueSize > 1, nameof(maxCliqueSize));
            Requires.True(minCliqueSize <= maxCliqueSize, "Максимальное количество подграфа меньше минимального");

            return new BronKerboschAlgorithm()
                .FindMaxCliques(this, minCliqueSize, maxCliqueSize);
        }
    }
}