using System.Windows;
using System.Windows.Controls;

namespace MarketBasketAnalysis.UI
{
    /// <summary>
    /// Анимация ожидания
    /// </summary>
    public partial class WaitingAnimationControl : ContentControl
    {
        /// <summary>
        /// Элемент управления, в котором отображается анимация ожидания
        /// </summary>
        private ContentControl _control;

        /// <summary>
        /// Отображается ли анимация в данный момент
        /// </summary>
        private bool _isAnimationActive;

        /// <summary>
        /// Текст, отображаемый рядом с анимацией ожидания
        /// </summary>
        public string Title
        {
            get
            {
                return (string)TitleLabel.Content;
            }
        }

        /// <summary>
        /// Отображается ли анимация в данный момент
        /// </summary>
        public bool IsAnimationActive
        {
            get
            {
                return _isAnimationActive;
            }
        }

        /// <summary>
        /// Инициализация графических компонентов анимации
        /// </summary>
        /// <param name="title">Текст, показываемый рядом с анимацией ожидания</param>
        /// <param name="control">Элемент управления, в котором необходимо отобразить анимацию</param>
        public WaitingAnimationControl(string title, ContentControl control)
        {
            _control = control;
            _isAnimationActive = false;
            InitializeComponent();
            TitleLabel.Content = title;
        }

        /// <summary>
        /// Запуск анимации
        /// </summary>
        public void StartAnimation()
        {
            if (!_isAnimationActive)
            {
                _isAnimationActive = true;
                Dispatcher.Invoke(() =>
                {
                    var content = (UIElement)_control.Content;
                    content.IsEnabled = false;
                    _control.Content = null;
                    _control.Content = new Grid { Children = { this, content } };
                });
            }
        }

        /// <summary>
        /// Остановка анимации
        /// </summary>
        public void StopAnimation()
        {
            if (_isAnimationActive)
            {
                _isAnimationActive = false;
                Dispatcher.Invoke(() =>
                {
                    var grid = (Grid)_control.Content;
                    var content = ((Grid)_control.Content).Children[1];
                    grid.Children.Clear();
                    content.IsEnabled = true;
                    _control.Content = content;
                });
            }
        }
    }
}
