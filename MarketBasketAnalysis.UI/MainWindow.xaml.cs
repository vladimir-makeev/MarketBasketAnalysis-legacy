using System;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Command;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MarketBasketAnalysis.Presentation.Common;
using MarketBasketAnalysis.Presentation.Views;

namespace MarketBasketAnalysis.UI
{
    /// <summary>
    /// Главное окно приложения
    /// </summary>
    public partial class MainWindow : MetroWindow, IMainView, INavigator, IMessageBox
    {
        /// <summary>
        /// Комнада нажатия кнопки перехода к поиску ассоциативных правил
        /// </summary>
        public RelayCommand MineAssociationRulesViewCommand { get; }

        /// <summary>
        /// Команда нажатия кнопки перехода к выполнению операции над списками ассоциативных правил
        /// </summary>
        public RelayCommand OperationsViewCommand { get; }

        /// <summary>
        /// Команда нажатия кнопки перехода к настройкам приложения
        /// </summary>
        public RelayCommand SettingsViewCommand { get; }

        /// <summary>
        /// Инициализация окна
        /// </summary>
        public MainWindow()
        {
            MineAssociationRulesViewCommand = new RelayCommand(() =>
                MineAssociationRulesViewSelected?.BeginInvoke(this, EventArgs.Empty, null, null));
            OperationsViewCommand = new RelayCommand(() =>
                OperationsViewSelected?.BeginInvoke(this, EventArgs.Empty, null, null));
            SettingsViewCommand = new RelayCommand(() =>
                SettingsViewSelected?.BeginInvoke(this, EventArgs.Empty, null, null));
            DataContext = this;
            InitializeComponent();
        }

        #region IMainView

        /// <summary>
        /// Событие выбора  представления поиска ассоциативных правил
        /// </summary>
        public event EventHandler MineAssociationRulesViewSelected;

        /// <summary>
        /// Событие выбора представления выполнения операций над списками ассоциативных правил
        /// </summary>
        public event EventHandler OperationsViewSelected;

        /// <summary>
        /// Событие выбора представления настроек приложения
        /// </summary>
        public event EventHandler SettingsViewSelected;

        #endregion

        #region INavigator

        /// <summary>
        /// Событие закрытия представления
        /// Закрытие выполняется программно или пользователем
        /// </summary>
        public event EventHandler<IView> ViewClosed;

        /// <summary>
        /// Показ представления
        /// </summary>
        /// <param name="view">Представление</param>
        public void ShowView(IView view) =>
            Dispatcher.Invoke(() =>
            {
                if (view == this)
                {
                    this.Show();
                    this.Closed += (sender, args) =>
                        ViewClosed?.Invoke(this, this);
                }
                else
                {
                    var tabItem = new MetroTabItem
                    {
                        Content = view,
                        Header = $"{((ContentControl)view).Tag} ({DateTime.Now.ToString("T")})",
                        CloseButtonEnabled = true,
                    };
                    tabItem.CloseTabCommand = new RelayCommand(() =>
                        ViewClosed?.Invoke(this, view), true);
                    ControlsHelper.SetHeaderFontSize(tabItem, 16);
                    MainTabControl.Items.Add(tabItem);
                    MainTabControl.SelectedItem = tabItem;
                }
            });

        /// <summary>
        /// Закрытие представления
        /// </summary>
        /// <param name="view">Представление</param>
        public void CloseView(IView view) =>
            Dispatcher.Invoke(() =>
            {
                if (view == this)
                    this.Close();
                else
                    Dispatcher.Invoke(() => MainTabControl.Items.Remove(view));
            });

        #endregion

        #region IMessageBox

        /// <summary>
        /// Показ сообщения
        /// </summary>
        /// <param name="title">Заголовок сообщения</param>
        /// <param name="message">Текст сообщения</param>
        public void ShowMessage(string title, string message) =>
            Dispatcher.Invoke(() => this.ShowMessageAsync(title, message));

        #endregion
    }
}