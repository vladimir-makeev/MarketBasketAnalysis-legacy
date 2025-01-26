using System.Linq;
using System.Collections.Generic;
using CodeContracts;
using MarketBasketAnalysis.Presentation.Common;
using MarketBasketAnalysis.Presentation.Views;
using MarketBasketAnalysis.DomainModel.AssociationRules.Mining;
using MarketBasketAnalysis.Services;
using MarketBasketAnalysis.Presentation.Views.EventArguments;
using MarketBasketAnalysis.DomainModel.ProductHierarchy;

namespace MarketBasketAnalysis.Presentation.Presenters
{
    /// <summary>
    /// Представитель, взаимодействующий с представлением настроек приложения
    /// </summary>
    public class SettingsPresenter : BasePresenter<ISettingsView>
    {
        /// <summary>
        /// Средство показа сообщений
        /// </summary>
        private readonly IMessageBox _messageBox;

        /// <summary>
        /// Группы товаров
        /// </summary>
        private readonly IReadOnlyList<string> _articleGroups;

        /// <summary>
        /// Хранилище правил замены товаров товарными группами в ассоциативных правилах
        /// </summary>
        private readonly IRepository<ItemReplacementRule> _itemReplacementRulesRepository;

        /// <summary>
        /// Хранилище правил исключения элементов транзакций из поиска ассоциативных правил
        /// </summary>
        private readonly IRepository<ItemDeletionRule> _itemDeletionRulesRepository;

        /// <summary>
        /// Создание представителя
        /// </summary>
        /// <param name="controller">Контроллер приложения</param>
        /// <param name="navigator">Средство показа, закрытия представлений</param>
        /// <param name="view">Представление настроек приложения</param>
        /// <param name="messageBox">Средство показа сообщений</param>
        /// <param name="repositoryCreator">Средство создания хранилища объектов</param>
        public SettingsPresenter
        (
            ApplicationController controller,
            INavigator navigator,
            ISettingsView view,
            IMessageBox messageBox,
            IRepositoryCreator repositoryCreator
        ) : base(controller, navigator, view)
        {
            Requires.NotNull(messageBox, nameof(messageBox));
            Requires.NotNull(repositoryCreator, nameof(repositoryCreator));

            _messageBox = messageBox;
            _itemDeletionRulesRepository = repositoryCreator.GetRepository<ItemDeletionRule>();
            _itemReplacementRulesRepository = repositoryCreator.GetRepository<ItemReplacementRule>();

            _articleGroups = repositoryCreator.GetRepository<HierarchyItem>()
                .Get()
                .AsParallel()
                .Select(item => item.ArticleGroup)
                .ToHashSet()
                .OrderBy(group => group)
                .ToList();

            view.SettingsSaveRequested += SaveSettings;
        }

        /// <summary>
        /// Обработчик запроса сохранения настроек
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="options">Настройки приложения, заданные пользователем</param>
        private void SaveSettings(object sender, SaveSettingsArgs options)
        {
            Requires.NotNull(options, nameof(options));

            bool HasEmptyStrings(IEnumerable<string> items) =>
                    items.Any(item => string.IsNullOrWhiteSpace(item));

            bool HasDuplicates(IEnumerable<string> items) =>
                items.GroupBy(item => item).Any(group => group.Count() > 1);

            if (HasEmptyStrings(options.ItemDeletionRules.Select(item => item.Item1)))
                _messageBox.ShowMessage("Ошибка ввода", "Список неучитываемых элементов не должен содержать пустые строки");
            else if (HasDuplicates(options.ItemDeletionRules.Select(item => item.Item1)))
                _messageBox.ShowMessage("Ошибка ввода", "Список неучитываемых элементов содержит повторяющиеся наименования");
            else if (HasEmptyStrings(options.ItemReplacementRules.Select(item => item.Item1)))
                _messageBox.ShowMessage("Ошибка ввода", "Список для замены не должен содержать пустые строки");
            else if (HasDuplicates(options.ItemReplacementRules.Select(item => item.Item1)))
                _messageBox.ShowMessage("Ошибка ввода", "Список для замены содержит повторяющиеся группы товаров");
            else if (options.ItemReplacementRules.Any(item => !_articleGroups.Contains(item.Item1)))
                _messageBox.ShowMessage("Ошибка ввода", "Список для замены содержит недопустимые группы товаров");
            else
            {
                _itemDeletionRulesRepository.Clear();
                _itemReplacementRulesRepository.Clear();
                _itemDeletionRulesRepository.Add
                (
                    items: options.ItemDeletionRules
                        .Select(item => new ItemDeletionRule(item.Item1, item.Item2))
                        .ToList()
                );
                _itemReplacementRulesRepository.Add
                (
                    items: options.ItemReplacementRules
                        .Select(item => new ItemReplacementRule(item.Item1, item.Item2))
                        .ToList()
                );
                _messageBox.ShowMessage("Сохранение настроек", "Настройки успешно сохранены");
            }
        }

        /// <summary>
        /// Показ представления, настроек приложения
        /// </summary>
        public override void Run()
        {
            view.ShowItemDeletionRules(_itemDeletionRulesRepository.Get());
            view.ShowItemReplacementRules(_itemReplacementRulesRepository.Get());
            view.ShowArticleGroups(_articleGroups);
            base.Run();
        }
    }
}