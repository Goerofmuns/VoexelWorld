using UnityEngine;
using System.Collections.Generic;

public class Air : BlockType
{
    public readonly byte type = 0;
    public bool isSolid(Block block) { return false; }
    public Vector2 tex(Block block) { return new Vector2(0, 0); }
    public void OnActivate(Block block, int x, int y, int z) { }
}

public class Dirt : BlockType
{
    public readonly byte type = 1;
    public bool isSolid(Block block) { return true; }
    public Vector2 tex(Block block) { return new Vector2(1, 1); }
    public void OnActivate(Block block, int x, int y, int z) { }
}

public class Stone : BlockType
{
    public readonly byte type = 2;
    public bool isSolid(Block block) { return true; }
    public Vector2 tex(Block block) { return new Vector2(0, 1); }
    public void OnActivate(Block block, int x, int y, int z) { }
}

public class Sand : BlockType
{
    public bool isSolid(Block block) { return true; }
    public Vector2 tex(Block block) { return new Vector2(2, 0); }
    public void OnActivate(Block block, int x, int y, int z) { }
}

/// <summary>
/// Block base struct.
/// </summary>

public struct Block
{
    public readonly byte type;

    public Block(byte newType)
    {
        type = newType;
    }

    public bool isSolid()
    {
        return BlockTypes.GetBlockType(type).isSolid(this);
    }

    public Vector2 tex() { return BlockTypes.GetBlockType(type).tex(this); }

    public void OnActivate(Block block, int x, int y, int z) { BlockTypes.GetBlockType(type).OnActivate(this, x, y, z); }

}

/// <summary>
/// Block interface.
/// </summary>
public interface BlockType
{
    bool isSolid(Block block);
    Vector2 tex(Block block);
    void OnActivate(Block block, int x, int y, int z);
}

/// <summary>
/// Block types class. Use GetBlockType(type);
/// </summary>
static class BlockTypes
{
    public static List<BlockType> types = new List<BlockType> { new Air(), new Stone(), new Dirt(), new Sand(), new Water() };

    public static BlockType GetBlockType(int type)
    {
        return types[type];
    }
}
