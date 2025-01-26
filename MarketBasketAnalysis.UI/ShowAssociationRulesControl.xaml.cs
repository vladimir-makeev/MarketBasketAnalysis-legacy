using System;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using QuickGraph;
using MarketBasketAnalysis.DomainModel.AssociationRules;
using MarketBasketAnalysis.DomainModel.AssociationRules.Graph;
using MarketBasketAnalysis.Presentation.Views;
using MarketBasketAnalysis.Presentation.Views.EventArguments;

namespace MarketBasketAnalysis.UI
{
    /// <summary>
    /// Элемент управления показа ассоциативных правил
    /// </summary>
    public partial class ShowAssociationRulesControl : ContentControl, IShowAssociationRulesView
    {
        /// <summary>
        /// Граф ассоциативных правил
        /// </summary>
        private IBidirectionalGraph<Vertex, Edge> _graph;

        /// <summary>
        /// Инициализация элемента управления
        /// </summary>
        public ShowAssociationRulesControl()
        {
            InitializeComponent();
        }

        #region IShowAssociationRulesView

        /// <summary>
        /// Событие запроса фильтрации ассоциативных правил
        /// </summary>
        public event EventHandler<FilteringArgs> FilterRequested;

        /// <summary>
        /// Событие запроса сброса примененных фильтров
        /// </summary>
        public event EventHandler FilterResetRequested;

        /// <summary>
        /// Событие запроса поиска максимальных подграфов графа ассоциативных правил
        /// </summary>
        public event EventHandler<CliquesFindingArgs> MaximalSubgraphsFindingRequested;

        /// <summary>
        /// Событие запроса сохранения ассоциативных правил
        /// </summary>
        public event EventHandler<string> SaveRequested;

        /// <summary>
        /// Показ ассоциативных правил
        /// </summary>
        /// <param name="rules">Ассоциативные правила</param>
        /// <param name="graph">Граф ассоциативных правил</param>
        public void ShowAssociationRules(IReadOnlyList<AssociationRule> rules, IBidirectionalGraph<Vertex, Edge> graph)
        {
            Dispatcher.Invoke(() =>
            {
                AssociationRulesDataGrid.ItemsSource = rules;
                _graph = graph;
                if (AssociationRulesTabControl.SelectedItem == AssociationRuleGraphTabItem)
                    AssociationRuleGraphLayout.Graph = graph;
                AssociationRulesCountLabel.Content = $"Количество ассоциативных правил: {rules.Count}";
            });
        }
        #endregion

        /// <summary>
        /// Обработка нажатия клавиши
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var animation = new WaitingAnimationControl("Применение фильтров", this);
                animation.StartAnimation();
                FilterRequested?.BeginInvoke(this, GetFilteringOptions(), (_) => animation.StopAnimation(), null);
            };
        }

        /// <summary>
        /// Обработчик нажатия кнопки применения фильтрации
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void ApplyFiltersClick(object sender, RoutedEventArgs e)
        {
            var animation = new WaitingAnimationControl("Применение фильтров", this);
            animation.StartAnimation();
            FilterRequested?.BeginInvoke(this, GetFilteringOptions(), (_) => animation.StopAnimation(), null);
        }

        /// <summary>
        /// Обработчик нажатия кнопки сброса примененных фильтров
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void ResetFiltersClick(object sender, RoutedEventArgs e)
        {
            SearchStringTextBox.Text = string.Empty;
            ExactMatchCheckBox.IsChecked = false;
            CaseSensitiveCheckBox.IsChecked = false;
            MinSupportNumericUpDown.Value = 0;
            MaxSupportNumericUpDown.Value = 1;
            MinConfidenceNumericUpDown.Value = 0;
            MaxConfidenceNumericUpDown.Value = 1;
            MinLiftNumericUpDown.Value = 0;
            MaxLiftNumericUpDown.Value = 1e4;
            MinConvictionNumericUpDown.Value = 0;
            MaxConvictionNumericUpDown.Value = 1e4;
            MinAbsoluteAssociationCoefNumericUpDown.Value = 0;
            MaxAbsoluteAssociationCoefNumericUpDown.Value = 1;
            MinAbsoluteContingencyCoefNumericUpDown.Value = 0;
            MaxAbsoluteContingencyCoefNumericUpDown.Value = 1;
            var animation = new WaitingAnimationControl("Сброс фильтров", this);
            animation.StartAnimation();
            FilterResetRequested?.BeginInvoke(this, EventArgs.Empty, (_) => animation.StopAnimation(), null);
        }

        /// <summary>
        /// Обработчик нажатия кнопки сохранения ассоциативных правил
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void SaveRulesClick(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog()
            {
                AddExtension = true,
                Filter = "Табличные данные (*.csv)|*.csv|Все файлы (*.*)|*.*"
            };
            if (dialog.ShowDialog() == true)
            {
                var animation = new WaitingAnimationControl("Сохранение правил", this);
                animation.StartAnimation();
                SaveRequested?.BeginInvoke(this, dialog.FileName, (_) => animation.StopAnimation(), null);
            }
        }

        /// <summary>
        /// Генерация параметров фильтрации на основе значений элементов пользовательского интерфейса
        /// </summary>
        /// <returns>Параметры фильтрации</returns>
        private FilteringArgs GetFilteringOptions() =>
            new FilteringArgs
            (
                searchString: SearchStringTextBox.Text,
                caseSensitive: CaseSensitiveCheckBox.IsChecked == true,
                exactMatch: ExactMatchCheckBox.IsChecked == true,
                support: new Interval<double>
                (
                    min: MinSupportNumericUpDown.Value ?? 0,
                    max: MaxSupportNumericUpDown.Value ?? 1
                ),
                confidence: new Interval<double>
                (
                    min: MinConfidenceNumericUpDown.Value ?? 0,
                    max: MaxConfidenceNumericUpDown.Value ?? 1
                ),
                lift: new Interval<double>
                (
                    min: MinLiftNumericUpDown.Value ?? 0,
                    max: MaxLiftNumericUpDown.Value ?? double.PositiveInfinity
                ),
                conviction: new Interval<double>
                (
                    min: MinConvictionNumericUpDown.Value ?? 0,
                    max: MaxConvictionNumericUpDown.Value ?? double.PositiveInfinity
                ),
                absoluteAssociationCoef: new Interval<double>
                (
                    min: MinAbsoluteAssociationCoefNumericUpDown.Value ?? 0,
                    max: MaxAbsoluteAssociationCoefNumericUpDown.Value ?? 1
                ),
                absoluteContingencyCoef: new Interval<double>
                (
                    min: MinAbsoluteContingencyCoefNumericUpDown.Value ?? 0,
                    max: MaxAbsoluteAssociationCoefNumericUpDown.Value ?? 1
                ),
                runTestOfIndependence: RunTestOfIndependenceCheckBox.IsChecked ?? false
            );

        /// <summary>
        /// Обработчик перехода ко вкладке с графом ассоциативных правил
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void TabItemSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AssociationRulesTabControl.SelectedItem == AssociationRuleGraphTabItem &&
                AssociationRuleGraphLayout.Graph != _graph)
                AssociationRuleGraphLayout.Graph = _graph;
        }

        /// <summary>
        /// Обработчик нажатия кнопки поиска максимальных подграфов графа ассоциативных правил
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void FindMaximalCliquesClick(object sender, RoutedEventArgs e)
        {
            var animation = new WaitingAnimationControl("Поиск максимальных подграфов", this);
            animation.StartAnimation();
            MaximalSubgraphsFindingRequested?.BeginInvoke
            (
                sender: this,
                e: new CliquesFindingArgs
                (
                    cliqueSize: new Interval<int>
                    (
                        min: (int)(MinCliqueSizeNumericUpDown.Value ?? 2),
                        max: (int)(MaxCliqueSizeNumericUpDown.Value ?? 10)
                    )
                ),
                callback: (_) => animation.StopAnimation(),
                @object: null
            );
        }
    }
}