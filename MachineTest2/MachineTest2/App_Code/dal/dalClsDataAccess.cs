using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Xml;
using System.Data.SqlTypes;

namespace App.Data
{
    public class dalClsDataAccess
    {
        private string connectionString;

        public dalClsDataAccess()
        {
            //
            // TODO: Add constructor logic here
            //

            // Retrieve the connection string from web.config
            connectionString = ConfigurationManager.ConnectionStrings["TestInvoice"].ConnectionString;
        }

        public DataTable StoreInvoiceWithDetails(int InvId, string InvNo, string InvDate, string InvParty, decimal InvDiscount, decimal InvTax, decimal InvAmount, string xmlInvoiceDetails)
        {
            DataTable resultTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_INSERT_INVOICE_AND_DETAILS", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Parameters
                    command.Parameters.Add(new SqlParameter("@InvId", SqlDbType.Int)).Value = InvId;
                    command.Parameters.Add(new SqlParameter("@InvNo", SqlDbType.VarChar, 50)).Value = InvNo;
                    command.Parameters.Add(new SqlParameter("@InvDate", SqlDbType.Date)).Value = DateTime.Parse(InvDate);
                    command.Parameters.Add(new SqlParameter("@InvParty", SqlDbType.NVarChar, 255)).Value = InvParty;
                    command.Parameters.Add(new SqlParameter("@InvDiscount", SqlDbType.Decimal)).Value = InvDiscount;
                    command.Parameters.Add(new SqlParameter("@InvTax", SqlDbType.Decimal)).Value = InvTax;
                    command.Parameters.Add(new SqlParameter("@InvAmount", SqlDbType.Decimal)).Value = InvAmount;
                    command.Parameters.Add(new SqlParameter("@InvoiceDetailsXML", SqlDbType.Xml)).Value = new SqlXml(new XmlTextReader(new StringReader(xmlInvoiceDetails)));

                    connection.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(resultTable);
                    }
                }
            }

            return resultTable;
        }

        public DataTable DestroyInvoiceWithDetails(int InvId)
        {
            DataTable resultTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_DELETE_INVOICE_AND_DETAILS", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Parameters
                    command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = InvId;

                    connection.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(resultTable);
                    }
                }
            }

            return resultTable;
        }

        public DataTable GetInvoiceList()
        {
            DataTable invoiceDataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SP_SELECT_INVOICES", connection))
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
}
