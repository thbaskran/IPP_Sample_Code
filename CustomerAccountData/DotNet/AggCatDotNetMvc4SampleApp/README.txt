App settings must be hard-coded in Web.config to use this sample application.

  <appSettings>
    <!-- Aggregation and Categorization Settings -->
    <add key="ConsumerKey" value="<<ConsumerKey>>" />
    <add key="ConsumerSecret" value="<<ConsumerSecret>>" />
    <add key="SAMLIdentityProviderID" value="<<SAMLIdentityProviderID>>" />
    <add key="PrivateKeyPath" value="<<PrivateKeyPath>>" />
    <add key="PrivateKeyPassword" value="<<PrivateKeyPassword>>" />
    <add key="CustomerId" value="<<CustomerId>>" />   <!-- This can be any unique identifier for your customer.  Keep track of these IDs, as you will need to call deleteCustomer to free up the user in development if necessary. -->
  </appSettings>