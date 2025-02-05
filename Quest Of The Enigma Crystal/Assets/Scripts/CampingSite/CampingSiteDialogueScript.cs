using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CampingSiteDialogueScript : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    public GameObject CanvasObject;
    public GameObject TextBox;

    private int index;
    [SerializeField]
    private GameObject TakeRestButton;
    [SerializeField]
    private GameObject TextRestButtonOutline;

    void Start()
    {
        List<string> list = new List<string>
        {
            "Hello traveller",
            "I can offer you new wares and upgrades"
        };
        lines = list.ToArray();
        textComponent.text = string.Empty;
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(textComponent.text == lines[index])
            {
                if (textComponent.text == lines[lines.Length - 1])
                {
                    TextBox.SetActive(false);
                    CanvasObject.SetActive(true);
                    TextRestButtonOutline.SetActive(false);
                    TakeRestButton.SetActive(false);
                    //index = 0;
                    //textComponent.text = string.Empty;
                    //StopAllCoroutines();
                }
                else
                {
                    NextLine();
                }
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
            
        }
    }
    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());

    }
    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }
    void NextLine()
    {
        if(index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
