using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorFORMS
{
    internal class TrainingSessionDataAccessLayer
    {
        private readonly string _connectionString;

        public TrainingSessionDataAccessLayer(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<TrainingSession> GetAll()
        {
            var sessions = new List<TrainingSession>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM sesionDeEntrenamiento", conn);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        sessions.Add(new TrainingSession
                        {
                            ID_Sesion = Convert.ToInt32(reader["ID_Sesion"]),
                            id_powerlifter = Convert.ToInt32(reader["id_powerlifter"]),
                            TipoDeSesion = reader["TipoDeSesion"]?.ToString() ?? string.Empty,
                            intensidad = reader["intensidad"]?.ToString() ?? string.Empty,
                            estadoDelAtleta = reader["estadoDelAtleta"]?.ToString() ?? string.Empty,
                            evaluacion = reader["evaluacion"]?.ToString() ?? string.Empty,
                        });
                    }
                }
            }

            return sessions;
        }

        public void Create(TrainingSession entity)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO sesionDeEntrenamiento (id_powerlifter, TipoDeSesion, intensidad, estadoDelAtleta, evaluacion) VALUES (@id_powerlifter, @TipoDeSesion, @intensidad, @estadoDelAtleta, @evaluacion)",
                    conn);

                cmd.Parameters.AddWithValue("@id_powerlifter", entity.id_powerlifter);
                cmd.Parameters.AddWithValue("@TipoDeSesion", entity.TipoDeSesion);
                cmd.Parameters.AddWithValue("@intensidad", entity.intensidad);
                cmd.Parameters.AddWithValue("@estadoDelAtleta", entity.estadoDelAtleta);
                cmd.Parameters.AddWithValue("@evaluacion", entity.evaluacion);

                cmd.ExecuteNonQuery();
            }
        }

        public void Update(TrainingSession entity)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "UPDATE sesionDeEntrenamiento SET id_powerlifter = @id_powerlifter, TipoDeSesion = @TipoDeSesion, intensidad = @intensidad, estadoDelAtleta = @estadoDelAtleta, evaluacion = @evaluacion WHERE ID_Sesion = @ID_Sesion",
                    conn);

                cmd.Parameters.AddWithValue("@ID_Sesion", entity.ID_Sesion);
                cmd.Parameters.AddWithValue("@id_powerlifter", entity.id_powerlifter);
                cmd.Parameters.AddWithValue("@TipoDeSesion", entity.TipoDeSesion);
                cmd.Parameters.AddWithValue("@intensidad", entity.intensidad);
                cmd.Parameters.AddWithValue("@estadoDelAtleta", entity.estadoDelAtleta);
                cmd.Parameters.AddWithValue("@evaluacion", entity.evaluacion);

                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM sesionDeEntrenamiento WHERE ID_Sesion = @ID_Sesion", conn);
                cmd.Parameters.AddWithValue("@ID_Sesion", id);
                cmd.ExecuteNonQuery();
            }
        }


    }
}
