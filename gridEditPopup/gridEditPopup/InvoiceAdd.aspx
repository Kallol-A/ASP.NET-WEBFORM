<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InvoiceAdd.aspx.cs" Inherits="InvoiceAdd" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Test Invoice | ARB Software</title>
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet"/>
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css"/>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>

    <style>
        .text-right {
            text-align: right;
        }
    </style>

</head>
<body>
    <form id="frmInvoice" runat="server">
        <asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server"></asp:ScriptManager>
        <div>
            <asp:MultiView ID="MultiView1" runat="server">

                <asp:View ID="create_invoice" runat="server">
                    <div class="container">
                        <h1>Add Invoice</h1>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Label ID="lblMsg" runat="server"></asp:Label>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-12 d-flex justify-content-end">
                                <asp:Button CssClass="btn btn-primary" ID="btnGoToViewList" runat="server" Text="View List" OnClick="btnGoToViewList_click" />
                            </div>

                            <div class="col-md-4">
                                <div class="form-group">
                                    <asp:Label ID="lblInvoiceNo" runat="server" Text="Invoice No." Font-Bold="True" Font-Size="Larger" ForeColor="Blue" CssClass="control-label"></asp:Label>
                                    <asp:Label ID="lblInvoiceNoReqd" runat="server" Text="*" Font-Bold="True" Font-Size="Larger" ForeColor="Red" CssClass="control-label"></asp:Label>
                                    <asp:TextBox ID="txtInvoiceNo" runat="server" CssClass="form-control" />
                                    <asp:Label ID="lblInvoiceNoValid" runat="server" ForeColor="Red"></asp:Label>
                                </div>
                            </div>
                        </div>
                        
                        <div class="row">
                            <div class="col-md-4">
                                <div class="form-group">
                                    <asp:Label ID="lblInvoiceDate" runat="server" Text="Date" Font-Bold="True" Font-Size="Larger" ForeColor="Blue" CssClass="control-label"></asp:Label>
                                    <asp:Label ID="lblInvoiceDateReqd" runat="server" Text="*" Font-Bold="True" Font-Size="Larger" ForeColor="Red" CssClass="control-label"></asp:Label>
                                    <asp:TextBox ID="txtInvoiceDate" runat="server" CssClass="form-control datepicker" />
                                    <asp:Label ID="lblInvoiceDateValid" runat="server" ForeColor="Red"></asp:Label>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-4">
                                <div class="form-group">
                                    <asp:Label ID="lblInvoiceParty" runat="server" Text="Party Name" Font-Bold="True" Font-Size="Larger" ForeColor="Blue" CssClass="control-label"></asp:Label>
                                    <asp:Label ID="lblInvoicePartyReqd" runat="server" Text="*" Font-Bold="True" Font-Size="Larger" ForeColor="Red" CssClass="control-label"></asp:Label>
                                    <asp:TextBox ID="txtInvoiceParty" runat="server" CssClass="form-control" />
                                    <asp:Label ID="lblInvoicePartyValid" runat="server" ForeColor="Red"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <h3>Invoice Detail</h3>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Label ID="lblErrorMessage" runat="server"></asp:Label>
                            </div>
                        </div>
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
                                <asp:GridView ID="gridViewProducts" runat="server" AutoGenerateColumns="false" Width="100%" OnRowDeleting="gridViewProducts_RowDeleting" DataKeyNames="product_name">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Serial No.">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="product_id" Visible="false" />

                                        <asp:TemplateField HeaderText="Product Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblProductName" runat="server" Text='<%# Eval("product_name") %>' data-product_name='<%# Eval("product_name") %>' />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtEditProductName" runat="server" Text='<%# Bind("product_name") %>' />
                                            </EditItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Quantity">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQuantity" runat="server" CssClass="text-right" Text='<%# Eval("product_quantity") %>' data-quantity='<%# Eval("product_quantity") %>' />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtEditQuantity" runat="server" CssClass="text-right" AutoPostBack="true" Text='<%# Bind("product_quantity") %>' OnTextChanged="UpdateAmountTextBox"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Rate (₹)">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRate" runat="server" Text='<%# Eval("product_rate") %>' data-rate='<%# Eval("product_rate") %>' />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtEditRate" runat="server" CssClass="text-right" AutoPostBack="true" Text='<%# Bind("product_rate") %>' OnTextChanged="UpdateAmountTextBox"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Discount (₹)">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDiscount" runat="server" Text='<%# Eval("product_discount") %>' data-discount='<%# Eval("product_discount") %>' />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtEditDiscount" runat="server" CssClass="text-right" AutoPostBack="true" Text='<%# Bind("product_discount") %>' OnTextChanged="UpdateAmountTextBox"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Tax (₹)">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTax" runat="server" Text='<%# Eval("product_tax") %>' data-tax='<%# Eval("product_tax") %>' />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtEditTax" runat="server" CssClass="text-right" AutoPostBack="true" Text='<%# Bind("product_tax") %>' OnTextChanged="UpdateAmountTextBox"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Amount (₹)">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmount" runat="server" Text='<%# Eval("product_amount") %>' data-amount='<%# Eval("product_amount") %>' />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtEditAmount" runat="server" CssClass="text-right" Text='<%# Bind("product_amount") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Actions">
                                            <ItemTemplate>
                                                <div style="text-align: center;">
                                                    <button type="button" id="btnEdit" class="btn btn-lg">
                                                        <img src="assets/img/edit.png" height="30" width="30" alt="Edit" style="vertical-align: middle;" />
                                                    </button>
                                                    <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="assets/img/delete.png" Height="30" Width="30" CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" style="vertical-align: middle;" />
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <asp:Label ID="lblGridViewValid" runat="server" ForeColor="Red"></asp:Label>

                                <!-- Modal -->
                                    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
                                      <div class="modal-dialog" role="document">
                                        <div class="modal-content">
                                          <div class="modal-header">
                                            <h5 class="modal-title" id="exampleModalLabel">Edit Product</h5>
                                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                              <span aria-hidden="true">&times;</span>
                                            </button>
                                          </div>
                                          <div class="modal-body">

                                              <div class="form-group">
                                                <label for="product_name" class="col-form-label">Product Name:</label>
                                                <asp:TextBox class="form-control" ID="product_name" runat="server" onkeypress="return false;" onkeydown="return preventDelete(event);" />
                                              </div>

                                              <div class="form-group">
                                                <label for="product_quantity" class="col-form-label">Quantity:</label>
                                                <asp:TextBox class="form-control" ID="product_quantity" runat="server" />
                                              </div>

                                              <div class="form-group">
                                                <label for="product_rate" class="col-form-label">Rate:</label>
                                                <asp:TextBox class="form-control" ID="product_rate" runat="server" />
                                              </div>

                                              <div class="form-group">
                                                <label for="product_discount" class="col-form-label">Discount:</label>
                                                <asp:TextBox class="form-control" ID="product_discount" runat="server" />
                                              </div>

                                              <div class="form-group">
                                                <label for="product_tax" class="col-form-label">Tax:</label>
                                                <asp:TextBox class="form-control" ID="product_tax" runat="server" />
                                              </div>

                                              <div class="form-group">
                                                <label for="product_amount" class="col-form-label">Amount:</label>
                                                <asp:TextBox class="form-control" ID="product_amount" runat="server" onkeypress="return false;" onkeydown="return preventDelete(event);" />
                                              </div>

                                          </div>
                                          <div class="modal-footer">
                                            <button type="button" class="btn btn-danger" data-dismiss="modal">Cancel</button>
                                            <asp:Button ID="btnUpdate" class="btn btn-primary" runat="server" Text="Update" OnClick="btnUpdate_Click" />
                                          </div>
                                        </div>
                                      </div>
                                    </div>
                                <!-- End Modal -->

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
                        <asp:Label ID="lblTest" runat="server" Font-Size="Larger" ForeColor="Red" CssClass="control-label"></asp:Label>
                        <asp:Label ID="lblMandatoryMessage" runat="server" Text="All * marked fields are mandatory" Font-Size="Larger" ForeColor="Red" CssClass="control-label"></asp:Label>
                        <div class="row d-flex justify-content-end">
                            <div class="col-md-2"> <!-- Each button takes 1/4 of their container's width (3 columns out of 12) -->
                                <asp:Button CssClass="btn btn-primary btn-block" ID="btnSubmit" runat="server" Text="Save" OnClick="btnSubmit_Click" />
                            </div>
                            <div class="col-md-2">
                                <asp:Button CssClass="btn btn-danger btn-block" ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" />
                            </div>
                        </div>
                    </div>
                </asp:View>

                <asp:View ID="list_invoice" runat="server">
                    <div class="container">
                        <h1>List Invoice</h1>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Label ID="lblMsgList" runat="server"></asp:Label>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-12 d-flex justify-content-end">
                                <asp:Button CssClass="btn btn-primary" ID="btnGoToNew" runat="server" Text="New" OnClick="btnGoToNew_click" />
                            </div>
                        </div>

                        <h2>Invoice List</h2>
                        <div class="col-md-12">
                            <asp:GridView ID="gridViewInvoices" runat="server" AutoGenerateColumns="false" Width="100%" DataKeyNames="id" OnRowDeleting="gridViewInvoices_RowDeleting">
                                <Columns>
                                    <asp:TemplateField HeaderText="Serial No.">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="inv_no" HeaderText="Invoice No." />
                                    <asp:BoundField DataField="inv_date" HeaderText="Invoice Date" DataFormatString="{0:dd-MM-yyyy}" />
                                    <asp:BoundField DataField="inv_party" HeaderText="Name of Party" />

                                    <asp:TemplateField HeaderText="Amount">
                                        <ItemTemplate>
                                            <%# Eval("inv_amount", "{0:0.00}") %>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Actions">
                                        <ItemTemplate>
                                            <div class="row">
                                                <div class="col-md-2 text-center">
                                                    <asp:HyperLink ID="lnkViewEdit" runat="server" NavigateUrl='<%# Eval("id", "invoiceEdit.aspx?id={0}") %>'>
                                                        <asp:Image ID="img" runat="server" ImageUrl="assets/img/edit.png" Height="30" Width="30" />
                                                    </asp:HyperLink>
                                                </div>
                                                <div class="col-md-2 text-center">
                                                    <asp:ImageButton ID="btnDeleteInv" runat="server" ImageUrl="assets/img/delete.png" Height="30" Width="30" CommandName="Delete" CommandArgument='<%# Eval("id") %>' OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                                                </div>
                                            </div>

                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </asp:View>
            </asp:MultiView>

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

            $("#btnSubmit").click(function () {
                var invoiceNo = $("#<%= txtInvoiceNo.ClientID %>").val();
                var invoiceDate = $("#<%= txtInvoiceDate.ClientID %>").val();
                var invoiceParty = $("#<%= txtInvoiceParty.ClientID %>").val();

                $("#<%= lblInvoiceNoValid.ClientID %>").text(!invoiceNo ? "Invoice No. cannot be blank!" : "");
                $("#<%= lblInvoiceDateValid.ClientID %>").text(!invoiceDate ? "Invoice Date cannot be blank!" : "");
                $("#<%= lblInvoicePartyValid.ClientID %>").text(!invoiceParty ? "Party Name cannot be blank!" : "");

                var gridViewRows = $("#<%= gridViewProducts.ClientID %> tr").length - 1; // Subtract 1 for the header row
                $("#<%= lblGridViewValid.ClientID %>").text(gridViewRows <= 0 ? "Please add at least one product in the invoice." : "");

                if (!invoiceNo || !invoiceDate || !invoiceParty || gridViewRows <= 0) {
                    return false; // Prevent form submission
                }
                return true;
            });

            // Capture the click event of the "Edit" button
            $("#gridViewProducts").on("click", "#btnEdit", function () {
                // Get the row containing the clicked button
                var row = $(this).closest("tr");

                // Retrieve data from the row and populate the modal fields
                var productName = row.find('[data-product_name]').data('product_name');
                var quantity = row.find('[data-quantity]').data('quantity');
                var rate = row.find('[data-rate]').data('rate');
                var discount = row.find('[data-discount]').data('discount');
                var tax = row.find('[data-tax]').data('tax');
                var amount = row.find('[data-amount]').data('amount');

                // Populate the modal fields with the retrieved data
                $("#product_name").val(productName);
                $("#product_quantity").val(quantity);
                $("#product_rate").val(rate);
                $("#product_discount").val(discount);
                $("#product_tax").val(tax);
                $("#product_amount").val(amount);

                // Show the modal
                $('#myModal').modal('show');
            });

            // Function to calculate the amount
            function calculateAmount() {
                var rate = parseFloat($('#<%= txtProductRate.ClientID %>').val()) || 0;
                var quantity = parseFloat($('#<%= txtProductQuantity.ClientID %>').val()) || 0;
                var discount = parseFloat($('#<%= txtProductDiscount.ClientID %>').val()) || 0;
                var tax = parseFloat($('#<%= txtProductTax.ClientID %>').val()) || 0;

                var amount = ((rate * quantity) - discount) + tax;
                $('#<%= txtAmount.ClientID %>').val(amount.toFixed(2)); // Set the value with 2 decimal places
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

            // Function to calculate and update product_amount using jQuery
            function calculateModalProductAmount() {
                var quantity = parseFloat($('#<%= product_quantity.ClientID %>').val());
                var rate = parseFloat($('#<%= product_rate.ClientID %>').val());
                var discount = parseFloat($('#<%= product_discount.ClientID %>').val());
                var tax = parseFloat($('#<%= product_tax.ClientID %>').val());

                var amount = quantity * rate - discount + tax;
        
                $('#<%= product_amount.ClientID %>').val(amount.toFixed(2)); // Format to 2 decimal places
            }

            // Attach the calculateProductAmount function to change events of relevant inputs using jQuery
            $('#<%= product_quantity.ClientID %>').change(calculateModalProductAmount);
            $('#<%= product_rate.ClientID %>').change(calculateModalProductAmount);
            $('#<%= product_discount.ClientID %>').change(calculateModalProductAmount);
            $('#<%= product_tax.ClientID %>').change(calculateModalProductAmount);

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

        function redirectToInvoiceEditPage(id) {
            window.location.href = "invoiceEdit.aspx?id=" + id;
            return false;
        }

    </script>
</body>
</html>
