using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public string levelToLoad = "MainLevel";

    public SceneFader sceneFader;

    public GameObject backButton;

    [Header("Main Menu Items")]
    public GameObject gameName;
    public GameObject playButton;
    public GameObject optionsButton;
    public GameObject creditsButton;
    public GameObject quitButton;

    [Header("Credits Items")]
    public GameObject creditsText;

    [Header("Options Items")]
    public GameObject optionsText;

    public void Play ()
	{
		sceneFader.FadeTo(levelToLoad);
        SceneManager.LoadScene("LevelSelect");
    }

    public void Options()
    {
        playButton.SetActive(false);
        optionsButton.SetActive(false);
        creditsButton.SetActive(false);
        quitButton.SetActive(false);

        optionsText.SetActive(true);
        backButton.SetActive(true);
    }

    public void Credits ()
    {
        playButton.SetActive(false);
        optionsButton.SetActive(false);
        creditsButton.SetActive(false);
        quitButton.SetActive(false);

        creditsText.SetActive(true);
        backButton.SetActive(true);
    }

    public void BackToMainMenu()
    {
        playButton.SetActive(true);
        optionsButton.SetActive(true);
        creditsButton.SetActive(true);
        quitButton.SetActive(true);

        optionsText.SetActive(false);
        creditsText.SetActive(false);
        backButton.SetActive(false);
    }

    public void Quit ()
	{
		Debug.Log("Exciting...");
		Application.Quit();
	}

}
