using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorFORMS
{
    internal class PowerlifterDataAcessLayer : ICrud<Powerlifter>
    {
        private string _connectionString;

        public PowerlifterDataAcessLayer(string connectionString)
        {
            _connectionString = connectionString;
        }



        public List<Powerlifter> GetAll()
        {
            List<Powerlifter> powerlifters = new List<Powerlifter>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT ID_powerlifter, edad, peso, altura FROM powerlifter";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Powerlifter powerlifter = new Powerlifter
                            {
                                ID_powerlifter = (int)reader["ID_powerlifter"],
                                edad = (int)reader["edad"],
                                peso = (decimal)reader["peso"],
                                altura = (decimal)reader["altura"]
                            };
                            powerlifters.Add(powerlifter);
                        }
                    }
                }
            }

            return powerlifters;
        }

        public void Create(Powerlifter powerlifter)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // No incluimos 'ID_powerlifter' ya que es autoincremental
                string query = "INSERT INTO powerlifter (edad, peso, altura) VALUES (@edad, @peso, @altura)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Agregar parámetros para edad, peso y altura
                    command.Parameters.AddWithValue("@edad", powerlifter.edad);
                    command.Parameters.AddWithValue("@peso", powerlifter.peso);
                    command.Parameters.AddWithValue("@altura", powerlifter.altura);

                    // Ejecutar el comando sin especificar el ID_powerlifter
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Update(Powerlifter powerlifter)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "UPDATE powerlifter SET edad = @edad, peso = @peso, altura = @altura WHERE ID_powerlifter = @ID_powerlifter";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID_powerlifter", powerlifter.ID_powerlifter);
                    command.Parameters.AddWithValue("@edad", powerlifter.edad);
                    command.Parameters.AddWithValue("@peso", powerlifter.peso);
                    command.Parameters.AddWithValue("@altura", powerlifter.altura);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int ID_powerlifter)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "DELETE FROM powerlifter WHERE ID_powerlifter = @ID_powerlifter";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID_powerlifter", ID_powerlifter);
                    command.ExecuteNonQuery();
                }
            }
        }

    }
}


