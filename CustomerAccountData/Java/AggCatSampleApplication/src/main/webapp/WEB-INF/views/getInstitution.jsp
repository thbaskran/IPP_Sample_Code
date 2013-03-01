
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
					<li class="active"><a href="#">Institutions</a></li>
					<li><a href="#">Account Details</a></li>
					<li><a href="#">Account Transactions</a></li>
				</ul>
			</div>

		</div>

		<div id="body">
			<div class="filter">
				<form method="post" action="searchInstitutionId.htm">
					<table cellpadding="0" cellspacing="0" border="0">
						<tr>
							<td>Institution Id: <input type="text" name="txtInstitutionId"/></td>
							<td><input type="submit" class="button l1 next" value="Search" /></td>
							<td width="50%" align="right"><a href="scrollInstitutions.htm?ScrollDirection=Up" title="clear search">Clear Search</a></td>
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
						<th width="10%">Intitution ID</th>
						<th width="25%">Intitution Name</th>
						<th>Home Link</th>
						<th width="10%">Contact Number</th>
						<th width="20%">Add</th>
					</tr>
				</thead>
				<c:forEach var="item" items='${institutions}'>
					<tr>
						<%
							int count = 0;
						%>
						<!--<td style="width: 20px" align="center"></td>
                 <td style="width: 50px" align="left" ></td> -->
						<c:forEach var="column" items='${item}'>

							<%
								count = count + 1;
							%>
							<td><c:out value="${column}"></c:out></td>
							<%
								if (count == 1) {
							%>
							<c:set var="institutionIdTemp" value="${column}" scope="request" />
							<%
								}
										if (count == 4) {
							%>
							<td><a
								href="getInstitutionDetail.htm?institutionId=<%=request.getAttribute("institutionIdTemp")%>"
								title="Click to discover and add accounts"> Discover And Add</a></td>
							<%
								}
							%>
						</c:forEach>
					</tr>
				</c:forEach>

			</table>
			<div class="pagination">
				<div class="pageCount">
					<div class="pageCount">
						<span class="currentPageNumber"><c:out
								value='${sessionScope["InstitutionPage"]}' /></span>
					</div>
				</div>

				<div class="navigation">
					<ul>
						<li><a href="scrollInstitutions.htm?ScrollDirection=Up"
							title="Previous">Previous</a></li>
						<li><a href="scrollInstitutions.htm?ScrollDirection=Down"
							title="Next">Next</a></li>
					</ul>
				</div>

			</div>
		</div>
	</div>
	<div id="footer">
		<div class="copyright">&copy; 2012 Aggregation And
			Categorization</div>
	</div>

</body>
</html>
