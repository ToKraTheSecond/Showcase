using Csharp_DigitRecognizer;
using System;

namespace DigitRecognizer
{
    class Program
    {
        static void Main(string[] args)
        {
            var trainingPath = "data/trainingsample.csv";
            var training = DataReader.ReadObservations(trainingPath);

            Console.ReadLine();
        }
    }
}
