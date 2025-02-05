using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MouseController : MonoBehaviour
{

    [SerializeField]
    private MapManager mapManager;

    [SerializeField]
    private UnitManager unitManager;

    // Start is called before the first frame update
    void Start()
    {
        //returnButton = gameObject.GetComponent<Button>().onClick.AddListener(OnClick());
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        MoveCursor(mouseWorldPos);

        if (Input.GetMouseButtonUp(0))
        {
            //Prevents clicking through UI elements but unsure if it can have any problems?
            if (EventSystem.current.IsPointerOverGameObject()) { return; }
            Vector3Int selectedTilePos = mapManager.ArenaTilemap.WorldToCell(mouseWorldPos);
            var selectedTile = mapManager.ArenaTilemap.GetTile(selectedTilePos);
            //var printValue = mapManager.dataFromTiles[selectedTile];
            if (unitManager.unitInAction)
            {
                if (unitManager.ClickUnit(selectedTilePos))
                {
                    string[] splittedString = unitManager.selectedUnit.name.Split(char.Parse("("));
                    string unitType = splittedString[0];
                    if (unitManager.nearbyEnemies.Count != 0)
                    {
                        if (unitManager.nearbyEnemies[0] != null)
                        {
                            unitManager.unitsDictionary.TryGetValue(selectedTilePos, out GameObject unitObject);
                            if (unitObject.tag == "Enemy" && unitManager.getAttackMode() && unitManager.nearbyEnemies.Contains(selectedTilePos))
                            {
                                Debug.Log("clicked enemy");
                                unitManager.AttackAction(unitType, selectedTilePos);
                                unitManager.ActionCompleted();
                            }
                        }
                    }
                }
                else
                {
                    Debug.Log("clicked empty");
                    //unitManager.ActionCompleted();
                }
            }
            else
            {
                if (unitManager.ClickUnit(selectedTilePos))
                {
                    return;
                }
                if (unitManager.selectedUnit != null && unitManager.selectedUnitPosition != null)
                {
                    unitManager.MoveUnit((Vector3Int)unitManager.selectedUnitPosition, selectedTilePos);
                }
            }
            //Temporary until we have the action menu UI
        }
    }

    void MoveCursor(Vector3 mouseWorldPos)
    {
        Vector3Int coordinate = mapManager.ArenaTilemap.WorldToCell(mouseWorldPos);
        BoundsInt bounds = mapManager.ArenaTilemap.cellBounds;

        if (bounds.Contains(coordinate))
        {
            transform.position = mapManager.ArenaTilemap.GetCellCenterWorld(coordinate);
        }
    }

}

