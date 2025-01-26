using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using GalaSoft.MvvmLight.Command;
using MarketBasketAnalysis.DomainModel.AssociationRules.Mining;
using MarketBasketAnalysis.Presentation.Views;
using MarketBasketAnalysis.Presentation.Views.EventArguments;

namespace MarketBasketAnalysis.UI
{
    /// <summary>
    /// Элемент управления редактирования настроек приложения
    /// </summary>
    public partial class SettingsControl : ContentControl, ISettingsView
    {
        /// <summary>
        /// Команда удаления правила удаления элементов транзакций
        /// </summary>
        public RelayCommand<object> RemoveItemDeletionRuleCommand { get; }

        /// <summary>
        /// Команда удаления правила замены СКЮ группами товаров
        /// </summary>
        public RelayCommand<object> RemoveItemReplacementRuleCommand { get; }

        /// <summary>
        /// Инициализация элемента управления
        /// </summary>
        public SettingsControl()
        {
            RemoveItemDeletionRuleCommand = new RelayCommand<object>(item =>
                ItemDeletionRulesDataGrid.Items.Remove(item));
            RemoveItemReplacementRuleCommand = new RelayCommand<object>(item =>
                ItemReplacementRulesDataGrid.Items.Remove(item));
            DataContext = this;
            InitializeComponent();
        }

        #region ISettingsView

        /// <summary>
        /// События запроса сохранения отредактированных настроек
        /// </summary>
        public event EventHandler<SaveSettingsArgs> SettingsSaveRequested;

        /// <summary>
        /// Показ правил удаления элементов транзакций
        /// </summary>
        /// <param name="rules">Правила удаления элементов транзакций</param>
        public void ShowItemDeletionRules(IReadOnlyList<ItemDeletionRule> rules) =>
            Dispatcher.Invoke(() =>
            {
                ItemDeletionRulesDataGrid.Items.Clear();
                rules.ToList().ForEach(rule =>
                    ItemDeletionRulesDataGrid.Items.Add((rule.Item, rule.ExactMatch).ToTuple()));
            });

        /// <summary>
        /// Показ правил замены СКЮ на группы товаров
        /// </summary>
        /// <param name="rules">Правила замены СКЮ группами товаров</param>
        public void ShowItemReplacementRules(IReadOnlyList<ItemReplacementRule> rules) =>
            Dispatcher.Invoke(() =>
            {
                ItemReplacementRulesDataGrid.Items.Clear();
                rules.ToList().ForEach(rule =>
                    ItemReplacementRulesDataGrid.Items.Add
                    (
                        newItem: (rule.ArticleGroup, rule.Scope).ToTuple())
                    );
            });

        /// <summary>
        /// Показ групп товаров
        /// </summary>
        /// <param name="groups">Группы товаров</param>
        public void ShowArticleGroups(IReadOnlyList<string> groups)
        {
            ArticleGroupComboBox.Items.Clear();
            groups.ToList().ForEach(group =>
                ArticleGroupComboBox.Items.Add(group));
        }

        #endregion

        /// <summary>
        /// Обработчик нажатия кнопки добавления правила удаления элементов транзакций
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void AddItemDeletionRuleClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ItemTextBox.Text))
            {
                ItemDeletionRulesDataGrid.Items.Add
                (
                    newItem: (ItemTextBox.Text, ExactMatchCheckBox.IsChecked == true).ToTuple()
                );
                ItemTextBox.Clear();
                ExactMatchCheckBox.IsChecked = false;
            }
        }

        /// <summary>
        /// Обработчик события добавления правила замены СКЮ группами товаров
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void AddItemReplacementRuleClick(object sender, RoutedEventArgs e)
        {
            var articleGroup = (string)ArticleGroupComboBox.SelectedItem;
            var scope = (ReplacementScope)ReplacementScopeComboBox.SelectedItem;
            if (!string.IsNullOrWhiteSpace(articleGroup))
            {
                ItemReplacementRulesDataGrid.Items.Add((articleGroup, scope).ToTuple());
                ArticleGroupComboBox.SelectedIndex = -1;
                ReplacementScopeComboBox.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Обработчик нажатия кнопки сохранения настроек приложения
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void SaveSettingsClick(object sender, RoutedEventArgs e)
        {
            var animation = new WaitingAnimationControl("Сохранение настроек", this);
            animation.StartAnimation();
            SettingsSaveRequested?.BeginInvoke
            (
                sender: this,
                e: new SaveSettingsArgs
                (
                    itemDeletionRules: ItemDeletionRulesDataGrid.Items.Cast<Tuple<string, bool>>()
                        .ToList(),
                    itemReplacementRules: ItemReplacementRulesDataGrid.Items.Cast<Tuple<string, ReplacementScope>>()
                        .ToList()
                ),
                callback: (_) => animation.StopAnimation(),
                @object: null
            );
        }
    }
}