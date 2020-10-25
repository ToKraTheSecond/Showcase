using System;

namespace CSharpProject
{
    public class Person
    {
        public string Name { get; private set; }
        public Person(string name)
        {
            Name = name;
        }
        public void PrintName()
        {
            Console.WriteLine($"My name is {Name}");
        }
    }
}
