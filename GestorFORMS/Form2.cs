using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GestorFORMS
{
    public partial class Form2 : Form
    {
        public string TableName { get; set; }

        
    public int IDPowerlifter { get { return int.TryParse(textBox1.Text, out int id) ? id : 0; } }
        public int Edad { get { return int.TryParse(textBox2.Text, out int edad) ? edad : 0; } }
        public decimal Peso { get { return decimal.TryParse(textBox3.Text, out decimal peso) ? peso : 0m; } }
        public decimal Altura { get { return decimal.TryParse(textBox4.Text, out decimal altura) ? altura : 0m; } }

        // Para la tabla "Usuarios"
        public string Usuario { get { return textBox6.Text; } }
        public string Contraseña { get { return textBox7.Text; } }
        public string Rol { get { return textBox8.Text; } }
        public DateTime FechaCreacion { get { return dateTimePicker1.Value; } }
        public int IDPowerlifterUsuario { get { return int.TryParse(textBox9.Text, out int id) ? id : 0; } }

        // Para la tabla "Entrenamientos"
        public int IDEntrenamiento { get { return int.TryParse(textBox11.Text, out int id) ? id : 0; } }
        public int IDPowerlifterEntrenamiento { get { return int.TryParse(textBox12.Text, out int id) ? id : 0; } }
        public int IDRutina { get { return int.TryParse(textBox13.Text, out int id) ? id : 0; } }
        public decimal Duracion { get { return decimal.TryParse(textBox14.Text, out decimal duracion) ? duracion : 0m; } }
        public string Sensaciones { get { return textBox15.Text; } }
        public string Notas { get { return textBox16.Text; } }




        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        public void InitializeForm(string tableName)
        {
            TableName = tableName;

            // Hacer visible u ocultar los TextBox según la tabla seleccionada
            switch (tableName)
            {
                case "Powerlifters":
                    textBox1.Visible = true;  // ID Powerlifter
                    textBox2.Visible = true;  // Edad
                    textBox3.Visible = true;  // Peso
                    textBox4.Visible = true;  // Altura
                    
                    break;

                case "Usuarios":
                   
                    textBox6.Visible = true;  // Usuario
                    textBox7.Visible = true;  // Contraseña
                    textBox8.Visible = true;  // Rol
                    textBox9.Visible = true;  // ID Powerlifter
                    dateTimePicker1.Visible = true; // Fecha de creación
                    break;

                case "Entrenamientos":
                    
                    textBox11.Visible = true; // ID Entrenamiento
                    textBox12.Visible = true; // ID Powerlifter
                    textBox13.Visible = true; // ID Rutina
                    textBox14.Visible = true; // Duración
                    textBox15.Visible = true; // Sensaciones
                    textBox16.Visible = true; // Notas
                    break;

                default:
                    MessageBox.Show("Tabla no configurada.");
                    break;
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                // Si el formulario se cierra con éxito, se guardan los valores
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;  // Si el usuario cancela
        }

    }
}
