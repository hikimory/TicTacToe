using System;

namespace Tic_Tac_Toe
{
    internal class Program
    {
        private static Point[] _coord;
        private static char[] _board;
        private static bool _isGameOver;
        private static int _currentIndex;
        private static int _lastIndex;
        private static int _turn;
        private static int[][] _winningCombination;
        private static PlayerMark _currentMark;
        private static PlayerMark _winnerMark;

        static void Main(string[] args)
        {
            Play();
        }

        private static void Play()
        {
            Initialization();
            PrintEnvironment();
            do
            {
                CheckButtonInfo();
                PrintMark(_currentIndex, _currentMark);
            } while (!_isGameOver);
            PrintWinner();
        }

        private static void Initialization()
        {
            _currentIndex = 4; // Center of the _board
            _lastIndex = 4; // Center of the _board
            _currentMark = PlayerMark.X_Player;
            _winnerMark = PlayerMark.None;
            _isGameOver = false;
            _turn = 0;

            // _board
            _board = new char[9];
            for (int i = 0; i < 9; i++)
            {
                _board[i] = '-';
            }

            // Points
            int num = 0;
            _coord = new Point[9];
            for (int i = 14; i < 21; i += 3)
            {
                for (int j = 22; j < 35; j += 6)
                {
                    _coord[num] = new Point(j, i);
                    num++;
                }
            }

            //Combinations
            _winningCombination = new int[8][];
            _winningCombination[0] = new int[3] { 0, 1, 2 }; // Check first row
            _winningCombination[1] = new int[3] { 3, 4, 5 }; // Check second Row
            _winningCombination[2] = new int[3] { 6, 7, 8 }; // Check third Row
            _winningCombination[3] = new int[3] { 0, 3, 6 }; // Check first column
            _winningCombination[4] = new int[3] { 1, 4, 7 }; // Check second Column
            _winningCombination[5] = new int[3] { 2, 5, 8 }; // Check third Column
            _winningCombination[6] = new int[3] { 0, 4, 8 }; // Check first Diagonal
            _winningCombination[7] = new int[3] { 2, 4, 6 }; // Check second Diagonal
        }

        private static void PrintMark(int offset, PlayerMark mark)
        {
            //Print previous mark
            Point lastPoint = _coord[_lastIndex];
            Console.SetCursorPosition(lastPoint.X, lastPoint.Y);
            Console.Write(_board[_lastIndex]);

            //Print current mark
            Point point = _coord[offset];
            Console.SetCursorPosition(point.X, point.Y);
            PrintWithColors(ConsoleColor.White, ConsoleColor.Red,
                () => { Console.Write(mark.ToSymbol()); });
            _lastIndex = _currentIndex;
        }

        private static PlayerMark GetMark()
        {
            return _turn % 2 == 0 ? PlayerMark.X_Player : PlayerMark.O_Player;
        }

        private static void CheckButtonInfo()
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    MakeMove(-3);
                    break;
                case ConsoleKey.DownArrow:
                    MakeMove(3);
                    break;
                case ConsoleKey.LeftArrow:
                    MakeMove(-1);
                    break;
                case ConsoleKey.RightArrow:
                    MakeMove(1);
                    break;
                case ConsoleKey.Enter:
                    if (SetValue(_currentIndex, _currentMark))
                    {
                        _isGameOver = IsGameOver();
                        _currentMark = GetMark();
                        UpdatePlayerInfo();
                    }
                    break;
                default:
                    break;
            }
        }

        private static void MakeMove(int offset)
        {
            if (offset == -1 && _currentIndex % 3 == 0) return; // Check left side
            if (offset == 1 && _currentIndex % 3 == 2) return; // Check right side
            if (offset == 3 && _currentIndex >= 6) return; // Check bottom side
            if (offset == -3 && _currentIndex <= 2) return; // Check uo side
            _currentIndex += offset;
        }

        private static bool SetValue(int offset, PlayerMark mark)
        {
            if (_board[offset] != '-') return false;
            _board[offset] = mark.ToSymbol();
            _turn++;
            _currentIndex = 4;
            return true;
        }

        private static bool IsGameOver()
        {
            return IsWin(PlayerMark.X_Player) || IsWin(PlayerMark.O_Player) || IsDraw();
        }

        private static bool IsWin(PlayerMark mark)
        {
            char c = mark.ToSymbol();
            for (int i = 0; i < _winningCombination.Length; i++)
            {
                if (_board[_winningCombination[i][0]] == c &&
                    _board[_winningCombination[i][1]] == c &&
                    _board[_winningCombination[i][2]] == c)
                {
                    _winnerMark = mark;
                    return true;
                }
            }
            return false;
        }

        private static bool IsDraw()
        {
            if(_turn == 9)
            {
                _winnerMark = PlayerMark.Draw;
                return true;
            }
            return false;
        }

        private static void PrintWinner()
        {
            string xWinner = """
              ██     ██               ██     ██ ██  ███     ██  ███     ██  ███████  ██████  
               ██   ██                ██     ██ ██  ████    ██  ████    ██  ██       ██   ██ 
                 ███       ██████     ██  █  ██ ██  ██  ██  ██  ██  ██  ██  █████    ██████  
               ██   ██                ██ ███ ██ ██  ██   ██ ██  ██   ██ ██  ██       ██   ██ 
              ██     ██                ███ ███  ██  ██    ████  ██    ████  ███████  ██   ██ 
              """;

            string oWinner = """
               ███████                ██     ██ ██  ███     ██  ███     ██  ███████  ██████  
              ██     ██               ██     ██ ██  ████    ██  ████    ██  ██       ██   ██ 
              ██     ██    ██████     ██  █  ██ ██  ██  ██  ██  ██  ██  ██  █████    ██████  
              ██     ██               ██ ███ ██ ██  ██   ██ ██  ██   ██ ██  ██       ██   ██ 
               ███████                 ███ ███  ██  ██    ████  ██    ████  ███████  ██   ██ 
              """;

            string draw = """
              ██████    ██████    █████   ██       ██ 
              ██    ██  ██   ██  ██   ██  ██       ██ 
              ██    ██  ██████   ███████  ██   █   ██ 
              ██    ██  ██   ██  ██   ██  ██  ███  ██ 
              ██████    ██   ██  ██   ██    ███ ███                              
              """;

            Console.Clear();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;

            switch (_winnerMark)
            {
                case PlayerMark.X_Player:
                    Console.WriteLine(xWinner);
                    break;
                case PlayerMark.O_Player:
                    Console.WriteLine(oWinner);
                    break;
                case PlayerMark.Draw:
                    Console.WriteLine(draw);
                    break;
                default:
                    break;
            }

            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void SetColorPalette()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void PrintHeader()
        {
            string header = """
               ██████████ ██   ██████        ██████████     ██       ██████        ██████████   ███████   ████████
              ░░░░░██░░░ ░██  ██░░░░██      ░░░░░██░░░     ████     ██░░░░██      ░░░░░██░░░   ██░░░░░██ ░██░░░░░ 
                  ░██    ░██ ██    ░░           ░██       ██░░██   ██    ░░           ░██     ██     ░░██░██      
                  ░██    ░██░██        █████    ░██      ██  ░░██ ░██        █████    ░██    ░██      ░██░███████ 
                  ░██    ░██░██       ░░░░░     ░██     ██████████░██       ░░░░░     ░██    ░██      ░██░██░░░░  
                  ░██    ░██░░██    ██          ░██    ░██░░░░░░██░░██    ██          ░██    ░░██     ██ ░██      
                  ░██    ░██ ░░██████           ░██    ░██     ░██ ░░██████           ░██     ░░███████  ░████████
                  ░░     ░░   ░░░░░░            ░░     ░░      ░░   ░░░░░░            ░░       ░░░░░░░   ░░░░░░░░ 
              """;

            Console.WriteLine();
            Console.WriteLine(header);
        }

        private static void DisplayBoard()
        {
            Console.SetCursorPosition(20, 13);
            Console.WriteLine("     |     |      ");
            Console.SetCursorPosition(20, 14);
            Console.WriteLine("  {0}  |  {1}  |  {2}", _board[0], _board[1], _board[2]);
            Console.SetCursorPosition(20, 15);
            Console.WriteLine("_____|_____|_____ ");
            Console.SetCursorPosition(20, 16);
            Console.WriteLine("     |     |      ");
            Console.SetCursorPosition(20, 17);
            Console.WriteLine("  {0}  |  {1}  |  {2}", _board[3], _board[4], _board[5]);
            Console.SetCursorPosition(20, 18);
            Console.WriteLine("_____|_____|_____ ");
            Console.SetCursorPosition(20, 19);
            Console.WriteLine("     |     |      ");
            Console.SetCursorPosition(20, 20);
            Console.WriteLine("  {0}  |  {1}  |  {2}", _board[6], _board[7], _board[8]);
            Console.SetCursorPosition(20, 21);
            Console.WriteLine("     |     |      ");
        }

        private static void PrintRules()
        {
            Console.SetCursorPosition(50, 13);
            Console.WriteLine("Press the Left Arrow to go Left");
            Console.SetCursorPosition(50, 15);
            Console.WriteLine("Press the Right Arrow to go Right");
            Console.SetCursorPosition(50, 17);
            Console.WriteLine("Press the Up Arrow to go Up");
            Console.SetCursorPosition(50, 19);
            Console.WriteLine("Press the Down Arrow to go Down");
            Console.SetCursorPosition(50, 21);
            Console.WriteLine("Press the Enter Arrow to Set Mark");
        }

        private static void PrintPlayerInfo()
        {
            Console.SetCursorPosition(30, 25);
            Console.Write("The ");
            PrintWithColors(ConsoleColor.White, ConsoleColor.Red, 
                    () => { Console.Write($"{_currentMark.ToSymbol()}"); });
            Console.Write("-player is walking now");
        }

        private static void UpdatePlayerInfo()
        {
            Console.SetCursorPosition(34, 25);
            PrintWithColors(ConsoleColor.White, ConsoleColor.Red,
                    () => { Console.Write($"{_currentMark.ToSymbol()}"); });
        }

        private static void PrintEnvironment()
        {
            SetColorPalette();
            PrintHeader();
            DisplayBoard();
            PrintRules();
            PrintPlayerInfo();
            PrintMark(_currentIndex, _currentMark);
        }

        private static void PrintWithColors(ConsoleColor prev, ConsoleColor curr, Action act)
        {
            Console.ForegroundColor = curr;
            act();
            Console.ForegroundColor = prev;
        }
    }
}