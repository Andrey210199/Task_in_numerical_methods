using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace OutputData
{
    class OutputData
    {
        public void outputMatrixX(StackPanel name, Dictionary<string, double> res)
        {
            Label r = new Label();

            if(name.Children[name.Children.Count - 1] is Label)
             name.Children.RemoveAt(name.Children.Count-1);

            r.Margin = new Thickness(0, 100, 0, 0);
           
            foreach (var item in res)
            {
                r.Content += $"{item.Key}: {item.Value}\n";
            }
            name.Children.Add(r);
        }

        public void outputXY(TextBlock name, Dictionary<string, List<double>> data)
        {
            name.Text = "x   y\n";

            for (int i = 0; i < data["x"].Count; i++)
            {
                name.Text += $"{data["x"][i]}   {data["y"][i]}\n";
            }
        }

        public void outputDictionary<S,T>(Dictionary<S, T> res, TextBlock name)
        {
            name.Text = "Ответ:\n";
            foreach (var it in res)
            {
                name.Text += $"{it.Key}: {it.Value}\n";
            }
        }

    }
}
