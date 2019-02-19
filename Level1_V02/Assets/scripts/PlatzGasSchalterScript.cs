using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatzGasSchalterScript : Interactive
{
    [SerializeField] public static Material platzGasOn, platzGasOff;
    [SerializeField] GameObject airDischarger;
    bool interacted;

    public override void Interact()
    {
        if (BunsenBrenner.platzGasSchalter == false && BunsenBrenner.hauptGasSchalter == true)
        {
            BunsenBrenner.platzGasSchalter = true;
            airDischarger.GetComponent<Renderer>().material = platzGasOn;
            Debug.Log("Platz Gas ein geschaltet");
            interacted = true;
        }
        else
        {
            BunsenBrenner.platzGasSchalter = false;
            airDischarger.GetComponent<Renderer>().material = platzGasOff;
            Debug.Log("Platz Gas aus geschaltet");
            interacted = false;
            BunsenBrenner.flamme1Bool = false;
            BunsenBrenner.flamme2Bool = false;
            BunsenBrenner.flamme3Bool = false;
            BunsenBrenner.flamme4Bool = false;
        }
    }

    private void Update()
    {
        if (BunsenBrenner.hauptGasSchalter == false && interacted)
        {
            BunsenBrenner.platzGasSchalter = false;
            airDischarger.GetComponent<Renderer>().material = platzGasOff;
            interacted = false;
            BunsenBrenner.flamme1Bool = false;
            BunsenBrenner.flamme2Bool = false;
            BunsenBrenner.flamme3Bool = false;
            BunsenBrenner.flamme4Bool = false;
        }

    }

    public void UpdateMaterial()
    {
        if (BunsenBrenner.platzGasSchalter == true)
        {
            airDischarger.GetComponent<Renderer>().material = platzGasOn;
        }
        else
        {
            airDischarger.GetComponent<Renderer>().material = platzGasOff;
        }
    }
}
