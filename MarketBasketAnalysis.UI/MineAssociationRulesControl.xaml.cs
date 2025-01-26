using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using MarketBasketAnalysis.Presentation.Views;
using MarketBasketAnalysis.Presentation.Views.EventArguments;

namespace MarketBasketAnalysis.UI
{
    /// <summary>
    /// Элемент управления задания параметров поиска ассоциативных правил
    /// </summary>
    public partial class MineAssociationRulesControl : ContentControl, IMineAssociationRulesView
    {
        /// <summary>
        /// Инициалиазация элемента управления
        /// </summary>
        public MineAssociationRulesControl() =>
            InitializeComponent();

        #region IMineAssociationRulesView

        /// <summary>
        /// Событие запроса поиска ассоциативных правил
        /// </summary>
        public event EventHandler<MiningArgs> AssociationRuleMiningRequested;

        /// <summary>
        /// Событие запроса загрузки ассоциативных правил
        /// </summary>
        public event EventHandler<string> AssociationRuleLoadingRequested;

        /// <summary>
        /// Показ прогресса поиска правил
        /// </summary>
        public void ShowMiningProgress() =>
            Dispatcher.Invoke(() =>
            {
                AssociationRuleStackPanel.IsEnabled = false;
                MiningProgressBar.Value = 0;
                MiningProgressStackPanel.Visibility = Visibility.Visible;
            });

        /// <summary>
        /// Скрытие прогресса поиска правил
        /// </summary>
        public void HideMiningProgress() =>
            Dispatcher.Invoke(() =>
            {
                AssociationRuleStackPanel.IsEnabled = true;
                MiningProgressStackPanel.Visibility = Visibility.Collapsed;
            });

        /// <summary>
        /// Обновление прогресса поиска правил
        /// </summary>
        /// <param name="progress">Новое значение прогресса</param>
        public void UpdateMiningProgress(double progress) =>
            Dispatcher.Invoke(() => MiningProgressBar.Value = progress);

        #endregion

        /// <summary>
        /// Обработчик нажатия кнопки открытия окна обзора файлов для
        /// выбора файла с транзакциями
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void OpenTransactionFileClick(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Табличные данные (*.csv)|*.csv|Все файлы (*.*)|*.*";
            if (dialog.ShowDialog() == true)
                TransactionFilePathTextBox.Text = dialog.FileName;
        }

        /// <summary>
        /// Обработчик нажатия кнопки поиска ассоциативных правил
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void MineRulesClick(object sender, RoutedEventArgs e) =>
            AssociationRuleMiningRequested?.BeginInvoke
            (
                sender: this,
                e: new MiningArgs
                (
                    transactionFilePath: TransactionFilePathTextBox.Text,
                    support: SupportNumericUpDown.Value ?? 0,
                    confidence: ConfidenceNumericUpDown.Value ?? 0,
                    applyItemDeletionRules: RemoveUninformativeItemsCheckBox.IsChecked == true,
                    applyItemReplacementRules: ReplaceArticlesWithGroupsCheckBox.IsChecked == true
                ),
                callback: null,
                @object: null
           );

        /// <summary>
        /// Обработчик нажатия кнопки загрузки ассоциативных правил из файла
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void LoadRulesClick(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Табличные данные (*.csv)|*.csv|Все файлы|*.*";
            if (dialog.ShowDialog() == true)
            {
                var animation = new WaitingAnimationControl("Загрузка правил", this);
                animation.StartAnimation();
                AssociationRuleLoadingRequested?.BeginInvoke(this, dialog.FileName, (_) => animation.StopAnimation(), null);
            }
        }
    }
}