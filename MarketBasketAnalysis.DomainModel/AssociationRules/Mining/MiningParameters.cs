using CodeContracts;

namespace MarketBasketAnalysis.DomainModel.AssociationRules.Mining
{
    /// <summary>
    /// Параметры поиска ассоциативных правил
    /// </summary>
    public class MiningParameters
    {
        /// <summary>
        /// Порог поддержки
        /// </summary>
        public double Support { get; }
        
        /// <summary>
        /// Порог достоверности
        /// </summary>
        public double Confidence { get; }

        /// <summary>
        /// Задание параметров поиска ассоциативных правил
        /// </summary>
        /// <param name="support">Порог поддержки</param>
        /// <param name="confidence">Порог достоверности</param>
        public MiningParameters(double support, double confidence)
        {
            Requires.InRange(support >= 0 && support <= 1, nameof(support));
            Requires.InRange(confidence >= 0 && confidence <= 1, nameof(confidence));

            this.Support = support;
            this.Confidence = confidence;
        }
    }
}