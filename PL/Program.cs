using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace PL
{
    class Program
    {
        static void Main(string[] args)
        {
            //Debug.Write(new EmailAddressAttribute().IsValid(""));
            a a=new a{a=2,b=3 };
            global::System.Console.WriteLine(a.a+" "+a.b);
            func(a);
            global::System.Console.WriteLine(a.a + " " + a.b);

        }
    }
    public class a
    {
        public int a { get; set; }
        public int b { get; set; }

    }
    public void func(a a)
    {
        a.a = 5;
        a.b = 1;
    }
}
