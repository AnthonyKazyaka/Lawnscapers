using UnityEngine;
using System.Collections;
using Lawnscapers.Extensions;
using System.Linq;
using UnityEngine.UI;
using System.Collections.Generic;

public class LevelSelectScreen : MonoBehaviour {

    Dictionary<int, string> levelNumberToSceneNameDictionary = new Dictionary<int, string>()
    {
        { 1, "Tile Prototype" }
    };

	// Use this for initialization
	void Start ()
    {
        Social.ReportProgress(GPGSIds.achievement_take_your_time, 0.0f, (bool success) =>
        {
            if(success)
            {
                StartCoroutine(ShowAchievementsAfterWait(10.0f));
            }
        });
	}

    private IEnumerator ShowAchievementsAfterWait(float seconds)
    {
        float beginTime = Time.time;
        while (Time.time < beginTime + seconds)
        {
            yield return null;
        }

        GooglePlayGames.PlayGamesPlatform.Instance.ShowAchievementsUI();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void LoadLevel(Object buttonClicked)
    {
        int levelNumber;
        Text buttonText = ((GameObject)buttonClicked).GetComponentInChildren<Text>();

        var successfulParse = int.TryParse(buttonText.text.Split(" ").Last(), out levelNumber);
        if(!successfulParse)
        {
            return;
        }

        var sceneName = levelNumberToSceneNameDictionary[levelNumber];

        Debug.Log("Level " + levelNumber + " selected!");
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
