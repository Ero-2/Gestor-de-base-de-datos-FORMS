using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorFORMS
{
    internal class Training
    {
        public int ID_entrenamiento { get; set; }
        public int id_powerlifter { get; set; }
        public int id_rutina { get; set; }
        public decimal duracion { get; set; }
        public string sensaciones { get; set; }
        public string notas { get; set; }
    }
}
