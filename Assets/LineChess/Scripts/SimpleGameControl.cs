using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
}
