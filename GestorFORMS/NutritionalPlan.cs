using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorFORMS
{
    internal class NutritionalPlan
    {
        public int id_plan { get; set; }
        public int id_powerlifter { get; set; }
        public string caloriasConsumidas { get; set; }
        public string proteinas { get; set; }
        public decimal carbohidratos { get; set; }
        public decimal grasas { get; set; }
    }
}
