using Model;
using System;

namespace CSharpApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var car = new Car(
                    4,
                    "Supercars",
                    Tuple.Create(1.5, 3.5));
            
            var bike = Model.Vehicle.Motorbike.NewMotorbike(
                    "kukuBike",
                    1.0);

            var somewheeledCar = Model.Functions.CreateCar(
                    4,
                    "Supacars",
                    1.5,
                    3.5);

            // try to avoid providing partially applied functions to C#
            var fourWheeledCar =
                    Model.Functions.CreateFourWheeledCar
                        .Invoke("Supacars")
                        .Invoke(1.5)
                        .Invoke(3.5);
        }
    }
}
