using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    private Player _player;

    [SerializeField]
    private GameManager.MowerTypes _currentMowerType = GameManager.MowerTypes.Reel;
    //public MowerTypes MowerType { get { return _mowerType; } set { _mowerType = value; } }

    [SerializeField]
    private KeyCode _reelKey = KeyCode.Alpha1;
    public KeyCode ReelKey { get { return _reelKey; } }

    [SerializeField]
    private KeyCode _pushKey = KeyCode.Alpha2;
    public KeyCode PushKey { get { return _pushKey; } }

    [SerializeField]
    private KeyCode _ridingKey = KeyCode.Alpha3;
    public KeyCode RidingKey { get { return _ridingKey; } }


    // Use this for initialization
    void Start ()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update ()
    {
        if (GameManager.Instance.IsPaused)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameManager.Instance.UnpauseGame();
                // Don't accept any more input this update
                return;
            }
        }
        if (!GameManager.Instance.IsPaused)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameManager.Instance.PauseGame();
                // Don't accept any more input this update
                return;
            }

            if (!_player.EquippedMower.IsMoving)
            {
                if (System.Math.Abs(Input.GetAxis("Horizontal")) > 0 || System.Math.Abs(Input.GetAxis("Vertical")) > 0)
                {
                    _player.EquippedMower.MoveMower(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                    Debug.Log(_player.EquippedMower.GetType().ToString());
                }

                if (Input.GetKeyDown(ReelKey) && GameManager.Instance.IsReelUnlocked && GameManager.Instance.CurrentPuzzle.IsReelMowerAllowed)
                {
                    SwitchEquippedMower(GameManager.MowerTypes.Reel);
                }
                else if (Input.GetKeyDown(PushKey) && GameManager.Instance.IsPushUnlocked && GameManager.Instance.CurrentPuzzle.IsPushMowerAllowed)
                {
                    SwitchEquippedMower(GameManager.MowerTypes.Push);
                }
                else if (Input.GetKeyDown(RidingKey) && GameManager.Instance.IsRidingUnlocked && GameManager.Instance.CurrentPuzzle.IsRidingMowerAllowed)
                {
                    SwitchEquippedMower(GameManager.MowerTypes.Riding);
                }
            }
        }
	}

    /// <summary>
    /// Switches the player's equipped mower.
    /// </summary>
    /// <returns>Returns true if mower type changed, false if mower type already equipped.</returns>
    private bool SwitchEquippedMower(GameManager.MowerTypes mowerType)
    {
        if(_currentMowerType == mowerType)
        {
            return false;
        }
        else
        {
            DisableMowerType(_currentMowerType);

            switch (mowerType)
            {
                case GameManager.MowerTypes.Reel:
                    EquipReelMower();
                    break;
                case GameManager.MowerTypes.Push:
                    EquipPushMower();
                    break;
                case GameManager.MowerTypes.Riding:
                    EquipRidingMower();
                    break;
                default:
                    break;
            }
        }

        return true;
    }

    private void EquipReelMower()
    {
        EnableMowerType(GameManager.MowerTypes.Reel);
    }

    private void EquipPushMower()
    {
        EnableMowerType(GameManager.MowerTypes.Push);
    }

    private void EquipRidingMower()
    {
        EnableMowerType(GameManager.MowerTypes.Riding);
    }

    private void DisableMowerType(GameManager.MowerTypes mowerToDisable)
    {
        switch (mowerToDisable)
        {
            case GameManager.MowerTypes.Reel:
                _player.EquippedMower.GetComponent<ReelMower>().enabled = false;
                break;
            case GameManager.MowerTypes.Push:
                _player.EquippedMower.GetComponent<PushMower>().enabled = false;
                break;
            case GameManager.MowerTypes.Riding:
                _player.EquippedMower.GetComponent<RidingMower>().enabled = false;
                break;
            default:
                break;
        }
    }

    private void EnableMowerType(GameManager.MowerTypes mowerToEnable)
    {
        GameObject mowerGameObject = GameObject.FindGameObjectWithTag("LawnMower");
        switch (mowerToEnable)
        {
            case GameManager.MowerTypes.Reel:
                _player.EquippedMower.GetComponent<ReelMower>().enabled = true;
                _player.EquippedMower = mowerGameObject.GetComponent<ReelMower>();
                break;
            case GameManager.MowerTypes.Push:
                PushMower pushMowerComponent = _player.EquippedMower.GetComponent<PushMower>();
                foreach(var mower in _player.EquippedMower.GetComponents<PushMower>())
                {
                    if(mower.GetType() == typeof(PushMower))
                    {
                        pushMowerComponent = mower;
                    }
                }

                pushMowerComponent.enabled= true;
                _player.EquippedMower = pushMowerComponent;
                break;
            case GameManager.MowerTypes.Riding:
                _player.EquippedMower.GetComponent<RidingMower>().enabled = true;
                _player.EquippedMower = mowerGameObject.GetComponent<RidingMower>();
                break;
            default:
                break;
        }
        _currentMowerType = mowerToEnable;
    }
}
