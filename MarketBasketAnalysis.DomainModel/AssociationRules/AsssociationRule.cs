using System;
using CodeContracts;
using MathNet.Numerics.Distributions;

namespace MarketBasketAnalysis.DomainModel.AssociationRules
{
    /// <summary>
    /// Ассоциативное правило
    /// </summary>
    public class AssociationRule
    {
        /// <summary>
        /// Левая часть правила (условие)
        /// </summary>
        public string LeftHandSide { get; private set; }

        /// <summary>
        /// Правая часть правила (следствие)
        /// </summary>
        public string RightHandSide { get; private set; }

        /// <summary>
        /// Количество транзакций, содержащих условие
        /// </summary>
        public int LHSCount { get; private set;  }

        /// <summary>
        /// Количество транзакций, содержащих следствие
        /// </summary>
        public int RHSCount { get; private set; }

        /// <summary>
        /// Количество транзакций, содержащих условие и следствие
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Поддержка - частота совместной встречаемости условия и следствия
        /// </summary>
        public double Support { get; private set; }

        /// <summary>
        /// Поддержка левой части правила
        /// </summary>
        public double LHSSupport { get; private set; }

        /// <summary>
        /// Поддержка правой части правила
        /// </summary>
        public double RHSSupport { get; private set; }

        /// <summary>
        /// Достоверность - частота встречаемости следствия в транзакциях
        /// при наличии условия
        /// </summary>
        public double Confidence { get; private set; }

        /// <summary>
        /// Мера интереса - отношение совместной частоты условия и следствия
        /// к произведению их частот встречаемости по отдельности
        /// </summary>
        public double Lift { get; private set; }

        /// <summary>
        /// Уверенность правила
        /// </summary>
        public double Conviction { get; private set; }

        /// <summary>
        /// Модуль коэффициента ассоциации - мера взаимосвязи условия и следствия
        /// </summary>
        public double AbsoluteAssociationCoef { get; private set; }

        /// <summary>
        /// Модуль коэффициента контингенции
        /// </summary>
        public double AbsoluteContingencyCoef { get; private set; }

        /// <summary>
        /// Значение критерия хи-квадрат для проверки гипотезы о независимости
        /// между левой и правой частями ассоциативного правила
        /// </summary>
        public bool AreHandSidesIndependent { get; private set; }

        /// <summary>
        /// Создание ассоциативнго правила, рассчет его метрик
        /// </summary>
        /// <param name="lhs">Условие правила</param>
        /// <param name="rhs">Следствие правила</param>
        /// <param name="transactionCount">Число транзакций в выборке</param>
        /// <param name="pairCount">Количество транзакций, содержащих условие и следствие</param>
        /// <param name="lhsCount">Количество транзакций, содержащих условие</param>
        /// <param name="rhsCount">Количество транзакций, содержащих следствие</param>
        public AssociationRule
        (
            string lhs, string rhs,
            int transactionCount, int pairCount,
            int lhsCount, int rhsCount
        )
        {
            Requires.NotNullOrEmpty(lhs, nameof(lhs));
            Requires.NotNullOrEmpty(rhs, nameof(rhs));
            Requires.True(lhs != rhs, "Левая часть правила не должна быть равна правой");
            Requires.InRange(transactionCount > 0, nameof(transactionCount));
            Requires.InRange(lhsCount > 0, nameof(lhsCount));
            Requires.InRange(rhsCount > 0, nameof(rhsCount));
            Requires.InRange(pairCount > 0, nameof(pairCount));
            Requires.True
            (
                condition: lhsCount <= transactionCount,
                message: "Количество транзакций, содержащих левую часть, " +
                         "не может быть больше их общего количества"
            );
            Requires.True
            (
                condition: rhsCount <= transactionCount,
                message: "Количество транзакций, содержащих правую часть, " +
                         "не может быть больше их общего количества"
            );
            Requires.True
            (
                condition: pairCount <= Math.Min(lhsCount, rhsCount),
                message: "Количество транзакций, содержащих левую и правую части, " +
                         "не должно быть больше числа транзакций, приходящихся на каждую часть"
            );

            this.LeftHandSide = lhs;
            this.RightHandSide = rhs;
            this.LHSCount = lhsCount;
            this.RHSCount = rhsCount;
            this.Count = pairCount;
            this.LHSSupport = LHSCount / (double)transactionCount;
            this.RHSSupport = RHSCount / (double)transactionCount;
            this.Support = pairCount / (double)transactionCount;
            this.Confidence = this.Support / (lhsCount / (double)transactionCount);
            this.Lift = this.Confidence / (rhsCount / (double)transactionCount);
            this.Conviction = (1 - RHSSupport) / (1 - Confidence);
            var a = (double)pairCount;
            var b = (double)lhsCount - pairCount;
            var c = (double)rhsCount - pairCount;
            var d = transactionCount - a - b - c;
            this.AbsoluteAssociationCoef = Math.Abs((a * d - b * c) / (a * d + b * c));
            this.AbsoluteContingencyCoef = Math.Abs((a * d - b * c) /
                Math.Sqrt((a + b) * (a + c) * (b + d) * (c + d)));
            var chiSquaredValue = transactionCount * Math.Pow(a * d - b * c, 2) /
                ((a + b) * (a + c) * (b + d) * (c + d));

            if (chiSquaredValue > ChiSquared.InvCDF(1, 0.99))
                this.AreHandSidesIndependent = false;
            else
                this.AreHandSidesIndependent = true;
        }

        /// <summary>
        /// Конструктор по умолчанию для десериализации объекта
        /// </summary>
        private AssociationRule()
        {

        }
    }
}