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
    int octaves;
    float layer;
    int worldX = 512;
    int worldY = 128;
    int worldZ = 512;
    bool isGenerated = false;
    bool genisland;
    bool domodulo;

    Texture2D worldtex;
    public Texture2D logotex;
    GameObject relay;

    void Start()
    {
        sdata = new Block[worldX, worldY, worldZ];
        gdata = new float[worldX, worldZ];
        worldtex = new Texture2D(worldX, worldZ);
    }

    void OnGUI()
    {
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), logotex); 
        GUI.DrawTexture(new Rect((Screen.width / 2) - 500, 70, 250, 250), worldtex);
        GUI.Label(new Rect(550, 70, 40, 20), scale.ToString());
        GUI.Label(new Rect(550, 90, 40, 20), octaves.ToString());
        scale = GUI.HorizontalSlider(new Rect(450, 70, 100, 20), scale, 1, 150.0f);
        octaves = (int)GUI.HorizontalSlider(new Rect(450, 90, 100, 20), octaves, 0, 5);
        domodulo = GUI.Toggle(new Rect(450, 130, 100, 20), domodulo, "FUNKYMODE");
        if (GUI.Button(new Rect(450, 110, 250, 25), "Generate Map"))
        {
            GenNoiseArray();
            GenIsland();
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
                GenIsland();
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
                    float distanceX = ((worldX / 2) - x) * ((worldX / 2) - x);
                    float distanceZ = ((worldZ / 2) - z) * ((worldZ / 2) - z);

                    float distance = Mathf.Sqrt(distanceX + distanceZ);
                    distance = distance / 256;

                    gdata[x, z] -= distance;
                    if (domodulo)
                    {
                        gdata[x, z] %= distance;
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
                int stone = (int)(gdata[x, z] * 80);
                for (int y = 0; y < worldY; y++)
                {
                    if (y <= stone)
                    {
                        sdata[x, y, z] = new Block(1);
                    }
                    else if (y <= stone && y > 20)
                    {
                        sdata[x, y, z] = new Block(1);
                    }
                    else if (y <= 2 + stone && y <= 3)
                    {
                        sdata[x, y, z] = new Block(3);
                    }
                    else if (y <= 3 + stone && y > 3)
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
                gdata[x, z] = (float)(PerlinNoise(x, z, 0, scale, octaves));
            }
        }
    }

    float PerlinNoise(int x, int y, int z, float scale, int octaves)
    {
        float rvalue;
        rvalue = Noise.GetOctaveNoise(x /scale, y/scale,  z/scale, octaves);

        return rvalue;
    }

}