using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Forms;

namespace GameOfLife
{
    internal class GameEngine
    {
        public uint CurrentGeneration { get; private set; }
        private bool[,] _field;
        private readonly int _rows;
        private readonly int _cols;
        private Random _random = new Random();

        public GameEngine(int rows, int cols, int density)
        {
            _rows = rows;
            _cols = cols;

            _field = new bool[_cols, _rows];
            for (int x = 0; x < _cols; x++)
            {
                for (int y = 0; y < _rows; y++)
                {
                    _field[x, y] = _random.Next(density) == 0;
                }
            }
        }
        private int CountNeighbours(int x, int y)
        {
            int count = 0;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    int col = (x + i + _cols) % _cols;
                    int row = (y + j + _rows) % _rows;

                    bool isSelfChecking = col == x && row == y;
                    bool hasLife = _field[col, row];

                    if (hasLife && !isSelfChecking)
                        count++;
                }
            }
            return count;
        }

        public void NextGeneration()
        {
            // _graphics.Clear(Color.Black);

            bool[,] newField = new bool[_cols, _rows];

            for (int x = 0; x < _cols; x++)
            {
                for (int y = 0; y < _rows; y++)
                {
                    var neighboursCount = CountNeighbours(x, y);
                    var hasLife = _field[x, y];

                    if (!hasLife && neighboursCount == 3)
                        newField[x, y] = true;
                    else if (hasLife && (neighboursCount < 2 || neighboursCount > 3))
                        newField[x, y] = false;
                    else
                        newField[x, y] = _field[x, y];

                    /*if (hasLife)
                        // subtract 1 in height and width to get border
                        _graphics.FillRectangle(Brushes.Crimson, x * _resolution, y * _resolution,
                            _resolution - 1, _resolution - 1);*/
                }
            }
            _field = newField;
            /*pictureBox1.Refresh();
            Text = $"Generation {++_currentGeneration}";*/
        }

        public bool[,] GetCurrentGeneration()
        {
            bool[,] result = new bool[_cols, _rows];
            for (int x = 0; x < _cols; x++)
            {
                for (int y = 0; y < _rows; y++)
                {
                    result[x, y] = _field[x, y];
                }
            }

            return result;
        }

        private bool ValidateCellPosition(int x, int y)
        {
            return x >= 0 && y >= 0 && x < _cols && y < _rows;
        }

        private void UpdateCell(int x, int y, bool state)
        {
            if (ValidateCellPosition(x, y))
                _field[x, y] = state;
        }

        public void AddCell(int x, int y)
        {
            UpdateCell(x, y, state: true);
        }

        public void RemoveCell(int x, int y)
        {
            UpdateCell(x, y, state: false);
        }
    }
}
