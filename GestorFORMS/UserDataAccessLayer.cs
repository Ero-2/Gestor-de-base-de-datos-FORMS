using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorFORMS
{
    internal class UserDataAccessLayer : ICrud<User>
    {

        private string HashPassword(string plainTextPassword)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(plainTextPassword);
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        private string _connectionString;

        public UserDataAccessLayer(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<User> GetAll()
        {
            List<User> users = new List<User>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT id_usuario, usuario, contraseña, rol, fecha_creacion, id_powerlifter FROM usuario";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            User user = new User
                            {
                                // Verificar si los valores en el lector son nulos antes de asignarlos
                                id_usuario = reader["id_usuario"] != DBNull.Value ? (int)reader["id_usuario"] : 0,
                                usuario = reader["usuario"] != DBNull.Value ? reader["usuario"].ToString() : string.Empty,
                                contraseña = reader["contraseña"] != DBNull.Value ? reader["contraseña"].ToString() : string.Empty,
                                rol = reader["rol"] != DBNull.Value ? reader["rol"].ToString() : string.Empty,
                                fecha_creacion = reader["fecha_creacion"] != DBNull.Value ? (DateTime)reader["fecha_creacion"] : default(DateTime),
                                id_powerlifter = reader["id_powerlifter"] != DBNull.Value ? (int)reader["id_powerlifter"] : 0
                            };
                            users.Add(user);
                        }
                    }
                }
            }

            return users;
        }

        public void Create(User user)
        {
            user.contraseña = HashPassword(user.contraseña);

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "INSERT INTO usuario (usuario, contraseña, rol, fecha_creacion, id_powerlifter) VALUES (@usuario, @contraseña, @rol, @fecha_creacion, @id_powerlifter)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@usuario", user.usuario);
                    command.Parameters.AddWithValue("@contraseña", user.contraseña);
                    command.Parameters.AddWithValue("@rol", user.rol);
                    command.Parameters.AddWithValue("@fecha_creacion", user.fecha_creacion);
                    command.Parameters.AddWithValue("@id_powerlifter", user.id_powerlifter);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Update(User user)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "UPDATE usuario SET usuario = @usuario, contraseña = @contraseña, rol = @rol, fecha_creacion = @fecha_creacion, id_powerlifter = @id_powerlifter WHERE id_usuario = @id_usuario";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id_usuario", user.id_usuario);
                    command.Parameters.AddWithValue("@usuario", user.usuario);
                    command.Parameters.AddWithValue("@contraseña", user.contraseña);
                    command.Parameters.AddWithValue("@rol", user.rol);
                    command.Parameters.AddWithValue("@fecha_creacion", user.fecha_creacion);
                    command.Parameters.AddWithValue("@id_powerlifter", user.id_powerlifter);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id_usuario)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "DELETE FROM usuario WHERE id_usuario = @id_usuario";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id_usuario", id_usuario);
                    command.ExecuteNonQuery();
                }
            }
        }


    }
}
