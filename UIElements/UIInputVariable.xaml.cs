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

namespace Task_in_numerical_methods.UIElements
{
    /// <summary>
    /// Логика взаимодействия для UIInputVariable.xaml
    /// </summary>

    using InputData;
    public partial class UIInputVariable : UserControl
    {
        InputData input = new InputData();
        public UIInputVariable()
        {
            InitializeComponent();
            DataContext = this;
            tText = "";
            bWidth = "60";
            wMargin = "0";
        }

        public string tText { get; set; }
        public string bWidth { get; set; }
        public string wMargin { get; set; }

        private void inputText_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox text = (TextBox)sender;
            input.validation(text);
        }
    }
}
