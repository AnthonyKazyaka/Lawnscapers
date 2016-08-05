using UnityEngine;
using System.Collections;
using System;

public class PuzzleGenerator : MonoBehaviour
{
    private int _width = 4;
    public int Width = 4;
    private int _height = 4;
    public int Height = 4;

	// Use this for initialization
	void Start () {
	
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
        Grass grassPrefab = (Grass)Resources.Load("Prefabs/Tiles/Grass");

        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                Vector3 newPosition = new Vector3(i - Width / 2.0f, j - Height / 2.0f, 0);
                Grass grass = (Grass)GameObject.Instantiate(grassPrefab, newPosition, grassPrefab.transform.rotation);
                grass.transform.SetParent(GameObject.Find("Test Tiles").transform);
            }
        }
    }
}
