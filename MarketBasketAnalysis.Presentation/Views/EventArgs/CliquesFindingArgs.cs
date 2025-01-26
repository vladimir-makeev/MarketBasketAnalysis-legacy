using CodeContracts;

namespace MarketBasketAnalysis.Presentation.Views.EventArguments
{
    /// <summary>
    /// Параметры запроса поиска максимальных подграфов графа ассоциативных правил
    /// </summary>
    public class CliquesFindingArgs
    {
        /// <summary>
        /// Диапазон допустимого количества вершин максимальных подграфов, которые требуется найти
        /// </summary>
        public Interval<int> CliqueSize { get; }

        /// <summary>
        /// Диапазон допустимого количества вершин максимальных подграфов, которые требуется найти
        /// </summary>
        /// <param name="cliqueSize">Диапазон количества вершин подграфов</param>
        public CliquesFindingArgs(Interval<int> cliqueSize)
        {
            Requires.NotNull(cliqueSize, nameof(cliqueSize));

            CliqueSize = cliqueSize;
        }
    }
}