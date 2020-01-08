using MediathequeTP.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediathequeTP
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            IHMMediatheque Imedia = new IHMMediatheque();

            Imedia.Start();

            Console.ReadLine();
        }
    }
}
