using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowGameResult : MonoBehaviour {

	public GameObject pauseBtn, winnerPanel, winnerText;
	private ChessJudge cj;
	private string name;
	private bool hasShowed;	// 是否已经显示过结果，减少资源负担

	void Start () {
		cj = ChessJudge.getSingleInstance ();
		name = "Winner";
		hasShowed = false;
	}
	
	void FixedUpdate () {

		if (cj.judgeGameResult ().Equals ("胜负未分") || hasShowed)
			return;
		else {		// 显示游戏结果
			switch (cj.judgeGameResult ()) {
			case "胜利":
				name = "Player 1";
				break;
			case "失败":
				name = "Player 2";
				break;
			case "平局":
				name = "Nobody";
				break;
			}

			winnerText.GetComponent<Text> ().text = name;
			pauseBtn.SetActive (false);
			winnerPanel.SetActive (true);

			hasShowed = true;
		}
	}
}
