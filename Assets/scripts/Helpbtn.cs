using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helpbtn : MonoBehaviour
{
    public GameObject helptext; 

    public void HelpPopUp ()
    {
        helptext.SetActive(!helptext.activeSelf);
    }

    public void Dissapear()
    {
        helptext.SetActive(false);
    }
}
