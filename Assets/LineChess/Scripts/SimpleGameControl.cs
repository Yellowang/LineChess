using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/* 
 * 常用的一些对游戏控制的方法
 * 因为从很多小脚本里移过来
 * 导致方法名大小写不一致...
 */
public class SimpleGameControl : MonoBehaviour {

	/* 加载场景游戏 */
	public void loadGameSence() {

		string currentSence = SceneManager.GetActiveScene ().name;

		if(currentSence.Equals("StartInterface"))
			SceneManager.LoadScene("Loading");
		else if(currentSence.Equals("ChessGame"))
			SceneManager.LoadScene("ChessGame");
		
	}

	/* 加载主界面场景 */
	public void loadMainSence() {

		SceneManager.LoadScene("StartInterface");
	}

	/* 退出游戏 */
	public void QuitGame() {

		Application.Quit();
	}

	/* 暂停游戏 */
	public void PauseGame() {

		Time.timeScale = 0;
	}

	/* 恢复暂停 */
	public void ResumesPausedGame () {

		Time.timeScale = 1;
	}

	/* 控制音量大小 */
	public void changeVolumeValue (Slider s) {
		this.transform.GetComponent<AudioSource> ().volume = s.value;
	}

	/* 保存背景音乐音量设置到SQLite */
	public void saveMusicAudioVolume (Slider musicSlider) {

		SQLiteHelper sql = new SQLiteHelper();

		sql.OpenConnection ();
		sql.UpdateValues("audioVolume", new string[]{"musicVolume"}, new string[]{musicSlider.value.ToString()}, "", "", "");

		sql.CloseConnection ();	// 关闭连接

	}

	/* 保存音效音量设置到SQLite */
	public void saveSoundAudioVolume (Slider soundSlider) {

		SQLiteHelper sql = new SQLiteHelper();

		sql.OpenConnection ();
		sql.UpdateValues("audioVolume", new string[]{"soundVolume"}, new string[]{soundSlider.value.ToString()}, "", "", "");

		sql.CloseConnection ();	// 关闭连接

	}
}
