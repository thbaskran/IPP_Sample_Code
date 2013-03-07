<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="DotNetAggCatSample._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .style1
        {
            color: #CC3300;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Welcome to the IPP .NET Customer Account Data API Demo!
    </h2>
    <asp:PlaceHolder runat="server" ID="ConfigElementsMissing" Visible="False">
        <p class="style1">
            The following fields must be specified in the web.config to use this demo:<br />
            <ul class="style1">
                <li>ConsumerKey</li>
                <li>ConsumerSecret</li>
                <li>SAMLIdentityProviderID</li>
                <li>PrivateKeyPath</li>
                <li>PrivateKeyPassword</li>
                <li>SAMLEndpoint</li>
            </ul>
    </asp:PlaceHolder>
    <p>
        For more information, visit our <a href="https://ipp.developer.intuit.com/index.php?title=0010_Intuit_Partner_Platform/0020_Aggregation_%26_Categorization_Apps"
            title="AggCat Docs">documentation on the Customer Account Data API</a> on developer.intuit.com.
    </p>
    <p>
        <strong>Customer Account Data API</strong>, with the core feature set of Aggregation
        and Categorization (<strong>AggCat</strong>), offers many unique benefits to you.&nbsp;
        For the rest of this documentation we refer to the Customer Account Data API as
        AggCat API.&nbsp; Click the following links for additional information on how your
        application can leverage these configurations to enhance and/or customize your offering:&nbsp;</p>
    <p>
        <ul>
            <li><a target="blank" href="https://ipp.developer.intuit.com/index.php?title=0010_Intuit_Partner_Platform/0020_Aggregation_%26_Categorization_Apps/0005_Service_Features/0010_Account_Type"
                title="Account Type">Account Types Supported</a></li>
            <li><a target="blank" href="https://ipp.developer.intuit.com/index.php?title=0010_Intuit_Partner_Platform/0020_Aggregation_%26_Categorization_Apps/0005_Service_Features/0020_Auto_Discovery"
                title="Auto Discovery">Auto Discovery of New Accounts</a></li>
            <li><a target="blank" href="https://ipp.developer.intuit.com/index.php?title=0010_Intuit_Partner_Platform/0020_Aggregation_%26_Categorization_Apps/0005_Service_Features/0030_Batch_Export"
                title="Batch Export">Batch Export</a></li>
            <li><a target="blank" href="https://ipp.developer.intuit.com/index.php?title=0010_Intuit_Partner_Platform/0020_Aggregation_%26_Categorization_Apps/0005_Service_Features/0040_Data_Availability"
                title="Data Availability">Data Availability</a></li>
            <li><a target="blank" href="https://ipp.developer.intuit.com/index.php?title=0010_Intuit_Partner_Platform/0020_Aggregation_%26_Categorization_Apps/0005_Service_Features/Categorization"
                title="Categorization">Intuit Categorization</a><a href="https://ipp.developer.intuit.com/index.php?title=0010_Intuit_Partner_Platform/0020_Aggregation_%26_Categorization_Apps/0005_Service_Features/Categorization"
                    title="Categorization">&nbsp;</a></li>
            <li><a target="blank" href="https://ipp.developer.intuit.com/index.php?title=0010_Intuit_Partner_Platform/0020_Aggregation_%26_Categorization_Apps/0005_Service_Features/Multi-Factor_Authentication"
                title="Multi Factor Authentication">Multi-Factor Authentication (MFA)</a></li>
            <li><a target="blank" href="https://ipp.developer.intuit.com/index.php?title=0010_Intuit_Partner_Platform/0020_Aggregation_%26_Categorization_Apps/0005_Service_Features/Software_Development_Kits"
                title="Software Development Kits">Software Development Kits</a></li>
        </ul>
    </p>
</asp:Content>
