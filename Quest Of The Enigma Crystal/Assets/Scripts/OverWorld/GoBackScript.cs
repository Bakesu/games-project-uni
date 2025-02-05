using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoBackScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(goBack);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void goBack()
    {
        SceneManager.LoadScene("OverWorld");
    }
}
