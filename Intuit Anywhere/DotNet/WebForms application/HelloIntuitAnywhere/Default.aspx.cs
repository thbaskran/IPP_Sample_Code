using System;
using System.Web;


namespace HelloIntuitAnywhere
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region OpenId

            // Hide Connect to Quickbooks widget and show Sign in widget
            IntuitInfo.Visible = false;
            IntuitSignin.Visible = true;
            // If Session has keys
            if (HttpContext.Current.Session.Keys.Count > 0)
            {
                // If there is a key OpenIdResponse
                if (HttpContext.Current.Session["OpenIdResponse"] != null)
                {
                    // Show the Sign in widget and disable the Connect to Quickbooks widget
                    IntuitSignin.Visible = false;
                    IntuitInfo.Visible = true;
                }

                // Sow information of the user if the keys are in the session
                if (Session["FriendlyIdentifier"] != null)
                {
                    friendlyIdentifier.Text = Session["friendlyIdentifier"].ToString();
                }
                if (Session["FriendlyName"] != null)
                {
                    friendlyName.Text = Session["FriendlyName"].ToString();
                }
                else
                {
                    friendlyName.Text = "User Didnt Login Via OpenID, look them up in your system";
                }

                if (Session["FriendlyEmail"] != null)
                {
                    friendlyEmail.Text = Session["FriendlyEmail"].ToString();
                }
                else
                {
                    friendlyEmail.Text = "User Didnt Login Via OpenID, look them up in your system";
                }
            }
            #endregion

            #region oAuth

            // If session has accesstoken and InvalidAccessToken is null
            if (HttpContext.Current.Session["accessToken"] != null && HttpContext.Current.Session["InvalidAccessToken"] == null)
            {
                // Show oAuthinfo(contains Get Customers Quickbooks List) and disable Connect to quickbooks widget
                oAuthinfo.Visible = true;
                connectToIntuitDiv.Visible = false;
            }

            #endregion
        }
    }

}
