using MediathequeTP.Classes;
using System;
using System.Collections.Generic;

namespace MediathequeTP
{
    class Program
    {
        static void Main(string[] args)
        {
      
            IHMMediatheque Imedia = new IHMMediatheque();

            Imedia.Start();

            Console.ReadLine();
        }
    }
}
