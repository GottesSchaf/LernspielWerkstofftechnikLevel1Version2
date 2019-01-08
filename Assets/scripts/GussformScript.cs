using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GussformScript : MonoBehaviour {

    public Transform slotWrenchTiegel, slotPleuelTiegel, slotZahnradTiegel, slotWrench, slotPleuel, slotZahnrad;
    public GameObject[] legierungForm;
    //public int abkuehlZeit;
    public GameObject zahnrad, pleuel, wrench;
    public GameObject BBScriptObjekt;
    BunsenBrenner bunsenBrenner;
    bool waiting;
    [SerializeField] Sprite tiegelLeer;

    void Start()
    {
        bunsenBrenner = BBScriptObjekt.GetComponent<BunsenBrenner>();
    }

    private void Update()
    {
        if (waiting == false)
        {
            StartCoroutine(KuehleAb());
        }
    }
    IEnumerator KuehleAb()
    {
        waiting = true;
        yield return new WaitForSeconds(1);
        //Schaue nach, welche Legierung in welchem Slot ist und kühle sie ab
        #region Slot Ueberpruefung
        //Schraubenschlüssel Slot
        #region Schraubenschluessel
        if (slotWrenchTiegel.transform.childCount > 0 && slotWrenchTiegel.transform.GetChild(0).CompareTag("20SiHot"))
        {
            slotWrenchTiegel.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelLeer;
            slotWrenchTiegel.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelLeer;
            if (bunsenBrenner.istTemp[0] > 25 && bunsenBrenner.istTemp[0] <= bunsenBrenner.BB1_Zieltemp[0])
            {
                bunsenBrenner.istTemp[0] -= bunsenBrenner.BB1_Zieltemp[0] / bunsenBrenner.BB1_Zeit[0];
            }
            else if (bunsenBrenner.istTemp[0] > bunsenBrenner.BB1_Zieltemp[0] && bunsenBrenner.istTemp[0] <= bunsenBrenner.BB1_Zieltemp[1])
            {
                bunsenBrenner.istTemp[0] -= (bunsenBrenner.BB1_Zieltemp[1] - bunsenBrenner.BB1_Zieltemp[0]) / bunsenBrenner.BB1_Zeit[1];
            }
            //3ter Graph Punkt
            else if (bunsenBrenner.istTemp[0] > bunsenBrenner.BB1_Zieltemp[1] && bunsenBrenner.graphPunkt4 == false && bunsenBrenner.graphPunkt5 == false)
            {
                bunsenBrenner.istTemp[0] -= (bunsenBrenner.BB1_Zieltemp[2] - bunsenBrenner.BB1_Zieltemp[1]) / bunsenBrenner.BB1_Zeit[2];
            }
            else if (bunsenBrenner.istTemp[0] > bunsenBrenner.BB1_Zieltemp[1] && bunsenBrenner.istTemp[0] <= bunsenBrenner.BB1_Zieltemp[2] && bunsenBrenner.graphPunkt4 == true && bunsenBrenner.graphPunkt5 == false)
            {
                bunsenBrenner.istTemp[0] -= (bunsenBrenner.BB1_Zieltemp[2] - bunsenBrenner.BB1_Zieltemp[1]) / bunsenBrenner.BB1_Zeit[2];
            }
            //4ter Graph Punkt
            else if (bunsenBrenner.istTemp[0] > bunsenBrenner.BB1_Zieltemp[2] && bunsenBrenner.graphPunkt4 == true && bunsenBrenner.graphPunkt5 == false)
            {
                bunsenBrenner.istTemp[0] -= (bunsenBrenner.BB1_Zieltemp[3] - bunsenBrenner.BB1_Zieltemp[2]) / bunsenBrenner.BB1_Zeit[3];
            }
            else if (bunsenBrenner.istTemp[0] > bunsenBrenner.BB1_Zieltemp[2] && bunsenBrenner.istTemp[0] <= bunsenBrenner.BB1_Zieltemp[3] && bunsenBrenner.graphPunkt4 == true && bunsenBrenner.graphPunkt5 == true)
            {
                bunsenBrenner.istTemp[0] -= (bunsenBrenner.BB1_Zieltemp[2] - bunsenBrenner.BB1_Zieltemp[1]) / bunsenBrenner.BB1_Zeit[2];
            }
            //5ter Graph Punkt
            else if (bunsenBrenner.istTemp[0] > bunsenBrenner.BB1_Zieltemp[3] && bunsenBrenner.graphPunkt4 == true && bunsenBrenner.graphPunkt5 == true)
            {
                bunsenBrenner.istTemp[0] -= (bunsenBrenner.BB1_Zieltemp[4] - bunsenBrenner.BB1_Zieltemp[3]) / bunsenBrenner.BB1_Zeit[4];
            }
            //---------------------------------------
            else if (bunsenBrenner.istTemp[0] <= 25)
            {
                bunsenBrenner.istTemp[0] = 25;
                GameObject newWrench = Instantiate(wrench);
                newWrench.transform.SetParent(slotWrench);
                newWrench.transform.tag = "Falsch";
                slotWrenchTiegel.transform.GetChild(0).tag = "Empty";
            }
        }
        //40%Si / 60%Ge
        else if (slotWrenchTiegel.transform.childCount > 0 && slotWrenchTiegel.transform.GetChild(0).CompareTag("40SiHot"))
        {
            slotWrenchTiegel.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelLeer;
            slotWrenchTiegel.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelLeer;
            if (bunsenBrenner.istTemp[1] > 25 && bunsenBrenner.istTemp[1] <= bunsenBrenner.BB2_Zieltemp[0])
            {
                bunsenBrenner.istTemp[1] -= bunsenBrenner.BB2_Zieltemp[0] / bunsenBrenner.BB2_Zeit[0];
            }
            else if (bunsenBrenner.istTemp[1] > bunsenBrenner.BB2_Zieltemp[0] && bunsenBrenner.istTemp[1] <= bunsenBrenner.BB2_Zieltemp[1])
            {
                bunsenBrenner.istTemp[1] -= (bunsenBrenner.BB2_Zieltemp[1] - bunsenBrenner.BB2_Zieltemp[0]) / bunsenBrenner.BB2_Zeit[1];
            }
            //3ter Graph Punkt
            else if (bunsenBrenner.istTemp[1] > bunsenBrenner.BB2_Zieltemp[1] && bunsenBrenner.graphPunkt4 == false && bunsenBrenner.graphPunkt5 == false)
            {
                bunsenBrenner.istTemp[1] -= (bunsenBrenner.BB2_Zieltemp[2] - bunsenBrenner.BB2_Zieltemp[1]) / bunsenBrenner.BB1_Zeit[2];
            }
            else if (bunsenBrenner.istTemp[1] > bunsenBrenner.BB2_Zieltemp[1] && bunsenBrenner.istTemp[1] <= bunsenBrenner.BB2_Zieltemp[2] && bunsenBrenner.graphPunkt4 == true && bunsenBrenner.graphPunkt5 == false)
            {
                bunsenBrenner.istTemp[1] -= (bunsenBrenner.BB2_Zieltemp[2] - bunsenBrenner.BB2_Zieltemp[1]) / bunsenBrenner.BB1_Zeit[2];
            }
            //4ter Graph Punkt
            else if (bunsenBrenner.istTemp[1] > bunsenBrenner.BB2_Zieltemp[2] && bunsenBrenner.graphPunkt4 == true && bunsenBrenner.graphPunkt5 == false)
            {
                bunsenBrenner.istTemp[1] -= (bunsenBrenner.BB2_Zieltemp[3] - bunsenBrenner.BB2_Zieltemp[2]) / bunsenBrenner.BB1_Zeit[3];
            }
            else if (bunsenBrenner.istTemp[1] > bunsenBrenner.BB2_Zieltemp[2] && bunsenBrenner.istTemp[1] <= bunsenBrenner.BB2_Zieltemp[3] && bunsenBrenner.graphPunkt4 == true && bunsenBrenner.graphPunkt5 == true)
            {
                bunsenBrenner.istTemp[1] -= (bunsenBrenner.BB2_Zieltemp[2] - bunsenBrenner.BB2_Zieltemp[1]) / bunsenBrenner.BB1_Zeit[2];
            }
            //5ter Graph Punkt
            else if (bunsenBrenner.istTemp[1] > bunsenBrenner.BB2_Zieltemp[3] && bunsenBrenner.graphPunkt4 == true && bunsenBrenner.graphPunkt5 == true)
            {
                bunsenBrenner.istTemp[1] -= (bunsenBrenner.BB2_Zieltemp[4] - bunsenBrenner.BB2_Zieltemp[3]) / bunsenBrenner.BB1_Zeit[4];
            }
            //---------------------------------------
            else if (bunsenBrenner.istTemp[1] <= 25)
            {
                bunsenBrenner.istTemp[1] = 25;
                GameObject newWrench = Instantiate(wrench);
                newWrench.transform.SetParent(slotWrench);
                newWrench.transform.tag = "Falsch";
                slotWrenchTiegel.transform.GetChild(0).tag = "Empty";
            }
        }
        //60%Si / 40%Ge
        else if (slotWrenchTiegel.transform.childCount > 0 && slotWrenchTiegel.transform.GetChild(0).CompareTag("60SiHot"))
        {
            slotWrenchTiegel.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelLeer;
            slotWrenchTiegel.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelLeer;
            if (bunsenBrenner.istTemp[2] > 25 && bunsenBrenner.istTemp[2] <= bunsenBrenner.BB3_Zieltemp[0])
            {
                bunsenBrenner.istTemp[2] -= bunsenBrenner.BB3_Zieltemp[0] / bunsenBrenner.BB3_Zeit[0];
            }
            else if (bunsenBrenner.istTemp[2] > bunsenBrenner.BB3_Zieltemp[0] && bunsenBrenner.istTemp[2] <= bunsenBrenner.BB3_Zieltemp[1])
            {
                bunsenBrenner.istTemp[2] -= (bunsenBrenner.BB3_Zieltemp[1] - bunsenBrenner.BB3_Zieltemp[0]) / bunsenBrenner.BB3_Zeit[1];
            }
            //3ter Graph Punkt
            else if (bunsenBrenner.istTemp[2] > bunsenBrenner.BB3_Zieltemp[1] && bunsenBrenner.graphPunkt4 == false && bunsenBrenner.graphPunkt5 == false)
            {
                bunsenBrenner.istTemp[2] -= (bunsenBrenner.BB3_Zieltemp[2] - bunsenBrenner.BB3_Zieltemp[1]) / bunsenBrenner.BB1_Zeit[2];
            }
            else if (bunsenBrenner.istTemp[2] > bunsenBrenner.BB3_Zieltemp[1] && bunsenBrenner.istTemp[2] <= bunsenBrenner.BB3_Zieltemp[2] && bunsenBrenner.graphPunkt4 == true && bunsenBrenner.graphPunkt5 == false)
            {
                bunsenBrenner.istTemp[2] -= (bunsenBrenner.BB3_Zieltemp[2] - bunsenBrenner.BB3_Zieltemp[1]) / bunsenBrenner.BB1_Zeit[2];
            }
            //4ter Graph Punkt
            else if (bunsenBrenner.istTemp[2] > bunsenBrenner.BB3_Zieltemp[2] && bunsenBrenner.graphPunkt4 == true && bunsenBrenner.graphPunkt5 == false)
            {
                bunsenBrenner.istTemp[2] -= (bunsenBrenner.BB3_Zieltemp[3] - bunsenBrenner.BB3_Zieltemp[2]) / bunsenBrenner.BB1_Zeit[3];
            }
            else if (bunsenBrenner.istTemp[2] > bunsenBrenner.BB3_Zieltemp[2] && bunsenBrenner.istTemp[2] <= bunsenBrenner.BB3_Zieltemp[3] && bunsenBrenner.graphPunkt4 == true && bunsenBrenner.graphPunkt5 == true)
            {
                bunsenBrenner.istTemp[2] -= (bunsenBrenner.BB3_Zieltemp[2] - bunsenBrenner.BB3_Zieltemp[1]) / bunsenBrenner.BB1_Zeit[2];
            }
            //5ter Graph Punkt
            else if (bunsenBrenner.istTemp[2] > bunsenBrenner.BB3_Zieltemp[3] && bunsenBrenner.graphPunkt4 == true && bunsenBrenner.graphPunkt5 == true)
            {
                bunsenBrenner.istTemp[2] -= (bunsenBrenner.BB3_Zieltemp[4] - bunsenBrenner.BB3_Zieltemp[3]) / bunsenBrenner.BB1_Zeit[4];
            }
            //---------------------------------------
            else if (bunsenBrenner.istTemp[2] <= 25)
            {
                bunsenBrenner.istTemp[2] = 25;
                GameObject newWrench = Instantiate(wrench);
                newWrench.transform.SetParent(slotWrench);
                newWrench.transform.tag = "Falsch";
                slotWrenchTiegel.transform.GetChild(0).tag = "Empty";
            }
        }
        //80%Si / 20%Ge
        else if (slotWrenchTiegel.transform.childCount > 0 && slotWrenchTiegel.transform.GetChild(0).CompareTag("80SiHot"))
        {
            slotWrenchTiegel.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelLeer;
            slotWrenchTiegel.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelLeer;
            if (bunsenBrenner.istTemp[3] > 25 && bunsenBrenner.istTemp[3] <= bunsenBrenner.BB4_Zieltemp[0])
            {
                bunsenBrenner.istTemp[3] -= bunsenBrenner.BB4_Zieltemp[0] / bunsenBrenner.BB4_Zeit[0];
            }
            else if (bunsenBrenner.istTemp[3] > bunsenBrenner.BB4_Zieltemp[0] && bunsenBrenner.istTemp[3] <= bunsenBrenner.BB4_Zieltemp[1])
            {
                bunsenBrenner.istTemp[3] -= (bunsenBrenner.BB4_Zieltemp[1] - bunsenBrenner.BB4_Zieltemp[0]) / bunsenBrenner.BB4_Zeit[1];
            }
            //3ter Graph Punkt
            else if (bunsenBrenner.istTemp[3] > bunsenBrenner.BB4_Zieltemp[1] && bunsenBrenner.graphPunkt4 == false && bunsenBrenner.graphPunkt5 == false)
            {
                bunsenBrenner.istTemp[3] -= (bunsenBrenner.BB4_Zieltemp[2] - bunsenBrenner.BB4_Zieltemp[1]) / bunsenBrenner.BB1_Zeit[2];
            }
            else if (bunsenBrenner.istTemp[3] > bunsenBrenner.BB4_Zieltemp[1] && bunsenBrenner.istTemp[3] <= bunsenBrenner.BB4_Zieltemp[2] && bunsenBrenner.graphPunkt4 == true && bunsenBrenner.graphPunkt5 == false)
            {
                bunsenBrenner.istTemp[3] -= (bunsenBrenner.BB4_Zieltemp[2] - bunsenBrenner.BB4_Zieltemp[1]) / bunsenBrenner.BB1_Zeit[2];
            }
            //4ter Graph Punkt
            else if (bunsenBrenner.istTemp[3] > bunsenBrenner.BB4_Zieltemp[2] && bunsenBrenner.graphPunkt4 == true && bunsenBrenner.graphPunkt5 == false)
            {
                bunsenBrenner.istTemp[3] -= (bunsenBrenner.BB4_Zieltemp[3] - bunsenBrenner.BB4_Zieltemp[2]) / bunsenBrenner.BB1_Zeit[3];
            }
            else if (bunsenBrenner.istTemp[3] > bunsenBrenner.BB4_Zieltemp[2] && bunsenBrenner.istTemp[3] <= bunsenBrenner.BB4_Zieltemp[3] && bunsenBrenner.graphPunkt4 == true && bunsenBrenner.graphPunkt5 == true)
            {
                bunsenBrenner.istTemp[3] -= (bunsenBrenner.BB4_Zieltemp[2] - bunsenBrenner.BB4_Zieltemp[1]) / bunsenBrenner.BB1_Zeit[2];
            }
            //5ter Graph Punkt
            else if (bunsenBrenner.istTemp[3] > bunsenBrenner.BB4_Zieltemp[3] && bunsenBrenner.graphPunkt4 == true && bunsenBrenner.graphPunkt5 == true)
            {
                bunsenBrenner.istTemp[3] -= (bunsenBrenner.BB4_Zieltemp[4] - bunsenBrenner.BB4_Zieltemp[3]) / bunsenBrenner.BB1_Zeit[4];
            }
            //---------------------------------------
            else if (bunsenBrenner.istTemp[3] <= 25)
            {
                bunsenBrenner.istTemp[3] = 25;
                GameObject newWrench = Instantiate(wrench);
                newWrench.transform.SetParent(slotWrench);
                newWrench.transform.tag = "Falsch";
                slotWrenchTiegel.transform.GetChild(0).tag = "Empty";
            }
        }
        #endregion
        //Pleuel Slot
        #region Pleuel
        else if (slotPleuelTiegel.transform.childCount > 0 && slotPleuelTiegel.transform.GetChild(0).CompareTag("20SiHot"))
        {
            slotPleuelTiegel.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelLeer;
            slotPleuelTiegel.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelLeer;
            if (bunsenBrenner.istTemp[0] > 25 && bunsenBrenner.istTemp[0] <= bunsenBrenner.BB1_Zieltemp[0])
            {
                bunsenBrenner.istTemp[0] -= bunsenBrenner.BB1_Zieltemp[0] / bunsenBrenner.BB1_Zeit[0];
            }
            else if (bunsenBrenner.istTemp[0] > bunsenBrenner.BB1_Zieltemp[0] && bunsenBrenner.istTemp[0] <= bunsenBrenner.BB1_Zieltemp[1])
            {
                bunsenBrenner.istTemp[0] -= (bunsenBrenner.BB1_Zieltemp[1] - bunsenBrenner.BB1_Zieltemp[0]) / bunsenBrenner.BB1_Zeit[1];
            }
            //3ter Graph Punkt
            else if (bunsenBrenner.istTemp[0] > bunsenBrenner.BB1_Zieltemp[1] && bunsenBrenner.graphPunkt4 == false && bunsenBrenner.graphPunkt5 == false)
            {
                bunsenBrenner.istTemp[0] -= (bunsenBrenner.BB1_Zieltemp[2] - bunsenBrenner.BB1_Zieltemp[1]) / bunsenBrenner.BB1_Zeit[2];
            }
            else if (bunsenBrenner.istTemp[0] > bunsenBrenner.BB1_Zieltemp[1] && bunsenBrenner.istTemp[0] <= bunsenBrenner.BB1_Zieltemp[2] && bunsenBrenner.graphPunkt4 == true && bunsenBrenner.graphPunkt5 == false)
            {
                bunsenBrenner.istTemp[0] -= (bunsenBrenner.BB1_Zieltemp[2] - bunsenBrenner.BB1_Zieltemp[1]) / bunsenBrenner.BB1_Zeit[2];
            }
            //4ter Graph Punkt
            else if (bunsenBrenner.istTemp[0] > bunsenBrenner.BB1_Zieltemp[2] && bunsenBrenner.graphPunkt4 == true && bunsenBrenner.graphPunkt5 == false)
            {
                bunsenBrenner.istTemp[0] -= (bunsenBrenner.BB1_Zieltemp[3] - bunsenBrenner.BB1_Zieltemp[2]) / bunsenBrenner.BB1_Zeit[3];
            }
            else if (bunsenBrenner.istTemp[0] > bunsenBrenner.BB1_Zieltemp[2] && bunsenBrenner.istTemp[0] <= bunsenBrenner.BB1_Zieltemp[3] && bunsenBrenner.graphPunkt4 == true && bunsenBrenner.graphPunkt5 == true)
            {
                bunsenBrenner.istTemp[0] -= (bunsenBrenner.BB1_Zieltemp[2] - bunsenBrenner.BB1_Zieltemp[1]) / bunsenBrenner.BB1_Zeit[2];
            }
            //5ter Graph Punkt
            else if (bunsenBrenner.istTemp[0] > bunsenBrenner.BB1_Zieltemp[3] && bunsenBrenner.graphPunkt4 == true && bunsenBrenner.graphPunkt5 == true)
            {
                bunsenBrenner.istTemp[0] -= (bunsenBrenner.BB1_Zieltemp[4] - bunsenBrenner.BB1_Zieltemp[3]) / bunsenBrenner.BB1_Zeit[4];
            }
            else if (bunsenBrenner.istTemp[0] <= 25)
            {
                bunsenBrenner.istTemp[0] = 25;
                GameObject newPleuel = Instantiate(pleuel);
                newPleuel.transform.SetParent(slotPleuel.transform);
                newPleuel.transform.tag = "Falsch";
                slotPleuelTiegel.transform.GetChild(0).tag = "Empty";
            }
        }
        //40%Si / 60%Ge
        else if (slotPleuelTiegel.transform.childCount > 0 && slotPleuelTiegel.transform.GetChild(0).CompareTag("40SiHot"))
        {
            slotPleuelTiegel.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelLeer;
            slotPleuelTiegel.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelLeer;
            if (bunsenBrenner.istTemp[1] > 25 && bunsenBrenner.istTemp[1] <= bunsenBrenner.BB2_Zieltemp[0])
            {
                bunsenBrenner.istTemp[1] -= bunsenBrenner.BB2_Zieltemp[0] / bunsenBrenner.BB2_Zeit[0];
            }
            else if (bunsenBrenner.istTemp[1] > bunsenBrenner.BB2_Zieltemp[0] && bunsenBrenner.istTemp[1] <= bunsenBrenner.BB2_Zieltemp[1])
            {
                bunsenBrenner.istTemp[1] -= (bunsenBrenner.BB2_Zieltemp[1] - bunsenBrenner.BB2_Zieltemp[0]) / bunsenBrenner.BB2_Zeit[1];
            }
            //3ter Graph Punkt
            else if (bunsenBrenner.istTemp[1] > bunsenBrenner.BB2_Zieltemp[1] && bunsenBrenner.graphPunkt4 == false && bunsenBrenner.graphPunkt5 == false)
            {
                bunsenBrenner.istTemp[1] -= (bunsenBrenner.BB2_Zieltemp[2] - bunsenBrenner.BB2_Zieltemp[1]) / bunsenBrenner.BB1_Zeit[2];
            }
            else if (bunsenBrenner.istTemp[1] > bunsenBrenner.BB2_Zieltemp[1] && bunsenBrenner.istTemp[1] <= bunsenBrenner.BB2_Zieltemp[2] && bunsenBrenner.graphPunkt4 == true && bunsenBrenner.graphPunkt5 == false)
            {
                bunsenBrenner.istTemp[1] -= (bunsenBrenner.BB2_Zieltemp[2] - bunsenBrenner.BB2_Zieltemp[1]) / bunsenBrenner.BB1_Zeit[2];
            }
            //4ter Graph Punkt
            else if (bunsenBrenner.istTemp[1] > bunsenBrenner.BB2_Zieltemp[2] && bunsenBrenner.graphPunkt4 == true && bunsenBrenner.graphPunkt5 == false)
            {
                bunsenBrenner.istTemp[1] -= (bunsenBrenner.BB2_Zieltemp[3] - bunsenBrenner.BB2_Zieltemp[2]) / bunsenBrenner.BB1_Zeit[3];
            }
            else if (bunsenBrenner.istTemp[1] > bunsenBrenner.BB2_Zieltemp[2] && bunsenBrenner.istTemp[1] <= bunsenBrenner.BB2_Zieltemp[3] && bunsenBrenner.graphPunkt4 == true && bunsenBrenner.graphPunkt5 == true)
            {
                bunsenBrenner.istTemp[1] -= (bunsenBrenner.BB2_Zieltemp[2] - bunsenBrenner.BB2_Zieltemp[1]) / bunsenBrenner.BB1_Zeit[2];
            }
            //5ter Graph Punkt
            else if (bunsenBrenner.istTemp[1] > bunsenBrenner.BB2_Zieltemp[3] && bunsenBrenner.graphPunkt4 == true && bunsenBrenner.graphPunkt5 == true)
            {
                bunsenBrenner.istTemp[1] -= (bunsenBrenner.BB2_Zieltemp[4] - bunsenBrenner.BB2_Zieltemp[3]) / bunsenBrenner.BB1_Zeit[4];
            }
            else if (bunsenBrenner.istTemp[1] <= 25)
            {
                bunsenBrenner.istTemp[1] = 25;
                GameObject newPleuel = Instantiate(pleuel);
                newPleuel.transform.SetParent(slotPleuel.transform);
                newPleuel.transform.tag = "Falsch";
                slotPleuelTiegel.transform.GetChild(0).tag = "Empty";
            }
        }
        //60%Si / 40%Ge
        else if (slotPleuelTiegel.transform.childCount > 0 && slotPleuelTiegel.transform.GetChild(0).CompareTag("60SiHot"))
        {
            slotPleuelTiegel.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelLeer;
            slotPleuelTiegel.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelLeer;
            if (bunsenBrenner.istTemp[2] > 25 && bunsenBrenner.istTemp[2] <= bunsenBrenner.BB3_Zieltemp[0])
            {
                bunsenBrenner.istTemp[2] -= bunsenBrenner.BB3_Zieltemp[0] / bunsenBrenner.BB3_Zeit[0];
            }
            else if (bunsenBrenner.istTemp[2] > bunsenBrenner.BB3_Zieltemp[0] && bunsenBrenner.istTemp[2] <= bunsenBrenner.BB3_Zieltemp[1])
            {
                bunsenBrenner.istTemp[2] -= (bunsenBrenner.BB3_Zieltemp[1] - bunsenBrenner.BB3_Zieltemp[0]) / bunsenBrenner.BB3_Zeit[1];
            }
            //3ter Graph Punkt
            else if (bunsenBrenner.istTemp[2] > bunsenBrenner.BB3_Zieltemp[1] && bunsenBrenner.graphPunkt4 == false && bunsenBrenner.graphPunkt5 == false)
            {
                bunsenBrenner.istTemp[2] -= (bunsenBrenner.BB3_Zieltemp[2] - bunsenBrenner.BB3_Zieltemp[1]) / bunsenBrenner.BB1_Zeit[2];
            }
            else if (bunsenBrenner.istTemp[2] > bunsenBrenner.BB3_Zieltemp[1] && bunsenBrenner.istTemp[2] <= bunsenBrenner.BB3_Zieltemp[2] && bunsenBrenner.graphPunkt4 == true && bunsenBrenner.graphPunkt5 == false)
            {
                bunsenBrenner.istTemp[2] -= (bunsenBrenner.BB3_Zieltemp[2] - bunsenBrenner.BB3_Zieltemp[1]) / bunsenBrenner.BB1_Zeit[2];
            }
            //4ter Graph Punkt
            else if (bunsenBrenner.istTemp[2] > bunsenBrenner.BB3_Zieltemp[2] && bunsenBrenner.graphPunkt4 == true && bunsenBrenner.graphPunkt5 == false)
            {
                bunsenBrenner.istTemp[2] -= (bunsenBrenner.BB3_Zieltemp[3] - bunsenBrenner.BB3_Zieltemp[2]) / bunsenBrenner.BB1_Zeit[3];
            }
            else if (bunsenBrenner.istTemp[2] > bunsenBrenner.BB3_Zieltemp[2] && bunsenBrenner.istTemp[2] <= bunsenBrenner.BB3_Zieltemp[3] && bunsenBrenner.graphPunkt4 == true && bunsenBrenner.graphPunkt5 == true)
            {
                bunsenBrenner.istTemp[2] -= (bunsenBrenner.BB3_Zieltemp[2] - bunsenBrenner.BB3_Zieltemp[1]) / bunsenBrenner.BB1_Zeit[2];
            }
            //5ter Graph Punkt
            else if (bunsenBrenner.istTemp[2] > bunsenBrenner.BB3_Zieltemp[3] && bunsenBrenner.graphPunkt4 == true && bunsenBrenner.graphPunkt5 == true)
            {
                bunsenBrenner.istTemp[2] -= (bunsenBrenner.BB3_Zieltemp[4] - bunsenBrenner.BB3_Zieltemp[3]) / bunsenBrenner.BB1_Zeit[4];
            }
            else if (bunsenBrenner.istTemp[2] <= 25)
            {
                bunsenBrenner.istTemp[2] = 25;
                GameObject newPleuel = Instantiate(pleuel);
                newPleuel.transform.SetParent(slotPleuel.transform);
                newPleuel.transform.tag = "Falsch";
                slotPleuelTiegel.transform.GetChild(0).tag = "Empty";
            }
        }
        //80%Si / 20%Ge
        else if (slotPleuelTiegel.transform.childCount > 0 && slotPleuelTiegel.transform.GetChild(0).CompareTag("80SiHot"))
        {
            slotPleuelTiegel.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelLeer;
            slotPleuelTiegel.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelLeer;
            if (bunsenBrenner.istTemp[3] > 25 && bunsenBrenner.istTemp[3] <= bunsenBrenner.BB4_Zieltemp[0])
            {
                bunsenBrenner.istTemp[3] -= bunsenBrenner.BB4_Zieltemp[0] / bunsenBrenner.BB4_Zeit[0];
            }
            else if (bunsenBrenner.istTemp[3] > bunsenBrenner.BB4_Zieltemp[0] && bunsenBrenner.istTemp[3] <= bunsenBrenner.BB4_Zieltemp[1])
            {
                bunsenBrenner.istTemp[3] -= (bunsenBrenner.BB4_Zieltemp[1] - bunsenBrenner.BB4_Zieltemp[0]) / bunsenBrenner.BB4_Zeit[1];
            }
            //3ter Graph Punkt
            else if (bunsenBrenner.istTemp[3] > bunsenBrenner.BB4_Zieltemp[1] && bunsenBrenner.graphPunkt4 == false && bunsenBrenner.graphPunkt5 == false)
            {
                bunsenBrenner.istTemp[3] -= (bunsenBrenner.BB4_Zieltemp[2] - bunsenBrenner.BB4_Zieltemp[1]) / bunsenBrenner.BB1_Zeit[2];
            }
            else if (bunsenBrenner.istTemp[3] > bunsenBrenner.BB4_Zieltemp[1] && bunsenBrenner.istTemp[3] <= bunsenBrenner.BB4_Zieltemp[2] && bunsenBrenner.graphPunkt4 == true && bunsenBrenner.graphPunkt5 == false)
            {
                bunsenBrenner.istTemp[3] -= (bunsenBrenner.BB4_Zieltemp[2] - bunsenBrenner.BB4_Zieltemp[1]) / bunsenBrenner.BB1_Zeit[2];
            }
            //4ter Graph Punkt
            else if (bunsenBrenner.istTemp[3] > bunsenBrenner.BB4_Zieltemp[2] && bunsenBrenner.graphPunkt4 == true && bunsenBrenner.graphPunkt5 == false)
            {
                bunsenBrenner.istTemp[3] -= (bunsenBrenner.BB4_Zieltemp[3] - bunsenBrenner.BB4_Zieltemp[2]) / bunsenBrenner.BB1_Zeit[3];
            }
            else if (bunsenBrenner.istTemp[3] > bunsenBrenner.BB4_Zieltemp[2] && bunsenBrenner.istTemp[3] <= bunsenBrenner.BB4_Zieltemp[3] && bunsenBrenner.graphPunkt4 == true && bunsenBrenner.graphPunkt5 == true)
            {
                bunsenBrenner.istTemp[3] -= (bunsenBrenner.BB4_Zieltemp[2] - bunsenBrenner.BB4_Zieltemp[1]) / bunsenBrenner.BB1_Zeit[2];
            }
            //5ter Graph Punkt
            else if (bunsenBrenner.istTemp[3] > bunsenBrenner.BB4_Zieltemp[3] && bunsenBrenner.graphPunkt4 == true && bunsenBrenner.graphPunkt5 == true)
            {
                bunsenBrenner.istTemp[3] -= (bunsenBrenner.BB4_Zieltemp[4] - bunsenBrenner.BB4_Zieltemp[3]) / bunsenBrenner.BB1_Zeit[4];
            }
            else if (bunsenBrenner.istTemp[3] <= 25)
            {
                bunsenBrenner.istTemp[3] = 25;
                GameObject newPleuel = Instantiate(pleuel);
                newPleuel.transform.SetParent(slotPleuel.transform);
                newPleuel.transform.tag = "Falsch";
                slotPleuelTiegel.transform.GetChild(0).tag = "Empty";
            }
        }
        #endregion
        //Zahnrad Slot
        #region Zahnrad
        else if (slotZahnradTiegel.transform.childCount > 0 && slotZahnradTiegel.transform.GetChild(0).CompareTag("20SiHot"))
        {
            slotZahnradTiegel.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelLeer;
            slotZahnradTiegel.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelLeer;
            if (bunsenBrenner.istTemp[0] > 25 && bunsenBrenner.istTemp[0] <= bunsenBrenner.BB1_Zieltemp[0])
            {
                bunsenBrenner.istTemp[0] -= bunsenBrenner.BB1_Zieltemp[0] / bunsenBrenner.BB1_Zeit[0];
            }
            else if (bunsenBrenner.istTemp[0] > bunsenBrenner.BB1_Zieltemp[0] && bunsenBrenner.istTemp[0] <= bunsenBrenner.BB1_Zieltemp[1])
            {
                bunsenBrenner.istTemp[0] -= (bunsenBrenner.BB1_Zieltemp[1] - bunsenBrenner.BB1_Zieltemp[0]) / bunsenBrenner.BB1_Zeit[1];
            }
            //3ter Graph Punkt
            else if (bunsenBrenner.istTemp[0] > bunsenBrenner.BB1_Zieltemp[1] && bunsenBrenner.graphPunkt4 == false && bunsenBrenner.graphPunkt5 == false)
            {
                bunsenBrenner.istTemp[0] -= (bunsenBrenner.BB1_Zieltemp[2] - bunsenBrenner.BB1_Zieltemp[1]) / bunsenBrenner.BB1_Zeit[2];
            }
            else if (bunsenBrenner.istTemp[0] > bunsenBrenner.BB1_Zieltemp[1] && bunsenBrenner.istTemp[0] <= bunsenBrenner.BB1_Zieltemp[2] && bunsenBrenner.graphPunkt4 == true && bunsenBrenner.graphPunkt5 == false)
            {
                bunsenBrenner.istTemp[0] -= (bunsenBrenner.BB1_Zieltemp[2] - bunsenBrenner.BB1_Zieltemp[1]) / bunsenBrenner.BB1_Zeit[2];
            }
            //4ter Graph Punkt
            else if (bunsenBrenner.istTemp[0] > bunsenBrenner.BB1_Zieltemp[2] && bunsenBrenner.graphPunkt4 == true && bunsenBrenner.graphPunkt5 == false)
            {
                bunsenBrenner.istTemp[0] -= (bunsenBrenner.BB1_Zieltemp[3] - bunsenBrenner.BB1_Zieltemp[2]) / bunsenBrenner.BB1_Zeit[3];
            }
            else if (bunsenBrenner.istTemp[0] > bunsenBrenner.BB1_Zieltemp[2] && bunsenBrenner.istTemp[0] <= bunsenBrenner.BB1_Zieltemp[3] && bunsenBrenner.graphPunkt4 == true && bunsenBrenner.graphPunkt5 == true)
            {
                bunsenBrenner.istTemp[0] -= (bunsenBrenner.BB1_Zieltemp[2] - bunsenBrenner.BB1_Zieltemp[1]) / bunsenBrenner.BB1_Zeit[2];
            }
            //5ter Graph Punkt
            else if (bunsenBrenner.istTemp[0] > bunsenBrenner.BB1_Zieltemp[3] && bunsenBrenner.graphPunkt4 == true && bunsenBrenner.graphPunkt5 == true)
            {
                bunsenBrenner.istTemp[0] -= (bunsenBrenner.BB1_Zieltemp[4] - bunsenBrenner.BB1_Zieltemp[3]) / bunsenBrenner.BB1_Zeit[4];
            }
            else if (bunsenBrenner.istTemp[0] <= 25)
            {
                bunsenBrenner.istTemp[0] = 25;
                GameObject newZahnrad = Instantiate(zahnrad);
                newZahnrad.transform.SetParent(slotZahnrad.transform);
                newZahnrad.transform.tag = "Falsch";
                slotZahnradTiegel.transform.GetChild(0).tag = "Empty";
            }
        }
        //40%Si / 60%Ge
        else if (slotZahnradTiegel.transform.childCount > 0 && slotZahnradTiegel.transform.GetChild(0).CompareTag("40SiHot"))
        {
            slotZahnradTiegel.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelLeer;
            slotZahnradTiegel.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelLeer;
            if (bunsenBrenner.istTemp[1] > 25 && bunsenBrenner.istTemp[1] <= bunsenBrenner.BB2_Zieltemp[0])
            {
                bunsenBrenner.istTemp[1] -= bunsenBrenner.BB2_Zieltemp[0] / bunsenBrenner.BB2_Zeit[0];
            }
            else if (bunsenBrenner.istTemp[1] > bunsenBrenner.BB2_Zieltemp[0] && bunsenBrenner.istTemp[1] <= bunsenBrenner.BB2_Zieltemp[1])
            {
                bunsenBrenner.istTemp[1] -= (bunsenBrenner.BB2_Zieltemp[1] - bunsenBrenner.BB2_Zieltemp[0]) / bunsenBrenner.BB2_Zeit[1];
            }
            //3ter Graph Punkt
            else if (bunsenBrenner.istTemp[1] > bunsenBrenner.BB2_Zieltemp[1] && bunsenBrenner.graphPunkt4 == false && bunsenBrenner.graphPunkt5 == false)
            {
                bunsenBrenner.istTemp[1] -= (bunsenBrenner.BB2_Zieltemp[2] - bunsenBrenner.BB2_Zieltemp[1]) / bunsenBrenner.BB1_Zeit[2];
            }
            else if (bunsenBrenner.istTemp[1] > bunsenBrenner.BB2_Zieltemp[1] && bunsenBrenner.istTemp[1] <= bunsenBrenner.BB2_Zieltemp[2] && bunsenBrenner.graphPunkt4 == true && bunsenBrenner.graphPunkt5 == false)
            {
                bunsenBrenner.istTemp[1] -= (bunsenBrenner.BB2_Zieltemp[2] - bunsenBrenner.BB2_Zieltemp[1]) / bunsenBrenner.BB1_Zeit[2];
            }
            //4ter Graph Punkt
            else if (bunsenBrenner.istTemp[1] > bunsenBrenner.BB2_Zieltemp[2] && bunsenBrenner.graphPunkt4 == true && bunsenBrenner.graphPunkt5 == false)
            {
                bunsenBrenner.istTemp[1] -= (bunsenBrenner.BB2_Zieltemp[3] - bunsenBrenner.BB2_Zieltemp[2]) / bunsenBrenner.BB1_Zeit[3];
            }
            else if (bunsenBrenner.istTemp[1] > bunsenBrenner.BB2_Zieltemp[2] && bunsenBrenner.istTemp[1] <= bunsenBrenner.BB2_Zieltemp[3] && bunsenBrenner.graphPunkt4 == true && bunsenBrenner.graphPunkt5 == true)
            {
                bunsenBrenner.istTemp[1] -= (bunsenBrenner.BB2_Zieltemp[2] - bunsenBrenner.BB2_Zieltemp[1]) / bunsenBrenner.BB1_Zeit[2];
            }
            //5ter Graph Punkt
            else if (bunsenBrenner.istTemp[1] > bunsenBrenner.BB2_Zieltemp[3] && bunsenBrenner.graphPunkt4 == true && bunsenBrenner.graphPunkt5 == true)
            {
                bunsenBrenner.istTemp[1] -= (bunsenBrenner.BB2_Zieltemp[4] - bunsenBrenner.BB2_Zieltemp[3]) / bunsenBrenner.BB1_Zeit[4];
            }
            else if (bunsenBrenner.istTemp[1] <= 25)
            {
                bunsenBrenner.istTemp[1] = 25;
                GameObject newZahnrad = Instantiate(zahnrad);
                newZahnrad.transform.SetParent(slotZahnrad.transform);
                newZahnrad.transform.tag = "Richtig";
                slotZahnradTiegel.transform.GetChild(0).tag = "Empty";
            }
        }
        //60%Si / 40%Ge
        else if (slotZahnradTiegel.transform.childCount > 0 && slotZahnradTiegel.transform.GetChild(0).CompareTag("60SiHot"))
        {
            slotZahnradTiegel.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelLeer;
            slotZahnradTiegel.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelLeer;
            if (bunsenBrenner.istTemp[2] > 25 && bunsenBrenner.istTemp[2] <= bunsenBrenner.BB3_Zieltemp[0])
            {
                bunsenBrenner.istTemp[2] -= bunsenBrenner.BB3_Zieltemp[0] / bunsenBrenner.BB3_Zeit[0];
            }
            else if (bunsenBrenner.istTemp[2] > bunsenBrenner.BB3_Zieltemp[0] && bunsenBrenner.istTemp[2] <= bunsenBrenner.BB3_Zieltemp[1])
            {
                bunsenBrenner.istTemp[2] -= (bunsenBrenner.BB3_Zieltemp[1] - bunsenBrenner.BB3_Zieltemp[0]) / bunsenBrenner.BB3_Zeit[1];
            }
            //3ter Graph Punkt
            else if (bunsenBrenner.istTemp[2] > bunsenBrenner.BB3_Zieltemp[1] && bunsenBrenner.graphPunkt4 == false && bunsenBrenner.graphPunkt5 == false)
            {
                bunsenBrenner.istTemp[2] -= (bunsenBrenner.BB3_Zieltemp[2] - bunsenBrenner.BB3_Zieltemp[1]) / bunsenBrenner.BB1_Zeit[2];
            }
            else if (bunsenBrenner.istTemp[2] > bunsenBrenner.BB3_Zieltemp[1] && bunsenBrenner.istTemp[2] <= bunsenBrenner.BB3_Zieltemp[2] && bunsenBrenner.graphPunkt4 == true && bunsenBrenner.graphPunkt5 == false)
            {
                bunsenBrenner.istTemp[2] -= (bunsenBrenner.BB3_Zieltemp[2] - bunsenBrenner.BB3_Zieltemp[1]) / bunsenBrenner.BB1_Zeit[2];
            }
            //4ter Graph Punkt
            else if (bunsenBrenner.istTemp[2] > bunsenBrenner.BB3_Zieltemp[2] && bunsenBrenner.graphPunkt4 == true && bunsenBrenner.graphPunkt5 == false)
            {
                bunsenBrenner.istTemp[2] -= (bunsenBrenner.BB3_Zieltemp[3] - bunsenBrenner.BB3_Zieltemp[2]) / bunsenBrenner.BB1_Zeit[3];
            }
            else if (bunsenBrenner.istTemp[2] > bunsenBrenner.BB3_Zieltemp[2] && bunsenBrenner.istTemp[2] <= bunsenBrenner.BB3_Zieltemp[3] && bunsenBrenner.graphPunkt4 == true && bunsenBrenner.graphPunkt5 == true)
            {
                bunsenBrenner.istTemp[2] -= (bunsenBrenner.BB3_Zieltemp[2] - bunsenBrenner.BB3_Zieltemp[1]) / bunsenBrenner.BB1_Zeit[2];
            }
            //5ter Graph Punkt
            else if (bunsenBrenner.istTemp[2] > bunsenBrenner.BB3_Zieltemp[3] && bunsenBrenner.graphPunkt4 == true && bunsenBrenner.graphPunkt5 == true)
            {
                bunsenBrenner.istTemp[2] -= (bunsenBrenner.BB3_Zieltemp[4] - bunsenBrenner.BB3_Zieltemp[3]) / bunsenBrenner.BB1_Zeit[4];
            }
            else if (bunsenBrenner.istTemp[2] <= 25)
            {
                bunsenBrenner.istTemp[2] = 25;
                GameObject newZahnrad = Instantiate(zahnrad);
                newZahnrad.transform.SetParent(slotZahnrad.transform);
                newZahnrad.transform.tag = "Falsch";
                slotZahnradTiegel.transform.GetChild(0).tag = "Empty";
            }
        }
        //80%Si / 20%Ge
        else if (slotZahnradTiegel.transform.childCount > 0 && slotZahnradTiegel.transform.GetChild(0).CompareTag("80SiHot"))
        {
            slotZahnradTiegel.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelLeer;
            slotZahnradTiegel.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelLeer;
            if (bunsenBrenner.istTemp[3] > 25 && bunsenBrenner.istTemp[3] <= bunsenBrenner.BB4_Zieltemp[0])
            {
                bunsenBrenner.istTemp[3] -= bunsenBrenner.BB4_Zieltemp[0] / bunsenBrenner.BB4_Zeit[0];
            }
            else if (bunsenBrenner.istTemp[3] > bunsenBrenner.BB4_Zieltemp[0] && bunsenBrenner.istTemp[3] <= bunsenBrenner.BB4_Zieltemp[1])
            {
                bunsenBrenner.istTemp[3] -= (bunsenBrenner.BB4_Zieltemp[1] - bunsenBrenner.BB4_Zieltemp[0]) / bunsenBrenner.BB4_Zeit[1];
            }
            //3ter Graph Punkt
            else if (bunsenBrenner.istTemp[3] > bunsenBrenner.BB4_Zieltemp[1] && bunsenBrenner.graphPunkt4 == false && bunsenBrenner.graphPunkt5 == false)
            {
                bunsenBrenner.istTemp[3] -= (bunsenBrenner.BB4_Zieltemp[2] - bunsenBrenner.BB4_Zieltemp[1]) / bunsenBrenner.BB1_Zeit[2];
            }
            else if (bunsenBrenner.istTemp[3] > bunsenBrenner.BB4_Zieltemp[1] && bunsenBrenner.istTemp[3] <= bunsenBrenner.BB4_Zieltemp[2] && bunsenBrenner.graphPunkt4 == true && bunsenBrenner.graphPunkt5 == false)
            {
                bunsenBrenner.istTemp[3] -= (bunsenBrenner.BB4_Zieltemp[2] - bunsenBrenner.BB4_Zieltemp[1]) / bunsenBrenner.BB1_Zeit[2];
            }
            //4ter Graph Punkt
            else if (bunsenBrenner.istTemp[3] > bunsenBrenner.BB4_Zieltemp[2] && bunsenBrenner.graphPunkt4 == true && bunsenBrenner.graphPunkt5 == false)
            {
                bunsenBrenner.istTemp[3] -= (bunsenBrenner.BB4_Zieltemp[3] - bunsenBrenner.BB4_Zieltemp[2]) / bunsenBrenner.BB1_Zeit[3];
            }
            else if (bunsenBrenner.istTemp[3] > bunsenBrenner.BB4_Zieltemp[2] && bunsenBrenner.istTemp[3] <= bunsenBrenner.BB4_Zieltemp[3] && bunsenBrenner.graphPunkt4 == true && bunsenBrenner.graphPunkt5 == true)
            {
                bunsenBrenner.istTemp[3] -= (bunsenBrenner.BB4_Zieltemp[2] - bunsenBrenner.BB4_Zieltemp[1]) / bunsenBrenner.BB1_Zeit[2];
            }
            //5ter Graph Punkt
            else if (bunsenBrenner.istTemp[3] > bunsenBrenner.BB4_Zieltemp[3] && bunsenBrenner.graphPunkt4 == true && bunsenBrenner.graphPunkt5 == true)
            {
                bunsenBrenner.istTemp[3] -= (bunsenBrenner.BB4_Zieltemp[4] - bunsenBrenner.BB4_Zieltemp[3]) / bunsenBrenner.BB1_Zeit[4];
            }
            else if (bunsenBrenner.istTemp[3] <= 25)
            {
                bunsenBrenner.istTemp[3] = 25;
                GameObject newZahnrad = Instantiate(zahnrad);
                newZahnrad.transform.SetParent(slotZahnrad.transform);
                newZahnrad.transform.tag = "Falsch";
                slotZahnradTiegel.transform.GetChild(0).tag = "Empty";
            }
        }
        #endregion
        waiting = false;
        #endregion
    }
}
