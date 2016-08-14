using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Puzzle : MonoBehaviour
{
    [SerializeField]
    private List<Tile> _gameTiles;
    public List<Tile> GameTiles { get { return _gameTiles; } set { _gameTiles = value; } }

    public List<Grass> GrassTiles { get { return GameTiles.OfType<Grass>().ToList(); } }

    public bool IsPuzzleComplete { get { return GrassTiles.Where(x => !x.IsMowed).Count() == 0; } }

    [SerializeField]
    private bool _isReelMowerAllowed = true;
    public bool IsReelMowerAllowed { get { return _isReelMowerAllowed; } private set { _isReelMowerAllowed = value; } }

    [SerializeField]
    private bool _isPushMowerAllowed = true;
    public bool IsPushMowerAllowed { get { return _isPushMowerAllowed; } private set { _isPushMowerAllowed = value; } }

    [SerializeField]
    private bool _isRidingMowerAllowed = true;
    public bool IsRidingMowerAllowed { get { return _isRidingMowerAllowed; } private set { _isRidingMowerAllowed = value; } }

    
    void Awake()
    {
        GameTiles = GameObject.FindObjectsOfType<Tile>().ToList();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(IsPuzzleComplete)
        {
            // Debug.Log("Puzzle complete!");
        }

        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            var grassTiles = GameObject.FindObjectsOfType<Grass>();
            foreach(Grass tile in grassTiles)
            {
                tile.IsMowed = false;
                tile.gameObject.GetComponent<Renderer>().material.color = tile.TallColor;
            }
        }
	}

}
