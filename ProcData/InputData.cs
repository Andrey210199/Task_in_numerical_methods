using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace InputData
{
    using OutputData;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Media;
    using Windows.ApplicationModel.VoiceCommands;

    class ValidateVariables
    {
       public bool isEnable;
       public List<double> variables = new List<double>();
    }

    class FillVar
    {
        public bool isEnable;
        public Dictionary<string, double> variables = new Dictionary<string, double>();
    }

    class InputData
    {
        public void validation(TextBox elem)
        {
            OutputData outD = new OutputData();
            elem.Text = string.Join(",", elem.Text.Split("."));
            elem.SelectionStart = elem.Text.Length;
            double res = 0;
            bool textB = double.TryParse(elem.Text, out res);
            if (!textB && elem.Text != "" && elem.Foreground != Brushes.Red)
            {
                elem.Foreground = Brushes.Red;
            }
            else if(textB && elem.Foreground != Brushes.Black)
            {
                elem.Foreground = Brushes.Black;
            }
                
        }

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

        public ValidateVariables buttonBlocked(StackPanel stackName, Button buttonName)
        {
            var stack = new Queue<DependencyObject>();
            stack.Enqueue(stackName);
            bool bVisible = true;
            ValidateVariables validate = new ValidateVariables();

            while (stack.Count > 0)
            {
                var item = stack.Dequeue();

                foreach (var i in LogicalTreeHelper.GetChildren(item))
                {  
                    if(!(i is String))
                     stack.Enqueue((DependencyObject)i);
                }

                if (item is TextBox)
                {
                    TextBox n = (TextBox)item;
                    if (n.Foreground == Brushes.Red || n.Text == "")
                    {
                        TextBlock fault = new TextBlock();
                        fault.Foreground = Brushes.Red;
                        fault.Text = "Введены не числа или поля не заполнены.";
                        fault.HorizontalAlignment = HorizontalAlignment.Center;
                        fault.Margin = new Thickness(5);
                        stackName.Children.Add(fault);
                        bVisible = false;
                        break;
                    }
                    else
                    {
                        double variable = double.Parse(n.Text);
                        validate.variables.Add(variable);
                    }

                }
            }
                if(bVisible)
                {
                    int stackItem = stackName.Children.Count - 1;
                    if (stackName.Children[stackItem] is TextBlock)
                    {
                        stackName.Children.RemoveAt(stackItem);
                    }
                    buttonName.IsEnabled = true;
                    
                }
                validate.isEnable = bVisible;
                return validate;            
        }

        public FillVar fillVariable(object sender, string[] variableName)
        {
            FillVar fillVar = new FillVar();
            Button but = (Button)sender;
            var isEnable = buttonBlocked((StackPanel)but.Parent, but);
            fillVar.isEnable = isEnable.isEnable;

            for (int i = 0; i < isEnable.variables.Count; i++)
            {
                fillVar.variables.Add(variableName[i], isEnable.variables[i]);
            }

            return fillVar;
        }

        public bool isButtonBlock(StackPanel stackName, Button buttonName)
        {
            var stack = new Queue<DependencyObject>();
            stack.Enqueue(stackName);
            bool bVisible = true;

            while (stack.Count > 0)
            {
                var item = stack.Dequeue();

                foreach (var i in LogicalTreeHelper.GetChildren(item))
                {
                    if (!(i is String))
                        stack.Enqueue((DependencyObject)i);
                }

                if (item is TextBox)
                {
                    TextBox n = (TextBox)item;
                    if (n.Foreground == Brushes.Red || n.Text == "")
                    {
                        TextBlock fault = new TextBlock();
                        fault.Foreground = Brushes.Red;
                        fault.Text = "Введены не числа или поля не заполнены.";
                        fault.HorizontalAlignment = HorizontalAlignment.Center;
                        fault.Margin = new Thickness(5);
                        stackName.Children.Add(fault);
                        bVisible = false;
                        break;
                    }
                    else
                    {
                        double variable = double.Parse(n.Text);
                    }

                }
            }
            if (bVisible)
            {
                int stackItem = stackName.Children.Count - 1;
                if (stackName.Children[stackItem] is TextBlock)
                {
                    stackName.Children.RemoveAt(stackItem);
                }
                buttonName.IsEnabled = true;

            }

            return bVisible;
        }

    }
}
