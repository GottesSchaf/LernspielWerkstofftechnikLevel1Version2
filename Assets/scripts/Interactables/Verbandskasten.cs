using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Verbandskasten : Interactive {

    [SerializeField] GameObject verbandskastenFenster;

    public override void Interact()
    {
        if (BunsenBrenner.verbrannt == true)
        {
            verbandskastenFenster.SetActive(true);
            BunsenBrenner.verbrannt = false;
        }
    }

    public void SchließeFenster()
    {
        verbandskastenFenster.SetActive(false);
    }
}
