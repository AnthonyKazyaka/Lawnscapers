using UnityEngine;
using System.Collections;

public class Grass : Tile
{
    public Texture2D TallTexture;
    public Texture2D ShortTexture;

    public bool IsMowed = false;

    // Use this for initialization
    void Start()
    {
        gameObject.GetComponent<Renderer>().material.mainTexture = TallTexture;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Mow();
        }
    }

    public virtual void Mow()
    {
        if (!IsMowed)
        {
            gameObject.GetComponent<Renderer>().material.mainTexture = ShortTexture;
            IsMowed = true;

            StatsTracker.GrassTilesMowed++;
        }
    }

    public override void Reset()
    {
        gameObject.GetComponent<Renderer>().material.mainTexture = TallTexture;
        IsMowed = false;
    }
}
