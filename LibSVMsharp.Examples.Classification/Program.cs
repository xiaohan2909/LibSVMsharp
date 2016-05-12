using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibSVMsharp.Helpers;
using LibSVMsharp.Extensions;

namespace LibSVMsharp.Examples.Classification
{
    class Program
    {
        static void Main(string[] args)
        {
            //Test Scale
            //// Load the datasets: In this example I use the same datasets for training and testing which is not suggested
            SVMProblem testSet = SVMProblemHelper.Load(@"Dataset\test.txt");
            SVMModel model = SVM.LoadModel(@"Model\train.txt.model");
            SVMScale svmScale = SVMScaleHelper.LoadRangeFile(@"Model\train.txt.range");
            svmScale.scaleSet(testSet);
            //double[] testResults = testSet.Predict(model);
            List<double[]> probabilityList;
            double[] testResults = testSet.PredictProbability(model,out probabilityList);
            //// Evaluate the test results
            int[,] confusionMatrix;
            double testAccuracy = testSet.EvaluateClassificationProblem(testResults, model.Labels, out confusionMatrix);
            // Print the resutls
            // Console.WriteLine("\n\nCross validation accuracy: " + crossValidationAccuracy);
            Console.WriteLine("\nTest accuracy: " + testAccuracy);
            Console.WriteLine("\nConfusion matrix:\n");

            // Print formatted confusion matrix
            Console.Write(String.Format("{0,6}", ""));
            for (int i = 0; i < model.Labels.Length; i++)
                Console.Write(String.Format("{0,5}", "(" + model.Labels[i] + ")"));
            Console.WriteLine();
            for (int i = 0; i < confusionMatrix.GetLength(0); i++)
            {
                Console.Write(String.Format("{0,5}", "(" + model.Labels[i] + ")"));
                for (int j = 0; j < confusionMatrix.GetLength(1); j++)
                    Console.Write(String.Format("{0,5}", confusionMatrix[i, j]));
                Console.WriteLine();
            }

            Console.WriteLine("\n\nPress any key to quit...");
            Console.ReadKey(false);

        }
    }
}
