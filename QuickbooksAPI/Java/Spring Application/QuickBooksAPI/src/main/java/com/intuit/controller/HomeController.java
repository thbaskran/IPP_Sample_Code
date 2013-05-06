package com.intuit.controller;

import java.util.ArrayList;
import java.util.List;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpSession;

import org.apache.log4j.Logger;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;

import com.intuit.ds.qb.QBCustomer;
import com.intuit.ds.qb.QBCustomerService;
import com.intuit.ds.qb.QBInvalidContextException;
import com.intuit.ds.qb.QBServiceFactory;
import com.intuit.platform.client.PlatformServiceType;
import com.intuit.platform.client.PlatformSessionContext;
import com.intuit.platform.client.security.OAuthCredentials;
import com.intuit.utils.WebUtils;

/*
 * This class is a controller for the application home page related activities.  
 */
@Controller
public class HomeController {

	public static final Logger LOG = Logger.getLogger(HomeController.class);

	/*
	 * This method is called when the user is redirected from Login Page /
	 * Application Page / Settings Page.
	 */
	@RequestMapping(value = "/home.htm", method = RequestMethod.GET)
	public String showHomePage(final HttpServletRequest request) {
		LOG.info("HomeController -> showHomePage()");

		String redirectTo;
		if (request.getSession().getAttribute("invalidateOAuth") != null) {
			LOG.info("Invalidate ");
			request.getSession().setAttribute("accessToken", null);
			request.getSession().setAttribute("accessTokenSecret", null);
			request.getSession().setAttribute("connectionStatus",
					"not_authorized");
			
			request.getSession().removeAttribute("invalidateOAuth");
		}

		final HttpSession session = request.getSession();
		if (session.getAttribute("displayUserName") != null
				|| session.getAttribute("firstName") != null
				|| session.getAttribute("lastName") != null
				|| session.getAttribute("connectionStatus") != null) {
			if (session.getAttribute("isLinkingRequired") != null) {
				redirectTo = "redirect:/linking.htm";
			} else {
				redirectTo = "home";
			}
		} else {
			redirectTo = "redirect:/login.htm?isLoggedIn=false";
		}

		return redirectTo;
	}

	/*
	 * This method is called when the user clicks on 'Get All QuickBooks
	 * Customers' Link on Home Page.
	 */
	@RequestMapping(value = "/customers.htm", method = RequestMethod.GET)
	public String getCustomers(final HttpServletRequest request,
			final Model model) {
		LOG.info("HomeController -> getCustomers()");

		final HttpSession session = request.getSession();
		final List<QBCustomer> customerList = new ArrayList<QBCustomer>();

		final String accesstoken = (String) session.getAttribute("accessToken");
		final String accessstokensecret = (String) session
				.getAttribute("accessTokenSecret");
		final String realmID = (String) session.getAttribute("realmId");
		final String dataSource = (String) session.getAttribute("dataSource");

		final PlatformSessionContext context = getPlatformContext(accesstoken,
				accessstokensecret, realmID, dataSource);

		if (context == null) {
			LOG.error("Error: PlatformSessionContext is null.");
		}

		QBCustomerService customerService = null;
		try {
			// Create the customer service.
			customerService = QBServiceFactory.getService(context,
					QBCustomerService.class);
		} catch (QBInvalidContextException e) {
			LOG.error("(QBInvalidContextException thrown by getService: "
					+ e.getMessage());
		}

		try {
			// Using the service, retrieve all customers and display their
			// names.
			final List<QBCustomer> customers = customerService.findAll(context,
					1, 100);

			for (QBCustomer customer : customers) {
				final String customerName = customer.getName();
				LOG.info("customerName : " + customerName);
				customerList.add(customer);

			}
		} catch (Exception e) {
			LOG.error("Exception thrown by findAll / OAuth tokens are invalidated");
			session.setAttribute("connectionStatus", "not_authorized");
		}

		model.addAttribute("customerList", customerList);
		return "home";
	}

	/*
	 * This method a helper to get the Context from SDK.
	 */
	public PlatformSessionContext getPlatformContext(final String accessToken,
			final String accessTokenSecret, final String realmID,
			final String dataSource) {

		PlatformServiceType serviceType;
		if (dataSource.equalsIgnoreCase("QBO")) {
			serviceType = PlatformServiceType.QBO;
		} else {
			serviceType = PlatformServiceType.QBD;
		}

		final OAuthCredentials oauthcredentials = new OAuthCredentials(
				WebUtils.OAUTH_CONSUMER_KEY, WebUtils.OAUTH_CONSUMER_SECRET,
				accessToken, accessTokenSecret);

		final PlatformSessionContext context = new PlatformSessionContext(
				oauthcredentials, WebUtils.APP_TOKEN, serviceType, realmID);

		return context;
	}
}
