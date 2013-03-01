package com.intuit.aggcat;

import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.List;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpSession;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;

import com.intuit.aggcat.util.TablePager;
import com.intuit.ipp.aggcat.data.Account;
import com.intuit.ipp.aggcat.data.AccountList;
import com.intuit.ipp.aggcat.data.ChallengeResponses;
import com.intuit.ipp.aggcat.data.Challenges.Challenge;
import com.intuit.ipp.aggcat.data.Challenges.Challenge.Choice;
import com.intuit.ipp.aggcat.data.Credential;
import com.intuit.ipp.aggcat.data.Credentials;
import com.intuit.ipp.aggcat.data.Institution;
import com.intuit.ipp.aggcat.data.InstitutionDetail;
import com.intuit.ipp.aggcat.data.InstitutionLogin;
import com.intuit.ipp.aggcat.data.Institutions;
import com.intuit.ipp.aggcat.data.Transaction;
import com.intuit.ipp.aggcat.data.TransactionList;
import com.intuit.ipp.aggcat.exception.AggCatException;
import com.intuit.ipp.aggcat.service.AggCatService;
import com.intuit.ipp.aggcat.service.ChallengeSession;
import com.intuit.ipp.aggcat.service.DiscoverAndAddAccountsResponse;
import com.intuit.ipp.aggcat.util.StringUtils;

/**
 * 
 * This class demonstrates the usage of various APIs in AggCat devkit. The following APIs are demonstrated:
 * getInstitutions,getInstitutionDetails,deleteCustomer,discoverAndAddAccounts,getAccountTransactions
 * 
 */
@Controller
public class AggCatApiController {

	/**
	 * Logger Object
	 */
	private static final Logger logger = LoggerFactory.getLogger(AggCatApiController.class);

	/**
	 * Date format used for TxnDate
	 */
	public static final String DATE_YYYYMMDD = "yyyy-MM-dd";

	/**
	 * This method will check whether the user is valid or not.
	 * 
	 * @param request
	 * @return isUserValid
	 */
	private boolean isUserValid(HttpServletRequest request) {
		if (null != request.getSession().getAttribute("User"))
			return true;
		else
			return false;
	}

	/**
	 * This method will fetch all the institutions
	 * 
	 * @param request
	 * @param model
	 * @return getInstitution
	 */
	@RequestMapping(value = "/getInstitution.htm", method = RequestMethod.GET)
	public String fetchInstitutions(HttpServletRequest request, Model model) {

		logger.info("Reached fetchInstitutions");
		if (!isUserValid(request)) {
			logger.info("User not authenticated");
			return "login";
		}

		try {
			HttpSession session = request.getSession();
			session.removeAttribute("InstitutionsTablePager");
			session.removeAttribute("InstitutionPage");
			/*
			 * Fetching the AggCatService instance from the session.
			 */
			AggCatService service = (AggCatService) session.getAttribute("AggCatService");
			logger.info("Created AggCatService");
			List<List<String>> institutionList = new ArrayList<List<String>>();
			List<String> item = new ArrayList<String>();
			/*
			 * Invoking the getInstitutions API to get the list of institutions.
			 */
			Institutions institutions = service.getInstitutions();
			logger.info("Fetched " + institutions.getInstitutions().size() + " institutions");
			/*
			 * Preparing a List<List<String>> from the Institutions object so as to render it in HTML table.
			 */
			for (Institution institution : institutions.getInstitutions()) {
				item = new ArrayList<String>();
				item.add(Long.toString(institution.getInstitutionId()));
				item.add(institution.getInstitutionName());
				item.add(institution.getHomeUrl());
				item.add(institution.getPhoneNumber());
				institutionList.add(item);
			}
			/*
			 * The tablePager manages the paging for an HTML table.
			 */
			TablePager tablePager = new TablePager(institutionList);
			session.setAttribute("InstitutionsTablePager", tablePager);
			model.addAttribute("institutions", tablePager.getFirstPage());
			session.setAttribute("InstitutionPage", tablePager.getPageNumber());

		} catch (AggCatException ex) {
			logger.error("Failed to fetch institutions, " + ex.getMessage());
			String errorCode = ex.getErrorCode();
			if (hasText(errorCode) && errorCode.equals("401")) {
				return "invalidateToken";
			}
			request.setAttribute("error", ex.getErrorMessage());
		}
		return "getInstitution";
	}

	/**
	 * This method will scroll the institution list
	 * 
	 * @param request
	 * @param model
	 * @return getInstitution
	 */
	@RequestMapping(value = "scrollInstitutions.htm", method = RequestMethod.GET)
	public String scrollInstitutionList(HttpServletRequest request, Model model) {
		String scrollDirection = null;
		if (!isUserValid(request)) {
			logger.info("User not authenticated");
			return "login";
		}
		/*
		 * Fetching the InstitutionsTablePager object from the Session
		 */
		TablePager tablePager = (TablePager) request.getSession().getAttribute("InstitutionsTablePager");
		/*
		 * Scrolling the HTML table depending on the scroll direction
		 */
		if (null != request.getParameter("ScrollDirection")) {
			scrollDirection = request.getParameter("ScrollDirection");
			if (scrollDirection.equals("Up"))
				model.addAttribute("institutions", tablePager.scrollUp());
			else
				model.addAttribute("institutions", tablePager.scrollDown());
			request.getSession().setAttribute("InstitutionPage", tablePager.getPageNumber());
		}
		return "getInstitution";
	}

	/**
	 * Fetches the institution details
	 * 
	 * @param request
	 * 
	 */
	@RequestMapping(value = "getInstitutionDetail.htm", method = RequestMethod.GET)
	public String fetchInstitutionDetails(HttpServletRequest request) {
		if (!isUserValid(request)) {
			logger.info("User not authenticated");
			return "login";
		}
		String userNameText = null;
		String userPasswordText = null;
		HttpSession session = request.getSession();
		/*
		 * Fetching the AggCatService from the session.
		 */
		AggCatService service = (AggCatService) session.getAttribute("AggCatService");
		try {
			logger.info("Created service for fetching Institutional detail");
			Long institutionId = Long.parseLong((String) request.getParameter("institutionId"));
			logger.info("Fetching Institutional detail for institutionId " + institutionId);
			session.setAttribute("InstitutionId", institutionId);
			/*
			 * Fetching the InstitutionDetail using the getInstitutionDetails API by passing institutionId
			 */
			InstitutionDetail institutionDetail = service.getInstitutionDetails(institutionId);
			logger.info("Fetched Institutional detail for institutionId " + institutionId);
			/*
			 * Fetching and caching the userNameText and userPasswordText for the current institution in the session.
			 * This is required to construct the InstitutionLogin object used  in discoverAndAddAccount API
			 */
			if (!(institutionDetail.getKeys().getKeies().get(0).isMask())) {
				userNameText = institutionDetail.getKeys().getKeies().get(0).getName();
				userPasswordText = institutionDetail.getKeys().getKeies().get(1).getName();
			} else {
				userNameText = institutionDetail.getKeys().getKeies().get(1).getName();
				userPasswordText = institutionDetail.getKeys().getKeies().get(0).getName();
			}
			logger.info("User Name Text for this institution is  " + userNameText);
			logger.info("User Password Text for this institution is  " + userPasswordText);
			session.setAttribute("UserNameText", userNameText);
			session.setAttribute("UserPasswordText", userPasswordText);

		} catch (AggCatException ex) {
			logger.error("Failed to fetch institution details " + ex.getMessage());
			//If the error code is 401 then the user is not authorized or the tokens might have expired
			String errorCode = ex.getErrorCode();
			if (hasText(errorCode) && errorCode.equals("401")) {
				return "invalidateToken";
			}
			request.setAttribute("error", ex.getErrorMessage());
		}
		return "discoverAndAddAccounts";
	}

	/**
	 * Search for a particular InstitutionId from list of institutions
	 * 
	 * @param request
	 * @param model
	 * 
	 */
	@RequestMapping(value = "searchInstitutionId.htm", method = RequestMethod.POST)
	public String searchInstitution(HttpServletRequest request, Model model) {

		if (!isUserValid(request)) {
			logger.info("User not authenticated");
			return "login";
		}
		String institutionId = request.getParameter("txtInstitutionId");
		/*
		 * Fetching the InstitutionsTablePager object from the session.
		 */
		TablePager tablePager = (TablePager) request.getSession().getAttribute("InstitutionsTablePager");
		/*
		 * Searching in the institutions table for the institutionId,(The first parameter is the column number, so passing 1 in this case)
		 */
		List<List<String>> institutionList = tablePager.search(1, institutionId);
		model.addAttribute("institutions", institutionList);
		/*
		 * institutionId is unique so only one record will be fetched
		 */
		if (institutionList.size() > 0)
			request.getSession().setAttribute("InstitutionPage", "Page 1/1");
		else
			request.getSession().setAttribute("InstitutionPage", "Page 0/0");
		return "getInstitution";
	}

	/**
	 * Scroll the transaction list
	 * 
	 * @param request
	 * @param model
	 */
	@RequestMapping(value = "scrollTransactionList.htm", method = RequestMethod.GET)
	public String scrollTransactionList(HttpServletRequest request, Model model) {
		String scrollDirection = null;
		if (!isUserValid(request)) {
			logger.info("User not authenticated");
			return "login";
		}
		/*
		 * Fetching the TransactionsTablePager instance from the session
		 */
		TablePager tablePager = (TablePager) request.getSession().getAttribute("TransactionsTablePager");
		/*
		 * Scrolling the TransactionList based on the ScrollDirection
		 */
		if (null != request.getParameter("ScrollDirection")) {
			scrollDirection = request.getParameter("ScrollDirection");
			if (scrollDirection.equals("Up"))
				model.addAttribute("TransactionList", tablePager.scrollUp());
			else
				model.addAttribute("TransactionList", tablePager.scrollDown());
			request.getSession().setAttribute("TransactionPage", tablePager.getPageNumber());
		}
		return "showAccountTransactions";
	}

	/**
	 * This method will do a discover and add, if additional challenges are required then a pop up will be displayed to answer the challenge questions.
	 * 
	 * @param request
	 * @param model
	 */

	@RequestMapping(value = "discoverAndAddAccountsSubmit.htm", method = RequestMethod.POST)
	public String discoverAndAddAccounts(HttpServletRequest request, Model model) {
		logger.info("Inside discover and add accounts");
		String redirectTo = "scrollInstitutions.htm?ScrollDirection=\"Up\"";
		if (!isUserValid(request)) {
			logger.info("User not authenticated");
			return "login";
		}

		String userName = null;
		String password = null;
		HttpSession session = request.getSession();

		/*
		 * Fetching the UserName and Password that user entered.
		 */
		if (null != request.getParameter("UserName")) {
			userName = (String) request.getParameter("UserName");
			password = (String) request.getParameter("Password");
		}
		/*
		 * Fetching the AggCatService instance from the session.
		 */
		AggCatService service = (AggCatService) session.getAttribute("AggCatService");
		try {
			List<String> challengeList = new ArrayList<String>();
			logger.info("Created service for discover and add accounts");
			/*
			 * Fetching the institutionId,userNameText and passwordText from the session.
			 */
			Long institutionId = (Long) session.getAttribute("InstitutionId");
			String userNameText = (String) session.getAttribute("UserNameText");
			String passwordText = (String) session.getAttribute("UserPasswordText");

			/*
			 * Creating the InstitutionLogin instance using the userName, password, userNameText, passwordText(userNameText,passwordText depends on
			 * the institution and its fetched using the getInstitutionDetails API)
			 */
			InstitutionLogin institutionLogin = createInstitutionLogin(userName, password, userNameText, passwordText);

			/*
			 * Temporarily deleting the customer. This is just to test the discoverAndAddAccounts API. In real time no need to delete the customer.
			 */
			service.deleteCustomer();

			/*
			 * Invoking the discoverAndAddAccounts API using the institutionId and institutionLogin instance. This API return
			 * DiscoverAndAddAccountsResponse which will either contain List<Challenge> or AccountList.If it contains List<Challenge> then user needs
			 * to answer the challenge questions.Then another call to the API discoverAndAddAccounts will be made with the challenge response obtained
			 * from the user along with the challenge session object returned in the current response.
			 */
			DiscoverAndAddAccountsResponse response = service.discoverAndAddAccounts(institutionId, institutionLogin);
			/*
			 * Checking whether the response contains challenges
			 */
			if (response.getChallenges() != null && response.getAccountList() == null) {
				List<Challenge> challenges = response.getChallenges().getChallenges();
				// Only Text is handled right now.
				/*
				 * Creating the challengeList object from the challenge object.
				 */
				for (Challenge challenge : challenges) {
					for (int counter = 0; counter < challenge.getTextsAndImagesAndChoices().size(); counter++) {

						if (!(challenge.getTextsAndImagesAndChoices().get(counter) instanceof byte[])
								&& (!(challenge.getTextsAndImagesAndChoices().get(counter) instanceof Choice))) {
							//Getting the challenge questions.
							challengeList.add(challenge.getTextsAndImagesAndChoices().get(counter).toString());
							logger.info("Question " + challenge.getTextsAndImagesAndChoices().get(counter).toString());
						}
					}
				}
				session.setAttribute("NumberOfChalenges", Integer.toString(challengeList.size()));
				/*
				 * Setting the list of challenges into the model so that it can be presented to the user and user can answer those challenges.
				 */
				model.addAttribute("Challenges", challengeList);
				/*
				 * Caching the ChallengeSession object which will be required in the next API call for discoverAndAddAccounts along with the challenge
				 * response obtained from the user
				 */
				session.setAttribute("ChallengeSession", response.getChallengeSession());
				/*
				 * Showing the challenges to user so that user can answer them.
				 */
				redirectTo = "challenges";
			} else {
				/*
				 * If the response does not contain challenges then additional authentication is not required and the account list is returned. Now
				 * selected fields from the account list is displayed to the user.
				 */
				List<List<String>> accountList = populateAccountList(response.getAccountList());
				model.addAttribute("AccountList", accountList);
				redirectTo = "accountDetails";
			}
		} catch (AggCatException ex) {
			logger.error("Failed to add accounts ", ex);
			String errorCode = ex.getErrorCode();
			/*
			 * If the error code is 401 then oAuth credentials are not valid or expired
			 */
			if (hasText(errorCode) && errorCode.equals("401")) {
				redirectTo = "invalidateToken";
			}
			request.setAttribute("error", ex.getErrorMessage());
			redirectTo = "discoverAndAddAccounts";
		}

		return redirectTo;
	}

	/**
	 * When the first call to the API discoverAndAddAccounts returns some challenges, user will have to answer them and then another call must be made
	 * to the API discoverAndAddAccounts with the challenge response (prepared from the answers obtained from user) and the challenge session object
	 * which is returned in the first API call.
	 * 
	 * @param request
	 * @param model
	 */
	@RequestMapping(value = "discoverAndAddAccountsMFA.htm", method = RequestMethod.POST)
	public String discoverAndAddWithMFA(HttpServletRequest request, Model model) {

		if (!isUserValid(request)) {
			logger.info("User not authenticated");
			return "login";
		}

		HttpSession session = request.getSession();
		ChallengeResponses challengeResponses = new ChallengeResponses();
		/*
		 * Fetching the AggCatService from the session.
		 */
		AggCatService service = (AggCatService) session.getAttribute("AggCatService");

		try {
			logger.info("Created service for discover and add accounts");
			List<String> challengeResponse = new ArrayList<String>();
			int count = Integer.parseInt((String) session.getAttribute("NumberOfChalenges"));
			/*
			 * Creating the challengeResponse using the answers obtained from the user.
			 */
			for (int counter = 0; counter < count; counter++) {
				if (request.getParameter(Integer.toString(counter)) != null)
					challengeResponse.add((String) request.getParameter(Integer.toString(counter)));
			}
			challengeResponses.setResponses(challengeResponse);

			/*
			 * Fetching the ChallengeSession instance from the session(which was returned during the first call to the API discoverAndAddAccounts)
			 */
			ChallengeSession challengeSession = (ChallengeSession) session.getAttribute("ChallengeSession");
			logger.info("Executing discover and add accounts for with challenge responses");
			/*
			 * Invoking the API discoverAndAddAccounts by providing challengeResponses and challengeSession This time the output will be an
			 * AccountList object (if successful) since the additional authentication is provided.
			 */
			AccountList accountListObj = service.discoverAndAddAccounts(challengeResponses, challengeSession);

			/*
			 * Preparing the accountList for rendering by using the selected fields from accountListObj.
			 */
			List<List<String>> accountList = populateAccountList(accountListObj);
			model.addAttribute("AccountList", accountList);
		} catch (AggCatException ex) {
			logger.error("Failed to add accounts ", ex);
			String errorCode = ex.getErrorCode();
			//If the error code is 401 then oAuth credentials are not valid or expired
			if (hasText(errorCode) && errorCode.equals("401")) {
				return "invalidateToken";
			}
			request.setAttribute("error", ex.getErrorMessage());
			return "discoverAndAddAccounts";
		}
		return "accountDetails";

	}

	/**
	 * Get Account transactions for an Account Id
	 * 
	 * @param request
	 * @param model
	 */
	@RequestMapping(value = "getAccountTransactions.htm", method = RequestMethod.POST)
	public String getAccountTransactions(HttpServletRequest request, Model model) {

		if (!isUserValid(request)) {
			logger.info("User not authenticated");
			return "login";
		}

		HttpSession session = request.getSession();
		String startTxnDate = null;
		String endTxnDate = null;
		String day;
		String month;
		String year;
		List<List<String>> txnDetailList = new ArrayList<List<String>>();
		List<String> item;
		AggCatService service = (AggCatService) session.getAttribute("AggCatService");
		try {
			session.removeAttribute("TransactionsTablePager");
			session.removeAttribute("TransactionPage");
			if (null != session.getAttribute("AccountId")) {

				/*
				 * Fetching the day ,month and year for start date
				 */
				day = (String) request.getParameter("startDay");
				month = (String) request.getParameter("startMonth");
				year = (String) request.getParameter("startYear");

				startTxnDate = year + "-" + month + "-" + day;

				/*
				 * Fetching the day ,month and year for end date
				 */
				day = (String) request.getParameter("endDay");
				month = (String) request.getParameter("endMonth");
				year = (String) request.getParameter("endYear");

				if (StringUtils.hasText(year) && StringUtils.hasText(month) && StringUtils.hasText(day)) {
					endTxnDate = year + "-" + month + "-" + day;
				}
				/*
				 * Fetching the accountId from the session.
				 */
				String accountId = (String) session.getAttribute("AccountId");

				/*
				 * Invoking the getAccountTransactions API by passing accountId,startTxnDate and endTxnDate The return value will be TransactionList
				 */
				TransactionList txnList = service.getAccountTransactions(Long.parseLong(accountId), startTxnDate, endTxnDate);

				/*
				 * Preparing the txnDetailList (which is a List<List<String>>) by using the selected fields from txnList.getLoanTransactions(), Just
				 * for demo purpose we are displaying only loan transactions.
				 */
				for (Transaction txn : txnList.getLoanTransactions()) {
					item = new ArrayList<String>();
					item.add(txn.getInstitutionTransactionId());
					if (txn.getPayeeName() != null)
						item.add(txn.getPayeeName());
					else
						item.add("");
					if (txn.getType() != null)
						item.add(txn.getType());
					else
						item.add("");
					if (txn.getAvailableDate() != null)
						item.add(txn.getAvailableDate().toString());
					else
						item.add("");
					if (txn.getAmount() != null)
						item.add(txn.getAmount().toPlainString());
					else
						item.add("");
					if (null != txn.getRunningBalanceAmount())
						item.add(txn.getRunningBalanceAmount().toPlainString());
					else
						item.add("");
					if (null != txn.getCurrencyType())
						item.add(txn.getCurrencyType().toString());
					else
						item.add("");
					txnDetailList.add(item);
				}
			}
			/*
			 * Creating the TablePager instance for txnDetailList
			 */
			TablePager tablePager = new TablePager(txnDetailList);
			/*
			 * Setting the first page for rendering in the model object.
			 */
			model.addAttribute("TransactionList", tablePager.getFirstPage());
			/*
			 * caching the TransactionsTablePager instance
			 */
			session.setAttribute("TransactionsTablePager", tablePager);
			session.setAttribute("TransactionPage", tablePager.getPageNumber());
		} catch (AggCatException ex) {
			logger.error("Failed to get account transactions.", ex);
			String errorCode = ex.getErrorCode();
			//If the error code is 401 then oAuth credentials are not valid or expired
			if (hasText(errorCode) && errorCode.equals("401")) {
				return "invalidateToken";
			}
			request.setAttribute("error", ex.getErrorMessage());
		}
		return "showAccountTransactions";
	}

	/**
	 * This method will redirect user to the account transactions page
	 * 
	 * @param request
	 */
	@RequestMapping(value = "getAccountTransactions.htm", method = RequestMethod.GET)
	public String getAccountTransactions(HttpServletRequest request) {

		if (!isUserValid(request)) {
			logger.info("User not authenticated");
			return "login";
		}
		request.getSession().removeAttribute("TransactionPage");
		request.getSession().setAttribute("AccountId", request.getParameter("accountId"));
		return "showAccountTransactions";
	}

	/**
	 * Creating an institutionlogin object
	 * 
	 * @param username
	 * @param password
	 * @param usernameText
	 * @param passwordText
	 * @return InstitutionLogin
	 */
	private InstitutionLogin createInstitutionLogin(String username, String password, String usernameText, String passwordText) {
		Credentials credentials = new Credentials();
		List<Credential> credentialList = new ArrayList<Credential>();

		Credential cred1 = new Credential();
		cred1.setName(usernameText);
		cred1.setValue(username);

		Credential cred2 = new Credential();
		cred2.setName(passwordText);
		cred2.setValue(password);

		credentialList.add(cred1);
		credentialList.add(cred2);

		credentials.setCredentials(credentialList);

		InstitutionLogin institutionLogin = new InstitutionLogin();
		institutionLogin.setCredentials(credentials);
		return institutionLogin;
	}

	/**
	 * Method to populate account list to UI object
	 * 
	 * @param accountListObj
	 * @return List<List<String>>
	 */
	private List<List<String>> populateAccountList(AccountList accountListObj) {
		List<List<String>> accountList = new ArrayList<List<String>>();
		List<String> item;
		SimpleDateFormat formatter = new SimpleDateFormat(DATE_YYYYMMDD);
		for (Account account : accountListObj.getBankingAccountsAndCreditAccountsAndLoanAccounts()) {
			item = new ArrayList<String>();

			item.add(Long.toString(account.getAccountId()));
			item.add(Long.toString(account.getInstitutionId()));
			if (null != account.getAggrSuccessDate())
				item.add(formatter.format(account.getAggrSuccessDate().getTime()));
			else
				item.add("");
			if (null != account.getBalanceAmount())
				item.add(account.getBalanceAmount().toString());
			else
				item.add("");
			if (null != account.getDescription())
				item.add(account.getDescription());
			else
				item.add("");
			if (null != account.getCurrencyCode())
				item.add(account.getCurrencyCode().toString());
			else
				item.add("");
			accountList.add(item);
		}

		return accountList;
	}

	/**
	 * Method to check whether the given string is null or empty
	 * 
	 * @param text
	 * @return boolean
	 */
	public boolean hasText(String text) {
		if (text != null && !"".equals(text.trim())) {
			return true;
		}
		return false;
	}
}
