using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infra.CrossCutting.Parsers
{
    public interface IOperator
    {

    }

    public class Or: IOperator
    {
        public string Value { get; private set; }

        public Or(string a, string b)
        {
            Value = a;
        }
    }   

    public class SearchFilterParser
    {
        public string Attribute { get; set; }

        public IOperator Value { get; set; }

        public SearchFilterParser(string attr, IOperator oper)
        {

        }

        public enum Operators
        {            
            OR,            
            IN,
            AND,            
            LIKE,
            EQUAL,
            RANGE,
            BETWEEN,           
            LESS_THAN,
            GREATER_THAN
        }
    }

    public class SearchParser
    {
        public static SearchParser Parser(string value)
        {
            var k = new SearchFilterParser("", new Or("abs","ssju"));

            return default;
        }
    }
}
