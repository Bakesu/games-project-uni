using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitCost : MonoBehaviour
{
    public int gold = 0;
    private void Start()
    {
        StartCoroutine(ExampleCoroutine());
        
    }
    IEnumerator ExampleCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        try
        {

            for (int i = 0; i < 9; i++)
            {
                GameObject _Go = GameObject.FindGameObjectWithTag("ShopInnerSquare" + i);
                if (transform.name.Contains(i.ToString()) && _Go.GetComponentInChildren<UnitGold>() != null)
                {
                    TextMeshProUGUI text = gameObject.GetComponent<TextMeshProUGUI>();
                    text.text = _Go.GetComponentInChildren<UnitGold>().buyUnitGold.ToString();
                    gameObject.SetActive(true);

                }
            }

        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
}
