using UnityEngine;
using System.Collections;

public class ModifyTerrain : MonoBehaviour
{

    World world;
    GameObject cameraGO;

    public Texture2D reticule;
    Rect retRect = new Rect((Screen.width / 2) - 10, (Screen.height / 2) - 10, 20, 20);
    byte i = 1;
    string blockstring;

    void Start()
    {
        world = gameObject.GetComponent("World") as World;
        cameraGO = GameObject.FindGameObjectWithTag("MainCamera");
        Screen.showCursor = false;
        Screen.lockCursor = true;
        print(BlockTypes.types.Count);
    }

    void Update()
    {
        if (Input.GetKeyDown("f")) { i++; }
        if (i > BlockTypes.types.Count-1) { i = 1; }
        if (Input.GetMouseButtonDown(0))
        {
            ReplaceBlockCenter(5, 0);
        }

        if (Input.GetMouseButtonDown(1))
        {
            AddBlockCenter(5, i);
        }
        if (Input.GetKeyDown("e"))
        {
           GetBlockCenter(5).OnActivate(GetBlockCenter(5), Mathf.RoundToInt(GetLocCenter(5, false).x), Mathf.RoundToInt(GetLocCenter(5, false).y), Mathf.RoundToInt(GetLocCenter(5, false).z));
        }

        LoadChunks(GameObject.FindGameObjectWithTag("Player").transform.position, 48, 48);
    }

    void OnGUI()
    {
        GUI.Label(retRect, reticule);
        GUI.Box(new Rect(10, 10, 50, 50), i.ToString());
    }

    public Vector3 GetLocCenter(float range, bool isAdd)
    {
        Ray ray = new Ray(cameraGO.transform.position, cameraGO.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.distance < range)
            {
                Vector3 pos = hit.point;
                if (!isAdd){ pos += (hit.normal * -0.5f); }
                else if (isAdd) { pos += (hit.normal * 0.5f); }
                return pos;
            }
        }
        return new Vector3(0, 0, 0);
    }

    public Block GetBlockCenter(float range)
    {
        Ray ray = new Ray(cameraGO.transform.position, cameraGO.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.distance < range)
            {
                Vector3 pos = hit.point;
                pos += (hit.normal * -0.5f);
                int x = Mathf.RoundToInt(pos.x);
                int y = Mathf.RoundToInt(pos.y);
                int z = Mathf.RoundToInt(pos.z);
                return world.data[x, y, z];
            }
        }
        return new Block(0);
    }

    public void ReplaceBlockCenter(float range, byte block)
    {
        Ray ray = new Ray(cameraGO.transform.position, cameraGO.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.distance < range)
            {
                ReplaceBlockAt(hit, block);
            }
        }
    }

    public void AddBlockCenter(float range, byte block)
    {
        Ray ray = new Ray(cameraGO.transform.position, cameraGO.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.distance < range)
            {
                AddBlockAt(hit, block);
            }
            Debug.DrawLine(ray.origin, ray.origin + (ray.direction * hit.distance), Color.green, 2);
        }
    }

    public void ReplaceBlockCursor(byte block)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            ReplaceBlockAt(hit, block);
            Debug.DrawLine(ray.origin, ray.origin + (ray.direction * hit.distance), Color.green, 2);
        }
    }

    public void AddBlockCursor(byte block)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            AddBlockAt(hit, block);
            Debug.DrawLine(ray.origin, ray.origin + (ray.direction * hit.distance), Color.green, 2);
        }
    }

    public void ReplaceBlockAt(RaycastHit hit, byte block)
    {
        Vector3 position = hit.point;
        position += (hit.normal * -0.5f);

        SetBlockAt(position, block);
    }

    public void AddBlockAt(RaycastHit hit, byte block)
    {
        Vector3 position = hit.point;
        position += (hit.normal * 0.5f);

        SetBlockAt(position, block);
    }

    public void SetBlockAt(Vector3 position, byte block)
    {
        int x = Mathf.RoundToInt(position.x);
        int y = Mathf.RoundToInt(position.y);
        int z = Mathf.RoundToInt(position.z);

        SetBlockAt(x, y, z, block);
    }

    public void SetBlockAt(int x, int y, int z, byte block)
    {
        //print("Adding: " + x + "," + y + "," + z);

        world.data[x, y, z] = new Block(block);
        UpdateChunkAt(x, y, z);
    }

    public void UpdateChunkAt(int x, int y, int z)
    {
        int updateX = Mathf.FloorToInt(x / world.chunksize);
        int updateY = Mathf.FloorToInt(y / world.chunksize);
        int updateZ = Mathf.FloorToInt(z / world.chunksize);

        //print("Updating: " + updateX + "/" + updateY + "/" + updateZ);

        world.chunks[updateX, updateY, updateZ].update = true;

        if (x - (world.chunksize * updateX) == 0 && updateX != 0)
        {
            world.chunks[updateX - 1, updateY, updateZ].update = true;
        }

        if (x - (world.chunksize * updateX) == 15 && updateX != world.chunks.GetLength(0) - 1)
        {
            world.chunks[updateX + 1, updateY, updateZ].update = true;
        }

        if (y - (world.chunksize * updateY) == 0 && updateY != 0)
        {
            world.chunks[updateX, updateY - 1, updateZ].update = true;
        }

        if (y - (world.chunksize * updateY) == 15 && updateY != world.chunks.GetLength(1) - 1)
        {
            world.chunks[updateX, updateY + 1, updateZ].update = true;
        }

        if (z - (world.chunksize * updateZ) == 0 && updateZ != 0)
        {
            world.chunks[updateX, updateY, updateZ - 1].update = true;
        }

        if (z - (world.chunksize * updateZ) == 15 && updateZ != world.chunks.GetLength(2) - 1)
        {
            world.chunks[updateX, updateY, updateZ + 1].update = true;
        }
    }

    public void LoadChunks(Vector3 playerPos, float distToLoad, float distToUnload)
    {
        for (int x = 0; x < world.chunks.GetLength(0); x++)
        {
            for (int z = 0; z < world.chunks.GetLength(2); z++)
            {
                float dist = Vector2.Distance(new Vector2(x * world.chunksize, z * world.chunksize), new Vector2(playerPos.x, playerPos.z));

                if (dist < distToLoad)
                {
                    if (world.chunks[x, 0, z] == null) { world.GenColumn(x, z); }
                }

                else if (dist > distToUnload)
                {
                    if (world.chunks[x, 0, z] != null) { world.UnloadColumn(x, z); }
                }
            }
        }
    }
}