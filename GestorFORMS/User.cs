using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorFORMS
{
    public class User
    {
        public int id_usuario { get; set; }
        public string usuario { get; set; }
        public string contraseña { get; set; }
        public string rol { get; set; }
        public DateTime fecha_creacion { get; set; }
        public int id_powerlifter { get; set; }

    }
}
