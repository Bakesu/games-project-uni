using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CampingSiteScript : MonoBehaviour
{
    public int playergold = 0;
    public TextMeshProUGUI showGold;
    public GameObject blackOutSquare;
    [SerializeField]
    private Button TakeRestButton;
    private int healAmount;

    void Start()
    {
        if (showGold != null)
        {
            playergold = WizardDataManager.instance.playerGold;
            showGold.text += playergold.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (showGold != null)
        {
            playergold = WizardDataManager.instance.playerGold;
            showGold.text = playergold.ToString();
        }
    }
    public void OnDoneButton()
    {
        Debug.Log("Done Button");
        SceneManager.LoadScene("OverWorld");
    }
    public IEnumerator FadeBlackOutSquare()
    {
        int fadeSpeed = 1;
        Color objectColor = blackOutSquare.GetComponent<SpriteRenderer>().color;
        float fadeAmount;
        while (blackOutSquare.GetComponent<SpriteRenderer>().color.a < 1)
        {
            fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);
            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            blackOutSquare.GetComponent<SpriteRenderer>().color = objectColor;
            yield return null;
        }

        yield return new WaitForSeconds(1);

        while (blackOutSquare.GetComponent<SpriteRenderer>().color.a > 0)
        {
            fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);
            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            blackOutSquare.GetComponent<SpriteRenderer>().color = objectColor;
            TakeRestButton.interactable = false;
            TakeRestButton.GetComponentInChildren<TextMeshProUGUI>().alpha = 0.5f;
            yield return null;
        }

    }
    public void OnTakeRestButton()
    {

        Debug.Log("Take Rest Button");
        StartCoroutine(FadeBlackOutSquare());

        foreach (GameObject unit in WizardDataManager.instance.RosterUnits)
        {
            healAmount = unit.GetComponent<Character>().maxHealth / 5;
            if(unit.GetComponent<Character>().currentHealth + healAmount > unit.GetComponent<Character>().maxHealth)
            {
                unit.GetComponent<Character>().currentHealth = unit.GetComponent<Character>().maxHealth;
            }

            unit.GetComponent<Character>().currentHealth += healAmount;

        }
    }
}
