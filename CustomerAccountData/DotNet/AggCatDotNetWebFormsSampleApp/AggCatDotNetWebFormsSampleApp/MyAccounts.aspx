<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="MyAccounts.aspx.cs" Inherits="AggCatDotNetWebFormsSampleApp.MyAccounts" %>
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
        <strong>Your Accounts&nbsp; </strong>
    </p>
    <asp:GridView ShowHeaderWhenEmpty="True" ID="AccountsGridView" runat="server" AutoGenerateColumns="False"
        CellPadding="10" ForeColor="#333333" GridLines="None" OnRowCommand="AccountsGridView_RowCommand" DataKeyNames="accountId">
        <AlternatingRowStyle BackColor="White"  />
        <Columns>
            <asp:BoundField DataField="accountNumber" HeaderText="Number" />
            <asp:BoundField DataField="accountNickname" HeaderText="Name" />
            <asp:BoundField DataField="balanceAmount" DataFormatString="{0:c}" HeaderText="Balance" />
            <asp:BoundField DataField="balanceDate" DataFormatString="{0:d}" HeaderText="Balance As Of" />
            <asp:BoundField DataField="lastTxnDate" DataFormatString="{0:d}" HeaderText="Last Transaction Date" />
            <asp:ButtonField Text="View Transactions" CommandName="ViewTransactions" />
            <asp:ButtonField Text="Delete Account" CommandName="DeleteAccount" />
        </Columns>
        <EditRowStyle BackColor="#2461BF" />
        <EmptyDataTemplate>
            <p>
                There are no accounts to display.</p>
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
    <br />
        <asp:Button runat="server" ID="RefreshTransactions" Text="Add Accounts" Style="color: #FFFFFF;
            font-family: Calibri; font-size: large; background-color: #336699" 
            onclick="AddAccounts_Click" />
</asp:Content>
