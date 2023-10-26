using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data; // Add this line
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;

public partial class Product : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataAccess dataAccess = new DataAccess();
            DataTable categories = dataAccess.GetCategories();

            // Populate the DropDownList
            if (categories.Rows.Count > 0)
            {
                ddlProductCategory.DataSource = categories;
                ddlProductCategory.DataTextField = "product_category"; // Replace with the actual column name
                ddlProductCategory.DataValueField = "id"; // Replace with the actual column name
                ddlProductCategory.DataBind();
            }

            // Fetch data from the database using the DataAccess class
            DataTable productsData = dataAccess.GetProducts("product_name");

            // Bind the data to the GridView control
            gridViewProducts.DataSource = productsData;
            gridViewProducts.DataBind();
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        DataAccess dataAccess = new DataAccess();

        string product_name = txtProductName.Text;
        string product_category = ddlProductCategory.SelectedItem.Text;
        string product_rate = txtProductRate.Text;
        string product_open_stock = txtProductOpenStock.Text;

        int categories = dataAccess.InsertProduct(product_name, product_category, product_rate, product_open_stock);
        if (categories > 0)
        {
            cancelForm();

            lblMsg.ForeColor = System.Drawing.Color.ForestGreen;
            lblMsg.Font.Size = FontUnit.Point(16);
            lblMsg.Text = "Product saved successfully.";

            DataTable productsData = dataAccess.GetProducts(ddlSearchProductCategory.SelectedItem.Value);

            // Reset GridView control
            gridViewProducts.DataSource = null;
            gridViewProducts.DataBind();

            // Bind the data to the GridView control
            gridViewProducts.DataSource = productsData;
            gridViewProducts.DataBind();
        }

        else
        {
            lblMsg.ForeColor = System.Drawing.Color.DarkRed;
            lblMsg.Font.Size = FontUnit.Point(16);
            lblMsg.Text = "Sorry! Could not save Product.";
        }
    }

    protected void ddlSearchProductCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataAccess dataAccess = new DataAccess();
        DataTable productsData = dataAccess.GetProducts(ddlSearchProductCategory.SelectedItem.Value);

        // Reset GridView control
        gridViewProducts.DataSource = null;
        gridViewProducts.DataBind();

        // Bind the data to the GridView control
        gridViewProducts.DataSource = productsData;
        gridViewProducts.DataBind();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.RawUrl, false);
    }

    private void cancelForm()
    {
        txtProductName.Text = "";
        txtProductRate.Text = "";
        txtProductOpenStock.Text = "";
        ddlProductCategory.SelectedIndex = 0;

    }
}