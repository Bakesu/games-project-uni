using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

[Serializable]
public class WizardDataManager : MonoBehaviour
{
    // Start is called before the first frame update
    //Wizard stats
    public List<GameObject> RosterUnits = new List<GameObject>();
    public GameObject UnitsParentGameObject;
    public List<GameObject> EnemyUnits = new List<GameObject>();
    public GameObject EnemyUnitsParentGameObject;
    public int playerGold = 0;
    public static WizardDataManager instance;
    public OverWorldData overWorldData;
    public bool battleWon = false;
    public bool bossBattleWon = false;
    public int battleReward = 0;

    public GameObject Apprentice;
    public GameObject Arbalist;
    public GameObject Archer;
    public GameObject Champion;
    public GameObject Cleric;
    public GameObject Enchanter;
    public GameObject HighPriest;
    public GameObject Infantryman;
    public GameObject Paladin;
    public GameObject Pikeman;
    public GameObject Sorcerer;

    public GameObject Revenant;
    public GameObject Wolf;
    public GameObject Orc;
    public GameObject Warlock;


    private void Awake()
    {
        if (instance != null)
        {
            // If the instance reference has already been set, and this is not
            // the instance reference, destroy this game object.
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        UnitsParentGameObject = Instantiate(new GameObject("Units"), transform);
        EnemyUnitsParentGameObject = Instantiate(new GameObject("EnemyUnits"), transform);

        this.overWorldData = new OverWorldData("Layer 0", "Node 0", new Vector2(0, 0));
        instance.addPlayerGold(50);

        addWizardUnit(Instantiate(Pikeman, UnitsParentGameObject.transform));
        addWizardUnit(Instantiate(Pikeman, UnitsParentGameObject.transform));
    }
    public void addPlayerGold(int gold)
    {
        this.playerGold += gold;
    }
    public bool removePlayerGold(int gold)
    {
        if (this.playerGold - gold >= 0)
        {
            this.playerGold -= gold;
            return true;
        }
        else
        {
            Debug.Log("Don't have enough gold");
            return false;
        }
    }
    public bool addWizardUnit(GameObject unit)
    {
        if (RosterUnits.Count < 9)
        {
            RosterUnits.Add(unit);
            return true;
        }
        else
        {
            Debug.Log("Can't have more units");
            return false;
        }

    }
    public bool removeWizardUnit(GameObject unit)
    {
        if (RosterUnits.Count >= 1)
        {
            //Scene scene = SceneManager.GetActiveScene();
            //if (scene.name == "Camping Site")
            //{
            //    instance.playerGold += unit.gold;
            //}
            RosterUnits.Remove(unit);
            return true;
        }
        else
        {
            Debug.Log("ERROR, Can't have less unit than 0 units");
            return false;
        }
    }

    public void addEnemyUnit(GameObject unit)
    {
        EnemyUnits.Add(unit);
    }

    public void removeEnemyUnit(GameObject unit)
    {
        EnemyUnits.Remove(unit);
    }

    public void removeAllEnemyUnits()
    {
        EnemyUnits.Clear();
    }

    public void removeAllRosterUnits()
    {
        RosterUnits.Clear();
    }
}

[Serializable]
public class OverWorldData
{
    public string layerName;
    public string nodeName;
    public Vector2 positionInOverWorld;
    public OverWorldData(string layerName, string nodeName, Vector2 position)
    {
        this.layerName = layerName;
        this.nodeName = nodeName;
        this.positionInOverWorld = position;
    }

}

[System.Serializable]
class SaveLoadData
{
    public WizardDataManager wizardData;
    public void SaveData()
    {
        SaveLoadData data = new SaveLoadData();
        data.wizardData = WizardDataManager.instance;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }
    public void LoadData()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveLoadData data = JsonUtility.FromJson<SaveLoadData>(json);

            WizardDataManager.instance = data.wizardData;
        }
    }
}