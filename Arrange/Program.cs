using System;
using System.IO;

namespace Arrange
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] enumerator = System.IO.Directory.GetDirectories(AppDomain.CurrentDomain.BaseDirectory);
            foreach (var item in enumerator)
            {
                if (Directory.Exists(item))
                {
                    string[] nls = System.IO.Directory.GetDirectories(item);
                    foreach (var l in nls)
                    {
                        Console.WriteLine(l);
                    }
                }
            }
        }
    }
}
