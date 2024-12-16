using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorFORMS
{
    internal class SeriesDataAccessLayer : ICrud <Series>
    {
        private readonly string _connectionString;

        public SeriesDataAccessLayer(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Series> GetAll()
        {
            List<Series> series = new List<Series>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT id_series, id_ejercicio_carga, sets, sensaciones, notas FROM series";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Series serie = new Series
                            {
                                id_series = (int)reader["id_series"],
                                id_ejercicio_carga = (int)reader["id_ejercicio_carga"],
                                sets = (int)reader["sets"],
                                sensaciones = reader["sensaciones"].ToString(),
                                notas = reader["notas"].ToString()
                            };
                            series.Add(serie);
                        }
                    }
                }
            }

            return series;
        }

        public void Create(Series serie)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "INSERT INTO series (id_ejercicio_carga, sets, sensaciones, notas) VALUES (@id_ejercicio_carga, @sets, @sensaciones, @notas)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id_ejercicio_carga", serie.id_ejercicio_carga);
                    command.Parameters.AddWithValue("@sets", serie.sets);
                    command.Parameters.AddWithValue("@sensaciones", serie.sensaciones);
                    command.Parameters.AddWithValue("@notas", serie.notas);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Update(Series serie)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "UPDATE series SET id_ejercicio_carga = @id_ejercicio_carga, sets = @sets, sensaciones = @sensaciones, notas = @notas WHERE id_series = @id_series";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id_series", serie.id_series);
                    command.Parameters.AddWithValue("@id_ejercicio_carga", serie.id_ejercicio_carga);
                    command.Parameters.AddWithValue("@sets", serie.sets);
                    command.Parameters.AddWithValue("@sensaciones", serie.sensaciones);
                    command.Parameters.AddWithValue("@notas", serie.notas);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id_series)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "DELETE FROM series WHERE id_series = @id_series";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id_series", id_series);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
