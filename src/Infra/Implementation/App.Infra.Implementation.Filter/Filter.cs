using App.Infra.Implementation.Filter.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace App.Infra.Implementation.Filter
{
    public class Filter
    {        
        private const string FILTER_SEPARATOR = "|";
        private readonly string[] FILTER_OPERATORS = new string[] { "!", "<", ">", "%", "/", "&", "=" };
        private const string FILTER_PATTERN_MATCH_RGX = @"^(\w*):{1}([\!\<\>\%\/\&]?\s?)(\""?[a-zA-Z0-9\sá-ú\~\,\-_]*\""?)$";

        public Configuration Configuration { get; private set; }

        public List<Expression> Expressions = new List<Expression>();

        public Filter(string input, Configuration configuration)
        {
            Configuration = configuration;

            var filters = input?.Split(FILTER_SEPARATOR) ?? new string[0];
           
            if (configuration != null && !configuration.isValidExpressionMax(filters.Length))
                throw new FilterException($"Allowed only {configuration.ExpressionMax} expression per search.");

            foreach (var filter in filters)
            {
                var expression = _getExpression(filter);
                
                if(configuration != null)
                {
                    if (expression.Operator != "" && !FILTER_OPERATORS.Any(x => x.Equals(expression.Operator)))
                        throw new FilterException($"Invalid operator.");

                    if (!configuration.isValidValueLenght(expression.Value))
                        throw new FilterException($"Invalid value lenght.");

                    if (!configuration.isValidCriteria(expression.Name))
                        throw new FilterException($"Invalid attribute name.");
                }

                Expressions.Add(expression);
            }        
        }

        private static Expression _getExpression(string expression)
        {
            Match match = Regex.Match(expression, FILTER_PATTERN_MATCH_RGX);

            if (match.Success)
            {
                //When operator not defined the default value is "="
                string opt = match.Groups[2].Value;
                if (opt == null || opt == string.Empty || opt == " ")
                    opt = "=";

                return new()
                {
                    Operator = opt,
                    Name = match.Groups[1].Value,
                    Value = match.Groups[3].Value
                };
            }

            throw new FilterException($"Invalid expression \"{expression}\".");
        }
    }
}