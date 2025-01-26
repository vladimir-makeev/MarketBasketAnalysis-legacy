namespace MarketBasketAnalysis.Presentation.Common
{
    /// <summary>
    /// Описание функционала показа сообщений
    /// </summary>
    public interface IMessageBox
    {
        /// <summary>
        /// Показ сообщения
        /// </summary>
        /// <param name="title">Заголовок сообщения</param>
        /// <param name="message">Текст сообщения</param>
        void ShowMessage(string title, string message);
    }
}