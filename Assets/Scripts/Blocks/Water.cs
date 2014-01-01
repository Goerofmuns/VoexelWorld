using UnityEngine;

public class Water : BlockType
{
    public bool isSolid(Block block) { return false; }
    public Vector2 tex(Block block) {return new Vector2(3, 0);}
    public GameObject worldGet() { return GameObject.FindGameObjectWithTag("world"); }
    public void OnActivate(Block block, int x, int y, int z) 
    {
        World world = worldGet().GetComponent<World>();
        world.data[x + 1, y, z] = new Block(4);
        Chunk chunk = world.chunks[x / 16 , y / 16, z / 16];
        chunk.update = true;
    }
}
