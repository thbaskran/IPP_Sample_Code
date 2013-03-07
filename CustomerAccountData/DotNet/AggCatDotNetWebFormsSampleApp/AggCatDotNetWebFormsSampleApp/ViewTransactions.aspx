<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ViewTransactions.aspx.cs" Inherits="AggCatDotNetWebFormsSampleApp.ViewTransactions" %>
    <%@ MasterType VirtualPath="~/Site.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style1
        {
            font-size: large;
            color: #006699;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <p class="style1">
        <strong>Your Account Transactions&nbsp; </strong>
    </p>
    <p>
        <asp:GridView ShowHeaderWhenEmpty="True" ID="AccountsGridView" runat="server" AutoGenerateColumns="False"
            CellPadding="10" ForeColor="#333333" GridLines="None" OnPageIndexChanging="AccountsGridView_PageIndexChanging" AllowPaging="True" PageSize="10">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:BoundField DataField="amount" HeaderText="Amount" DataFormatString="{0:c}" />
                <asp:BoundField DataField="payeeName" HeaderText="Payee" />
                <asp:BoundField DataField="institutionTransactionId" HeaderText="Transaction ID" />
                <asp:BoundField DataField="postedDate" DataFormatString="{0:d}" HeaderText="Date Posted" />
            </Columns>
            <EditRowStyle BackColor="#2461BF" />
            <EmptyDataTemplate>
                <p>
                    There are no transactions to display.</p>
            </EmptyDataTemplate>
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#EFF3FB" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#F5F7FB" />
            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
            <SortedDescendingCellStyle BackColor="#E9EBEF" />
            <SortedDescendingHeaderStyle BackColor="#4870BE" />
            <SortedAscendingCellStyle BackColor="#F5F7FB"></SortedAscendingCellStyle>
            <SortedAscendingHeaderStyle BackColor="#6D95E1"></SortedAscendingHeaderStyle>
            <SortedDescendingCellStyle BackColor="#E9EBEF"></SortedDescendingCellStyle>
            <SortedDescendingHeaderStyle BackColor="#4870BE"></SortedDescendingHeaderStyle>
        </asp:GridView>
    </p>
    <p>
        Start Date:&nbsp;&nbsp;<asp:TextBox runat="server" ID="StartDate"></asp:TextBox><br />
        End Date:&nbsp;&nbsp;&nbsp;<asp:TextBox runat="server" ID="EndDate"></asp:TextBox><br />
        <br />
        <asp:Button runat="server" ID="RefreshTransactions" Text="View Transactions" Style="color: #FFFFFF;
            font-family: Calibri; font-size: large; background-color: #336699" 
            onclick="RefreshTransactions_Click" />
    </p>
</asp:Content>
