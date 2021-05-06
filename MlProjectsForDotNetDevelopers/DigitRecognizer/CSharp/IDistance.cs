using System;
using System.Collections.Generic;
using System.Text;

namespace Csharp_DigitRecognizer
{
    public interface IDistance
    {
        double Between(int[] pixels1, int[] pixels2);
    }
}
