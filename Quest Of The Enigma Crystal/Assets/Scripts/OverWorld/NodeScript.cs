using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeScript : MonoBehaviour
{
    [SerializeField] string terrain;
    // Start is called before the first frame update
    public string getTerrain()
    {
        return terrain;
    }
}
