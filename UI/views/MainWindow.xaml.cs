using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System;
using System.ComponentModel;

using Core;
using MazeGenerator;


namespace UI
{
    public partial class MainWindow : Window, IWorldListener
    {
        private Simulation _sim;

        private MazeCell[] _maze;
        private const int MazeWidth = 16;
        private const int MazeHeight = 16;

        private bool _disableUIUpdates;
        public bool DisableUIUpdates
        {
            get { return _disableUIUpdates; }
            set { _disableUIUpdates = value; }
        }

        SimConfig _config = new()
        {
            Seed = 0,
            Width = 100,
            Height = 100,
            TickRate = 30
        };

        public MainWindow()
        {
            InitializeComponent();

            // Create a simulation and run it for now
            _sim = new(_config, this);

            // Generate a maze
            _maze = Program.RecursiveMaze(MazeWidth, MazeHeight);
        }

        public void OnWorldUpdate(World world)
        {
            Dispatcher.Invoke(() => {
                UpdateUI(world);
            });
        }
        private void UpdateUI(World world)
        {
            // Don't update the canvas if UI updates are disabled
            if (_disableUIUpdates) return;

            // Clear the canvas
            simCanvas.Children.Clear();

            // Draw the maze first
            DrawMaze();

            // Use the world to redraw the canvas
            var tileWidth = simCanvas.ActualWidth / world.Width;
            var tileHeight = simCanvas.ActualHeight / world.Height;

            var robot = world.GetRobot();

            // Draw the robot
            Ellipse robotRect = new();
            robotRect.Width = Robot.Size;
            robotRect.Height = Robot.Size;

            Canvas.SetLeft(robotRect, robot.PosX - Robot.Size / 2);
            Canvas.SetTop(robotRect, robot.PosY - Robot.Size / 2);

            SolidColorBrush brush = new()
            {
                Color = Color.FromArgb(128, 128, 128, 0)
            };
            robotRect.Fill = brush;

            simCanvas.Children.Add(robotRect);
        }
        private void DrawMaze()
        {   
            // Use the world to redraw the canvas
            var cellWidth = simCanvas.ActualWidth / MazeWidth;
            var cellHeight = simCanvas.ActualHeight / MazeHeight;

            SolidColorBrush brush = new()
            {
                Color = Color.FromArgb(128, 128, 128, 0)
            }; // Black colour for maze walls

            Rectangle left, right, top, bottom;
            int thickness = 2;
            MazeCell cell;
            for (int i = 0; i < _maze.Length; i++)
            {
                cell = _maze[i];

                if (cell.Left)
                {
                    left = new Rectangle();
                    left.Height = cellHeight;
                    left.Width = thickness;
                    left.Fill = brush;
                    left.StrokeThickness = 0;

                    Canvas.SetLeft(left, (i % MazeWidth) * cellWidth);
                    Canvas.SetTop(left, (i / MazeWidth) * cellHeight);

                    simCanvas.Children.Add(left);
                }
                if (cell.Right)
                {
                    right = new Rectangle();
                    right.Height = cellHeight;
                    right.Width = thickness;
                    right.Fill = brush;
                    right.StrokeThickness = 0;

                    Canvas.SetLeft(right, ((i % MazeWidth) + 1) * cellWidth - thickness);
                    Canvas.SetTop(right, (i / MazeWidth) * cellHeight);

                    simCanvas.Children.Add(right);
                }
                if (cell.Top)
                {
                    top = new Rectangle();
                    top.Height = thickness;
                    top.Width = cellWidth;
                    top.Fill = brush;
                    top.StrokeThickness = 0;

                    Canvas.SetLeft(top, (i % MazeWidth) * cellWidth);
                    Canvas.SetTop(top, (i / MazeWidth) * cellHeight);

                    simCanvas.Children.Add(top);
                }
                if (cell.Bottom)
                {
                    bottom = new Rectangle();
                    bottom.Height = thickness;
                    bottom.Width = cellWidth;
                    bottom.Fill = brush;
                    bottom.StrokeThickness = 0;

                    Canvas.SetLeft(bottom, ((i % MazeWidth)) * cellWidth);
                    Canvas.SetTop(bottom, ((i / MazeWidth) + 1) * cellHeight - thickness);

                    simCanvas.Children.Add(bottom);
                }
            }
        }

        private void btnResetSimClick(object sender, RoutedEventArgs e)
        {
            // Stop the current active simulation
            _sim.End();

            // Create the simulation again using the same config file
            _sim = new Simulation(_config, this);
        }
        private void btnStopSimClick(object sender, RoutedEventArgs e)
        {
            _sim.End();
        }

        private void btnStartSimClick(object sender, RoutedEventArgs e)
        {
            _sim.Start();
        }
        private void cbDisableUIUpdates(object sender, RoutedEventArgs e)
        {
            _disableUIUpdates = true;
        }
        private void cbEnableUIUpdates(object sender, RoutedEventArgs e)
        {
            _disableUIUpdates = false;
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            // Stop the simulation so thread resources can be released
            _sim.End();

            base.OnClosing(e);
        }
    }
}