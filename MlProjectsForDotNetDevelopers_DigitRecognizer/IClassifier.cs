using System;
using System.Collections.Generic;
using System.Text;

namespace Csharp_DigitRecognizer
{
    public interface IClassifier
    {
        internal void Train(IEnumerable<Observation> trainingSet);
        string Predict(int[] pixels);
    }
}
