using UnityEngine;
using System;
using System.Collections;

public class MenuGUI : MonoBehaviour
{

    public Block[,,] sdata;
    float[,] gdata;

    public int chunksize = 16;
    public int sliderloc;

    float scale;
    float power;
    float layer;
    int worldX = 512;
    int worldY = 64;
    int worldZ = 512;
    bool isGenerated = false;

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
        scale = GUI.HorizontalSlider(new Rect(450, 70, 100, 20), scale, 1, 100.0f);
        power = GUI.HorizontalSlider(new Rect(450, 90, 100, 20), power, 1, 5);
        if (GUI.Button(new Rect(450, 110, 250, 25),"Generate Map"))
        {
            GenNoiseArray();
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
                    int distanceX = ((worldX / 2) - x) * ((worldX / 2) - x);
                    int distanceZ = ((worldZ / 2) - z) * ((worldZ / 2) - z);

                    int distancetoCenter = (int)Mathf.Sqrt(distanceX + distanceZ);

                    if (distancetoCenter > 100)
                    {
                        sdata[x, y, z] = new Block(0);
                    }
                }
            }
        }
    }

    void GenNoiseArray()
    {
        for (int x = 0; x < worldX; x++)
        {
            for (int z = 0; z < worldZ; z++)
            {
                if (scale == 0) { scale = UnityEngine.Random.Range(1, 100); }
                if (power == 0) { power = UnityEngine.Random.Range(1, 5); }
                int stone = PerlinNoise(x, 0, z, scale, power, 1.2f);
                stone += PerlinNoise(x, 300, z, 20, 4, 0) + 10;
                int dirt = PerlinNoise(x, 100, z, 50, 3, 0) + 1;
                gdata[x, z] = stone;
                gdata[x, z] = gdata[x, z] / 256;
                for (int y = 0; y < worldY; y++)
                {
                    if (y <= stone)
                    {
                        sdata[x, y, z] = new Block(1);
                    }
                    else if (y <= dirt + stone)
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
                worldtex.SetPixel(x, z, new Color(gdata[x,z], gdata[x,z], gdata[x,z]));
            }
        }
        worldtex.Apply();
    }

    int PerlinNoise(int x, int y, int z, float scale, float height, float power)
    {
        float rvalue;
        rvalue = Noise.GetOctaveNoise(((double)x) / scale, ((double)y) / scale, ((double)z) / scale, 10);
        rvalue *= height;

        if (power != 0)
        {
            rvalue = Mathf.Pow(rvalue, power);
        }
        return (int)rvalue;
    }

}
