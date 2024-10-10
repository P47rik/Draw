﻿class Program
{
    static char[,] screen = new char[25, 80];
    static ConsoleColor[,] screenColors = new ConsoleColor[25, 80];
    static int cursorX = 0, cursorY = 0;
    static ConsoleColor currentColor = ConsoleColor.White;
    static string currentChar = "█";
    static ConsoleColor cursorColor = ConsoleColor.White;

    static void InitScreen()
    {
        for (int y = 0; y < 25; y++)
        {
            for (int x = 0; x < 80; x++)
            {
                screen[y, x] = ' ';
                screenColors[y, x] = ConsoleColor.Black;
            }
        }
    }

    static void DrawScreen()
    {
        Console.SetCursorPosition(0, 0);
        for (int y = 0; y < 25; y++)
        {
            for (int x = 0; x < 80; x++)
            {
                Console.SetCursorPosition(x, y);
                Console.ForegroundColor = screenColors[y, x];
                Console.Write(screen[y, x]);
            }
        }
        Console.SetCursorPosition(cursorX, cursorY);
        Console.BackgroundColor = cursorColor;
        Console.Write(" ");
        Console.ResetColor();
        Console.SetCursorPosition(cursorX, cursorY);
        Console.CursorVisible = false;
    }

    static void DisplaySettings()
    {
        Console.SetCursorPosition(0, 25);
        Console.WriteLine("Color: " + currentColor);
        Console.WriteLine("Cursor: " + cursorX + ", " + cursorY);
        Console.WriteLine("Character: " + currentChar);
        Console.WriteLine("Cursor Color: " + cursorColor);
    }

    static void Backspace()
    {
        if (cursorX > 0)
        {
            cursorX--;
            screen[cursorY, cursorX] = ' ';
            screenColors[cursorY, cursorX] = ConsoleColor.Black;
            DrawScreen();
        }
    }

    static void MoveCursor(int dx, int dy)
    {
        int newX = cursorX + dx;
        int newY = cursorY + dy;
        if (newX >= 0 && newX < 80 && newY >= 0 && newY < 25)
        {
            cursorX = newX;
            cursorY = newY;
        }
    }

    static void DrawChar(string c, ConsoleColor color)
    {
        for (int i = 0; i < c.Length; i++)
        {
            if (cursorX + i < 80)
            {
                screen[cursorY, cursorX + i] = c[i];
                screenColors[cursorY, cursorX + i] = color;
            }
        }
    }

    static void SetColor(ConsoleColor color)
    {
        currentColor = color;
        Console.ForegroundColor = color;
    }

    static void DisplayMenu()
    {
        Console.Clear();
        ConsoleKeyInfo keyInfo;
        ConsoleKey key;
        int selectedOption = 1;
        bool optionSelected = false;

        int menuWidth = 20;
        int menuHeight = 6;
        int menuX = (Console.WindowWidth - menuWidth) / 2;
        int menuY = (Console.WindowHeight - menuHeight) / 2;

        for (int y = menuY - 1; y <= menuY + menuHeight; y++)
        {
            Console.SetCursorPosition(menuX - 1, y);
            Console.Write("|");
            Console.SetCursorPosition(menuX + menuWidth, y);
            Console.Write("|");
        }
        Console.SetCursorPosition(menuX - 1, menuY - 1);
        Console.Write("+");
        Console.SetCursorPosition(menuX + menuWidth, menuY - 1);
        Console.Write("+");
        Console.SetCursorPosition(menuX - 1, menuY + menuHeight);
        Console.Write("+");
        Console.SetCursorPosition(menuX + menuWidth, menuY + menuHeight);
        Console.Write("+");

        do
        {
            Console.SetCursorPosition(menuX, menuY);
            Console.WriteLine(selectedOption == 1 ? "> Új rajz" : "  Új rajz");
            Console.SetCursorPosition(menuX, menuY + 1);
            Console.WriteLine(selectedOption == 2 ? "> Mentés" : "  Mentés");
            Console.SetCursorPosition(menuX, menuY + 2);
            Console.WriteLine(selectedOption == 3 ? "> Betöltés" : "  Betöltés");
            Console.SetCursorPosition(menuX, menuY + 3);
            Console.WriteLine(selectedOption == 4 ? "> Fájl törlése" : "  Fájl törlése");
            Console.SetCursorPosition(menuX, menuY + 4);
            Console.WriteLine(selectedOption == 5 ? "> Kilépés" : "  Kilépés");
            keyInfo = Console.ReadKey(true);
            key = keyInfo.Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    if (selectedOption > 1)
                    {
                        selectedOption--;
                    }
                    break;
                case ConsoleKey.DownArrow:
                    if (selectedOption < 5)
                    {
                        selectedOption++;
                    }
                    break;
                case ConsoleKey.Enter:
                    optionSelected = true;
                    break;
            }
        } while (!optionSelected);

        switch (selectedOption)
        {
            case 1:
                CreateNewDrawing();
                break;
            case 2:
                SaveDrawing();
                break;
            case 3:
                LoadExistingDrawing();
                break;
            case 4:
                DeleteDrawing();
                break;
            case 5:
                Environment.Exit(0);
                break;
        }
    }

    static void DeleteDrawing()
    {
        string[] drawingFiles = Directory.GetFiles(".", "*.txt");

        if (drawingFiles.Length == 0)
        {
            Console.WriteLine("Nem található ilyen.");
            return;
        }
        Console.WriteLine("Válassz egy fáljt a törléshez: ");

        for (int i = 0; i < drawingFiles.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {Path.GetFileNameWithoutExtension(drawingFiles[i])}");
        }

        Console.Write("Válassz egy rajzot:  ");
        string input = Console.ReadLine();

        string[] selectedDrawingIndices = input.Split(',');

        foreach (string index in selectedDrawingIndices)
        {
            if (int.TryParse(index, out int selectedDrawingIndex) && selectedDrawingIndex >= 1 && selectedDrawingIndex <= drawingFiles.Length)
            {
                string selectedDrawingFile = drawingFiles[selectedDrawingIndex - 1];
                File.Delete(selectedDrawingFile);
                Console.WriteLine($"A fájl sikeresen törölve: {selectedDrawingFile}");
            }
            else
            {
                Console.WriteLine($"Nem található vagy hibás: {index}");
            }
        }
    }

    static void LoadExistingDrawing()
    {
        string[] drawingFiles = Directory.GetFiles(".", "*.txt");

        if (drawingFiles.Length == 0)
        {
            Console.WriteLine("Nem található ilyen.");
            return;
        }
        Console.WriteLine("Válassz egy fáljt: ");

        for (int i = 0; i < drawingFiles.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {Path.GetFileNameWithoutExtension(drawingFiles[i])}");
        }

        Console.Write("Válassz egy rajzot:  ");
        string input = Console.ReadLine();

        string[] selectedDrawingIndices = input.Split(',');

        foreach (string index in selectedDrawingIndices)
        {
            if (int.TryParse(index, out int selectedDrawingIndex) && selectedDrawingIndex >= 1 && selectedDrawingIndex <= drawingFiles.Length)
            {
                string selectedDrawingFile = drawingFiles[selectedDrawingIndex - 1];
                string[] lines = File.ReadAllLines(selectedDrawingFile);

                InitScreen();

                for (int y = 0; y < Math.Min(lines.Length, 25); y++)
                {
                    for (int x = 0; x < Math.Min(lines[y].Length, 80); x++)
                    {
                        screen[y, x] = lines[y][x];
                        screenColors[y, x] = ConsoleColor.White;
                    }
                }

                DrawScreen();
                EditDrawing();
            }
            else
            {
                Console.WriteLine($"Nem található vagy hibás: {index}");
            }
        }
    }

    static void CreateNewDrawing()
    {
        InitScreen();
        DrawScreen();
        EditDrawing();
    }

    static void EditDrawing()
    {
        while (true)
        {
            DrawScreen();
            DisplaySettings();

            ConsoleKeyInfo originalKeyInfo = Console.ReadKey(true);
            ConsoleKey originalKey = originalKeyInfo.Key;

            switch (originalKey)
            {
                case ConsoleKey.Backspace:
                    Backspace();
                    break;
                case ConsoleKey.UpArrow:
                    MoveCursor(0, -1);
                    break;
                case ConsoleKey.DownArrow:
                    MoveCursor(0, 1);
                    break;
                case ConsoleKey.LeftArrow:
                    MoveCursor(-1, 0);
                    break;
                case ConsoleKey.RightArrow:
                    MoveCursor(1, 0);
                    break;
                case ConsoleKey.Spacebar:
                    DrawChar(currentChar, currentColor);
                    break;
                case ConsoleKey.D0:
                    SetColor(ConsoleColor.DarkBlue);
                    break;
                case ConsoleKey.D1:
                    SetColor(ConsoleColor.Red);
                    break;
                case ConsoleKey.D2:
                    SetColor(ConsoleColor.Green);
                    break;
                case ConsoleKey.D3:
                    SetColor(ConsoleColor.Yellow);
                    break;
                case ConsoleKey.D4:
                    SetColor(ConsoleColor.Blue);
                    break;
                case ConsoleKey.D5:
                    SetColor(ConsoleColor.Magenta);
                    break;
                case ConsoleKey.D6:
                    SetColor(ConsoleColor.Cyan);
                    break;
                case ConsoleKey.D7:
                    SetColor(ConsoleColor.DarkGreen);
                    break;
                case ConsoleKey.D8:
                    SetColor(ConsoleColor.DarkYellow);
                    break;
                case ConsoleKey.D9:
                    SetColor(ConsoleColor.DarkRed);
                    break;
                case ConsoleKey.NumPad1:
                    currentChar = "█";
                    break;
                case ConsoleKey.NumPad2:
                    currentChar = "▓";
                    break;
                case ConsoleKey.NumPad3:
                    currentChar = "▒";
                    break;
                case ConsoleKey.NumPad4:
                    currentChar = "░";
                    break;
                case ConsoleKey.Escape:
                    SaveDrawing();
                    return;
            }
        }
    }
    static void SaveDrawing()
    {
        Console.Write("Enter the file name to save the drawing: ");
        string? fileName = Console.ReadLine();
        if (string.IsNullOrEmpty(fileName))
        {
            Console.WriteLine("Invalid file name.");
            return;
        }
        string filePath = fileName + ".txt";
        string[] lines = new string[25];
        for (int y = 0; y < 25; y++)
        {
            string line = "";
            for (int x = 0; x < 80; x++)
            {
                line += screen[y, x];
            }
            lines[y] = line;
        }
        File.WriteAllLines(filePath, lines);
        Console.WriteLine("Drawing saved successfully!");
    }
    static void Main(string[] args)
    {
        InitScreen();
        while (true)
        {
            DisplayMenu();
        }
    }
}