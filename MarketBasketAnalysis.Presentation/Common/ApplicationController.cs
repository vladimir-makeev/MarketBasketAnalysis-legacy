using System.Windows.Threading;
using Unity;

namespace MarketBasketAnalysis.Presentation.Common
{
    /// <summary>
    /// Контроллер приложения, представляющий централизованный контроль
    /// над представлениями, представителями и их зависимостями
    /// </summary>
    public class ApplicationController
    {
        public Dispatcher _uiDispatcher;

        /// <summary>
        /// Контейнер инверсии управления
        /// </summary>
        private readonly IUnityContainer _container;

        /// <summary>
        /// Создания контроллера приложения: инициализация IoC-контейнера
        /// </summary>
        public ApplicationController(Dispatcher dispatcher)
        {
            _container = new UnityContainer();
            _container.RegisterInstance(this);
            _uiDispatcher = dispatcher;
        }

        /// <summary>
        /// Регистрация зависимости в форме одиночки
        /// </summary>
        /// <typeparam name="T">Класс зависимости</typeparam>
        /// <returns>Контроллер приложения</returns>
        public ApplicationController RegisterSingleton<T>() where T : class
        {
            _container.RegisterSingleton<T>();
            return this;
        }

        /// <summary>
        /// Регистрация зависимости в форме одиночки для интерфейса и класса его реализующего
        /// или базового класса и класса-наследника
        /// </summary>
        /// <typeparam name="T">Интерфейс (родительский класс)</typeparam>
        /// <typeparam name="U">Класс, реализующий интерфейс (класс-наследник)</typeparam>
        /// <returns>Контроллер приложения</returns>
        public ApplicationController RegisterSingleton<T, U>() where U : class, T
        {
            _container.RegisterSingleton<U>();
            _container.RegisterSingleton<T, U>();
            return this;
        }

        /// <summary>
        /// Регистрация зависимости класса
        /// </summary>
        /// <typeparam name="T">Класс зависимости</typeparam>
        /// <returns>Контроллер приложения</returns>
        public ApplicationController Register<T>() where T : class
        {
            _container.RegisterType<T>();
            return this;
        }

        /// <summary>
        /// Регистрация зависимости в виде интерфейса и реализующего его класса или 
        /// базового класса и класса-наследника
        /// </summary>
        /// <typeparam name="T">Интерфейс (родительский класс)</typeparam>
        /// <typeparam name="U">Класс, реализующий интерфейс (класс-потомок)</typeparam>
        /// <returns>Контроллер приложения</returns>
        public ApplicationController Register<T, U>() where U : class, T
        {
            _container.RegisterType<T, U>();
            return this;
        }

        /// <summary>
        /// Регистрация зависимости представителя с созданием
        /// его экземпляра и вызовом метода запуска
        /// </summary>
        /// <typeparam name="T">Тип представителя</typeparam>
        public void Run<T>() where T : class, IPresenter
        {
            if (!_container.IsRegistered<T>())
                _container.RegisterType<T>();
            _uiDispatcher.Invoke(() => _container.Resolve<T>().Run());
        }

        /// <summary>
        /// Регистрация зависимости параметризованного представителя
        /// с созданием  его экземпляра и вызовом процедуры его запуска
        /// </summary>
        /// <typeparam name="T">Тип представителя</typeparam>
        /// <typeparam name="U">Тип параметра представителя</typeparam>
        /// <param name="parameter">Параметр представителя</param>
        public void Run<T, U>(U parameter) where T : class, IPresenter<U>
        {
            if (!_container.IsRegistered<T>())
                _container.RegisterType<T>();
            _uiDispatcher.Invoke(() => _container.Resolve<T>().Run(parameter));
        }
    }
}