using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour {

	public float loadingSpeed = 1;
	private float targetValue, value;
	private AsyncOperation operation;

	void Start () {
		value = 0;
		if (SceneManager.GetActiveScene().name == "Loading")
		{
			StartCoroutine(AsyncLoading());		// 启动协程
		}
	}

	IEnumerator AsyncLoading()
	{
		operation = SceneManager.LoadSceneAsync("ChessGame");

		operation.allowSceneActivation = false;	// 阻止当加载完成自动切换

		yield return operation;
	}

	void Update () {
		
		targetValue = operation.progress;
		if (operation.progress >= 0.9f)
		{
			targetValue = 1.0f;		// operation.progress的值最大为0.9

		}

		if (targetValue != value)
		{
			value = Mathf.Lerp(value, targetValue, Time.deltaTime * loadingSpeed);		// 插值运算

			if (Mathf.Abs(value - targetValue) < 0.01f)
			{
				value = targetValue;
			}
		}

		if ((int)(value * 100) == 100)
		{
			operation.allowSceneActivation = true;			// 允许异步加载完毕后自动切换场景
		}
	}
}
