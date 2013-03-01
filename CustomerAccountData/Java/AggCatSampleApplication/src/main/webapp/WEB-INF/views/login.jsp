
<%@ page language="java" contentType="text/html; charset=ISO-8859-1"
	pageEncoding="ISO-8859-1"%>
<%@ page import="com.intuit.aggcat.util.WebUtil"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<link rel="stylesheet" type="text/css" href="css/style.css" />
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>Login | AGGCAT</title>
</head>

<body>

<div id="wrapper" class="login">
    	
        <!-- <div class="logo">
        	<h1>
            	<a href="#" title="AGGCAT">Aggregation And Categorization</a>
            </h1>
        </div> -->
        
        <div class="logo"><h2>Welcome to Aggregation And Categorization</h2></div>
        <p>Please Sign In to continue</p>
        
        <div class="container">
        	<form method="post" action="login.htm">
            	<div class="row">
                	<label>UserName</label>
                    <input type="text" name="loginUserName"/>
                </div>
            	<div class="row">
                	<label>Password</label>
                    <input type="password" name="loginPassCode"/>
                    <!-- <a href="#" title="Forgot Password">Forgot Password</a> -->
                </div>
                <div class="endStripe">
                	<input type="submit" class="button l1 next" value="Sign In" />
                </div>
            <% if(request.getParameter("isValidUser") != null && request.getParameter("isValidUser").equals("false")){ 
			%>
				<div class="error">Invalid Credentials</div>
			<% } else if(request.getParameter("error") != null) { %>
				<div class="error"><%= request.getParameter("error") %></div>
			<% } %>
            </form>
        </div>
        
        <div class="footer">
            
            <div class="copyright">&copy; 2012 Aggregation And Categorization</div>
        
        </div>
    
</div>

</body>
</html>
