using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Chunk : MonoBehaviour
{

    List<Vector3> verts = new List<Vector3>();
    List<int> tris = new List<int>();
    List<Vector2> uv = new List<Vector2>();

    float tUnit = 0.25f;
    
    Mesh mesh;
    MeshCollider col;
    
    public GameObject worldGO;
    World world;

    int facecount;
    public bool update;
    public int chunkSize = 16;
    public int chunkX;
    public int chunkY;
    public int chunkZ;

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        col = GetComponent<MeshCollider>();
        world = worldGO.GetComponent<World>();
        GenerateMesh();
    }

    void LateUpdate()
    {
        if (update)
        {
            GenerateMesh();
            update = false;
        }
    }

    void Cube(Vector2 texturePos)
    {
        tris.Add(facecount * 4);
        tris.Add(facecount * 4 + 1);
        tris.Add(facecount * 4 + 2);
        tris.Add(facecount * 4);
        tris.Add(facecount * 4 + 2);
        tris.Add(facecount * 4 + 3);

        uv.Add(new Vector2(tUnit * texturePos.x + tUnit, tUnit * texturePos.y));
        uv.Add(new Vector2(tUnit * texturePos.x + tUnit, tUnit * texturePos.y + tUnit));
        uv.Add(new Vector2(tUnit * texturePos.x, tUnit * texturePos.y + tUnit));
        uv.Add(new Vector2(tUnit * texturePos.x, tUnit * texturePos.y));

        facecount++;
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = verts.ToArray();
        mesh.uv = uv.ToArray();
        mesh.triangles = tris.ToArray();
        mesh.Optimize();
        mesh.RecalculateNormals();

        col.sharedMesh = null;
        col.sharedMesh = mesh;

        verts.Clear();
        uv.Clear();
        tris.Clear();

        facecount = 0;
    }

    public void GenerateMesh()
    {
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    if (BlockType(x, y, z) != 0)
                    {
                        if (BlockType(x, y + 1, z) == 0)
                        {
                            cubetop(x, y, z, BlockType(x, y, z));
                        }

                        if (BlockType(x, y - 1, z) == 0)
                        {
                            cubebot(x, y, z, BlockType(x, y, z));
                        }

                        if (BlockType(x + 1, y, z) == 0)
                        {
                            cubeeast(x, y, z, BlockType(x, y, z));
                        }

                        if (BlockType(x - 1, y, z) == 0)
                        {
                            cubewest(x, y, z, BlockType(x, y, z));
                        }

                        if (BlockType(x, y, z + 1) == 0)
                        {
                            cubenorth(x, y, z, BlockType(x, y, z));
                        }

                        if (BlockType(x, y, z - 1) == 0)
                        {
                            cubesouth(x, y, z, BlockType(x, y, z));
                        }
                    }
                }
            }
        }
        UpdateMesh();
    }
    
    byte BlockType(int x, int y, int z)
    {
        return world.Block(x + chunkX, y + chunkY, z + chunkZ).type;
    }

    Block block(int x, int y, int z)
    {
        return world.Block(x + chunkX, y + chunkY, z + chunkZ);
    }

    Vector2 tfunc(int x, int y, int z,bool isTop)
    {
      return block(x, y, z).tex();       
    }

    void cubetop(int x, int y, int z, byte block)
    {
        verts.Add(new Vector3(x, y, z + 1));
        verts.Add(new Vector3(x + 1, y, z + 1));
        verts.Add(new Vector3(x + 1, y, z));
        verts.Add(new Vector3(x, y, z));

        Vector2 texturePos = new Vector2(0, 0);

        texturePos = tfunc(x, y, z, true);

        Cube(texturePos);

    }

    void cubenorth(int x, int y, int z, byte block)
    {
        verts.Add(new Vector3(x + 1, y - 1, z + 1));
        verts.Add(new Vector3(x + 1, y, z + 1));
        verts.Add(new Vector3(x, y, z + 1));
        verts.Add(new Vector3(x, y - 1, z + 1));

        Vector2 texturePos = new Vector2(0, 0);

        texturePos = tfunc(x, y, z, false);

        Cube(texturePos);
    }

    void cubeeast(int x, int y, int z, byte block)
    {
        verts.Add(new Vector3(x + 1, y - 1, z));
        verts.Add(new Vector3(x + 1, y, z));
        verts.Add(new Vector3(x + 1, y, z + 1));
        verts.Add(new Vector3(x + 1, y - 1, z + 1));

        Vector2 texturePos = new Vector2(0, 0);

        texturePos = tfunc(x, y, z, false);

        Cube(texturePos);
    }

    void cubesouth(int x, int y, int z, byte block)
    {
        verts.Add(new Vector3(x, y - 1, z));
        verts.Add(new Vector3(x, y, z));
        verts.Add(new Vector3(x + 1, y, z));
        verts.Add(new Vector3(x + 1, y - 1, z));

        Vector2 texturePos = new Vector2(0, 0);

        texturePos = tfunc(x, y, z, false);

        Cube(texturePos);
    }

    void cubewest(int x, int y, int z, byte block)
    {
        verts.Add(new Vector3(x, y - 1, z + 1));
        verts.Add(new Vector3(x, y, z + 1));
        verts.Add(new Vector3(x, y, z));
        verts.Add(new Vector3(x, y - 1, z));

        Vector2 texturePos = new Vector2(0, 0);

        texturePos = tfunc(x, y, z, false);

        Cube(texturePos);
    }

    void cubebot(int x, int y, int z, byte block)
    {
        verts.Add(new Vector3(x, y - 1, z));
        verts.Add(new Vector3(x + 1, y - 1, z));
        verts.Add(new Vector3(x + 1, y - 1, z + 1));
        verts.Add(new Vector3(x, y - 1, z + 1));

        Vector2 texturePos = new Vector2(0, 0);

        texturePos = tfunc(x, y, z, false);

        Cube(texturePos);
    }
}