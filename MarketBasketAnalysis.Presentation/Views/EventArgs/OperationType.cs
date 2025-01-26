namespace MarketBasketAnalysis.Presentation.Views.EventArguments
{
    /// <summary>
    /// Тип операции над списками ассоциативных правил
    /// </summary>
    public enum OperationType
    {
        /// <summary>
        /// Поиск общих элементов списков
        /// </summary>
        Intersection,

        /// <summary>
        /// Поиск уникальных элементов списков
        /// </summary>
        SymmetricDifference
    }
}