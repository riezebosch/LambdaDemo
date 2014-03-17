using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LambdaDemo
{
    delegate int MyDelegate(string input);
    delegate TResult MyDelegate<TInput, TResult>(TInput input);
    delegate TResult MyDelegate<T1, T2, TResult>(T1 input, T2 input);

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            MyDelegate d = new MyDelegate(Convert);
            int result = d.Invoke("1");
            Assert.AreEqual(1, result);

            d("2");

            // Verkorte schrijfwijze voor initializatie
            MyDelegate d2 = Convert;

        }

        private static int Convert(string number)
        {
            return int.Parse(number);
        }


        #region Demo2

        delegate void Print(string input);

        [TestMethod]
        public void TestMulticastDelegate()
        {
            Print p = PrintToConsole;
            p += Console.WriteLine;

            p("hier een tekstje");
        }

        private static void PrintToConsole(string input)
        {
            Console.WriteLine(input);
        }
        #endregion

        [TestMethod]
        public void AnonymousDelegate()
        {
            // tussenstatpje tussen .NET 1.1 en .NET 3.0
            // Dit is een anonymous method, maar wordt door
            // de compiler gewoon in een aparte methode gestopt!
            Print p = delegate(string input) { Console.WriteLine(input); };
            p("input");

        }

        [TestMethod]
        public void TestDelegateAsParameter()
        {
            Execute(delegate(string s) { Console.WriteLine(s); });
        }

        private static void Execute(Print p)
        {
            p("tekst");
        }

        [TestMethod]
        public void TestDelegateWithOuterVariable()
        {
            string input = "Goedemorgen";
            Execute(delegate(string s) { Console.WriteLine(s + input); });

        }

        class OuterVariablePlaceholder
        {
            public string input;
            public void Method(string s)
            {
                Console.WriteLine(s + input);
            }
        }

        [TestMethod]
        public void TestDelegateWithOuterVariableCompilerRewritten()
        {
            var placeholder = new OuterVariablePlaceholder();
            placeholder.input = "Goedemorgen";
            Execute(placeholder.Method);

        }

        [TestMethod]
        public void DelegateSomeNotations()
        {
            Execute(PrintToConsole);
            Execute(delegate(string s) { Console.WriteLine(s); });
            Execute(s => Console.WriteLine(s));
            Execute((string s) => Console.WriteLine(s));
            Execute((string s) => { Console.WriteLine(s); });
        }

        [TestMethod]
        public void DelegatesWithGenerics()
        {
            MyDelegate<int, double> d = ConvertIntToDouble;
        }

        private static double ConvertIntToDouble(int input)
        {
            return (double)input;
        }
    }
}
