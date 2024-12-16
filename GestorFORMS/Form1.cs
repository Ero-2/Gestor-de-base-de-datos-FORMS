using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.IO;
namespace GestorFORMS
{
    public partial class guardarPowerlifters : Form
    {
        private string _connectionString = "Server=DESKTOP-T61VTOI\\SQLEXPRESS; Database=PowerliftingAtletass; Integrated Security=True;";
        private ProductRepository _productRepository;
        private CrudManager _crudManager;
        private PowerlifterDataAcessLayer _powerlifterRepository;
        private List<Powerlifter> powerlifterList;

        private BindingSource bindingSource = new BindingSource();



        public guardarPowerlifters()
        {
            InitializeComponent();
            _productRepository = new ProductRepository();

            // Inicializar CrudManager una sola vez con la cadena de conexión correcta
            _crudManager = new CrudManager(_connectionString);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBoxTables.Items.Add("Powerlifters");
            comboBoxTables.Items.Add("Usuarios");
            comboBoxTables.Items.Add("Entrenamientos");
            comboBoxTables.Items.Add("Ejercicios");
            comboBoxTables.Items.Add("Sesiones");
            comboBoxTables.Items.Add("Carga");
            comboBoxTables.Items.Add("Series");
            comboBoxTables.Items.Add("Objetivos");
            comboBoxTables.Items.Add("Rutinas");
            comboBoxTables.Items.Add("Plan Nutricional");


            comboBoxTables.SelectedIndex = 0;


            dataGridView1.AllowUserToAddRows = true; // Muestra una fila vacía al final
            dataGridView1.AutoGenerateColumns = true; // Genera columnas automáticamente

            // Si la tabla seleccionada es "Usuarios", se agrega la columna 'fecha_creacion'
            if (comboBoxTables.SelectedItem.ToString() == "Usuarios")
            {
                DataGridViewCalendarColumn fechaColumn = new DataGridViewCalendarColumn();
                fechaColumn.HeaderText = "fecha_creacion";
                fechaColumn.Name = "fecha_creacion";
                dataGridView1.Columns.Add(fechaColumn);




            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            // Verifica si el comboBoxTables tiene un valor seleccionado
            string selectedTable = comboBoxTables.SelectedItem?.ToString();

            if (selectedTable == "Powerlifters")
            {
                // Llama al método LoadPowerlifters para cargar y mostrar los datos
                LoadPowerlifters();
            }
            else
            {
                MessageBox.Show("Por favor, selecciona una tabla válida para cargar los datos.",
                                "Tabla no seleccionada",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
            }
        }



        private void buttonCrear_Click(object sender, EventArgs e)
        {
            string tableName = comboBoxTables.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(tableName))
            {
                MessageBox.Show("Por favor, selecciona una tabla.");
                return;
            }

            if (tableName != "Powerlifters")
            {
                MessageBox.Show("Por favor, selecciona la tabla 'Powerlifters'.");
                return;
            }

            var currentList = dataGridView1.DataSource as BindingList<Powerlifter>;
            if (currentList == null)
            {
                MessageBox.Show("No hay lista de Powerlifters cargada.");
                return;
            }

            // Agregar un nuevo Powerlifter vacío a la lista
            Powerlifter newP = new Powerlifter { edad = 0, peso = 0, altura = 0 };
            currentList.Add(newP);

            // Seleccionar la celda "edad" de la última fila para edición
            dataGridView1.CurrentCell = dataGridView1.Rows[currentList.Count - 1].Cells["edad"];

        }

        private void LogError(Exception ex)
        {
            string logFilePath = "error_log.txt";
            string message = $"{DateTime.Now}: {ex.ToString()}\n";
            File.AppendAllText(logFilePath, message);
        }


        private void buttonActualizar_Click(object sender, EventArgs e)
        {

            string tableName = comboBoxTables.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(tableName))
            {
                MessageBox.Show("Por favor, selecciona una tabla.");
                return;
            }

            // Obtener la fila seleccionada
            var selectedRow = dataGridView1.SelectedRows[0];
            if (selectedRow != null)
            {
                // Crear la entidad a partir de los datos de la fila seleccionada
                var updatedEntity = new Powerlifter
                {
                    ID_powerlifter = (int)selectedRow.Cells["ID_powerlifter"].Value,  // Asegúrate de que la columna tiene el nombre correcto
                    edad = int.Parse(selectedRow.Cells["Edad"].Value.ToString()),
                    peso = decimal.Parse(selectedRow.Cells["Peso"].Value.ToString()),
                    altura = decimal.Parse(selectedRow.Cells["Altura"].Value.ToString())
                };

                // Actualizar en la base de datos
                var repository = _crudManager.GetRepository<Powerlifter>("Powerlifters");
                repository.Update(updatedEntity);

                MessageBox.Show("Datos actualizados correctamente.");
            }
            else
            {
                MessageBox.Show("Por favor, selecciona una fila para actualizar.");
            }


        }

        private void buttonEliminar_Click(object sender, EventArgs e)
        {
            string tableName = comboBoxTables.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(tableName))
            {
                MessageBox.Show("Por favor, selecciona una tabla.");
                return;
            }

            // Obtener la fila seleccionada
            var selectedRow = dataGridView1.SelectedRows[0];
            if (selectedRow != null)
            {
                int id = (int)selectedRow.Cells["ID_powerlifter"].Value;

                // Eliminar de la base de datos
                var repository = _crudManager.GetRepository<Powerlifter>("Powerlifters");
                repository.Delete(id);

                // Eliminar la fila del DataGridView
                dataGridView1.Rows.Remove(selectedRow);

                MessageBox.Show("Registro eliminado correctamente.");
            }
            else
            {
                MessageBox.Show("Por favor, selecciona una fila para eliminar.");
            }
        }

        private void comboBoxTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedTable = comboBoxTables.SelectedItem.ToString();

            // Limpiar las columnas previas
            dataGridView1.Columns.Clear();

            switch (selectedTable)
            {
                case "Powerlifters":
                    LoadPowerlifters();
                    break;
                case "Usuarios":
                    LoadUsers();
                    DataGridViewCalendarColumn fechaColumn = new DataGridViewCalendarColumn();
                    fechaColumn.HeaderText = "fecha_creacion";
                    fechaColumn.Name = "fecha_creacion";
                    dataGridView1.Columns.Add(fechaColumn);
                    break;
                case "Entrenamientos":
                    LoadTrainings();
                    break;
                case "Ejercicios":
                    LoadExercises();
                    break;
                case "Sesiones":
                    LoadSessions();
                    break;
                case "Carga":
                    LoadLoads();
                    break;
                case "Series":
                    LoadSeries();
                    break;
                case "Objetivos":
                    LoadGoals();
                    break;
                case "Rutinas":
                    LoadRoutine();
                    break;
                case "Plan Nutricional":
                    LoadNutritionalPlan();
                    break;
                default:
                    MessageBox.Show("Tabla no encontrada.");
                    break;





            }
        }

        private string HashPassword(string plainTextPassword)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(plainTextPassword);
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        private void dataGridView1_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            // Verifica si la fila es la nueva fila de ingreso de datos
            if (dataGridView1.Rows[e.RowIndex].IsNewRow) return;

            // Obtén los valores de las celdas
            var edad = dataGridView1.Rows[e.RowIndex].Cells["edad"].Value;
            var peso = dataGridView1.Rows[e.RowIndex].Cells["peso"].Value;
            var altura = dataGridView1.Rows[e.RowIndex].Cells["altura"].Value;

            // Valida que los valores no sean nulos o inválidos
            if (edad != null && peso != null && altura != null)
            {
                try
                {
                    // Crea un objeto Powerlifter
                    Powerlifter newPowerlifter = new Powerlifter
                    {
                        edad = Convert.ToInt32(edad),
                        peso = Convert.ToDecimal(peso),
                        altura = Convert.ToDecimal(altura)
                    };

                    // Guarda en la base de datos
                    string connectionString = "Server=DESKTOP-T61VTOI\\SQLEXPRESS; Database=PowerliftingAtletass; Integrated Security=True;";
                    PowerlifterDataAcessLayer dal = new PowerlifterDataAcessLayer(connectionString);
                    dal.Create(newPowerlifter);

                    // Refresca el DataGridView para mostrar los cambios
                    LoadPowerlifters();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al guardar los datos: {ex.Message}");
                }
            }




        }



        private BindingList<Powerlifter> powerlifterListt;

        private void LoadPowerlifters()
        {
            var dal = new PowerlifterDataAcessLayer(_connectionString);
            List<Powerlifter> powerlifters = dal.GetAll();

            // Convertimos la lista a un BindingList y la asignamos a la variable global
            powerlifterListt = new BindingList<Powerlifter>(powerlifters);
            dataGridView1.DataSource = powerlifterListt;

            dataGridView1.AllowUserToAddRows = true;
            dataGridView1.AutoGenerateColumns = true;
        }

        private void AddEmptyRow(BindingList<Powerlifter> bindingList)
        {
            // Crear una nueva instancia de Powerlifter (fila vacía)
            Powerlifter emptyPowerlifter = new Powerlifter
            {
                // No asignar valores, solo crear un objeto vacío
            };

            // Agregar el nuevo Powerlifter (vacío) al BindingList
            bindingList.Add(emptyPowerlifter);
        }

        private BindingList<User> userList;

        private void LoadUsers()
        {
            try
            {
                // Obtener los usuarios desde la base de datos
                string connectionString = "Server=DESKTOP-T61VTOI\\SQLEXPRESS; Database=PowerliftingAtletass; Integrated Security=True;";
                var userRepository = _crudManager.GetRepository<User>("Usuarios");
                List<User> users = userRepository.GetAll();

                // Convertir la lista a BindingList para vincularla al DataGridView
                userList = new BindingList<User>(users);

                // Enlazar el BindingList al DataGridView
                dataGridView1.DataSource = userList;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los usuarios: {ex.Message}");
            }
        }

        private BindingList<Exercise> exerciseList;

        private void LoadExercises()
        {
            try
            {
                // Asegúrate de que el repositorio esté correctamente instanciado
                var ejercicioRepository = new ExerciseDataAcessLayer(_connectionString);
                List<Exercise> ejercicios = ejercicioRepository.GetAll();

                // Si tienes un DataGridView, puedes asignar la lista de ejercicios como fuente de datos
                dataGridView1.DataSource = new BindingList<Exercise>(ejercicios);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los ejercicios: {ex.Message}");
            }
        }



        private BindingList<Training> trainingList;

        private void LoadTrainings()
        {
            try
            {
                // Obtener los entrenamientos desde la base de datos
                string connectionString = "Server=DESKTOP-T61VTOI\\SQLEXPRESS; Database=PowerliftingAtletass; Integrated Security=True;"; // Cambiar por la cadena de conexión
                var trainingRepository = _crudManager.GetRepository<Training>("Entrenamientos");
                List<Training> trainings = trainingRepository.GetAll();

                // Convertir la lista a BindingList para vincularla al DataGridView
                trainingList = new BindingList<Training>(trainings);

                // Enlazar el BindingList al DataGridView
                dataGridView1.DataSource = trainingList;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los entrenamientos: {ex.Message}");
            }
        }

        private BindingList<TrainingSession> sessionList;

        private void LoadSessions()
        {
            try
            {
                // Obtener la conexión y el repositorio
                string connectionString = "Server=DESKTOP-T61VTOI\\SQLEXPRESS; Database=PowerliftingAtletass; Integrated Security=True;";
                TrainingSessionDataAccessLayer dal = new TrainingSessionDataAccessLayer(connectionString);

                // Obtener todas las sesiones desde la base de datos
                List<TrainingSession> sessions = dal.GetAll();

                // Convertir la lista a BindingList para enlazarla al DataGridView
                sessionList = new BindingList<TrainingSession>(sessions);

                // Enlazar el BindingList al DataGridView
                dataGridView1.DataSource = sessionList;

                // Permitir agregar nuevas filas directamente en el DataGridView
                dataGridView1.AllowUserToAddRows = true;

                // Personalizar columnas si es necesario
                dataGridView1.AutoResizeColumns();

                // Mensaje si no hay datos
                if (sessions.Count == 0)
                {
                    MessageBox.Show("No hay datos en la tabla 'sesionDeEntrenamiento'.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las sesiones: {ex.Message}");
            }
        }



        private BindingList<Load> loadList;

        private void LoadLoads()
        {
            try
            {
                // Cadena de conexión de la base de datos
                string connectionString = "Server=DESKTOP-T61VTOI\\SQLEXPRESS; Database=PowerliftingAtletass; Integrated Security=True;"; // Cambiar por tu cadena de conexión

                // Instanciar el repositorio de Cargas
                var loadRepository = _crudManager.GetRepository<Load>("Carga");

                // Obtener las cargas desde la base de datos
                List<Load> loads = loadRepository.GetAll();

                // Convertir la lista a BindingList
                loadList = new BindingList<Load>(loads);

                // Enlazar el BindingList al DataGridView
                dataGridView1.DataSource = loadList;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las cargas: {ex.Message}");
            }
        }

        private BindingList<Series> seriesList;

        private void LoadSeries()
        {
            try
            {
                // Cadena de conexión de la base de datos
                string connectionString = "Server=DESKTOP-T61VTOI\\SQLEXPRESS; Database=PowerliftingAtletass; Integrated Security=True;"; // Cambiar por tu cadena de conexión

                // Instanciar el repositorio de Series
                var seriesRepository = _crudManager.GetRepository<Series>("Series");

                // Obtener las series desde la base de datos
                List<Series> series = seriesRepository.GetAll();

                // Convertir la lista a BindingList
                seriesList = new BindingList<Series>(series);

                // Enlazar el BindingList al DataGridView
                dataGridView1.DataSource = seriesList;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las series: {ex.Message}");
            }
        }

        private BindingList<Goal> goalList;

        private void LoadGoals()
        {
            try
            {
                // Cadena de conexión de la base de datos
                string connectionString = "Server=DESKTOP-T61VTOI\\SQLEXPRESS; Database=PowerliftingAtletass; Integrated Security=True;"; // Cambiar por tu cadena de conexión

                // Instanciar el repositorio de Goals (No de Series)
                var goalsRepository = _crudManager.GetRepository<Goal>("Goals"); // Cambia "Goals" por el nombre correcto de la entidad

                // Obtener los goals desde la base de datos
                List<Goal> goals = goalsRepository.GetAll(); // Usa GetAll() para obtener los Goals

                // Convertir la lista a BindingList
                goalList = new BindingList<Goal>(goals);

                // Enlazar el BindingList al DataGridView
                dataGridView1.DataSource = goalList;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los goals: {ex.Message}");
            }
        }
        

        private BindingList<Routine> routineList;

        private void LoadRoutine()
        {
            try
            {
                // Cadena de conexión de la base de datos
                string connectionString = "Server=DESKTOP-T61VTOI\\SQLEXPRESS; Database=PowerliftingAtletass; Integrated Security=True;"; // Cambiar por tu cadena de conexión

                // Instanciar la capa de acceso a datos para la tabla `rutinas`
                RoutineDataAcessLayer dal = new RoutineDataAcessLayer(connectionString);

                // Obtener todas las rutinas desde la base de datos
                List<Routine> routines = dal.GetAll();

                // Convertir la lista de rutinas a BindingList para la actualización dinámica del DataGridView
                routineList = new BindingList<Routine>(routines);

                // Enlazar el BindingList al DataGridView
                dataGridView1.DataSource = routineList;

                // Verificar si la lista está vacía
                if (routines.Count == 0)
                {
                    MessageBox.Show("No hay datos en la tabla 'rutinas'.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las rutinas: {ex.Message}");
            }
        }


        private BindingList<NutritionalPlan> nutritionalPlanList;

        private void LoadNutritionalPlan()
        {
            try
            {
                // Cadena de conexión de la base de datos
                string connectionString = "Server=DESKTOP-T61VTOI\\SQLEXPRESS; Database=PowerliftingAtletass; Integrated Security=True;"; // Cambiar por tu cadena de conexión

                // Instanciar la capa de acceso a datos para la tabla `PlanAlimenticio`
                NutritionalPlanDataAcessLayer dal = new NutritionalPlanDataAcessLayer(connectionString);

                // Obtener todos los planes nutricionales desde la base de datos
                List<NutritionalPlan> nutritionalPlans = dal.GetAll();

                // Convertir la lista de planes nutricionales a BindingList para la actualización dinámica del DataGridView
                nutritionalPlanList = new BindingList<NutritionalPlan>(nutritionalPlans);

                // Enlazar el BindingList al DataGridView
                dataGridView1.DataSource = nutritionalPlanList;

                // Verificar si la lista está vacía
                if (nutritionalPlans.Count == 0)
                {
                    MessageBox.Show("No hay datos en la tabla 'PlanAlimenticio'.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los planes nutricionales: {ex.Message}");
            }
        }






        private void crearuser_Click(object sender, EventArgs e)
        {
            string tableName = comboBoxTables.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(tableName))
            {
                MessageBox.Show("Por favor, selecciona una tabla.");
                return;
            }

            if (tableName != "Usuarios")
            {
                MessageBox.Show("Por favor, selecciona la tabla 'Usuarios'.");
                return;
            }

            // Se asume que dataGridView1 está enlazado a userList (BindingList<User>)
            User newUser = new User
            {
                id_usuario = 0,
                usuario = "",
                contraseña = "", // El usuario la escribirá en el DataGridView
                rol = "",
                fecha_creacion = DateTime.Now,
                id_powerlifter = 0
            };

            userList.Add(newUser);
            dataGridView1.CurrentCell = dataGridView1.Rows[userList.Count - 1].Cells["usuario"];
        }

        private void actuser_Click(object sender, EventArgs e)
        {
            string tableName = comboBoxTables.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(tableName))
            {
                MessageBox.Show("Por favor, selecciona una tabla.");
                return;
            }

            // Obtener la fila seleccionada
            var selectedRow = dataGridView1.SelectedRows[0];
            if (selectedRow != null)
            {
                // Crear la entidad a partir de los datos en las celdas
                var updatedUser = new User
                {
                    id_usuario = (int)selectedRow.Cells["id_usuario"].Value,
                    usuario = selectedRow.Cells["usuario"].Value.ToString(),
                    contraseña = selectedRow.Cells["contraseña"].Value.ToString(),
                    rol = selectedRow.Cells["rol"].Value.ToString(),
                    fecha_creacion = DateTime.Parse(selectedRow.Cells["fecha_creacion"].Value.ToString()),
                    id_powerlifter = (int)selectedRow.Cells["id_powerlifter"].Value
                };

                // Actualizar en la lista enlazada
                var userRepository = _crudManager.GetRepository<User>("Usuarios");
                userRepository.Update(updatedUser);

                // Actualizar la fila en el DataGridView
                selectedRow.Cells["usuario"].Value = updatedUser.usuario;
                selectedRow.Cells["contraseña"].Value = updatedUser.contraseña;
                selectedRow.Cells["rol"].Value = updatedUser.rol;
                selectedRow.Cells["fecha_creacion"].Value = updatedUser.fecha_creacion;
                selectedRow.Cells["id_powerlifter"].Value = updatedUser.id_powerlifter;

                MessageBox.Show("Datos de usuario actualizados correctamente.");
            }
        }

        private void eliminaruser_Click(object sender, EventArgs e)
        {
            string tableName = comboBoxTables.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(tableName))
            {
                MessageBox.Show("Por favor, selecciona una tabla.");
                return;
            }

            // Obtener la fila seleccionada
            var selectedRow = dataGridView1.SelectedRows[0];
            if (selectedRow != null)
            {
                // Obtener el ID del usuario de la fila seleccionada
                int id = (int)selectedRow.Cells["id_usuario"].Value;

                // Eliminar el usuario en la base de datos
                var userRepository = _crudManager.GetRepository<User>("Usuarios");
                userRepository.Delete(id);

                // Eliminar la fila en el DataGridView
                userList.RemoveAt(selectedRow.Index);

                MessageBox.Show("Usuario eliminado correctamente.");
            }
        }

        private void CrearEntrenamiento_Click(object sender, EventArgs e)
        {
            try
            {
                // Crear un nuevo ejercicio (puedes obtener estos datos de un formulario o DataGridView)
                Exercise newExercise = new Exercise
                {
                    id_entrenamiento = 1,  // Ejemplo: debes obtener estos datos desde algún input
                    tipoMovimiento = "Sentadilla",
                    setss = 3,
                    reps = 10,
                    cargaUtilizada = 100.0m,
                    descansoEntreSeries = 60,
                    tempo = "2-0-2",
                    RPE = 8,
                    RIR = 2,
                    notas = "Notas del ejercicio"
                };

                // Instanciar el repositorio de ejercicios
                var ejercicioRepository = new ExerciseDataAcessLayer(_connectionString);

                // Crear el nuevo ejercicio en la base de datos
                ejercicioRepository.Create(newExercise);

                MessageBox.Show("Ejercicio guardado exitosamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el ejercicio: {ex.Message}");
            }
        }

        private void ActualizarEntrenamiento_Click(object sender, EventArgs e)
        {
            string tableName = comboBoxTables.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(tableName))
            {
                MessageBox.Show("Por favor, selecciona una tabla.");
                return;
            }

            // Obtener la fila seleccionada
            var selectedRow = dataGridView1.SelectedRows[0];
            if (selectedRow != null)
            {
                // Crear la entidad a partir de los datos en las celdas
                var updatedTraining = new Training
                {
                    ID_entrenamiento = (int)selectedRow.Cells["ID_entrenamiento"].Value,
                    id_powerlifter = (int)selectedRow.Cells["id_powerlifter"].Value,
                    id_rutina = (int)selectedRow.Cells["id_rutina"].Value,
                    duracion = decimal.Parse(selectedRow.Cells["duracion"].Value.ToString()),
                    sensaciones = selectedRow.Cells["sensaciones"].Value.ToString(),
                    notas = selectedRow.Cells["notas"].Value.ToString()
                };

                // Actualizar en la lista enlazada
                var trainingRepository = _crudManager.GetRepository<Training>("Entrenamientos");
                trainingRepository.Update(updatedTraining);

                // Actualizar la fila en el DataGridView
                selectedRow.Cells["id_powerlifter"].Value = updatedTraining.id_powerlifter;
                selectedRow.Cells["id_rutina"].Value = updatedTraining.id_rutina;
                selectedRow.Cells["duracion"].Value = updatedTraining.duracion;
                selectedRow.Cells["sensaciones"].Value = updatedTraining.sensaciones;
                selectedRow.Cells["notas"].Value = updatedTraining.notas;

                MessageBox.Show("Datos de entrenamiento actualizados correctamente.");
            }
        }

        private void eliminarEntrenamiento_Click(object sender, EventArgs e)
        {
            string tableName = comboBoxTables.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(tableName))
            {
                MessageBox.Show("Por favor, selecciona una tabla.");
                return;
            }

            // Obtener la fila seleccionada
            var selectedRow = dataGridView1.SelectedRows[0];
            if (selectedRow != null)
            {
                // Obtener el ID del entrenamiento de la fila seleccionada
                int id = (int)selectedRow.Cells["ID_entrenamiento"].Value;

                // Eliminar el entrenamiento en la base de datos
                var trainingRepository = _crudManager.GetRepository<Training>("Entrenamientos");
                trainingRepository.Delete(id);

                // Eliminar la fila en el DataGridView
                trainingList.RemoveAt(selectedRow.Index);

                MessageBox.Show("Entrenamiento eliminado correctamente.");
            }
        }

        private void crearEJERCICIO_Click(object sender, EventArgs e)
        {
            try
            {
                string tableName = comboBoxTables.SelectedItem?.ToString();

                if (string.IsNullOrEmpty(tableName) || tableName != "Ejercicios")
                {
                    MessageBox.Show("Por favor, selecciona la tabla 'Ejercicios'.");
                    return;
                }

                // Crear una nueva instancia de ejercicio con valores predeterminados
                Exercise newExercise = new Exercise
                {
                    ID_ejercicio = 0,  // Valor predeterminado
                    id_entrenamiento = 0,
                    tipoMovimiento = "",
                    setss = 0,
                    reps = 0,
                    cargaUtilizada = 0.0m,
                    descansoEntreSeries = 0,
                    tempo = "",
                    RPE = 0,
                    RIR = 0,
                    notas = ""
                };

                // Agregar el nuevo ejercicio a la lista enlazada
                if (exerciseList == null)
                {
                    exerciseList = new BindingList<Exercise>();
                    dataGridView1.DataSource = exerciseList;
                }

                exerciseList.Add(newExercise);

                // Establecer el foco en la primera celda de la nueva fila
                dataGridView1.CurrentCell = dataGridView1.Rows[exerciseList.Count - 1].Cells["tipoMovimiento"];
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al crear el ejercicio: {ex.Message}");
            }
        }

        private void ActualizarEjercicio_Click(object sender, EventArgs e)
        {
            string tableName = comboBoxTables.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(tableName))
            {
                MessageBox.Show("Por favor, selecciona una tabla.");
                return;
            }

            // Obtener la fila seleccionada
            var selectedRow = dataGridView1.SelectedRows[0];
            if (selectedRow != null)
            {
                // Crear la entidad a partir de los datos en el DataGridView
                var updatedEntity = new Exercise
                {
                    ID_ejercicio = (int)selectedRow.Cells["ID_ejercicio"].Value,
                    id_entrenamiento = (int)selectedRow.Cells["id_entrenamiento"].Value,
                    tipoMovimiento = selectedRow.Cells["tipoMovimiento"].Value.ToString(),
                    setss = (int)selectedRow.Cells["setss"].Value,
                    reps = (int)selectedRow.Cells["reps"].Value,
                    cargaUtilizada = (decimal)selectedRow.Cells["cargaUtilizada"].Value,
                    descansoEntreSeries = (int)selectedRow.Cells["descansoEntreSeries"].Value,
                    tempo = selectedRow.Cells["tempo"].Value.ToString(),
                    RPE = (int)selectedRow.Cells["RPE"].Value,
                    RIR = (int)selectedRow.Cells["RIR"].Value,
                    notas = selectedRow.Cells["notas"].Value.ToString()
                };

                // Actualizar en la base de datos
                var repository = _crudManager.GetRepository<Exercise>("Ejercicios");
                repository.Update(updatedEntity);

                MessageBox.Show("Ejercicio actualizado correctamente.");
            }
        }

        private void eliminarEjercicio_Click(object sender, EventArgs e)
        {
            string tableName = comboBoxTables.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(tableName))
            {
                MessageBox.Show("Por favor, selecciona una tabla.");
                return;
            }

            // Obtener la fila seleccionada
            var selectedRow = dataGridView1.SelectedRows[0];
            if (selectedRow != null)
            {
                // Obtener el ID del ejercicio seleccionado
                int id = (int)selectedRow.Cells["ID_ejercicio"].Value;

                // Eliminar el ejercicio de la base de datos
                var repository = _crudManager.GetRepository<Exercise>("Ejercicios");
                repository.Delete(id);

                // Eliminar la fila del DataGridView
                dataGridView1.Rows.Remove(selectedRow);

                MessageBox.Show("Ejercicio eliminado correctamente.");
            }
        }

        private void sesionesCrear_Click(object sender, EventArgs e)
        {
            try
            {
                string tableName = comboBoxTables.SelectedItem?.ToString();

                if (string.IsNullOrEmpty(tableName) || tableName != "Sesiones")
                {
                    MessageBox.Show("Por favor, selecciona la tabla 'Sesiones'.");
                    return;
                }

                // Agregar una nueva fila vacía al DataGridView para ingresar los datos
                int rowIndex = dataGridView1.Rows.Add();
                dataGridView1.Rows[rowIndex].Cells["TipoDeSesion"].Selected = true; // Establecer foco en la primera celda

                MessageBox.Show("Ingresa los datos para la nueva sesión.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al crear sesión: {ex.Message}");
            }
        }

        private void sesionActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                string tableName = comboBoxTables.SelectedItem?.ToString();

                if (string.IsNullOrEmpty(tableName))
                {
                    MessageBox.Show("Por favor, selecciona una tabla.");
                    return;
                }

                // Obtener la fila seleccionada
                var selectedRow = dataGridView1.SelectedRows[0];

                if (selectedRow != null)
                {
                    // Crear la entidad a partir de los datos de la fila seleccionada
                    var updatedSession = new TrainingSession
                    {
                        ID_Sesion = (int)selectedRow.Cells["ID_Sesion"].Value,
                        id_powerlifter = (int)selectedRow.Cells["id_powerlifter"].Value,
                        TipoDeSesion = selectedRow.Cells["TipoDeSesion"].Value.ToString(),
                        intensidad = selectedRow.Cells["intensidad"].Value.ToString(),
                        estadoDelAtleta = selectedRow.Cells["estadoDelAtleta"].Value.ToString(),
                        evaluacion = selectedRow.Cells["evaluacion"].Value.ToString()
                    };

                    // Actualizar la sesión en la base de datos
                    var sessionRepository = _crudManager.GetRepository<TrainingSession>("sesionDeEntrenamiento");
                    sessionRepository.Update(updatedSession);

                    MessageBox.Show("Sesión actualizada correctamente.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar sesión: {ex.Message}");
            }
        }

        private void SessionEliminar_Click(object sender, EventArgs e)
        {

            try
            {
                string tableName = comboBoxTables.SelectedItem?.ToString();

                if (string.IsNullOrEmpty(tableName))
                {
                    MessageBox.Show("Por favor, selecciona una tabla.");
                    return;
                }

                // Obtener la fila seleccionada
                var selectedRow = dataGridView1.SelectedRows[0];

                if (selectedRow != null)
                {
                    // Obtener el ID de la sesión seleccionada
                    int idSesion = (int)selectedRow.Cells["ID_Sesion"].Value;

                    // Eliminar la sesión de la base de datos
                    var sessionRepository = _crudManager.GetRepository<TrainingSession>("sesionDeEntrenamiento");
                    sessionRepository.Delete(idSesion);

                    // Eliminar la fila del DataGridView
                    dataGridView1.Rows.Remove(selectedRow);

                    MessageBox.Show("Sesión eliminada correctamente.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar sesión: {ex.Message}");
            }
        }

        private void CargaCrear_Click(object sender, EventArgs e)
        {
            try
            {
                string tableName = comboBoxTables.SelectedItem?.ToString();

                if (string.IsNullOrEmpty(tableName) || tableName != "Carga")
                {
                    MessageBox.Show("Por favor, selecciona la tabla 'Carga'.");
                    return;
                }

                // Crear una nueva instancia de carga con valores predeterminados
                Load newLoad = new Load
                {
                    ID_carga = 0,  // Valor predeterminado
                    peso = 0.0m,
                    reps = 0,
                    setss = 0,
                    RPE = 0,
                    RIR = 0,
                    notas = ""
                };

                // Agregar la nueva carga a la lista enlazada
                if (loadList == null)
                {
                    loadList = new BindingList<Load>();
                    dataGridView1.DataSource = loadList;
                }

                loadList.Add(newLoad);

                // Establecer el foco en la primera celda de la nueva fila
                dataGridView1.CurrentCell = dataGridView1.Rows[loadList.Count - 1].Cells["peso"];
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al crear la carga: {ex.Message}");
            }
        }

        private void CargaActualizar_Click(object sender, EventArgs e)
        {
            string tableName = comboBoxTables.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(tableName))
            {
                MessageBox.Show("Por favor, selecciona una tabla.");
                return;
            }

            // Obtener la fila seleccionada
            var selectedRow = dataGridView1.SelectedRows[0];
            if (selectedRow != null)
            {
                // Crear la entidad a partir de los datos en la fila
                var updatedLoad = new Load
                {
                    ID_carga = (int)selectedRow.Cells["ID_carga"].Value,
                    peso = (decimal)selectedRow.Cells["Peso"].Value,
                    reps = (int)selectedRow.Cells["Reps"].Value,
                    setss = (int)selectedRow.Cells["Setss"].Value,
                    RPE = (int)selectedRow.Cells["RPE"].Value,
                    RIR = (int)selectedRow.Cells["RIR"].Value,
                    notas = (string)selectedRow.Cells["Notas"].Value
                };

                // Actualizar la entidad en la base de datos
                var loadRepository = _crudManager.GetRepository<Load>("Carga");
                loadRepository.Update(updatedLoad);

                MessageBox.Show("Carga actualizada correctamente.");
            }
        }

        private void CargaDelete_Click(object sender, EventArgs e)
        {
            string tableName = comboBoxTables.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(tableName))
            {
                MessageBox.Show("Por favor, selecciona una tabla.");
                return;
            }

            // Obtener la fila seleccionada
            var selectedRow = dataGridView1.SelectedRows[0];
            if (selectedRow != null)
            {
                // Obtener el ID de la carga seleccionada
                int id = (int)selectedRow.Cells["ID_carga"].Value;

                // Eliminar la carga de la base de datos
                var loadRepository = _crudManager.GetRepository<Load>("Carga");
                loadRepository.Delete(id);

                // Eliminar la fila del DataGridView
                dataGridView1.Rows.Remove(selectedRow);

                MessageBox.Show("Carga eliminada correctamente.");
            }
        }

        private void CrearSeries_Click(object sender, EventArgs e)
        {
            try
            {
                string tableName = comboBoxTables.SelectedItem?.ToString();

                if (string.IsNullOrEmpty(tableName) || tableName != "Series")
                {
                    MessageBox.Show("Por favor, selecciona la tabla 'Series'.");
                    return;
                }

                // Crear una nueva instancia de serie con valores predeterminados
                Series newSeries = new Series
                {
                    id_series = 0,  // Valor predeterminado
                    id_ejercicio_carga = 0,
                    sets = 0,
                    sensaciones = "",
                    notas = ""
                };

                // Agregar la nueva serie a la lista enlazada
                if (seriesList == null)
                {
                    seriesList = new BindingList<Series>();
                    dataGridView1.DataSource = seriesList;
                }

                seriesList.Add(newSeries);

                // Establecer el foco en la primera celda de la nueva fila
                dataGridView1.CurrentCell = dataGridView1.Rows[seriesList.Count - 1].Cells["sets"];
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al crear la serie: {ex.Message}");
            }
        }

        private void ActualizarSeries_Click(object sender, EventArgs e)
        {
            string tableName = comboBoxTables.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(tableName))
            {
                MessageBox.Show("Por favor, selecciona una tabla.");
                return;
            }

            // Obtener la fila seleccionada
            var selectedRow = dataGridView1.SelectedRows[0];
            if (selectedRow != null)
            {
                // Crear la entidad a partir de los datos en la fila
                var updatedSeries = new Series
                {
                    id_series = (int)selectedRow.Cells["ID_Series"].Value,
                    id_ejercicio_carga = (int)selectedRow.Cells["ID_Ejercicio_Carga"].Value,
                    sets = (int)selectedRow.Cells["Sets"].Value,
                    sensaciones = (string)selectedRow.Cells["Sensaciones"].Value,
                    notas = (string)selectedRow.Cells["Notas"].Value
                };

                // Actualizar la entidad en la base de datos
                var seriesRepository = _crudManager.GetRepository<Series>("Series");
                seriesRepository.Update(updatedSeries);

                MessageBox.Show("Serie actualizada correctamente.");
            }
        }

        private void EliminarSeries_Click(object sender, EventArgs e)
        {
            string tableName = comboBoxTables.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(tableName))
            {
                MessageBox.Show("Por favor, selecciona una tabla.");
                return;
            }

            // Obtener la fila seleccionada
            var selectedRow = dataGridView1.SelectedRows[0];
            if (selectedRow != null)
            {
                // Obtener el ID de la serie seleccionada
                int id = (int)selectedRow.Cells["ID_Series"].Value;

                // Eliminar la serie de la base de datos
                var seriesRepository = _crudManager.GetRepository<Series>("Series");
                seriesRepository.Delete(id);

                // Eliminar la fila del DataGridView
                dataGridView1.Rows.Remove(selectedRow);

                MessageBox.Show("Serie eliminada correctamente.");
            }
        }

        private void CrearObjetivos_Click(object sender, EventArgs e)
        {
            try
            {
                string tableName = comboBoxTables.SelectedItem?.ToString();

                if (string.IsNullOrEmpty(tableName) || tableName != "Objetivos")
                {
                    MessageBox.Show("Por favor, selecciona la tabla 'Objetivos'.");
                    return;
                }

                // Crear una nueva instancia de objetivo con valores predeterminados
                Goal newGoal = new Goal
                {
                    ID_Objetivos = 0,  // Valor predeterminado
                    id_powerlifter = 0,
                    SetsEsperados = 0,
                    RepsEsperados = 0,
                    CargaEstimada = 0.0m,
                    notas = ""
                };

                // Agregar el nuevo objetivo a la lista enlazada
                if (goalList == null)
                {
                    goalList = new BindingList<Goal>();
                    dataGridView1.DataSource = goalList;
                }

                goalList.Add(newGoal);

                // Establecer el foco en la primera celda de la nueva fila
                dataGridView1.CurrentCell = dataGridView1.Rows[goalList.Count - 1].Cells["SetsEsperados"];
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al crear el objetivo: {ex.Message}");
            }
        }

        private void ActualizarObjetivos_Click(object sender, EventArgs e)
        {
            string tableName = comboBoxTables.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(tableName))
            {
                MessageBox.Show("Por favor, selecciona una tabla.");
                return;
            }

            // Obtener la fila seleccionada
            var selectedRow = dataGridView1.SelectedRows[0];
            if (selectedRow != null)
            {
                // Crear la entidad a partir de los datos en la fila
                var updatedGoal = new Goal
                {
                    ID_Objetivos = (int)selectedRow.Cells["ID_Objetivos"].Value,
                    id_powerlifter = (int)selectedRow.Cells["ID_Powerlifter"].Value,
                    SetsEsperados = (int)selectedRow.Cells["SetsEsperados"].Value,
                    RepsEsperados = (int)selectedRow.Cells["RepsEsperados"].Value,
                    CargaEstimada = (decimal)selectedRow.Cells["CargaEstimada"].Value,
                    notas = (string)selectedRow.Cells["Notas"].Value
                };

                // Actualizar la entidad en la base de datos
                var goalRepository = _crudManager.GetRepository<Goal>("Objetivo");
                goalRepository.Update(updatedGoal);

                MessageBox.Show("Objetivo actualizado correctamente.");
            }
        }

        private void eliminarObjetivo_Click(object sender, EventArgs e)
        {
            string tableName = comboBoxTables.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(tableName))
            {
                MessageBox.Show("Por favor, selecciona una tabla.");
                return;
            }

            // Obtener la fila seleccionada
            var selectedRow = dataGridView1.SelectedRows[0];
            if (selectedRow != null)
            {
                // Obtener el ID del objetivo seleccionado
                int id = (int)selectedRow.Cells["ID_Objetivos"].Value;

                // Eliminar el objetivo de la base de datos
                var goalRepository = _crudManager.GetRepository<Goal>("Objetivo");
                goalRepository.Delete(id);

                // Eliminar la fila del DataGridView
                dataGridView1.Rows.Remove(selectedRow);

                MessageBox.Show("Objetivo eliminado correctamente.");
            }
        }

        private void CrearRutinas_Click(object sender, EventArgs e)
        {
            string tableName = comboBoxTables.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(tableName) || tableName != "Rutinas")
            {
                MessageBox.Show("Por favor, selecciona la tabla 'Rutinas'.");
                return;
            }

            try
            {
                // Crear una nueva instancia de la rutina con valores predeterminados
                Routine newRoutine = new Routine
                {
                    id_rutina = 0, // Valor predeterminado
                    descripcion = "", // Campo vacío
                    fecha = 0, // Fecha predeterminada
                    tipoDeEntreno = "", // Tipo de entrenamiento vacío
                    estado = "", // Estado vacío
                    intensidad = "", // Intensidad vacía
                    notas = "" // Notas vacías
                };

                // Agregar la rutina a la lista enlazada (BindingList)
                if (routineList == null)
                {
                    routineList = new BindingList<Routine>();
                    dataGridView1.DataSource = routineList;
                }
                routineList.Add(newRoutine);

                // Enfocar en la celda editable (descripción)
                dataGridView1.CurrentCell = dataGridView1.Rows[routineList.Count - 1].Cells["descripcion"];
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al crear la rutina: {ex.Message}");
            }
        }

        private void ActualizarRutinas_Click(object sender, EventArgs e)
        {
            string tableName = comboBoxTables.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(tableName))
            {
                MessageBox.Show("Por favor, selecciona una tabla.");
                return;
            }

            // Obtener la fila seleccionada
            var selectedRow = dataGridView1.SelectedRows[0];
            if (selectedRow != null)
            {
                // Crear la entidad a partir de los datos en la fila
                var updatedRoutine = new Routine
                {
                    id_rutina = (int)selectedRow.Cells["id_rutina"].Value,
                    descripcion = (string)selectedRow.Cells["descripcion"].Value,
                    fecha = (int)selectedRow.Cells["fecha"].Value,
                    tipoDeEntreno = (string)selectedRow.Cells["tipoDeEntreno"].Value,
                    estado = (string)selectedRow.Cells["estado"].Value,
                    intensidad = (string)selectedRow.Cells["intensidad"].Value,
                    notas = (string)selectedRow.Cells["notas"].Value
                };

                // Actualizar la entidad en la base de datos
                var routineRepository = _crudManager.GetRepository<Routine>("rutinas");
                routineRepository.Update(updatedRoutine);

                MessageBox.Show("Rutina actualizada correctamente.");
            }
        }

        private void EliminarRutinas_Click(object sender, EventArgs e)
        {
            string tableName = comboBoxTables.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(tableName))
            {
                MessageBox.Show("Por favor, selecciona una tabla.");
                return;
            }

            // Obtener la fila seleccionada
            var selectedRow = dataGridView1.SelectedRows[0];
            if (selectedRow != null)
            {
                // Obtener el ID de la rutina seleccionada
                int id = (int)selectedRow.Cells["id_rutina"].Value;

                // Eliminar la rutina de la base de datos
                var routineRepository = _crudManager.GetRepository<Routine>("rutinas");
                routineRepository.Delete(id);

                // Eliminar la fila del DataGridView
                dataGridView1.Rows.Remove(selectedRow);

                MessageBox.Show("Rutina eliminada correctamente.");
            }
        }

        private void CrearPlanNutri_Click(object sender, EventArgs e)
        {
            try
            {
                string tableName = comboBoxTables.SelectedItem?.ToString();

                if (string.IsNullOrEmpty(tableName) || tableName != "Plan Nutricional")
                {
                    MessageBox.Show("Por favor, selecciona la tabla 'Plan Nutricional'.");
                    return;
                }

                // Crear una nueva instancia de Plan Nutricional con valores predeterminados
                NutritionalPlan newNutritionalPlan = new NutritionalPlan
                {
                    id_plan = 0,  // Valor predeterminado
                    id_powerlifter = 0,
                    caloriasConsumidas = "",
                    proteinas = "",
                    carbohidratos = 0.0m,
                    grasas = 0.0m
                };

                // Agregar el nuevo plan nutricional a la lista enlazada
                if (nutritionalPlanList == null)
                {
                    nutritionalPlanList = new BindingList<NutritionalPlan>();
                    dataGridView1.DataSource = nutritionalPlanList;
                }

                nutritionalPlanList.Add(newNutritionalPlan);

                // Establecer el foco en la primera celda de la nueva fila
                dataGridView1.CurrentCell = dataGridView1.Rows[nutritionalPlanList.Count - 1].Cells["caloriasConsumidas"];
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al crear el plan nutricional: {ex.Message}");
            }
        }

        private void ActualizarPlanNutri_Click(object sender, EventArgs e)
        {
            try
            {
                // Verificar si hay una fila seleccionada
                if (dataGridView1.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Por favor, selecciona un plan nutricional para actualizar.");
                    return;
                }

                // Obtener la fila seleccionada
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                // Validar los datos antes de actualizar
                if (string.IsNullOrEmpty(selectedRow.Cells["caloriasConsumidas"].Value?.ToString()) ||
                    string.IsNullOrEmpty(selectedRow.Cells["proteinas"].Value?.ToString()) ||
                    selectedRow.Cells["carbohidratos"].Value == null ||
                    selectedRow.Cells["grasas"].Value == null)
                {
                    MessageBox.Show("Por favor, complete todos los campos.");
                    return;
                }

                // Crear una nueva instancia de NutritionalPlan con los valores de la fila seleccionada
                NutritionalPlan updatedPlan = new NutritionalPlan
                {
                    id_plan = Convert.ToInt32(selectedRow.Cells["id_plan"].Value),
                    id_powerlifter = Convert.ToInt32(selectedRow.Cells["id_powerlifter"].Value),
                    caloriasConsumidas = selectedRow.Cells["caloriasConsumidas"].Value.ToString(),
                    proteinas = selectedRow.Cells["proteinas"].Value.ToString(),
                    carbohidratos = Convert.ToDecimal(selectedRow.Cells["carbohidratos"].Value),
                    grasas = Convert.ToDecimal(selectedRow.Cells["grasas"].Value)
                };

                // Instanciar la capa de acceso a datos
                NutritionalPlanDataAcessLayer dal = new NutritionalPlanDataAcessLayer("your_connection_string");

                // Llamar al método Update para actualizar el plan nutricional
                dal.Update(updatedPlan);

                // Recargar los planes nutricionales
                LoadNutritionalPlan();

                MessageBox.Show("Plan nutricional actualizado exitosamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar el plan nutricional: {ex.Message}");
            }
        }

        private void EliminarPlanNutri_Click(object sender, EventArgs e)
        {
            try
            {
                // Verificar si se ha seleccionado una fila
                if (dataGridView1.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Por favor, selecciona un plan nutricional para eliminar.");
                    return;
                }

                // Obtener la fila seleccionada
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                // Obtener el ID del plan nutricional
                int idPlan = Convert.ToInt32(selectedRow.Cells["id_plan"].Value);

                // Instanciar la capa de acceso a datos
                NutritionalPlanDataAcessLayer dal = new NutritionalPlanDataAcessLayer("your_connection_string");

                // Llamar al método Delete para eliminar el plan nutricional
                dal.Delete(idPlan);

                // Recargar los planes nutricionales
                LoadNutritionalPlan();

                MessageBox.Show("Plan nutricional eliminado exitosamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar el plan nutricional: {ex.Message}");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string tableName = comboBoxTables.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(tableName))
            {
                MessageBox.Show("Por favor, selecciona una tabla.");
                return;
            }

            if (tableName != "Powerlifters")
            {
                MessageBox.Show("Por favor, selecciona la tabla 'Powerlifters'.");
                return;
            }

            var repository = _crudManager.GetRepository<Powerlifter>("Powerlifters");
            if (repository == null)
            {
                MessageBox.Show($"Repositorio para la tabla '{tableName}' no encontrado.");
                return;
            }

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                // Evitar la fila de nueva entrada
                if (row.IsNewRow) continue;

                // Si el ID es 0, significa que no está guardado todavía
                if (row.Cells["ID_powerlifter"].Value == null || Convert.ToInt32(row.Cells["ID_powerlifter"].Value) == 0)
                {
                    // Obtener los valores editados por el usuario
                    var edadValue = row.Cells["edad"].Value;
                    var pesoValue = row.Cells["peso"].Value;
                    var alturaValue = row.Cells["altura"].Value;

                    if (edadValue != null && pesoValue != null && alturaValue != null)
                    {
                        if (int.TryParse(edadValue.ToString(), out int edad) &&
                            decimal.TryParse(pesoValue.ToString(), out decimal peso) &&
                            decimal.TryParse(alturaValue.ToString(), out decimal altura))
                        {
                            Powerlifter newPowerlifter = new Powerlifter
                            {
                                edad = edad,
                                peso = peso,
                                altura = altura
                            };

                            try
                            {
                                // Guardar en la base
                                repository.Create(newPowerlifter);

                                // Asignar el ID generado al DataGridView
                                row.Cells["ID_powerlifter"].Value = newPowerlifter.ID_powerlifter;

                                MessageBox.Show($"Powerlifter guardado exitosamente con ID {newPowerlifter.ID_powerlifter}.");
                            }
                            catch (Exception ex)
                            {
                                LogError(ex);
                                MessageBox.Show($"Error al guardar el Powerlifter: {ex.Message}");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Campos numéricos inválidos (edad, peso, altura).");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Por favor, completa todos los campos obligatorios (edad, peso, altura).");
                    }
                }
            }

            // Opcional: recargar la lista desde la base
            LoadPowerlifters();
        }

        private void GuadarUser_Click(object sender, EventArgs e)
        {
            string tableName = comboBoxTables.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(tableName))
            {
                MessageBox.Show("Por favor, selecciona una tabla.");
                return;
            }

            if (tableName != "Usuarios")
            {
                MessageBox.Show("Por favor, selecciona la tabla 'Usuarios'.");
                return;
            }

            var repository = _crudManager.GetRepository<User>("Usuarios");
            if (repository == null)
            {
                MessageBox.Show($"Repositorio para la tabla '{tableName}' no encontrado.");
                return;
            }

            // Recorrer las filas para guardar los usuarios no guardados
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;

                if (row.Cells["id_usuario"].Value != null && (int)row.Cells["id_usuario"].Value == 0)
                {
                    string usuario = row.Cells["usuario"].Value?.ToString();
                    string contraseña = row.Cells["contraseña"].Value?.ToString(); // Esta es la contraseña en texto claro
                    string rol = row.Cells["rol"].Value?.ToString();
                    DateTime fechaCreacion = DateTime.Parse(row.Cells["fecha_creacion"].Value.ToString());
                    int idPowerlifter = (int)row.Cells["id_powerlifter"].Value;

                    if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(contraseña))
                    {
                        MessageBox.Show("Por favor llena todos los campos obligatorios.");
                        continue;
                    }

                    User newUser = new User
                    {
                        usuario = usuario,
                        contraseña = contraseña, // Se va a hashear en el Create antes de guardar
                        rol = rol,
                        fecha_creacion = fechaCreacion,
                        id_powerlifter = idPowerlifter
                    };

                    try
                    {
                        repository.Create(newUser); // Aquí se aplica el hash en la capa DAL
                        row.Cells["id_usuario"].Value = newUser.id_usuario;
                        MessageBox.Show($"Usuario guardado exitosamente con ID {newUser.id_usuario}.");
                    }
                    catch (Exception ex)
                    {
                        LogError(ex);
                        MessageBox.Show($"Error al guardar el usuario: {ex.Message}");
                    }
                }
            }

            // Recargar la lista si lo deseas
            LoadUsers();
        }

        private void GuardarRutinas_Click(object sender, EventArgs e)
        {
            string tableName = comboBoxTables.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(tableName) || tableName != "Rutinas")
            {
                MessageBox.Show("Por favor, selecciona la tabla 'Rutinas'.");
                return;
            }

            try
            {
                // Iterar sobre las filas del DataGridView
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    // Saltar filas nuevas vacías
                    if (row.IsNewRow) continue;

                    // Verificar si la fila tiene valores nuevos o editados
                    if (row.Cells["id_rutina"].Value == null || Convert.ToInt32(row.Cells["id_rutina"].Value) == 0)
                    {
                        // Obtener los valores de las celdas
                        var descripcionValue = row.Cells["descripcion"].Value?.ToString();
                        var fechaValue = row.Cells["fecha"].Value != null
                                         ? Convert.ToInt32(row.Cells["fecha"].Value)
                                         : 0; // Si la fecha está vacía, asignar un valor predeterminado

                        var tipoDeEntrenoValue = row.Cells["tipoDeEntreno"].Value?.ToString();
                        var estadoValue = row.Cells["estado"].Value?.ToString();
                        var intensidadValue = row.Cells["intensidad"].Value?.ToString();
                        var notasValue = row.Cells["notas"].Value?.ToString();

                        // Validar que los valores no sean nulos
                        if (!string.IsNullOrEmpty(descripcionValue) && fechaValue != 0 && !string.IsNullOrEmpty(tipoDeEntrenoValue))
                        {
                            // Crear una nueva instancia de rutina
                            Routine newRoutine = new Routine
                            {
                                descripcion = descripcionValue,
                                fecha = fechaValue,  // Guardar la fecha como un valor entero
                                tipoDeEntreno = tipoDeEntrenoValue,
                                estado = estadoValue,
                                intensidad = intensidadValue,
                                notas = notasValue
                            };

                            // Guardar en la base de datos
                            var dal = new RoutineDataAcessLayer(_connectionString);
                            dal.Create(newRoutine);

                            // Actualizar el ID generado en la fila
                            row.Cells["id_rutina"].Value = newRoutine.id_rutina;

                            MessageBox.Show("Rutina guardada exitosamente.");
                        }
                        else
                        {
                            MessageBox.Show("Por favor, completa todos los campos requeridos (descripción, fecha, tipo de entreno).");
                        }
                    }
                }

                // Opcional: Recargar los datos desde la base de datos
                LoadRoutine();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar la rutina: {ex.Message}");
            }
        }

        private void GuardarEntrenamientos_Click(object sender, EventArgs e)
        {
            string tableName = comboBoxTables.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(tableName) || tableName != "Ejercicios")
            {
                MessageBox.Show("Por favor, selecciona la tabla 'Ejercicios'.");
                return;
            }

            try
            {
                // Iterar sobre las filas del DataGridView
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    // Saltar filas nuevas vacías
                    if (row.IsNewRow) continue;

                    // Verificar si la fila tiene valores nuevos o editados
                    if (row.Cells["ID_ejercicio"].Value == null || Convert.ToInt32(row.Cells["ID_ejercicio"].Value) == 0)
                    {
                        // Obtener los valores de las celdas
                        var idEntrenamientoValue = row.Cells["id_entrenamiento"].Value != null
                            ? Convert.ToInt32(row.Cells["id_entrenamiento"].Value)
                            : 0;

                        var tipoMovimientoValue = row.Cells["tipoMovimiento"].Value?.ToString();
                        var setssValue = row.Cells["setss"].Value != null
                            ? Convert.ToInt32(row.Cells["setss"].Value)
                            : 0;

                        var repsValue = row.Cells["reps"].Value != null
                            ? Convert.ToInt32(row.Cells["reps"].Value)
                            : 0;

                        var cargaUtilizadaValue = row.Cells["cargaUtilizada"].Value != null
                            ? Convert.ToDecimal(row.Cells["cargaUtilizada"].Value)
                            : 0.0m;

                        var descansoEntreSeriesValue = row.Cells["descansoEntreSeries"].Value != null
                            ? Convert.ToInt32(row.Cells["descansoEntreSeries"].Value)
                            : 0;

                        var tempoValue = row.Cells["tempo"].Value?.ToString();
                        var RPEValue = row.Cells["RPE"].Value != null
                            ? Convert.ToInt32(row.Cells["RPE"].Value)
                            : 0;

                        var RIRValue = row.Cells["RIR"].Value != null
                            ? Convert.ToInt32(row.Cells["RIR"].Value)
                            : 0;

                        var notasValue = row.Cells["notas"].Value?.ToString();

                        // Validar que los valores no sean nulos
                        if (idEntrenamientoValue != 0 && !string.IsNullOrEmpty(tipoMovimientoValue) && setssValue != 0)
                        {
                            // Crear una nueva instancia de ejercicio
                            Exercise newExercise = new Exercise
                            {
                                id_entrenamiento = idEntrenamientoValue,
                                tipoMovimiento = tipoMovimientoValue,
                                setss = setssValue,
                                reps = repsValue,
                                cargaUtilizada = cargaUtilizadaValue,
                                descansoEntreSeries = descansoEntreSeriesValue,
                                tempo = tempoValue,
                                RPE = RPEValue,
                                RIR = RIRValue,
                                notas = notasValue
                            };

                            // Guardar en la base de datos
                            var ejercicioRepository = new ExerciseDataAcessLayer(_connectionString);
                            ejercicioRepository.Create(newExercise);

                            // Actualizar el ID generado en la fila
                            row.Cells["ID_ejercicio"].Value = newExercise.ID_ejercicio;

                            MessageBox.Show("Ejercicio guardado exitosamente.");
                        }
                        else
                        {
                            MessageBox.Show("Por favor, completa todos los campos requeridos (ID Entrenamiento, Tipo de Movimiento, Sets).");
                        }
                    }
                }

                // Opcional: Recargar los datos desde la base de datos
                LoadExercises();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el ejercicio: {ex.Message}");
            }
        }

        private void GuardarEjercicios_Click(object sender, EventArgs e)
        {
            string tableName = comboBoxTables.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(tableName) || tableName != "Ejercicios")
            {
                MessageBox.Show("Por favor, selecciona la tabla 'Ejercicios'.");
                return;
            }

            try
            {
                // Iterar sobre las filas del DataGridView
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    // Saltar filas nuevas vacías
                    if (row.IsNewRow) continue;

                    // Verificar si la fila tiene valores nuevos o editados
                    if (row.Cells["ID_ejercicio"].Value == null || Convert.ToInt32(row.Cells["ID_ejercicio"].Value) == 0)
                    {
                        // Obtener los valores de las celdas
                        var idEntrenamientoValue = row.Cells["id_entrenamiento"].Value != null
                            ? Convert.ToInt32(row.Cells["id_entrenamiento"].Value)
                            : 0;

                        var tipoMovimientoValue = row.Cells["tipoMovimiento"].Value?.ToString();
                        var setssValue = row.Cells["setss"].Value != null
                            ? Convert.ToInt32(row.Cells["setss"].Value)
                            : 0;

                        var repsValue = row.Cells["reps"].Value != null
                            ? Convert.ToInt32(row.Cells["reps"].Value)
                            : 0;

                        var cargaUtilizadaValue = row.Cells["cargaUtilizada"].Value != null
                            ? Convert.ToDecimal(row.Cells["cargaUtilizada"].Value)
                            : 0.0m;

                        var descansoEntreSeriesValue = row.Cells["descansoEntreSeries"].Value != null
                            ? Convert.ToInt32(row.Cells["descansoEntreSeries"].Value)
                            : 0;

                        var tempoValue = row.Cells["tempo"].Value?.ToString();
                        var RPEValue = row.Cells["RPE"].Value != null
                            ? Convert.ToInt32(row.Cells["RPE"].Value)
                            : 0;

                        var RIRValue = row.Cells["RIR"].Value != null
                            ? Convert.ToInt32(row.Cells["RIR"].Value)
                            : 0;

                        var notasValue = row.Cells["notas"].Value?.ToString();

                        // Validar que los valores no sean nulos
                        if (idEntrenamientoValue != 0 && !string.IsNullOrEmpty(tipoMovimientoValue) && setssValue != 0)
                        {
                            // Crear una nueva instancia de ejercicio
                            Exercise newExercise = new Exercise
                            {
                                id_entrenamiento = idEntrenamientoValue,
                                tipoMovimiento = tipoMovimientoValue,
                                setss = setssValue,
                                reps = repsValue,
                                cargaUtilizada = cargaUtilizadaValue,
                                descansoEntreSeries = descansoEntreSeriesValue,
                                tempo = tempoValue,
                                RPE = RPEValue,
                                RIR = RIRValue,
                                notas = notasValue
                            };

                            // Guardar el nuevo ejercicio en la base de datos
                            var ejercicioRepository = new ExerciseDataAcessLayer(_connectionString);
                            ejercicioRepository.Create(newExercise);

                            // Actualizar el ID generado en la fila
                            row.Cells["ID_ejercicio"].Value = newExercise.ID_ejercicio;

                            MessageBox.Show("Ejercicio guardado exitosamente.");
                        }
                        else
                        {
                            MessageBox.Show("Por favor, completa todos los campos requeridos (ID Entrenamiento, Tipo de Movimiento, Sets).");
                        }
                    }
                }

                // Opcional: Recargar los datos desde la base de datos
                LoadExercises();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el ejercicio: {ex.Message}");
            }
        }

        private void GuardarSesiones_Click(object sender, EventArgs e)
        {
            string tableName = comboBoxTables.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(tableName) || tableName != "Sesiones")
            {
                MessageBox.Show("Por favor, selecciona la tabla 'Sesiones'.");
                return;
            }

            try
            {
                // Iterar sobre las filas del DataGridView
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    // Saltar filas nuevas vacías
                    if (row.IsNewRow) continue;

                    // Verificar si la fila tiene valores nuevos o editados
                    if (row.Cells["ID_Sesion"].Value == null || Convert.ToInt32(row.Cells["ID_Sesion"].Value) == 0)
                    {
                        // Obtener los valores de las celdas
                        var idPowerlifterValue = row.Cells["id_powerlifter"].Value != null
                            ? Convert.ToInt32(row.Cells["id_powerlifter"].Value)
                            : 0;

                        var tipoDeSesionValue = row.Cells["TipoDeSesion"].Value?.ToString();
                        var intensidadValue = row.Cells["intensidad"].Value?.ToString();
                        var estadoDelAtletaValue = row.Cells["estadoDelAtleta"].Value?.ToString();
                        var evaluacionValue = row.Cells["evaluacion"].Value?.ToString();

                        // Validar que los valores no sean nulos
                        if (idPowerlifterValue != 0 && !string.IsNullOrEmpty(tipoDeSesionValue))
                        {
                            // Crear una nueva instancia de sesión
                            TrainingSession newSession = new TrainingSession
                            {
                                id_powerlifter = idPowerlifterValue,
                                TipoDeSesion = tipoDeSesionValue,
                                intensidad = intensidadValue,
                                estadoDelAtleta = estadoDelAtletaValue,
                                evaluacion = evaluacionValue
                            };

                            // Guardar en la base de datos
                            var sessionRepository = new TrainingSessionDataAccessLayer(_connectionString);
                            sessionRepository.Create(newSession);

                            // Actualizar el ID generado en la fila
                            row.Cells["ID_Sesion"].Value = newSession.ID_Sesion;

                            MessageBox.Show("Sesión guardada exitosamente.");
                        }
                        else
                        {
                            MessageBox.Show("Por favor, completa todos los campos requeridos (ID Powerlifter, Tipo de Sesión).");
                        }
                    }
                }

                // Opcional: Recargar los datos desde la base de datos
                LoadSessions();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar la sesión: {ex.Message}");
            }
        }

        private void GuardarCarga_Click(object sender, EventArgs e)
        {
            string tableName = comboBoxTables.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(tableName) || tableName != "Carga")
            {
                MessageBox.Show("Por favor, selecciona la tabla 'Carga'.");
                return;
            }

            try
            {
                // Iterar sobre las filas del DataGridView
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    // Saltar filas nuevas vacías
                    if (row.IsNewRow) continue;

                    // Verificar si la fila tiene valores nuevos o editados
                    if (row.Cells["ID_carga"].Value == null || Convert.ToInt32(row.Cells["ID_carga"].Value) == 0)
                    {
                        // Obtener los valores de las celdas
                        var pesoValue = row.Cells["peso"].Value != null
                            ? Convert.ToDecimal(row.Cells["peso"].Value)
                            : 0.0m;

                        var repsValue = row.Cells["reps"].Value != null
                            ? Convert.ToInt32(row.Cells["reps"].Value)
                            : 0;

                        var setssValue = row.Cells["setss"].Value != null
                            ? Convert.ToInt32(row.Cells["setss"].Value)
                            : 0;

                        var RPEValue = row.Cells["RPE"].Value != null
                            ? Convert.ToInt32(row.Cells["RPE"].Value)
                            : 0;

                        var RIRValue = row.Cells["RIR"].Value != null
                            ? Convert.ToInt32(row.Cells["RIR"].Value)
                            : 0;

                        var notasValue = row.Cells["notas"].Value?.ToString();

                        // Validar que los valores no sean nulos
                        if (pesoValue != 0 && repsValue != 0 && setssValue != 0)
                        {
                            // Crear una nueva instancia de carga
                            Load newLoad = new Load
                            {
                                peso = pesoValue,
                                reps = repsValue,
                                setss = setssValue,
                                RPE = RPEValue,
                                RIR = RIRValue,
                                notas = notasValue
                            };

                            // Guardar en la base de datos
                            var loadRepository = new LoadDataAcessLayer(_connectionString);
                            loadRepository.Create(newLoad);

                            // Actualizar el ID generado en la fila
                            row.Cells["ID_carga"].Value = newLoad.ID_carga;

                            MessageBox.Show("Carga guardada exitosamente.");
                        }
                        else
                        {
                            MessageBox.Show("Por favor, completa todos los campos requeridos (Peso, Reps, Sets).");
                        }
                    }
                }

                // Opcional: Recargar los datos desde la base de datos
                LoadLoads();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar la carga: {ex.Message}");
            }
        }

        private void GuardarSeries_Click(object sender, EventArgs e)
        {
            string tableName = comboBoxTables.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(tableName) || tableName != "Series")
            {
                MessageBox.Show("Por favor, selecciona la tabla 'Series'.");
                return;
            }

            try
            {
                // Iterar sobre las filas del DataGridView
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    // Saltar filas nuevas vacías
                    if (row.IsNewRow) continue;

                    // Verificar si la fila tiene valores nuevos o editados
                    if (row.Cells["id_series"].Value == null || Convert.ToInt32(row.Cells["id_series"].Value) == 0)
                    {
                        // Obtener los valores de las celdas
                        var idEjercicioCargaValue = row.Cells["id_ejercicio_carga"].Value != null
                            ? Convert.ToInt32(row.Cells["id_ejercicio_carga"].Value)
                            : 0;

                        var setsValue = row.Cells["sets"].Value != null
                            ? Convert.ToInt32(row.Cells["sets"].Value)
                            : 0;

                        var sensacionesValue = row.Cells["sensaciones"].Value?.ToString();
                        var notasValue = row.Cells["notas"].Value?.ToString();

                        // Validar que los valores no sean nulos
                        if (idEjercicioCargaValue != 0 && setsValue != 0)
                        {
                            // Crear una nueva instancia de serie
                            Series newSeries = new Series
                            {
                                id_ejercicio_carga = idEjercicioCargaValue,
                                sets = setsValue,
                                sensaciones = sensacionesValue,
                                notas = notasValue
                            };

                            // Guardar en la base de datos
                            var seriesRepository = new SeriesDataAccessLayer(_connectionString);
                            seriesRepository.Create(newSeries);

                            // Actualizar el ID generado en la fila
                            row.Cells["id_series"].Value = newSeries.id_series;

                            MessageBox.Show("Serie guardada exitosamente.");
                        }
                        else
                        {
                            MessageBox.Show("Por favor, completa todos los campos requeridos (ID Ejercicio Carga, Sets).");
                        }
                    }
                }

                // Opcional: Recargar los datos desde la base de datos
                LoadSeries();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar la serie: {ex.Message}");
            }
        }

        private void GuardarObjetivos_Click(object sender, EventArgs e)
        {
            string tableName = comboBoxTables.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(tableName) || tableName != "Objetivos")
            {
                MessageBox.Show("Por favor, selecciona la tabla 'Objetivos'.");
                return;
            }

            try
            {
                // Iterar sobre las filas del DataGridView
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    // Saltar filas nuevas vacías
                    if (row.IsNewRow) continue;

                    // Verificar si la fila tiene valores nuevos o editados
                    if (row.Cells["ID_Objetivos"].Value == null || Convert.ToInt32(row.Cells["ID_Objetivos"].Value) == 0)
                    {
                        // Obtener los valores de las celdas
                        var idPowerlifterValue = row.Cells["id_powerlifter"].Value != null
                            ? Convert.ToInt32(row.Cells["id_powerlifter"].Value)
                            : 0;

                        var setsEsperadosValue = row.Cells["SetsEsperados"].Value != null
                            ? Convert.ToInt32(row.Cells["SetsEsperados"].Value)
                            : 0;

                        var repsEsperadosValue = row.Cells["RepsEsperados"].Value != null
                            ? Convert.ToInt32(row.Cells["RepsEsperados"].Value)
                            : 0;

                        var cargaEstimadaValue = row.Cells["CargaEstimada"].Value != null
                            ? Convert.ToDecimal(row.Cells["CargaEstimada"].Value)
                            : 0.0m;

                        var notasValue = row.Cells["notas"].Value?.ToString();

                        // Validar que los valores no sean nulos
                        if (idPowerlifterValue != 0 && setsEsperadosValue != 0 && repsEsperadosValue != 0)
                        {
                            // Crear una nueva instancia de objetivo
                            Goal newGoal = new Goal
                            {
                                id_powerlifter = idPowerlifterValue,
                                SetsEsperados = setsEsperadosValue,
                                RepsEsperados = repsEsperadosValue,
                                CargaEstimada = cargaEstimadaValue,
                                notas = notasValue
                            };

                            // Guardar en la base de datos
                            var goalRepository = new GoalDataAccessLayer(_connectionString);
                            goalRepository.Create(newGoal);

                            // Actualizar el ID generado en la fila
                            row.Cells["ID_Objetivos"].Value = newGoal.ID_Objetivos;

                            MessageBox.Show("Objetivo guardado exitosamente.");
                        }
                        else
                        {
                            MessageBox.Show("Por favor, completa todos los campos requeridos (ID Powerlifter, Sets Esperados, Reps Esperados).");
                        }
                    }
                }

                // Opcional: Recargar los datos desde la base de datos
                LoadGoals();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el objetivo: {ex.Message}");
            }
        }

        private void GuardarPlan_Click(object sender, EventArgs e)
        {
            string tableName = comboBoxTables.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(tableName) || tableName != "Plan Nutricional")
            {
                MessageBox.Show("Por favor, selecciona la tabla 'Plan Nutricional'.");
                return;
            }

            try
            {
                // Iterar sobre las filas del DataGridView
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    // Saltar filas nuevas vacías
                    if (row.IsNewRow) continue;

                    // Verificar si la fila tiene valores nuevos o editados
                    if (row.Cells["id_plan"].Value == null || Convert.ToInt32(row.Cells["id_plan"].Value) == 0)
                    {
                        // Obtener los valores de las celdas
                        var idPowerlifterValue = row.Cells["id_powerlifter"].Value != null
                            ? Convert.ToInt32(row.Cells["id_powerlifter"].Value)
                            : 0;

                        var caloriasConsumidasValue = row.Cells["caloriasConsumidas"].Value?.ToString();
                        var proteinasValue = row.Cells["proteinas"].Value?.ToString();
                        var carbohidratosValue = row.Cells["carbohidratos"].Value != null
                            ? Convert.ToDecimal(row.Cells["carbohidratos"].Value)
                            : 0.0m;

                        var grasasValue = row.Cells["grasas"].Value != null
                            ? Convert.ToDecimal(row.Cells["grasas"].Value)
                            : 0.0m;

                        // Validar que los valores no sean nulos
                        if (idPowerlifterValue != 0 && !string.IsNullOrEmpty(caloriasConsumidasValue))
                        {
                            // Crear una nueva instancia de Plan Nutricional
                            NutritionalPlan newPlan = new NutritionalPlan
                            {
                                id_powerlifter = idPowerlifterValue,
                                caloriasConsumidas = caloriasConsumidasValue,
                                proteinas = proteinasValue,
                                carbohidratos = carbohidratosValue,
                                grasas = grasasValue
                            };

                            // Guardar en la base de datos
                            var nutritionalPlanRepository = new NutritionalPlanDataAcessLayer(_connectionString);
                            nutritionalPlanRepository.Create(newPlan);

                            // Actualizar el ID generado en la fila
                            row.Cells["id_plan"].Value = newPlan.id_plan;

                            MessageBox.Show("Plan nutricional guardado exitosamente.");
                        }
                        else
                        {
                            MessageBox.Show("Por favor, completa todos los campos requeridos (ID Powerlifter, Calorías Consumidas).");
                        }
                    }
                }

                // Opcional: Recargar los datos desde la base de datos
                LoadNutritionalPlan();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el plan nutricional: {ex.Message}");
            }
        }
    }
    
}

