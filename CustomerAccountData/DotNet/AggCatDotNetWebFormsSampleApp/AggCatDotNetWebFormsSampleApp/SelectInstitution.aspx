<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="SelectInsitution.aspx.cs" Inherits="AggCatDotNetWebFormsSampleApp.SelectInstitution" %>
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
            font-size: medium;
            color: #336699;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <span class="style1"><strong>Select Your Insitution</strong></span><br />
    
    <br />
    <asp:DropDownList ID="institutions" runat="server" AutoPostBack="True" OnSelectedIndexChanged="institutions_SelectedIndexChanged">
    </asp:DropDownList>&nbsp;&nbsp;<asp:LinkButton runat="server" ID="SelectCCBank" onclick="SelectCCBank_Click">Select CC Bank</asp:LinkButton>
    <br/><br/>
    <asp:PlaceHolder runat="server" ID="InstitutionDetails" Visible="False">
        <p class="style2">
            <strong>Insitution Details:</strong></p>
        <p>
            <strong>ID:</strong>&nbsp;
            <asp:Label runat="server" ID="InstitutionId"></asp:Label><br />
            <strong>Name:</strong>&nbsp;
            <asp:Label runat="server" ID="InsitutionName"></asp:Label>
            <br />
            <strong>Website:</strong>&nbsp;
            <asp:Label runat="server" ID="Website"></asp:Label>
            <br />
            <strong>Phone Number:</strong>&nbsp;
            <asp:Label runat="server" ID="PhoneNumer"></asp:Label>
            <br />
            <strong>Address:</strong>&nbsp;
            <asp:Label runat="server" ID="Address"></asp:Label>
            <br />
            <strong>Email:</strong>&nbsp;
            <asp:Label runat="server" ID="Email"></asp:Label>
        </p>
        <asp:Button ID="FindAccounts" runat="server" Text="Find Accounts At this Institution"
            Style="color: #FFFFFF; font-weight: 700; font-family: Calibri; font-size: large;
            background-color: #6699FF" OnClick="FindAccounts_Click" />
    </asp:PlaceHolder>
</asp:Content>
