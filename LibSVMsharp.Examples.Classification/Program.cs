using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibSVMsharp.Helpers;
using LibSVMsharp.Extensions;
using LibSVMsharp.Classifier;
using MySqlTools;
using MySql.Data.MySqlClient;

namespace LibSVMsharp.Examples.Classification
{
    class Program
    {
        static void Main(string[] args)
        {
            ////Test
            //SVMProblem testSet = SVMProblemHelper.Load(@"Dataset\test.txt");
            //SVMClassifier svmWorker = new SVMClassifier(@"Model\train.txt.model",@"Model\train.txt.range");
            //List<double[]> probabilityList;
            //double[] testResults = svmWorker.classifyProbability(testSet,out probabilityList);
            ////// Evaluate the test results
            //int[,] confusionMatrix;
            //double testAccuracy = testSet.EvaluateClassificationProblem(testResults, svmWorker.Model.Labels, out confusionMatrix);
            //// Print the resutls
            //// Console.WriteLine("\n\nCross validation accuracy: " + crossValidationAccuracy);
            //Console.WriteLine("\nTest accuracy: " + testAccuracy);
            //Console.WriteLine("\nConfusion matrix:\n");

            //// Print formatted confusion matrix
            //Console.Write(String.Format("{0,6}", ""));
            //for (int i = 0; i < svmWorker.Model.Labels.Length; i++)
            //    Console.Write(String.Format("{0,5}", "(" + svmWorker.Model.Labels[i] + ")"));
            //Console.WriteLine();
            //for (int i = 0; i < confusionMatrix.GetLength(0); i++)
            //{
            //    Console.Write(String.Format("{0,5}", "(" + svmWorker.Model.Labels[i] + ")"));
            //    for (int j = 0; j < confusionMatrix.GetLength(1); j++)
            //        Console.Write(String.Format("{0,5}", confusionMatrix[i, j]));
            //    Console.WriteLine();
            //}
            ////////////////////////////////Test DataBase//////////////////////////////////////////
            //MySqlConnection mysqlConn = MysqlHelper.Open_Conn(MysqlHelper.ConnStr);
            SVMClassifier svmClassifier = new SVMClassifier(@"Model\train.txt.model", @"Model\train.txt.range");
            int result=svmClassifier.identifyType(new double[] {2.923466222, 2.550460114, 2.552259944, 4, 1.000705688, 1.145442191, 0.536380201, 1960.723278, 1.146250516, 0.033910685 });
            Console.WriteLine("result:"+result);
            Console.WriteLine("\nPress any key to quit...");
            Console.ReadKey(false);

        }
    }
}
