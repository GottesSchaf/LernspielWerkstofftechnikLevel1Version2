using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zeigeKaputtScreen : MonoBehaviour {

    [SerializeField] GameObject infoBox;
    bool hineinGelaufen;

    private void OnTriggerEnter(Collider other)
    {
        if (hineinGelaufen == false && other.gameObject.tag == "Player")
        {
            infoBox.SetActive(true);
            hineinGelaufen = true;
        }
    }
}