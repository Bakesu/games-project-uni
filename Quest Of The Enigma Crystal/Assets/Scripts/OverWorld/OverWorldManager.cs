using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OverWorldManager : MonoBehaviour
{
    float moveSpeed = 6f;
    Vector2 targetPos;

    HashSet<Collider2D> visitedNodes = new HashSet<Collider2D>();
    EncounterManager encounterScript;

    hudScript hudScript;

    void Start()
    {
        hudScript = GameObject.Find("HUD").GetComponent<hudScript>(); 
        encounterScript = GameObject.Find("Behavior").GetComponent<EncounterManager>();

        OverWorldData overWorldData = WizardDataManager.instance.overWorldData;
        transform.parent = GameObject.Find(overWorldData.layerName).transform.Find(overWorldData.nodeName);
        transform.position = overWorldData.positionInOverWorld;
        targetPos = overWorldData.positionInOverWorld;
        visitedNodes.Add(transform.parent.GetComponent<Collider2D>());
        Camera.main.transform.position = new Vector3(overWorldData.positionInOverWorld.x, overWorldData.positionInOverWorld.y, Camera.main.transform.position.z);
    }

    void Awake()
    {
        if (WizardDataManager.instance.bossBattleWon)
        {
            hudScript = GameObject.Find("HUD").GetComponent<hudScript>();
            hudScript.ShowBossBattleWonDialog();
            WizardDataManager.instance.bossBattleWon = false;
        }
        else if (WizardDataManager.instance.battleWon)
        {
            hudScript = GameObject.Find("HUD").GetComponent<hudScript>();
            hudScript.ShowBattleWonDialog();
            WizardDataManager.instance.battleWon = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !hudScript.encountersUI.activeSelf)
        {
            StartCoroutine(MoveToNode());
        }
        MovePlayerGameObject();

        if (Input.GetKeyUp(KeyCode.R))
        {
            Debug.Log("Reset");
            WizardDataManager.instance.overWorldData = new OverWorldData("Layer 0", "Node 0", new Vector2(0, 0));
            SceneManager.LoadScene("OverWorld");
        }
    }

    void MovePlayerGameObject()
    {
        if ((Vector2)transform.position != targetPos)
        {
            // Move the player object
            transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            // Make the camera follow the player object
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, new Vector3(targetPos.x, targetPos.y, Camera.main.transform.position.z), moveSpeed * Time.deltaTime);
        }
        WizardDataManager.instance.overWorldData.positionInOverWorld = transform.position; //TODO: Burde ligge et andet sted
    }

    IEnumerator MoveToNode()
    {
        Vector2 mousePos2D = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

        if (hit.collider == null) yield break;
        if ((Vector2)transform.position != targetPos) yield break;
        if (visitedNodes.Contains(hit.collider)) yield break;
        if (!RoadExists(hit)) yield break;

        visitedNodes.Add(hit.collider);

        targetPos = (Vector2)hit.collider.gameObject.transform.position;
        transform.parent = hit.collider.transform;

        WizardDataManager.instance.overWorldData.layerName = hit.collider.transform.parent.name;
        WizardDataManager.instance.overWorldData.nodeName = hit.collider.transform.name;

        hudScript.ShowGoToCampButton(false);

        Debug.Log(WizardDataManager.instance.playerGold.ToString());

        yield return new WaitForSeconds(0.5f); //Making UI pop up after 1 second

        if (hit.collider.transform.Find("Enemies") != null)
        {
            EnemyEncounter enemyEncounter = encounterScript.GetEnemyEncounter();
            hudScript.ShowEnemyEncounter(enemyEncounter);
        }
        else if (hit.collider.transform.Find("Campsite") != null)
        {
            hudScript.ShowGoToCampButton(true);
        }
        else if (hit.collider.transform.Find("Boss") != null)
        {
            EnemyEncounter bossEncounter = encounterScript.GetBossEncounter();
            hudScript.ShowBossEncounter(bossEncounter);
        }
        else
        {
            RandomEncounters();
        }
        yield break;
    }

    private bool RoadExists(RaycastHit2D hit)
    {
        // Get player current node number and current layer number
        String playerLayer = transform.parent.parent.name;
        String playerNode = transform.parent.name;

        // Get collider layer and number
        String colliderLayer = hit.collider.transform.parent.name;
        String colliderNode = hit.collider.gameObject.name;

        // Get road game object using player layer and node and collider layer and node || null if road does not exist
        GameObject roadFromCurrentNodeToColliderNode =
            GameObject.Find("Road: " + playerLayer + " " + playerNode + " - " + colliderLayer + " " + colliderNode);
        if (roadFromCurrentNodeToColliderNode == null)
        {
            return false;
        } else
        {
            return true;
        }
    }

    void RandomEncounters()
    {
        IEncounter randomEncounter = encounterScript.GetRandomEncounter();
        switch (randomEncounter.encounterType)
        {
            case EncounterType.NORMAL:
                hudScript.ShowDefaultEncounter(randomEncounter);
                break;
            case EncounterType.GOLD:
                hudScript.ShowGoldEncounter(randomEncounter as GoldEncounter);
                break;
            case EncounterType.HEAL:
                hudScript.ShowHealEncounter(randomEncounter as HealEncounter);
                break;
            case EncounterType.TAKEDMG:
                hudScript.ShowTakeDamageEncounter(randomEncounter as TakeDamageEncounter);
                break;
            case EncounterType.RANDOM_ENEMY:
                hudScript.ShowRandomEnemyEncounter(randomEncounter as RandomEnemyEncounter);
                break;
            default:
                Debug.Log("Encounter type not found");
                break;
        }

        //var randomNumber = UnityEngine.Random.Range(0, 100);
        //if(randomNumber < 10)
        //{
        //    hudScript.foundGoldEncounter();
        //}
        //if(randomNumber > 10 && randomNumber < 20)
        //{
        //    hudScript.randomBanditEncounter();
        //}
    }
}

