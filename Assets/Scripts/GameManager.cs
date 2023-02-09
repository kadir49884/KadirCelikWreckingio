using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEditor;

public class GameManager : MonoBehaviour
{

	private bool IsGameFinish { get; set; }
	private bool IsGameStarted { get; set; }

	public Action GameStart { get; set; }
	public Action GameWin { get; set; }
	public Action GameFail { get; set; }
	public Action EnemyDead { get; set; }

	public bool RunGame { get => IsGameStarted && !IsGameFinish; }


	public static GameManager Instance { get => instance; set => instance = value; }

	private static GameManager instance;


	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		
	}

	private void Start()
	{

		GameStart += Initialize;
		GameWin += Game_Win;
		GameFail += Game_Fail;
	}

	


	public void Initialize()
	{
		IsGameStarted = true;
	}

	private void Game_Win()
	{
		IsGameFinish = true;
	}


	private void Game_Fail()
	{
		IsGameFinish = true;
	}

	

	public void RestartLevel()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

}



