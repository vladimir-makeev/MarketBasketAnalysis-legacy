using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using MarketBasketAnalysis.Presentation.Views;
using MarketBasketAnalysis.Presentation.Views.EventArguments;

namespace MarketBasketAnalysis.UI
{
    /// <summary>
    /// Элемент управления выполнения операций над списками ассоциативных правил
    /// </summary>
    public partial class OperationsControl : ContentControl, IOperationsView
    {
        /// <summary>
        /// Инициализация элемента управления
        /// </summary>
        public OperationsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Событие запроса запуска операции
        /// </summary>
        public event EventHandler<OperationArgs> OperationExecutionRequested;

        /// <summary>
        /// Обработчик нажатия кнопки указания пути к файлу
        /// с первым списком ассоциативных правил
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void OpenFirstFileClick(object sender, RoutedEventArgs e) =>
            OpenFile(FirstFilePathTextBox);

        /// <summary>
        /// Обработчик нажатия кнопки указания пути к файлу
        /// со вторым списком ассоциативных правил
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void OpenSecondFileClick(object sender, RoutedEventArgs e) =>
            OpenFile(SecondFilePathTextBox);

        /// <summary>
        /// Открытие окна с обзором файлов для выбора файла с ассоциативными правилами
        /// </summary>
        /// <param name="textBox">
        /// Элемент управления редактирования текста, куда будет записан путь к выбранному файлу
        /// </param>
        private void OpenFile(TextBox textBox)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Табличные данные (*.csv)|*.csv|Все файлы|*.*";
            if (dialog.ShowDialog() == true)
                textBox.Text = dialog.FileName;
        }

        /// <summary>
        /// Обработчик нажатия кнопки запроса выполнения операции
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void ExecuteOperationClick(object sender, RoutedEventArgs e)
        {
            var animation = new WaitingAnimationControl("Выполнение операции", this);
            OperationExecutionRequested?.BeginInvoke
            (
                sender: this,
                e: new OperationArgs
                (
                    firstAssociationRulesFielPath: FirstFilePathTextBox.Text,
                    secondAssociationRulesFilePath: SecondFilePathTextBox.Text,
                    operationType: IntersectionCheckBox.IsChecked == true ?
                        OperationType.Intersection :
                        OperationType.SymmetricDifference,
                    considerDirection: ConsiderDirectionCheckBox.IsChecked == true
                ),
                callback: (_) => animation.StopAnimation(),
                @object: null
            );
        }
    }
}
