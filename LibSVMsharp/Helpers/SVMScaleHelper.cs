using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;

namespace LibSVMsharp.Helpers
{
    public static class SVMScaleHelper
    {
       // private double lower = -1.0, upper = 1.0;
        public static SVMScale LoadRangeFile(string filename)
        {
            double lower = -1.0, upper = 1.0,y_lower=-1,y_upper=1,y_min=0,y_max=0;
            bool yIsScale = false;
            Dictionary<int, double> featureMin = new Dictionary<int, double>();
            Dictionary<int, double> featureMax = new Dictionary<int, double>();
            if (String.IsNullOrWhiteSpace(filename) || !File.Exists(filename))
            {
                return null;
            }
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";
            using (StreamReader sr = new StreamReader(filename))
            {
                string line = sr.ReadLine();
                if (line.Trim().Equals("y"))
                {
                    yIsScale = true;
                    line = sr.ReadLine();
                    string[] ylu = line.Trim().Split(' ');
                    y_lower = Convert.ToDouble(ylu[0].Trim(), provider);
                    y_upper = Convert.ToDouble(ylu[1].Trim(), provider);
                    line = sr.ReadLine();
                    string[] yMinMax = line.Trim().Split(' ');
                    y_min = Convert.ToDouble(yMinMax[0].Trim(), provider);
                    y_max = Convert.ToDouble(yMinMax[0].Trim(), provider);
                }
                if (line.Trim().Equals("x"))
                {
                    line = sr.ReadLine();
                    string[] xlu = line.Trim().Split(' ');
                    lower = Convert.ToDouble(xlu[0].Trim(), provider);
                    upper = Convert.ToDouble(xlu[1].Trim(), provider);
                }
                while (true)
                {
                    line = sr.ReadLine();
                    if (line == null) break;
                    string[] list = line.Trim().Split(' ');
                    int label = Convert.ToInt32(list[0]);
                    double featureLower = Convert.ToDouble(list[1].Trim(), provider);
                    double featureUpper = Convert.ToDouble(list[2].Trim(), provider);
                    featureMin.Add(label, featureLower);
                    featureMax.Add(label, featureUpper);
                }
            }
            SVMScale svmScale = new SVMScale(featureMin, featureMax,yIsScale);
            svmScale.setLowerAndUpper(lower, upper);
            if (yIsScale) svmScale.setYParams(y_lower, y_upper,y_min,y_max);
            return svmScale;
        }
    }
}
