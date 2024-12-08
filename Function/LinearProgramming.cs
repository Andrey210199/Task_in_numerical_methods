using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LinearProgramming
{
    struct XSimTab
    {
      public Dictionary<string, double> x;
      public Dictionary<string, List<double>> table;
    }

    class LinearProgramming
    {
        private XSimTab simTable(double[,] a, double[,] b, double[] fx)
        {
            int col = a.GetLength(1);
            int colA = a.GetLength(0) + col;
            int row = a.GetLength(0);
            XSimTab simT = new XSimTab();
            simT.table = new Dictionary<string, List<double>>();
            simT.x = new Dictionary<string, double>();
            simT.table.Add("b", new List<double>());
            simT.table.Add("fx", new List<double>());

            for(int i = 0; i<colA; i++)
            {  
                if(i < fx.Length)
                {
                    simT.table["fx"].Add(-1 * fx[i]);
                    simT.x.Add($"x{i+1}", 0);
                }   
                else
                {
                    simT.table["fx"].Add(0);
                }
                   
                simT.table.Add($"x{i}", new List<double>());

                for(int j = 0; j<row; j++)
                {
                    if (i<1)
                    {
                        simT.table["b"].Add(b[j, 0]);
                    }

                    //Только для <= в системе уравнений
                    if(i< col)
                    {
                        simT.table[$"x{i}"].Add(a[j, i]);
                    }
                    else
                    {
                        double bA = (i - col) == j ? 1 : 0;
                        simT.table[$"x{i}"].Add(bA);
                    }
                   
                }
            }
            simT.table["fx"].Add(0);
            return simT;
        }

        //Только для max
        private int calcMaxFx(XSimTab simTab)
        {
            int index = -1;
            double preE = 0;

            for(int i = 1; i< simTab.table["fx"].Count; i++)
            {
                if(simTab.table["fx"][i] < 0)
                {
                    index = Math.Abs(simTab.table["fx"][i]) > preE ? i : index;
                    preE = simTab.table["fx"][i];
                }
            }
            return index;
        }

        private Dictionary<string, double> calcMainElem(XSimTab simTab, int mainColIndex)
        {
            Dictionary<string, double> mainCREX = new Dictionary<string, double>();
            mainCREX.Add("C", mainColIndex);
            double preE = 0;
            double preO = -1;

            for(int i = 0; i< simTab.table[$"x{mainColIndex}"].Count; i++)
            {
                double now = simTab.table[$"x{mainColIndex}"][i];
                double nowO = now != 0 ? simTab.table["b"][i]/now : -1;
               
                if(nowO < 0)
                    continue;

                if(nowO < preO || preO == -1)
                {
                    mainCREX["E"] = now;
                    mainCREX["R"] = i;
                    mainCREX[$"X"] = nowO;
                }

                preE = now;
                preO = nowO;
                
            }

            return mainCREX;
        }

        private XSimTab calcNextSimTab(XSimTab curSimTab, Dictionary<string, double> mainCREX)
        {
            XSimTab newSimTab = new XSimTab();
            newSimTab.table = new Dictionary<string, List<double>>();
            newSimTab.x = new Dictionary<string, double>();
            string mainC = $"x{mainCREX["C"]}";
            string curX = $"x{mainCREX["C"]+1}";

            foreach(var item in curSimTab.x)
            {
                newSimTab.x[item.Key] = item.Value;
            }
            newSimTab.x[curX] = mainCREX["X"];


            foreach (var item in curSimTab.table)
            {
                newSimTab.table.Add(item.Key, new List<double>());
                int iC = item.Value.Count;
                for(int i = 0; i < iC; i++)
                {
                    if(i == mainCREX["R"])
                    {
                        newSimTab.table[item.Key].Add(curSimTab.table[item.Key][i]/mainCREX["E"]);
                    }
                   else if(item.Key == "fx")
                    {
                        double t = curSimTab.table[i< iC-1 ? $"x{i}":"b"][(int)mainCREX["R"]] * curSimTab.table["fx"][(int)mainCREX["C"]];
                        newSimTab.table[item.Key].Add(curSimTab.table[item.Key][i] - t / mainCREX["E"]);
                    }
                    else
                    {
                        double t = curSimTab.table[item.Key][(int)mainCREX["R"]] * curSimTab.table[mainC][i];
                        newSimTab.table[item.Key].Add(curSimTab.table[item.Key][i] - t / mainCREX["E"]);
                    }
                }
            }

            return newSimTab;
        }

        private XSimTab simMet(XSimTab curSimTab)
        {
            XSimTab simTab = curSimTab;
            double minFx = simTab.table["fx"].Min();
            int i = 0;
            
            while(minFx < 0)
            {
                int mainCal = calcMaxFx(simTab);
                var mainCREX = calcMainElem(simTab, mainCal);
                simTab = calcNextSimTab(curSimTab, mainCREX);
               
                if(i == 100000000)
                    { break; }
                minFx = simTab.table["fx"].Min();
                i++;
            }

            return simTab;
        }

        private Dictionary<string, double> simRes(XSimTab simTab)
        {
            var simRes = new Dictionary<string, double>();
            simRes.Add("max", simTab.table["fx"][simTab.table["fx"].Count - 1]); //только для max

            foreach (var item in simTab.x)
            {
                simRes.Add(item.Key, item.Value);
            }

            return simRes;
        }

        public Dictionary<string, double> calc(double[,] a, double[,] b, double[] fx)
        {
            var simTab = simTable(a,b,fx);
            var resSimTab = simMet(simTab);
            return simRes(resSimTab);
        }
    }
}
