using System.IO;
using System.Linq;
using System.Collections.Generic;
using CodeContracts;
using MarketBasketAnalysis.Services;
using MarketBasketAnalysis.DomainModel.AssociationRules;
using MarketBasketAnalysis.DomainModel.AssociationRules.Operations;
using MarketBasketAnalysis.Presentation.Common;
using MarketBasketAnalysis.Presentation.Views;
using MarketBasketAnalysis.Presentation.Views.EventArguments;

namespace MarketBasketAnalysis.Presentation.Presenters
{
    /// <summary>
    /// Представитель, взаимодействующий с представлением
    /// выполнения операций над списками ассоциативных правил
    /// </summary>
    public class OperationsPresenter : BasePresenter<IOperationsView>
    {
        /// <summary>
        /// Средство показа сообщений
        /// </summary>
        private readonly IMessageBox _messageBox;

        /// <summary>
        /// Средство записи объектов в файлы, их чтения из файлов
        /// </summary>
        private readonly ISerializeStream _serializeStream;

        /// <summary>
        /// Исполнитель операций над списками ассоциативных правил
        /// </summary>
        private readonly Operations _operations;

        /// <summary>
        /// Создание представителя
        /// </summary>
        /// <param name="controller">Контроллер приложения</param>
        /// <param name="navigator">Средство показа, закрытия представлений</param>
        /// <param name="view">Представление выполнения операций над списками ассоциативных правил</param>
        /// <param name="messageBox">Средство показа сообщений</param>
        /// <param name="serializeStream">Средство записи объектов в файлы, чтения их из файлов</param>
        /// <param name="operations">Исполнитель операций над списками ассоциативных правил</param>
        public OperationsPresenter
            (ApplicationController controller,
            INavigator navigator,
            IOperationsView view,
            IMessageBox messageBox,
            ISerializeStream serializeStream,
            Operations operations)
            : base(controller, navigator, view)
        {
            Requires.NotNull(messageBox, nameof(messageBox));
            Requires.NotNull(operations, nameof(operations));
            Requires.NotNull(serializeStream, nameof(serializeStream));

            _messageBox = messageBox;
            _serializeStream = serializeStream;
            _operations = operations;

            view.OperationExecutionRequested += ExecuteOperation;
        }

        /// <summary>
        /// Обработчик запроса выполнения операции:
        /// 1) осуществляет загрузку правил из указанных файлов,
        /// 2) производит над ними заданную пользователем операцию
        /// 3) отображает результат операции, вызывая представителей
        /// для показа ассоциативных каждого результирующего списка
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="options">Параметры операции, заданные пользователем</param>
        private void ExecuteOperation(object sender, OperationArgs options)
        {
            Requires.NotNull(options, nameof(options));

            if (string.IsNullOrWhiteSpace(options.FirstAssociationRulesFilePath))
                _messageBox.ShowMessage("Ошибка ввода", "Не задан путь к первому файлу с ассоциативными правилами");
            else if (string.IsNullOrWhiteSpace(options.SecondAssociaitonRulesFilePath))
                _messageBox.ShowMessage("Ошибка ввода", "Не задан путь ко второму файлу с ассоциативными правилами");
            else if (!File.Exists(options.FirstAssociationRulesFilePath))
                _messageBox.ShowMessage("Ошибка ввода", "Не удалось найти первый файл по заданному пути");
            else if (!File.Exists(options.SecondAssociaitonRulesFilePath))
                _messageBox.ShowMessage("Ошибка ввода", "Не удалось найти второй файл по указанному пути");
            else
            {
                List<AssociationRule> firstRuleList = null;
                List<AssociationRule> secondRuleList = null;

                try
                {
                    firstRuleList = _serializeStream.Read<AssociationRule>(options.FirstAssociationRulesFilePath);
                    secondRuleList = _serializeStream.Read<AssociationRule>(options.SecondAssociaitonRulesFilePath);
                }
                catch
                {
                    _messageBox.ShowMessage("Операции над правилами", "В ходе чтения данных произошла ошибка");
                    return;
                }

                if (firstRuleList.Count == 0 || secondRuleList.Count == 0)
                {
                    _messageBox.ShowMessage("Операции над правилами", "Один или более из указанных файлов не содержат ассоциативных правил");
                    return;
                }

                OperationResult result = null;

                if (options.OperationType == OperationType.Intersection)
                    result = _operations.Intersection(firstRuleList, secondRuleList, options.ConsiderDirection);
                else
                    result = _operations.SymmetricDifference(firstRuleList, secondRuleList, options.ConsiderDirection);

                if (result.FirstResultSet.Count == 0 && result.SecondResultSet.Count == 0)
                {
                    _messageBox.ShowMessage("Операции над правилами", "Результат операции не содержит ассоциативных правил");
                    return;
                }

                controller.Run<ShowAssociationRulesPresenter, IReadOnlyList<AssociationRule>>(result.FirstResultSet.ToList());
                controller.Run<ShowAssociationRulesPresenter, IReadOnlyList<AssociationRule>>(result.SecondResultSet.ToList());
            }
        }
    }
}