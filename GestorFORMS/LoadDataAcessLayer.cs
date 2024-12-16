using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorFORMS
{
    internal class LoadDataAcessLayer : ICrud<Load> 
    {
        private string _connectionString;

        public LoadDataAcessLayer(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Load> GetAll()
        {
            List<Load> loads = new List<Load>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT ID_carga, peso, reps, setss, RPE, RIR, notas FROM carga";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Load load = new Load
                            {
                                ID_carga = (int)reader["ID_carga"],
                                peso = (decimal)reader["peso"],
                                reps = (int)reader["reps"],
                                setss = (int)reader["setss"],
                                RPE = (int)reader["RPE"],
                                RIR = (int)reader["RIR"],
                                notas = reader["notas"]?.ToString()
                            };
                            loads.Add(load);
                        }
                    }
                }
            }

            return loads;
        }

        public void Create(Load load)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "INSERT INTO carga (peso, reps, setss, RPE, RIR, notas) VALUES (@peso, @reps, @setss, @RPE, @RIR, @notas)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@peso", load.peso);
                    command.Parameters.AddWithValue("@reps", load.reps);
                    command.Parameters.AddWithValue("@setss", load.setss);
                    command.Parameters.AddWithValue("@RPE", load.RPE);
                    command.Parameters.AddWithValue("@RIR", load.RIR);
                    command.Parameters.AddWithValue("@notas", load.notas ?? (object)DBNull.Value);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Update(Load load)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "UPDATE carga SET peso = @peso, reps = @reps, setss = @setss, RPE = @RPE, RIR = @RIR, notas = @notas WHERE ID_carga = @ID_carga";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID_carga", load.ID_carga);
                    command.Parameters.AddWithValue("@peso", load.peso);
                    command.Parameters.AddWithValue("@reps", load.reps);
                    command.Parameters.AddWithValue("@setss", load.setss);
                    command.Parameters.AddWithValue("@RPE", load.RPE);
                    command.Parameters.AddWithValue("@RIR", load.RIR);
                    command.Parameters.AddWithValue("@notas", load.notas ?? (object)DBNull.Value);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int ID_carga)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "DELETE FROM carga WHERE ID_carga = @ID_carga";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID_carga", ID_carga);
                    command.ExecuteNonQuery();
                }
            }
        }

    }
}
