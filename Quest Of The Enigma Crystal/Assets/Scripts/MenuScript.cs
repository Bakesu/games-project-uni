using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnNewGameButton()
    {
        Debug.Log("New Game Button");
        SceneManager.LoadScene(1);
    }
    public void OnQuitButton()
    {
        Debug.Log("Quit Game Button");
        Application.Quit();
    }

    public void OnLoadNewGameButton()
    {
        Debug.Log("Load New Game Button");
    }
}
