<%@ taglib uri="http://java.sun.com/jsp/jstl/core" prefix="c"%>
<%@ page session="true"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<link rel="stylesheet" type="text/css" href="css/style.css" />
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>Get Institution | Aggregation And Categorization</title>
<script>
	function cancelAdd() {
		document.location.href = "scrollInstitutions.htm?ScrollDirection=Up";
	}
</script>
</head>

<body>

	<div id="wrapper">

		<div id="header">

			<div class="topNavigation">

				<ul>
					<li>Welcome, <span class="username">Username</span></li>
					<li><a href="#" title="Sign Out">Sign Out</a></li>
				</ul>

			</div>

			<div class="logo">
				<h1>
					<a href="#" title="AGGCAT">Aggregation And Categorization</a>
				</h1>
			</div>

			<div class="primaryNavigation">
				<ul>
					<li class="active"><a href="#" title="Get Institution">Get Institution</a></li>
					<li><a href="#" title="Account Details">Account Details</a></li>
					<li><a href="#" title="Get Account Trtansactions">Get Account Transactions</a></li>
				</ul>
			</div>

		</div>

		<div id="body">

			<h2>Get Institution</h2>

			<table cellpadding="0" cellspacing="0" border="0" width="100%">
				<thead>
					<tr>
						<th width="10%">Intitution ID</th>
						<th width="25%">Intitution Name</th>
						<th>Home Link</th>
						<th width="10%">Contact Number</th>
						<th width="10%">Add</th>
					</tr>
				</thead>
				<c:forEach var="item" items='${institutions}'>
					<tr>
						<%int count = 0; %>
						<!--<td style="width: 20px" align="center"></td>
                 <td style="width: 50px" align="left" ></td> -->
						<c:forEach var="column" items='${item}'>

							<% 	count = count + 1; %>
							<td><c:out value="${column}"></c:out></td>
							<%	if(count == 1) { %>
									<c:set var="institutionIdTemp" value="${column}" scope="request"/>		
							<%	} if(count == 4) { %>
									<td><a href="getInstitutionDetail.htm?institutionId=<%=request.getAttribute("institutionIdTemp") %>" title="Click to discover and add accounts"> Discover And Add</a></td>>
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
	<div class="popupBg"></div>
	<div class="popup">
		<div class="container">
			<h2>Secret Questions</h2>

			<div class="filter quest">
			<%int counter=0; %>
				<form method="post" action="discoverAndAddAccountsMFA.htm">
					<c:forEach var="questions" items='${Challenges}'>
						
						<div class="row">
							<label><c:out value="${questions}"></c:out></label>
						</div>
						<div class="row">
							<label>Answer :</label>  <input type="text" name=<%=counter++ %> />
						</div>
					</c:forEach>
					<div class="endStripe">
						<input type="button" class="button l1 next" value="Cancel" onclick="javascript:cancelAdd();"/>
						<input type="submit" class="button l1 next" value="Submit" /> 
					</div>
				</form>
			</div>
		</div>
	</div>

</body>
</html>
