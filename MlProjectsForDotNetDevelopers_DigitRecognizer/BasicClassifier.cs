using System;
using System.Collections.Generic;
using System.Text;

namespace Csharp_DigitRecognizer
{
    public class BasicClassifier : IClassifier
    {
        internal IEnumerable<Observation> data;
        private readonly IDistance distance;
        public BasicClassifier(IDistance distance)
        {
            this.distance = distance;
        }
        void IClassifier.Train(IEnumerable<Observation> trainingSet)
        {
            this.data = trainingSet;
        }
        public string Predict(int[] pixels)
        {
            Observation currentBest = null;
            var shortest = Double.MaxValue;
            foreach (Observation obs in this.data)
            {
                var dist = this.distance.Between(obs.Pixels,
                pixels);
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
