using App.Infra.Implementation.Filter.Exceptions;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace App.Infra.Implementation.Filter
{
    public class Filter
    {
        private const string FILTER_SEPARATOR = "|";
        private const string FILTER_PATTERN_MATCH_RGX = @"^(\w*):{1}([\!\<\>\%\/\&]?\s?)(\""?[a-zA-Z0-9\sá-ú\~\,]*\""?)$";

        public List<Expression> Expressions = new List<Expression>();

        public Filter(string input, Configuration configuration)
        {
            var filters = input?.Split(FILTER_SEPARATOR) ?? new string[0];

            if (configuration != null && !configuration.isValidExpressionMax(filters.Length))
                throw new FilterException($"Allowed only {configuration.ExpressionMaxAllowed} expression per search.");

            foreach (var filter in filters)
            {
                var expression = _getExpression(filter);
                if (configuration != null && !configuration.isValidValueLenght(expression.Value))
                    throw new FilterException($"Invalid value lenght.");

                Expressions.Add(expression);
            }
        }

        private static Expression _getExpression(string expression)
        {
            Match match = Regex.Match(expression, FILTER_PATTERN_MATCH_RGX);

            if (match.Success)
            {
                return new()
                {
                    Name = match.Groups[1].Value,
                    Operator = match.Groups[2].Value,
                    Value = match.Groups[3].Value
                };
            }

            throw new FilterException($"Invalid expression \"{expression}\".");
        }
    }
}