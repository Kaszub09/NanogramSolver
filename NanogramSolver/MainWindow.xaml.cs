using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Microsoft.Win32;

namespace NanogramSolver
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int x, y;
        static Brush brush1 = System.Windows.Media.Brushes.Black;
        static Brush brushGreen = System.Windows.Media.Brushes.DarkGreen;
        static Brush brush0 = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/images/x.png")));
        Rectangle[,] rectList;
        Nanogram nano;
        List<int[,]> solutions;
        int currentSoultionIndex;

        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void InitializeGrid()
        {
            currentSoultionIndex = 0;
            rectList = new Rectangle[x, y];
            boardGrid.Children.Clear();
            boardGrid.RowDefinitions.Clear();
            boardGrid.ColumnDefinitions.Clear();
            for (int i = 0; i <= x; i++)
            {
                boardGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            }
            for (int i = 0; i <= y; i++)
            {
                boardGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            }

            for (int j = 0; j < y; j++)
            {
                for (int i = 0; i < x; i++)
                {

                    Rectangle rect = new Rectangle();
                    Grid.SetRow(rect, i);
                    Grid.SetColumn(rect, j);
                    boardGrid.Children.Add(rect);
                    rectList[i, j] = rect;

                }
            }

            List<int[]> row = nano.GetRows();
            List<int[]> col = nano.GetCols();
            for (int j = 0; j < y; j++)
            {
                Rectangle rect = new Rectangle();
                rect.Fill = brushGreen;
                foreach (int a in col[j])
                    rect.ToolTip += a.ToString()+"\n";
                Grid.SetRow(rect, x);
                Grid.SetColumn(rect, j);
                boardGrid.Children.Add(rect);
            }
            for (int i = 0; i < x; i++)
            {
                Rectangle rect = new Rectangle();
                rect.Fill = brushGreen;
                foreach (int a in row[i])
                    rect.ToolTip += a.ToString() + " ";
                Grid.SetRow(rect, i);
                Grid.SetColumn(rect, y);
                boardGrid.Children.Add(rect);
            }

        }

        private void DisableButtons()
        {
            solveButton.IsEnabled = false;
            nextSolutionButton.IsEnabled = false;
            chooseFileButton.IsEnabled = false;
        }

        private void EnableButtons()
        {
            solveButton.IsEnabled = true;
            nextSolutionButton.IsEnabled = true;
            chooseFileButton.IsEnabled = true;
        }

        private async void solveButton_Click(object sender, RoutedEventArgs e)
        {

            if (!nano.FinishedSolving())
            {
                if ((bool)solveForAll.IsChecked)
                    nano.allSolutions = true;
                else
                    nano.allSolutions = false;
                DisableButtons();
                timeSolvedTextBox.Text = "Solving...";
                var watch = System.Diagnostics.Stopwatch.StartNew();
                await Task.Run(() => nano.Solve());
                timeSolvedTextBox.Text = watch.Elapsed.ToString();
                solutions = nano.GetAllSolutions();
                nextSolutionButton_Click(sender, e);
                EnableButtons();
            }
        }

        private void nextSolutionButton_Click(object sender, RoutedEventArgs e)
        {
            if (solutions.Any())
            {
                currentSoultionIndex = (currentSoultionIndex + 1) % solutions.Count;
                for (int i = 0; i < x; i++)
                {
                    for (int j = 0; j < y; j++)
                    {
                        if (solutions[currentSoultionIndex][i, j] == 1)
                        {
                            rectList[i, j].Fill = brush1;
                        }
                        else if (solutions[currentSoultionIndex][i, j] == 0)
                        {
                            rectList[i, j].Fill = brush0;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < x; i++)
                    for (int j = 0; j < y; j++)
                            rectList[i, j].Fill = brush1;
                timeSolvedTextBox.Text = "It's impossible to solve!";
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            timeSolvedTextBox.Text = "Solving...";
            var watch = System.Diagnostics.Stopwatch.StartNew();
            nano.SolveAsMuchPossibleWithoutGuessing();
            timeSolvedTextBox.Text = watch.Elapsed.ToString();
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    if (nano.board[i, j] == 1)
                    {
                        rectList[i, j].Fill = brush1;
                    }
                    else if (nano.board[i, j] == 0)
                    {
                        rectList[i, j].Fill = brush0;
                    }
                }
            }
        }

        

        private void chooseFileButton_Click(object sender, RoutedEventArgs e)
        {
           OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "Txt files (*.txt) | *.txt"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                nano = new Nanogram(openFileDialog.FileName);
                x = nano.GetNumberOfRows();
                y = nano.GetNumberOfColumns();
                InitializeGrid();
                boardGrid.Height = x;
                boardGrid.Width = y;
            }
        }
    }
}
