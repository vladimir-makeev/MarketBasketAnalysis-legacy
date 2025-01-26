using CodeContracts;

namespace MarketBasketAnalysis.Presentation.Views.EventArguments
{
    /// <summary>
    /// Параметры запроса фильтрации ассоциативных правил
    /// </summary>
    public class FilteringArgs
    {
        /// <summary>
        /// Поисковая строка
        /// </summary>
        public string SearchString { get; }

        /// <summary>
        /// Требуется ли полное соответствие поисковой строки
        /// частям ассоциативных правил при их поиске
        /// </summary>
        public bool ExactMatch { get; }

        /// <summary>
        /// Учет регистра букв при поиске ассоциативных правил
        /// </summary>
        public bool CaseSensitive { get; }

        /// <summary>
        /// Интервал поддержки правила
        /// </summary>
        public Interval<double> Support { get; }

        /// <summary>
        /// Интервал достоверности правила
        /// </summary>
        public Interval<double> Confidence { get; }

        /// <summary>
        /// Интервал меры интереса
        /// </summary>
        public Interval<double> Lift { get; }

        public Interval<double> Conviction { get; }

        /// <summary>
        /// Интервал модуля коэффициента ассоциации
        /// </summary>
        public Interval<double> AbsoluteAssociationCoef { get; }

        /// <summary>
        /// Интервал модуля коэффициента контингенции
        /// </summary>
        public Interval<double> AbsoluteContingencyCoef { get; }

        /// <summary>
        /// Требуется ли проверка гипотезы о независимости
        /// правой и левой частях ассоциативных правил
        /// </summary>
        public bool RunTestOfIndependence { get; }

        /// <summary>
        /// Задание опций фильтрации ассоциативных правил
        /// </summary>
        /// <param name="searchString">Поисковая строка</param>
        /// <param name="caseSensitive">
        /// Требуется ли полное соответствие поисковой строки
        /// частям ассоциативных правил при их поиске
        /// </param>
        /// <param name="exactMatch">Учет регистра букв при поиске ассоциативных правил</param>
        /// <param name="support">Интервал поддержки правила</param>
        /// <param name="confidence">Интервал достоверности правила</param>
        /// <param name="lift">Интервал меры интереса</param>
        /// <param name="conviction">Интервал меры уверенности</param>
        /// <param name="absoluteAssociationCoef">Интервал модуля коэффициента ассоциации</param>
        /// <param name="absoluteContingencyCoef">Интервал модуля коэффициента контингенции</param>
        /// <param name="runTestOfIndependence">Требуется ли проверка гипотезы о независимости частей правил</param>
        public FilteringArgs
        (
            string searchString,
            bool exactMatch,
            bool caseSensitive,
            Interval<double> support,
            Interval<double> confidence,
            Interval<double> lift,
            Interval<double> conviction,
            Interval<double> absoluteAssociationCoef,
            Interval<double> absoluteContingencyCoef,
            bool runTestOfIndependence
        )
        {
            Requires.NotNull(searchString, nameof(searchString));
            Requires.NotNull(support, nameof(support));
            Requires.NotNull(confidence, nameof(confidence));
            Requires.NotNull(lift, nameof(lift));
            Requires.NotNull(conviction, nameof(conviction));
            Requires.NotNull(absoluteAssociationCoef, nameof(absoluteAssociationCoef));

            this.SearchString = searchString;
            this.ExactMatch = exactMatch;
            this.CaseSensitive = caseSensitive;
            this.Support = support;
            this.Confidence = confidence;
            this.Lift = lift;
            this.Conviction = conviction;
            this.AbsoluteAssociationCoef = absoluteAssociationCoef;
            this.AbsoluteContingencyCoef = absoluteContingencyCoef;
            this.RunTestOfIndependence = runTestOfIndependence;
        }
    }
}
