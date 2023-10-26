<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Product.aspx.cs" Inherits="Product" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Machine Test</title>
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h1>Add Product</h1>
            <div class="row">
                <div class="col-md-12">
                    <asp:Label ID="lblMsg" runat="server"></asp:Label>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <asp:Label ID="lblProductName" runat="server" Text="Product Name" Font-Bold="True" Font-Size="Larger" ForeColor="Blue" CssClass="control-label"></asp:Label>
                        <asp:Label ID="lblProductNameReqd" runat="server" Text="*" Font-Bold="True" Font-Size="Larger" ForeColor="Red" CssClass="control-label"></asp:Label>
                        <asp:TextBox ID="txtProductName" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator
                            ID="rfvProductName"
                            runat="server"
                            ControlToValidate="txtProductName"
                            InitialValue=""
                            ErrorMessage="Please enter a value"
                            Display="Dynamic"
                            ForeColor="Red"
                        ></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <asp:Label ID="lblProductCategory" runat="server" Text="Category" Font-Bold="True" Font-Size="Larger" ForeColor="Blue" CssClass="control-label"></asp:Label>
                        <asp:DropDownList ID="ddlProductCategory" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <asp:Label ID="lblProductRate" runat="server" Text="Rate" Font-Bold="True" Font-Size="Larger" ForeColor="Blue" CssClass="control-label"></asp:Label>
                        <asp:Label ID="lblProductRateReqd" runat="server" Text="*" Font-Bold="True" Font-Size="Larger" ForeColor="Red" CssClass="control-label"></asp:Label>
                        <asp:TextBox ID="txtProductRate" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator
                            ID="rfvProductRate"
                            runat="server"
                            ControlToValidate="txtProductRate"
                            InitialValue=""
                            ErrorMessage="Please enter a value"
                            Display="Dynamic"
                            ForeColor="Red"
                        ></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <asp:Label ID="lblProductOpenStock" runat="server" Text="Opening Stock" Font-Bold="True" Font-Size="Larger" ForeColor="Blue" CssClass="control-label"></asp:Label>
                        <asp:TextBox ID="txtProductOpenStock" runat="server" CssClass="form-control" />
                    </div>
                </div>
            </div>
            <asp:Label ID="lblMandatoryMessage" runat="server" Text="All * marked fields are mandatory" Font-Size="Larger" ForeColor="Red" CssClass="control-label"></asp:Label>
            <div class="row d-flex justify-content-center">
                <div class="col-md-2"> <!-- Each button takes 1/4 of their container's width (3 columns out of 12) -->
                    <asp:Button CssClass="btn btn-primary btn-block" ID="btnSubmit" runat="server" Text="Add" OnClick="btnSubmit_Click" />
                </div>
                <div class="col-md-2">
                    <asp:Button CssClass="btn btn-danger btn-block" ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"  CausesValidation="false" />
                </div>
            </div>
        </div>
        <hr />
        <div class="row mt-6 d-flex justify-content-center">
            <div class="container">
                <div class="col-md-6">
                    <asp:Label ID="lblSearchProductCategory" runat="server" Text="Sort Product Master By" Font-Bold="True" Font-Size="Larger" />
                    <asp:DropDownList ID="ddlSearchProductCategory" runat="server" Height="36px" AutoPostBack="true" OnSelectedIndexChanged="ddlSearchProductCategory_SelectedIndexChanged" >
                        <asp:ListItem Text="Product Name" Value="product_name" />
                        <asp:ListItem Text="Product Category" Value="product_category" />
                        <asp:ListItem Text="Rate" Value="product_rate" />
                        <asp:ListItem Text="Opening Stock" Value="product_open_stock" />
                    </asp:DropDownList>
                </div>
                <h2>Product List</h2>
                <div class="col-md-6">
                    <asp:GridView ID="gridViewProducts" runat="server" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="id" HeaderText="ID" />
                            <asp:BoundField DataField="product_name" HeaderText="Product Name" />
                            <asp:BoundField DataField="product_category" HeaderText="Product Category" />
                            <asp:BoundField DataField="product_rate" HeaderText="Rate" />
                            <asp:BoundField DataField="product_open_stock" HeaderText="Opening Stock" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
