using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour
{
    protected bool _playerHitRecorded = false;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "LawnMower" && !_playerHitRecorded)
        {
            GameManager.Instance.CurrentPuzzle.FailPuzzle();
            _playerHitRecorded = true;
        }
    }

    public virtual void Reset()
    {
        _playerHitRecorded = false;
    }
}
