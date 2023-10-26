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

public partial class InvoiceAdd : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            dalClsDataAccess dataAccess = new dalClsDataAccess();

            if (Request.QueryString["activeView"] != null)
            {
                int activeViewIndex;
                if (int.TryParse(Request.QueryString["activeView"], out activeViewIndex))
                {
                    MultiView1.ActiveViewIndex = activeViewIndex;
                }
            }
            else
            {
                MultiView1.ActiveViewIndex = 0;
            }

            // Create a data source (e.g., a DataTable) if it doesn't exist
            if (ViewState["ProductData"] == null)
            {
                DataTable dt = GetProductDataTable();
                ViewState["ProductData"] = dt;
            }

            // Fetch data from the database using the dalClsDataAccess class
            DataTable invoicesData = dataAccess.GetInvoiceList();

            // Bind the data to the GridView control
            gridViewInvoices.DataSource = invoicesData;
            gridViewInvoices.DataBind();

        }

        lblMsg.Text = null;
        lblMsgList.Text = null;

    }

    protected void btnGoToViewList_click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = 1;

        bllClsInvoice clsInvoice = new bllClsInvoice();

        DataTable invoicesData = clsInvoice.listInvoice();

        // Bind the data to the GridView control
        gridViewInvoices.DataSource = invoicesData;
        gridViewInvoices.DataBind();
    }

    protected void btnGoToNew_click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = 0;
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
        BindGridViewProducts();
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

        // Remove the row from the DataTable
        dt.Rows.RemoveAt(rowIndex);

        // Adjust the serial numbers in the "Sl" column
        UpdateSerialNumbers(gridViewProducts);

        // Rebind the GridView with the updated data source
        gridViewProducts.DataSource = dt;
        gridViewProducts.DataBind();

        // Calculate the Grand Total after deleting a row
        if (dt.Rows.Count == 0)
        {
            txtGrandTotal.Text = "0.00";
        }
    }

    protected void gridViewInvoices_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        bllClsInvoice clsInvoice = new bllClsInvoice();


        // Get the ID from the CommandArgument property and convert it to int
        int invId = Convert.ToInt32(gridViewInvoices.DataKeys[e.RowIndex].Values["id"]);

        DataTable result = clsInvoice.deleteInvoiceWithDetails(invId);
        if (Convert.ToInt32(result.Rows[0]["exec_flag"]) == 1)
        {
            lblMsgList.ForeColor = System.Drawing.Color.ForestGreen;
            lblMsgList.Font.Size = FontUnit.Point(16);
            lblMsgList.Text = result.Rows[0]["exec_msg"].ToString();
        }
        else
        {
            lblMsgList.ForeColor = System.Drawing.Color.DarkRed;
            lblMsgList.Font.Size = FontUnit.Point(16);
            lblMsgList.Text = result.Rows[0]["exec_msg"].ToString();
        }

        BindGridViewInvoices();
    }

    protected void UpdateSerialNumbers(GridView gridView)
    {
        int serialNumber = 1;

        foreach (GridViewRow row in gridView.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                // Find the first cell in the row (assuming it's a DataCell)
                TableCell cell = row.Cells[0]; // Change to the appropriate column index if the first column differs

                // Set the text of the cell to the serial number
                cell.Text = serialNumber.ToString();

                serialNumber++;
            }
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

    private void BindGridViewProducts()
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

    private void BindGridViewInvoices()
    {
        dalClsDataAccess dataAccess = new dalClsDataAccess();

        // Fetch data from the database using the dalClsDataAccess class
        DataTable invoicesData = dataAccess.GetInvoiceList();

        // Bind the data to the gridViewInvoices
        gridViewInvoices.DataSource = invoicesData;
        gridViewInvoices.DataBind();
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

        DataTable result = clsInvoice.saveInvoiceWithDetails(inv_no, inv_date, inv_party, inv_discount, inv_tax, inv_amount, dt);
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