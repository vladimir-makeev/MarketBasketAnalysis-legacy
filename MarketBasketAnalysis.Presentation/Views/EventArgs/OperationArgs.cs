using CodeContracts;

namespace MarketBasketAnalysis.Presentation.Views.EventArguments
{
    /// <summary>
    /// Параметры запроса выполнения операции над списками ассоциативных правил
    /// </summary>
    public class OperationArgs
    {
        /// <summary>
        /// Путь к файлу с первым списком ассоциативных правил
        /// </summary>
        public string FirstAssociationRulesFilePath { get; }

        /// <summary>
        /// Путь к файлу со вторым списком ассоциативных правил
        /// </summary>
        public string SecondAssociaitonRulesFilePath { get; }

        /// <summary>
        /// Тип операции над списками ассоциативных правил
        /// </summary>
        public OperationType OperationType { get; }

        /// <summary>
        /// Следует ли учитывать направлении связи в ассоциативных правилах
        /// при выполнении операции
        /// </summary>
        public bool ConsiderDirection { get; }

        /// <summary>
        /// Задание параметров выполнения операций на списками ассоциативных правил
        /// </summary>
        /// <param name="firstAssociationRulesFielPath">Путь к файлу с первым списком ассоциативных правил</param>
        /// <param name="secondAssociationRulesFilePath">Путь к файлу со вторым списком ассоциативных правил</param>
        /// <param name="operationType">Тип операции над списками ассоциативных правил</param>
        /// <param name="considerDirection">
        /// Следует ли учитывать направлении связи в ассоциативных правилах
        /// при выполнении операции
        /// </param>
        public OperationArgs
            (string firstAssociationRulesFielPath,
            string secondAssociationRulesFilePath,
            OperationType operationType,
            bool considerDirection)
        {
            Requires.NotNull(firstAssociationRulesFielPath, nameof(firstAssociationRulesFielPath));
            Requires.NotNull(secondAssociationRulesFilePath, nameof(secondAssociationRulesFilePath));

            this.FirstAssociationRulesFilePath = firstAssociationRulesFielPath;
            this.SecondAssociaitonRulesFilePath = secondAssociationRulesFilePath;
            this.OperationType = operationType;
            this.ConsiderDirection = considerDirection;
        }
    }
}