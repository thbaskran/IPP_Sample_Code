
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
					<a href="scrollInstitutions.htm?ScrollDirection=Up" title="AGGCAT">Aggregation And Categorization</a>
				</h1>
			</div>

			<div class="primaryNavigation">
				<ul>
					<li><a href="#">Institutions</a></li>
					<li><a href="#">Account Details</a></li>
					<li class="active"><a href="#">Account Transactions</a></li>
				</ul>
			</div>

		</div>

		<div id="body">
			<div class="filter">
				<h3>Filter By:</h3>
				<form method="post" action="getAccountTransactions.htm">
					<table cellpadding="0" cellspacing="0" border="0">
						<tr>
							<td>Account ID</td>
							<td>Start Date (MM-DD-YYYY):</td>
							<td colspan="2">End Date (MM-DD-YYYY):</td>
						</tr>
						<tr>
							<td><label><c:out
										value='${sessionScope["AccountId"]}' /></label></td>
							<td class="date">
								<input type="text" name="startMonth" maxlength="2"/>
								<input type="text" name="startDay" maxlength="2"/>
								<input type="text" name="startYear" maxlength="4"/></td>
							<td class="date">
								<input type="text" name="endMonth" maxlength="2"/>
								<input type="text" name="endDay" maxlength="2"/>
								<input type="text" name="endYear" maxlength="4"/></td>
							<td><input type="submit" value="Filter"	class="button l2 next" /></td>
						</tr>
					</table>
				</form>
			</div>
			<% if(request.getAttribute("error") != null) { %>
				<div class="error"><%= request.getAttribute("error") %></div>
			<% } %>
			<table cellpadding="0" cellspacing="0" border="0" width="100%">
				<thead>
					<tr>
						<th width="20%">Transaction ID</th>
						<th>Payee Name</th>
						<th width="10%">Type</th>
						<th width="10%">Date</th>
						<th width="10%">Amount</th>
						<th width="10%">Running Balance</th>
						<th width="5%">Currency Type</th>
					</tr>
				</thead>
				<c:forEach var="item" items='${TransactionList}'>
					<tr>
						<c:forEach var="column" items='${item}'>
							<td><c:out value="${column}"></c:out></td>
						</c:forEach>
					</tr>
				</c:forEach>
			</table>

			<div class="pagination">

				<div class="pageCount">
					<div class="pageCount">
						<span class="currentPageNumber"><c:out value='${sessionScope["TransactionPage"]}' /></span>
					</div>
				</div>

				<div class="navigation">
					<ul>
						<li><a href="scrollTransactionList.htm?ScrollDirection=Up"
							title="Previous">Previous</a></li>
						<li><a href="scrollTransactionList.htm?ScrollDirection=Down"
							title="Next">Next</a></li>
					</ul>
				</div>

			</div>

		</div>

	</div>

	<div id="footer">
		<div class="copyright">&copy; 2012 Aggregation And Categorization</div>
	</div>

</body>
</html>
