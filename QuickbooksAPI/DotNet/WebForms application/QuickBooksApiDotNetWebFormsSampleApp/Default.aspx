<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="QuickBooksApiDotNetWebFormsSampleApp._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Welcome to the QuickBooks API WebForms Sample Application
    </h2>
    <p>
        To learn more about the Intuit Partner Platform, please visit <a href="http://developer.intuit.com/"
            title="Intuit Partner Platform" target="_blank">developer.intuit.com</a>.
    </p>
    <p>
        Please view our <a href="http://ippdocs.intuit.com/0025_QuickBooksAPI"
            title="QuickBooks API" target="_blank">QuickBooks API</a> documentation for more information on integrating your application.
    </p>
    <!-- Code Written for Demonstration Purpose Only -->
    <div id="intuit OpenId Code Region">
        <div runat="server" id="IntuitSignin" style="padding-top: 30px">
            <ipp:login href="OpenIdHandler.aspx"></ipp:login>
        </div>
        <div runat="server" id="IntuitInfo">
            <strong>OpenID Information:</strong><br />
            Welcome:
            <asp:Label Visible="true" Text="" runat="server" ID="friendlyName" />
            <br />
            E-mail Address:
            <asp:Label Visible="true" Text="" runat="server" ID="friendlyEmail" /><br />
            Friendly Identifier:
            <asp:Label Visible="true" Text="" runat="server" ID="friendlyIdentifier" /><br />
            <br/>
            <div runat="server" id="connectToIntuitDiv">
                <ipp:connecttointuit></ipp:connecttointuit>
            </div>
            <br />
            <br />
        </div>
        <div runat="server" id="oAuthinfo" visible="false">
            <a href="QuickBooksCustomers.aspx" id="QBCustomers">Get QuickBooks Customer List</a><br />
            <br />
            <br />
            <a href="#" id="Disconnect" onclick="confirmPost('/Disconnect.aspx');">Disconnect from
                QuickBooks</a><br />
        </div>
    </div>
</asp:Content>
