using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeigeInfozettel : Interactive {

    [SerializeField] GameObject fensterZumOeffnen;

    public override void Interact()
    {
        fensterZumOeffnen.SetActive(true);
    }

    public void CloseThisWindow()
    {
        fensterZumOeffnen.SetActive(false);
    }
}
