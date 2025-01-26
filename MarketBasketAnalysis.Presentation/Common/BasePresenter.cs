using CodeContracts;

namespace MarketBasketAnalysis.Presentation.Common
{
    /// <summary>
    /// Базовый класс представителя
    /// </summary>
    /// <typeparam name="T">Тип представления</typeparam>
    public abstract class BasePresenter<T> : IPresenter where T: class, IView
    {
        /// <summary>
        /// Контроллер приложения
        /// </summary>
        protected readonly ApplicationController controller;

        /// <summary>
        /// Средство показа, закрытия представлений
        /// </summary>
        protected readonly INavigator navigator;

        /// <summary>
        /// Представление
        /// </summary>
        protected readonly T view;

        /// <summary>
        /// Создание представителя
        /// </summary>
        /// <param name="controller">Контроллер приложения</param>
        /// <param name="navigator">Средство показа, закрытия представления</param>
        /// <param name="view">Представление</param>
        public BasePresenter(ApplicationController controller, INavigator navigator, T view)
        {
            Requires.NotNull(controller, nameof(controller));
            Requires.NotNull(navigator, nameof(navigator));
            Requires.NotNull(view, nameof(view));

            this.controller = controller;
            this.navigator = navigator;
            this.view = view;
        }

        /// <summary>
        /// Запуск представителя, показ представления
        /// </summary>
        public virtual void Run() =>
            navigator.ShowView(view);
    }

    /// <summary>
    /// Базовый класс параметризованного представителя
    /// </summary>
    /// <typeparam name="T">Тип представления</typeparam>
    /// <typeparam name="U">Тип параметра, передающегося представителю при его запуске</typeparam>
    public abstract class BasePresenter<T, U>: IPresenter<U> where T: class, IView
    {
        /// <summary>
        /// Контроллер приложения
        /// </summary>
        protected readonly ApplicationController controller;

        /// <summary>
        /// Средство показа, закрытия представлений
        /// </summary>
        protected readonly INavigator navigator;

        /// <summary>
        /// Представление
        /// </summary>
        protected readonly T view;

        /// <summary>
        /// Параметр представителя
        /// </summary>
        protected U parameter;

        /// <summary>
        /// Создание параметризованного представителя
        /// </summary>
        /// <param name="controller">Контроллер приложения</param>
        /// <param name="navigator">Средство показа, закрытия представлений</param>
        /// <param name="view">Представление</param>
        public BasePresenter(ApplicationController controller, INavigator navigator, T view)
        {
            Requires.NotNull(controller, nameof(controller));
            Requires.NotNull(navigator, nameof(navigator));
            Requires.NotNull(view, nameof(view));

            this.controller = controller;
            this.navigator = navigator;
            this.view = view;
        }

        /// <summary>
        /// Запуск представителя с параметром, показ представления
        /// </summary>
        /// <param name="parameter">Параметр</param>
        public virtual void Run(U parameter)
        {
            this.parameter = parameter;
            navigator.ShowView(view);
        }
    }
}