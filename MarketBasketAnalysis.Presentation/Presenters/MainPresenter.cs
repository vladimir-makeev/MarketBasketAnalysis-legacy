using System;
using MarketBasketAnalysis.Presentation.Common;
using MarketBasketAnalysis.Presentation.Views;

namespace MarketBasketAnalysis.Presentation.Presenters
{
    /// <summary>
    /// Представитель c реализацией перехода между представлениями
    /// поиска ассоциативных правил, настроек
    /// </summary>
    public class MainPresenter : BasePresenter<IMainView>
    {
        /// <summary>
        /// Создание представителя
        /// </summary>
        /// <param name="controller">Контроллер приложения</param>
        /// <param name="navigator">Средство показа, закрытия представлений</param>
        /// <param name="view">Представление</param>
        public MainPresenter(ApplicationController controller, INavigator navigator, IMainView view)
            : base(controller, navigator, view)
        {
            view.MineAssociationRulesViewSelected += ShowMineAssociationRulesView;
            view.OperationsViewSelected += ShowOperationsView;
            view.SettingsViewSelected += ShowSettingsView;
        }

        /// <summary>
        /// Обработчик выбора представления поиска ассоциативных правил
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void ShowMineAssociationRulesView(object sender, EventArgs e) =>
            controller.Run<MineAssociationRulesPresenter>();

        /// <summary>
        /// Обработчик выбора представления настроек приложения
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void ShowSettingsView(object sender, EventArgs e) =>
            controller.Run<SettingsPresenter>();

        /// <summary>
        /// Показ представления с операциями над ассоциативными правилами
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void ShowOperationsView(object sender, EventArgs e) =>
            controller.Run<OperationsPresenter>();
    }
}