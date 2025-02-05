using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    [HideInInspector]
    public List<Sprite> Sprites;
    [HideInInspector]
    public int itemSlotNumber = 0;
    private void Awake()
    {
        Sprite apprenticeSprite = Resources.Load<Sprite>("Sprites/apprentice_sprite");
        Sprite clericSprite = Resources.Load<Sprite>("Sprites/cleric_sprite");
        Sprite highpriestSprite = Resources.Load<Sprite>("Sprites/highpriest_sprite");
        Sprite enchanterSprite = Resources.Load<Sprite>("Sprites/enchanter_sprite");
        Sprite sorcererSprite = Resources.Load<Sprite>("Sprites/sorcerer_sprite");

        Sprite pikemanSprite = Resources.Load<Sprite>("Sprites/pikeman_sprite");
        Sprite infantrymanSprite = Resources.Load<Sprite>("Sprites/infantryman_sprite");
        Sprite paladinSprite = Resources.Load<Sprite>("Sprites/paladin_sprite");
        Sprite championSprite = Resources.Load<Sprite>("Sprites/champion_sprite");
        Sprite archerSprite = Resources.Load<Sprite>("Sprites/archer_sprite");
        Sprite arbalistSprite = Resources.Load<Sprite>("Sprites/arbalist_sprite");
        
        for (int i = 0; i < WizardDataManager.instance.RosterUnits.Count; i++)
        {
            if (transform.childCount <= 0 && transform.name.Contains(i.ToString()))
            {
                itemSlotNumber = i;
                GameObject unit = new GameObject(WizardDataManager.instance.RosterUnits[i].name);

                unit.AddComponent<RectTransform>().sizeDelta = GetComponent<RectTransform>().sizeDelta;

                unit.AddComponent<DragDrop>();

                RectTransform rect = unit.GetComponent<RectTransform>();
                rect.SetParent(transform);
                rect.localPosition = new Vector3(1, 1, 1);
                rect.localScale = new Vector3(1, 1, 1);

                unit.SetActive(true);
                unit.AddComponent<CanvasGroup>();
                unit.AddComponent<CanvasRenderer>();
                
              
                if (unit.name.Contains("Pikeman"))
                {
                    Image image = unit.AddComponent<Image>();
                    image.sprite = pikemanSprite;

                    UnitGold unitGold = unit.AddComponent<UnitGold>();
                    unitGold.sellUnitGold = WizardDataManager.instance.RosterUnits[i].GetComponent<Pikeman>().gold;
                }
                else if (unit.name.Contains("Infantryman"))
                {
                    Image image = unit.AddComponent<Image>();
                    image.sprite = infantrymanSprite;

                    UnitGold unitGold = unit.AddComponent<UnitGold>();
                    unitGold.sellUnitGold = WizardDataManager.instance.RosterUnits[i].GetComponent<Infantryman>().gold;
                }
                else if (unit.name.Contains("Paladin"))
                {
                    Image image = unit.AddComponent<Image>();
                    image.sprite = paladinSprite;

                    UnitGold unitGold = unit.AddComponent<UnitGold>();
                    unitGold.sellUnitGold = WizardDataManager.instance.RosterUnits[i].GetComponent<Paladin>().gold;
                }
                else if (unit.name.Contains("Champion"))
                {
                    Image image = unit.AddComponent<Image>();
                    image.sprite = championSprite;

                    UnitGold unitGold = unit.AddComponent<UnitGold>();
                    unitGold.sellUnitGold = WizardDataManager.instance.RosterUnits[i].GetComponent<Champion>().gold;
                }
                else if (unit.name.Contains("Archer"))
                {
                    Image image = unit.AddComponent<Image>();
                    image.sprite = archerSprite;

                    UnitGold unitGold = unit.AddComponent<UnitGold>();
                    unitGold.sellUnitGold = WizardDataManager.instance.RosterUnits[i].GetComponent<Archer>().gold;
                }
                else if (unit.name.Contains("Arbalist"))
                {
                    Image image = unit.AddComponent<Image>();
                    image.sprite = arbalistSprite;

                    UnitGold unitGold = unit.AddComponent<UnitGold>();
                    unitGold.sellUnitGold = WizardDataManager.instance.RosterUnits[i].GetComponent<Arbalist>().gold;
                }
                else if (unit.name.Contains("Apprentice"))
                {
                    Image image = unit.AddComponent<Image>();
                    image.sprite = apprenticeSprite;

                    UnitGold unitGold = unit.AddComponent<UnitGold>();
                    unitGold.sellUnitGold = WizardDataManager.instance.RosterUnits[i].GetComponent<Apprentice>().gold;
                }
                else if (unit.name.Contains("Enchanter"))
                {
                    Image image = unit.AddComponent<Image>();
                    image.sprite = enchanterSprite;

                    UnitGold unitGold = unit.AddComponent<UnitGold>();
                    unitGold.sellUnitGold = WizardDataManager.instance.RosterUnits[i].GetComponent<Enchanter>().gold;
                }
                else if (unit.name.Contains("Sorcerer"))
                {
                    Image image = unit.AddComponent<Image>();
                    image.sprite = sorcererSprite;

                    UnitGold unitGold = unit.AddComponent<UnitGold>();
                    unitGold.sellUnitGold = WizardDataManager.instance.RosterUnits[i].GetComponent<Enchanter>().gold;
                }
                else if (unit.name.Contains("Cleric"))
                {
                    Image image = unit.AddComponent<Image>();
                    image.sprite = clericSprite;

                    UnitGold unitGold = unit.AddComponent<UnitGold>();
                    unitGold.sellUnitGold = WizardDataManager.instance.RosterUnits[i].GetComponent<Cleric>().gold;
                }
                else if (unit.name.Contains("HighPriest"))
                {
                    Image image = unit.AddComponent<Image>();
                    image.sprite = highpriestSprite;

                    UnitGold unitGold = unit.AddComponent<UnitGold>();
                    unitGold.sellUnitGold = WizardDataManager.instance.RosterUnits[i].GetComponent<HighPriest>().gold;
                }
            }
        }
        
    }
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
        GameObject dropped = eventData.pointerDrag;
        DragDrop dragDrop = dropped.GetComponent<DragDrop>();
        GameObject unit = null;

        if (transform.childCount <= 0 && eventData.pointerDrag.GetComponent<RectTransform>() != null && !dropped.name.Contains("shop"))
        {
            dragDrop.parentAfterDrag = transform;

        }
        else if(transform.childCount <= 0 && dropped.GetComponent<RectTransform>() != null && dropped.name.Contains("shop") && WizardDataManager.instance.playerGold - dropped.GetComponent<UnitGold>().buyUnitGold >= 0)
        {
            dragDrop.parentAfterDrag = transform;
            dropped.name = dropped.name.Replace("shop", "");
            WizardDataManager.instance.playerGold -= dropped.GetComponent<UnitGold>().buyUnitGold;

            if (dropped.name.Contains("Pikeman"))
            {
                unit = Instantiate(WizardDataManager.instance.Pikeman, WizardDataManager.instance.UnitsParentGameObject.transform);
            }
            else if (dropped.name.Contains("Infantryman"))
            {
                unit = Instantiate(WizardDataManager.instance.Infantryman, WizardDataManager.instance.UnitsParentGameObject.transform);
            }
            else if (dropped.name.Contains("Paladin"))
            {
                 unit = Instantiate(WizardDataManager.instance.Paladin, WizardDataManager.instance.UnitsParentGameObject.transform);
            }
            else if (dropped.name.Contains("Champion"))
            {
                unit = Instantiate(WizardDataManager.instance.Champion, WizardDataManager.instance.UnitsParentGameObject.transform);
            }
            else if (dropped.name.Contains("Archer"))
            {
                unit = Instantiate(WizardDataManager.instance.Archer, WizardDataManager.instance.UnitsParentGameObject.transform);
            }
            else if (dropped.name.Contains("Arbalist"))
            {
                unit = Instantiate(WizardDataManager.instance.Arbalist, WizardDataManager.instance.UnitsParentGameObject.transform);
            }
            else if (dropped.name.Contains("Apprentice"))
            {
                unit = Instantiate(WizardDataManager.instance.Apprentice, WizardDataManager.instance.UnitsParentGameObject.transform);
            }
            else if (dropped.name.Contains("Enchanter"))
            {
                unit = Instantiate(WizardDataManager.instance.Enchanter, WizardDataManager.instance.UnitsParentGameObject.transform);
            }
            else if (dropped.name.Contains("Sorcerer"))
            {
                unit = Instantiate(WizardDataManager.instance.Enchanter, WizardDataManager.instance.UnitsParentGameObject.transform);
            }
            else if (dropped.name.Contains("Cleric"))
            {
                unit = Instantiate(WizardDataManager.instance.Cleric, WizardDataManager.instance.UnitsParentGameObject.transform);
            }
            else if (dropped.name.Contains("HighPriest"))
            {
                unit = Instantiate(WizardDataManager.instance.HighPriest, WizardDataManager.instance.UnitsParentGameObject.transform);
            }

            WizardDataManager.instance.addWizardUnit(unit);
        }
    }
}
