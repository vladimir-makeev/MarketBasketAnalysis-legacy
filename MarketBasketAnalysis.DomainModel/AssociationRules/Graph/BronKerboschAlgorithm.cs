using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;
using CodeContracts;
using System;

namespace MarketBasketAnalysis.DomainModel.AssociationRules.Graph
{
    /// <summary>
    /// Алгоритм Брона-Кербоша для поиска максимальных клик в графе,
    /// адаптированный для поиска максимальных подграфов в графе ассоциативных правил,
    /// где каждая пара вершин соединена хотя бы одним ребром
    /// </summary>
    internal class BronKerboschAlgorithm
    {
        /// <summary>
        /// Стек с множествами вершин в рекурсивной процедуре алгоритма
        /// </summary>
        private class Stack
        {
            /// <summary>
            /// Множество вершин, содержащее полный подграф на данном шаге
            /// </summary>
            public IEnumerable<object> Clique;

            /// <summary>
            /// Множество вершин, которые могут быть включены в полный подграф
            /// </summary>
            public IEnumerable<object> ProbablyCliqueVertices;

            /// <summary>
            /// Множество вершин, использовавшихся при расширении полного подграфа
            /// </summary>
            public IEnumerable<object> ExcludedVertices;
        }

        /// <summary>
        /// Поиск максимальных подграфов графа ассоциативных правил,
        /// где каждая пара вершин соединена хотя бы одним ребром и
        /// число вершин которых принадлежит заданному диапазону
        /// </summary>
        /// <param name="graph">Граф ассоциативных правил</param>
        /// <param name="minCliqueSize">Минимальное число вершин в подграфе</param>
        /// <param name="maxCliqueSize">Максимальное число вершин в подграфе</param>
        /// <returns>
        /// Найденные максимальные подграфы графа ассоциативных правил
        /// </returns>
        public List<Graph> FindMaxCliques(Graph graph, int minCliqueSize, int maxCliqueSize)
        {
            Requires.NotNull(graph, nameof(maxCliqueSize));
            Requires.InRange(minCliqueSize > 1, nameof(minCliqueSize));
            Requires.InRange(maxCliqueSize > 1, nameof(maxCliqueSize));
            Requires.True(minCliqueSize <= maxCliqueSize, "Максимальный размер клик меньше минимального");

            var rules = graph.Edges.Select(edge => edge.AssociationRule).ToList();
            var items = rules.Select(rule => new[] { rule.LeftHandSide, rule.RightHandSide })
                .SelectMany(array => array)
                .Distinct()
                .ToDictionary(item => item, item => (object)item);

            return RunBronKerboschAlgorithm
            (
                verticesNeighbours: rules.SelectMany(rule => new[]
                {
                    (rule.LeftHandSide, rule.RightHandSide),
                    (rule.RightHandSide, rule.LeftHandSide)
                }).Distinct()
                .GroupBy(tuple => items[tuple.Item1], tuple => items[tuple.Item2])
                .ToDictionary(group => group.Key, group => group.Cast<object>()),
                maxCliqueSize: maxCliqueSize
            ).Select(clique => clique.Cast<string>().ToList())
            .Where(clique => clique.Count >= minCliqueSize && clique.Count <= maxCliqueSize)
            .Select(clique => rules.Where
            (
                rule => clique.Contains(rule.LeftHandSide) && clique.Contains(rule.RightHandSide)
            ).ToList())
            .Select(rules => new Graph(rules))
            .ToList();
        }

        /// <summary>
        /// Реализация алгоритма Брона-Кербоша
        /// </summary>
        /// <param name="verticesNeighbours">Словарь, где ключ - вершина, значение - список вершин, смежных с ней</param>
        /// <param name="maxCliqueSize">Наибольшее количество вершин подграфов</param>
        /// <returns>Найденные максимальные подграфы в виде списков их вершин</returns>
        private IEnumerable<IEnumerable<object>> RunBronKerboschAlgorithm
            (Dictionary<object, IEnumerable<object>> verticesNeighbours,
            int maxCliqueSize)
        {
            var stacks = new List<Stack>();
            var cliques = new ConcurrentBag<IEnumerable<object>>();

            stacks.Add(new Stack
            {
                Clique = new object[0],
                ProbablyCliqueVertices = verticesNeighbours.Keys,
                ExcludedVertices = new object[0]
            });

            while (stacks.Count != 0)
            {
                stacks = stacks.AsParallel().Select(stack =>
                {
                    var newStacks = new List<Stack>();
                    if (!stack.ProbablyCliqueVertices.Any() && !stack.ExcludedVertices.Any())
                        cliques.Add(stack.Clique);
                    if (stack.Clique.ToList().Count == maxCliqueSize)
                        return null;
                    foreach (var vertex in stack.ProbablyCliqueVertices.ToList())
                    {
                        newStacks.Add
                        (
                            new Stack
                            {
                                Clique = stack.Clique.Append(vertex),
                                ProbablyCliqueVertices = stack.ProbablyCliqueVertices
                                    .Intersect(verticesNeighbours[vertex]),
                                ExcludedVertices = stack.ExcludedVertices
                                    .Intersect(verticesNeighbours[vertex])
                            }
                        );
                        stack.ProbablyCliqueVertices = stack.ProbablyCliqueVertices.Where(probablyVertex => probablyVertex != vertex);
                        stack.ExcludedVertices = stack.ExcludedVertices.Append(vertex);
                    }
                    return newStacks;
                }).Where(stacks => stacks != null)
                .SelectMany(stacks => stacks)
                .ToList();
                GC.Collect();
            }
            return cliques;
        }
    }
}