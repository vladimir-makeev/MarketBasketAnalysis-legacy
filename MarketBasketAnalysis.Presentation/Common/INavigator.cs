using System;

namespace MarketBasketAnalysis.Presentation.Common
{
    /// <summary>
    /// Описание функционала показа, закрытия представлений
    /// </summary>
    public interface INavigator
    {
        /// <summary>
        /// Событие закрытия представления
        /// Закрытие выполняется программно или пользователем
        /// </summary>
        event EventHandler<IView> ViewClosed;

        /// <summary>
        /// Показ представления
        /// </summary>
        /// <param name="view">Представление для показа</param>
        void ShowView(IView view);

        /// <summary>
        /// Закрытие представления
        /// </summary>
        /// <param name="view">Представление, которое требуется закрыть</param>
        void CloseView(IView view);
    }
}