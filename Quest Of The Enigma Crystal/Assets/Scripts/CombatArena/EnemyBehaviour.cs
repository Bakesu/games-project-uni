using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using static EnemyBehaviour;
using static UnityEngine.UI.CanvasScaler;

public class EnemyBehaviour : MonoBehaviour
{
    private const int Radius = 20;
    public UnitType enemyType;
    public UnitManager unitManager;
    public Pathfinder pathFinder;
    private GameObject lowestHealthUnit;
    private static EnemyBehaviour _instance;
    private List<Vector3Int> targetOutsideMoveRange;
    private bool unitLocated = false;
    private Vector3Int targetToMoveTo;
    internal bool playingOutEnemyTurn;
    private int tileValue;
    private float bestMove;
    private Vector3Int pathToTarget;

    public static EnemyBehaviour Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<EnemyBehaviour>();
            }
            return _instance;
        }
    }

    public enum UnitType
    {
        Wizard,
        Archer,
        Knight
    }

    // Start is called before the first frame update
    void Start()
    {
        targetOutsideMoveRange = new List<Vector3Int>();
        pathFinder = new Pathfinder();
    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void OnEnemyTurn(Dictionary<Vector3Int, GameObject> unitsDictionary)
    {
        var enemyUnits = new Dictionary<Vector3Int, GameObject>();
        foreach (var unit in unitsDictionary)
        {
            // For each unit with the "enemy" tag, calculate the best possible move and execute it
            if (unit.Value.tag == "Enemy")
            {
                enemyUnits.Add(unit.Key, unit.Value);
            }
        }
        StartCoroutine(CalculateAllEnemyMoves(enemyUnits));
    }

    private IEnumerator CalculateAllEnemyMoves(Dictionary<Vector3Int, GameObject> enemyUnits)
    {
        bool firstMovingUnit = true;
        foreach (var unit in enemyUnits)
        {
            if (!firstMovingUnit)
            {
                yield return new WaitForSeconds(2f);
            }
            firstMovingUnit = false;
            Debug.Log(unit.Key);
            CalculateBestEnemyMove(unit);
        }
        playingOutEnemyTurn = false;
    }

    internal void CalculateBestEnemyMove(KeyValuePair<Vector3Int, GameObject> unitPair)
    {
        string[] splittedString = unitPair.Value.name.Split(char.Parse("("));
        string unitType = splittedString[0];
        var nearbyEnemies = unitManager.GetNearbyEnemies(unitPair.Key, unitPair.Value, unitPair.Value.GetComponent<Character>().attackDistance, false);
        var possibleMoves = pathFinder.CalculatePossibleMoves(unitPair.Key, unitPair.Value.GetComponent<Character>().moveDistance);
        MapManager.Instance.ShowPossibleMoves(nearbyEnemies);
        if (nearbyEnemies.Count != 0)
        {
            bestMove = 0f;
            foreach (var tile in nearbyEnemies)
            {
                // If tile contains an ally unit, continue
                if (unitManager.unitsDictionary.ContainsKey(tile))
                {
                    unitManager.unitsDictionary.TryGetValue(tile, out GameObject unitObject);
                    if (unitObject.tag == "Ally")
                    {
                        targetOutsideMoveRange.Add(tile);
                    }
                    foreach (var move in possibleMoves)
                    {
                        if (unitPair.Value.GetComponent<Character>().attackDistance == 1)
                        {
                            pathToTarget = unitPair.Key;
                            break;
                        }
                        var enemiesNearMove = unitManager.GetNearbyEnemies(move, unitPair.Value, unitPair.Value.GetComponent<Character>().attackDistance, false);
                        var distanceFromStartToNewTile = UnityEngine.Vector3.Distance(tile, move);
                        if (distanceFromStartToNewTile > bestMove && enemiesNearMove.Contains(tile))
                        {
                            bestMove = distanceFromStartToNewTile;
                            pathToTarget = move;
                            Debug.Log(bestMove);
                            Debug.Log(pathToTarget);
                        }
                    }
                    unitManager.MoveUnit(unitPair.Key, pathToTarget);
                    unitManager.selectedUnit = unitPair.Value;
                    StartCoroutine(waitForMove(unitType, nearbyEnemies[0]));
                    MapManager.Instance.ClearPossibleMoves();
                    return;
                }
            }
        }
        // If there are no attackable units in range, move to the tile closest to the lowest health ally unit in range
        MoveAndAttack(unitPair, unitType);
    }

    private void MoveAndAttack(KeyValuePair<Vector3Int, GameObject> unitPair, string unitType)
    {
        List<Vector3Int> nearbyEnemies;
        Debug.Log("no attack found");
        // Calculate attack from each tile
        var possibleMovesList = pathFinder.CalculatePossibleMoves(unitPair.Key, unitPair.Value.GetComponent<Character>().moveDistance);
        var unitAttackRange = unitPair.Value.GetComponent<Character>().attackDistance;
        var distanceToNearestTarget = 999f;
        var nearestTargetTile = new Vector3Int();
        foreach (var playerCharacter in unitManager.playerPartyDictionary)
        {
            var distanceBetweenUnits = UnityEngine.Vector3.Distance(unitPair.Key, playerCharacter.Key);
            if (distanceBetweenUnits < distanceToNearestTarget)
            {
                distanceToNearestTarget = distanceBetweenUnits;
                nearestTargetTile = playerCharacter.Key;
            }
        }
        Vector3Int pathToTarget = new Vector3Int();
        bestMove = 999f;
        Debug.Log(nearestTargetTile);
        var listOfMovesWithPossibleAttacks = new List<Vector3Int>();
        foreach (var possibleMove in possibleMovesList)
        {
            var tempList = unitManager.GetNearbyEnemies(possibleMove, unitPair.Value, unitPair.Value.GetComponent<Character>().attackDistance, false);
            if (tempList.Count != 0)
            {
                listOfMovesWithPossibleAttacks.Add(possibleMove);
            }
            if (listOfMovesWithPossibleAttacks.Count != 0) continue;

            distanceToNearestTarget = UnityEngine.Vector3.Distance(possibleMove, nearestTargetTile);
            if (distanceToNearestTarget < bestMove)
            {
                bestMove = distanceToNearestTarget;
                pathToTarget = possibleMove;
            }
        }
        if(listOfMovesWithPossibleAttacks.Count != 0)
        {
            bestMove = 999f;
            foreach (var move in listOfMovesWithPossibleAttacks)
            {
                var distanceFromStartToNewTile = UnityEngine.Vector3.Distance(unitPair.Key, move);
                if (distanceFromStartToNewTile < bestMove)
                {
                    bestMove = distanceFromStartToNewTile;
                    pathToTarget = move;
                    Debug.Log(bestMove);
                    Debug.Log(pathToTarget);
                }
            }
        }
        unitManager.MoveUnit(unitPair.Key, pathToTarget);
        MapManager.Instance.ClearPossibleMoves();
        nearbyEnemies = unitManager.GetNearbyEnemies(pathToTarget, unitPair.Value, unitPair.Value.GetComponent<Character>().attackDistance, false);
        if (nearbyEnemies.Count != 0)
        {
            unitManager.selectedUnit = unitPair.Value;
            StartCoroutine(waitForMove(unitType, nearbyEnemies[0]));
        }
    }

    private IEnumerator waitForMove(string unit, Vector3Int target)
    {
        yield return new WaitForSeconds(1);
        unitManager.AttackAction(unit, target);
    }


    ////Calculate the best possible move for enemy unit based on tile values
    //internal void CalculateEnemyBestMove()
    //{
    //    var tilesInMovementRange = pathFinder.CalculatePossibleMoves(targetToMoveTo, unitManager.selectedUnit.GetComponent<Character>().moveDistance);
    //    bool moveFound = false;
    //    //Create a scenario for each tile in movement range
    //    var scenario = new Scenario();
    //    foreach (var tile in tilesInMovementRange)
    //    {
    //        //Define the scenario value for each tile
    //        var tempScenario = DefineTileScenarioValue(tile);
    //        //If the scenario value is higher than the current scenario value, then it is a better tile
    //        if (tempScenario != null && tempScenario.scenarioValue > scenario.scenarioValue)
    //        {
    //            scenario = tempScenario;
    //            moveFound = true;
    //        }
    //        //handle the case for when there are two tiles with the same value and keep the one that uses less steps
    //        if (tempScenario.unitPosition != null && tempScenario.scenarioValue == scenario.scenarioValue)
    //        {
    //            //count the steps towards the closest enemy unit
    //            moveFound = true;
    //            var tempSteps = pathFinder.CalculatePossibleMoves(tempScenario.unitPosition, unitManager.selectedUnit.GetComponent<Character>().moveDistance).Count;
    //            var scenarioSteps = pathFinder.CalculatePossibleMoves(scenario.unitPosition, unitManager.selectedUnit.GetComponent<Character>().moveDistance).Count;

    //            if (tempSteps < scenarioSteps)
    //            {
    //                scenario = tempScenario;
    //            }
    //        }
    //        //If there are no attackable units in range, move to the tile closest to the lowest health ally unit in range
    //        if (tempScenario.unitPosition == null && !moveFound)
    //        {
    //            scenario = AttackLowestHealthStrategy();
    //        }


    //    }
    //    var TilesInAttackRange = pathFinder.CalculatePossibleMoves(targetToMoveTo, unitManager.selectedUnit.GetComponent<Character>().attackDistance);
    //    //var bestMove = pathFinder.GetBestMove(selectedUnitPosition.Value, nearbyEnemies);

    //}
    private Scenario AttackLowestHealthStrategy()
    {
        throw new NotImplementedException();

    }

    private Scenario DefineTileScenarioValue(Vector3Int tile)
    {
        Scenario attackScenario = AttackBasedOnUnitType();

        return attackScenario;



    }

    private Scenario AttackBasedOnUnitType()
    {
        switch (enemyType)
        {
            case UnitType.Knight:
                return MeleeAttackStrategy();
            case UnitType.Archer:
                return RangedAttackstrategy();
            case UnitType.Wizard:
                return RangedAttackstrategy();
            default:
                break;
        }
        return new Scenario();
    }

    private Scenario MeleeAttackStrategy()
    {
        throw new NotImplementedException();
    }

    private Scenario RangedAttackstrategy()
    {
        throw new NotImplementedException();
    }


}
