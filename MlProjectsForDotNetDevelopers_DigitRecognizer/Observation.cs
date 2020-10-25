using System;
using System.Collections.Generic;
using System.Text;

namespace Csharp_DigitRecognizer
{
    class Observation
    {
        public Observation(string label, int[] pixels)
        {
            this.Label = label;
            this.Pixels = pixels;
        }

        public string Label { get; private set; }
        public int[] Pixels { get; private set; }
    }
}
