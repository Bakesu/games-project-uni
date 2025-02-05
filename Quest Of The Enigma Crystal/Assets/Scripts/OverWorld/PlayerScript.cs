using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addMoney(int addedAmount)
    {
        PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("gold") + addedAmount);
    }

    public int getMoney()
    {
        return PlayerPrefs.GetInt("gold"); ;
    }

    public int getHealth()
    {
        return PlayerPrefs.GetInt("health");
    }

}
