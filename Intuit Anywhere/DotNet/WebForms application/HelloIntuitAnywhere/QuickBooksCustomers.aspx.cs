using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Web;
using Intuit.Ipp.Core;
using Intuit.Ipp.Security;
using Intuit.Ipp.Services;
using HelloIntuitAnywhere.Utilities;

namespace HelloIntuitAnywhere
{
    /// <summary>
    /// Controller which connects to QuickBooks and pulls customer Info.
    /// This flow will make use of Data Service SDK V2 to create OAuthRequest and connect to 
    /// Customer Data under the service context and display data inside the grid.
    /// </summary>
    public partial class QuickBooksCustomers : System.Web.UI.Page
    {

        /// <summary>
        /// Page Load Event, pulls Customer data from QuickBooks using SDK and Binds it to Grid
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Event Args.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session.Keys.Count > 0)
            {
                String realmId = HttpContext.Current.Session["realm"].ToString();
                String accessToken = HttpContext.Current.Session["accessToken"].ToString();
                String accessTokenSecret = HttpContext.Current.Session["accessTokenSecret"].ToString();
                String consumerKey = ConfigurationManager.AppSettings["consumerKey"].ToString(CultureInfo.InvariantCulture);
                String consumerSecret = ConfigurationManager.AppSettings["consumerSecret"].ToString(CultureInfo.InvariantCulture);
                IntuitServicesType intuitServiceType = (IntuitServicesType)HttpContext.Current.Session["intuitServiceType"];

                OAuthRequestValidator oauthValidator = new OAuthRequestValidator(accessToken, accessTokenSecret, consumerKey, consumerSecret);
                ServiceContext context = new ServiceContext(oauthValidator, realmId, intuitServiceType);
                DataServices commonService = new DataServices(context);

                try
                {
                    switch(intuitServiceType )
                    {
                        case IntuitServicesType.QBO:
                            Intuit.Ipp.Data.Qbo.Customer qboCustomer = new Intuit.Ipp.Data.Qbo.Customer();
                            IEnumerable<Intuit.Ipp.Data.Qbo.Customer> qboCustomers =  commonService.FindAll(qboCustomer, 1, 10) as IEnumerable<Intuit.Ipp.Data.Qbo.Customer>;
                            grdQuickBooksCustomers.DataSource = qboCustomers;
                            break;
                        case IntuitServicesType.QBD:
                            //FindAll() is a GET operation for QBD, so we need to use the respective Query object instead to POST the start page and records per page (ChunkSize).
                            Intuit.Ipp.Data.Qbd.CustomerQuery qbdCustomerQuery = new Intuit.Ipp.Data.Qbd.CustomerQuery();
                            qbdCustomerQuery.ItemElementName = Intuit.Ipp.Data.Qbd.ItemChoiceType4.StartPage;
                            qbdCustomerQuery.Item = "1";
                            qbdCustomerQuery.ChunkSize = "10";
                            IEnumerable<Intuit.Ipp.Data.Qbd.Customer> qbdCustomers = qbdCustomerQuery.ExecuteQuery<Intuit.Ipp.Data.Qbd.Customer>(context) as IEnumerable<Intuit.Ipp.Data.Qbd.Customer>;
                            grdQuickBooksCustomers.DataSource = qbdCustomers;
                            break;
                        default:
                            throw new Exception("Data Source not defined.");
                    }

                    grdQuickBooksCustomers.DataBind();
                    if (grdQuickBooksCustomers.Rows.Count > 0)
                    {
                        GridLocation.Visible = true;
                        MessageLocation.Visible = false;
                    }
                    else
                    {
                        GridLocation.Visible = false;
                        MessageLocation.Visible = true;
                    }
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}