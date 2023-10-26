using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Configuration;
using System.Data;
using System.Data.SqlClient;

using App.Data;
using App.Business;

public partial class InvoiceEdit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataTable dt = GetProductDataTable();
            ViewState["ProductData"] = dt;

            if (Request.QueryString["id"] != null)
            {
                int id;
                if (int.TryParse(Request.QueryString["id"], out id))
                {
                    // Call the function to retrieve and display the invoice details
                    DisplayInvoiceDetails(id);
                    BindInvoiceDetails(id);
                }
            }
        }
    }

    protected void DisplayInvoiceDetails(int invoiceId)
    {
        dalClsDataAccess dataAccess = new dalClsDataAccess();
        DataTable invoiceData = dataAccess.GetInvoiceDetails(invoiceId);

        if (invoiceData != null && invoiceData.Rows.Count > 0)
        {
            DataRow invoiceRow = invoiceData.Rows[0]; // Assuming the first row contains the main invoice data

            txtInvoiceNo.Text = invoiceRow.Field<string>("inv_no");
            txtInvoiceDate.Text = invoiceRow.Field<DateTime>("inv_date").ToString("dd/MM/yyyy");
            txtInvoiceParty.Text = invoiceRow.Field<string>("inv_party");

            // Continue to populate other textboxes as needed
        }
    }

    private void BindInvoiceDetails(int invoiceId)
    {
        dalClsDataAccess dataAccess = new dalClsDataAccess();
        DataTable invoiceData = dataAccess.GetInvoiceDetails(invoiceId);
        ViewState["ProductData"] = invoiceData;

        if (invoiceData.Rows.Count > 0)
        {
            gridViewProducts.DataSource = invoiceData;
            gridViewProducts.DataBind();
        }
    }

    protected void btnAddRow_Click(object sender, EventArgs e)
    {
        // Retrieve the DataTable from ViewState
        DataTable dt = ViewState["ProductData"] as DataTable;

        // Add a new row
        DataRow newRow = dt.NewRow();
        newRow["product_name"] = txtProductName.Text;
        newRow["product_rate"] = txtProductRate.Text;
        newRow["product_quantity"] = txtProductQuantity.Text;
        newRow["product_discount"] = txtProductDiscount.Text;
        newRow["product_tax"] = txtProductTax.Text;
        newRow["product_amount"] = txtAmount.Text;

        dt.Rows.Add(newRow);

        // Rebind the data to the GridView
        gridViewProducts.DataSource = dt;
        gridViewProducts.DataBind();

        // Clear the textboxes for new input
        txtProductName.Text = string.Empty;
        txtProductRate.Text = string.Empty;
        txtProductQuantity.Text = string.Empty;
        txtProductDiscount.Text = string.Empty;
        txtProductTax.Text = string.Empty;
        txtAmount.Text = string.Empty;

        // Calculate the Grand Total after adding a row
        ClientScript.RegisterStartupScript(this.GetType(), "calculateGrandTotal", "calculateGrandTotal();", true);
    }

    protected DataTable GetProductDataTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("product_name");
        dt.Columns.Add("product_rate");
        dt.Columns.Add("product_quantity");
        dt.Columns.Add("product_discount");
        dt.Columns.Add("product_tax");
        dt.Columns.Add("product_amount");

        return dt;
    }

    protected void gridViewProducts_RowEditing(object sender, GridViewEditEventArgs e)
    {
        // Set the GridView's EditIndex to enable editing mode for the selected row
        gridViewProducts.EditIndex = e.NewEditIndex;
        BindGridView();
    }

    protected void gridViewProducts_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        // Access the DataTable from ViewState
        DataTable dt = ViewState["ProductData"] as DataTable;

        if (dt != null)
        {
            // Get the updated values from the GridView
            string product_name = gridViewProducts.DataKeys[e.RowIndex]["product_name"].ToString();
            string newRate = (gridViewProducts.Rows[e.RowIndex].FindControl("txtEditRate") as TextBox).Text;
            string newQuantity = (gridViewProducts.Rows[e.RowIndex].FindControl("txtEditQuantity") as TextBox).Text;
            string newDiscount = (gridViewProducts.Rows[e.RowIndex].FindControl("txtEditDiscount") as TextBox).Text;
            string newTax = (gridViewProducts.Rows[e.RowIndex].FindControl("txtEditTax") as TextBox).Text;
            string newAmount = (gridViewProducts.Rows[e.RowIndex].FindControl("txtEditAmount") as TextBox).Text;

            // Locate the row you are updating based on "product_name"
            DataRow rowToUpdate = dt.AsEnumerable().FirstOrDefault(row => row["product_name"].ToString() == product_name);

            if (rowToUpdate != null)
            {
                // Update the DataRow with the edited values
                rowToUpdate["product_rate"] = newRate;
                rowToUpdate["product_quantity"] = newQuantity;
                rowToUpdate["product_discount"] = newDiscount;
                rowToUpdate["product_tax"] = newTax;
                rowToUpdate["product_amount"] = newAmount;
                // Update other columns as needed
            }

            // Save the changes back to ViewState
            ViewState["ProductData"] = dt;

            // Exit edit mode
            gridViewProducts.EditIndex = -1;

            // Rebind the GridView to reflect the changes
            BindGridView(); // Implement a method to bind data to the GridView
        }
    }

    protected void gridViewProducts_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        // Exit edit mode
        gridViewProducts.EditIndex = -1;

        // Rebind the GridView to display the original data
        BindGridView(); // Implement a method to bind data to the GridView
    }

    protected void gridViewProducts_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // Find the Delete button and add a JavaScript confirmation prompt
            ImageButton btnDelete = (ImageButton)e.Row.FindControl("btnDelete");
        }
    }

    protected void gridViewProducts_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        // Determine the index of the row being deleted
        int rowIndex = e.RowIndex;

        // Retrieve the DataTable from ViewState
        DataTable dt = ViewState["ProductData"] as DataTable;

        if (dt != null)
        {
            // Remove the row from the DataTable
            dt.Rows.RemoveAt(rowIndex);

            // Rebind the GridView with the updated data source
            gridViewProducts.DataSource = dt;
            gridViewProducts.DataBind();

            // Calculate the Grand Total after deleting a row
            if (dt.Rows.Count == 0)
            {
                txtGrandTotal.Text = "0.00";
            }
        }
    }

    private void UpdateSerialNumbers(DataTable dt)
    {
        int sl = 1;

        foreach (DataRow row in dt.Rows)
        {
            row["Sl"] = sl;
            sl++;
        }
    }

    protected void UpdateAmountTextBox(object sender, EventArgs e)
    {
        // Find the parent GridViewRow of the TextBox
        GridViewRow row = (GridViewRow)((Control)sender).NamingContainer;

        // Find the TextBox controls in the row
        TextBox txtEditQuantity = (TextBox)row.FindControl("txtEditQuantity");
        TextBox txtEditRate = (TextBox)row.FindControl("txtEditRate");
        TextBox txtEditDiscount = (TextBox)row.FindControl("txtEditDiscount");
        TextBox txtEditTax = (TextBox)row.FindControl("txtEditTax");
        TextBox txtEditAmount = (TextBox)row.FindControl("txtEditAmount");

        // Get the values from the TextBoxes
        int quantity = string.IsNullOrEmpty(txtEditQuantity.Text) ? 0 : int.Parse(txtEditQuantity.Text);
        decimal rate = string.IsNullOrEmpty(txtEditRate.Text) ? 0 : decimal.Parse(txtEditRate.Text);
        decimal discount = string.IsNullOrEmpty(txtEditDiscount.Text) ? 0 : decimal.Parse(txtEditDiscount.Text);
        decimal tax = string.IsNullOrEmpty(txtEditTax.Text) ? 0 : decimal.Parse(txtEditTax.Text);

        // Calculate the amount
        decimal amount = CalculateAmount(rate, quantity, discount, tax);

        // Update the txtEditAmount TextBox
        txtEditAmount.Text = amount.ToString("0.00"); // Format amount to 2 decimal places
    }

    private void BindGridView()
    {
        // Retrieve the DataTable from ViewState
        DataTable dt = ViewState["ProductData"] as DataTable;

        // Check if there is data to bind
        if (dt != null)
        {
            gridViewProducts.DataSource = dt;
            gridViewProducts.DataBind();
        }
        else
        {
            gridViewProducts.DataSource = null;
            gridViewProducts.DataBind();
        }
    }

    private decimal CalculateAmount(object rate, object quantity, object discount, object tax)
    {
        decimal rateValue = Convert.ToDecimal(rate);
        decimal quantityValue = Convert.ToDecimal(quantity);
        decimal discountValue = Convert.ToDecimal(discount);
        decimal taxValue = Convert.ToDecimal(tax);

        // Calculate the amount
        decimal amount = ((rateValue * quantityValue) - discountValue) + taxValue;

        return amount;
    }


    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        bllClsInvoice clsInvoice = new bllClsInvoice();

        string inv_no = txtInvoiceNo.Text;
        string inv_date = txtInvoiceDate.Text;
        string inv_party = txtInvoiceParty.Text;
        decimal inv_discount = 0;
        decimal inv_tax = 0;
        decimal inv_amount = decimal.Parse(txtGrandTotal.Text);

        DataTable dt = ViewState["ProductData"] as DataTable;

        if (Request.QueryString["id"] != null)
        {
            int id;
            if (int.TryParse(Request.QueryString["id"], out id))
            {
                DataTable result = clsInvoice.saveInvoiceWithDetails(inv_no, inv_date, inv_party, inv_discount, inv_tax, inv_amount, dt, id);
                if (Convert.ToInt32(result.Rows[0]["exec_flag"]) == 1)
                {
                    lblMsg.ForeColor = System.Drawing.Color.ForestGreen;
                    lblMsg.Font.Size = FontUnit.Point(16);
                    lblMsg.Text = result.Rows[0]["exec_msg"].ToString();
                }
                else
                {
                    lblMsg.ForeColor = System.Drawing.Color.DarkRed;
                    lblMsg.Font.Size = FontUnit.Point(16);
                    lblMsg.Text = result.Rows[0]["exec_msg"].ToString();
                }
            }
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        // Redirect to invoiceAdd.aspx
        Response.Redirect("invoiceAdd.aspx?activeView=1");
    }
}