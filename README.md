# Приложение для поиска и анализа ассоциативных правил

## Стек технологий

C#, SQLite, WPF, .NET Framework 4.7.2.

## Назначение приложения

1. Исследование покупательского поведения.
2. Развитие системы персональных предложений.
3. Оптимизация ассортимента товаров.

## Функциональные возможности

1. Поиск ассоциативных правил на основе указанного файла транзакций и заданных порогов поддержки, достоверности.
2. Вычисление мер значимости ассоциативных правил: поддержки, достоверности, меры Lift, меры Conviction, коэффициента ассоциации, коэффициента контингенции.
3. Поиск обобщенных ассоциативных правил для иерархии с уровнями "Товар", "Группа товаров".
4. Исключение из поиска правил ассоциаций определенного элемента транзакций.
5. Просмотр найденных ассоциативных правил в виде таблицы, графа.
6. Поиск и отображение максимальных подграфов графа ассоциативных правил.
7. Выделение общих и уникальных элементов пары списков ассоциативных правил.
8. Загрузка из файла, сохранение в файл найденных ассоциативных правил.

## Запуск в среде разработки

1. Установить среду разработки Visual Studio.
2. В Visual Studio Installer установить:
- рабочую нагрузку "Разработка классических приложений .NET";
- отдельный компонент "Пакет SDK для .NET Framework 4.7.2".
3. Открыть файл решения `MarketBasketAnalysis.sln`.
4. Назначить запускаемым проектом `MarketBasketAnalysis.UI`.
5. Нажать кнопку "Пуск".

## Дополнительная информация

Приложение разрабатывалось для торговой сети [Командор](https://sm-komandor.ru) в 2019-2020 годах и представляет дальнейшее развитие [аналогичного приложения на языке R](https://github.com/vladimir-makeev/Apriori).

<details>
  <summary><b>Пример использования</b></summary>
1. Создать CSV-файл транзакций со следующим содержимым:

```csv
Сигареты Вог|Ананас большой 1кг|Аджика Махеевъ Острая 190г ст/б; 12
Анчоус сушеный 1кг|Батончик Натс Арахис 50г; 36|Брюшки лосося с/м 1кг; 20
Ваза Вероника хрусталь 880|Вафли Сладонеж Мнямка 200г; 24|Вино Канти бел п/сух 12% 0,75л
Водолазка Беллиссима Икебана 3|Газ вода Пепси Лайт 1,75л пл/б; 6|Журнал Охота и рыбалка XXI век
Зубная щетка Колгейт Плюс жесткая|Игрушка Набор Черепашка-Ниндзя НО 3451А (5 видов)|Кастрюля Витесс 18см 2,65л VS-1036
```

2. Запустить приложение, в главном меню выбрать пункт "Поиск ассоциативных правил", указать путь к файлу с транзакциями и нажать кнопку "Поиск правил".

![Поиск ассоциативных правил](https://github.com/user-attachments/assets/470b1013-7071-4a24-8d60-49c374b1e2c3)

3. По умолчанию найденные ассоциативные правила будут отображены в виде таблицы, возможно переключение на графовое представление.

![Таблица найденных ассоциативных правил](https://github.com/user-attachments/assets/fc1f1469-647e-45d6-a828-1db7a18ea44a)

![Граф найденных ассоциативных правил](https://github.com/user-attachments/assets/042ee489-0386-4a4d-8a25-f70f191aa29d)
</details>
