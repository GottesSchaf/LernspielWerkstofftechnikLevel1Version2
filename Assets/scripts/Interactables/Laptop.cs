using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laptop : MonoBehaviour {

    BunsenBrenner bunsenBrenner;
    public GameObject BBScriptObj;
    public GameObject graphZahnrad, graphAlle;

    private void Start()
    {
        bunsenBrenner = BBScriptObj.GetComponent<BunsenBrenner>();
    }
    private void Update()
    {
        //if (true)
        //{
        //    StartCoroutine(ZeigeGraphZahnrad());
        //}
        //else if (true)
        //{
        //    StartCoroutine(ZeigeGraphAlle());
        //}
        if((bunsenBrenner.slot1.transform.childCount > 0 && bunsenBrenner.slot1.transform.GetChild(0).tag == "Geschmolzen") || (bunsenBrenner.slot2.transform.childCount > 0 && bunsenBrenner.slot2.transform.GetChild(0).tag == "Geschmolzen") || (bunsenBrenner.slot3.transform.childCount > 0 && bunsenBrenner.slot3.transform.GetChild(0).tag == "Geschmolzen") || (bunsenBrenner.slot4.transform.childCount > 0 && bunsenBrenner.slot4.transform.GetChild(0).tag == "Geschmolzen"))
        {
            graphAlle.SetActive(false);
            graphZahnrad.SetActive(true);
        }
        else if ((bunsenBrenner.slot1.transform.childCount > 0 && bunsenBrenner.slot1.transform.GetChild(0).tag == "20SiHot" || bunsenBrenner.slot2.transform.childCount > 0 && bunsenBrenner.slot2.transform.GetChild(0).tag == "20SiHot" || bunsenBrenner.slot3.transform.childCount > 0 && bunsenBrenner.slot3.transform.GetChild(0).tag == "20SiHot" || bunsenBrenner.slot4.transform.childCount > 0 && bunsenBrenner.slot4.transform.GetChild(0).tag == "20SiHot") && (bunsenBrenner.slot1.transform.childCount > 0 && bunsenBrenner.slot1.transform.GetChild(0).tag == "40SiHot" || bunsenBrenner.slot2.transform.childCount > 0 && bunsenBrenner.slot2.transform.GetChild(0).tag == "40SiHot" || bunsenBrenner.slot3.transform.childCount > 0 && bunsenBrenner.slot3.transform.GetChild(0).tag == "40SiHot" || bunsenBrenner.slot4.transform.childCount > 0 && bunsenBrenner.slot4.transform.GetChild(0).tag == "40SiHot") && (bunsenBrenner.slot1.transform.childCount > 0 && bunsenBrenner.slot1.transform.GetChild(0).tag == "60SiHot" || bunsenBrenner.slot2.transform.childCount > 0 && bunsenBrenner.slot2.transform.GetChild(0).tag == "60SiHot" || bunsenBrenner.slot3.transform.childCount > 0 && bunsenBrenner.slot3.transform.GetChild(0).tag == "60SiHot" || bunsenBrenner.slot4.transform.childCount > 0 && bunsenBrenner.slot4.transform.GetChild(0).tag == "60SiHot") && (bunsenBrenner.slot1.transform.childCount > 0 && bunsenBrenner.slot1.transform.GetChild(0).tag == "80SiHot" || bunsenBrenner.slot2.transform.childCount > 0 && bunsenBrenner.slot2.transform.GetChild(0).tag == "80SiHot" || bunsenBrenner.slot3.transform.childCount > 0 && bunsenBrenner.slot3.transform.GetChild(0).tag == "80SiHot" || bunsenBrenner.slot4.transform.childCount > 0 && bunsenBrenner.slot4.transform.GetChild(0).tag == "80SiHot"))
        {
            graphZahnrad.SetActive(false);
            graphAlle.SetActive(true);
        }
    }
    IEnumerator ZeigeGraphZahnrad()
    {
        yield return new WaitForSeconds(1);
    }
    IEnumerator ZeigeGraphAlle()
    {
        yield return new WaitForSeconds(1);
    }
}
