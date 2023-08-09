using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infra.Implementation.Filter.Interfaces
{
    public interface IPaginate
    {
        public int Next { get; }

        public int Prev { get; }

        public int[] Pages { get; }

        public int Page { get; set; }

        public int Total { get; set; }

        public Filter Filter { get; set; }
    }
}
