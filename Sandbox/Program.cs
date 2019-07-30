using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox
{
    class Program
    {
        public class CA
        {
        }

        public class CB : CA
        {
        }

        static void Main(string[] args)
        {
            var blist = new List<CB>();
            var alist = (IList<CA>)blist;

            blist.Add(new CB());

            Console.WriteLine(alist[0]);
        }
    }
}
