package com.intuit.aggcat.util;

import java.io.IOException;
import java.util.Properties;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.core.io.ClassPathResource;
import org.springframework.core.io.support.PropertiesLoaderUtils;

import com.intuit.ipp.aggcat.core.Context;
import com.intuit.ipp.aggcat.core.OAuthAuthorizer;
import com.intuit.ipp.aggcat.exception.AggCatException;
import com.intuit.ipp.aggcat.service.AggCatService;

/**
 * A utility class for this sample web app.This class reads the consumerkey,consumersecret,samlproviderId from the properties file and generates the
 * OAuthAuthorizer instance using these keys along with the userId.This OAuthAuthorizer instance is used for the creation of context instance which in
 * turn is used for the creation of AggCatService instance.
 */

public class WebUtil {

	/**
	 * Variable that holds the logger object.
	 */
	private static final Logger LOG = LoggerFactory.getLogger(WebUtil.class);

	/**
	 * Variable that holds the Properties object.
	 */
	private static Properties propConfig = null;

	/**
	 * Variable that holds the properties file name
	 */
	private static String PROP_FILE = "sample-app.properties";

	/**
	 * Variable that holds the consumer key
	 */
	private static String OAUTH_CONSUMER_KEY;

	/**
	 * Variable that holds the consumer secret.
	 */
	private static String OAUTH_CONSUMER_SECRET;

	/**
	 * Variable that holds the saml provider id
	 */
	private static String SAML_PROVIDER_ID;

	/**
	 * Static block that loads the properties file and caches the consumer key,consumer secret and saml provider Id
	 */
	static {
		try {
			propConfig = PropertiesLoaderUtils.loadProperties(new ClassPathResource(PROP_FILE));

			OAUTH_CONSUMER_KEY = propConfig.getProperty("aggcat_consumerKey");
			OAUTH_CONSUMER_SECRET = propConfig.getProperty("aggcat_consumerSecret");
			SAML_PROVIDER_ID = propConfig.getProperty("aggcat_samlProviderId");

		} catch (IOException e) {
			LOG.error("Properties File can not be loaded!!! " + e.getLocalizedMessage());
		}

	}

	/**
	 * This method will first create OAuthAuthorizer instance using consumerkey, consumersecret samlproviderid and user id. This OAuthAuthorizer
	 * object will be used to create the context object, which in turn is used to create the AggCatService instance.
	 * 
	 * @param userId
	 * @return AggCatService
	 * @throws AggCatException
	 */
	public AggCatService getAggCatService(String userId) throws AggCatException {
		AggCatService service = null;
		try {
			/**
			 * Creating the OAuthAuthorizer instance using OAUTH_CONSUMER_KEY,OAUTH_CONSUMER_SECRET,SAML_PROVIDER_ID and userId. In this step, the
			 * oAuth accesstoken,accesstokensecret are generated and the OAuthAuthorizer instance is ready to use for generating the context instance
			 */
			OAuthAuthorizer oauthAuthorizer = new OAuthAuthorizer(OAUTH_CONSUMER_KEY, OAUTH_CONSUMER_SECRET, SAML_PROVIDER_ID, userId);

			/**
			 * Using the OAuthAuthorizer instance to create the context.
			 */
			Context context = new Context(oauthAuthorizer);
			/**
			 * Using the context instance to create the AggCatService.
			 */
			service = new AggCatService(context);

		} catch (AggCatException ex) {
			LOG.error(ex.getMessage());
			throw new AggCatException("Exception while generating OAuth tokens. Please check whether the configured keys and cert files are valid.",
					ex);
		}
		return service;

	}
}
