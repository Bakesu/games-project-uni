using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class MapManager : MonoBehaviour
{
    private static MapManager _instance;
    private Tilemap arenaTilemap;
    private Tilemap decorTilemap;

    [SerializeField]
    public List<TileData> tileDataList;

    public Dictionary<TileBase, TileData> dataFromTiles;
    public List<Vector3Int> possibleMoveList;
    public static MapManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MapManager>();
            }
            return _instance;
        }
    }
    private void Awake()
    {
        var tileMaps = gameObject.GetComponentsInChildren<Tilemap>();
        arenaTilemap = tileMaps[0];
        decorTilemap = tileMaps[1];

        dataFromTiles = new Dictionary<TileBase, TileData>();

        foreach (var tileData in tileDataList)
        {
            foreach (var tile in tileData.tiles)
            {
                dataFromTiles.Add(tile, tileData);
            }
        }
    }

    internal void ShowPossibleMoves(List<Vector3Int> possibleMoveList)
    {
        foreach (var possibleMove in possibleMoveList)
        {
            arenaTilemap.SetTileFlags(possibleMove, TileFlags.None);
            arenaTilemap.SetColor(possibleMove, Color.grey);
        }
        this.possibleMoveList = possibleMoveList;
    }

    //Find the shortest path between two points using manhatten distance
    //internal List<Vector3Int> FindPath(Vector3Int selectedUnitPos, Vector3Int selectedTilePos)
    //{
    //    var path = pathfinder.FindPath(selectedUnitPos, selectedTilePos);
    //    return path;
    //}

    public Tilemap ArenaTilemap
    {
        get
        {
            return arenaTilemap;
        }
    }

    public Tilemap DecorTilemap
    {
        get
        {
            return decorTilemap;
        }
    }
    
        internal void ClearPossibleMoves()
    {
        if (possibleMoveList != null)
        {
            foreach (var item in possibleMoveList)
            {
                arenaTilemap.SetTileFlags(item, TileFlags.None);
                arenaTilemap.SetColor(item, Color.white);
            }
        }
        this.possibleMoveList = null;
    }

}
