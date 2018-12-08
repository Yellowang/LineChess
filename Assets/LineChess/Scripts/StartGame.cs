using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class StartGame : MonoBehaviour {

	private GameObject box_black, box_white, electricity_H, electricity_V, piece_AI, piece_player, pillar;
	private int chessSize = ChessJudge.getSingleInstance().getChessSize();

	void Start(){

		/* 加载资源 */
		box_black = (GameObject)Resources.Load ("Box_Black");
		box_white = (GameObject)Resources.Load ("Box_White");
		electricity_H = (GameObject)Resources.Load ("Electricity_Horizontal");
		electricity_V = (GameObject)Resources.Load ("Electricity_Vertical");
		piece_AI = (GameObject)Resources.Load ("Piece_AI");
		piece_player = (GameObject)Resources.Load ("Piece_Player");
		pillar = Resources.Load ("Pillar_0") as GameObject;
	}

	/* 实例化各种Prefab并置于对应的位置和层级关系 */
	public void createEnvironment() {
		
		createChessBOard ();
		createPillars ();
		createFences ();
		createPlayerAndAI ();
	}
		
	/* 生成棋盘，添加棋格 */
	private void createChessBOard() {

		int sum = 0;
		while (sum < 2 * chessSize) {
			for (int i = 0; i <= sum && i < chessSize; i++) {
				for (int j = 0; j <= sum && j < chessSize; j++) {
					if (i + j == sum) {
						GameObject onePiece = null;
						if (sum % 2 == 0) {
							onePiece = Instantiate (box_white) as GameObject;	// 实例化白色格子
						} else {
							onePiece = Instantiate (box_black) as GameObject;	// 实例化黑色格子
						}
						onePiece.transform.parent = GameObject.FindGameObjectWithTag ("ChessBoard").transform;					// 设置为子物体
						onePiece.transform.localPosition = new Vector3 (i * 2 - (chessSize - 1), 0f, j * 2 - (chessSize - 1));	// 设置位置
						onePiece.transform.name = "Box(" + (i * 2 + 1) + ", " + (j * 2 + 1) + ")";		// 设置名字
					}
				}
			}
			sum++;

			// Thread.Sleep(100);		// 形成逐渐出现的效果  无效
		}

	}

	/* 添加柱子 */
	private void createPillars() {

		for (int i = 0; i <= chessSize; i++) {
			for (int j = 0; j <= chessSize; j++) {
				GameObject onePillar = Instantiate(pillar) as GameObject;
				onePillar.transform.parent = GameObject.FindGameObjectWithTag ("Pillars").transform;		// 设置为子物体
				onePillar.transform.localPosition = new Vector3 (i * 2 - chessSize, 0f, j * 2 - chessSize);	// 设置位置
				onePillar.transform.name = "Box(" + (i * 2) + ", " + (j * 2) + ")";	// 设置名字
			}
		}
	}
		
	/* 生成围在棋盘周围的栅栏 */
	private void createFences() {

		/* 上 */
		for (int i = 0; i < chessSize; i++) {
			GameObject oneFence = Instantiate(electricity_H) as GameObject;
			oneFence.transform.parent = GameObject.FindGameObjectWithTag ("Fences").transform;	// 设置为子物体
			oneFence.transform.localPosition = new Vector3 (i * 2 - chessSize, 0f, chessSize);	// 设置位置
			oneFence.transform.name = "Fence(" + (1 + i * 2) + ", " + 2 * chessSize + ")";	// 设置名字
		}

		/* 下 */
		for (int i = 0; i < chessSize; i++) {
			GameObject oneFence = Instantiate(electricity_H) as GameObject;
			oneFence.transform.parent = GameObject.FindGameObjectWithTag ("Fences").transform;	// 设置为子物体
			oneFence.transform.localPosition = new Vector3 (i * 2 - chessSize, 0f, -chessSize);	// 设置位置
			oneFence.transform.name = "Fence(" + (1 + i * 2) + ", 0)";	// 设置名字
		}

		/* 左 */
		for (int i = 0; i < chessSize; i++) {
			GameObject oneFence = Instantiate(electricity_V) as GameObject;
			oneFence.transform.parent = GameObject.FindGameObjectWithTag ("Fences").transform;	// 设置为子物体
			oneFence.transform.localPosition = new Vector3 (-chessSize, 0f, i * 2 - chessSize);	// 设置位置
			oneFence.transform.name = "Fence(0, " + (1 + i * 2) + ")";	// 设置名字
		}

		/* 右 */
		for (int i = 0; i < chessSize; i++) {
			GameObject oneFence = Instantiate(electricity_V) as GameObject;
			oneFence.transform.parent = GameObject.FindGameObjectWithTag ("Fences").transform;	// 设置为子物体
			oneFence.transform.localPosition = new Vector3 (chessSize, 0f, i * 2 - chessSize);	// 设置位置
			oneFence.transform.name = "Fence(" + 2 * chessSize + ", " + (1 + i * 2) + ")";	// 设置名字
		}
	}

	/* 生成玩家棋子和AI棋子模型 */
	private void createPlayerAndAI() {

		/* 创建棋子游戏对象 */
		GameObject player = Instantiate (piece_player) as GameObject;
		GameObject AI = Instantiate (piece_AI) as GameObject;

		/* 设置为子物体 */
		player.transform.parent = GameObject.FindGameObjectWithTag ("Pieces").transform;
		AI.transform.parent = GameObject.FindGameObjectWithTag ("Pieces").transform;

		/* 设置位置 */
		player.transform.localPosition = new Vector3(-(chessSize - 1f), 0f, -(chessSize - 1f));
		AI.transform.localPosition = new Vector3((chessSize - 1f), 0f, (chessSize - 1f));
	}

}
