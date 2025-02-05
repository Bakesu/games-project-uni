using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class TileData : ScriptableObject
{

    public TileBase[] tiles;

    public int G, H;
    public int F { get { return G + H; } }

    public bool isWalkable = true;

    public TileData previousTile;

    public Vector3Int position;
}
