using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorFORMS
{
    internal class Routine
    {
        public int id_rutina { get; set; }
        public string descripcion { get; set; }
        public int fecha { get; set; }
        public string tipoDeEntreno { get; set; }
        public string estado { get; set; }
        public string intensidad { get; set; }
        public string notas { get; set; }

    }
}
