using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Mono.Data.Sqlite;

public class GameSettings : MonoBehaviour {

	public GameObject musicAudio, soundAudio;
	public Slider musicSlider, soundSlider;

	private SQLiteHelper sql;
	private string sqlStr;

	void Start () {

		sql = new SQLiteHelper();
		sql.OpenConnection ();

		/* 创建表settings */
		sql.CreateTable("audioVolume", 	// 创建储存游戏设置的settings表
			new string[]{"musicVolume", "soundVolume"},
			new string[]{"FLOAT","FLOAT"});
		
		sqlStr = "SELECT COUNT(*) FROM audioVolume";
		SqliteDataReader reader  = sql.ExecuteQuery (sqlStr);
		if (int.Parse (reader ["COUNT(*)"].ToString()) == 0 ){
			sql.InsertValues("audioVolume", new string[]{"'0.5'","'0.5'"});
		}

		/* 读取用户设置的值 */
		reader = sql.ReadFullTable ("audioVolume");
		reader.Read ();
		float musicVolume = reader.GetFloat(reader.GetOrdinal("musicVolume"));
		float soundVolume = reader.GetFloat(reader.GetOrdinal("soundVolume"));
			
		/* 根据值初始化声音大小和Slider的值 */
		musicAudio.transform.GetComponent<AudioSource> ().volume = musicVolume;
		soundAudio.transform.GetComponent<AudioSource> ().volume = soundVolume;
		musicSlider.value = musicVolume;
		soundSlider.value = soundVolume;

		sql.CloseConnection ();
	}


	/* 打印整张表 */
	private void printFullTable() {
		SqliteDataReader reader = sql.ReadFullTable ("audioVolume");
		while(reader.Read()) 
		{
			Debug.Log(reader.GetFloat(reader.GetOrdinal("musicVolume")) + ", " + reader.GetFloat(reader.GetOrdinal("soundVolume")));

		}
	}

}
