using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibSVMsharp.Helpers;
using LibSVMsharp.Extensions;
using System.Globalization;

namespace LibSVMsharp.Classifier
{
    /// <summary>
    /// SVM分类器
    /// </summary>
    public class SVMClassifier
    {
        public SVMModel Model{get;set;}
        private SVMScale svmScale;
        private bool useScale = false;
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="modelPath">model的路径</param>
        /// <param name="rangFilePath">range文件的路径</param>
        public SVMClassifier(string modelPath, string rangFilePath = null)
        {
            Model = SVM.LoadModel(modelPath);
            if (rangFilePath != null)
            {
                svmScale = SVMScaleHelper.LoadRangeFile(rangFilePath);
                useScale = true;
            }
                
        }
        /// <summary>
        /// 根据特征向量识别用电器的类型
        /// </summary>
        /// <param name="vectorArray">特征向量</param>
        /// <returns>0表示未知电器，其他的数字表示相应电器的索引</returns>
        public int identifyType(double[] vectorArray)
        {
            List<double[]> estimationsList;
            SVMProblem testSet = vectorsToProblem(vectorArray);
            int preResult= (int)classifyProbability(testSet, out estimationsList)[0];
            double[] probabilities = estimationsList[0];
            double maxProb= probabilities.Max();
            return maxProb>0.4 ? preResult : 0;
        }
        /// <summary>
        /// 对测试集概率输出分类
        /// </summary>
        /// <param name="testSet">测试集</param>
        /// <param name="estimationsList">概率输出</param>
        /// <returns></returns>
        private double[] classifyProbability(SVMProblem testSet, out List<double[]> estimationsList)
        {
            if (useScale) svmScale.scaleSet(testSet);
            return testSet.PredictProbability(Model,out estimationsList);
        }
        /// <summary>
        /// 对某个测试集进行分类
        /// </summary>
        /// <param name="testSet">测试集</param>
        /// <returns></returns>
        private double[] classify(SVMProblem testSet)
        {
            if (useScale) svmScale.scaleSet(testSet);
            return testSet.Predict(Model);
        }
        private SVMProblem vectorsToProblem(double[] vectorArray)
        {
            SVMProblem problem = new SVMProblem();
            List<SVMNode> nodes = new List<SVMNode>();
            for (int i = 0; i < vectorArray.Length; i++)
            {
                SVMNode node = new SVMNode();
                node.Index = i + 1;
                node.Value = vectorArray[i];
                nodes.Add(node);
            }
            problem.Add(nodes.ToArray(),0);
            return problem;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vecterString"></param>
        /// <returns></returns>
        private SVMProblem convertStrToProblem(string vecterString)
        {
            SVMProblem problem = new SVMProblem();
            string[] list = vecterString.Trim().Split(' ');
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";

            double y = Convert.ToDouble(list[0].Trim(), provider);

            List<SVMNode> nodes = new List<SVMNode>();
            for (int i = 1; i < list.Length; i++)
            {
                string[] temp = list[i].Split(':');
                SVMNode node = new SVMNode();
                node.Index = Convert.ToInt32(temp[0].Trim());
                node.Value = Convert.ToDouble(temp[1].Trim(), provider);
                nodes.Add(node);
            }
            problem.Add(nodes.ToArray(), y);
            return problem;
        }
    }
}
