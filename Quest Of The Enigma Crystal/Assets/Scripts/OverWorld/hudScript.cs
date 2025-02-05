using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class hudScript : MonoBehaviour
{
    TextMeshProUGUI goldText;
    GameObject goToCamp;

    public GameObject encountersUI;
    Text encounterText;
    Text encounterEffect;
    Image encounterSprite;
    Button leftButton;
    TextMeshProUGUI leftButtonText;
    Button rightButton;
    TextMeshProUGUI rightButtonText;

    // Start is called before the first frame update
    void Awake()
    {
        goldText = GameObject.Find("Gold").GetComponent<TextMeshProUGUI>();
        goToCamp = GameObject.Find("GoToCamp");
        goToCamp.GetComponentInChildren<Button>().onClick.AddListener(GoToCampPressed);
        goToCamp.gameObject.SetActive(false);

        encountersUI = GameObject.Find("EncountersUI");
        encounterText = GameObject.Find("EncounterText").GetComponent<Text>();
        encounterEffect = GameObject.Find("EncounterEffect").GetComponent<Text>();
        encounterSprite = GameObject.Find("EncounterSprite").GetComponent<Image>();

        leftButton = GameObject.Find("LeftButton").GetComponent<Button>();
        leftButtonText = leftButton.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        rightButton = GameObject.Find("RightButton").GetComponent<Button>();
        rightButtonText = rightButton.transform.Find("Text").GetComponent<TextMeshProUGUI>();

        encountersUI.SetActive(false);
        WizardDataManager.instance.gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        goldText.text = "Gold: " + WizardDataManager.instance.playerGold.ToString(); //TODO: virker som bad practice, da vi updatere hele tiden
    }

    public void ShowGoToCampButton(bool shouldShow)
    {
        goToCamp.gameObject.SetActive(shouldShow);
    }

    void GoToCampPressed()
    {
        WizardDataManager.instance.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        SceneManager.LoadScene("CampingSite");
    }

    public void ShowDefaultEncounter(IEncounter encounter )
    {
        encountersUI.SetActive(true);
        encounterText.text = encounter.contentText;
        if (encounter is IEncounterEffect effectEncounter)
        {
            encounterEffect.text = effectEncounter.effectText;
        }
        leftButton.gameObject.SetActive(false);
        rightButtonText.text = "Continue!";
        rightButton.onClick.AddListener(ContinueButtonPressed);
        encounterSprite.sprite = Resources.Load<Sprite>(encounter.spriteName);
    }
    public void ShowGoldEncounter(GoldEncounter encounter)
    {
        ShowDefaultEncounter(encounter);
        WizardDataManager.instance.addPlayerGold(encounter.goldFound);
    }

    public void ShowHealEncounter(HealEncounter encounter)
    {
        ShowDefaultEncounter(encounter);
        var rosterUnits = WizardDataManager.instance.RosterUnits;

        Character lowestHealthUnit = null;
        float minHealth = float.MaxValue;

        foreach (var unit in rosterUnits)
        {
            Character character = unit.GetComponent<Character>();
            if (character != null && character.currentHealth < minHealth)
            {
                lowestHealthUnit = character;
                minHealth = character.currentHealth;
            }
        }

        if (lowestHealthUnit != null)
        {
            lowestHealthUnit.Heal(encounter.healthHealed);
        }
    }

    public void ShowTakeDamageEncounter(TakeDamageEncounter encounter)
    {
        ShowDefaultEncounter(encounter);
        var rosterUnits = WizardDataManager.instance.RosterUnits;

        Character highestHealthUnit = null;
        float maxHealth = 0;

        foreach (var unit in rosterUnits)
        {
            Character character = unit.GetComponent<Character>();
            if (character != null && character.currentHealth > maxHealth)
            {
                highestHealthUnit = character;
                maxHealth = character.currentHealth;
            };
        }

        if (highestHealthUnit != null && highestHealthUnit.currentHealth > encounter.damageTaken)
        {
            highestHealthUnit.TakeDamage(encounter.damageTaken);
        }
    }

    public void ShowEnemyEncounter(EnemyEncounter encounter)
    {
        encountersUI.SetActive(true);
        encounterText.text = encounter.contentText;
        encounterEffect.text = "";
        encounterSprite.sprite = Resources.Load<Sprite>(encounter.spriteName);
        leftButton.gameObject.SetActive(false);
        rightButtonText.text = "Fight!";
        rightButton.onClick.AddListener(() => FightButtonPressed(encounter));
    }
    public void ShowRandomEnemyEncounter(RandomEnemyEncounter encounter)
    {
        encountersUI.SetActive(true);
        encounterText.text = encounter.contentText;
        encounterEffect.text = encounter.effectText;
        encounterSprite.sprite = Resources.Load<Sprite>(encounter.spriteName);
        rightButtonText.text = "Fight!";
        rightButton.onClick.AddListener(() => FightButtonPressed(encounter));
        if (WizardDataManager.instance.playerGold < encounter.bribeAmount)
        {
            leftButtonText.text = "Pay";
            leftButton.interactable = false;
        }
        else
        {
            leftButtonText.text = "Pay";
            leftButton.onClick.AddListener(() => PayButtonPressed(encounter.bribeAmount));
        }
    }

    void FightButtonPressed(IEncounterEnemy encounter)
    {
        //set up combat scene
        foreach (GameObject enemyUnit in encounter.enemyUnits)
        {
            WizardDataManager.instance.addEnemyUnit(enemyUnit);
        }
        WizardDataManager.instance.battleReward = encounter.battleReward;
        WizardDataManager.instance.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        SceneManager.LoadScene("CombatScene");
    }

    void PayButtonPressed(int bribeAmount)
    {
        WizardDataManager.instance.removePlayerGold(bribeAmount);
        encounterText.text = "You pay the bandits and they let you pass through.";
        encounterEffect.text = "";
        leftButtonText.text = "Continue!";
        leftButton.onClick.RemoveAllListeners();
        leftButton.onClick.AddListener(ContinueButtonPressed);
        rightButton.gameObject.SetActive(false);
    }
    public void ShowBattleWonDialog()
    {
        encountersUI.SetActive(true);
        encounterText.text = "You defeat your enemies and loot them";
        encounterSprite.sprite = Resources.Load<Sprite>("mainchar_sprite");
        int battleReward = WizardDataManager.instance.battleReward;
        encounterEffect.text = "- You loot " + battleReward.ToString() + " gold from the enemies";
        leftButtonText.text = "Continue!";
        rightButton.gameObject.SetActive(false);
        leftButton.onClick.AddListener(ContinueButtonPressed);
        WizardDataManager.instance.addPlayerGold(battleReward);
    }

    public void ShowBossEncounter(EnemyEncounter bossEncounter)
    {
        encountersUI.SetActive(true);
        encounterText.text = bossEncounter.contentText;
        encounterEffect.text = "";
        encounterSprite.sprite = Resources.Load<Sprite>(bossEncounter.spriteName);
        leftButton.gameObject.SetActive(false);
        rightButtonText.text = "Fight!";
        rightButton.onClick.AddListener(() =>
        {
            foreach (GameObject enemyUnit in bossEncounter.enemyUnits)
            {
                WizardDataManager.instance.addEnemyUnit(enemyUnit);
            }
            WizardDataManager.instance.battleReward = bossEncounter.battleReward;
            WizardDataManager.instance.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            SceneManager.LoadScene("CombatSceneBoss");
        });
    }

    public void ShowBossBattleWonDialog()
    {
        encountersUI.SetActive(true);
        encounterText.text = "Congratulations! You defeated the boss. As we only have implemented the first zone this is the end of the game! Thank you for playing. ";
        encounterSprite.sprite = Resources.Load<Sprite>("mainchar_sprite");
        encounterEffect.text = "";
        rightButtonText.text = "Main menu";
        rightButton.gameObject.SetActive(true);
        leftButton.gameObject.SetActive(false);

        rightButton.onClick.AddListener(() => {
            foreach (Transform child in WizardDataManager.instance.EnemyUnitsParentGameObject.transform)
            {
                WizardDataManager.instance.removeWizardUnit(child.gameObject);
                Destroy(child.gameObject);
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

    void ContinueButtonPressed()
    {
        ResetEncounterButtons();
    }

    void ResetEncounterButtons()
    {
        encounterText.text = "";
        encounterEffect.text = "";
        leftButton.gameObject.SetActive(true);
        leftButton.onClick.RemoveAllListeners();
        leftButton.interactable = true;
        rightButton.gameObject.SetActive(true);
        rightButton.onClick.RemoveAllListeners();
        rightButton.interactable = true;
        encountersUI.SetActive(false);
    }

}
