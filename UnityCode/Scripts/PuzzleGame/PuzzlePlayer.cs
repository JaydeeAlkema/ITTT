using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// The state of the game.
/// </summary>
public enum GAMESTATE
{
	LIVE,
	END
}

public class PuzzlePlayer : MonoBehaviour
{
	[SerializeField] private GAMESTATE gameState;
	[Space]
	[SerializeField] private int collectablesCollected = 0;
	[SerializeField] private string collectableTag = "Collectable";
	[SerializeField] private string goalTag = "Goal";
	[SerializeField] private float timer = 0.00f;
	[Space]
	[SerializeField] private TextMeshProUGUI timerText;
	[SerializeField] private TextMeshProUGUI scoreText;
	[SerializeField] private GameObject endGameScreen;

	private void Start()
	{
		endGameScreen.SetActive(false);
	}

	/// <summary>
	/// Updates an timer, get's input, updates UI. Does a lot in here.
	/// </summary>
	private void Update()
	{
		timer += Time.deltaTime;

		timerText.text = "Time: " + timer.ToString("F2");
		scoreText.text = "Score: " + collectablesCollected.ToString();

		if(gameState == GAMESTATE.END)
		{
			if(Input.GetKeyDown(KeyCode.Space))
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
		if(Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.G))
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		if(Input.GetKey(KeyCode.Escape))
			Application.Quit();

	}

	/// <summary>
	/// Trigger event. Used to collect and get the end.
	/// </summary>
	/// <param name="other"></param>
	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag(collectableTag))
		{
			collectablesCollected++;
			Destroy(other.gameObject);
		}
		if(other.CompareTag(goalTag))
		{
			EndGame();
		}
	}

	/// <summary>
	/// End Game event.
	/// </summary>
	private void EndGame()
	{
		endGameScreen.SetActive(true);
		gameState = GAMESTATE.END;
	}
}
