using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using CodeContracts;
using QuickGraph;
using MarketBasketAnalysis.Services;
using MarketBasketAnalysis.Presentation.Common;
using MarketBasketAnalysis.DomainModel.AssociationRules;
using MarketBasketAnalysis.DomainModel.AssociationRules.Graph;
using MarketBasketAnalysis.Presentation.Views;
using MarketBasketAnalysis.Presentation.Views.EventArguments;

namespace MarketBasketAnalysis.Presentation.Presenters
{
    /// <summary>
    /// Представитель, взаимодействующий с представлением отображения ассоциативных правил
    /// </summary>
    public class ShowAssociationRulesPresenter : BasePresenter<IShowAssociationRulesView, IReadOnlyList<AssociationRule>>
    {
        /// <summary>
        /// Средство показа сообщений
        /// </summary>
        private readonly IMessageBox _messageBox;

        /// <summary>
        /// Средство записи объектов в файлы, чтения их из файлов
        /// </summary>
        private readonly ISerializeStream _serializeStream;

        /// <summary>
        /// Отфильтрованные ассоциативные правила
        /// </summary>
        private IReadOnlyList<AssociationRule> _filteredRules;

        /// <summary>
        /// Создание представителя
        /// </summary>
        /// <param name="controller">Контроллер приложения</param>
        /// <param name="navigator">Средство показа, закрытия представлений</param>
        /// <param name="view">Представление показа ассоциативных правил</param>
        /// <param name="messageBox">Средство показа сообщений</param>
        /// <param name="serializeStream">Средство записи объектов в файлы, чтения их из файлов</param>
        public ShowAssociationRulesPresenter
        (
            ApplicationController controller,
            INavigator navigator,
            IShowAssociationRulesView view,
            IMessageBox messageBox,
            ISerializeStream serializeStream
        ) : base(controller, navigator, view)
        {
            Requires.NotNull(messageBox, nameof(messageBox));
            Requires.NotNull(serializeStream, nameof(serializeStream));

            _messageBox = messageBox;
            _serializeStream = serializeStream;

            view.FilterResetRequested += ResetFilter;
            view.SaveRequested += SaveAssociationRules;
            view.FilterRequested += FilterAssociationRules;
            view.MaximalSubgraphsFindingRequested += FindMaximalCliques;
        }

        /// <summary>
        /// Обработчик запроса сброса фильтров ассоциативных правил
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void ResetFilter(object sender, EventArgs e)
        {
            _filteredRules = parameter;
            view.ShowAssociationRules(parameter, new Graph(parameter));
        }

        /// <summary>
        /// Обработчик запроса сохранения отфильтрованных ассоциативных правил
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="path">Путь к файлу для сохранения ассоциативных правил</param>
        private void SaveAssociationRules(object sender, string path)
        {
            Requires.NotNull(path, nameof(path));

            if (_filteredRules.Count == 0)
                _messageBox.ShowMessage("Ошибка", "Отсутствуют ассоциативные правила, сохранение невозможно");
            else if (string.IsNullOrEmpty(path))
                _messageBox.ShowMessage("Ошибка ввода", "Путь к файлу сохранения не может быть пустым");
            else
            {
                try
                {
                    _serializeStream.Write(path, _filteredRules);
                    _messageBox.ShowMessage("Сохранение правил", $"Ассоциативные правила сохранены в файл {path}");
                }
                catch (IOException)
                {
                    _messageBox.ShowMessage("Сохранение правил", $"Возникла ошибка при записи данных в файл {path}");
                }
                catch
                {;
                    _messageBox.ShowMessage("Сохранение правил", "Во время сохранения данных произошла ошибка");
                }
            }
        }

        /// <summary>
        /// Обработчик запроса фильтрации ассоциативных правил
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="options">Значения фильтров, заданные пользователем</param>
        private void FilterAssociationRules(object sender, FilteringArgs options)
        {
            Requires.NotNull(options, nameof(options));

            bool IsIntervalIncorrect(Interval<double> interval, double maxThreshold = 1) =>
                !(interval.Min >= 0 && interval.Max <= maxThreshold && interval.Min < interval.Max);

            if (IsIntervalIncorrect(options.Support))
                _messageBox.ShowMessage("Ошибка ввода", "Неверно заданы границы поддержки");
            else if (IsIntervalIncorrect(options.Confidence))
                _messageBox.ShowMessage("Ошибка ввода", "Неверно заданы границы достоверности");
            else if (IsIntervalIncorrect(options.Lift, double.PositiveInfinity))
                _messageBox.ShowMessage("Ошибка ввода", "Неверно заданы границы меры интереса");
            else if (IsIntervalIncorrect(options.Conviction, double.PositiveInfinity))
                _messageBox.ShowMessage("Ошибка ввода", "Неверно заданы границы меры уверенности");
            else if (IsIntervalIncorrect(options.AbsoluteAssociationCoef))
                _messageBox.ShowMessage("Ошибка ввода", "Неверно заданы границы коэффициента ассоциации");
            else if (IsIntervalIncorrect(options.AbsoluteContingencyCoef))
                _messageBox.ShowMessage("Ошибка ввода", "Неверно заданы границы коэффициента контингенции");
            else
            {
                _filteredRules = parameter
                    .AsParallel()
                    .Where(rule =>
                        rule.Support >= options.Support.Min &&
                        rule.Support <= options.Support.Max &&
                        rule.Confidence >= options.Confidence.Min &&
                        rule.Confidence <= options.Confidence.Max &&
                        rule.Lift >= options.Lift.Min &&
                        rule.Lift <= options.Lift.Max &&
                        rule.Conviction >= options.Conviction.Min &&
                        rule.Conviction <= options.Conviction.Max &&
                        rule.AbsoluteAssociationCoef >= options.AbsoluteAssociationCoef.Min &&
                        rule.AbsoluteAssociationCoef <= options.AbsoluteAssociationCoef.Max &&
                        rule.AbsoluteContingencyCoef >= options.AbsoluteContingencyCoef.Min &&
                        rule.AbsoluteAssociationCoef <= options.AbsoluteContingencyCoef.Max)
                    .ToList();
                if (options.RunTestOfIndependence)
                    _filteredRules = _filteredRules.Where(rule => !rule.AreHandSidesIndependent)
                        .ToList();
            }

            if (!string.IsNullOrWhiteSpace(options.SearchString))
            {
                bool IsMatch(AssociationRule rule)
                {
                    var lhs = rule.LeftHandSide;
                    var rhs = rule.RightHandSide;
                    var searchString = options.SearchString;

                    if (!options.CaseSensitive)
                    {
                        lhs = lhs.ToLower();
                        rhs = rhs.ToLower();
                        searchString = searchString.ToLower();
                    }

                    if (options.ExactMatch)
                        return lhs == searchString || rhs == searchString;
                    return
                        lhs.Contains(searchString) || rhs.Contains(searchString);
                }

                _filteredRules = _filteredRules.Where(IsMatch).ToArray();
            }

            view.ShowAssociationRules(_filteredRules, new Graph(_filteredRules));
        }

        /// <summary>
        /// Обработчик запроса поиска максимальных клик в ассоциативных правилах
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="options">Параметры поиска клик, заданные пользователем</param>
        private void FindMaximalCliques(object sender, CliquesFindingArgs options)
        {
            Requires.NotNull(options, nameof(options));

            if (options.CliqueSize.Min <= 1 || options.CliqueSize.Max <= 1 || options.CliqueSize.Min > options.CliqueSize.Max)
                _messageBox.ShowMessage("Ошибка ввода", "Неверно заданы границы интервала размера клик");
            else
            {
                var cliques = new Graph(_filteredRules)
                    .FindMaxCliques(options.CliqueSize.Min, options.CliqueSize.Max);
                if (cliques.Count == 0)
                    _messageBox.ShowMessage("Поиск максимальных клик", "Не найдено ни одной клики заданных размеров");
                else
                    controller.Run<ShowMaximalSubgraphsPresenter, IReadOnlyList<BidirectionalGraph<Vertex, Edge>>>(cliques);
            }
        }

        /// <summary>
        /// Оотображение ассоциативных правил в представлении и его показ
        /// </summary>
        /// <param name="rules">Ассоциативные правила</param>
        public override void Run(IReadOnlyList<AssociationRule> rules)
        {
            Requires.NotNull(rules, nameof(rules));

            _filteredRules = rules;
            view.ShowAssociationRules(rules, new Graph(rules));
            base.Run(rules);
        }
    }
}