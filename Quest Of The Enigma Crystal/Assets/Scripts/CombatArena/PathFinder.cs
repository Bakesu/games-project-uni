using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.UI.CanvasScaler;


public class Pathfinder
{
    public List<Vector3Int> CalculatePossibleMoves(Vector3Int selectedUnitPos, int unitMoveRange)
    {
        int x = selectedUnitPos.x;
        int y = selectedUnitPos.y;
        int z = 0; //Since we're on a 2d map
        List<Vector3Int> result = new List<Vector3Int>();
        Queue<Vector3Int> queue = new Queue<Vector3Int>();
        var arenaMapSize = MapManager.Instance.ArenaTilemap.size;

        var max_x = arenaMapSize.x - 1;
        var max_y = arenaMapSize.y - 1;

        bool[,] visited = new bool[arenaMapSize.x,arenaMapSize.y];
        int[,] distance = new int[arenaMapSize.x,arenaMapSize.y];

        queue.Enqueue(new Vector3Int(x, y, z));
        visited[x, y] = true;
        distance[x, y] = 0;



        while (queue.Count > 0)
        {
            Vector3Int currentVector = queue.Dequeue();
            int currentX = currentVector.x;
            int currentY = currentVector.y;

            // Define the four adjacent neighbors (up, down, left, right)
            int[] dxValues = { 0, 0, -1, 1 };
            int[] dyValues = { -1, 1, 0, 0 };

            for (int i = 0; i < dxValues.Length; i++)
            {
                int dx = dxValues[i];
                int dy = dyValues[i];

                int newX = currentX + dx;
                int newY = currentY + dy;

                if (newX < 0 || newX > max_x || newY < 0 || newY > max_y) continue; // Skip points outside the map boundaries
                if (visited[newX, newY]) continue; // Skip already visited points

                int newDistance = distance[currentX, currentY] + 1;

                if (newDistance > unitMoveRange) continue; // Skip points beyond the move range
                var selectedUnit = UnitManager.Instance.unitsDictionary[selectedUnitPos];
                if (IsValidMove(selectedUnit, new Vector3Int(newX, newY, z)))
                {
                    result.Add(new Vector3Int(newX, newY, z));
                    queue.Enqueue(new Vector3Int(newX, newY, z));
                    visited[newX, newY] = true;
                    distance[newX, newY] = newDistance;
                }
            }
        }

        return result;
    }

public bool IsValidMove(GameObject unitObject, Vector3Int selectedTilePos)
    {
        var arenaCellBounds = MapManager.Instance.ArenaTilemap.cellBounds;
        if (!arenaCellBounds.Contains(selectedTilePos)) return false;
        if (UnitManager.Instance.unitsDictionary.ContainsKey(selectedTilePos)) return false;
        return true;
    }

    private int GetManhattenDistance(Vector3Int startPos, Vector3Int endPos)
    {
        return Mathf.Abs(startPos.x - endPos.x) + Mathf.Abs(startPos.y - endPos.y);
    }

    public List<Vector3Int> GetMovePath(Vector3Int startPos, Vector3Int endPos)
    {
        List<Vector3Int> result = new List<Vector3Int>();
        
        int manhattenDistance = GetManhattenDistance(startPos, endPos);
        //Debug.Log(manhattenDistance);
        int xDistance = endPos.x - startPos.x;
        int yDistance = endPos.y - startPos.y;
        int absXDistance = Mathf.Abs(xDistance);
        Vector3Int prevPos = startPos;
        for (int i = 1; i < manhattenDistance + 1; i++)
        {
            if (absXDistance > 0)
            {
                var newPos = xDistance > 0 ? new Vector3Int(prevPos.x + 1, prevPos.y, prevPos.z) : new Vector3Int(prevPos.x - 1, prevPos.y, prevPos.z);
                result.Add(newPos);
                absXDistance -= 1;
                prevPos = newPos;
            } else
            {
                var newPos = yDistance > 0 ? new Vector3Int(prevPos.x, prevPos.y + 1, prevPos.z) : new Vector3Int(prevPos.x, prevPos.y - 1, prevPos.z);
                result.Add(newPos);
                prevPos = newPos;
            }
        }
        return result;
    }
}