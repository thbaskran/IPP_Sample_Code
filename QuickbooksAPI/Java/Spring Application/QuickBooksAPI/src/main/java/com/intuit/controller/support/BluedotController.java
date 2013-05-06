package com.intuit.controller.support;

import java.io.IOException;
import java.io.PrintWriter;
import java.util.List;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;

import org.apache.log4j.Logger;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;

import com.intuit.platform.client.PlatformClient;
import com.intuit.platform.client.PlatformSessionContext;
import com.intuit.utils.WebUtils;

/*
 * This class is a controller for the application when user completes the OAuth flow in the application.  
 */
@Controller
@RequestMapping("/bluedot.htm")
public class BluedotController {

	public static final Logger LOG = Logger.getLogger(BluedotController.class);

	/*
	 * This method is called by default when the user click on the 'Blue Dot'
	 * link on the application in the top right corner.
	 */
	@RequestMapping(method = RequestMethod.GET)
	public void getBluedotMenu(final HttpServletRequest request,
			final HttpServletResponse response) throws IOException {

		// Retrieve the credentials from the session. In a production app these
		// would be retrieved from a persistent store.
		LOG.info("### BlueDotMenuServlet ###");
		final HttpSession session = request.getSession();
		final WebUtils webutils = new WebUtils();
		final String accesstoken = (String) session.getAttribute("accessToken");
		final String accessstokensecret = (String) session
				.getAttribute("accessTokenSecret");
		final String realmID = (String) session.getAttribute("realmId");
		final String dataSource = (String) session.getAttribute("dataSource");

		response.setContentType("text/plain");
		PlatformSessionContext context = null;
		final PrintWriter out = response.getWriter();
		try {
			if (accesstoken != null && accessstokensecret != null
					&& realmID != null) {
				context = webutils.getPlatformContext(accesstoken,
						accessstokensecret, realmID, dataSource);
				final PlatformClient pClient = new PlatformClient();
				final StringBuffer stringBuffer = new StringBuffer();
				final List<String> menuList = pClient.getAppMenu(context);
				if (menuList != null) {
					for (String mItem : menuList) {
						stringBuffer.append(mItem);
						out.println(mItem);
					}
				}
			}
			else {
				response.sendError(403);
			}
		} catch (Exception e) {
			LOG.error("Exception in BlueDotMenuServlet " + e.getMessage());
		}

		LOG.info("#### BlueDotMenuServlet leaving now....####");
	}
}
