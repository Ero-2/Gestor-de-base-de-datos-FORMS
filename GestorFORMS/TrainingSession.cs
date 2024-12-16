using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorFORMS
{
    internal class TrainingSession
    {
        
            public int ID_Sesion { get; set; }
            public int id_powerlifter { get; set; }
            public string TipoDeSesion { get; set; }
            public string intensidad { get; set; }
            public string estadoDelAtleta { get; set; }
            public string evaluacion { get; set; }
        
    }
}
