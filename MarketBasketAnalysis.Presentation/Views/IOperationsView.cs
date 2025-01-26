using System;
using MarketBasketAnalysis.Presentation.Common;
using MarketBasketAnalysis.Presentation.Views.EventArguments;

namespace MarketBasketAnalysis.Presentation.Views
{
    /// <summary>
    /// Представление выполнения операций над списками ассоциативных правил
    /// </summary>
    public interface IOperationsView : IView
    {
        /// <summary>
        /// Событие запроса выполнения операции
        /// </summary>
        event EventHandler<OperationArgs> OperationExecutionRequested;
    }
}