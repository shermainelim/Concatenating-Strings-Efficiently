using System;
using System.Text;

namespace Concat__vs_StringBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            //String Concat
            DateTime start = DateTime.Now;
            string x = "";
            for (int i = 0; i < 100000; i++)
            {
                x += "!";
            }
            DateTime end = DateTime.Now;
            Console.WriteLine("Time taken for String Concat: {0}", end - start);


            // String Builder
            DateTime start1 = DateTime.Now;
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < 100000; i++)
            {
                builder.Append("!");
            }
            string x1 = builder.ToString();
            DateTime end1 = DateTime.Now;
            Console.WriteLine("Time taken for String Builder: {0}", end1 - start1);
        }
    }
}
