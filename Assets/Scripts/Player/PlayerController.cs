using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    private Player _player;
    public Player Player { get { return _player; } private set { _player = value; } }

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

    private GameManager.MowerTypes _mowerTypeToSwitchTo = GameManager.MowerTypes.Reel;

    [SerializeField]
    private float _minimumTouchVelocity = 200.0f;
    public float MinimumTouchVelocity { get { return _minimumTouchVelocity; } }

    private Vector2 _previousMousePosition;
    private Vector2 _currentMousePosition { get { return new Vector2(Input.mousePosition.x, Input.mousePosition.y); } }


    // Use this for initialization
    void Start ()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
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
            bool touchInputReceived = Input.touchCount > 0;
            float touchVelocity = 0.0f;

            Vector2 gestureMovementDirection = new Vector2(0, 0);

            if (touchInputReceived && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                Touch touch = Input.GetTouch(0);
                var touchDelta = touch.deltaPosition;
                touchVelocity = Mathf.Abs((touch.deltaPosition / touch.deltaTime).magnitude);

                gestureMovementDirection = (touchVelocity > MinimumTouchVelocity) ? ((Mathf.Abs(touchDelta.x) > Mathf.Abs(touchDelta.y)) ? gestureMovementDirection = new Vector2(touchDelta.x, 0).normalized : gestureMovementDirection = new Vector2(0, touchDelta.y).normalized) : gestureMovementDirection;
            }

            if (Input.mousePresent && Input.GetMouseButton(0))
            {
                var mousePositionDelta = _currentMousePosition - _previousMousePosition;
                touchVelocity = Mathf.Abs((mousePositionDelta / Time.deltaTime).magnitude);

                gestureMovementDirection = (touchVelocity > MinimumTouchVelocity) ? ((Mathf.Abs(mousePositionDelta.x) > Mathf.Abs(mousePositionDelta.y)) ? gestureMovementDirection = new Vector2(mousePositionDelta.x, 0).normalized : gestureMovementDirection = new Vector2(0, mousePositionDelta.y).normalized) : gestureMovementDirection;
                Debug.Log("Velocity: " + touchVelocity);
                Debug.Log("Delta: " + mousePositionDelta);
            }



            if (!Player.EquippedMower.IsMoving)
            {
                if(_mowerTypeToSwitchTo != _currentMowerType)
                {
                    SwitchEquippedMower(_mowerTypeToSwitchTo);
                }

                if (gestureMovementDirection.magnitude > 0)
                {
                    Player.EquippedMower.Move(gestureMovementDirection.x, gestureMovementDirection.y);
                }
                else if (System.Math.Abs(Input.GetAxis("Horizontal")) > 0 || System.Math.Abs(Input.GetAxis("Vertical")) > 0)
                {
                    Player.EquippedMower.Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                }
            }

            if (Input.GetKeyDown(ReelKey) && GameManager.Instance.IsReelUnlocked && GameManager.Instance.CurrentPuzzle.IsReelMowerAllowed)
            {
                _mowerTypeToSwitchTo = GameManager.MowerTypes.Reel;
            }
            else if (Input.GetKeyDown(PushKey) && GameManager.Instance.IsPushUnlocked && GameManager.Instance.CurrentPuzzle.IsPushMowerAllowed)
            {
                _mowerTypeToSwitchTo = GameManager.MowerTypes.Push;
            }
            else if (Input.GetKeyDown(RidingKey) && GameManager.Instance.IsRidingUnlocked && GameManager.Instance.CurrentPuzzle.IsRidingMowerAllowed)
            {
                _mowerTypeToSwitchTo = GameManager.MowerTypes.Riding;
            }

            _previousMousePosition = _currentMousePosition;
        }
	}

    /// <summary>
    /// Switches the player's equipped mower.
    /// </summary>
    /// <returns>Returns true if mower type changed, false if mower type already equipped.</returns>
    private bool SwitchEquippedMower(GameManager.MowerTypes mowerType, bool forceSwitch = false)
    {
        if(!forceSwitch && _currentMowerType == mowerType)
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
                Player.EquippedMower.GetComponent<ReelMower>().enabled = false;
                break;
            case GameManager.MowerTypes.Push:
                Player.EquippedMower.GetComponent<PushMower>().enabled = false;
                break;
            case GameManager.MowerTypes.Riding:
                Player.EquippedMower.GetComponent<RidingMower>().enabled = false;
                break;
            default:
                break;
        }
    }

    private void DisableAllMowerTypes()
    {
        Player.EquippedMower.GetComponent<ReelMower>().enabled = false;

        foreach (var mower in Player.EquippedMower.GetComponents<PushMower>())
        {
            if (mower.GetType() == typeof(PushMower))
            {
                mower.enabled = false;
            }
        }

        Player.EquippedMower.GetComponent<RidingMower>().enabled = false;
    }

    private void EnableMowerType(GameManager.MowerTypes mowerToEnable)
    {
        GameObject mowerGameObject = GameObject.FindGameObjectWithTag("LawnMower");
        switch (mowerToEnable)
        {
            case GameManager.MowerTypes.Reel:
                Player.EquippedMower.GetComponent<ReelMower>().enabled = true;
                Player.EquippedMower = mowerGameObject.GetComponent<ReelMower>();
                break;
            case GameManager.MowerTypes.Push:
                PushMower pushMowerComponent = Player.EquippedMower.GetComponent<PushMower>();
                foreach(var mower in Player.EquippedMower.GetComponents<PushMower>())
                {
                    if(mower.GetType() == typeof(PushMower))
                    {
                        pushMowerComponent = mower;
                    }
                }

                pushMowerComponent.enabled= true;
                Player.EquippedMower = pushMowerComponent;
                break;
            case GameManager.MowerTypes.Riding:
                Player.EquippedMower.GetComponent<RidingMower>().enabled = true;
                Player.EquippedMower = mowerGameObject.GetComponent<RidingMower>();
                break;
            default:
                break;
        }
        _currentMowerType = mowerToEnable;
    }

    public void SetMowerType(GameManager.MowerTypes type)
    {
        DisableAllMowerTypes();
        SwitchEquippedMower(type, true);
        _mowerTypeToSwitchTo = type;
    }
}
