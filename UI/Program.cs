using System;

namespace UI
{
    public class A
    {
        public int a { get; set; }
        public int b { get; set; }

    }
    
    class Program
    {
        static void Main(string[] args)
        {
            A a = new A { a = 2, b = 3 };
            Console.WriteLine(a.a+" "+a.b);
            F(a);
            Console.WriteLine(a.a + " " + a.b);
            Console.ReadKey();
        }

        public static void F(A a)
        {
            a.a = 5;
            a.b = 1;
        }
    }

}
