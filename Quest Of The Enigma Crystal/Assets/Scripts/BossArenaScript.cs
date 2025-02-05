using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArenaScript : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        WizardDataManager.instance.bossBattleWon = true;
    }
}
