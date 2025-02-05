using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using static UnityEngine.UI.CanvasScaler;

public class UnitManager : MonoBehaviour
{
    [SerializeField]
    private MapManager mapManager;

    [SerializeField]
    private GameObject apprenticePrefab;
    [SerializeField]
    private GameObject archerPrefab;

    [SerializeField]
    private Canvas UiCanvas;

    [SerializeField]
    private AudioClip audioMagic;
    [SerializeField]
    private AudioClip audioArrow;

    private static UnitManager _instance;
    private Pathfinder pathFinder;

    internal GameObject movingUnit; 
    internal GameObject selectedUnit;
    internal Vector3Int? selectedUnitPosition;
    internal bool unitInAction = false;
    private bool attackMode = false;


    private List<Vector3Int> movePath = new List<Vector3Int>();
    internal List<Vector3Int> nearbyEnemies = new List<Vector3Int>();
    internal Dictionary<Vector3Int, GameObject> playerPartyDictionary = new Dictionary<Vector3Int, GameObject>();
    internal Dictionary<Vector3Int, GameObject> unitsDictionary = new Dictionary<Vector3Int, GameObject>();




    public static UnitManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UnitManager>();
            }
            return _instance;
        }
    }

    // Singleton pattern to ensure only one instance of UnitManager exists
    void Start()
    {
        pathFinder = new Pathfinder();

        List<GameObject> friendlyUnits = WizardDataManager.instance.RosterUnits;
        for (int i = 0; i < friendlyUnits.Count; i++)
        {
            friendlyUnits[i].tag = "Ally";
            friendlyUnits[i].transform.position = mapManager.ArenaTilemap.GetCellCenterWorld(new Vector3Int(1, 1+i, 0));
            unitsDictionary.Add(new Vector3Int(1, 1 + i, 0), friendlyUnits[i]);
            playerPartyDictionary.Add(new Vector3Int(1, 1 + i, 0), friendlyUnits[i]);
        }   

        List<GameObject> enemyUnits = WizardDataManager.instance.EnemyUnits;
        for (int i = 0; i < enemyUnits.Count; i++)
        {
            enemyUnits[i].GetComponent<SpriteRenderer>().color = new Color(1f, 0.3f, 0.3f);
            enemyUnits[i].tag = "Enemy";
            enemyUnits[i].transform.position = mapManager.ArenaTilemap.GetCellCenterWorld(new Vector3Int(16, 1+i, 0)); //TODO: hardcoded to y= 12
            unitsDictionary.Add(new Vector3Int(16, 1 + i, 0), enemyUnits[i]);
        }

        // Make enemies face player on spawn
        foreach (Vector3Int key in unitsDictionary.Keys)
        {
            unitsDictionary.TryGetValue(key, out GameObject unitGameObject);
            if(unitGameObject.tag == "Enemy")
            {
                unitGameObject.GetComponent<SpriteRenderer>().flipX = !unitGameObject.GetComponent<SpriteRenderer>().flipX;
            }
        }
    }

    // Currently only used for handling movement of units
    void Update()
    {
        //TODO: Meget hacket måde at sørge for de bliver ved med at være røde
        var enemyUnits = WizardDataManager.instance.EnemyUnits;
        for (int i = 0; i < enemyUnits.Count; i++)
        {
            if (enemyUnits[i] == null) continue;
            enemyUnits[i].GetComponent<SpriteRenderer>().color = new Color(1f, 0.5f, 0.5f);
        }

        if (movePath.Count == 1)
        {
            MoveAlongPath();
            
            
        }
        else if (movePath.Count > 0)
        {

            MoveAlongPath();
        }
        else if(movingUnit != null && movePath.Count == 0)
        {
            if (movingUnit.tag == "Ally") ShowActionMenu();
            movingUnit = null;

        }
    }
    //Manages when a unit is clicked is called in the MouseController
    internal bool ClickUnit(Vector3Int selectedTilePos)
    {
        if(unitInAction)
        {
            return true;
        }
        unitsDictionary.TryGetValue(selectedTilePos, out GameObject unitObject);
        if (unitObject == null) return false;
        if (unitObject == selectedUnit)
        {
            DeselectUnit();
            return true;
        }
        if (unitObject.GetComponent<Character>().hasMoved) return false; // Ikke sikker p� om den skal return true eller false
        if (unitObject.tag == "Enemy") return true;
        if (selectedUnit != null) DeselectUnit();
        SelectUnit(unitObject, selectedTilePos);
        return true;
    }
    //Manages when a unit is selected and shows possible moves
    private void SelectUnit(GameObject unitObject, Vector3Int selectedTilePos)
    {
        var unitInfo = unitObject.GetComponent<Character>();
        var numberOfMoves = unitInfo.moveDistance;

        selectedUnit = unitObject;
        selectedUnitPosition = selectedTilePos;
        selectedUnit.GetComponentInChildren<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f);

        var listOfPossibleMoves = pathFinder.CalculatePossibleMoves(selectedTilePos, numberOfMoves);
        if (!selectedUnit.GetComponent<Character>().hasMoved)
        {
            mapManager.ShowPossibleMoves(listOfPossibleMoves);
        }
    }
    //Called whenever we return from any action to the main board
    public void DeselectUnit()
    {
        selectedUnit.GetComponentInChildren<SpriteRenderer>().color = Color.white;
        mapManager.ClearPossibleMoves();
        selectedUnit = null;
        selectedUnitPosition = null;
    }

    //An alternative to deselect unit which is called when actions are followed through
    internal void ActionCompleted()
    {
        unitInAction = false;
        HideActionMenu();
        mapManager.ClearPossibleMoves();
        selectedUnit = null;
        selectedUnitPosition = null;
    }

    //Called when a unit is moved and handles the visual movement as well
    internal void MoveUnit(Vector3Int originTilePos, Vector3Int targetTilePos)
    {
        // Try to get the unit at the origin position
        unitsDictionary.TryGetValue(originTilePos, out GameObject unitObject);
        if (unitObject == null) return;

        // Check if the target position is a valid move
        if (MapManager.Instance.possibleMoveList.Contains(targetTilePos) || unitObject.tag == "Enemy")
        {
            // Calculate the path to the target position
            movePath = pathFinder.GetMovePath(originTilePos, targetTilePos);
            movingUnit = unitObject;

            // Update the unit's position in the dictionary
            Util.RenameKey(unitsDictionary, originTilePos, targetTilePos);
            if (unitObject.tag == "Ally")
            {
                Util.RenameKey(playerPartyDictionary, originTilePos, targetTilePos);
            }

            // Mark the unit as having moved and clear possible moves
            movingUnit.GetComponent<Character>().hasMoved = true;
            mapManager.ClearPossibleMoves();
        }
        else if (unitObject.tag == "Ally")
        {
            DeselectUnit();
        }
    }

    //Called when a unit has moved and "fight" has been clicked by the player and shows the possible targets for the unit - further use could also support healing and support spells
    internal void ShowUnitsInRange(GameObject unitObject)
    {
        int attackRadius;
        
        attackRadius = unitObject.GetComponent<Character>().attackDistance;
        Vector3Int unitPos = mapManager.ArenaTilemap.WorldToCell(unitObject.transform.position);
        nearbyEnemies = GetNearbyEnemies(unitPos, unitObject, attackRadius, false);
    }

    
    //Help method for ShowUnitsInRange which returns a list of nearby enemies
    public List<Vector3Int> GetNearbyEnemies(Vector3Int unitPos, GameObject selectedUnit, int radius, bool count)
    {
        List<Vector3Int> nearbyEnemies = new List<Vector3Int>();

        //If the unit is an enemy, we want to find nearby allies instead
        //selectedUnit = unitsDictionary[unitPos];
        if (selectedUnit.tag == "Enemy")
        {
            GameObject[] allies = GameObject.FindGameObjectsWithTag("Ally");

            foreach (GameObject ally in allies)
            {
                Vector3Int allyPos = mapManager.ArenaTilemap.WorldToCell(ally.transform.position);
                int distanceX = Mathf.Abs(unitPos.x - allyPos.x);
                int distanceY = Mathf.Abs(unitPos.y - allyPos.y);

                if (distanceX + distanceY <= radius)
                {
                    nearbyEnemies.Add(allyPos);
                }
            }
            return nearbyEnemies;
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            Vector3Int enemyPos = mapManager.ArenaTilemap.WorldToCell(enemy.transform.position);
            int distanceX = Mathf.Abs(unitPos.x - enemyPos.x);
            int distanceY = Mathf.Abs(unitPos.y - enemyPos.y);

            if (distanceX+distanceY <= radius)
            {
                nearbyEnemies.Add(enemyPos);
            }
        }
        if (!count)
        {
            mapManager.ShowPossibleMoves(nearbyEnemies);
        }
        return nearbyEnemies;
    }



    // Calculates the path the unit should take and moves it along the path
    private void MoveAlongPath()
    {
        var step = 5 * Time.deltaTime;
        movingUnit.transform.position = Vector3.MoveTowards(movingUnit.transform.position, mapManager.ArenaTilemap.GetCellCenterWorld(movePath[0]), step);

        if (Vector2.Distance(movingUnit.transform.position, mapManager.ArenaTilemap.GetCellCenterWorld(movePath[0])) < 0.001f)
        {
            movingUnit.transform.position = mapManager.ArenaTilemap.GetCellCenterWorld(movePath[0]);
            movePath.RemoveAt(0);

        }

    }

    internal void AttackAction(String unitType, Vector3Int enemyUnitPos)
    {
        // Play the appropriate attack animation and sound based on unit type
        if (unitType == "Archer")
        {
            StartCoroutine(PlayAttackAnimation("archer_attack_animation", enemyUnitPos));
            AudioSource audio = GetComponent<AudioSource>();
            audio.clip = audioArrow;
            audio.PlayDelayed(0.38f);
        }
        if (unitType == "Apprentice")
        {
            StartCoroutine(PlayAttackAnimation("apprentice_attack", enemyUnitPos));
            AudioSource audio = GetComponent<AudioSource>();
            audio.clip = audioMagic;
            audio.PlayDelayed(0.38f);
        }
        if (unitType == "Arbalist")
        {
            StartCoroutine(PlayAttackAnimation("arbalist_attack_animation", enemyUnitPos));
            AudioSource audio = GetComponent<AudioSource>();
            audio.clip = audioArrow;
            audio.PlayDelayed(0.38f);
        }
        if (unitType == "Pikeman")
        {
            StartCoroutine(PlayAttackAnimation("pikeman_attack_animation", enemyUnitPos));
            GetComponent<AudioSource>().PlayDelayed(0.38f);
        }
        if (unitType == "Infantryman")
        {
            StartCoroutine(PlayAttackAnimation("infantryman_attack", enemyUnitPos));
            GetComponent<AudioSource>().PlayDelayed(0.38f);
        }
        if (unitType == "Paladin")
        {
            StartCoroutine(PlayAttackAnimation("paladin_attack_animation", enemyUnitPos));
            GetComponent<AudioSource>().PlayDelayed(0.38f);
        }
        if (unitType == "Champion")
        {
            StartCoroutine(PlayAttackAnimation("champion_attack_animation", enemyUnitPos));
            GetComponent<AudioSource>().PlayDelayed(0.38f);

        }
        if (unitType == "Enchanter")
        {
            StartCoroutine(PlayAttackAnimation("enchanter_attack_animation", enemyUnitPos));
            AudioSource audio = GetComponent<AudioSource>();
            audio.clip = audioMagic;
            audio.PlayDelayed(0.38f);
        }
        if (unitType == "Cleric")
        {
            StartCoroutine(PlayAttackAnimation("cleric_attack_animation", enemyUnitPos));
            AudioSource audio = GetComponent<AudioSource>();
            audio.clip = audioMagic;
            audio.PlayDelayed(0.38f);
        }
        if (unitType == "HighPriest")
        {
            StartCoroutine(PlayAttackAnimation("highpriest_attack_animation", enemyUnitPos));
            AudioSource audio = GetComponent<AudioSource>();
            audio.clip = audioMagic;
            audio.PlayDelayed(0.38f);
        }
        if (unitType == "Revenant")
        {
            StartCoroutine(PlayAttackAnimation("revenant_attack_animation", enemyUnitPos));
            GetComponent<AudioSource>().PlayDelayed(0.38f);
        }
        if (unitType == "Warlock")
        {
            StartCoroutine(PlayAttackAnimation("warlock_attack_animation", enemyUnitPos));
            AudioSource audio = GetComponent<AudioSource>();
            audio.clip = audioMagic;
            audio.PlayDelayed(0.38f);
        }
        if (unitType == "Wolf")
        {
            StartCoroutine(PlayAttackAnimation("wolf_attack_animation", enemyUnitPos));
        }
        if (unitType == "Orc")
        {
            StartCoroutine(PlayAttackAnimation("orc_attack_animation", enemyUnitPos));
        }

        GetComponent<AudioSource>().PlayDelayed(0.38f);
    }


    IEnumerator PlayAttackAnimation(string animationName, Vector3Int enemyUnitPos)
    {
        // Get the unit type and damage
        string[] splittedString = selectedUnit.name.Split(char.Parse("("));
        string unitType = splittedString[0];
        float damage = selectedUnit.GetComponent<Character>().damageDealt;

        // Determine the direction to face based on the target position
        GameObject animatingUnit = selectedUnit;
        Vector3Int selectedUnitPos = Util.GetKey(unitsDictionary, selectedUnit);
        if (selectedUnitPos.x > enemyUnitPos.x && selectedUnit.tag == "Ally")
        {
            if(unitType == "Cleric")
            {
                selectedUnit.GetComponent<SpriteRenderer>().flipX = false;
            } else
            {
                selectedUnit.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
        if (selectedUnitPos.x < enemyUnitPos.x && selectedUnit.tag == "Enemy")
        {
            if (unitType == "Cleric")
            {
                selectedUnit.GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                selectedUnit.GetComponent<SpriteRenderer>().flipX = false;
            }
        }

        // Play the attack animation
        selectedUnit.GetComponent<Character>().ChangeAnimationState(animationName);
        yield return new WaitForSeconds(0.6f); // TODO: Hardcoded animation time - adjust as needed

        // Apply damage to the target unit
        DamageUnit(enemyUnitPos, damage);
        yield return new WaitForSeconds(1.2f);

        // Reset the unit's direction
        if(unitType == "Cleric")
        {
            animatingUnit.GetComponent<SpriteRenderer>().flipX = true;
        } 
        else
        {
            if (animatingUnit.tag == "Ally") animatingUnit.GetComponent<SpriteRenderer>().flipX = false;
            else if (animatingUnit.tag == "Enemy") animatingUnit.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    //Deal the damage to the enemy unit and remove it from the dictionary if it dies
private void DamageUnit(Vector3Int enemyUnitPos, float damage)
    {
        // Try to get the unit at the target position
        unitsDictionary.TryGetValue(enemyUnitPos, out GameObject unitObject);
        unitObject.GetComponent<Character>().TakeDamage((int)damage);

        // Check if the unit is still active
        if (!unitObject.activeSelf)
        {
            Destroy(unitObject);
            unitsDictionary.Remove(enemyUnitPos);
        }

        // Count the remaining ally and enemy units
        int allyCount = 0;
        int enemyCount = 0;
        foreach (Vector3Int key in unitsDictionary.Keys)
        {
            unitsDictionary.TryGetValue(key, out GameObject unitGameObject);
            if(unitGameObject.tag == "Ally")
            {
                allyCount++;
            }
            if(unitGameObject.tag == "Enemy")
            {
                enemyCount++;
            }
        }

        // Check for battle outcome
        if(enemyCount == 0)
        {
            UiCanvas.transform.Find("WonBattleUI").gameObject.SetActive(true);
            UiCanvas.transform.Find("WonBattleUI").transform.Find("WonBattleUI").Find("Button").GetComponent<Button>().onClick.AddListener(() => UnitManager.Instance.BattleWon());
        } else if(allyCount == 0)
        {
            UiCanvas.transform.Find("LostBattleUI").gameObject.SetActive(true);
            UiCanvas.transform.Find("LostBattleUI").transform.Find("LostBattleUI").Find("Button").GetComponent<Button>().onClick.AddListener(() => {
                foreach (Vector3Int key in unitsDictionary.Keys)
                {
                    unitsDictionary.TryGetValue(key, out GameObject unitGameObject);
                    if (unitGameObject.tag == "Ally")
                    {
                        WizardDataManager.instance.removeWizardUnit(unitGameObject);
                        Destroy(unitGameObject);
                    }
                }
                WizardDataManager.instance.removeAllEnemyUnits();
                WizardDataManager.instance.removeAllRosterUnits();
                WizardDataManager.instance.addWizardUnit(Instantiate(WizardDataManager.instance.Pikeman, WizardDataManager.instance.UnitsParentGameObject.transform));
                WizardDataManager.instance.addWizardUnit(Instantiate(WizardDataManager.instance.Pikeman, WizardDataManager.instance.UnitsParentGameObject.transform));
                WizardDataManager.instance.playerGold = 50;
                WizardDataManager.instance.overWorldData = new OverWorldData("Layer 0", "Node 0", new Vector2(0, 0));
                foreach (Transform child in WizardDataManager.instance.EnemyUnitsParentGameObject.transform)
                {
                    child.GetComponent<Character>().Heal(999999999);
                    child.position = new Vector3(-50000, -50000, 0);
                }
                SceneManager.LoadScene("StartMenuScene");
                }
            );
        }
    }

    public void BattleWon()
    {
        WizardDataManager.instance.battleWon = true;
        WizardDataManager.instance.removeAllEnemyUnits();
        unitsDictionary.Values.ToList().ForEach((unit) => { 
            unit.GetComponent<Character>().hasMoved = false;
            });
        SceneManager.LoadScene("OverWorld");
    }

    internal void ShowActionMenu()
    {
        // Get the selected unit
        var unitObject = selectedUnit;
        string[] splittedString = unitObject.name.Split(char.Parse("("));
        string unitType = splittedString[0];
        Vector3Int unitPos = mapManager.ArenaTilemap.WorldToCell(selectedUnit.gameObject.transform.position);
        int attackRadius;

        // Determine the attack radius based on unit type
        if (unitType == "Cleric")
        {
            attackRadius = unitObject.GetComponent<Cleric>().attackDistance;
            UiCanvas.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Button>().interactable = true;
        }
        else
        {
            attackRadius = unitObject.GetComponent<Character>().attackDistance;
        }

        // Get nearby enemies within attack radius
        var nearbyEnemiesList = GetNearbyEnemies(unitPos, unitObject, attackRadius, true);

        // Update the action menu based on whether there are enemies in range
        if (nearbyEnemiesList.Count == 0)
        {
            UiCanvas.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().SetText("Not in range");
            UiCanvas.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Button>().interactable = false;
        }
        else
        {
            UiCanvas.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().fontSize = 12;
            UiCanvas.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().SetText("Attack");
            UiCanvas.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Button>().interactable = true;
        }

        // Set the unit in action and show the action menu
        unitInAction = true;
        UiCanvas.transform.GetChild(0).gameObject.SetActive(true);
    }

    internal void HideActionMenu()
    {
        UiCanvas.transform.GetChild(0).gameObject.SetActive(false);
    }

    //If no action is selected, move unit back to original position, should also give back action points
    //TODO: Yet to be actually used in the game
    internal void CancelledAction()
    {
        HideActionMenu();
        //Move unit back to original position ?
        unitInAction = false;
        movingUnit.transform.position = mapManager.ArenaTilemap.GetCellCenterWorld(movePath[0]);
    }

    public void setUnitInAction(bool value)
    {
        unitInAction = value;
    }

    public bool getAttackMode()
    {
        return attackMode;
    }

    public void setAttackMode(bool value)
    {
        attackMode = value;
    }


}
