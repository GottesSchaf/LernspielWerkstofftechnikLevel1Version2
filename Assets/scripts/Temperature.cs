using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temperature : MonoBehaviour {

    public double value = 0.00;
    public GameObject text;

    void Update ()
    {
        GetComponent<UnityEngine.UI.Text>().text = value.ToString();
    }
}
