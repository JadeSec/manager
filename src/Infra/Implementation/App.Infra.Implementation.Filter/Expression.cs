﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infra.Implementation.Filter
{
    public class Expression
    {
        public string Name { get; set; }

        public string Operator { get; set; }

        public string Value { get; set; }
    }
}