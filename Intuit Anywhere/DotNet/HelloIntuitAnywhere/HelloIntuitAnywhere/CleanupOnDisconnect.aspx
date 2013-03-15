<%@ Page Title="Cleanup on Disconnect Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="CleanupOnDisconnect.aspx.cs" Inherits="HelloIntuitAnywhere.CleanupOnDisconnect" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
 <div>
        <h2>Succesfully Disconnected</h2>
        <p>
            Your QuickBooks file is no longer connected to Hello Intuit Anywhere.  Any changes that you make in QuickBooks will no longer be reflected in this app.
        </p>
        <p>
            If you did this in error, please go to your settings page to reauthorize this app to connect to your QuickBooks company.
        </p>
        <p>
            <b>Please note: </b>Your subscription to Hello Intuit Anywhere has not been cancelled.
            To cancel your subscription, please contact support.</p>
    </div>
</asp:Content>