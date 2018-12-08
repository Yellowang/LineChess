using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMain : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {
		
	}

	public void loadMainSence() {

		SceneManager.LoadScene("StartInterface");
	}
}
