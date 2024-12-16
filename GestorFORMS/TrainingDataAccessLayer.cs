using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorFORMS
{
    internal class TrainingDataAccessLayer : ICrud<Training>
    {
        private string _connectionString;

        public TrainingDataAccessLayer(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Training> GetAll()
        {
            List<Training> trainings = new List<Training>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT ID_entrenamiento, id_powerlifter, id_rutina, duracion, sensaciones, notas FROM entrenamiento";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Training training = new Training
                            {
                                ID_entrenamiento = (int)reader["ID_entrenamiento"],
                                id_powerlifter = (int)reader["id_powerlifter"],
                                id_rutina = (int)reader["id_rutina"],
                                duracion = (decimal)reader["duracion"],
                                sensaciones = reader["sensaciones"].ToString(),
                                notas = reader["notas"].ToString()
                            };
                            trainings.Add(training);
                        }
                    }
                }
            }

            return trainings;
        }

        public void Create(Training training)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "INSERT INTO entrenamiento (id_powerlifter, id_rutina, duracion, sensaciones, notas) VALUES (@id_powerlifter, @id_rutina, @duracion, @sensaciones, @notas)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id_powerlifter", training.id_powerlifter);
                    command.Parameters.AddWithValue("@id_rutina", training.id_rutina);
                    command.Parameters.AddWithValue("@duracion", training.duracion);
                    command.Parameters.AddWithValue("@sensaciones", training.sensaciones);
                    command.Parameters.AddWithValue("@notas", training.notas);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Update(Training training)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "UPDATE entrenamiento SET id_powerlifter = @id_powerlifter, id_rutina = @id_rutina, duracion = @duracion, sensaciones = @sensaciones, notas = @notas WHERE ID_entrenamiento = @ID_entrenamiento";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID_entrenamiento", training.ID_entrenamiento);
                    command.Parameters.AddWithValue("@id_powerlifter", training.id_powerlifter);
                    command.Parameters.AddWithValue("@id_rutina", training.id_rutina);
                    command.Parameters.AddWithValue("@duracion", training.duracion);
                    command.Parameters.AddWithValue("@sensaciones", training.sensaciones);
                    command.Parameters.AddWithValue("@notas", training.notas);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int ID_entrenamiento)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "DELETE FROM entrenamiento WHERE ID_entrenamiento = @ID_entrenamiento";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID_entrenamiento", ID_entrenamiento);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
