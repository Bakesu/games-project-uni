using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBehaviour : MonoBehaviour
{

    [SerializeField]
    private UnitManager unitManager;
    [SerializeField]
    private Button fightButton;
    [SerializeField]
    private Button spellBookButton;
    [SerializeField]
    private Button waitButton;
    [SerializeField]
    private Button endTurnButton;
    [SerializeField]
    private Button hackButton;
    [SerializeField]
    private EnemyBehaviour enemyBehaviour;

    private bool firstswitch;



    // Start is called before the first frame update
    void Start()
    {
        enemyBehaviour = EnemyBehaviour.Instance;
        fightButton.onClick.AddListener(OnFightClicked);
        spellBookButton.onClick.AddListener(OnSpellBookButtonClicked);
        waitButton.onClick.AddListener(OnWaitClicked);
        endTurnButton.onClick.AddListener(EndTurnPressed);
        hackButton.onClick.AddListener(() =>
        {
            var listOfEnemiesToRemove = new List<Vector3Int>();
            foreach (var entry in unitManager.unitsDictionary)
            {
                if(entry.Value.tag == "Enemy")
                {
                    Destroy(entry.Value);
                    listOfEnemiesToRemove.Add(entry.Key);
                }
            }
            foreach(var entry in listOfEnemiesToRemove)
            {
                unitManager.unitsDictionary.Remove(entry);
            }
            GameObject.Find("UICanvas").transform.Find("WonBattleUI").gameObject.SetActive(true);
            GameObject.Find("UICanvas").transform.Find("WonBattleUI").transform.Find("WonBattleUI").Find("Button").GetComponent<Button>().onClick.AddListener(() => UnitManager.Instance.BattleWon());
        });
    }

    private void Update()
    {
        if(enemyBehaviour.playingOutEnemyTurn)
        {
            endTurnButton.interactable = false;
        }
        else
        {
            endTurnButton.interactable = true;
        }

        if (!enemyBehaviour.playingOutEnemyTurn && firstswitch)
        {
            EnablePlayerTurn();
            firstswitch = false;
        }
    }

    //Enables the player moves after the enemy has finished their turn
    public void EnablePlayerTurn()
    {
        Debug.Log("Enabling player turn");
        foreach (Vector3Int key in unitManager.unitsDictionary.Keys)
        {
            unitManager.unitsDictionary.TryGetValue(key, out GameObject unitObject);
            unitObject.GetComponent<Character>().hasMoved = false;
            if(unitObject.tag == "Ally")
            {
                unitObject.GetComponentInChildren<SpriteRenderer>().color = Color.white;
            }
        }
    }

    void OnFightClicked()
    {
        unitManager.setAttackMode(true); 
        unitManager.ShowUnitsInRange(unitManager.selectedUnit);
        unitManager.HideActionMenu();
    }
    void OnWaitClicked()
    {
        unitManager.ActionCompleted();
    }
    void OnSpellBookButtonClicked()
    {
        unitManager.selectedUnit.GetComponent<Cleric>().currentHealth += 2;
        unitManager.selectedUnit.GetComponent<Cleric>().healthBar.setHealth(unitManager.selectedUnit.GetComponent<Cleric>().currentHealth);
        unitManager.ActionCompleted();
    }
    void EndTurnPressed()
    {
        GameObject[] allies = GameObject.FindGameObjectsWithTag("Ally");
        if (unitManager.selectedUnit != null)
        {
            unitManager.ActionCompleted();
        }
        unitManager.setAttackMode(false);
        enemyBehaviour.playingOutEnemyTurn = true;
        firstswitch = true;
        enemyBehaviour.OnEnemyTurn(unitManager.unitsDictionary);
        
    }
}
