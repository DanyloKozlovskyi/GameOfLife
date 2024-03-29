﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
// 17: 12
namespace GameOfLife
{
    public partial class Form1 : Form
    {
        private Graphics _graphics;
        private int _resolution;
        private GameEngine _gameEngine;

        public Form1()
        {
            InitializeComponent();
        }

        private void StartGame()
        {
            if (timer1.Enabled)
                return;

            nudResolution.Enabled = false;
            nudDensity.Enabled = false;
            _resolution = (int)nudResolution.Value;

            _gameEngine = new GameEngine
            (
                rows: pictureBox1.Height / _resolution,
                cols: pictureBox1.Width / _resolution,
                density: (int)nudDensity.Value
            );

            Text = $"Generation {_gameEngine.CurrentGeneration}";

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            _graphics = Graphics.FromImage(pictureBox1.Image);
            timer1.Start();
        }

        private void StopGame()
        {
            if (!timer1.Enabled)
                return;

            timer1.Stop();
            nudResolution.Enabled = true;
            nudDensity.Enabled = true;
        }

        private void DrawNextGeneration()
        {
            _graphics.Clear(Color.Black);

            var field = _gameEngine.GetCurrentGeneration();

            for (int x = 0; x < field.GetLength(0); x++)
                for (int y = 0; y < field.GetLength(1); y++)
                    if (field[x, y])
                        _graphics.FillRectangle(Brushes.Crimson, x * _resolution, y * _resolution,
                            _resolution - 1, _resolution - 1);
                
            pictureBox1.Refresh();
            Text = $"Generation {_gameEngine.CurrentGeneration}";
            _gameEngine.NextGeneration();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DrawNextGeneration();
        }

        private void bStop_Click(object sender, EventArgs e)
        {
            StopGame();
        }

        private void bStart_Click(object sender, EventArgs e)
        {
            StartGame();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!timer1.Enabled)
                return;

            if (e.Button == MouseButtons.Left)
            {
                var x = e.Location.X / _resolution;
                var y = e.Location.Y / _resolution;
                _gameEngine.AddCell(x, y);
            }

            if (e.Button == MouseButtons.Right)
            {
                var x = e.Location.X / _resolution;
                var y = e.Location.Y / _resolution;
                _gameEngine.RemoveCell(x, y);
            }
        }
    }
}
