<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="InstitutionLogin.aspx.cs" Inherits="AggCatDotNetWebFormsSampleApp.Institution_Login" ClientIDMode="Static" %>
    <%@ MasterType VirtualPath="~/Site.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style1
        {
            font-size: large;
            color: #006699;
        }
        .style2
        {
            font-size: small;
            color: #CC3300;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <p class="style1">
        <strong>Login to your Institution</strong></p>
    <p class="style2">
        Note:&nbsp; MFA Logins are not currently supported in this demo</p>
    <asp:Panel ID="LoginControls" runat="server">
    </asp:Panel>
    <asp:Button ID="Login" runat="server" Style="color: #FFFFFF; font-weight: 700; font-family: Calibri;
        font-size: large; background-color: #6699FF" onclick="Login_Click" Text="Login" />
</asp:Content>
