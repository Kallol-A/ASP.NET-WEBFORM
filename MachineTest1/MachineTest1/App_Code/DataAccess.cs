using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for DataAccess
/// </summary>
public class DataAccess
{
    private string connectionString;

    public DataAccess()
    {
        //
        // TODO: Add constructor logic here
        //
        // Retrieve the connection string from web.config
        connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
    }

    public DataTable GetCategories()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            using (SqlCommand command = new SqlCommand("sp_GetDistinctProductCategories", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }
    }

    public int InsertProduct(string productName, string productCategory, string productRate, string productOpenStock)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            using (SqlCommand command = new SqlCommand("sp_InsertProduct", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@product_name", productName);
                command.Parameters.AddWithValue("@product_category", productCategory);
                command.Parameters.AddWithValue("@product_rate", productRate);
                command.Parameters.AddWithValue("@product_open_stock", productOpenStock);
                int result = command.ExecuteNonQuery();
                return result;
            }
        }
    }

    public DataTable GetProducts(string orderBySearch)
    {
        DataTable productsDataTable = new DataTable();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            using (SqlCommand command = new SqlCommand("SP_Search_By_Group", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@groupby", orderBySearch);

                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(productsDataTable);
                }
            }
        }

        return productsDataTable;
    }
}