using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace InputData
{
    class InputData
    {
        public double[,] fillMatrix(StackPanel name, int row, int collumn)
        {
            double[,] matrix = new double[row, collumn];

            foreach (var item in name.Children)
            {
                if (item is TextBox)
                {
                    TextBox n = (TextBox)item;
                    string[] arrN = n.Name.Split("_");
                    int start = int.Parse(arrN[arrN.Length - 2]);
                    int end = int.Parse(arrN[arrN.Length - 1]);
                    double text = 0;
                    double.TryParse(n.Text, out text);

                    matrix[start, end] = text;
                }
            }

            return matrix;
        }

    }
}
