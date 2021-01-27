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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> colors;
        Button temp_button;
        int need;
        int[,] use_pos;
        Dictionary<int, Button> list;
        int size;
        int count;
        int choise_x;
        int choise_y;
        public MainWindow()
        {
            InitializeComponent();
            choise_x = -1;
            choise_y = -1;
            size = 5;
            need = 5 / 2 + 1;
            use_pos = new int[size, size];
            colors = new List<string>() {"Rounded", "Rounded2", "Rounded3" };
            list = new Dictionary<int, Button>();
            Random rnd = new Random();
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                {
                    use_pos[i, j] = 0;
                    TextBlock rt = new TextBlock();
                    rt.MouseDown += grid_main_MouseDown;
                    rt.Width = 100;
                    rt.Height = 100;
                    Grid.SetColumn(rt, i);
                    Grid.SetRow(rt, j);
                    grid_main.Children.Add(rt);
                }

            for (int i = 0; i < 3; i++)
            {
                create_button(i, rnd);
                count++;
            }
        }

        private void create_button(int num, Random rnd)
        {
            int tmp;
            int x, y;
            tmp = rnd.Next(0, colors.Count);
            Button button = new Button();
            button.Name = "button" + num;
            button.Width = 60;
            button.Height = 60;
            button.Style = this.FindResource(colors[tmp]) as Style;
            button.Click += Button_Click;
            bool t = true;
            do
            {
                x = rnd.Next(0, size);
                y = rnd.Next(0, size);
                if (use_pos[x, y] == 0)
                    t = false;
            } while (t);
            use_pos[x, y] = 1;
            list[x * 10 + y] = button;
            Grid.SetColumn(button, x);
            Grid.SetRow(button, y);
            grid_main.Children.Add(button);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            temp_button = btn;
            choise_x = Grid.GetColumn(btn);
            choise_y = Grid.GetRow(btn);
        }

        private void grid_main_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(choise_x != -1 && choise_y != -1)
            {
                TextBlock tx = (TextBlock)sender;
                int temp_x = -1, temp_y = -1;
                temp_x = Grid.GetColumn(tx);
                temp_y = Grid.GetRow(tx);
                int t_x = Grid.GetColumn(temp_button);
                int t_y = Grid.GetRow(temp_button);
                use_pos[t_x, t_y] = 0;
                list[t_x * 10 + t_y] = null;
                Grid.SetColumn(temp_button, temp_x);
                Grid.SetRow(temp_button, temp_y);
                use_pos[temp_x, temp_y] = 1;
                list[temp_x * 10 + temp_y] = temp_button;
                choise_x = -1;
                choise_y = -1;
                if (!check())
                {
                    Random rnd = new Random();
                    int temp_size = count;
                    for (int i = temp_size; i < temp_size + 3; i++)
                    {
                        create_button(i, rnd);
                        count++;
                    }
                }
            }
        }

        private bool check()
        {
            bool t = false;
            for (int i = 0; i < size - need + 1; i++)
            {
                for (int j = 0; j < size - need + 1; j++)
                {
                    if (use_pos[i, j] == 1 && use_pos[i, j + 1] == 1 && use_pos[i, j + 2] == 1)
                    {
                        Style temp_style = list[i * 10 + j].Style;
                        if (list[i * 10 + j + 1].Style == temp_style && list[i * 10 + j + 2].Style == temp_style)
                        {
                            grid_main.Children.Remove(list[i * 10 + j]);
                            grid_main.Children.Remove(list[i * 10 + j + 1]);
                            grid_main.Children.Remove(list[i * 10 + j + 2]);
                            list[i * 10 + j] = null;
                            list[i * 10 + j + 1] = null;
                            list[i * 10 + j + 2] = null;
                            t = true;
                            use_pos[i, j] = 0;
                            use_pos[i, j + 1] = 0;
                            use_pos[i, j + 2] = 0;
                        }
                    }
                    if (use_pos[i, j] == 1 && use_pos[i + 1, j] == 1 && use_pos[i + 2, j] == 1)
                    {
                        Style temp_style = list[i * 10 + j].Style;
                        if (list[(i + 1) * 10 + j].Style == temp_style && list[(i + 2) * 10 + j].Style == temp_style)
                        {
                            grid_main.Children.Remove(list[(i) * 10 + j]);
                            grid_main.Children.Remove(list[(i + 1) * 10 + j]);
                            grid_main.Children.Remove(list[(i + 2) * 10 + j]);
                            list[(i) * 10 + j] = null;
                            list[(i + 1) * 10 + j] = null;
                            list[(i + 2) * 10 + j] = null;
                            t = true;
                            use_pos[i, j] = 0;
                            use_pos[i + 1, j] = 0;
                            use_pos[i + 2, j] = 0;
                        }
                    }
                }
            }
            return t;
        }
    }
}
