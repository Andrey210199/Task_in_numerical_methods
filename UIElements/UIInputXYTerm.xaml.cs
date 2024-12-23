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
    /// Логика взаимодействия для UIInputXYTerm.xaml
    /// </summary>
    using InputData;
    public partial class UIInputXYTerm : UserControl
    {
        InputData inputData = new InputData();
        public UIInputXYTerm()
        {
            InitializeComponent();
            BeforeText = "";
            BetweenText = " ";
            AfterText = "";
            DataContext = this;
        }

        public string BeforeText { get; set; }
        public string AfterText { get; set; }
        public string BetweenText { get; set; }

        private void inputX_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox text = (TextBox)sender;
            inputData.validation(text);
        }

    }
}
