using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Scenario
{
    public float scenarioValue;
    public Vector3Int unitPosition;
    public Vector3Int targetTile;

    public Scenario(float scenarioValue, Vector3Int unitPosition, Vector3Int targetTile)
    {
        this.scenarioValue = scenarioValue;
        this.unitPosition = unitPosition;
        this.targetTile = targetTile;
    }

    public Scenario()
    {
        this.scenarioValue = -1000;
        this.unitPosition = new Vector3Int(-1, -1, -1);
        this.targetTile = new Vector3Int(-1, -1, -1);
    }
}
