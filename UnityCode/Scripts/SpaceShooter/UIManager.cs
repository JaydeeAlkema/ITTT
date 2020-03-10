using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Manages the UI.
/// </summary>
public class UIManager : MonoBehaviour
{
	private static UIManager instance;

	[SerializeField] private Arduino arduino;
	[Space]
	[SerializeField] private float updateInterval = 0.25f;
	[Space]
	[SerializeField] private int playerScore;
	[SerializeField] private TextMeshProUGUI scoreCounter;
	[SerializeField] private Slider playerFuelSlider;
	[SerializeField] private Slider playerHealthSlider;
	[Space]
	[SerializeField] private SpaceShooterPlayer shooterPlayer;
	[SerializeField] private int playerHighscore;
	[Space]
	[SerializeField] private bool gameOver = false;
	[SerializeField] private GameObject gameOverScreen;
	[SerializeField] private TextMeshProUGUI gameOverScore;
	[SerializeField] private TextMeshProUGUI gameOverHighScore;
	[SerializeField] private float shakeWeight;


	public static UIManager Instance { get => instance; set => instance = value; }

	private void Awake()
	{
		if(!Instance && Instance != this)
			Instance = this;
	}

	private void Update()
	{
		if(gameOver)
			shakeWeight += arduino.GyroAccel;
		if(shakeWeight <= -3000 || Input.GetKey(KeyCode.R))
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	private void Start()
	{
		StartCoroutine(UpdateUserInterface());
		gameOverScreen.SetActive(false);
		playerHighscore = PlayerPrefs.GetInt("Highscore");
	}

	/// <summary>
	/// I don't want to update the UI every frame. Because you won't notice the difference anyway.
	/// </summary>
	/// <returns></returns>
	private IEnumerator UpdateUserInterface()
	{
		while(true)
		{
			yield return new WaitForSeconds(updateInterval);
			scoreCounter.text = "Score: \n" + playerScore;
			playerFuelSlider.value = shooterPlayer.FuelLeft;
			playerHealthSlider.value = shooterPlayer.HitPointsLeft;

			if(playerHealthSlider.value <= 0 || playerFuelSlider.value < 0)
			{
				ToggleGameOver();
			}
		}
	}

	/// <summary>
	/// A simple function to add points.
	/// </summary>
	/// <param name="pointsToAdd"></param>
	public void AddPoints(int pointsToAdd)
	{
		playerScore += pointsToAdd;
	}

	/// <summary>
	/// Toggles the game over.
	/// </summary>
	private void ToggleGameOver()
	{
		gameOver = true;

		shooterPlayer.gameObject.SetActive(false);
		gameOverScreen.SetActive(true);

		if(playerScore > playerHighscore)
		{
			playerHighscore = playerScore;
			PlayerPrefs.SetInt("Highscore", playerHighscore);
		}

		gameOverScore.text = "Score: \n" + playerScore;
		gameOverHighScore.text = "Highscore: \n" + playerHighscore;

	}
}
