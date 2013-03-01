
<%@ page language="java" contentType="text/html; charset=ISO-8859-1"
	pageEncoding="ISO-8859-1"%>
<%@ page import="com.intuit.aggcat.util.WebUtil"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<link rel="stylesheet" type="text/css" href="css/style.css" />
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>OAuth Tokens Session Expired | AGGCAT</title>
</head>

<body>

	<div id="wrapper" class="login">

		<!-- <div class="logo">
        	<h1>
            	<a href="#" title="AGGCAT">Aggregation And Categorization</a>
            </h1>
        </div> -->

		<div class="logo">
			<h2>OAuth tokens session invalidated</h2>
		</div>

		<div class="container">
			<form method="get" action="signOut.htm">
				<div class="row" style="padding-bottom:55px;"></div>
				<div class="row">
					<label>The OAuth tokens for the logged in users session got
						expired. Please Sign In again to renew the tokens.</label>
				</div>
				<div class="row">
					<label>Please click on Sign Out button.</label>
				</div>
				<div class="endStripe">
					<input type="submit" class="button l1 next" value="Sign Out" />
				</div>
			</form>
		</div>

		<div class="footer">
			<div class="copyright">&copy; 2012 Aggregation And Categorization</div>
		</div>

	</div>

</body>
</html>
