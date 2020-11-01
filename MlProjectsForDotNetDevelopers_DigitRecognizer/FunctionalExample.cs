using System;
using System.Collections.Generic;

namespace Csharp_DigitRecognizer
{
    public class FunctionalExample
    {
        private IEnumerable<Observation> data;
        public Func<int[], int[], int> Distance { get; }

        public FunctionalExample(Func<int[], int[], int> distance)
        {
            Distance = distance;
        }

        public void Train(IEnumerable<Observation> trainingSet)
        {
            data = trainingSet;
        }

        public string Predict(int[] pixels)
        {
            Observation currentBest = null;
            var shortest = Double.MaxValue;

            foreach (Observation obs in data)
            {
                var dist = Distance(obs.Pixels, pixels);
                if (dist < shortest)
                {
                    shortest = dist;
                    currentBest = obs;
                }
            }

            return currentBest.Label;
        }
    }
}
