using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorFORMS
{
    internal class ExerciseDataAcessLayer
    {
        private string _connectionString;

        public ExerciseDataAcessLayer(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Obtener todos los ejercicios
        public List<Exercise> GetAll()
        {
            List<Exercise> exercises = new List<Exercise>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT ID_ejercicio, id_entrenamiento, tipoMovimiento, setss, reps, cargaUtilizada, descansoEntreSeries, tempo, RPE, RIR, notas FROM ejercicio";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            exercises.Add(new Exercise
                            {
                                ID_ejercicio = (int)reader["ID_ejercicio"],
                                id_entrenamiento = (int)reader["id_entrenamiento"],
                                tipoMovimiento = reader["tipoMovimiento"].ToString(),
                                setss = (int)reader["setss"],
                                reps = (int)reader["reps"],
                                cargaUtilizada = (decimal)reader["cargaUtilizada"],
                                descansoEntreSeries = (int)reader["descansoEntreSeries"],
                                tempo = reader["tempo"].ToString(),
                                RPE = (int)reader["RPE"],
                                RIR = (int)reader["RIR"],
                                notas = reader["notas"].ToString()
                            });
                        }
                    }
                }
            }

            return exercises;
        }

        // Crear un nuevo ejercicio
        public void Create(Exercise entity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "INSERT INTO ejercicio (id_entrenamiento, tipoMovimiento, setss, reps, cargaUtilizada, descansoEntreSeries, tempo, RPE, RIR, notas) " +
                               "VALUES (@id_entrenamiento, @tipoMovimiento, @setss, @reps, @cargaUtilizada, @descansoEntreSeries, @tempo, @RPE, @RIR, @notas)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id_entrenamiento", entity.id_entrenamiento);
                    command.Parameters.AddWithValue("@tipoMovimiento", entity.tipoMovimiento);
                    command.Parameters.AddWithValue("@setss", entity.setss);
                    command.Parameters.AddWithValue("@reps", entity.reps);
                    command.Parameters.AddWithValue("@cargaUtilizada", entity.cargaUtilizada);
                    command.Parameters.AddWithValue("@descansoEntreSeries", entity.descansoEntreSeries);
                    command.Parameters.AddWithValue("@tempo", entity.tempo);
                    command.Parameters.AddWithValue("@RPE", entity.RPE);
                    command.Parameters.AddWithValue("@RIR", entity.RIR);
                    command.Parameters.AddWithValue("@notas", entity.notas);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Actualizar un ejercicio existente
        public void Update(Exercise entity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "UPDATE ejercicio SET id_entrenamiento = @id_entrenamiento, tipoMovimiento = @tipoMovimiento, setss = @setss, reps = @reps, " +
                               "cargaUtilizada = @cargaUtilizada, descansoEntreSeries = @descansoEntreSeries, tempo = @tempo, RPE = @RPE, RIR = @RIR, notas = @notas " +
                               "WHERE ID_ejercicio = @ID_ejercicio";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID_ejercicio", entity.ID_ejercicio);
                    command.Parameters.AddWithValue("@id_entrenamiento", entity.id_entrenamiento);
                    command.Parameters.AddWithValue("@tipoMovimiento", entity.tipoMovimiento);
                    command.Parameters.AddWithValue("@setss", entity.setss);
                    command.Parameters.AddWithValue("@reps", entity.reps);
                    command.Parameters.AddWithValue("@cargaUtilizada", entity.cargaUtilizada);
                    command.Parameters.AddWithValue("@descansoEntreSeries", entity.descansoEntreSeries);
                    command.Parameters.AddWithValue("@tempo", entity.tempo);
                    command.Parameters.AddWithValue("@RPE", entity.RPE);
                    command.Parameters.AddWithValue("@RIR", entity.RIR);
                    command.Parameters.AddWithValue("@notas", entity.notas);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Eliminar un ejercicio
        public void Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "DELETE FROM ejercicio WHERE ID_ejercicio = @ID_ejercicio";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID_ejercicio", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }

}
