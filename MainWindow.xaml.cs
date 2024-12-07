using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;



namespace Task_in_numerical_methods
{
    using OxyPlot;
    using OxyPlot.Series;
    using ZnachFunk;
    using NonlinearEquation;
    using Slau;
    using SlauIteration;
    using SlauRun;
    using SNU;
    using Polinom;
    using InterSqr;
    using LessSqruare;
    using NumIntegration;
    using FiniteDiffMethod;
    using FiniteDifferent;
    using LinearProgramming;

    using System.Windows.Automation.Text;
    using OxyPlot.Wpf;

    using GenerateElements;
    using InputData;
    using OutputData;
    using System.Xml.Linq;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public required PlotModel GraphModel { get; set; }
        public required PlotModel InterGr { get; set; }
        public required PlotModel InterSqGr { get; set; }
        public required PlotModel LessSqGr { get; set; }
        public required PlotModel EulerGr { get; set; }
        public required PlotModel MEulerGr { get; set; }
        public required PlotModel RKutGr { get; set; }


        public MainWindow()
        {
            InitializeComponent();
            response.Text = "";
        }

        GenerateElements generate = new GenerateElements();
        InputData input = new InputData();
        OutputData output = new OutputData();

        private PlotModel CreatePlotModel(Dictionary<string, List<double>> points)
        {
            DataContext = null;
            var plotModel = new PlotModel();
            var verticalAxis = new LineSeries()
            {
                Title = $"Series 1",
                Color = OxyPlot.OxyColors.Blue,
                StrokeThickness = 1,
            };

            for (int i = 0; i < points["x"].Count; i++)
            {
                verticalAxis.Points.Add(new DataPoint(points["x"][i], points["y"][i]));
            }
            plotModel.Series.Add(verticalAxis);
            plotModel.InvalidatePlot(true);
            return plotModel;
        }

        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string? tabItem = ((sender as TabControl)?.SelectedItem as TabItem)?.Header as string;

            switch (tabItem)
            {
                case "СЛАУ":
                    if (slauInput.Children.Count == 0)
                    {
                        double[,] d = { { 3, 2, 1, 1, 1 }, { 3, 3, 2, 2, -1 }, { 3, 3, 3, 4, -4 }, { 3, 3, 3, 5, -5 } };
                        double[,] f = { { 3, 1, -1, 1, 0 }, { 1, -4, 1, -1, 0 }, { -1, 1, 4, 1, 0 }, { 1, 2, 1, -5, 0 } };
                        double[,] r = { { -7, 2, 0, 0, -5 }, { 1, -12, -4, 0, -8 }, { 0, -1, 6, -1, -2 }, { 0, 0, 3, 5, 4 } };
                        string[] headNames = { "A", "B" };
                        string[] bValue = { "3m", "m-6", "15-m", "m+2"};
                        generate.inputGenerate(5, 4, slauInput, d, headNames);
                        generate.inputGenerate(5, 4, slauFaultInput, f, headNames);
                        generate.inputGenerate(5, 4, slauRunInput, r, headNames);

                        foreach (var item in slauFaultInput.Children)
                        {
                            if (item is TextBox)
                            {
                                TextBox n = (TextBox)item;

                                if (n.Name.LastIndexOf('4') == n.Name.Length - 1)
                                {
                                    int i = n.Name[n.Name.Length - 3] -'0';
                                    n.Text = bValue[i];
                                    n.IsEnabled = false;
                                    n.Width = 40;
                                }

                            }

                        }

                    }

                    break;

                case "Интерполяция":
                    if (InterInput.Children.Count == 0)
                    {
                        double[,] inter = { { -2, -3 }, { 1, 0 }, { 2, -2 } };
                        double[,] interSqr = { { 1, 1.2 }, { 1.2, 2 }, { 1.4, 3.2 }, { 1.6, 4.2 }, { 1.8, 4.8 }, { 2, 6.1 } };
                        double[,] lessSqr = { { -1, 6 }, { 0, 1 }, { 1, -4 }, { 2, -9 } };
                        string[] headName = { "x", "y" };
                        generate.inputGenerate(2, 3, InterInput, inter, headName);
                        generate.inputGenerate(2, 6, InterSqInput, interSqr, headName);
                        generate.inputGenerate(2, 4, LessSqInput, lessSqr, headName);
                    }
                    break;
                case "Линейное программирование":
                    double[,] a = { { 1, 2 }, {-5,3}, {4,6} };
                    double[,] b = { {14}, {15}, {24} };
                    generate.inputGenerate(2, 3, LinearProgA, a, ["A", ""]);
                    for(int i = 0; i<3; i++)
                    {
                        TextBlock text = new TextBlock();
                        text.Text = "<=";
                        text.Margin = new Thickness(-10, 12,0,0);
                        LinearProgO.Children.Add(text);
                    }
                    generate.inputGenerate(1, 3, LinearProgB, b, ["", "B"]);
                    break;
            }

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            response.Text = "";

            string[] fName= [ "F", "Fmax", "Fmin", "Абсолютная погрешность", "Относительная погрешность" ];

            double x = double.Parse(xValue.Text);
            double y = - double.Parse(yValue.Text);
            double xF = double.Parse(xFault.Text);
            double yF = double.Parse(yFault.Text);
            double m = double.Parse(mValue.Text);
            double k = double.Parse(kValue.Text);

            ZnachFunk znach = new ZnachFunk(x, y, xF, yF, m, k);
            Dictionary<string, double> resp = znach.Caclulate();

            foreach (var item in resp)
            {
                response.Text += $"{item.Key}: {item.Value}\n";
            }

        }
       
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            var noonLinear = new NonlinearEquation();

            double n = double.Parse(sqrtVal.Text);
            double m = double.Parse(mVal.Text);
            double c = double.Parse(cVal.Text);
            double fault = double.Parse(faultVal.Text);

            var points = noonLinear.calculate(n,m,c, fault);
            sqrtLiniar.Text += $" {points["x"].Last()}";
            GraphModel = CreatePlotModel(points);
            DataContext = this;
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            double[,] matrix = input.fillMatrix(slauInput, 4, 5);
            Slau slau= new Slau();
            var res = slau.calc(matrix, 4);

            output.outputMatrixX(slauInput, res);
        }

        private void slauFaultRes_Click(object sender, RoutedEventArgs e)
        {
            double[,] matrix = input.fillMatrix(slauFaultInput, 4, 5);
            double m = double.Parse(ver.Text);
            double fault = double.Parse(slauFF.Text);
            double x1 = double.Parse(slauFX1.Text)*m;
            double x2 = double.Parse(slauFX2.Text);
            double x3 = double.Parse(slauFX3.Text);
            double x4 = double.Parse(slauFX4.Text);

            matrix[0, 4] = 3 * m;
            matrix[1,4] = m-6;
            matrix[2,4] = 15-m;
            matrix[3,4] = m+2;

            SlauIteration slauI = new SlauIteration(matrix, fault, x1, x2, x3, x4);

            var res = slauI.calc();
            output.outputMatrixX(slauFaultInput, res);
        }

        private void slauRunRes_Click(object sender, RoutedEventArgs e)
        {
            
            double[,] matrix = input.fillMatrix(slauRunInput, 4, 5);
            SlauRun slau = new SlauRun();

            var res = slau.calc(matrix);
            output.outputMatrixX(slauRunInput, res);
            
        }

        private void SnuRes_Click(object sender, RoutedEventArgs e)
        {
            double fault = double.Parse(SnuFault.Text);
            Dictionary<string, double> item = new Dictionary<string, double>();
            item.Add("m",double.Parse(SnuM.Text));
            item.Add("m1", double.Parse(SnuM1.Text));
            item.Add("n", double.Parse(SnuN.Text));
            item.Add("n1", double.Parse(SnuN1.Text));
            SNU sNU = new SNU();
            Dictionary<string, double> res = sNU.calc(fault, item);
            output.outputDictionary(res, SnuResText);

        }

        private void InterRes_Click(object sender, RoutedEventArgs e)
        {
            double[,] matrix = input.fillMatrix(InterInput, 4, 2);
            Polinom pol = new Polinom(matrix);
            Dictionary<string, List<double>> res = pol.calc();
            output.outputXY(interResT, res);
            InterGr = CreatePlotModel(res);
            DataContext = this;
        }

        private void InterSqRes_Click(object sender, RoutedEventArgs e)
        {
            double[,] matrix = input.fillMatrix(InterSqInput, 6, 2);
            double foundX = 1.1;
            InterSqr interSqr = new InterSqr(matrix,foundX);
            Dictionary<string, List<double>> res = interSqr.calc();
            InterSqResT.Text = $"При x {res["x"][1]}, y равно {res["y"][1]}";
            InterSqGr = CreatePlotModel(res);
            DataContext = this;
        }

        private void LessSqRes_Click(object sender, RoutedEventArgs e)
        {
            double[,] matrix = input.fillMatrix(LessSqInput, 4, 2);
            LessSqruare less = new LessSqruare();
            Dictionary<string, List<double>> res = less.calc(matrix);

            output.outputXY(LessSqResT, res);
            LessSqGr = CreatePlotModel(res);
            DataContext = this;
        }

        private void NumInterButton_Click(object sender, RoutedEventArgs e)
        {
            int n = int.Parse(NumInterN.Text);
            double c = double.Parse(NumInterC.Text);
            double a = double.Parse(NumInterA.Text);
            double b = double.Parse(NumInterB.Text);
            double m = double.Parse(NumInterM.Text);
            double m1 = double.Parse(NumInterM1.Text);
            double[] abcmm1 = { a, b, c, m, m1 };

            NumIntegration numIntegration = new NumIntegration();
            Dictionary<string, double> res = numIntegration.calc(n, abcmm1);
            output.outputDictionary(res, NumInterResT);
        }

        private void FDifferentBRes_Click(object sender, RoutedEventArgs e)
        {
            double m = double.Parse(FDifferentM.Text);
            double c = double.Parse(FDifferentC.Text);
            double xs = double.Parse(FDifferentXS.Text);
            double xe = double.Parse(FDifferentXE.Text);
            double y = double.Parse(FDifferentYS.Text);
            double h = double.Parse(FDifferentH.Text);

            FiniteDiffMethod method = new FiniteDiffMethod(h, [xs,xe], y, [m,c]);
            var euler = method.calcEuler();
            var mEuler = method.calcModEuler();
            var rKut = method.calcRungeKut();
            output.outputXY(EulerResT, euler);
            output.outputXY(MEulerResT, mEuler);
            output.outputXY(RKutResT, rKut);

            EulerGr = CreatePlotModel(euler);
            MEulerGr = CreatePlotModel(mEuler);
            RKutGr = CreatePlotModel(rKut);

            DataContext = this;
        }

        private void SecondTimeResB_Click(object sender, RoutedEventArgs e)
        {
            double m = double.Parse(SecondTimeM.Text);
            double c = double.Parse(SecondTimeC.Text);
            double x = double.Parse(SecondTimeX.Text);
            double y = double.Parse (SecondTimeY.Text);
            double x1 = double.Parse(SecondTimeX1.Text);
            double y1 = double.Parse(SecondTimeY1.Text);
            double h = double.Parse(SecondTimeH.Text);

            FiniteDifferent finite = new FiniteDifferent();
            double[,] xyArr = x>x1? new double[,] { { x,y }, { x1, y1 } } : new double[,] { { x1,y1 }, { x, y } };
            var res = finite.calc([m,c], xyArr, h);
            output.outputXY(SecondTimeResT, res);
        }

        private void LinearProgResB_Click(object sender, RoutedEventArgs e)
        {
            var a = input.fillMatrix(LinearProgA, 3, 2);
            var b = input.fillMatrix(LinearProgB, 3, 1);
            double[] fXIndex = [1, 2];
            LinearProgramming lin = new LinearProgramming();
            var res = lin.calc(a, b, fXIndex);
            output.outputDictionary(res, LinearProgResT);
        }
    }
}