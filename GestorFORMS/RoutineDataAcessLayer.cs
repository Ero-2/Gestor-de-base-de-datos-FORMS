using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorFORMS
{
    internal class RoutineDataAcessLayer : ICrud<Routine>
    {
        private readonly string _connectionString;

        public RoutineDataAcessLayer(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Routine> GetAll()
        {
            List<Routine> routines = new List<Routine>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM rutinas";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Routine routine = new Routine
                            {
                                id_rutina = (int)reader["id_rutina"],
                                descripcion = (string)reader["descripcion"],
                                fecha = (int)reader["fecha"],
                                tipoDeEntreno = (string)reader["tipoDeEntreno"],
                                estado = (string)reader["estado"],
                                intensidad = (string)reader["intensidad"],
                                notas = reader["notas"] as string
                            };
                            routines.Add(routine);
                        }
                    }
                }
            }

            return routines;
        }

        public void Create(Routine routine)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "INSERT INTO rutinas (id_usuario, nombre, descripcion, fecha, tipoDeEntreno, estado, intensidad, notas) " +
                               "VALUES (@id_usuario, @nombre, @descripcion, @fecha, @tipoDeEntreno, @estado, @intensidad, @notas)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    
                    command.Parameters.AddWithValue("@descripcion", routine.descripcion);
                    command.Parameters.AddWithValue("@fecha", routine.fecha);
                    command.Parameters.AddWithValue("@tipoDeEntreno", routine.tipoDeEntreno);
                    command.Parameters.AddWithValue("@estado", routine.estado);
                    command.Parameters.AddWithValue("@intensidad", routine.intensidad);
                    command.Parameters.AddWithValue("@notas", routine.notas);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Update(Routine routine)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "UPDATE rutinas SET id_usuario = @id_usuario, nombre = @nombre, descripcion = @descripcion, " +
                               "fecha = @fecha, tipoDeEntreno = @tipoDeEntreno, estado = @estado, intensidad = @intensidad, notas = @notas " +
                               "WHERE id_rutina = @id_rutina";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id_rutina", routine.id_rutina);
                    command.Parameters.AddWithValue("@descripcion", routine.descripcion);
                    command.Parameters.AddWithValue("@fecha", routine.fecha);
                    command.Parameters.AddWithValue("@tipoDeEntreno", routine.tipoDeEntreno);
                    command.Parameters.AddWithValue("@estado", routine.estado);
                    command.Parameters.AddWithValue("@intensidad", routine.intensidad);
                    command.Parameters.AddWithValue("@notas", routine.notas);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "DELETE FROM rutinas WHERE id_rutina = @id_rutina";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id_rutina", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
