using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibSVMsharp
{
    public class SVMScale
    {
        public double lower = -1.0, upper = 1.0,y_lower,y_upper,y_min,y_max;
        private bool yIsScale = false;
        private Dictionary<int, double> featureMin;
        private Dictionary<int, double> featureMax;
        public SVMScale(Dictionary<int, double> featureMin, Dictionary<int, double> featureMax, bool yIsScale)
        {
            this.featureMin = featureMin;
            this.featureMax = featureMax;
            this.yIsScale = yIsScale;
        }
        /// <summary>
        /// 将一个集合按照scale规则缩放
        /// </summary>
        /// <param name="problem"></param>
        public bool scaleSet(SVMProblem srcSet)
        {
            for (int i = 0; i < srcSet.Length; i++)
            {
                int label = (int)srcSet.Y[i];
                srcSet.Y[i] = scaleLabel(srcSet.Y[i]);
                SVMNode[] vectorNode = srcSet.X[i];
                for (int j = 0; j < vectorNode.Length; j++)
                {
                    double minFeature = featureMin[vectorNode[j].Index];
                    double maxFeature = featureMax[vectorNode[j].Index];
                    if (vectorNode[j].Value == minFeature)
                        vectorNode[j].Value = lower;
                    else if (vectorNode[j].Value == maxFeature)
                        vectorNode[j].Value = upper;
                    else
                        vectorNode[j].Value = lower + (upper - lower) * (vectorNode[j].Value - minFeature) / (maxFeature - minFeature);
                }
            }
            return true;
        }
        /// <summary>
        /// 对标签值得缩放，如果设置了的话
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private double scaleLabel(double value)
        {
            if (yIsScale)
            {
                if (value == y_min)
                    value = y_lower;
                else if (value == y_max)
                    value = y_upper;
                else value = y_lower + (y_upper - y_lower) *(value - y_min) / (y_max - y_min);
            }
            return value;
        }
        public void setLowerAndUpper(double l, double u){
            this.lower = l;
            this.upper = u;
        }
        public void setYParams(double yl, double yu, double yMin, double yMax)
        {
            this.y_lower = yl;
            this.y_upper = yu;
            this.y_min = yMin;
            this.y_max = yMax;
        }

    }
}
