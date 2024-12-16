using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GestorFORMS
{
    internal class NutritionalPlanDataAcessLayer : ICrud<NutritionalPlan>
    {
        private readonly string _connectionString;

        public NutritionalPlanDataAcessLayer(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Método para obtener todos los planes nutricionales
        public List<NutritionalPlan> GetAll()
        {
            List<NutritionalPlan> nutritionalPlans = new List<NutritionalPlan>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT id_plan, id_powerlifter, caloriasConsumidas, proteinas, carbohidratos, grasas FROM PlanAlimenticio";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            NutritionalPlan nutritionalPlan = new NutritionalPlan
                            {
                                id_plan = (int)reader["id_plan"],
                                id_powerlifter = (int)reader["id_powerlifter"],
                                caloriasConsumidas = reader["caloriasConsumidas"] as string,
                                proteinas = reader["proteinas"] as string,
                                carbohidratos = (decimal)reader["carbohidratos"],
                                grasas = (decimal)reader["grasas"]
                            };
                            nutritionalPlans.Add(nutritionalPlan);
                        }
                    }
                }
            }

            return nutritionalPlans;
        }

        // Método para crear un nuevo plan nutricional
        public void Create(NutritionalPlan nutritionalPlan)
        {
            // Validar que no haya campos obligatorios nulos
            if (nutritionalPlan.id_powerlifter == 0 || string.IsNullOrEmpty(nutritionalPlan.caloriasConsumidas))
            {
                MessageBox.Show("Faltan datos obligatorios (por ejemplo, calorías consumidas).");
                return;
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "INSERT INTO plan_alimenticio (id_powerlifter, caloriasConsumidas, proteinas, carbohidratos, grasas) " +
                               "VALUES (@id_powerlifter, @caloriasConsumidas, @proteinas, @carbohidratos, @grasas)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id_powerlifter", nutritionalPlan.id_powerlifter);
                    command.Parameters.AddWithValue("@caloriasConsumidas", nutritionalPlan.caloriasConsumidas ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@proteinas", nutritionalPlan.proteinas ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@carbohidratos", nutritionalPlan.carbohidratos);
                    command.Parameters.AddWithValue("@grasas", nutritionalPlan.grasas);
                    command.ExecuteNonQuery();
                }
            }

        }

        // Método para actualizar un plan nutricional
        public void Update(NutritionalPlan nutritionalPlan)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "UPDATE plan_alimenticio SET id_powerlifter = @id_powerlifter, caloriasConsumidas = @caloriasConsumidas, " +
                               "proteinas = @proteinas, carbohidratos = @carbohidratos, grasas = @grasas " +
                               "WHERE id_plan = @id_plan";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id_plan", nutritionalPlan.id_plan);
                    command.Parameters.AddWithValue("@id_powerlifter", nutritionalPlan.id_powerlifter);
                    command.Parameters.AddWithValue("@caloriasConsumidas", nutritionalPlan.caloriasConsumidas);
                    command.Parameters.AddWithValue("@proteinas", nutritionalPlan.proteinas);
                    command.Parameters.AddWithValue("@carbohidratos", nutritionalPlan.carbohidratos);
                    command.Parameters.AddWithValue("@grasas", nutritionalPlan.grasas);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Método para eliminar un plan nutricional
        public void Delete(int id_plan)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "DELETE FROM plan_alimenticio WHERE id_plan = @id_plan";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id_plan", id_plan);
                    command.ExecuteNonQuery();
                }
            }

        }
    }
}
