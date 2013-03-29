using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace HelloIntuitAnywhere
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Get tokens from the AppSettings in config files
            string applicationToken = ConfigurationManager.AppSettings["applicationToken"];
            string consumerKey = ConfigurationManager.AppSettings["consumerKey"];
            string consumerSecret = ConfigurationManager.AppSettings["consumerSecret"];
            // Check whether the keys are null or empty.
            if (string.IsNullOrWhiteSpace(applicationToken) || string.IsNullOrWhiteSpace(consumerKey) || string.IsNullOrWhiteSpace(consumerSecret))
            {
                // Show Error message
                this.errorDiv.Visible = true;
                this.mainContetntDiv.Visible = false;
            }
            else
            {
                // Show main content
                this.errorDiv.Visible = false;
                this.mainContetntDiv.Visible = true;
            }

            // Read value from session and check flag which decides the display of blue dot menu
            object flag = Session["Flag"];
            if (flag != null)
            {
                bool flagValue = Convert.ToBoolean(flag.ToString());
                if (flagValue)
                {
                    // Show BlueDot widget
                    this.blueDotDiv.Visible = true;
                }
                else
                {
                    // Disable BlueDot widget
                    this.blueDotDiv.Visible = false;
                }
            }
            else
            {
                // Disable BlueDot widget
                this.blueDotDiv.Visible = false;
            }
        }
    }
}
