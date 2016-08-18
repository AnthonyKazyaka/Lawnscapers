using UnityEngine;
using System.Collections;

public abstract class MovingObstacle : MonoBehaviour
{
    [SerializeField]
    protected Transform _startTransform;
    public Transform StartTransform { get { return _startTransform; } set { _startTransform = value; } }

    [SerializeField]
    protected Transform _endTransform;
    public Transform EndTransform { get { return _endTransform; } set { _endTransform = value; } }

    [SerializeField]
    protected bool _loopMovement = true;
    public bool LoopMovement { get { return _loopMovement; } set { _loopMovement = value; } }

    [SerializeField]
    protected float _waitTimeAtEndPoints = 2.0f;
    public float WaitTimeAtEndPoints { get { return _waitTimeAtEndPoints; } set { _waitTimeAtEndPoints = value; } }

    public bool IsCurrentlyAtEndPoint { get; set; }

    protected float _timeLeftWaitingAtEndPoint;
    protected int _direction = 1;

    // Use this for initialization
    void Start ()
    {
        IsCurrentlyAtEndPoint = false;
	}
	
	// Update is called once per frame
	protected virtual void FixedUpdate ()
    {
        if(!IsCurrentlyAtEndPoint)
        {
            Move();
        }
        if (LoopMovement)
        { 
            if (IsCurrentlyAtEndPoint && _timeLeftWaitingAtEndPoint > 0.0f)
            {
                _timeLeftWaitingAtEndPoint -= Time.deltaTime;

                if (_timeLeftWaitingAtEndPoint <= 0)
                {
                    IsCurrentlyAtEndPoint = false;
                }
            }
            else
            {
                _timeLeftWaitingAtEndPoint = WaitTimeAtEndPoints;
            }
        }
	}

    public abstract void Move();

}
