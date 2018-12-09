using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System;

public class SQLiteHelper {

	private SqliteConnection dbConnection;	// 数据库连接
	private SqliteCommand dbCommand;		// SQL命令
	private SqliteDataReader dataReader;	// 读取的数据
	private static readonly object obj = new object();

	/* 执行SQL语句 */
	public SqliteDataReader ExecuteQuery(string queryString) {

		dbCommand = dbConnection.CreateCommand();
		dbCommand.CommandText = queryString;
		dataReader = dbCommand.ExecuteReader();

		return dataReader;
	}

	/* 关闭数据库连接 */
	public void OpenConnection() {
		
		dbConnection = new SqliteConnection("data source=lineChess.db");				// 数据库连接
		dbConnection.Open ();
	}

	/* 关闭数据库连接 */
	public void CloseConnection() {
		
		/* 销毁Command */
		if(dbCommand != null){
			dbCommand.Cancel();
		}
		dbCommand = null;

		/* 销毁Reader */
		if(dataReader != null){
			dataReader.Close();
		}
		dataReader = null;

		/* 销毁Connection */
		if(dbConnection != null){
			dbConnection.Close();
		}
		dbConnection = null;
	}

	/* 插入数据 */
	public void InsertValues(string tableName,string[] values) {
		
		int fieldCount = ReadFullTable(tableName).FieldCount;			// 获取数据表中字段数目

		if(values.Length!=fieldCount){		// 当插入的数据长度不等于字段数目时引发异常
			throw new SqliteException("values.Length!=fieldCount");
		}

		string queryString = "INSERT INTO " + tableName + " VALUES (" + values[0];
		for(int i = 1; i < values.Length; i++)
		{
			queryString += ", " + values[i];
		}
		queryString += " )";

		ExecuteQuery(queryString);
	}

	/* 更新数据 */
	public void UpdateValues(string tableName,string[] colNames,string[] colValues,string key,string operation,string value) {
		
		if(colNames.Length!=colValues.Length) {		// 当字段名称和字段数值不对应时引发异常
			throw new SqliteException("colNames.Length!=colValues.Length");
		}

		string queryString = "UPDATE " + tableName + " SET " + colNames[0] + "=" + colValues[0];
		for(int i = 1; i < colValues.Length; i++) 
		{
			queryString += ", " + colNames[i] + "=" + colValues[i];
		}
		if(key != "" && operation != "" && value != "")
			queryString += " WHERE " + key + operation + value;

		ExecuteQuery(queryString);
	}

	/* 读取整张表 */
	public SqliteDataReader ReadFullTable(string tableName) {
		
		string queryString = "SELECT * FROM " + tableName;

		return ExecuteQuery (queryString);
	}

	/* 创建表 */
	public SqliteDataReader CreateTable(string tableName,string[] colNames,string[] colTypes) {

		string queryString = "CREATE TABLE IF NOT EXISTS " + tableName + "( " + colNames [0] + " " + colTypes [0];
		for (int i = 1; i < colNames.Length; i++) 
		{
			queryString += ", " + colNames[i] + " " + colTypes[i];
		}
		queryString +=  "  ) ";

		return ExecuteQuery(queryString);
	}

}
