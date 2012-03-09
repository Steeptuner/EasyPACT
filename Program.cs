using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyPACT
{
    class Program
    {
        static void Main(string[] args)
        {
            Liquid Liq = new Liquid(1, 0, 101325);
            Console.WriteLine(Liq.Density);
            Console.WriteLine(Liq.ViscosityDynamic);
            Console.WriteLine("{0:E2}",Liq.ViscosityKinematic);
            Console.WriteLine("Меняем Т.");
            Liq.Temperature = 10;
            
            Console.WriteLine(Liq.Density);
            Console.WriteLine(Liq.ViscosityDynamic);
            Console.WriteLine("{0:E2}", Liq.ViscosityKinematic);
        }
    }
}
