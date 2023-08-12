using System;
using SFML.Learning;
using SFML.Window;

class Program : Game
{
    static string bgMenu = LoadTexture("background_menu.png");
    static string bgGame = LoadTexture("background_game.png");

    static string clickedSound = LoadSound("clickedSound.wav");
    static string defeatSound = LoadSound("defeatSound.wav");
    static string winSound = LoadSound("winSound.wav");

    static string menuMusic = LoadMusic("Music_0.wav");
    static string level1Music = LoadMusic("Music_1.wav");
    static string level2Music = LoadMusic("Music_2.wav");
    static string level3Music = LoadMusic("Music_3.wav");

    static int[,] numbers;
    static Random rnd = new Random();
    static int menuWindow = 0;
    static bool shutDown = false;
    static bool startGame = false;
    static bool isVictory = false;
    static bool awardReceived = false;
    static bool playDefeatSound = false;

    static int cellSize = 200;                  // Размер ячейки
    static int leftOffset;                // Отступ по X
    static int topOffset = 50;                 // Отступ по Y
    static int spaceBetweenCells = 20;          // Пробел между ячейками

    static int difficulty;                  // Сложность: 1 - легко, 2 - средне, 3 - сложно
    static int numbersAmount;               // кол-во цифр
    static float initialNumber;               // начальное число
    static float lastNumber;                // финальное число
    static int numberIDfromMousePosition;   // ID цифры от положения мыши
    static int selectedNumbersAmount = 0;        // кол-во выбранных цифр
    static int playerSelectedNumber1;
    static int playerSelectedNumber2;
    static int playerSelectedNumber3;
    static int playerSelectedNumber4;
    static int playerSelectedNumber5;
    static int playerSelectedOperator1;
    static int playerSelectedOperator2;
    static int playerSelectedOperator3;
    static int playerSelectedOperator4;
    static int playerSelectedOperator5;
    static float intermediateCalculations1;
    static float intermediateCalculations2;
    static float intermediateCalculations3;
    static float intermediateCalculations4;
    static float intermediateCalculations5;
    static float timer = 0;
    static int spentTime = 0;
    static int highscore = 0;
    static int levelsOpen = 1;

    static void DisplayMenu()
    {
        DrawSprite(bgMenu, 0, 0);

        switch (menuWindow)
        {
            case 0:
                SetFillColor(192, 192, 192);
                FillRectangle(600, 100, 400, 80);
                FillRectangle(600, 800, 400, 80);
                FillRectangle(1380, 800, 220, 80);

                SetFillColor(0, 0, 20);
                DrawText(700, 90, "Начать", 72);
                DrawText(645, 790, "Инструкция", 72);
                DrawText(1415, 790, "Выход", 72);

                if (MouseX > 600 && MouseX < 1000 && MouseY > 100 && MouseY < 180 && GetMouseButtonDown(Mouse.Button.Left)) { menuWindow = 1; PlaySound(clickedSound); }
                if (MouseX > 600 && MouseX < 1000 && MouseY > 800 && MouseY < 880 && GetMouseButtonDown(Mouse.Button.Left)) { menuWindow = 2; PlaySound(clickedSound); }
                if (MouseX > 1380 && MouseX < 1600 && MouseY > 800 && MouseY < 880 && GetMouseButtonDown(Mouse.Button.Left)) { shutDown = true; PlaySound(clickedSound); }
                break;

            case 1:
                SetFillColor(0, 255, 0);
                FillRectangle(185, 195, 410, 90);
                SetFillColor(255, 255, 0);
                if (levelsOpen >= 2) FillRectangle(595, 195, 410, 90);
                SetFillColor(255, 0, 0);
                if (levelsOpen >= 3) FillRectangle(1005, 195, 410, 90);

                SetFillColor(192, 192, 192);
                FillRectangle(500, 100, 600, 80);
                FillRectangle(190, 200, 400, 80);
                if (levelsOpen >= 2) FillRectangle(600, 200, 400, 80);
                if (levelsOpen >= 3) FillRectangle(1010, 200, 400, 80);
                FillRectangle(0, 800, 220, 80);

                SetFillColor(0, 0, 20);
                DrawText(530, 90, "Выберите сложность:", 72);
                DrawText(310, 190, "Легко", 72);
                if (levelsOpen >= 2) DrawText(710, 190, "Средне", 72);
                if (levelsOpen >= 3) DrawText(1120, 190, "Сложно", 72);
                DrawText(30, 790, "Назад", 72);

                if (MouseX > 190 && MouseX < 590 && MouseY > 200 && MouseY < 280 && GetMouseButtonDown(Mouse.Button.Left)) { difficulty = 1; startGame = true; PlaySound(clickedSound); }
                if (MouseX > 600 && MouseX < 1000 && MouseY > 200 && MouseY < 280 && GetMouseButtonDown(Mouse.Button.Left) && levelsOpen >= 2) { difficulty = 2; startGame = true; PlaySound(clickedSound); }
                if (MouseX > 1010 && MouseX < 1410 && MouseY > 200 && MouseY < 280 && GetMouseButtonDown(Mouse.Button.Left) && levelsOpen >= 3) { difficulty = 3; startGame = true; PlaySound(clickedSound); }
                if (MouseX > 0 && MouseX < 220 && MouseY > 800 && MouseY < 880 && GetMouseButtonDown(Mouse.Button.Left)) { menuWindow = 0; PlaySound(clickedSound); }
                break;

            case 2:
                SetFillColor(192, 192, 192);
                FillRectangle(0, 800, 220, 80);
                FillRectangle(0, 0, 1600, 500);

                SetFillColor(0, 0, 20);
                DrawText(30, 790, "Назад", 72);
                DrawText(50, 0, "Цель игры: приравнять начальное число (слева) к финальному числу (справа) с помощью", 50);
                DrawText(5, 50, "математических операций, находящихся в центре(сложение, вычитание, умножение и деление)", 50);
                DrawText(50, 100, "Задействоваться может любая комбинация, главное провести 5 мат. операций и получить", 50);
                DrawText(5, 150, "необходимый ответ. Очки зарабатываются после успешного прохождения уровня, в", 50);
                DrawText(5, 200, "зависимости от затраченного времени. После прохождения первого уровня сложности -", 50);
                DrawText(5, 250, "открываются следующий, с большим колличеством возможных очков (всего 3).", 50);
                DrawText(50, 300, "Горячие клавиши: \"R\" - обнулить все операции; \"Backspace\" - перезапуск уровня;", 50);
                DrawText(5, 350, "\"Space\" (При успешном прохождении уровня) - перейти на следующий уровень;", 50);
                DrawText(5, 400, "\"Escape\" - выход в меню.", 50);
                if (MouseX > 0 && MouseX < 220 && MouseY > 800 && MouseY < 880 && GetMouseButtonDown(Mouse.Button.Left)) { menuWindow = 0; PlaySound(clickedSound); }
                break;
        }
    }

    static void InitNumbers()
    {
        if (difficulty == 3)
        {
            leftOffset = 360;
            numbersAmount = 12;
        }
        else
        {
            leftOffset = 440;
            numbersAmount = 9;
        }

        numbers = new int[numbersAmount, 4];

        for (int i = 0; i < numbersAmount; i++)
        {
            switch (difficulty)
            {
                case 1:
                    numbers[i, 0] = rnd.Next(2, 11);            // Цифра
                    numbers[i, 1] = rnd.Next(1, 3);            // Оператор: 1 - Сложение (+), 2 - Вычитания (-), 3 - Умножение (*), 4 - Деление (/) Цвет: Сложение - синий, Вычитание - жёлтый, Умножение - зелёный, Деление - красный
                    numbers[i, 2] = (i % 3) * (cellSize + spaceBetweenCells) + leftOffset;     // Положение по X
                    numbers[i, 3] = (i / 3) * (cellSize + spaceBetweenCells) + topOffset;     // Положение по Y
                    break;
                case 2:
                    numbers[i, 0] = rnd.Next(2, 10);
                    numbers[i, 1] = rnd.Next(1, 3);
                    numbers[i, 2] = (i % 3) * (cellSize + spaceBetweenCells) + leftOffset;
                    numbers[i, 3] = (i / 3) * (cellSize + spaceBetweenCells) + topOffset;
                    break;
                case 3:
                    numbers[i, 0] = rnd.Next(2, 100);
                    numbers[i, 1] = rnd.Next(1, 3);
                    numbers[i, 2] = (i % 4) * (cellSize + spaceBetweenCells) + leftOffset;
                    numbers[i, 3] = (i / 4) * (cellSize + spaceBetweenCells) + topOffset;
                    break;
            }
        }
        if (difficulty > 1)
        {
            int cash = rnd.Next(0, numbersAmount);      // Создание умножения
            numbers[cash, 0] = 2;
            numbers[cash, 1] = 3;

            int cash2 = rnd.Next(0, numbersAmount);     // Создание деления
            while (true)
            {
                if (cash != cash2) break;
                cash2 = rnd.Next(0, numbersAmount);
            }
            numbers[cash2, 0] = 2;
            numbers[cash2, 1] = 4;
        }
    }

    static void StartGameAnimation(int animation)
    {
        int y = 0;

        switch (animation)
        {
            case 1:
                SetFillColor(0, 255, 0);
                break;
            case 2:
                SetFillColor(255, 255, 0);
                break;
            case 3:
                SetFillColor(255, 0, 0);
                break;
        }

        for (int x = 0; x < 3200 && y < 1800; x += 50, y+= 25)
        {
            for (int i = 0; i < 32; i++)
            {
                DrawText(50 * i, y, "1", 72);
            }
            for (int i = 0; i < 18; i++)
            {
                DrawText(x, 50 * i, "0", 72);
            }
            DisplayWindow();
            Delay(10);
        }
    }

    static void Hotkeys()
    {
        if (GetKeyUp(Keyboard.Key.Escape))                              // Выход в меню
        {
            isVictory = false;
            startGame = false;
            awardReceived = false;
            playDefeatSound = false;
            selectedNumbersAmount = 0;
            timer = 0;
        }
        if (GetKeyUp(Keyboard.Key.R))                                   // Ресет выбранных значений
        {
            selectedNumbersAmount = 0;
            playDefeatSound = false;
        }
        if (GetKeyUp(Keyboard.Key.Space) && isVictory == true)          // Увеличение сложности
        {
            isVictory = false;
            awardReceived = false;
            selectedNumbersAmount = 0;
            if (difficulty < 3) difficulty++;
            if (levelsOpen < difficulty) levelsOpen = difficulty;
            InitNumbers();
            initialNumber = rnd.Next(2, 100);
            GenerateFinalNumber();
            timer = 0;
        }
        if (GetKeyDown(Keyboard.Key.BackSpace))                         // Перезапуск уровня
        {
            isVictory = false;
            awardReceived = false;
            playDefeatSound = false;
            selectedNumbersAmount = 0;
            InitNumbers();
            initialNumber = rnd.Next(2, 100);
            GenerateFinalNumber();
            timer = 0;
        }
    }

    static void DrawNumbers()
    {
        for (int i = 0; i < numbersAmount; i++)
        {
            if (numbers[i, 1] == 1)
            {
                SetFillColor(0, 0, 255);
                FillRectangle(numbers[i, 2] - 5, numbers[i, 3] - 5, cellSize + 10, cellSize + 10);

                SetFillColor(192, 192, 192);
                FillRectangle(numbers[i, 2], numbers[i, 3], cellSize, cellSize);

                SetFillColor(0, 0, 255);
                DrawText(numbers[i, 2] + (cellSize / 4) - 10, numbers[i, 3] + (cellSize / 4), "+ " + numbers[i, 0], 72);
            }

            if (numbers[i, 1] == 2)
            {
                SetFillColor(255, 255, 0);
                FillRectangle(numbers[i, 2] - 5, numbers[i, 3] - 5, cellSize + 10, cellSize + 10);

                SetFillColor(192, 192, 192);
                FillRectangle(numbers[i, 2], numbers[i, 3], cellSize, cellSize);

                SetFillColor(255, 255, 0);
                DrawText(numbers[i, 2] + (cellSize / 4) - 10, numbers[i, 3] + (cellSize / 4), "- " + numbers[i, 0], 72);
            }

            if (numbers[i, 1] == 3)
            {
                SetFillColor(0, 255, 0);
                FillCircle(numbers[i, 2] + (cellSize / 2), numbers[i, 3] + (cellSize / 2), (cellSize / 2) + 5);

                SetFillColor(192, 192, 192);
                FillCircle(numbers[i, 2] + (cellSize / 2), numbers[i, 3] + (cellSize / 2), cellSize / 2);

                SetFillColor(0, 200, 0);
                DrawText(numbers[i, 2] + (cellSize / 4) - 10, numbers[i, 3] + (cellSize / 4), "* " + numbers[i, 0], 72);
            }

            if (numbers[i, 1] == 4)
            {
                SetFillColor(255, 0, 0);
                FillCircle(numbers[i, 2] + (cellSize / 2), numbers[i, 3] + (cellSize / 2), (cellSize / 2) + 5);

                SetFillColor(192, 192, 192);
                FillCircle(numbers[i, 2] + (cellSize / 2), numbers[i, 3] + (cellSize / 2), cellSize / 2);

                SetFillColor(255, 0, 0);
                DrawText(numbers[i, 2] + (cellSize / 4) - 10, numbers[i, 3] + (cellSize / 4), "/ " + numbers[i, 0], 72);
            }
        }
    }

    static void GenerateFinalNumber()
    {
        lastNumber = initialNumber;

        for (int i = 0; i < 5; i++)
        {
            int numberID = rnd.Next(0, numbersAmount);

            switch (numbers[numberID, 1])
            {
                case 1:
                    lastNumber += numbers[numberID, 0];
                    break;

                case 2:
                    lastNumber -= numbers[numberID, 0];
                    break;

                case 3:
                    lastNumber *= numbers[numberID, 0];
                    break;

                case 4:
                    lastNumber /= numbers[numberID, 0];
                    break;
            }
        }
    }

    static int GetIDNumberFromMousePosition() // Получение ID цифры от положения мыши
    {
        for (int i = 0; i < numbersAmount; i++)
        {
            if (MouseX > numbers[i, 2] && MouseX < numbers[i, 2] + cellSize && MouseY > numbers[i, 3] && MouseY < numbers[i, 3] + cellSize) return i;
        }
        return - 1;
    }

    static void DeterminationOfSelectedNumbers()
    {
        if (numberIDfromMousePosition != -1 && GetMouseButtonUp(Mouse.Button.Left) && timer != 0)
        {
            PlaySound(clickedSound);
            switch (selectedNumbersAmount)
            {
                case 0:
                    playerSelectedNumber1 = numbers[numberIDfromMousePosition, 0];
                    playerSelectedOperator1 = numbers[numberIDfromMousePosition, 1];
                    selectedNumbersAmount++;
                    break;

                case 1:
                    playerSelectedNumber2 = numbers[numberIDfromMousePosition, 0];
                    playerSelectedOperator2 = numbers[numberIDfromMousePosition, 1];
                    selectedNumbersAmount++;
                    break;

                case 2:
                    playerSelectedNumber3 = numbers[numberIDfromMousePosition, 0];
                    playerSelectedOperator3 = numbers[numberIDfromMousePosition, 1];
                    selectedNumbersAmount++;
                    break;

                case 3:
                    playerSelectedNumber4 = numbers[numberIDfromMousePosition, 0];
                    playerSelectedOperator4 = numbers[numberIDfromMousePosition, 1];
                    selectedNumbersAmount++;
                    break;

                case 4:
                    playerSelectedNumber5 = numbers[numberIDfromMousePosition, 0];
                    playerSelectedOperator5 = numbers[numberIDfromMousePosition, 1];
                    selectedNumbersAmount++;
                    break;
            }
        }
    }

    static void DrawSelectedNumbers()
    {
        if (selectedNumbersAmount >= 1)
        {
            SetFillColor(148, 0, 211);
            DrawText(310, 740, playerSelectedNumber1.ToString(), 50);
            switch (playerSelectedOperator1)
            {
                case 1:
                    DrawText(215, 735, "+", 50);
                    break;
                case 2:
                    DrawText(220, 735, "-", 50);
                    break;
                case 3:
                    DrawText(215, 755, "*", 50);
                    break;
                case 4:
                    DrawText(220, 740, "/", 50);
                    break;
            }
            SetFillColor(218, 165, 32);
            DrawLine(374, 755, 374, 900, 5);
            DrawLine(130, 830, 374, 830, 5);

            if (selectedNumbersAmount >= 2)
            {
                SetFillColor(148, 0, 211);
                DrawText(554, 740, playerSelectedNumber2.ToString(), 50);
                switch (playerSelectedOperator2)
                {
                    case 1:
                        DrawText(459, 735, "+", 50);
                        break;
                    case 2:
                        DrawText(464, 735, "-", 50);
                        break;
                    case 3:
                        DrawText(459, 755, "*", 50);
                        break;
                    case 4:
                        DrawText(464, 740, "/", 50);
                        break;
                }
                SetFillColor(218, 165, 32);
                DrawLine(618, 755, 618, 900, 5);
                DrawLine(374, 830, 618, 830, 5);

                if (selectedNumbersAmount >= 3)
                {
                    SetFillColor(148, 0, 211);
                    DrawText(798, 740, playerSelectedNumber3.ToString(), 50);
                    switch (playerSelectedOperator3)
                    {
                        case 1:
                            DrawText(703, 735, "+", 50);
                            break;
                        case 2:
                            DrawText(708, 735, "-", 50);
                            break;
                        case 3:
                            DrawText(703, 755, "*", 50);
                            break;
                        case 4:
                            DrawText(708, 740, "/", 50);
                            break;
                    }
                    SetFillColor(218, 165, 32);
                    DrawLine(862, 755, 862, 900, 5);
                    DrawLine(618, 830, 862, 830, 5);

                    if (selectedNumbersAmount >= 4)
                    {
                        SetFillColor(148, 0, 211);
                        DrawText(1042, 740, playerSelectedNumber4.ToString(), 50);
                        switch (playerSelectedOperator4)
                        {
                            case 1:
                                DrawText(947, 735, "+", 50);
                                break;
                            case 2:
                                DrawText(952, 735, "-", 50);
                                break;
                            case 3:
                                DrawText(947, 755, "*", 50);
                                break;
                            case 4:
                                DrawText(952, 740, "/", 50);
                                break;
                        }
                        SetFillColor(218, 165, 32);
                        DrawLine(1106, 755, 1106, 900, 5);
                        DrawLine(862, 830, 1106, 830, 5);

                        if (selectedNumbersAmount >= 5)
                        {
                            SetFillColor(148, 0, 211);
                            DrawText(1286, 740, playerSelectedNumber5.ToString(), 50);
                            switch (playerSelectedOperator5)
                            {
                                case 1:
                                    DrawText(1191, 735, "+", 50);
                                    break;
                                case 2:
                                    DrawText(1196, 735, "-", 50);
                                    break;
                                case 3:
                                    DrawText(1191, 755, "*", 50);
                                    break;
                                case 4:
                                    DrawText(1196, 740, "/", 50);
                                    break;
                            }
                            SetFillColor(218, 165, 32);
                            DrawLine(1350, 755, 1350, 830, 5);
                            DrawLine(1106, 830, 1350, 830, 5);
                        }
                    }
                }
            }
        }
    }

    static void IntermediateCalculations()
    {
        if (selectedNumbersAmount >= 1)
        {
            switch (playerSelectedOperator1)
            {
                case 1:
                    intermediateCalculations1 = initialNumber + playerSelectedNumber1;
                    DrawText(130, 830, intermediateCalculations1.ToString(), 50);
                    break;
                case 2:
                    intermediateCalculations1 = initialNumber - playerSelectedNumber1;
                    DrawText(130, 830, intermediateCalculations1.ToString(), 50);
                    break;
                case 3:
                    intermediateCalculations1 = initialNumber * playerSelectedNumber1;
                    DrawText(130, 830, intermediateCalculations1.ToString(), 50);
                    break;
                case 4:
                    intermediateCalculations1 = initialNumber / playerSelectedNumber1;
                    DrawText(130, 830, intermediateCalculations1.ToString(), 50);
                    break;
            }
            if (selectedNumbersAmount >= 2)
            {
                switch (playerSelectedOperator2)
                {
                    case 1:
                        intermediateCalculations2 = intermediateCalculations1 + playerSelectedNumber2;
                        DrawText(374, 830, intermediateCalculations2.ToString(), 50);
                        break;
                    case 2:
                        intermediateCalculations2 = intermediateCalculations1 - playerSelectedNumber2;
                        DrawText(374, 830, intermediateCalculations2.ToString(), 50);
                        break;
                    case 3:
                        intermediateCalculations2 = intermediateCalculations1 * playerSelectedNumber2;
                        DrawText(374, 830, intermediateCalculations2.ToString(), 50);
                        break;
                    case 4:
                        intermediateCalculations2 = intermediateCalculations1 / playerSelectedNumber2;
                        DrawText(374, 830, intermediateCalculations2.ToString(), 50);
                        break;
                }
                if (selectedNumbersAmount >= 3)
                {
                    switch (playerSelectedOperator3)
                    {
                        case 1:
                            intermediateCalculations3 = intermediateCalculations2 + playerSelectedNumber3;
                            DrawText(618, 830, intermediateCalculations3.ToString(), 50);
                            break;
                        case 2:
                            intermediateCalculations3 = intermediateCalculations2 - playerSelectedNumber3;
                            DrawText(618, 830, intermediateCalculations3.ToString(), 50);
                            break;
                        case 3:
                            intermediateCalculations3 = intermediateCalculations2 * playerSelectedNumber3;
                            DrawText(618, 830, intermediateCalculations3.ToString(), 50);
                            break;
                        case 4:
                            intermediateCalculations3 = intermediateCalculations2 / playerSelectedNumber3;
                            DrawText(618, 830, intermediateCalculations3.ToString(), 50);
                            break;
                    }
                    if (selectedNumbersAmount >= 4)
                    {
                        switch (playerSelectedOperator4)
                        {
                            case 1:
                                intermediateCalculations4 = intermediateCalculations3 + playerSelectedNumber4;
                                DrawText(862, 830, intermediateCalculations4.ToString(), 50);
                                break;
                            case 2:
                                intermediateCalculations4 = intermediateCalculations3 - playerSelectedNumber4;
                                DrawText(862, 830, intermediateCalculations4.ToString(), 50);
                                break;
                            case 3:
                                intermediateCalculations4 = intermediateCalculations3 * playerSelectedNumber4;
                                DrawText(862, 830, intermediateCalculations4.ToString(), 50);
                                break;
                            case 4:
                                intermediateCalculations4 = intermediateCalculations3 / playerSelectedNumber4;
                                DrawText(862, 830, intermediateCalculations4.ToString(), 50);
                                break;
                        }
                        if (selectedNumbersAmount >= 5)
                        {
                            switch (playerSelectedOperator5)
                            {
                                case 1:
                                    intermediateCalculations5 = intermediateCalculations4 + playerSelectedNumber5;
                                    DrawText(1106, 830, intermediateCalculations5.ToString(), 50);
                                    break;
                                case 2:
                                    intermediateCalculations5 = intermediateCalculations4 - playerSelectedNumber5;
                                    DrawText(1106, 830, intermediateCalculations5.ToString(), 50);
                                    break;
                                case 3:
                                    intermediateCalculations5 = intermediateCalculations4 * playerSelectedNumber5;
                                    DrawText(1106, 830, intermediateCalculations5.ToString(), 50);
                                    break;
                                case 4:
                                    intermediateCalculations5 = intermediateCalculations4 / playerSelectedNumber5;
                                    DrawText(1106, 830, intermediateCalculations5.ToString(), 50);
                                    break;
                            }
                        }
                    }
                }
            }
        }
    }
    
    static void DrawTheFinalResultAndCheckForVictory()
    {
        if (selectedNumbersAmount >= 5)
        {
            if (intermediateCalculations5 != lastNumber)
            {
                SetFillColor(255, 0, 0);
                if (playDefeatSound == false) PlaySound(defeatSound);
                playDefeatSound = true;
            }
            else
            {
                SetFillColor(0, 255, 0);
                isVictory = true;
                if (awardReceived == false)
                {
                    PlaySound(winSound);
                    highscore += spentTime;
                }
                awardReceived = true;
            }
            DrawText(1350, 740, "= " + intermediateCalculations5.ToString(), 50);
            DrawLine(1350, 830, 1600, 830, 5);
            DrawLine(1350, 755, 1350, 830, 5);
        }
    }

    static void Timer()
    {
        if (isVictory == false) timer += DeltaTime;

        switch (difficulty)
        {
            case 1:
                spentTime = 60 - (int)timer;
                break;
            case 2:
                spentTime = 120 - (int)timer;
                break;
            case 3:
                spentTime = 180 - (int)timer;
                break;
        }
        if (spentTime < 0) spentTime = 0;

        SetFillColor(218, 165, 32);
        FillCircle(80, 80, 80);
        SetFillColor(192, 192, 192);
        FillCircle(80, 80, 75);
        SetFillColor(218, 165, 32);
        DrawText(15, 0, spentTime.ToString(), 100);
    }

    static void DrawPlayerScore()
    {
        SetFillColor(218, 165, 32);
        FillRectangle(1300, 0, 300, 100);
        SetFillColor(192, 192, 192);
        FillRectangle(1305, 5, 290, 90);
        SetFillColor(148, 0, 211);
        DrawText(1310, 10, "Ваш счёт: " + highscore.ToString(), 50);
    }

    static void DrawWinOrLose()
    {
        if (isVictory == true && difficulty != 3)
        {
            SetFillColor(148, 0, 211);
            FillRectangle(numbers[0, 2], numbers[0, 3], (numbersAmount / 3) * (cellSize + spaceBetweenCells) - spaceBetweenCells, 3 * cellSize + 2 * spaceBetweenCells);

            SetFillColor(218, 165, 32);
            DrawText(numbers[0, 2], numbers[0, 3], "Победа! Нажми \"Space\" для перехода", 50);
            DrawText(numbers[0, 2], numbers[0, 3] + 50, "на следующий уровень или\"Backspace\"", 50);
            DrawText(numbers[0, 2], numbers[0, 3] + 100, "для перезапуска уровня", 50);
        }

        if (isVictory == false && selectedNumbersAmount == 5)
        {

            SetFillColor(148, 0, 211);
            FillRectangle(numbers[0, 2], numbers[0, 3], (numbersAmount / 3) * (cellSize + spaceBetweenCells) - spaceBetweenCells, 3 * cellSize + 2 * spaceBetweenCells);

            SetFillColor(218, 165, 32);
            DrawText(numbers[0, 2], numbers[0, 3], "Ходы закончились! Нажми \"R\" для", 50);
            DrawText(numbers[0, 2], numbers[0, 3] + 50, "сброса цифр или \"Backspace\"", 50);
            DrawText(numbers[0, 2], numbers[0, 3] + 100, "для перезапуска уровня", 50);
        }
    }

    static void Main(string[] args)
    {

        InitWindow(1600, 900, "Find Sum");

        SetFont("Caveat-Regular.ttf");      // Установка шрифта

        while (true)
        {
            DispatchEvents();

            ClearWindow();

            PlayMusic(menuMusic, 50);
            StopMusic(level1Music);
            StopMusic(level2Music);
            StopMusic(level3Music);

            if (shutDown == true) break;        // Выход

            DisplayMenu();                      // Отрисовка меню
           
            DisplayWindow();

            Delay(1);

            if (startGame == true)
            {
                InitNumbers();      // Заполнение массива

                initialNumber = rnd.Next(2, 100);       // Генерация начального числа

                GenerateFinalNumber();                  // Генерация финального числа

                StartGameAnimation(difficulty);         // Анимация запуска

                while (true) // Начало игры
                {
                    DispatchEvents();

                    ClearWindow();

                    StopMusic(menuMusic);
                    if (difficulty != 1) StopMusic(level1Music);
                    if (difficulty != 2) StopMusic(level2Music);
                    if (difficulty != 3) StopMusic(level3Music);
                    if (difficulty == 1) PlayMusic(level1Music, 50);
                    if (difficulty == 2) PlayMusic(level2Music, 50);
                    if (difficulty == 3) PlayMusic(level3Music, 50);

                    numberIDfromMousePosition = GetIDNumberFromMousePosition();

                    Hotkeys();                          // Горячие клавиши

                    DeterminationOfSelectedNumbers();   // Запись выбранных цифр в переменные

                    if (startGame == false)             // Выход в меню
                    {
                        menuWindow = 0;
                        break;
                    }

                    DrawSprite(bgGame, 0, 0);     // Фон игры

                    DrawNumbers();                  // Отрисовка цифр

                    SetFillColor(192, 192, 192);
                    FillRectangle(0, 750, 1600, 150);       // Отрисовка подложки

                    SetFillColor(218, 165, 32);             // Отрисовка начальных линий
                    DrawLine(130, 755, 130, 900, 5);
                    DrawLine(1350, 830, 1350, 900, 5);
                    DrawLine(1350, 830, 1600, 830, 5);

                    SetFillColor(148, 0, 211);
                    DrawText(5, 750, initialNumber.ToString(), 100);                // Отрисовка первого числа
                    DrawText(1350, 830, "= " + lastNumber.ToString(), 50);          // Отрисовка финального числа

                    Timer();                                                        // Отрисовка таймера времени

                    DrawSelectedNumbers();                                          // Отрисовка выбранных цифр

                    IntermediateCalculations();                                     // Отрисовка промежуточных вычислений

                    DrawTheFinalResultAndCheckForVictory();                         // Отрисовка финального результата и проверка на победу

                    DrawPlayerScore();                                              // Отрисовка счёта игрока

                    DrawWinOrLose();                                                // Отрисовка победы/поражения и дальнейших действий

                    DisplayWindow();

                    Delay(1);
                }
            }
        }
    }
}
