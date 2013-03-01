package com.intuit.aggcat.util;

import java.util.ArrayList;
import java.util.List;

/**
 * This class will help in paging an HTML table. Accepts a List of List as the datasource for HTML table.The inner list will represent a
 * row in the table
 * 
 */
public class TablePager {

	/**
	 * Variable that holds the datasource for the table
	 */
	private List<List<String>> dataSource;

	/**
	 * This variable holds the page size
	 */
	private int pageSize = 10;

	/**
	 * This variable holds the page size
	 */
	private int currentIndex = 1;

	/**
	 * Constructor CTablePager
	 * 
	 * @param dataSource
	 */
	public TablePager(List<List<String>> dataSource) {
		this.dataSource = dataSource;
	}

	/**
	 * Returns the datasource
	 * 
	 * @return dataSource
	 */
	public List<List<String>> getDataSource() {
		return dataSource;
	}

	/**
	 * sets the datasource
	 * 
	 * @param dataSource
	 */
	public void setDataSource(List<List<String>> dataSource) {
		this.dataSource = dataSource;
	}

	/**
	 * Returns the page size
	 * 
	 * @return pageSize
	 */
	public int getPageSize() {
		return pageSize;
	}

	/**
	 * Sets the page size
	 * 
	 * @param pageSize
	 */
	public void setPageSize(int pageSize) {
		this.pageSize = pageSize;
	}

	/**
	 * This is for scrolling up the table
	 * 
	 * @return currentPageDataSource
	 */
	public List<List<String>> scrollUp() {

		if (currentIndex > pageSize)
			currentIndex -= pageSize;

		return getDataForRange();

	}

	/**
	 * Method which returns the first page
	 * 
	 * @return List<List<String>>
	 */
	public List<List<String>> getFirstPage() {

		currentIndex = 1;

		return getDataForRange();
	}

	/**
	 * This is for scrolling down the table
	 * 
	 * @return currentPageDataSource
	 */
	public List<List<String>> scrollDown() {

		if (currentIndex <= dataSource.size() -(pageSize+1))
			currentIndex += pageSize;

		return getDataForRange();
	}

	/**
	 * Returns the page number
	 * 
	 * @return pageNumberString
	 */
	public String getPageNumber() {
		try {
			if (dataSource.size() > 0)
				return "Page " + (((currentIndex) / pageSize) + 1) + "/" + dataSource.size() / pageSize;
			else
				return "Page 0/0";
		} catch (Exception ex) {
			return "Page 0/0";
		}
	}

	/**
	 * Gets the currentPageDataSource using the page size and current page index
	 * 
	 * @return currentPageDataSource
	 */
	private List<List<String>> getDataForRange() {

		List<List<String>> currentPageDataSource = new ArrayList<List<String>>();
		try {
			int count = 0;
			for (int counter = currentIndex; counter < dataSource.size(); counter++) {
				count++;
				if (count > pageSize)
					break;
				else
					currentPageDataSource.add(dataSource.get(counter));
			}
			return currentPageDataSource;
		} catch (Exception ex) {
			return currentPageDataSource;
		}
	}

	public List<List<String>> search(int columnNumber, String item) {

		List<List<String>> currentPageDataSource = new ArrayList<List<String>>();
		for (int counter = 0; counter < dataSource.size(); counter++) {
			if (dataSource.get(counter).get(columnNumber - 1).equals(item)) {
				currentPageDataSource.add(dataSource.get(counter));
				break;
			}
		}
		return currentPageDataSource;
	}

}
