using System;
using System.IO;
using System.Collections.Generic;
using CodeContracts;
using MarketBasketAnalysis.Services;
using MarketBasketAnalysis.Presentation.Common;
using MarketBasketAnalysis.Presentation.Views;
using MarketBasketAnalysis.DomainModel.AssociationRules;
using MarketBasketAnalysis.DomainModel.AssociationRules.Mining;
using MarketBasketAnalysis.Presentation.Views.EventArguments;
using MarketBasketAnalysis.DomainModel.ProductHierarchy;

namespace MarketBasketAnalysis.Presentation.Presenters
{
    /// <summary>
    /// Представитель, взаимодействующий с представлением поиска ассоциативных правил
    /// </summary>
    public class MineAssociationRulesPresenter : BasePresenter<IMineAssociationRulesView>
    {
        /// <summary>
        /// Средство показа сообщений
        /// </summary>
        private readonly IMessageBox _messageBox;

        /// <summary>
        /// Средство поиска ассоциативных правил
        /// </summary>
        private readonly Miner _miner;

        /// <summary>
        /// Средство создания репозиториев
        /// </summary>
        private readonly IRepositoryCreator _repositoryCreator;

        /// <summary>
        /// Средство записи объектов в файлы, их чтения из файлов
        /// </summary>
        private readonly ISerializeStream _serializeStream;

        /// <summary>
        /// Средство чтения транзакций
        /// </summary>
        private readonly ITransactionReader _transactionReader;

        /// <summary>
        /// Создание представителя
        /// </summary>
        /// <param name="controller">Контроллер приложения</param>
        /// <param name="navigator">Средство показа, закрытия представлений</param>
        /// <param name="view">Представление поиска ассоциативных правил</param>
        /// <param name="messageBox">Средство показа сообщений</param>
        /// <param name="miner">Средство поиска ассоциативных правил</param>
        /// <param name="serializeStream">Средство записи объектов в файлы, чтения их из файлов</param>
        /// <param name="repositoryCreator">Средство создания хранилищ объектов</param>
        /// <param name="transactionReader">Средство чтения транзакций</param>
        public MineAssociationRulesPresenter
        (
             ApplicationController controller,
             INavigator navigator,
             IMineAssociationRulesView view,
             IMessageBox messageBox,
             Miner miner,
             ISerializeStream serializeStream,
             IRepositoryCreator repositoryCreator,
             ITransactionReader transactionReader
        ) : base(controller, navigator, view)
        {
            Requires.NotNull(miner, nameof(miner));
            Requires.NotNull(messageBox, nameof(messageBox));
            Requires.NotNull(serializeStream, nameof(serializeStream));
            Requires.NotNull(repositoryCreator, nameof(repositoryCreator));
            Requires.NotNull(transactionReader, nameof(transactionReader));

            _miner = miner;
            _messageBox = messageBox;
            _serializeStream = serializeStream;
            _repositoryCreator = repositoryCreator;
            _transactionReader = transactionReader;

            navigator.ViewClosed += CancelMining;
            _miner.MiningProgressChanged += UpdateMiningProgress;
            view.AssociationRuleLoadingRequested += LoadAssociationRules;
            view.AssociationRuleMiningRequested += MineAssociationRules;
        }

        /// <summary>
        /// Обработчик закрытия представления, отменяющий поиск ассоцитивных правил
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="closedView">Закрытое представление</param>
        private void CancelMining(object sender, IView closedView)
        {
            Requires.NotNull(closedView, nameof(closedView));

            if (closedView == view)
                _miner.CancelMining();
        }

        /// <summary>
        /// Обработчик изменения прогресса поиска ассоциативных правил,
        /// обновляющий его значение в представлении
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="progress">Значение прогресса поиска правил</param>
        private void UpdateMiningProgress(object sender, double progress)
        {
            Requires.InRange(progress >= 0 && progress <= 100, nameof(progress));

            view.UpdateMiningProgress(progress);
        }

        /// <summary>
        /// Обработчик запроса загрузки ассоциативных правил из файла
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="path">Путь к файлу с ассоциативными правилами</param>
        private void LoadAssociationRules(object sender, string path)
        {
            Requires.NotNull(path, nameof(path));

            if (string.IsNullOrWhiteSpace(path))
                _messageBox.ShowMessage("Ошибка ввода", "Не задан путь к файлу с ассоциативными правилами");
            else if (!File.Exists(path))
                _messageBox.ShowMessage("Ошибка ввода", "Не удалось найти файл по заданному пути");
            else
            {
                List<AssociationRule> rules;
                try
                {
                    rules = _serializeStream.Read<AssociationRule>(path);
                }
                catch
                {
                    _messageBox.ShowMessage("Загрузка правил", "В ходе чтения данных произошла ошибка");
                    return;
                }
                if (rules.Count == 0)
                    _messageBox.ShowMessage("Загрузка правил", "Указанный файл не содержит ассоциативных правил");
                else
                    controller.Run<ShowAssociationRulesPresenter, IReadOnlyList<AssociationRule>>(rules);
            }
        }

        /// <summary>
        /// Обработчик запроса поиска ассоциативных правил
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="options">Параметры поиска ассоциативных правил, заданные пользователем</param>
        private void MineAssociationRules(object sender, MiningArgs options)
        {
            Requires.NotNull(options, nameof(options));

            if (options.Support < 0 || options.Support > 1)
                _messageBox.ShowMessage("Ошибка ввода", "Указаны неверный порог поддержки");
            else if (options.Confidence < 0 || options.Confidence > 1)
                _messageBox.ShowMessage("Ошибка ввода", "Указан неверный порог достоверности");
            else if (string.IsNullOrWhiteSpace(options.TransactionFilePath))
                _messageBox.ShowMessage("Ошибка ввода", "Путь к файлу с транзакциями не может быть пустым");
            else if (!File.Exists(options.TransactionFilePath))
                _messageBox.ShowMessage("Ошибка ввода", "Не найден файл с транзакциями по указанному пути");
            else
            {
                view.ShowMiningProgress();
                List<AssociationRule> rules = null;
                try
                {
                    rules = _miner.Mine
                    (
                        transactions: _transactionReader.ReadTransactions(options.TransactionFilePath),
                        parameters: new MiningParameters
                        (
                            support: options.Support,
                            confidence: options.Confidence
                        ),
                        converter: new ItemsetsConverter
                        (
                            items: _repositoryCreator.GetRepository<HierarchyItem>().Get(),
                            rules: options.ReplaceArticlesWithGroups ?
                                _repositoryCreator.GetRepository<ItemReplacementRule>().Get() :
                                null
                        ),
                        itemDeletionRules: options.ApplyItemDeletionRules ?
                                _repositoryCreator.GetRepository<ItemDeletionRule>().Get() :
                                null
                    );
                }
                catch (OperationCanceledException)
                {

                }
                catch
                {
                    _messageBox.ShowMessage("Поиск правил", "При поиске ассоциативных правил возникла ошибка");
                }
                finally
                {
                    view.HideMiningProgress();
                }

                if (rules != null)
                {
                    if (rules.Count != 0)
                        controller.Run<ShowAssociationRulesPresenter, IReadOnlyList<AssociationRule>>(rules);
                    else
                    {
                        _messageBox.ShowMessage
                        (
                            title: "Поиск правил",
                            message: "Не найдено ассоциативных правил, удовлетворяющих условиям поиска"
                        );
                    }
                }
            }
        }
    }
}