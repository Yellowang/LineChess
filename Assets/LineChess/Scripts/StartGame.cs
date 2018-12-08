using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {
		
	}

	// 异步加载游戏场景
	public void loadGameSence() {

		SceneManager.LoadScene ("Loading");
	}
}
