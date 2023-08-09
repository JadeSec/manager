using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infra.Implementation.Filter
{
    public class Criteria
    {
        public string Name { get; set; }

        public string[] Operators { get; set; }

        public Criteria(string name, string[] operators)
        {
            Name = name;
            Operators = operators;
        }
    }
}
