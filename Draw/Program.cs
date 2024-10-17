class Program
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
    static void EditDrawing()
    {
        while (true)
        {
            DrawScreen();
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
    static void DisplayMenu()
    {
        Console.Clear();
        ConsoleKeyInfo keyInfo;
        ConsoleKey key;
        int selectedOption = 1;
        bool optionSelected = false;

        int menuWidth = 20;
        int menuHeight = 5;
        int menuX = (Console.WindowWidth - menuWidth) / 2;
        int menuY = (Console.WindowHeight - menuHeight) / 2;
        do
        {
            Console.SetCursorPosition(menuX, menuY);
            Console.WriteLine(selectedOption == 1 ? "> Új rajz" : "  Új rajz");
            Console.SetCursorPosition(menuX, menuY + 1);
            Console.WriteLine(selectedOption == 2 ? "> Betöltés" : "  Betöltés");
            Console.SetCursorPosition(menuX, menuY + 2);
            Console.WriteLine(selectedOption == 3 ? "> Fájl törlése" : "  Fájl törlése");
            Console.SetCursorPosition(menuX, menuY + 3);
            Console.WriteLine(selectedOption == 4 ? "> Kilépés" : "  Kilépés");
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
                LoadExistingDrawing();
                break;
            case 3:
                DeleteDrawing();
                break;
            case 4:
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

    int selectedOption = 0;
    bool optionSelected = false;

    do
    {
        Console.Clear();
        Console.WriteLine("Válassz egy fájlt a törléshez: ");
        
        for (int i = 0; i < drawingFiles.Length; i++)
        {
            Console.WriteLine($"{(selectedOption == i ? "> " : "  ")}{Path.GetFileNameWithoutExtension(drawingFiles[i])}");
        }

        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
        ConsoleKey key = keyInfo.Key;

        switch (key)
        {
            case ConsoleKey.UpArrow:
                if (selectedOption > 0)
                {
                    selectedOption--;
                }
                break;
            case ConsoleKey.DownArrow:
                if (selectedOption < drawingFiles.Length - 1)
                {
                    selectedOption++;
                }
                break;
            case ConsoleKey.Enter:
                optionSelected = true;
                break;
        }
    } while (!optionSelected);

    string selectedDrawingFile = drawingFiles[selectedOption];
    File.Delete(selectedDrawingFile);
    Console.WriteLine($"A fájl sikeresen törölve: {selectedDrawingFile}");
    Console.WriteLine("Nyomj meg egy gombot a folytatáshoz...");
    Console.ReadKey();
}
    static void LoadExistingDrawing()
{
    string[] drawingFiles = Directory.GetFiles(".", "*.txt");

    if (drawingFiles.Length == 0)
    {
        Console.WriteLine("Nem található ilyen.");
        return;
    }

    int selectedOption = 0;
    bool optionSelected = false;

    do
    {
        Console.Clear();
        Console.WriteLine("Válassz egy fájlt: ");
        
        for (int i = 0; i < drawingFiles.Length; i++)
        {
            Console.WriteLine($"{(selectedOption == i ? "> " : "  ")}{Path.GetFileNameWithoutExtension(drawingFiles[i])}");
        }

        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
        ConsoleKey key = keyInfo.Key;

        switch (key)
        {
            case ConsoleKey.UpArrow:
                if (selectedOption > 0)
                {
                    selectedOption--;
                }
                break;
            case ConsoleKey.DownArrow:
                if (selectedOption < drawingFiles.Length - 1)
                {
                    selectedOption++;
                }
                break;
            case ConsoleKey.Enter:
                optionSelected = true;
                break;
        }
    } while (!optionSelected);

    string selectedDrawingFile = drawingFiles[selectedOption];
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
    static void CreateNewDrawing()
    {
        InitScreen();
        DrawScreen();
        EditDrawing();
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
