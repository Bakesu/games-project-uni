using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleric : Character
{
    // Start is called before the first frame update
    internal override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(2);
        }

    }
}
