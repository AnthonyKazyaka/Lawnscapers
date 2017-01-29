using UnityEngine;
using System.Collections;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using GooglePlayGames.BasicApi;
using UnityEngine.UI;
using System.Linq;

public class MainMenuScreen : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        // recommended for debugging:
        PlayGamesPlatform.DebugLogEnabled = true;
        // Activate the Google Play Games platform
        PlayGamesPlatform.Activate();

        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                Debug.Log("Login success");
                EnableGooglePlayElements();
            }
            else
            {
                Debug.Log("Login failed");
                DisableGooglePlayElements();
            }
        });
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ToggleGooglePlayElements(bool showElements)
    {
        Button achievementsButton = GameObject.Find("AchievementsButton").GetComponent<Button>();
        achievementsButton.interactable = showElements;
    }

    public void EnableGooglePlayElements()
    {
        ToggleGooglePlayElements(true);
    }

    public void DisableGooglePlayElements()
    {
        ToggleGooglePlayElements(false);
    }

    public void PlayButtonClick()
    {
        Debug.Log("Play!");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level Select");
    }

    public void AchievementsButtonClick()
    {
        Social.ShowAchievementsUI();
    }

    public void OptionsButtonClick()
    {
        Debug.Log("Options");
    }

    public void ExitGameClick()
    {
        Debug.Log("Exiting the game!");
        Application.Quit();
    }

}