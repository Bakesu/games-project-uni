using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.UI.CanvasScaler;
using Random = System.Random;

public class Sell : MonoBehaviour, IDropHandler
{    
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnSell");
        if (eventData.pointerDrag != null) //Noget med at uniten skal være en del af vores spillers hold
        {
            GameObject dropped = eventData.pointerDrag;

            if(dropped != null && !dropped.name.Contains("shop")){
                

                int count = 0;
                for( int i = 0; i < WizardDataManager.instance.RosterUnits.Count; i++)
                {
                    if (WizardDataManager.instance.RosterUnits[i] != null && WizardDataManager.instance.RosterUnits[i].name.Contains(dropped.name) && count < 1)
                    {
                        count = 1;
                        Destroy(dropped);
                        Destroy(WizardDataManager.instance.RosterUnits[i]);
                        WizardDataManager.instance.RosterUnits.RemoveAt(i);
                        WizardDataManager.instance.playerGold += dropped.GetComponent<UnitGold>().sellUnitGold;
                        
                        //if (dropped.name.Contains("Pikeman"))
                        //{
                        //    unit = Instantiate(WizardDataManager.instance.Pikeman, WizardDataManager.instance.UnitsParentGameObject.transform);
                        //}
                        //else if (dropped.name.Contains("Infantryman"))
                        //{
                        //    unit = Instantiate(WizardDataManager.instance.Infantryman, WizardDataManager.instance.UnitsParentGameObject.transform);
                        //}
                        //else if (dropped.name.Contains("Paladin"))
                        //{
                        //    unit = Instantiate(WizardDataManager.instance.Paladin, WizardDataManager.instance.UnitsParentGameObject.transform);
                        //}
                        //else if (dropped.name.Contains("Champion"))
                        //{
                        //    unit = Instantiate(WizardDataManager.instance.Champion, WizardDataManager.instance.UnitsParentGameObject.transform);
                        //}
                        //else if (dropped.name.Contains("Archer"))
                        //{
                        //    unit = Instantiate(WizardDataManager.instance.Archer, WizardDataManager.instance.UnitsParentGameObject.transform);
                        //}
                        //else if (dropped.name.Contains("Arbalist"))
                        //{
                        //    unit = Instantiate(WizardDataManager.instance.Arbalist, WizardDataManager.instance.UnitsParentGameObject.transform);
                        //}
                        //else if (dropped.name.Contains("Apprentice"))
                        //{
                        //    unit = Instantiate(WizardDataManager.instance.Apprentice, WizardDataManager.instance.UnitsParentGameObject.transform);
                        //}
                        //else if (dropped.name.Contains("Enchanter"))
                        //{
                        //    unit = Instantiate(WizardDataManager.instance.Enchanter, WizardDataManager.instance.UnitsParentGameObject.transform);
                        //}
                        //else if (dropped.name.Contains("Sorcerer"))
                        //{
                        //    unit = Instantiate(WizardDataManager.instance.Enchanter, WizardDataManager.instance.UnitsParentGameObject.transform);
                        //}
                        //else if (dropped.name.Contains("Cleric"))
                        //{
                        //    unit = Instantiate(WizardDataManager.instance.Cleric, WizardDataManager.instance.UnitsParentGameObject.transform);
                        //}
                        //else if (dropped.name.Contains("HighPriest"))
                        //{
                        //    unit = Instantiate(WizardDataManager.instance.HighPriest, WizardDataManager.instance.UnitsParentGameObject.transform);
                        //}
                        //DestroyImmediate(unit, true);
                    }
                }
                

            }            
        }
    }
}
