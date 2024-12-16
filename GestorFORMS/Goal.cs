using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorFORMS
{
    internal class Goal
    {
        public int ID_Objetivos { get; set; }
        public int id_powerlifter { get; set; }
        public int SetsEsperados { get; set; }
        public int RepsEsperados { get; set; }
        public decimal CargaEstimada { get; set; }
        public string notas { get; set; }
    }
}
