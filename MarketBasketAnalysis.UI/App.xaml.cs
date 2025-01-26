using System.Windows;
using MarketBasketAnalysis.Services;
using MarketBasketAnalysis.DomainModel.AssociationRules.Mining;
using MarketBasketAnalysis.Presentation.Common;
using MarketBasketAnalysis.Presentation.Presenters;
using MarketBasketAnalysis.Presentation.Views;
using MarketBasketAnalysis.Infrastructure;

namespace MarketBasketAnalysis.UI
{
    /// <summary>
    /// Класс приложения, с которого начинается выполнение программы
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Запуск приложения: внедрение зависимостей, показ главного окна
        /// </summary>
        public App()
        {
            RepositoryCreator.LoadData();
            new ApplicationController(Dispatcher)
                .RegisterSingleton<ITransactionReader, TransactionReader>()
                .RegisterSingleton<ISerializeStream, SerializeStream>()
                .RegisterSingleton<IRepositoryCreator, RepositoryCreator>()
                .Register<Miner>()
                .RegisterSingleton<INavigator, MainWindow>()
                .RegisterSingleton<IMessageBox, MainWindow>()
                .RegisterSingleton<IMainView, MainWindow>()
                .Register<IMineAssociationRulesView, MineAssociationRulesControl>()
                .Register<IShowAssociationRulesView, ShowAssociationRulesControl>()
                .Register<IOperationsView, OperationsControl>()
                .Register<IShowMaximalSubgraphsView, ShowMaximalSubgraphsControl>()
                .Register<ISettingsView, SettingsControl>()
                .Run<MainPresenter>();
        }
    }
}