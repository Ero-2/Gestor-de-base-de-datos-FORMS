using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GestorFORMS
{
    internal class DataGridViewCalendarColumn : DataGridViewColumn
    {
        public DataGridViewCalendarColumn() : base(new DataGridViewCalendarCell())
        { 
        }

        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                // Asegurarse de que la plantilla del cell sea un DataGridViewCalendarCell
                if (value != null && !value.GetType().IsAssignableFrom(typeof(DataGridViewCalendarCell)))
                {
                    throw new InvalidCastException("Debe especificar una instancia de DataGridViewCalendarCell");
                }
                base.CellTemplate = value;
            }
        }
    }
}
