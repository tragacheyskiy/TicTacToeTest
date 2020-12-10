using System;
using System.Text.RegularExpressions;
using TicTacToeTest.Models;

namespace TicTacToeTest.Services
{
    internal class GameGrid
    {
        private static readonly Regex gridTemplate = new Regex("\\[[0-2],[0-2],[0-2],[0-2],[0-2],[0-2],[0-2],[0-2],[0-2]\\]");

        private string currentGameStatus;

        public static string EmptyGrid => "[0,0,0,0,0,0,0,0,0]";

        public static bool IsGridCorrect(string grid) => grid.Length == EmptyGrid.Length && gridTemplate.IsMatch(grid);

        private static bool IsItLinearWin(params MarkType[] line) => IsCellsEquals(line);

        private static bool IsItDiagonalWin(params MarkType[] diagonal) => IsCellsEquals(diagonal);

        private static string GetWinnerByMark(MarkType markType) => markType == MarkType.Cross ? GameStatus.CrossWon : GameStatus.ZeroWon;

        private static MarkType[] GetParsedCells(string[] cells)
        {
            MarkType[] parsedCells = new MarkType[cells.Length];

            for (int i = 0; i < cells.Length; i++)
            {
                parsedCells[i] = Enum.Parse<MarkType>(cells[i]);
            }

            return parsedCells;
        }

        private static bool IsGridFilled(MarkType[] cells)
        {
            foreach (MarkType cell in cells)
            {
                if (cell == MarkType.Clear)
                {
                    return false;
                }
            }

            return true;
        }

        private static bool IsCellsEquals(MarkType[] cells)
        {
            if (cells[0] == MarkType.Clear)
            {
                return false;
            }

            for (int i = 1; i < cells.Length; i++)
            {
                if (cells[i] != cells[0])
                {
                    return false;
                }
            }

            return true;
        }

        public string CheckGridAndGetGameStatus(string grid)
        {
            currentGameStatus = GameStatus.Goes;

            string[] cells = grid.Trim('[', ']').Split(',');
            MarkType[] parsedCells = GetParsedCells(cells);

            CheckDiagonalWin(parsedCells);
            CheckLinearWin(parsedCells);
            CheckDraw(parsedCells);

            return currentGameStatus;
        }

        private void CheckDiagonalWin(MarkType[] cells)
        {
            if (IsItDiagonalWin(cells[0], cells[4], cells[8]))
            {
                currentGameStatus = GetWinnerByMark(cells[0]);
            }

            if (IsItDiagonalWin(cells[2], cells[4], cells[6]))
            {
                currentGameStatus = GetWinnerByMark(cells[2]);
            }
        }

        private void CheckLinearWin(MarkType[] cells)
        {
            for (int i = 0, j = 0; j < 3; i += 3, j++)
            {
                if (IsItLinearWin(cells[i], cells[i + 1], cells[i + 2]))
                {
                    currentGameStatus = GetWinnerByMark(cells[i]);
                    break;
                }

                if (IsItLinearWin(cells[j], cells[j + 3], cells[j + 6]))
                {
                    currentGameStatus = GetWinnerByMark(cells[j]);
                    break;
                }
            }
        }

        private void CheckDraw(MarkType[] cells)
        {
            if (IsGridFilled(cells))
            {
                currentGameStatus = GameStatus.Draw;
            }
        }
    }
}
