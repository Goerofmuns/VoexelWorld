using UnityEngine;
using System;
using System.Collections;

public class MenuGUI : MonoBehaviour
{

    public Block[, ,] sdata;
    float[,] gdata;

    public int chunksize = 16;
    public int sliderloc;

    float scale;
    float power;
    float layer;
    int worldX = 512;
    int worldY = 128;
    int worldZ = 512;
    bool isGenerated = false;
    bool genisland;

    Texture2D worldtex;
    GameObject relay;

    void Start()
    {
        sdata = new Block[worldX, worldY, worldZ];
        gdata = new float[worldX, worldZ];
        worldtex = new Texture2D(worldX, worldZ);
    }

    void OnGUI()
    {
        GUI.DrawTexture(new Rect((Screen.width / 2) - 500, 70, 250, 250), worldtex);
        GUI.Label(new Rect(550, 70, 40, 20), scale.ToString());
        GUI.Label(new Rect(550, 90, 40, 20), power.ToString());
        genisland = GUI.Toggle(new Rect(450, 135, 150, 20), genisland, "Generate Island");
        scale = GUI.HorizontalSlider(new Rect(450, 70, 100, 20), scale, 1, 150.0f);
        power = GUI.HorizontalSlider(new Rect(450, 90, 100, 20), power, 0, 30);
        if (GUI.Button(new Rect(450, 110, 250, 25), "Generate Map"))
        {
            GenNoiseArray();
            if (genisland == true) { GenIsland(); }
            GenArray();
            GenImage();
            relay = GameObject.Find("Relay");
            relay.GetComponent<DataRelay>().data = sdata;
            isGenerated = true;

        }
        if (GUI.Button(new Rect((Screen.width / 2) - 250, Screen.height / 2 + 250, 500, 50), "Load Level"))
        {
            if (isGenerated)
            {
                Application.LoadLevel("3Dmain");
            }
            else
            {
                GenNoiseArray();
                if (genisland == true) { GenIsland(); }
                GenArray();
                relay = GameObject.Find("Relay");
                relay.GetComponent<DataRelay>().data = sdata;
                Application.LoadLevel("3Dmain");
            }
        }
    }

    void GenIsland()
    {
        for (int x = 0; x < worldX; x++)
        {
            for (int z = 0; z < worldZ; z++)
            {
                for (int y = 0; y < worldY; y++)
                {
                    float distanceX = ((worldX / 2) - x) * ((worldX / 2) - x);
                    float distanceZ = ((worldZ / 2) - z) * ((worldZ / 2) - z);

                    float distance = Mathf.Sqrt(distanceX + distanceZ);
                    distance = distance / 512;

                    gdata[x, z] -= distance;
                }
            }
        }
    }

    void GenArray()
    {
        for (int x = 0; x < worldX; x++)
        {
            for (int z = 0; z < worldZ; z++)
            {
                int stone = (int)(gdata[x, z] * 50);
                for (int y = 0; y < worldY; y++)
                {
                    if (y <= stone)
                    {
                        sdata[x, y, z] = new Block(1);
                    }
                    else if (y <= 5 + stone)
                    {
                        sdata[x, y, z] = new Block(2);
                    }
                }
            }
        }
    }

    void GenImage()
    {
        for (int x = 0; x < worldX; x++)
        {
            for (int z = 0; z < worldZ; z++)
            {
                worldtex.SetPixel(x, z, new Color(gdata[x, z], gdata[x, z], gdata[x, z]));
            }
        }
        worldtex.Apply();
    }

    void GenNoiseArray()
    {
        for (int x = 0; x < worldX; x++)
        {
            for (int z = 0; z < worldZ; z++)
            {
                gdata[x, z] = (float)(PerlinNoise(x, 0, z, scale, 1));
            }
        }
    }

    float PerlinNoise(int x, int y, int z, float scale, float power)
    {
        float rvalue;
        rvalue = Noise.GetOctaveNoise(x /scale, y/scale,  z/scale, 5);

        return rvalue;
    }

}