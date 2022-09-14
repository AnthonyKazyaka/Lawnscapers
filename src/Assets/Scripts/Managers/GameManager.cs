using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    [SerializeField]
    private bool _isPaused = false;
    public bool IsPaused { get { return _isPaused; } private set { _isPaused = value; } }

    public enum MowerTypes
    {
        Reel,
        Push,
        Riding
    }

    [SerializeField]
    private bool _isReelUnlocked = true;
    public bool IsReelUnlocked { get { return _isReelUnlocked; } private set { _isReelUnlocked = value; } }

    [SerializeField]
    private bool _isPushUnlocked = false;
    public bool IsPushUnlocked { get { return _isPushUnlocked; } private set { _isPushUnlocked = value; } }

    [SerializeField]
    private bool _isRidingUnlocked = false;
    public bool IsRidingUnlocked { get { return _isRidingUnlocked; } private set { _isRidingUnlocked = value; } }

    [SerializeField]
    private Puzzle _currentPuzzle;
    public Puzzle CurrentPuzzle { get { return _currentPuzzle; } set { _currentPuzzle = value; } }

    [SerializeField]
    private PlayerController _playerController;
    public PlayerController PlayerController
    {
        get
        {
            if (_playerController == null) {
                _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            }
            return _playerController;
        }
    }


    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        _instance = this;

        LoadGameData();

        FindPuzzle();
    }

    private void LoadGameData()
    {
        LoadSettings();
        LoadStats();
        LoadUnlocks();
    }

    private void LoadSettings()
    {
        
    }

    private void LoadStats()
    {

    }

    private void LoadUnlocks()
    {
        // Load from player prefs or XML
        if(true)
        {
            
        }
    }

    public void UnlockMowerType(MowerTypes typeToUnlock)
    {
        //UnlockedMowers[typeToUnlock] = true;
        switch(typeToUnlock)
        {
            case MowerTypes.Reel:
                IsReelUnlocked = true;
                break;
            case MowerTypes.Push:
                IsPushUnlocked = true;
                break;
            case MowerTypes.Riding:
                IsRidingUnlocked = true;
                break;
            default:
                break;
        }
    }

	// Use this for initialization
	void Start ()
    {
        StatsTracker.LoadGameStats();
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void PauseGame()
    {
        IsPaused = true;
    }

    public void UnpauseGame()
    {
        IsPaused = false;
    }

    private void FindPuzzle()
    {
        CurrentPuzzle = GameObject.FindObjectOfType<Puzzle>();
    }
}
