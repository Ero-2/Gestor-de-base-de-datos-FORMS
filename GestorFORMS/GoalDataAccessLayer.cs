using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GestorFORMS
{
    internal class GoalDataAccessLayer : ICrud<Goal>
    {
       private readonly string _connectionString;

    public GoalDataAccessLayer(string connectionString)
    {
        _connectionString = connectionString;
    }

        public List<Goal> GetAll()
        {
            List<Goal> goals = new List<Goal>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Objetivo";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Goal goal = new Goal
                                {
                                    ID_Objetivos = (int)reader["ID_Objetivos"],
                                    id_powerlifter = (int)reader["id_powerlifter"],
                                    SetsEsperados = (int)reader["SetsEsperados"],
                                    RepsEsperados = (int)reader["RepsEsperados"],
                                    CargaEstimada = (decimal)reader["CargaEstimada"],
                                    notas = reader["notas"] as string
                                };
                                goals.Add(goal);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al ejecutar la consulta: {ex.Message}");
                    }
                }
            }

            return goals;
        }

        public void Create(Goal goal)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string query = "INSERT INTO Objetivo (id_powerlifter, SetsEsperados, RepsEsperados, CargaEstimada, notas) " +
                           "VALUES (@id_powerlifter, @SetsEsperados, @RepsEsperados, @CargaEstimada, @notas)";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id_powerlifter", goal.id_powerlifter);
                command.Parameters.AddWithValue("@SetsEsperados", goal.SetsEsperados);
                command.Parameters.AddWithValue("@RepsEsperados", goal.RepsEsperados);
                command.Parameters.AddWithValue("@CargaEstimada", goal.CargaEstimada);
                command.Parameters.AddWithValue("@notas", goal.notas);
                command.ExecuteNonQuery();
            }
        }
    }

    public void Update(Goal goal)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string query = "UPDATE Objetivo SET id_powerlifter = @id_powerlifter, SetsEsperados = @SetsEsperados, " +
                           "RepsEsperados = @RepsEsperados, CargaEstimada = @CargaEstimada, notas = @notas " +
                           "WHERE ID_Objetivos = @ID_Objetivos";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ID_Objetivos", goal.ID_Objetivos);
                command.Parameters.AddWithValue("@id_powerlifter", goal.id_powerlifter);
                command.Parameters.AddWithValue("@SetsEsperados", goal.SetsEsperados);
                command.Parameters.AddWithValue("@RepsEsperados", goal.RepsEsperados);
                command.Parameters.AddWithValue("@CargaEstimada", goal.CargaEstimada);
                command.Parameters.AddWithValue("@notas", goal.notas);
                command.ExecuteNonQuery();
            }
        }
    }

    public void Delete(int id)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string query = "DELETE FROM Objetivo WHERE ID_Objetivos = @ID_Objetivos";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ID_Objetivos", id);
                command.ExecuteNonQuery();
            }
        }
    }
    }
}





