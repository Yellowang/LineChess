﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class PieceControl : MonoBehaviour {

	public float moveSpeed = 5.0f;		// 移动速度
	public float rotateSpeed = 5.0f;	// 转身速率

	private ChessJudge cj;
	private int chessSize;
	private int[,] chess;
	private StartGame game;

	private float h, v;
	private int x, z;	// 对应矩阵中的下标
	private bool playersMove = true;	// 判断player能否移动和落子，对方回合、转身中时不能
	private string direction;			// 面朝的方向

	private GameObject electricity_H;
	private GameObject electricity_V;

	/* 测试 */
	void OnGUI() {
		GUI.Label(new Rect(200,50,200,500), 
			"(h, v):\t(" + h + ", " + v + ")\n" + 
			"能否移动:\t" + playersMove + "\n" +
		//	"能否落子:\t" + JudgeMoveCondition() + "\n" +
			"方向:\t" + direction + "\n" +
			"坐标:\t(" + (Mathf.RoundToInt (transform.localPosition.x) + chessSize) + ", " + (Mathf.RoundToInt (transform.localPosition.z) + chessSize) + ")\n" +
			"");
	}

	void Start () {

		cj = ChessJudge.getSingleInstance();
		chessSize = cj.getChessSize();
		chess = cj.getChess ();
		game = new StartGame ();
			
		h = 0;
		v = 0;
		direction = "forward";

		electricity_H = (GameObject)Resources.Load ("Electricity_Horizontal");
		electricity_V = (GameObject)Resources.Load ("Electricity_Vertical");
	}

	void FixedUpdate() {

		/* 根据按键修改press字符串 */
		if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) { h = 0; v = 1; playersMove = false; } 
		if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) { h = 0; v = -1; playersMove = false; }
		if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) { h = -1; v = 0; playersMove = false; }
		if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) { h = 1; v = 0; playersMove = false; }
			
		if (h != 0 || v != 0) {
			Rotating ();
		}
			
	}

	void LateUpdate() {

		/* 按WSAD朝着面朝方向移动 */
		if (playersMove && ((Input.GetKey(KeyCode.W) && v == 1) || (Input.GetKey(KeyCode.S) && v == -1)||
			(Input.GetKey(KeyCode.A) && h == -1) || (Input.GetKey(KeyCode.D) && h == 1))) {
			Move ();
		}

		/* 按空格键添加栅栏（落子） */
		if (Input.GetKeyDown (KeyCode.Space)) {
			JudgeMoveCondition ();
			Debug.Log (cj.judgeGameResult ());
			//cj.printChess ();	// 测试
		}
	}

	/* 转身 */
	private void Rotating ()
	{
		if (!playersMove) {
			Vector3 dir = new Vector3 (h, 0, v);	// 获取方向
			Quaternion quaDir = Quaternion.LookRotation(dir,Vector3.up);			// 将方向转换为四元数
			transform.rotation = Quaternion.Lerp(transform.rotation, quaDir, Time.fixedDeltaTime * rotateSpeed); 		// 缓慢转动到目标点
		
			/* 转到一定程度时可以移动 */
			int rotY = (int)transform.eulerAngles.y;

			if ((v == 1 && Mathf.Abs(rotY % 360 - 0) <= 5) || (v == -1 && Mathf.Abs(rotY % 360 - 180) <= 5) ||
				(h == -1 && Mathf.Abs(rotY % 360 - 270) <= 5) || (h == 1 && Mathf.Abs(rotY % 360 - 90) <= 5)) {

				if (v == 1) {
					transform.localEulerAngles = new Vector3 (0f, 0f, 0f);
					direction = "forward";
				} else if (v == -1) {
					transform.localEulerAngles = new Vector3 (0f, 180f, 0f);
					direction = "back";
				} else if (h == -1) {
					transform.localEulerAngles = new Vector3 (0f, 270f, 0f);
					direction = "left";
				} else if (h == 1) {
					transform.localEulerAngles = new Vector3 (0f, 90f, 0f);
					direction = "right";
				} 
						
				playersMove = true;

			}
		}
	}

	/* 移动 */
	private void Move() {
		
		transform.Translate(Vector3.forward * moveSpeed * Time.fixedDeltaTime);
	}

	/* 判断落子条件是否成立，需要位置合适且无栅栏 */
	private bool JudgeMoveCondition() {

		/* 判断能否移动 */
		if (!playersMove)
			return false;

		/* 判断位置 */
		int posX = Mathf.RoundToInt (transform.localPosition.x);
		int posZ = Mathf.RoundToInt (transform.localPosition.z);
		if ((posX % 2 == 0) || (posZ % 2 == 0))
			return false;

		/* 判断是否有栅栏 */
		x = posX + chessSize;
		z = posZ + chessSize;

		if (x < 0 || z < 0 || x > 2 * chessSize || z > 2 * chessSize)
			return false;
		
		switch (direction) {
		case "forward":
			if (chess [x, z + 1] == 1)
				return false;
			else
				addFence (x, z + 1);
			break;
		case "back":
			if (chess[x, z - 1] == 1)
				return false;
			else
				addFence (x, z - 1);
			break;
		case "left":
			if (chess[x - 1, z] == 1)
				return false;
			else
				addFence (x - 1, z);
			break;
		case "right":
			if (chess[x + 1, z] == 1)
				return false;
			else
				addFence (x + 1, z);
			break;
		}

		cj.setPlayerPos (x, z);
		return true;
	}

	/* 在指定处添加栅栏并更新ChessJudge */
	private void addFence(int x, int z) {

		/* 初始化Fence */
		GameObject oneFence;

		if (x % 2 == 1)
			oneFence = Instantiate (electricity_H) as GameObject;
		else 
			oneFence = Instantiate (electricity_V) as GameObject;
		
		oneFence.transform.parent = GameObject.FindGameObjectWithTag ("Fences").transform;	// 设置为子物体
		oneFence.transform.localPosition = new Vector3 (-(chessSize - x) - x % 2, 0f, -(chessSize - z) - z % 2);	// 设置位置

		oneFence.transform.name = "Fence(" + x + "," + z + ")";	// 设置名字

		cj.setFence (x, z);
	}
}