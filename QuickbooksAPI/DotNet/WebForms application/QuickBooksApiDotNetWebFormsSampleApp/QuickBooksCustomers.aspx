<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeBehind="QuickBooksCustomers.aspx.cs"
    Inherits="QuickBooksApiDotNetWebFormsSampleApp.QuickBooksCustomers" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    
    <div>
        <h2>
            QuickBooks Customer Data</h2>
        <br />
        <div style="overflow: auto; width: 100%" runat="server" id="GridLocation">
            <asp:GridView ID="grdQuickBooksCustomers" AutoGenerateColumns="False" runat="server"
                Width="900px" CellPadding="4" ForeColor="#333333" GridLines="None">
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                <Columns>
                    <asp:TemplateField HeaderText="Customer Name">
                        <ItemTemplate>
                            <asp:Label ID="lblCustomerName" runat="server" Text='<%# Bind("ShowAs") %>'></asp:Label></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Open Balance">
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("OpenBalance.Amount", "{0:C}") %>'></asp:Label></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Overdue Balance">
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# (null == Eval("OverDueBalance")) ? "$0.00" : Eval("OverDueBalance", "{0:C}") %>'></asp:Label></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="NonProfit?">
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("NonProfit") %>'></asp:Label></ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EditRowStyle BackColor="#999999" />
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
            </asp:GridView>
        </div>
        <br />
        <br />
    </div>
    <div runat="server" id="MessageLocation">
        No Customer Data Found!
        <br />
        <br />
        <a href="Default.aspx">Back to Home Page</a>
    </div>
    
</asp:Content>
