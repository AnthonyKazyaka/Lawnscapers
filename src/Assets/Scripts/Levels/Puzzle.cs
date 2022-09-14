using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using GooglePlayGames.BasicApi;
using System.Threading;
using UnityEngine.UI;

public class Puzzle : MonoBehaviour
{
    public string PuzzleName = string.Empty;

    [SerializeField]
    private List<Tile> _gameTiles;
    public List<Tile> GameTiles { get { return _gameTiles; } set { _gameTiles = value; } }

    public List<Grass> GrassTiles { get { return GameTiles.OfType<Grass>().ToList(); } }

    public bool IsPuzzleComplete { get { return GrassTiles.Where(x => !x.IsMowed).Count() == 0; } }

    [SerializeField]
    private Transform _playerStartTransform;
    public Transform PlayerStartTransform { get { return _playerStartTransform; } private set { _playerStartTransform = value; } }

    [SerializeField]
    private int _actionsTaken = 0;
    public int ActionsTaken { get { return _actionsTaken; } set { _actionsTaken = value; } }

    [SerializeField]
    private int _score = 0;
    public int Score { get { return _score; } private set { _score = value; } }

    [SerializeField]
    private int _maxPossibleScore = 10000;
    public int MaxPossibleScore { get { return _maxPossibleScore; } }

    [SerializeField]
    private bool _isReelMowerAllowed = true;
    public bool IsReelMowerAllowed { get { return _isReelMowerAllowed; } private set { _isReelMowerAllowed = value; } }

    [SerializeField]
    private bool _isPushMowerAllowed = true;
    public bool IsPushMowerAllowed { get { return _isPushMowerAllowed; } private set { _isPushMowerAllowed = value; } }

    [SerializeField]
    private bool _isRidingMowerAllowed = true;
    public bool IsRidingMowerAllowed { get { return _isRidingMowerAllowed; } private set { _isRidingMowerAllowed = value; } }

    public bool HasBeenCompletedPreviously { get; set; }

    private bool _completionRecorded = false;

    private string _puzzleCompletedSaveName { get { return "LevelCompleted: " + PuzzleName; } }
    private string _puzzleHighScoreSaveName { get { return "Highest Score: " + PuzzleName; } }

    void Start()
    {
        //PlayerPrefs.DeleteAll();

        if (string.IsNullOrEmpty(PuzzleName))
        {
            PuzzleName = SceneManager.GetActiveScene().name;
        }
        if(!string.IsNullOrEmpty(PuzzleName) && PlayerPrefs.HasKey(_puzzleCompletedSaveName))
        {
            int levelCompleted = PlayerPrefs.GetInt(_puzzleCompletedSaveName);
            HasBeenCompletedPreviously = (levelCompleted == 1) ? true : false;
        }
        else
        {
            HasBeenCompletedPreviously = false;
        }

        if(PlayerStartTransform == null)
        {
            PlayerStartTransform = this.transform;
        }
    }

    void Awake()
    {
        GameTiles = GameObject.FindObjectsOfType<Tile>().ToList();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!GameManager.Instance.IsPaused)
        {
            if (IsPuzzleComplete && !_completionRecorded)
            {
                Debug.Log("Puzzle complete!");
                CompletePuzzle();
            }
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                Reset();
            }
            StatsTracker.TimeInGame += Time.deltaTime;
        }
        else
        {
            StatsTracker.TimeInMenus += Time.deltaTime;
        }
	}

    public void FailPuzzle()
    {
        CalculateScore();
        Debug.Log("You suck");
        //GameManager.Instance.PauseGame();
    }

    public void CompletePuzzle()
    {
        if (!HasBeenCompletedPreviously)
        {
            PlayerPrefs.SetInt(_puzzleCompletedSaveName, 1);
            PlayerPrefs.SetInt(_puzzleHighScoreSaveName, Score);
            HasBeenCompletedPreviously = true;

            StatsTracker.LevelsCompleted++;
        }
        else
        {
            if(!PlayerPrefs.HasKey(_puzzleHighScoreSaveName))
            {
                var previousHighScore = PlayerPrefs.GetInt(_puzzleHighScoreSaveName);
                if(Score > previousHighScore)
                {
                    PlayerPrefs.SetInt(_puzzleHighScoreSaveName, Score);
                }
            }
            StatsTracker.LevelsReplayed++;
        }

        Debug.Log(string.Format("Levels Completed: {0}; Levels Replayed: {1}\r\nTotal Score: {2}; Grass Mowed: {3}",
                                    StatsTracker.LevelsCompleted.ToString(),
                                    StatsTracker.LevelsReplayed.ToString(),
                                    StatsTracker.TotalScore,
                                    StatsTracker.GrassTilesMowed));

        CalculateScore();
        StatsTracker.SaveGameStats();

        if(StatsTracker.GrassTilesMowed < 1000)
        {
            if (Social.localUser.authenticated)
            {
                Social.ReportProgress(GPGSIds.achievement_find_yourself_a_hobby, 0.0f, (bool success) =>
                {
                    if (!success)
                    {
                        Reset();
                    }
                });

                PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_find_yourself_a_hobby, GrassTiles.Count, (bool success) =>
                {
                    Debug.Log("Did it increment? " + ((success) ? "Yes" : "No"));
                    if (!success)
                    {
                        var text = GameObject.Find("LogText").GetComponent<Text>();
                        text.text = "Didn't work, trying to fully unlock the achievement.";
                        text.enabled = true;

                        Social.ReportProgress(GPGSIds.achievement_find_yourself_a_hobby, 100.0f, (bool success2) =>
                        {
                            if (!success2)
                            {
                                ReturnToLevelSelect(2.0f);
                            }
                        });
                    }
                });
            }
        }

        _completionRecorded = true;

        ReturnToMainMenu(5.0f);
    }

    public void ReturnToLevelSelect(float secondsToWait)
    {
        StartCoroutine(ReturnToScene("Level Select", secondsToWait));
    }

    public void ReturnToMainMenu(float secondsToWait)
    {
        StartCoroutine(ReturnToScene("Main Menu", secondsToWait));
    }

    public IEnumerator ReturnToScene(string sceneName, float secondsToWait)
    {
        float beginTime = Time.time;
        while (Time.time < beginTime + secondsToWait)
        {
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }

    private void CalculateScore()
    {
        Score = Mathf.RoundToInt((float) MaxPossibleScore / (float) ActionsTaken);
        if(Score > MaxPossibleScore || Score < 0)
        {
            Score = 0;
        }

        Debug.Log("Score: " + Score);
        Debug.Log("Time In Game: " + StatsTracker.TimeInGame);
        Debug.Log("Time In Menus: " + StatsTracker.TimeInMenus);
    }

    public void Reset()
    {
        GrassTiles.ForEach(x => x.Reset());
        GameObject.FindObjectsOfType<Obstacle>().ToList().ForEach(x => x.Reset());

        PlayerController playerController = GameManager.Instance.PlayerController;

        playerController.Player.EquippedMower.Reset();
        GameManager.MowerTypes mowerType;

        if (IsReelMowerAllowed)
        {
            mowerType = GameManager.MowerTypes.Reel;
        }
        else if(IsPushMowerAllowed)
        {
            mowerType = GameManager.MowerTypes.Push;
        }
        else
        {
            mowerType = GameManager.MowerTypes.Riding;
        }

        playerController.SetMowerType(mowerType);
        playerController.Player.EquippedMower.transform.position = PlayerStartTransform.position;

        _completionRecorded = false;
        Score = 0;
        ActionsTaken = 0;
    }

}
