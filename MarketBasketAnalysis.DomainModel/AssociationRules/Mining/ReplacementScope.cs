namespace MarketBasketAnalysis.DomainModel.AssociationRules.Mining
{
    /// <summary>
    /// Область действия замены товаров товарными группами в асоциативных правилах
    /// </summary>
    public enum ReplacementScope
    {
        /// <summary>
        /// Левая часть правила
        /// </summary>
        LeftHandSide,

        /// <summary>
        /// Правая часть правила
        /// </summary>
        RightHandSide,

        /// <summary>
        /// Обе части правила
        /// </summary>
        BothHandSides
    }
}