using CodeContracts;
using QuickGraph;

namespace MarketBasketAnalysis.DomainModel.AssociationRules.Graph
{
    /// <summary>
    /// Ребро графа ассоциативных правил
    /// </summary>
    public class Edge : Edge<Vertex>
    {

        /// <summary>
        /// Ассоциативное правило, представляющее связь между начальной и конечной вершинами
        /// </summary>
        public AssociationRule AssociationRule { get; }

        /// <summary>
        /// Создание вершины графа
        /// </summary>
        /// <param name="rule">Ассоциативное правило для создания ребра графа</param>
        internal Edge(AssociationRule rule) :
            base
            (
                new Vertex(rule.LeftHandSide, rule.LHSCount, rule.LHSSupport),
                new Vertex(rule.RightHandSide, rule.RHSCount, rule.RHSSupport)
            )
        {
            Requires.NotNull(rule, nameof(rule));

            AssociationRule = rule;
        }
    }
}