using System;using System.Collections.Generic;using System.Linq;using System.Web;using System.Web.UI;using System.Web.UI.WebControls;using System.Configuration;using System.Data;using System.Data.SqlClient;public partial class invoiceAdd : System.Web.UI.Page{    protected void Page_Load(object sender, EventArgs e)    {        if (!IsPostBack)        {            DataAccess dataAccess = new DataAccess();

            if (Request.QueryString["activeView"] != null)
            {
                int activeViewIndex;
                if (int.TryParse(Request.QueryString["activeView"], out activeViewIndex))
                {
                    MultiView1.ActiveViewIndex = activeViewIndex;
                }
            } else
            {
                MultiView1.ActiveViewIndex = 0;
            }

            // Create a data source (e.g., a DataTable) if it doesn't exist
            if (ViewState["ProductData"] == null)            {                DataTable dt = GetProductDataTable();                ViewState["ProductData"] = dt;            }

            // Fetch data from the database using the DataAccess class
            DataTable invoicesData = dataAccess.GetInvoiceList();

            // Bind the data to the GridView control
            gridViewInvoices.DataSource = invoicesData;
            gridViewInvoices.DataBind();
        }    }

    protected void btnGoToViewList_click(object sender, EventArgs e)    {        MultiView1.ActiveViewIndex = 1;

        DataAccess dataAccess = new DataAccess();
        
        // Fetch data from the database using the DataAccess class
        DataTable invoicesData = dataAccess.GetInvoiceList();

        // Bind the data to the GridView control
        gridViewInvoices.DataSource = invoicesData;
        gridViewInvoices.DataBind();
    }    protected void btnGoToNew_click(object sender, EventArgs e)    {        MultiView1.ActiveViewIndex = 0;    }    protected void btnAddRow_Click(object sender, EventArgs e)    {
        // Retrieve the DataTable from ViewState
        DataTable dt = ViewState["ProductData"] as DataTable;

        // Determine the next serial number based on the number of rows in the GridView
        int nextSerialNumber = gridViewProducts.Rows.Count + 1;

        // Add a new row
        DataRow newRow = dt.NewRow();
        newRow["sl"] = nextSerialNumber;
        newRow["product_name"] = txtProductName.Text;        newRow["rate"] = txtProductRate.Text;        newRow["quantity"] = txtProductQuantity.Text;        newRow["discount"] = txtProductDiscount.Text;        newRow["tax"] = txtProductTax.Text;        newRow["amount"] = txtAmount.Text;        dt.Rows.Add(newRow);

        // Rebind the data to the GridView
        gridViewProducts.DataSource = dt;        gridViewProducts.DataBind();

        // Clear the textboxes for new input
        txtProductName.Text = string.Empty;        txtProductRate.Text = string.Empty;        txtProductQuantity.Text = string.Empty;        txtProductDiscount.Text = string.Empty;        txtProductTax.Text = string.Empty;        txtAmount.Text = string.Empty;

        // Calculate the Grand Total after adding a row
        ClientScript.RegisterStartupScript(this.GetType(), "calculateGrandTotal", "calculateGrandTotal();", true);
    }    protected DataTable GetProductDataTable()    {        DataTable dt = new DataTable();
        dt.Columns.Add("sl");
        dt.Columns.Add("product_name");        dt.Columns.Add("rate");        dt.Columns.Add("quantity");        dt.Columns.Add("discount");        dt.Columns.Add("tax");        dt.Columns.Add("amount");        return dt;    }

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
                rowToUpdate["rate"] = newRate;
                rowToUpdate["quantity"] = newQuantity;
                rowToUpdate["discount"] = newDiscount;
                rowToUpdate["tax"] = newTax;
                rowToUpdate["amount"] = newAmount;
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

        // Remove the row from the DataTable
        dt.Rows.RemoveAt(rowIndex);
        
        // Adjust the serial numbers in the "Sl" column
        UpdateSerialNumbers(dt);
        
        // Rebind the GridView with the updated data source
        gridViewProducts.DataSource = dt;
        gridViewProducts.DataBind();

        // Calculate the Grand Total after deleting a row
        if(dt.Rows.Count == 0)
        {
            txtGrandTotal.Text = "0.00";
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
        TextBox txtEditRate = (TextBox)row.FindControl("txtEditRate");
        TextBox txtEditQuantity = (TextBox)row.FindControl("txtEditQuantity");
        TextBox txtEditDiscount = (TextBox)row.FindControl("txtEditDiscount");
        TextBox txtEditTax = (TextBox)row.FindControl("txtEditTax");
        TextBox txtEditAmount = (TextBox)row.FindControl("txtEditAmount");

        // Get the values from the TextBoxes
        decimal rate = decimal.Parse(txtEditRate.Text);
        int quantity = int.Parse(txtEditQuantity.Text);
        decimal discount = decimal.Parse(txtEditDiscount.Text);
        decimal tax = decimal.Parse(txtEditTax.Text);

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
        DataAccess dataAccess = new DataAccess();

        string inv_no = txtInvoiceNo.Text;
        string inv_date = txtInvoiceDate.Text;
        string inv_party = txtInvoiceParty.Text;
        decimal inv_amount = decimal.Parse(txtGrandTotal.Text);

        DataTable dt = ViewState["ProductData"] as DataTable;

        int result = dataAccess.insertInvoice(inv_no, inv_date, inv_party, inv_amount, dt);
        if (result > 0)
        {
            lblMsg.ForeColor = System.Drawing.Color.ForestGreen;
            lblMsg.Font.Size = FontUnit.Point(16);
            lblMsg.Text = "Invoice created successfully.";
        } else
        {
            lblMsg.ForeColor = System.Drawing.Color.DarkRed;
            lblMsg.Font.Size = FontUnit.Point(16);
            lblMsg.Text = "Sorry! Could not create Invoice.";
        }
    }
}