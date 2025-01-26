using System;
using MarketBasketAnalysis.Presentation.Common;

namespace MarketBasketAnalysis.Presentation.Views
{
    /// <summary>
    /// Описание представления выбора:
    /// - представления поиска ассоциативных правил
    /// - представления выполнения операций над списками ассоциативных правил;
    /// - представления настроек приложения
    /// </summary>
    public interface IMainView : IView
    {
        /// <summary>
        /// Событие выбора представления поиска ассоциативных правил
        /// </summary>
        event EventHandler MineAssociationRulesViewSelected;

        /// <summary>
        /// Событие выбора представления выполнения операций над списками ассоциативных правил
        /// </summary>
        event EventHandler OperationsViewSelected;

        /// <summary>
        /// Событие выбора представления настроек приложения
        /// </summary>
        event EventHandler SettingsViewSelected;
    }
}