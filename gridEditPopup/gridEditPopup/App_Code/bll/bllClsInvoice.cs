using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using App.Data;

namespace App.Business
{
    public class bllClsInvoice
    {
        public bllClsInvoice()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public DataTable listInvoice()
        {
            dalClsDataAccess clsDataAccess = new dalClsDataAccess();
            return clsDataAccess.GetInvoiceList();
        }

        private string DataTableToXml(DataTable dtInvoiceDetails)
        {
            StringBuilder xmlInvoiceDetails = new StringBuilder();

            xmlInvoiceDetails.Append("<invoice_detail>");

            for (int i = 0; i < dtInvoiceDetails.Rows.Count; i++)
            {
                xmlInvoiceDetails.Append("<item>");

                xmlInvoiceDetails.Append("<product_name>" + dtInvoiceDetails.Rows[i]["product_name"].ToString() + "</product_name>");
                xmlInvoiceDetails.Append("<product_quantity>" + dtInvoiceDetails.Rows[i]["product_quantity"].ToString() + "</product_quantity>");
                xmlInvoiceDetails.Append("<product_rate>" + dtInvoiceDetails.Rows[i]["product_rate"].ToString() + "</product_rate>");
                xmlInvoiceDetails.Append("<product_discount>" + dtInvoiceDetails.Rows[i]["product_discount"].ToString() + "</product_discount>");
                xmlInvoiceDetails.Append("<product_tax>" + dtInvoiceDetails.Rows[i]["product_tax"].ToString() + "</product_tax>");
                xmlInvoiceDetails.Append("<product_amount>" + dtInvoiceDetails.Rows[i]["product_amount"].ToString() + "</product_amount>");

                xmlInvoiceDetails.Append("</item>");
            }

            xmlInvoiceDetails.Append("</invoice_detail>");

            return xmlInvoiceDetails.ToString();
        }

        public DataTable saveInvoiceWithDetails(string InvNo, string InvDate, string InvParty, decimal InvDiscount, decimal InvTax, decimal InvAmount, DataTable dtInvoiceDetails, int InvId = 0)
        {
            dalClsDataAccess clsDataAccess = new dalClsDataAccess();

            // Create an XML string from the DataTable
            string xmlInvoiceDetails = DataTableToXml(dtInvoiceDetails);

            return clsDataAccess.StoreInvoiceWithDetails(InvId, InvNo, InvDate, InvParty, InvDiscount, InvTax, InvAmount, xmlInvoiceDetails);
        }

        public DataTable deleteInvoiceWithDetails(int InvId)
        {
            dalClsDataAccess clsDataAccess = new dalClsDataAccess();
            return clsDataAccess.DestroyInvoiceWithDetails(InvId);
        }
    }
}