using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace GenerateElements
{
    class GenerateElements
    {
        public void inputGenerate(int row, int collumn, StackPanel name, double[,] d, string[] headName)
        {
            int y = -45;
            int x = -row * 24;

            TextBlock a = new TextBlock();
            TextBlock b = new TextBlock();

            a.Text = headName[0];
            b.Text = headName[1];
            a.TextAlignment = TextAlignment.Right;
            b.TextAlignment = TextAlignment.Right;
            a.Margin = new Thickness(0, 0, row * (24 + 5), y / 2);
            b.Margin = new Thickness(0, 0, 15, y / 4);
            name.Children.Add(a);
            name.Children.Add(b);

            for (int i = 0; i < collumn; i++)
            {
                for (int j = 0; j < row; j++)
                {
                    TextBox text = new TextBox();
                    text.Name = $"{name.Name}_{i}_{j}";
                    text.Width = 30;
                    text.Height = 20;
                    text.Margin = new Thickness(x, 0, 0, y);
                    text.Text = $"{d[i, j]}";
                    name.Children.Add(text);
                    x += 65;
                }
                y -= 45;
                x = -row * 24;
            }
        }
    }
}
