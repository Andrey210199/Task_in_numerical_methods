using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ZnachFunk
{
    class ZnachFunk
    {
        Dictionary<string, double> param = new Dictionary<string, double>();


        public ZnachFunk(double x, double y, double xF, double yF, double m, double k)
        {

            param["x"] = x;
            param["y"] = y;
            param["xMax"] = x + xF;
            param["xMin"] = x - xF;
            param["yMax"] = y + (yF/100 *y);
            param["yMin"] = y - yF/100 * y;
            param["m"] = m;
            param["k"] = k;

        }
        
        public Dictionary<string, double> Caclulate()
        {
            Dictionary<string, double> resp = new Dictionary<string, double>();

            double funck = param["m"] * Math.Exp(param["x"]) + Math.Pow(param["k"], param["y"]);
            double funckMax = param["m"] * Math.Exp(param["xMax"]) + Math.Pow(param["k"], param["yMax"]);
            double funckMin = param["m"] * Math.Exp(param["xMin"]) + Math.Pow(param["k"], param["yMin"]);
            double absFault = ((funck - funckMin)+(funckMax - funck))/2*100;
            double otnFault = (absFault/funck);
            resp["F"] = funck;
            resp["Fmin"] = funckMin< funckMax ? funckMin: funckMax;
            resp["Fmax"] = funckMax > funckMin ? funckMax : funckMin;
            resp["AbsFault"] = Math.Abs(absFault);
            resp["OtnFault"] = Math.Abs(otnFault);

            return resp;
            
        }


    }
}
