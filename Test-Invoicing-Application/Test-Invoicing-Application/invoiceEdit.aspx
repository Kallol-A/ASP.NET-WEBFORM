<%@ Page Language="C#" AutoEventWireup="true" CodeFile="invoiceEdit.aspx.cs" Inherits="invoiceEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Test Invoice | ARB Software</title>
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet"/>
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css"/>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <style>
        .text-right {
            text-align: right;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div class="container">
                <h1>Edit Invoice</h1>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Label ID="lblMsg" runat="server"></asp:Label>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <asp:Label ID="lblInvoiceNo" runat="server" Text="Invoice No." Font-Bold="True" Font-Size="Larger" ForeColor="Blue" CssClass="control-label"></asp:Label>
                            <asp:Label ID="lblInvoiceNoReqd" runat="server" Text="*" Font-Bold="True" Font-Size="Larger" ForeColor="Red" CssClass="control-label"></asp:Label>
                            <asp:TextBox ID="txtInvoiceNo" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                </div>
                        
                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <asp:Label ID="lblInvoiceDate" runat="server" Text="Date" Font-Bold="True" Font-Size="Larger" ForeColor="Blue" CssClass="control-label"></asp:Label>
                            <asp:Label ID="lblInvoiceDateReqd" runat="server" Text="*" Font-Bold="True" Font-Size="Larger" ForeColor="Red" CssClass="control-label"></asp:Label>
                            <asp:TextBox ID="txtInvoiceDate" runat="server" CssClass="form-control datepicker" />
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <asp:Label ID="lblInvoiceParty" runat="server" Text="Party Name" Font-Bold="True" Font-Size="Larger" ForeColor="Blue" CssClass="control-label"></asp:Label>
                            <asp:Label ID="lblInvoicePartyReqd" runat="server" Text="*" Font-Bold="True" Font-Size="Larger" ForeColor="Red" CssClass="control-label"></asp:Label>
                            <asp:TextBox ID="txtInvoiceParty" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                </div>
                <h3>Invoice Detail</h3>
                <div class="row">
                    <div class="col-md-12">
                        <div class="form-group">
                            <asp:Table class="table table-bordered" ID="tblInvoiceProductData" runat="server">
                                <asp:TableHeaderRow>
                                    <asp:TableHeaderCell>Product Name</asp:TableHeaderCell>
                                    <asp:TableHeaderCell>Quantity</asp:TableHeaderCell>
                                    <asp:TableHeaderCell>Rate (₹)</asp:TableHeaderCell>
                                    <asp:TableHeaderCell>Discount (₹)</asp:TableHeaderCell>
                                    <asp:TableHeaderCell>Tax (₹)</asp:TableHeaderCell>
                                    <asp:TableHeaderCell>Amount (₹)</asp:TableHeaderCell>
                                    <asp:TableHeaderCell>Action</asp:TableHeaderCell>
                                </asp:TableHeaderRow>
                                <asp:TableRow>
                                    <asp:TableCell>
                                        <asp:TextBox ID="txtProductName" runat="server" CssClass="form-control"></asp:TextBox>
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:TextBox ID="txtProductQuantity" runat="server" CssClass="form-control text-right"></asp:TextBox>
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:TextBox ID="txtProductRate" runat="server" CssClass="form-control text-right"></asp:TextBox>                                                
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:TextBox ID="txtProductDiscount" runat="server" CssClass="form-control text-right"></asp:TextBox>
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:TextBox ID="txtProductTax" runat="server" CssClass="form-control text-right"></asp:TextBox>
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control amount-input text-right" onkeypress="return false;" onkeydown="return preventDelete(event);"></asp:TextBox>
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:Button ID="btnAddRow" runat="server" Text="Add" CssClass="btn btn-primary" OnClick="btnAddRow_Click" />
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:GridView ID="gridViewProducts" runat="server" AutoGenerateColumns="false" Width="100%" OnRowEditing="gridViewProducts_RowEditing" OnRowDeleting="gridViewProducts_RowDeleting" OnRowUpdating="gridViewProducts_RowUpdating" OnRowCancelingEdit="gridViewProducts_RowCancelingEdit" DataKeyNames="product_name">
                            <Columns>
                                <%--<asp:BoundField DataField="id" HeaderText="Sl" />--%>
                                <asp:TemplateField HeaderText="Serial No.">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="product_name" HeaderText="Product Name" />

<%--                                <asp:TemplateField HeaderText="Product Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProductName" runat="server" Text='<%# Eval("product_name") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEditProductName" runat="server" Text='<%# Bind("product_name") %>' />
                                    </EditItemTemplate>
                                </asp:TemplateField>--%>

                                <asp:TemplateField HeaderText="Quantity">
                                    <ItemTemplate>
                                        <asp:Label ID="lblQuantity" runat="server" CssClass="text-right" Text='<%# Eval("product_quantity") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEditQuantity" runat="server" CssClass="text-right" AutoPostBack="true" Text='<%# Bind("product_quantity") %>' OnTextChanged="UpdateAmountTextBox"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Rate (₹)">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRate" runat="server" Text='<%# Eval("product_rate") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEditRate" runat="server" CssClass="text-right" AutoPostBack="true" Text='<%# Bind("product_rate") %>' OnTextChanged="UpdateAmountTextBox"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Discount (₹)">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDiscount" runat="server" Text='<%# Eval("product_discount") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEditDiscount" runat="server" CssClass="text-right" AutoPostBack="true" Text='<%# Bind("product_discount") %>' OnTextChanged="UpdateAmountTextBox"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Tax (₹)">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTax" runat="server" Text='<%# Eval("product_tax") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEditTax" runat="server" CssClass="text-right" AutoPostBack="true" Text='<%# Bind("product_tax") %>' OnTextChanged="UpdateAmountTextBox"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Amount (₹)">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAmount" runat="server" Text='<%# Eval("product_amount") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEditAmount" runat="server" CssClass="text-right" Text='<%# Bind("product_amount") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Actions">
                                    <ItemTemplate>
                                        <div style="text-align: center;">
                                            <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="images/edit.png" Height="30" Width="30" CommandName="Edit" OnClientClick="return confirm('Are you sure you want to edit this item?');" />
                                            <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="images/delete.png" Height="30" Width="30" CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                                        </div>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <div style="text-align: center;width: 65px;">
                                            <asp:ImageButton ID="btnUpdate" runat="server" ImageUrl="images/update.png" Height="30" Width="30" CommandName="Update" OnClientClick="return confirm('Are you sure you want to save your changes?');" />
                                            <asp:ImageButton ID="btnCancel" runat="server" ImageUrl="images/cancel.png" Height="30" Width="30" CommandName="Cancel" OnClientClick="return confirm('Are you sure you want to discard your changes?');" />
                                        </div>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>

                        <div class="row">
                            <div class="col-md-10 d-flex justify-content-end">
                                <div class="col-md-4 pr-0">
                                    <asp:Label ID="lblGrandTotal" runat="server" Text="Grand Total Amount:" Font-Bold="True" Font-Size="Larger" ForeColor="Blue" CssClass="control-label"></asp:Label>
                                </div>
                                <div class="col-md-3 pl-5 pr-0">
                                    <asp:TextBox ID="txtGrandTotal" runat="server" Text="0.00" Font-Bold="True" CssClass="form-control text-right" onkeypress="return false;" onkeydown="return preventDelete(event);"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <asp:Label ID="lblMandatoryMessage" runat="server" Text="All * marked fields are mandatory" Font-Size="Larger" ForeColor="Red" CssClass="control-label"></asp:Label>
                <div class="row d-flex justify-content-end">
                    <div class="col-md-2"> <!-- Each button takes 1/4 of their container's width (3 columns out of 12) -->
                        <asp:Button CssClass="btn btn-primary btn-block" ID="btnSubmit" runat="server" Text="Save" OnClick="btnSubmit_Click" />
                    </div>
                    <div class="col-md-2">
                        <asp:Button CssClass="btn btn-danger btn-block" ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" OnClick="btnCancel_Click" />
                    </div>
                </div>
            </div>
        </div>
    </form>
    <script type="text/javascript">
        $(function () {
            $(".datepicker").datepicker({
                dateFormat: 'dd/mm/yy',  // Date format
                changeMonth: true,      // Show month dropdown
                changeYear: true,       // Show year dropdown
                yearRange: "-100:+0",   // Allow selection of the last 100 years
                showButtonPanel: true,  // Show a button panel with today and done buttons
            });

            // Function to calculate the amount
            function calculateAmount() {
                var rate = parseFloat(document.getElementById('<%= txtProductRate.ClientID %>').value) || 0;
                var quantity = parseFloat(document.getElementById('<%= txtProductQuantity.ClientID %>').value) || 0;
                var discount = parseFloat(document.getElementById('<%= txtProductDiscount.ClientID %>').value) || 0;
                var tax = parseFloat(document.getElementById('<%= txtProductTax.ClientID %>').value) || 0;

                var amount = ((rate * quantity) - discount) + tax;
                document.getElementById('<%= txtAmount.ClientID %>').value = amount.toFixed(2); // Set the value with 2 decimal places
            }

            // Function to calculate and update the Grand Total
            function calculateGrandTotal() {
                var grandTotal = 0;

                // Get the GridView control
                var gridView = document.getElementById('<%= gridViewProducts.ClientID %>');

                // Get all the rows in the GridView
                var rows = gridView.getElementsByTagName('tr');

                // Loop through the rows, starting from the second row (index 1) to skip the header row
                for (var i = 1; i < rows.length; i++) {
                    var amountCell = rows[i].cells[6]; // Assuming "Amount" is in the 7th cell (index 6)

                    // Get the text from the Amount cell and parse it as a floating-point number
                    var amount = parseFloat(amountCell.innerText);

                    // Add the amount to the Grand Total
                    grandTotal += amount;
                }

                // Update the txtGrandTotal TextBox with the calculated Grand Total
                document.getElementById('<%= txtGrandTotal.ClientID %>').value = grandTotal.toFixed(2); // Set the value with 2 decimal places
            }

            // Attach the calculateAmount function to the change event of the input fields
            document.getElementById('<%= txtProductRate.ClientID %>').addEventListener('change', calculateAmount);
            document.getElementById('<%= txtProductQuantity.ClientID %>').addEventListener('change', calculateAmount);
            document.getElementById('<%= txtProductDiscount.ClientID %>').addEventListener('change', calculateAmount);
            document.getElementById('<%= txtProductTax.ClientID %>').addEventListener('change', calculateAmount);

            document.getElementById('<%= btnAddRow.ClientID %>').addEventListener('click', calculateGrandTotal);

            // Initial calculation
            calculateAmount();
            calculateGrandTotal();
        });

        function preventDelete(event) {
            // Get the key code of the pressed key
            var keyCode = event.keyCode || event.which;

            // Prevent the backspace and delete keys (8 is backspace, 46 is delete)
            if (keyCode === 8 || keyCode === 46) {
                event.preventDefault();
                return false;
            }
        }
    </script>
</body>
</html>
