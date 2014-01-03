using UnityEngine;
using System.Collections;

public class World : MonoBehaviour
{

    public GameObject chunk;
    public Chunk[, ,] chunks;

    public Block[, ,] data;

    public int chunksize = 16;
    int worldX = 512;
    int worldY = 128;
    int worldZ = 512;

    void Start()
    {
        data = GameObject.Find("Relay").GetComponent<DataRelay>().data;

        chunks = new Chunk[Mathf.FloorToInt(worldX / chunksize),
            Mathf.FloorToInt(worldY / chunksize),
            Mathf.FloorToInt(worldZ / chunksize)];
    }

    public Block Block(int x, int y, int z)
    {
        if (x >= worldX || x < 0 || y >= worldY || y < 0 || z >= worldZ || z < 0)
        {
            return new Block(1);
        }
        return data[x, y, z];
    }

    public void GenColumn(int x, int z)
    {

        for (int y = 0; y < chunks.GetLength(1); y++)
        {

            GameObject newChunk = Instantiate(chunk, new Vector3(x * chunksize - 0.5f, y * chunksize + 0.5f, z * chunksize - 0.5f), new Quaternion(0, 0, 0, 0)) as GameObject;

            chunks[x, y, z] = newChunk.GetComponent<Chunk>() as Chunk;
            chunks[x, y, z].worldGO = gameObject;
            chunks[x, y, z].chunkSize = chunksize;
            chunks[x, y, z].chunkX = x * chunksize;
            chunks[x, y, z].chunkY = y * chunksize;
            chunks[x, y, z].chunkZ = z * chunksize;


        }
    }

    public void UnloadColumn(int x, int z)
    {
        for (int y = 0; y < chunks.GetLength(1); y++)
        {
            Object.Destroy(chunks[x, y, z].gameObject);
        }
    }

}