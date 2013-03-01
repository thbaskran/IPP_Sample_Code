<%@ taglib uri="http://java.sun.com/jsp/jstl/core" prefix="c"%>
<%@ page session="true"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<link rel="stylesheet" type="text/css" href="css/style.css" />
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>Get Institution | Aggregation And Categorization</title>
</head>

<body>

	<div id="wrapper">

		<div id="header">

			<div class="topNavigation">

				<ul>
					<li>Welcome, <span class="username"><c:out value='${sessionScope["emailMapped"]}' /></span></li>
					<li><a href="signOut.htm" title="Sign Out">Sign Out</a></li>
				</ul>

			</div>

			<div class="logo">
				<h1>
					<a href="scrollInstitutions.htm?ScrollDirection=Up" title="Aggregation And Categorization"> Aggregation And Categorization</a>
				</h1>
			</div>

			<div class="primaryNavigation">
				<ul>
					<li><a href="#">Institutions</a></li>
					<li class="active"><a href="#">Account Details</a></li>
					<li><a href="#">Account Transactions</a></li>
				</ul>
			</div>

		</div>

		<% if(request.getAttribute("error") != null) { %>
			<div class="error"><%= request.getAttribute("error") %></div>
		<% } %>
		
		<div id="body">
			<table cellpadding="0" cellspacing="0" border="0" width="100%">
				<thead>
					<tr>
						<th width="10%">Account Id</th>
						<th width="10%">Institution Id</th>
						<th width="10%">Date</th>
						<th width="20%">Balance Amount</th>
						<th width="20%">Description</th>
						<th width="10%">Currency</th>
						<th>Transaction History</th>
					</tr>
				</thead>
				<c:forEach var="item" items='${AccountList}'>
					<tr>
						<%int count = 0; %>
						<!--<td style="width: 20px" align="center"></td>
                 		<td style="width: 50px" align="left" ></td> -->
						<c:forEach var="column" items='${item}'>

							<% 	count = count + 1; %>
							<td><c:out value="${column}"></c:out></td>
							<%	if(count == 1) { %>
							<c:set var="accountId" value="${column}" scope="request" />
							<%	} if(count == 6) { %>
							<td><a
								href="getAccountTransactions.htm?accountId=<%=request.getAttribute("accountId") %>"
								title="Click to get transaction details"> Fetch Transaction
									Details</a></td>
							<%	} %>
						</c:forEach>
					</tr>
				</c:forEach>
			</table>

		</div>
	</div>

	<div id="footer">
		<div class="copyright">&copy; 2012 Aggregation And Categorization</div>
	</div>

</body>
</html>
