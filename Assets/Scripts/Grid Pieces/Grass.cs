using UnityEngine;
using System.Collections;

public class Grass : Tile
{

    public Color TallColor = Color.green;
    public Color ShortColor = Color.yellow;

    public bool IsMowed = false;

    // Use this for initialization
    void Start()
    {
        gameObject.GetComponent<Renderer>().material.color = TallColor;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Mow();
        }
    }

    public virtual void Mow()
    {
        if (!IsMowed)
        {
            gameObject.GetComponent<Renderer>().material.color = ShortColor;
            IsMowed = true;

            StatsTracker.GrassTilesMowed++;
        }
    }

    public override void Reset()
    {
        gameObject.GetComponent<Renderer>().material.color = TallColor;
        IsMowed = false;
    }
}
