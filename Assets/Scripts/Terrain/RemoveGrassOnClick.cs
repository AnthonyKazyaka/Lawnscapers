using UnityEngine;
using System.Collections;
using System;

public class RemoveGrassOnClick : MonoBehaviour
{

    public float Radius = 0.5f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //Debug.Log("mouse click!");

            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                //Debug.Log("raycast hit!");
                Terrain terrain = GameObject.FindObjectOfType<Terrain>();

                int TerrainDetailMapSize = terrain.terrainData.detailResolution;
                if (terrain.terrainData.size.x != terrain.terrainData.size.z)
                {
                    //Debug.Log("X and Y Size of terrain have to be the same (RemoveGrass.CS Line 43)");
                    return;
                }

                float PrPxSize = TerrainDetailMapSize / terrain.terrainData.size.x;

                Vector3 TexturePoint3D = hit.point - terrain.transform.position;
                TexturePoint3D = TexturePoint3D * PrPxSize;

                //Debug.Log(TexturePoint3D);

                float[] xymaxmin = new float[4];
                xymaxmin[0] = TexturePoint3D.z + Radius;
                xymaxmin[1] = TexturePoint3D.z - Radius;
                xymaxmin[2] = TexturePoint3D.x + Radius;
                xymaxmin[3] = TexturePoint3D.x - Radius;


                int[,] map = terrain.terrainData.GetDetailLayer(0, 0, terrain.terrainData.detailWidth, terrain.terrainData.detailHeight, 0);

                for (int y = 0; y < terrain.terrainData.detailHeight; y++)
                {
                    for (int x = 0; x < terrain.terrainData.detailWidth; x++)
                    {
                        float distance = (float)Math.Sqrt(Math.Pow(TexturePoint3D.x - y, 2) + Math.Pow(TexturePoint3D.z - x, 2));
                        //if (xymaxmin[0] > x && xymaxmin[1] < x && xymaxmin[2] > y && xymaxmin[3] < y)
                        //{
                        //    map[x, y] = 0;
                        //}

                        if(distance <= Radius)
                        {
                            map[x, y] = 0;
                        }
                    }
                }
                terrain.terrainData.SetDetailLayer(0, 0, 0, map);
            }
        }
    }
}
