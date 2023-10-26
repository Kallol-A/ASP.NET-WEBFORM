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
    private int newId;

    public DataAccess()
    {
        //
        // TODO: Add constructor logic here
        //

        // Retrieve the connection string from web.config
        connectionString = ConfigurationManager.ConnectionStrings["TestInvoice"].ConnectionString;
    }

    public int insertInvoice(string inv_no, string inv_date, string inv_party, decimal inv_amount, DataTable dt)    {        int result = 0;        using (SqlConnection connection = new SqlConnection(connectionString))        {
            connection.Open();            string sqlquery = "INSERT INTO [dbo].[tb_invoice] (inv_no, inv_date, inv_party, inv_discount, inv_tax, inv_amount) VALUES (@inv_no, CONVERT(DATE, @inv_date, 103), @inv_party, 0, 0, @inv_amount); SELECT SCOPE_IDENTITY();";            SqlCommand command = new SqlCommand(sqlquery, connection);            command.Parameters.AddWithValue("@inv_no", inv_no);            command.Parameters.AddWithValue("@inv_date", inv_date);            command.Parameters.AddWithValue("@inv_party", inv_party);            command.Parameters.AddWithValue("@inv_discount", 0);            command.Parameters.AddWithValue("@inv_tax", 0);            command.Parameters.AddWithValue("@inv_amount", inv_amount);
            newId = Convert.ToInt32(command.ExecuteScalar());            connection.Close();

            foreach (DataRow row in dt.Rows)            {
                connection.Open();

                string sqlquery2 = "INSERT INTO [dbo].[tb_invoice_details] (invoice_id, product_name, product_rate, product_quantity, product_discount, product_tax, product_amount) VALUES (@invoice_id, @product_name, @product_rate, @product_quantity, @product_discount, @product_tax, @product_amount)";

                using (SqlCommand command2 = new SqlCommand(sqlquery2, connection))
                {
                    command2.Parameters.AddWithValue("@invoice_id", newId); // Example value, replace with the appropriate value
                    command2.Parameters.AddWithValue("@product_name", row["product_name"]);
                    command2.Parameters.AddWithValue("@product_rate", row["rate"]);
                    command2.Parameters.AddWithValue("@product_quantity", row["quantity"]);
                    command2.Parameters.AddWithValue("@product_discount", row["discount"]);
                    command2.Parameters.AddWithValue("@product_tax", row["tax"]);
                    command2.Parameters.AddWithValue("@product_amount", row["amount"]);

                    result += command2.ExecuteNonQuery();
                }
                connection.Close();
            }            return result;        }    }

    public int updateInvoice(int invoiceId, string inv_no, string inv_date, string inv_party, decimal inv_amount, DataTable dt)
    {
        int result = 0;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            // Update the main invoice information
            string updateInvoiceQuery = "UPDATE [dbo].[tb_invoice] " +
                                        "SET inv_no = @inv_no, " +
                                        "inv_date = CONVERT(DATE, @inv_date, 103), " +
                                        "inv_party = @inv_party, " +
                                        "inv_amount = @inv_amount " +
                                        "WHERE id = @invoiceId";
            using (SqlCommand updateInvoiceCommand = new SqlCommand(updateInvoiceQuery, connection))
            {
                updateInvoiceCommand.Parameters.AddWithValue("@invoiceId", invoiceId);
                updateInvoiceCommand.Parameters.AddWithValue("@inv_no", inv_no);
                updateInvoiceCommand.Parameters.AddWithValue("@inv_date", inv_date);
                updateInvoiceCommand.Parameters.AddWithValue("@inv_party", inv_party);
                updateInvoiceCommand.Parameters.AddWithValue("@inv_amount", inv_amount);
                result = updateInvoiceCommand.ExecuteNonQuery();
            }

            // Delete existing invoice details for the invoice
            string deleteInvoiceDetailsQuery = "DELETE FROM [dbo].[tb_invoice_details] WHERE invoice_id = @invoiceId";
            using (SqlCommand deleteInvoiceDetailsCommand = new SqlCommand(deleteInvoiceDetailsQuery, connection))
            {
                deleteInvoiceDetailsCommand.Parameters.AddWithValue("@invoiceId", invoiceId);
                deleteInvoiceDetailsCommand.ExecuteNonQuery();
            }

            // Insert updated invoice details
            foreach (DataRow row in dt.Rows)
            {
                string insertInvoiceDetailsQuery = "INSERT INTO [dbo].[tb_invoice_details] " +
                    "(invoice_id, product_name, product_rate, product_quantity, product_discount, product_tax, product_amount) " +
                    "VALUES (@invoice_id, @product_name, @product_rate, @product_quantity, @product_discount, @product_tax, @product_amount)";
                using (SqlCommand insertInvoiceDetailsCommand = new SqlCommand(insertInvoiceDetailsQuery, connection))
                {
                    insertInvoiceDetailsCommand.Parameters.AddWithValue("@invoice_id", invoiceId);
                    insertInvoiceDetailsCommand.Parameters.AddWithValue("@product_name", row["product_name"]);
                    insertInvoiceDetailsCommand.Parameters.AddWithValue("@product_rate", row["product_rate"]);
                    insertInvoiceDetailsCommand.Parameters.AddWithValue("@product_quantity", row["product_quantity"]);
                    insertInvoiceDetailsCommand.Parameters.AddWithValue("@product_discount", row["product_discount"]);
                    insertInvoiceDetailsCommand.Parameters.AddWithValue("@product_tax", row["product_tax"]);
                    insertInvoiceDetailsCommand.Parameters.AddWithValue("@product_amount", row["product_amount"]);
                    result += insertInvoiceDetailsCommand.ExecuteNonQuery();
                }
            }

            connection.Close();
        }

        return result;
    }

    public DataTable GetInvoiceList()
    {
        DataTable invoiceDataTable = new DataTable();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            using (SqlCommand command = new SqlCommand("sp_select_invoices", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(invoiceDataTable);
                }
            }
        }

        return invoiceDataTable;
    }

    public DataTable GetInvoiceDetails(int invoiceId)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string query = @"
            SELECT * FROM [dbo].tb_invoice i
            LEFT JOIN [dbo].tb_invoice_details d ON i.id = d.invoice_id
            WHERE i.id = @invoiceId";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@invoiceId", invoiceId);

                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    return dataTable;
                }
            }
        }
    }
}