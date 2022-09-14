using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;

public class PuzzleGenerator : MonoBehaviour
{
    private int _width = 4;
    public int Width = 4;
    private int _height = 4;
    public int Height = 4;

    public string GeneratedName = "Puzzle Tiles";

    void Awake()
    {
        SetDimensionsEqual();
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if(_width != Width || _height != Height)
        {
            GeneratePuzzle();
        }
	}

    public void GeneratePuzzle()
    {
        Debug.Log("Regenerating puzzle");
        
        _width = Width;
        _height = Height;

        Grass grassPrefab = ((GameObject)Resources.Load("Prefabs/Tiles/Grass")).GetComponent<Grass>();
        Border borderPrefab = ((GameObject)Resources.Load("Prefabs/Tiles/Border")).GetComponent<Border>();

        List<Tile> tileChildren = GameObject.FindObjectsOfType<Tile>().Where(x => x.transform.parent.name == GeneratedName).ToList();

        foreach (var tileChild in tileChildren)
        {
            GameObject.DestroyImmediate(tileChild.gameObject);
        }

        for (int i = 1; i <= Width; i++)
        {
            for (int j = 1; j <= Height; j++)
            {
                Vector3 newPosition = new Vector3(i - Width / 2.0f, j - Height / 2.0f, 0);

                if (i == 1 || i == Width)
                {
                    int direction = (i - Width / 2.0f) > 0 ? 1 : -1;
                    Vector3 borderPosition = newPosition;
                    borderPosition.x += direction * (grassPrefab.transform.localScale.x / 2.0f + borderPrefab.transform.localScale.x / 2.0f);
                    borderPosition.z = -0.5f;

                    GenerateTilePrefab(borderPrefab, borderPosition, Quaternion.AngleAxis(0.0f, Vector3.forward));
                }
                
                if (j == 1 || j == Height)
                {
                    int direction = (j - Height / 2.0f) > 0 ? 1 : -1;
                    Vector3 borderPosition = newPosition;
                    borderPosition.y += direction * (grassPrefab.transform.localScale.y / 2.0f + borderPrefab.transform.localScale.x / 2.0f);
                    borderPosition.z = -0.5f;

                    GenerateTilePrefab(borderPrefab, borderPosition, Quaternion.AngleAxis(90.0f, Vector3.forward));
                }

                GenerateTilePrefab(grassPrefab, newPosition, grassPrefab.transform.rotation);
            }
        }
    }

    public bool HaveDimensionsChanged()
    {
        return HasHeightChanged() || HasWidthChanged();
    }

    public bool HasHeightChanged()
    {
        return (_width != Width);
    }

    public bool HasWidthChanged()
    {
        return (_height != Height);
    }

    private Tile GenerateTilePrefab(Tile prefab, Vector3 position, Quaternion rotation)
    {
        Tile tile = (Tile)GameObject.Instantiate(prefab, position, rotation);
        tile.transform.SetParent(GameObject.Find(GeneratedName).transform);

        return tile;
    }

    public void SetDimensionsEqual()
    {
        Height = _height;
        Width = _width;
    }

}
