using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausescreenScript : MonoBehaviour {

    [SerializeField] List<GameObject> fenster = new List<GameObject>();


    public void ZurueckButton()
    {
        fenster[0].SetActive(true);
        fenster[1].SetActive(false);
    }

    public void OptionenButton()
    {
        fenster[0].SetActive(false);
        fenster[1].SetActive(true);
    }
}
