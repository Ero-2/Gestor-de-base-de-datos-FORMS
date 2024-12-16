using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorFORMS
{
    internal class ProductDataAcessLayer
    {
        private string _connectionString;

       

        public List<Product> GetAllProducts()
        {
            List<Product> products = new List<Product>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT Id, Name, Price FROM Products";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Product product = new Product
                            {
                                Id = (int)reader["Id"],
                                Name = reader["Name"].ToString(),
                                Price = (decimal)reader["Price"]
                            };
                            products.Add(product);
                        }
                    }
                }
            }

            return products;
        }

        public void CreateProduct(Product product)
        {
            // Implement the "Create" operation
        }

        public void UpdateProduct(Product product)
        {
            // Implement the "Update" operation
        }

        public void DeleteProduct(int productId)
        {
            // Implement the "Delete" operation
        }
    }
}
