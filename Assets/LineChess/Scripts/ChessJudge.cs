﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ChessJudge 
{ 
	private static ChessJudge instance = new ChessJudge();    // 创建ChessJudge的一个对象
	private int chessSize = 6;     // 棋盘大小 大小2~6
	private int[, ] chess;
	private int playerPosX, playerPosZ, AIPosX, AIPosZ;        // 棋子位置（数组下标）

	/* private构造函数，并初始化属性 */
	private ChessJudge(){

		chess = new int[2 * chessSize + 1, 2 * chessSize + 1];
		playerPosX = 1;
		playerPosZ = 1;
		AIPosX = 2 * chessSize - 1;
		AIPosZ = 2 * chessSize - 1;

		/* 玩家 */
		chess [playerPosX, playerPosZ] = 1;    // 玩家所在位置
		chess [AIPosX, AIPosZ] = 1;    // AI所在位置

		/* 柱子 */
		for (int i = 0; i < chessSize * 2 + 1; i += 2) {
			for (int j = 0; j < chessSize * 2 + 1; j+= 2) {
				chess [i, j] = 1;
			}
		}

		/* 围栏 */
		for (int i = 1; i < chessSize * 2 + 1; i += 2) {
			chess [i, 0] = 1;
			chess [i, 12] = 1;
			chess [0, i] = 1;
			chess [12, i] = 1;
		}
	}    

	/* 获取唯一可用的对象 */
	public static ChessJudge getSingleInstance(){
		return instance;
	}

	/* 判断游戏结果，胜负相对于玩家来说 */
	public string judgeGameResult() {

		/* 判断玩家是否被包围 */
		bool p = (chess [playerPosX - 1, playerPosZ] + chess [playerPosX + 1, playerPosZ] + 
			chess [playerPosX, playerPosZ - 1] + chess [playerPosX, playerPosZ + 1] == 4);

		/* 判断AI是否被包围 */
		bool a = (chess [AIPosX - 1, AIPosZ] + chess [AIPosX + 1, AIPosZ] + 
			chess [AIPosX, AIPosZ - 1] + chess [AIPosX, AIPosZ + 1] == 4); 

		if (p && a)
			return "平局";
		else if (p && !a)
			return "失败";
		else if (!p && a)
			return "胜利";
		else
			return "胜负未分";
	}

	public bool playerHasFence(string direction) {
		switch (direction) {
		case "forward" : return (chess [playerPosX, playerPosZ + 1] == 1);
		case "back" : return (chess [playerPosX, playerPosZ - 1] == 1);
		case "left" : return (chess [playerPosX - 1, playerPosZ] == 1);
		case "right" : return (chess [playerPosX + 1, playerPosZ] == 1);
		}
		return false;
	}

	public int[, ] getChess() {
		return chess;
	}

	public int getChessSize() {
		return chessSize;
	}

	public int getPlayerPosX() {
		return playerPosX;
	}

	public int getPlayerPosZ() {
		return playerPosZ;
	}

	public int getAIPosX() {
		return AIPosX;
	}

	public int getAIPosZ() {
		return AIPosZ;
	}

	public void setPlayerPos(int x, int z) {

		chess [playerPosX, playerPosZ] = 0;
		chess [x, z] = 1;

		playerPosX = x;
		playerPosZ = z;

	}

	public void setFence(int x, int z) {
		chess [x, z] = 1;
	}

	public void printChess() {
		string chessStr = "";
		for (int i = 0; i < 2 * chessSize + 1; i++) {
			for (int j = 0; j < 2 * chessSize + 1; j++) {
				chessStr += (chess[i, j] + " ");
			}
			chessStr += ("\n");
		}
		chessStr += judgeGameResult ();
		Debug.Log (chessStr);
	}
} 

