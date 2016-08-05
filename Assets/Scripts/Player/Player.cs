using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    [SerializeField]
    private LawnMower _equippedMower;
    public LawnMower EquippedMower { get { return _equippedMower; } set { _equippedMower = value; } }

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}
}
