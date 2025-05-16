using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CapaEntidades;

public class DataCustomer
{
    public readonly string connectionString = "Data Source=PINKISITA\\SQLEXPRESS; Initial Catalog=Semana07; User ID=userPrueba; Password=123456; TrustServerCertificate=True;";

    public List<Customer> GetCustomers()
    {
        var list = new List<Customer>();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand("GetCustomersByName", connection);
            command.CommandType = CommandType.StoredProcedure;

            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new Customer
                {
                    CustomerId = Convert.ToInt32(reader["customer_id"]),
                    Name = reader["name"].ToString(),
                    Address = reader["address"].ToString(),
                    Phone = reader["phone"].ToString(),
                    Active = Convert.ToBoolean(reader["active"])
                });
            }

            reader.Close();
        }

        return list;
    }

    public List<Customer> GetCustomersByName(string name)
    {
        var list = new List<Customer>();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand("GetCustomersByName", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@name", name);

            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new Customer
                {
                    CustomerId = Convert.ToInt32(reader["customer_id"]),
                    Name = reader["name"].ToString(),
                    Address = reader["address"].ToString(),
                    Phone = reader["phone"].ToString(),
                    Active = Convert.ToBoolean(reader["active"])
                });
            }

            reader.Close();
        }

        return list;
    }

    public void AddCustomer(Customer customer)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand("InsertCustomer", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@name", customer.Name);
            command.Parameters.AddWithValue("@address", customer.Address);
            command.Parameters.AddWithValue("@phone", customer.Phone);
            command.Parameters.AddWithValue("@active", customer.Active);

            connection.Open();
            command.ExecuteNonQuery();
        }
    }

    public void UpdateCustomer(Customer customer)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand("UpdateUser", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Customer_id", customer.CustomerId);
            command.Parameters.AddWithValue("@Name", (object)customer.Name ?? DBNull.Value);
            command.Parameters.AddWithValue("@Address", (object)customer.Address ?? DBNull.Value);
            command.Parameters.AddWithValue("@Phone", (object)customer.Phone ?? DBNull.Value);

            connection.Open();
            command.ExecuteNonQuery();
        }
    }

    public void DeleteCustomer(int customerId)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand("DeleteUser", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Customer_id", customerId);

            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}
