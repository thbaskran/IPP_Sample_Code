package com.intuit.aggcat;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpSession;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;

import com.intuit.aggcat.util.WebUtil;
import com.intuit.ipp.aggcat.exception.AggCatException;
import com.intuit.ipp.aggcat.service.AggCatService;

/**
 * This class is a controller for the application Log In / Log out activities.
 */
@Controller
public class LoginController {

	public static final Logger LOG = LoggerFactory.getLogger(LoginController.class);

	/*
	 * This method is called when the user enters the application path
	 */
	@RequestMapping(value = "login.htm", method = RequestMethod.GET)
	public String showLoginPage() {
		LOG.info("LoginController -> showLoginPage()");

		return "login";
	}

	/**
	 * This method is called when the user submits the Login form. This method will create the AggCat service and cache it in the session
	 */
	@RequestMapping(value = "login.htm", method = RequestMethod.POST)
	public String authenticateUser(final HttpServletRequest request) {
		LOG.info("LoginController -> authenticateUser()");

		String redirectTo;

		final String userName = request.getParameter("loginUserName");
		final String passCode = request.getParameter("loginPassCode");

		final HttpSession session = request.getSession();

	//	if ("user".equals(userName) && "password".equals(passCode)) {
			if ("password".equals(passCode)) {
			session.setAttribute("displayUserName", "User");
			session.setAttribute("emailMapped", "user@intuit.com");
			session.setAttribute("firstName", null);
			session.setAttribute("lastName", null);
			session.setAttribute("User", userName);
			
			try {
				WebUtil webUtil = new WebUtil();
				/*
				 * Creating the AggCatService instance using the username and caching it in the session
				 */
				AggCatService service = webUtil.getAggCatService(userName);
				session.setAttribute("AggCatService", service);
				redirectTo = "redirect:/getInstitution.htm";
			} catch (AggCatException e) {
				LOG.error("", e);
				redirectTo = "redirect:/login.htm?error=" + e.getErrorMessage();
			}
		} else {
			redirectTo = "redirect:/login.htm?isValidUser=false";
		}

		return redirectTo;
	}

	/**
	 * Flushing out the attributes from session when user sign out.
	 * 
	 * @param request
	 * @return
	 */
	@RequestMapping(value = "signOut.htm", method = RequestMethod.GET)
	public String signOut(HttpServletRequest request) {

		HttpSession session = request.getSession();
		session.removeAttribute("displayUserName");
		session.removeAttribute("emailMapped");
		session.removeAttribute("firstName");
		session.removeAttribute("lastName");
		session.removeAttribute("User");
		session.removeAttribute("TransactionList");
		session.removeAttribute("TransactionsTablePager");
		session.removeAttribute("TransactionPage");
		session.removeAttribute("NumberOfChalenges");
		session.removeAttribute("InstitutionId");
		session.removeAttribute("UserNameText");
		session.removeAttribute("UserPasswordText");
		session.removeAttribute("InstitutionPage");
		session.removeAttribute("AggCatService");

		return "login";
	}
}
