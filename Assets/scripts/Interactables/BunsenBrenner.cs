using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BunsenBrenner : MonoBehaviour
{

    public GameObject[] bunsenBrennerObjekt;
    public GameObject[] gasSchalter;
    public Transform[] bunsenBrennerTransf;
    [SerializeField] Transform tempGOTransf;
    [SerializeField] GameObject tempGO;
    //Array zum abspeichern der Rate in °C der jeweiligen Bunsen Brenner
    [Header("Soll der 4. / 4. und 5. Graphpunkt berechnet werden?")]
    [Header("Nur auswählen, wenn auch valide Daten angegeben sind!")]
    public bool graphPunkt4, graphPunkt5;
    [Header("Element 0 ist die niedrigste Temp. und geht aufwärts.")]
    //--------------Zieltemperaturen-----------------
    public List<float> BB1_Zieltemp = new List<float>();
    public List<float> BB2_Zieltemp = new List<float>();
    public List<float> BB3_Zieltemp = new List<float>();
    public List<float> BB4_Zieltemp = new List<float>();
    //-----------Zeiten für Bunsen Brenner-----------
    public List<int> BB1_Zeit = new List<int>();
    public List<int> BB2_Zeit = new List<int>();
    public List<int> BB3_Zeit = new List<int>();
    public List<int> BB4_Zeit = new List<int>();
    //-----------------------------------------------
    public float[] istTemp = new float[4]; //Aktuelle Temperatur der Tiegel
    public static Transform instance;
    public CameraFollow followCam;
    public static bool hauptGasSchalter = false, platzGasSchalter = false, bBGasSchalter = false, waiting = false; //Zur Überprüfung ob die jeweiligen Gas Schalter bereits betätigt wurden
    public Text ausgabeText;
    public Transform slot1, slot2, slot3, slot4; //Bunsen Brenner Slots
    [SerializeField] GameObject flamme1, flamme2, flamme3, flamme4; //Die 3D Flammen im Raum 3
    public static bool flamme1Bool, flamme2Bool, flamme3Bool, flamme4Bool; //Zur Überprüfung, ob eine Flamme eingeschalten ist für die Rechnung
    [SerializeField] GameObject tiegelZahnrad;
    [SerializeField] Window_Graph windowGraph; //Der Graph zur Anzeige der Abkühlkurve des 20er Tiegels
    [SerializeField] Window_Graph_Tiegel2 windowGraphTiegel2; //Der Graph zur Anzeige der Abkühlkurve des 40er Tiegels
    [SerializeField] Window_Graph_Tiegel3 windowGraphTiegel3; //Der Graph zur Anzeige der Abkühlkurve des 60er Tiegels
    [SerializeField] Window_Graph_Tiegel4 windowGraphTiegel4; //Der Graph zur Anzeige der Abkühlkurve des 80er Tiegels
    [SerializeField] GameObject[] tiegelAufBB; //Zum Abändern des Materials eines Tiegels, falls dieser erhitzt wurde
    int tiegelFarbe, tiegel2Farbe, tiegel3Farbe, tiegel4Farbe; //Wert für die Farbe der Graphen (Farbwert selbst wird im Graphen selbst eingestellt, nicht über diese Zahl)
    [SerializeField] ParticleSystem[] BunsenBrennerFlammen; //Zum Anzeigen des Partikelsystems der Flammen
    bool tiegel1Heated, tiegel2Heated, tiegel3Heated, tiegel4Heated; //Absicherung, dass die Tiegel aufgeheizt sind
    [SerializeField] Material[] tiegelMat; //Material der Tiegel zum abändern wenn die erhitzt oder abgekühlt sind
    [SerializeField] Sprite[] tiegelSprite; //Gleiches wie Material, nur das Sprite des jeweiligen Tiegels
    bool zeigeVerbrennung = false; //Für die Überprüfung, ob des Verbrannt Bild gezeigt werden soll
    public static bool verbrannt = false; //Überprüfung, ob der Spieler sich bereits verbrannt hat
    [SerializeField] GameObject verbranntFenster; //Infofenster falls man sich verbrannt hat
    [SerializeField] GameObject erstVerarztenFenster; //Infofenster falls man mit Verbrennung weiter arbeiten möchte
    [SerializeField] Transform infoBBAusgeschaltet; //Falls man den Raum verlassen möchte, solange das Gas noch eingeschalten ist
    public static bool tiegelLocked20, tiegelLocked40, tiegelLocked60, tiegelLocked80; //Zum verhindern, dass man die Tiegel ziehen kann während sie erhitzt werden
    bool[] tiegel1Graph = new bool[6] { false, false, false, false, false, false };
    bool[] tiegel2Graph = new bool[6] { false, false, false, false, false, false };
    bool[] tiegel3Graph = new bool[6] { false, false, false, false, false, false };
    bool[] tiegel4Graph = new bool[6] { false, false, false, false, false, false };
    [SerializeField] GameObject hinweisToolSchmelzen;

    // Use this for initialization
    void Start()
    {
        //-------------Bei Neustart des Spiels werden alle Variablen zurückgesetzt----------------
        hauptGasSchalter = false;
        platzGasSchalter = false;
        bBGasSchalter = false;
        waiting = false;
        instance = null;
        verbrannt = false;
        tiegelLocked20 = false;
        tiegelLocked40 = false;
        tiegelLocked60 = false;
        tiegelLocked80 = false;
        flamme1Bool = false;
        flamme2Bool = false;
        flamme3Bool = false;
        flamme4Bool = false;
        //----------------------------------------------------------------------------------------
        Vector3[] shuffleArray = new Vector3[bunsenBrennerTransf.Length];
        List<int> usedRnd = new List<int>();
        for (int i = 0; i < bunsenBrennerObjekt.Length; i++)
        {
            shuffleArray[i] = bunsenBrennerObjekt[i].transform.position;
        }
        //Mische die Bunsen Brenner, sodass die Studenten nicht schummeln können
        for (int i = 0; i < bunsenBrennerObjekt.Length; i++)
        {
            int rnd = UnityEngine.Random.Range(0, bunsenBrennerObjekt.Length);
            while (usedRnd.Contains(rnd))
            {
                rnd = UnityEngine.Random.Range(0, bunsenBrennerObjekt.Length);
            }
            usedRnd.Add(rnd);
            bunsenBrennerObjekt[rnd].transform.position = shuffleArray[i];
        }
        for (int i = 0; i < istTemp.Length; i++)
        {
            istTemp[i] = 25;
        }
    }

    // Update is called once per frame
    void Update()
    {
        #region Tiegel auf Bunsenbrenner
        //Falls ein Tiegel auf einen Bunsen Brenner in der 2D Ansicht gelegt wird, zeige diesen auch in 3D
        if (slot1.transform.childCount > 0)
        {
            tiegelAufBB[0].SetActive(true);
        }
        else
        {
            tiegelAufBB[0].SetActive(false);
        }
        if (slot2.transform.childCount > 0)
        {
            tiegelAufBB[1].SetActive(true);
        }
        else
        {
            tiegelAufBB[1].SetActive(false);
        }
        if (slot3.transform.childCount > 0)
        {
            tiegelAufBB[2].SetActive(true);
        }
        else
        {
            tiegelAufBB[2].SetActive(false);
        }
        if (slot4.transform.childCount > 0)
        {
            tiegelAufBB[3].SetActive(true);
        }
        else
        {
            tiegelAufBB[3].SetActive(false);
        }
        #endregion
        //CheckInstance();
        //Wenn das Hauptgas und das Platzgas eingeschalten ist, dann rufe die Bunsen Brenner Rechnung auf
        if (hauptGasSchalter && platzGasSchalter && waiting == false)
        {
            //time = DateTime.Now.Ticks;
            StartCoroutine(BunsenBrennerRechnung());
        }
        //falls eines der beiden aus geht / ist, dann schalte alle Flammen aus
        else if (hauptGasSchalter == false || platzGasSchalter == false)
        {
            waiting = false;
            flamme1.SetActive(false);
            BunsenBrennerFlammen[0].gameObject.SetActive(false);
            flamme2.SetActive(false);
            BunsenBrennerFlammen[1].gameObject.SetActive(false);
            flamme3.SetActive(false);
            BunsenBrennerFlammen[2].gameObject.SetActive(false);
            flamme4.SetActive(false);
            BunsenBrennerFlammen[3].gameObject.SetActive(false);
        }
        //Falls man die Flamme anmachen möchte, ohne das ein Tiegel auf dem Bunsen Brenner liegt, dann zeige den Verbrannt Hinweis. Anschließend kann bis zur Verarztung nicht weiter getan werden
        if (flamme1.activeInHierarchy == true && slot1.transform.childCount == 0 && zeigeVerbrennung == false)
        {
            zeigeVerbrennung = true;
            verbranntFenster.SetActive(true);
            verbrannt = true;
            flamme1.SetActive(false);
            flamme1Bool = false;
            BunsenBrennerFlammen[0].gameObject.SetActive(false);
        }
        //Gleiches für den zweiten Bunsen Brenner
        else if (flamme2.activeInHierarchy == true && slot2.transform.childCount == 0 && zeigeVerbrennung == false)
        {
            zeigeVerbrennung = true;
            verbranntFenster.SetActive(true);
            verbrannt = true;
            flamme2.SetActive(false);
            flamme2Bool = false;
            BunsenBrennerFlammen[1].gameObject.SetActive(false);
        }
        //Gleiches für den dritten Bunsen Brenner
        else if (flamme3.activeInHierarchy == true && slot3.transform.childCount == 0 && zeigeVerbrennung == false)
        {
            zeigeVerbrennung = true;
            verbranntFenster.SetActive(true);
            verbrannt = true;
            flamme3.SetActive(false);
            flamme3Bool = false;
            BunsenBrennerFlammen[2].gameObject.SetActive(false);
        }
        //Gleiches für den vierten Bunsen Brenner
        else if (flamme4.activeInHierarchy == true && slot4.transform.childCount == 0 && zeigeVerbrennung == false)
        {
            zeigeVerbrennung = true;
            verbranntFenster.SetActive(true);
            verbrannt = true;
            flamme4.SetActive(false);
            flamme4Bool = false;
            BunsenBrennerFlammen[3].gameObject.SetActive(false);
        }
    }

    public void CheckInstance()
    {
        if (instance == this.bunsenBrennerTransf[0])
        {
            instance = this.bunsenBrennerTransf[0];
            followCam = CameraFollow.instance;

            if (CameraFollow.instance.closeupInteraction)
            {
                bunsenBrennerTransf[0].GetComponent<BoxCollider>().enabled = true;
            }
        }
        else if (instance == this.bunsenBrennerTransf[1])
        {
            instance = this.bunsenBrennerTransf[1];
            followCam = CameraFollow.instance;

            if (CameraFollow.instance.closeupInteraction)
            {
                bunsenBrennerTransf[0].GetComponent<BoxCollider>().enabled = true;
            }

        }
        else if (instance == this.bunsenBrennerTransf[2])
        {
            instance = this.bunsenBrennerTransf[2];
            followCam = CameraFollow.instance;

            if (CameraFollow.instance.closeupInteraction)
            {
                bunsenBrennerTransf[0].GetComponent<BoxCollider>().enabled = true;
            }

        }
        else if (instance == this.bunsenBrennerTransf[3])
        {
            instance = this.bunsenBrennerTransf[3];
            followCam = CameraFollow.instance;

            if (CameraFollow.instance.closeupInteraction)
            {
                bunsenBrennerTransf[0].GetComponent<BoxCollider>().enabled = true;
            }

        }
    }

    bool tiegel20Heating;
    public IEnumerator BunsenBrennerRechnung()
    {
        waiting = true;
        //long timetocool = time + new TimeSpan(0, 0, 0, 10, 0).Ticks;
        //Solange das Feuer an ist, berechne die Temperatur der Tiegel
        while (true)
        {
            //time = DateTime.Now.Ticks;
            yield return new WaitForSeconds(1);
            //if (istTemp[0] <= 900 && istTemp[0] >= 650)
            //{
            //    modifier = 0.0f;
            //}
            //else if (time > timetocool)
            //{
            //    modifier = 1.0f;
            //}
            #region Bunsen Brenner Abfrage
            //100% Cu / 0% Al || Wenn der Tiegel auf einem Bunsen Brenner liegt und die jeweilige Flamme an ist, erhitze den Tiegel
            #region 100% Cu / 0% Al
            if (slot1.transform.childCount > 0 || slot2.transform.childCount > 0 || slot3.transform.childCount > 0 || slot4.transform.childCount > 0)
            {
                #region aufheizen
                if (flamme1Bool && (slot1.transform.GetChild(0).CompareTag("20SiCold") || slot1.transform.GetChild(0).CompareTag("20SiHot")) || flamme2Bool && (slot2.transform.GetChild(0).CompareTag("20SiCold") || slot2.transform.GetChild(0).CompareTag("20SiHot")) || flamme3Bool && (slot3.transform.GetChild(0).CompareTag("20SiCold") || slot3.transform.GetChild(0).CompareTag("20SiHot")) || flamme4Bool && (slot4.transform.GetChild(0).CompareTag("20SiCold") || slot4.transform.GetChild(0).CompareTag("20SiHot")))
                {
                    tiegelLocked20 = true;
                    //Wenn der Tiegel aufgeheizt wurde, lösche den vorigen Graphen bei erneutem abkühlen
                    if (tiegel1Heated)
                    {
                        windowGraph.DeleteGraph();
                        tiegel1Heated = false;
                        for (int i = 0; i < tiegel1Graph.Length; i++)
                        {
                            tiegel1Graph[i] = false;
                        }
                    }

                    if (istTemp[0] < BB1_Zieltemp[0])
                    {
                        istTemp[0] += BB1_Zieltemp[0] / BB1_Zeit[0];
                        tiegel1Heated = true;
                    }
                    else if (istTemp[0] < BB1_Zieltemp[1])
                    {
                        istTemp[0] += (BB1_Zieltemp[1] - BB1_Zieltemp[0]) / BB1_Zeit[1];
                        tiegel1Heated = true;
                    }
                    else if (istTemp[0] < BB1_Zieltemp[2])
                    {
                        istTemp[0] += (BB1_Zieltemp[2] - BB1_Zieltemp[1]) / BB1_Zeit[2];
                        tiegel1Heated = true;
                    }
                    //3ter Punkt im Graphen
                    else if (istTemp[0] >= BB1_Zieltemp[2] && graphPunkt4 == false && graphPunkt5 == false)
                    {
                        if (slot1.transform.childCount > 0 && slot1.transform.GetChild(0).CompareTag("20SiCold"))
                        {
                            tiegel1Heated = true;
                            slot1.transform.GetChild(0).tag = "20SiHot";
                            tiegelAufBB[0].gameObject.GetComponent<Renderer>().material = tiegelMat[0];
                            slot1.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[0];
                            slot1.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[0];
                        }
                        else if (slot2.transform.childCount > 0 && slot2.transform.GetChild(0).CompareTag("20SiCold"))
                        {
                            tiegel1Heated = true;
                            slot2.transform.GetChild(0).tag = "20SiHot";
                            tiegelAufBB[1].gameObject.GetComponent<Renderer>().material = tiegelMat[0];
                            slot2.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[0];
                            slot2.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[0];
                        }
                        else if (slot3.transform.childCount > 0 && slot3.transform.GetChild(0).CompareTag("20SiCold"))
                        {
                            tiegel1Heated = true;
                            slot3.transform.GetChild(0).tag = "20SiHot";
                            tiegelAufBB[2].gameObject.GetComponent<Renderer>().material = tiegelMat[0];
                            slot3.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[0];
                            slot3.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[0];
                        }
                        else if (slot4.transform.childCount > 0 && slot4.transform.GetChild(0).CompareTag("20SiCold"))
                        {
                            tiegel1Heated = true;
                            slot4.transform.GetChild(0).tag = "20SiHot";
                            tiegelAufBB[3].gameObject.GetComponent<Renderer>().material = tiegelMat[0];
                            slot4.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[0];
                            slot4.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[0];
                        }
                        istTemp[0] = BB1_Zieltemp[2];
                    }
                    else if (istTemp[0] < BB1_Zieltemp[3] && graphPunkt4 == true && graphPunkt5 == false)
                    {
                        istTemp[0] += (BB1_Zieltemp[3] - BB1_Zieltemp[2]) / BB1_Zeit[3];
                        tiegelFarbe = 20;
                        tiegel1Heated = true;
                    }
                    //4ter Punkt im Graphen
                    else if (istTemp[0] >= BB1_Zieltemp[3] && graphPunkt4 == true && graphPunkt5 == false)
                    {
                        if (slot1.transform.childCount > 0 && slot1.transform.GetChild(0).CompareTag("20SiCold"))
                        {
                            tiegel1Heated = true;
                            slot1.transform.GetChild(0).tag = "20SiHot";
                            tiegelAufBB[0].gameObject.GetComponent<Renderer>().material = tiegelMat[0];
                            slot1.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[0];
                            slot1.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[0];
                        }
                        else if (slot2.transform.childCount > 0 && slot2.transform.GetChild(0).CompareTag("20SiCold"))
                        {
                            tiegel1Heated = true;
                            slot2.transform.GetChild(0).tag = "20SiHot";
                            tiegelAufBB[1].gameObject.GetComponent<Renderer>().material = tiegelMat[0];
                            slot2.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[0];
                            slot2.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[0];
                        }
                        else if (slot3.transform.childCount > 0 && slot3.transform.GetChild(0).CompareTag("20SiCold"))
                        {
                            tiegel1Heated = true;
                            slot3.transform.GetChild(0).tag = "20SiHot";
                            tiegelAufBB[2].gameObject.GetComponent<Renderer>().material = tiegelMat[0];
                            slot3.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[0];
                            slot3.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[0];
                        }
                        else if (slot4.transform.childCount > 0 && slot4.transform.GetChild(0).CompareTag("20SiCold"))
                        {
                            tiegel1Heated = true;
                            slot4.transform.GetChild(0).tag = "20SiHot";
                            tiegelAufBB[3].gameObject.GetComponent<Renderer>().material = tiegelMat[0];
                            slot4.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[0];
                            slot4.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[0];
                        }
                        istTemp[0] = BB1_Zieltemp[3];
                    }
                    else if (istTemp[0] < BB1_Zieltemp[4] && graphPunkt4 == true && graphPunkt5 == true)
                    {
                        istTemp[0] += (BB1_Zieltemp[4] - BB1_Zieltemp[3]) / BB1_Zeit[4];
                        tiegelFarbe = 20;
                        tiegel1Heated = true;
                    }
                    //5ter Graph Punkt
                    else if (istTemp[0] >= BB1_Zieltemp[4] && graphPunkt4 == true && graphPunkt5 == true)
                    {
                        if (slot1.transform.childCount > 0 && slot1.transform.GetChild(0).CompareTag("20SiCold"))
                        {
                            tiegel1Heated = true;
                            slot1.transform.GetChild(0).tag = "20SiHot";
                            tiegelAufBB[0].gameObject.GetComponent<Renderer>().material = tiegelMat[0];
                            slot1.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[0];
                            slot1.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[0];
                        }
                        else if (slot2.transform.childCount > 0 && slot2.transform.GetChild(0).CompareTag("20SiCold"))
                        {
                            tiegel1Heated = true;
                            slot2.transform.GetChild(0).tag = "20SiHot";
                            tiegelAufBB[1].gameObject.GetComponent<Renderer>().material = tiegelMat[0];
                            slot2.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[0];
                            slot2.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[0];
                        }
                        else if (slot3.transform.childCount > 0 && slot3.transform.GetChild(0).CompareTag("20SiCold"))
                        {
                            tiegel1Heated = true;
                            slot3.transform.GetChild(0).tag = "20SiHot";
                            tiegelAufBB[2].gameObject.GetComponent<Renderer>().material = tiegelMat[0];
                            slot3.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[0];
                            slot3.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[0];
                        }
                        else if (slot4.transform.childCount > 0 && slot4.transform.GetChild(0).CompareTag("20SiCold"))
                        {
                            tiegel1Heated = true;
                            slot4.transform.GetChild(0).tag = "20SiHot";
                            tiegelAufBB[3].gameObject.GetComponent<Renderer>().material = tiegelMat[0];
                            slot4.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[0];
                            slot4.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[0];
                        }
                        istTemp[0] = BB1_Zieltemp[4];
                    }
                }
                #endregion
                //Sonst kühl das ganze mit gleichen Raten ab
                else
                {
                    tiegelLocked20 = false;
                    if (istTemp[0] > 25 && istTemp[0] <= BB1_Zieltemp[0])
                    {
                        if (istTemp[0] >= 25 && istTemp[0] < 100)
                        {
                            if (slot1.transform.childCount > 0 && slot1.transform.GetChild(0).CompareTag("20SiHot"))
                            {
                                slot1.transform.GetChild(0).tag = "20SiCold";
                                tiegelAufBB[0].gameObject.GetComponent<Renderer>().material = tiegelMat[1];
                                slot1.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[1];
                                slot1.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[1];
                            }
                            else if (slot2.transform.childCount > 0 && slot2.transform.GetChild(0).CompareTag("20SiHot"))
                            {
                                slot2.transform.GetChild(0).tag = "20SiCold";
                                tiegelAufBB[1].gameObject.GetComponent<Renderer>().material = tiegelMat[1];
                                slot2.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[1];
                                slot2.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[1];
                            }
                            else if (slot3.transform.childCount > 0 && slot3.transform.GetChild(0).CompareTag("20SiHot"))
                            {
                                slot3.transform.GetChild(0).tag = "20SiCold";
                                tiegelAufBB[2].gameObject.GetComponent<Renderer>().material = tiegelMat[1];
                                slot3.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[1];
                                slot3.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[1];
                            }
                            else if (slot4.transform.childCount > 0 && slot4.transform.GetChild(0).CompareTag("20SiHot"))
                            {
                                slot4.transform.GetChild(0).tag = "20SiCold";
                                tiegelAufBB[3].gameObject.GetComponent<Renderer>().material = tiegelMat[1];
                                slot4.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[1];
                                slot4.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[1];
                            }
                        }
                        if (slot1.transform.childCount > 0 && (slot1.transform.GetChild(0).CompareTag("20SiHot") || slot1.transform.GetChild(0).CompareTag("20SiCold")))
                        {
                            tiegelFarbe = 20;
                        }
                        else if (slot2.transform.childCount > 0 && (slot2.transform.GetChild(0).CompareTag("20SiHot") || slot2.transform.GetChild(0).CompareTag("20SiCold")))
                        {
                            tiegelFarbe = 40;
                        }
                        else if (slot3.transform.childCount > 0 && (slot3.transform.GetChild(0).CompareTag("20SiHot") || slot3.transform.GetChild(0).CompareTag("20SiCold")))
                        {
                            tiegelFarbe = 60;
                        }
                        else if (slot4.transform.childCount > 0 && (slot4.transform.GetChild(0).CompareTag("20SiHot") || slot4.transform.GetChild(0).CompareTag("20SiCold")))
                        {
                            tiegelFarbe = 80;
                        }
                        //-------------------Showgraph--------------------
                        if (tiegel1Graph[0] == false && graphPunkt4 && graphPunkt5)
                        {
                            windowGraph.ShowGraph(25, BB1_Zeit[0] + BB1_Zeit[1] + BB1_Zeit[2] + BB1_Zeit[3] + BB1_Zeit[4], tiegelFarbe);
                            tiegel1Graph[0] = true;
                        }
                        else if (tiegel1Graph[0] == false && graphPunkt4 && graphPunkt5 == false)
                        {
                            windowGraph.ShowGraph(25, BB1_Zeit[0] + BB1_Zeit[1] + BB1_Zeit[2] + BB1_Zeit[3], tiegelFarbe);
                            tiegel1Graph[0] = true;
                        }
                        else if (tiegel1Graph[0] == false && graphPunkt4 == false && graphPunkt5 == false)
                        {
                            windowGraph.ShowGraph(25, BB1_Zeit[0] + BB1_Zeit[1] + BB1_Zeit[2], tiegelFarbe);
                            tiegel1Graph[0] = true;
                        }
                        //------------------------------------------------
                        istTemp[0] -= BB1_Zieltemp[0] / BB1_Zeit[0];
                    }
                    else if (istTemp[0] > BB1_Zieltemp[0] && istTemp[0] <= BB1_Zieltemp[1])
                    {
                        if (slot1.transform.childCount > 0 && (slot1.transform.GetChild(0).CompareTag("20SiHot") || slot1.transform.GetChild(0).CompareTag("20SiCold")))
                        {
                            tiegelFarbe = 20;
                        }
                        else if (slot2.transform.childCount > 0 && (slot2.transform.GetChild(0).CompareTag("20SiHot") || slot2.transform.GetChild(0).CompareTag("20SiCold")))
                        {
                            tiegelFarbe = 40;
                        }
                        else if (slot3.transform.childCount > 0 && (slot3.transform.GetChild(0).CompareTag("20SiHot") || slot3.transform.GetChild(0).CompareTag("20SiCold")))
                        {
                            tiegelFarbe = 60;
                        }
                        else if (slot4.transform.childCount > 0 && (slot4.transform.GetChild(0).CompareTag("20SiHot") || slot4.transform.GetChild(0).CompareTag("20SiCold")))
                        {
                            tiegelFarbe = 80;
                        }
                        //-------------------Showgraph--------------------
                        if (tiegel1Graph[1] == false && graphPunkt4 && graphPunkt5)
                        {
                            windowGraph.ShowGraph(BB1_Zieltemp[0], BB1_Zeit[0] + BB1_Zeit[1] + BB1_Zeit[2] + BB1_Zeit[3], tiegelFarbe);
                            tiegel1Graph[1] = true;
                        }
                        else if (tiegel1Graph[1] == false && graphPunkt4 && graphPunkt5 == false)
                        {
                            windowGraph.ShowGraph(BB1_Zieltemp[0], BB1_Zeit[0] + BB1_Zeit[1] + BB1_Zeit[2], tiegelFarbe);
                            tiegel1Graph[1] = true;
                        }
                        else if (tiegel1Graph[1] == false && graphPunkt4 == false && graphPunkt5 == false)
                        {
                            windowGraph.ShowGraph(BB1_Zieltemp[0], BB1_Zeit[0] + BB1_Zeit[1], tiegelFarbe);
                            tiegel1Graph[1] = true;
                        }
                        //------------------------------------------------
                        istTemp[0] -= (BB1_Zieltemp[1] - BB1_Zieltemp[0]) / BB1_Zeit[1];
                    }
                    //3ter Graph Punkt
                    else if (istTemp[0] > BB1_Zieltemp[1] && graphPunkt4 == false && graphPunkt5 == false)
                    {
                        if (slot1.transform.childCount > 0 && (slot1.transform.GetChild(0).CompareTag("20SiHot") || slot1.transform.GetChild(0).CompareTag("20SiCold")))
                        {
                            tiegelFarbe = 20;
                        }
                        else if (slot2.transform.childCount > 0 && (slot2.transform.GetChild(0).CompareTag("20SiHot") || slot2.transform.GetChild(0).CompareTag("20SiCold")))
                        {
                            tiegelFarbe = 40;
                        }
                        else if (slot3.transform.childCount > 0 && (slot3.transform.GetChild(0).CompareTag("20SiHot") || slot3.transform.GetChild(0).CompareTag("20SiCold")))
                        {
                            tiegelFarbe = 60;
                        }
                        else if (slot4.transform.childCount > 0 && (slot4.transform.GetChild(0).CompareTag("20SiHot") || slot4.transform.GetChild(0).CompareTag("20SiCold")))
                        {
                            tiegelFarbe = 80;
                        }
                        //-------------------Showgraph--------------------
                        if (tiegel1Graph[2] == false)
                        {
                            windowGraph.ShowGraph(BB1_Zieltemp[2], 0, tiegelFarbe);
                            windowGraph.ShowGraph(BB1_Zieltemp[1], BB1_Zeit[0], tiegelFarbe);
                            tiegel1Graph[2] = true;
                        }
                        if(tiegel1Graph[1] == false && BB1_Zieltemp[1] == BB1_Zieltemp[0])
                        {
                            windowGraph.ShowGraph(BB1_Zieltemp[0], BB1_Zeit[0] + BB1_Zeit[1], tiegelFarbe);
                            tiegel1Graph[1] = true;
                        }
                        //------------------------------------------------
                        istTemp[0] -= (BB1_Zieltemp[2] - BB1_Zieltemp[1]) / BB1_Zeit[2];
                    }
                    else if (istTemp[0] > BB1_Zieltemp[1] && istTemp[0] <= BB1_Zieltemp[2] && graphPunkt4 == true)
                    {
                        if (slot1.transform.childCount > 0 && (slot1.transform.GetChild(0).CompareTag("20SiHot") || slot1.transform.GetChild(0).CompareTag("20SiCold")))
                        {
                            tiegelFarbe = 20;
                        }
                        else if (slot2.transform.childCount > 0 && (slot2.transform.GetChild(0).CompareTag("20SiHot") || slot2.transform.GetChild(0).CompareTag("20SiCold")))
                        {
                            tiegelFarbe = 40;
                        }
                        else if (slot3.transform.childCount > 0 && (slot3.transform.GetChild(0).CompareTag("20SiHot") || slot3.transform.GetChild(0).CompareTag("20SiCold")))
                        {
                            tiegelFarbe = 60;
                        }
                        else if (slot4.transform.childCount > 0 && (slot4.transform.GetChild(0).CompareTag("20SiHot") || slot4.transform.GetChild(0).CompareTag("20SiCold")))
                        {
                            tiegelFarbe = 80;
                        }
                        //-------------------Showgraph--------------------
                        if (tiegel1Graph[2] == false && graphPunkt4 && graphPunkt5 == false)
                        {
                            windowGraph.ShowGraph(BB1_Zieltemp[1], BB1_Zeit[0] + BB1_Zeit[1], tiegelFarbe);
                            tiegel1Graph[2] = true;
                        }
                        else if(tiegel1Graph[2] == false && graphPunkt4 && graphPunkt5)
                        {
                            windowGraph.ShowGraph(BB1_Zieltemp[1], BB1_Zeit[0] + BB1_Zeit[1] + BB1_Zeit[2], tiegelFarbe);
                            tiegel1Graph[2] = true;
                        }
                        //------------------------------------------------
                        istTemp[0] -= (BB1_Zieltemp[2] - BB1_Zieltemp[1]) / BB1_Zeit[2];
                    }
                    //4ter Graph Punkt
                    else if (istTemp[0] > BB1_Zieltemp[2] && graphPunkt4 == true && graphPunkt5 == false)
                    {
                        if (slot1.transform.childCount > 0 && (slot1.transform.GetChild(0).CompareTag("20SiHot") || slot1.transform.GetChild(0).CompareTag("20SiCold")))
                        {
                            tiegelFarbe = 20;
                        }
                        else if (slot2.transform.childCount > 0 && (slot2.transform.GetChild(0).CompareTag("20SiHot") || slot2.transform.GetChild(0).CompareTag("20SiCold")))
                        {
                            tiegelFarbe = 40;
                        }
                        else if (slot3.transform.childCount > 0 && (slot3.transform.GetChild(0).CompareTag("20SiHot") || slot3.transform.GetChild(0).CompareTag("20SiCold")))
                        {
                            tiegelFarbe = 60;
                        }
                        else if (slot4.transform.childCount > 0 && (slot4.transform.GetChild(0).CompareTag("20SiHot") || slot4.transform.GetChild(0).CompareTag("20SiCold")))
                        {
                            tiegelFarbe = 80;
                        }
                        //-------------------Showgraph--------------------
                        if (tiegel1Graph[3] == false)
                        {
                            windowGraph.ShowGraph(BB1_Zieltemp[3], 0, tiegelFarbe);
                            windowGraph.ShowGraph(BB1_Zieltemp[2], BB1_Zeit[0], tiegelFarbe);
                            tiegel1Graph[3] = true;
                        }
                        if(tiegel1Graph[2] == false && BB1_Zieltemp[2] == BB1_Zieltemp[1])
                        {
                            windowGraph.ShowGraph(BB1_Zieltemp[0], BB1_Zeit[0] + BB1_Zeit[1] + BB1_Zeit[2], tiegelFarbe);
                            tiegel1Graph[2] = true;
                        }
                        //------------------------------------------------
                        istTemp[0] -= (BB1_Zieltemp[3] - BB1_Zieltemp[2]) / BB1_Zeit[3];
                    }
                    else if (istTemp[0] > BB1_Zieltemp[2] && istTemp[0] <= BB1_Zieltemp[3] && graphPunkt4 == true && graphPunkt5 == true)
                    {
                        if (slot1.transform.childCount > 0 && (slot1.transform.GetChild(0).CompareTag("20SiHot") || slot1.transform.GetChild(0).CompareTag("20SiCold")))
                        {
                            tiegelFarbe = 20;
                        }
                        else if (slot2.transform.childCount > 0 && (slot2.transform.GetChild(0).CompareTag("20SiHot") || slot2.transform.GetChild(0).CompareTag("20SiCold")))
                        {
                            tiegelFarbe = 40;
                        }
                        else if (slot3.transform.childCount > 0 && (slot3.transform.GetChild(0).CompareTag("20SiHot") || slot3.transform.GetChild(0).CompareTag("20SiCold")))
                        {
                            tiegelFarbe = 60;
                        }
                        else if (slot4.transform.childCount > 0 && (slot4.transform.GetChild(0).CompareTag("20SiHot") || slot4.transform.GetChild(0).CompareTag("20SiCold")))
                        {
                            tiegelFarbe = 80;
                        }
                        //-------------------Showgraph--------------------
                        if (tiegel1Graph[3] == false)
                        {
                            windowGraph.ShowGraph(BB1_Zieltemp[2], BB1_Zeit[0] + BB1_Zeit[1], tiegelFarbe);
                            tiegel1Graph[3] = true;
                        }
                        //------------------------------------------------
                        istTemp[0] -= (BB1_Zieltemp[3] - BB1_Zieltemp[2]) / BB1_Zeit[3];
                    }
                    //5ter Graph Punkt
                    else if (istTemp[0] > BB1_Zieltemp[3] && graphPunkt4 == true && graphPunkt5 == true)
                    {
                        if (slot1.transform.childCount > 0 && (slot1.transform.GetChild(0).CompareTag("20SiHot") || slot1.transform.GetChild(0).CompareTag("20SiCold")))
                        {
                            tiegelFarbe = 20;
                        }
                        else if (slot2.transform.childCount > 0 && (slot2.transform.GetChild(0).CompareTag("20SiHot") || slot2.transform.GetChild(0).CompareTag("20SiCold")))
                        {
                            tiegelFarbe = 40;
                        }
                        else if (slot3.transform.childCount > 0 && (slot3.transform.GetChild(0).CompareTag("20SiHot") || slot3.transform.GetChild(0).CompareTag("20SiCold")))
                        {
                            tiegelFarbe = 60;
                        }
                        else if (slot4.transform.childCount > 0 && (slot4.transform.GetChild(0).CompareTag("20SiHot") || slot4.transform.GetChild(0).CompareTag("20SiCold")))
                        {
                            tiegelFarbe = 80;
                        }
                        //-------------------Showgraph--------------------
                        if (tiegel1Graph[4] == false)
                        {
                            windowGraph.ShowGraph(BB1_Zieltemp[4], 0, tiegelFarbe);
                            windowGraph.ShowGraph(BB1_Zieltemp[3], BB1_Zeit[0], tiegelFarbe);
                            tiegel1Graph[4] = true;
                        }
                        if (tiegel1Graph[3] == false && BB1_Zieltemp[3] == BB1_Zieltemp[2])
                        {
                            windowGraph.ShowGraph(BB1_Zieltemp[0], BB1_Zeit[0] + BB1_Zeit[1] + BB1_Zeit[2] + BB1_Zeit[3], tiegelFarbe);
                            tiegel1Graph[3] = true;
                        }
                        //------------------------------------------------
                        istTemp[0] -= (BB1_Zieltemp[4] - BB1_Zieltemp[3]) / BB1_Zeit[4];
                    }
                    else if (istTemp[0] < 25)
                    {
                        istTemp[0] = 25;
                    }
                }
                #endregion
                //90% Cu / 10% Al
                #region 90% Cu / 10% Al
                #region aufheizen
                if (flamme1Bool && (slot1.transform.GetChild(0).CompareTag("40SiCold") || slot1.transform.GetChild(0).CompareTag("40SiHot")) || flamme2Bool && (slot2.transform.GetChild(0).CompareTag("40SiCold") || slot2.transform.GetChild(0).CompareTag("40SiHot")) || flamme3Bool && (slot3.transform.GetChild(0).CompareTag("40SiCold") || slot3.transform.GetChild(0).CompareTag("40SiHot")) || flamme4Bool && (slot4.transform.GetChild(0).CompareTag("40SiCold") || slot4.transform.GetChild(0).CompareTag("40SiHot")))
                {
                    tiegelLocked40 = true;
                    if (tiegel2Heated)
                    {
                        windowGraphTiegel2.DeleteGraph();
                        for (int i = 0; i < tiegel2Graph.Length; i++)
                        {
                            tiegel2Graph[i] = false;
                        }
                        tiegel2Heated = false;
                    }
                    if (istTemp[1] <= BB2_Zieltemp[0])
                    {
                        istTemp[1] += BB2_Zieltemp[0] / BB2_Zeit[0];
                        tiegel2Heated = true;
                    }
                    else if (istTemp[1] <= BB2_Zieltemp[1])
                    {
                        istTemp[1] += (BB2_Zieltemp[1] - BB2_Zieltemp[0]) / BB2_Zeit[1];
                        tiegel2Heated = true;
                    }
                    else if (istTemp[1] < BB2_Zieltemp[2])
                    {
                        istTemp[1] += (BB2_Zieltemp[2] - BB2_Zieltemp[1]) / BB2_Zeit[2];
                        tiegel2Heated = true;
                    }
                    //3ter Punkt im Graphen
                    else if (istTemp[1] >= BB2_Zieltemp[2] && graphPunkt4 == false && graphPunkt5 == false)
                    {
                        if (slot1.transform.childCount > 0 && slot1.transform.GetChild(0).CompareTag("40SiCold"))
                        {
                            tiegel2Heated = true;
                            slot1.transform.GetChild(0).tag = "40SiHot";
                            tiegelAufBB[0].gameObject.GetComponent<Renderer>().material = tiegelMat[0];
                            slot1.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[0];
                            slot1.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[0];
                        }
                        else if (slot2.transform.childCount > 0 && slot2.transform.GetChild(0).CompareTag("40SiCold"))
                        {
                            tiegel2Heated = true;
                            slot2.transform.GetChild(0).tag = "40SiHot";
                            tiegelAufBB[1].gameObject.GetComponent<Renderer>().material = tiegelMat[0];
                            slot2.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[0];
                            slot2.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[0];
                        }
                        else if (slot3.transform.childCount > 0 && slot3.transform.GetChild(0).CompareTag("40SiCold"))
                        {
                            tiegel2Heated = true;
                            slot3.transform.GetChild(0).tag = "40SiHot";
                            tiegelAufBB[2].gameObject.GetComponent<Renderer>().material = tiegelMat[0];
                            slot3.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[0];
                            slot3.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[0];
                        }
                        else if (slot4.transform.childCount > 0 && slot4.transform.GetChild(0).CompareTag("40SiCold"))
                        {
                            tiegel2Heated = true;
                            slot4.transform.GetChild(0).tag = "40SiHot";
                            tiegelAufBB[3].gameObject.GetComponent<Renderer>().material = tiegelMat[0];
                            slot4.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[0];
                            slot4.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[0];
                        }
                        istTemp[1] = BB2_Zieltemp[2];
                    }
                    else if (istTemp[1] < BB2_Zieltemp[3] && graphPunkt4 == true && graphPunkt5 == false)
                    {
                        istTemp[1] += (BB2_Zieltemp[3] - BB2_Zieltemp[2]) / BB2_Zeit[3];
                        tiegelFarbe = 20;
                        tiegel2Heated = true;
                    }
                    //4ter Punkt im Graphen
                    else if (istTemp[1] >= BB2_Zieltemp[3] && graphPunkt4 == true && graphPunkt5 == false)
                    {
                        if (slot1.transform.childCount > 0 && slot1.transform.GetChild(0).CompareTag("40SiCold"))
                        {
                            tiegel2Heated = true;
                            slot1.transform.GetChild(0).tag = "40SiHot";
                            tiegelAufBB[0].gameObject.GetComponent<Renderer>().material = tiegelMat[0];
                            slot1.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[0];
                            slot1.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[0];
                        }
                        else if (slot2.transform.childCount > 0 && slot2.transform.GetChild(0).CompareTag("40SiCold"))
                        {
                            tiegel2Heated = true;
                            slot2.transform.GetChild(0).tag = "40SiHot";
                            tiegelAufBB[1].gameObject.GetComponent<Renderer>().material = tiegelMat[0];
                            slot2.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[0];
                            slot2.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[0];
                        }
                        else if (slot3.transform.childCount > 0 && slot3.transform.GetChild(0).CompareTag("40SiCold"))
                        {
                            tiegel2Heated = true;
                            slot3.transform.GetChild(0).tag = "40SiHot";
                            tiegelAufBB[2].gameObject.GetComponent<Renderer>().material = tiegelMat[0];
                            slot3.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[0];
                            slot3.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[0];
                        }
                        else if (slot4.transform.childCount > 0 && slot4.transform.GetChild(0).CompareTag("40SiCold"))
                        {
                            tiegel2Heated = true;
                            slot4.transform.GetChild(0).tag = "40SiHot";
                            tiegelAufBB[3].gameObject.GetComponent<Renderer>().material = tiegelMat[0];
                            slot4.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[0];
                            slot4.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[0];
                        }
                        istTemp[1] = BB2_Zieltemp[3];
                    }
                    else if (istTemp[1] < BB2_Zieltemp[4] && graphPunkt4 == true && graphPunkt5 == true)
                    {
                        istTemp[1] += (BB2_Zieltemp[4] - BB2_Zieltemp[3]) / BB2_Zeit[4];
                        tiegelFarbe = 20;
                        tiegel2Heated = true;
                    }
                    //5ter Graph Punkt
                    else if (istTemp[1] >= BB2_Zieltemp[4] && graphPunkt4 == true && graphPunkt5 == true)
                    {
                        if (slot1.transform.childCount > 0 && slot1.transform.GetChild(0).CompareTag("40SiCold"))
                        {
                            tiegel2Heated = true;
                            slot1.transform.GetChild(0).tag = "40SiHot";
                            tiegelAufBB[0].gameObject.GetComponent<Renderer>().material = tiegelMat[0];
                            slot1.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[0];
                            slot1.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[0];
                        }
                        else if (slot2.transform.childCount > 0 && slot2.transform.GetChild(0).CompareTag("40SiCold"))
                        {
                            tiegel2Heated = true;
                            slot2.transform.GetChild(0).tag = "40SiHot";
                            tiegelAufBB[1].gameObject.GetComponent<Renderer>().material = tiegelMat[0];
                            slot2.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[0];
                            slot2.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[0];
                        }
                        else if (slot3.transform.childCount > 0 && slot3.transform.GetChild(0).CompareTag("40SiCold"))
                        {
                            tiegel2Heated = true;
                            slot3.transform.GetChild(0).tag = "40SiHot";
                            tiegelAufBB[2].gameObject.GetComponent<Renderer>().material = tiegelMat[0];
                            slot3.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[0];
                            slot3.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[0];
                        }
                        else if (slot4.transform.childCount > 0 && slot4.transform.GetChild(0).CompareTag("40SiCold"))
                        {
                            tiegel2Heated = true;
                            slot4.transform.GetChild(0).tag = "40SiHot";
                            tiegelAufBB[3].gameObject.GetComponent<Renderer>().material = tiegelMat[0];
                            slot4.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[0];
                            slot4.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[0];
                        }
                        istTemp[1] = BB2_Zieltemp[4];
                    }
                }
                #endregion
                else
                {
                    tiegelLocked40 = false;
                    if (istTemp[1] > 25 && istTemp[1] <= BB2_Zieltemp[0])
                    {
                        if (istTemp[1] >= 25 && istTemp[1] < 100)
                        {
                            if (slot1.transform.childCount > 0 && slot1.transform.GetChild(0).CompareTag("40SiHot"))
                            {
                                slot1.transform.GetChild(0).tag = "40SiCold";
                                tiegelAufBB[0].gameObject.GetComponent<Renderer>().material = tiegelMat[1];
                                slot1.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[1];
                                slot1.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[1];
                            }
                            else if (slot2.transform.childCount > 0 && slot2.transform.GetChild(0).CompareTag("40SiHot"))
                            {
                                slot2.transform.GetChild(0).tag = "40SiCold";
                                tiegelAufBB[1].gameObject.GetComponent<Renderer>().material = tiegelMat[1];
                                slot2.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[1];
                                slot2.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[1];
                            }
                            else if (slot3.transform.childCount > 0 && slot3.transform.GetChild(0).CompareTag("40SiHot"))
                            {
                                slot3.transform.GetChild(0).tag = "40SiCold";
                                tiegelAufBB[2].gameObject.GetComponent<Renderer>().material = tiegelMat[1];
                                slot3.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[1];
                                slot3.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[1];
                            }
                            else if (slot4.transform.childCount > 0 && slot4.transform.GetChild(0).CompareTag("40SiHot"))
                            {
                                slot4.transform.GetChild(0).tag = "40SiCold";
                                tiegelAufBB[3].gameObject.GetComponent<Renderer>().material = tiegelMat[1];
                                slot4.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[1];
                                slot4.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[1];
                            }
                        }
                        if (slot1.transform.childCount > 0 && (slot1.transform.GetChild(0).CompareTag("40SiHot") || slot1.transform.GetChild(0).CompareTag("40SiCold")))
                        {
                            tiegel2Farbe = 20;
                        }
                        else if (slot2.transform.childCount > 0 && (slot2.transform.GetChild(0).CompareTag("40SiHot") || slot2.transform.GetChild(0).CompareTag("40SiCold")))
                        {
                            tiegel2Farbe = 40;
                        }
                        else if (slot3.transform.childCount > 0 && (slot3.transform.GetChild(0).CompareTag("40SiHot") || slot3.transform.GetChild(0).CompareTag("40SiCold")))
                        {
                            tiegel2Farbe = 60;
                        }
                        else if (slot4.transform.childCount > 0 && (slot4.transform.GetChild(0).CompareTag("40SiHot") || slot4.transform.GetChild(0).CompareTag("40SiCold")))
                        {
                            tiegel2Farbe = 80;
                        }
                        //-------------------Showgraph--------------------
                        if (tiegel2Graph[0] == false && graphPunkt4 && graphPunkt5)
                        {
                            windowGraphTiegel2.ShowGraph(25, BB2_Zeit[0] + BB2_Zeit[1] + BB2_Zeit[2] + BB2_Zeit[3] + BB2_Zeit[4], tiegel2Farbe);
                            tiegel2Graph[0] = true;
                        }
                        else if (tiegel2Graph[0] == false && graphPunkt4 && graphPunkt5 == false)
                        {
                            windowGraphTiegel2.ShowGraph(25, BB2_Zeit[0] + BB2_Zeit[1] + BB2_Zeit[2] + BB2_Zeit[3], tiegel2Farbe);
                            tiegel2Graph[0] = true;
                        }
                        else if (tiegel2Graph[0] == false && graphPunkt4 == false && graphPunkt5 == false)
                        {
                            windowGraphTiegel2.ShowGraph(25, BB2_Zeit[0] + BB2_Zeit[1] + BB2_Zeit[2], tiegel2Farbe);
                            tiegel2Graph[0] = true;
                        }
                        //------------------------------------------------
                        istTemp[1] -= BB2_Zieltemp[0] / BB2_Zeit[0];
                    }
                    else if (istTemp[1] > BB2_Zieltemp[0] && istTemp[1] <= BB2_Zieltemp[1])
                    {
                        if (slot1.transform.childCount > 0 && (slot1.transform.GetChild(0).CompareTag("40SiHot") || slot1.transform.GetChild(0).CompareTag("40SiCold")))
                        {
                            tiegel2Farbe = 20;
                        }
                        else if (slot2.transform.childCount > 0 && (slot2.transform.GetChild(0).CompareTag("40SiHot") || slot2.transform.GetChild(0).CompareTag("40SiCold")))
                        {
                            tiegel2Farbe = 40;
                        }
                        else if (slot3.transform.childCount > 0 && (slot3.transform.GetChild(0).CompareTag("40SiHot") || slot3.transform.GetChild(0).CompareTag("40SiCold")))
                        {
                            tiegel2Farbe = 60;
                        }
                        else if (slot4.transform.childCount > 0 && (slot4.transform.GetChild(0).CompareTag("40SiHot") || slot4.transform.GetChild(0).CompareTag("40SiCold")))
                        {
                            tiegel2Farbe = 80;
                        }
                        //-------------------Showgraph--------------------
                        if (tiegel2Graph[1] == false && graphPunkt4 && graphPunkt5)
                        {
                            windowGraphTiegel2.ShowGraph(BB2_Zieltemp[0], BB2_Zeit[0] + BB2_Zeit[1] + BB2_Zeit[2] + BB2_Zeit[3], tiegel2Farbe);
                            tiegel2Graph[1] = true;
                        }
                        else if (tiegel2Graph[1] == false && graphPunkt4 && graphPunkt5 == false)
                        {
                            windowGraphTiegel2.ShowGraph(BB2_Zieltemp[0], BB2_Zeit[0] + BB2_Zeit[1] + BB2_Zeit[2], tiegel2Farbe);
                            tiegel2Graph[1] = true;
                        }
                        else if (tiegel2Graph[1] == false && graphPunkt4 == false && graphPunkt5 == false)
                        {
                            windowGraphTiegel2.ShowGraph(BB2_Zieltemp[0], BB2_Zeit[0] + BB2_Zeit[1], tiegel2Farbe);
                            tiegel2Graph[1] = true;
                        }
                        //------------------------------------------------
                        istTemp[1] -= (BB2_Zieltemp[1] - BB2_Zieltemp[0]) / BB2_Zeit[1];
                    }
                    //3ter Graph Punkt
                    else if (istTemp[1] > BB2_Zieltemp[1] && graphPunkt4 == false && graphPunkt5 == false)
                    {
                        if (slot1.transform.childCount > 0 && (slot1.transform.GetChild(0).CompareTag("40SiHot") || slot1.transform.GetChild(0).CompareTag("40SiCold")))
                        {
                            tiegel2Farbe = 20;
                        }
                        else if (slot2.transform.childCount > 0 && (slot2.transform.GetChild(0).CompareTag("40SiHot") || slot2.transform.GetChild(0).CompareTag("40SiCold")))
                        {
                            tiegel2Farbe = 40;
                        }
                        else if (slot3.transform.childCount > 0 && (slot3.transform.GetChild(0).CompareTag("40SiHot") || slot3.transform.GetChild(0).CompareTag("40SiCold")))
                        {
                            tiegel2Farbe = 60;
                        }
                        else if (slot4.transform.childCount > 0 && (slot4.transform.GetChild(0).CompareTag("40SiHot") || slot4.transform.GetChild(0).CompareTag("40SiCold")))
                        {
                            tiegel2Farbe = 80;
                        }
                        //-------------------Showgraph--------------------
                        if (tiegel2Graph[2] == false)
                        {
                            windowGraphTiegel2.ShowGraph(BB2_Zieltemp[2], 0, tiegel2Farbe);
                            windowGraphTiegel2.ShowGraph(BB2_Zieltemp[1], BB2_Zeit[0], tiegel2Farbe);
                            tiegel2Graph[2] = true;
                        }
                        if (tiegel2Graph[1] == false && BB2_Zieltemp[1] == BB2_Zieltemp[0])
                        {
                            windowGraphTiegel2.ShowGraph(BB2_Zieltemp[0], BB2_Zeit[0] + BB2_Zeit[1], tiegel2Farbe);
                            tiegel2Graph[1] = true;
                        }
                        //------------------------------------------------
                        istTemp[1] -= (BB2_Zieltemp[2] - BB2_Zieltemp[1]) / BB2_Zeit[2];
                    }
                    else if (istTemp[1] > BB2_Zieltemp[1] && istTemp[1] <= BB2_Zieltemp[2] && graphPunkt4 == true && graphPunkt5 == false)
                    {
                        if (slot1.transform.childCount > 0 && (slot1.transform.GetChild(0).CompareTag("40SiHot") || slot1.transform.GetChild(0).CompareTag("40SiCold")))
                        {
                            tiegel2Farbe = 20;
                        }
                        else if (slot2.transform.childCount > 0 && (slot2.transform.GetChild(0).CompareTag("40SiHot") || slot2.transform.GetChild(0).CompareTag("40SiCold")))
                        {
                            tiegel2Farbe = 40;
                        }
                        else if (slot3.transform.childCount > 0 && (slot3.transform.GetChild(0).CompareTag("40SiHot") || slot3.transform.GetChild(0).CompareTag("40SiCold")))
                        {
                            tiegel2Farbe = 60;
                        }
                        else if (slot4.transform.childCount > 0 && (slot4.transform.GetChild(0).CompareTag("40SiHot") || slot4.transform.GetChild(0).CompareTag("40SiCold")))
                        {
                            tiegel2Farbe = 80;
                        }
                        //-------------------Showgraph--------------------
                        if (tiegel2Graph[2] == false && graphPunkt4 && graphPunkt5 == false)
                        {
                            windowGraphTiegel2.ShowGraph(BB2_Zieltemp[1], BB2_Zeit[0] + BB2_Zeit[1], tiegel2Farbe);
                            tiegel2Graph[2] = true;
                        }
                        else if (tiegel2Graph[2] == false && graphPunkt4 && graphPunkt5)
                        {
                            windowGraphTiegel2.ShowGraph(BB2_Zieltemp[1], BB2_Zeit[0] + BB2_Zeit[1] + BB2_Zeit[2], tiegel2Farbe);
                            tiegel2Graph[2] = true;
                        }
                        //------------------------------------------------
                        istTemp[1] -= (BB2_Zieltemp[2] - BB2_Zieltemp[1]) / BB2_Zeit[2];
                    }
                    //4ter Graph Punkt
                    else if (istTemp[1] > BB2_Zieltemp[2] && graphPunkt4 == true && graphPunkt5 == false)
                    {
                        if (slot1.transform.childCount > 0 && (slot1.transform.GetChild(0).CompareTag("40SiHot") || slot1.transform.GetChild(0).CompareTag("40SiCold")))
                        {
                            tiegel2Farbe = 20;
                        }
                        else if (slot2.transform.childCount > 0 && (slot2.transform.GetChild(0).CompareTag("40SiHot") || slot2.transform.GetChild(0).CompareTag("40SiCold")))
                        {
                            tiegel2Farbe = 40;
                        }
                        else if (slot3.transform.childCount > 0 && (slot3.transform.GetChild(0).CompareTag("40SiHot") || slot3.transform.GetChild(0).CompareTag("40SiCold")))
                        {
                            tiegel2Farbe = 60;
                        }
                        else if (slot4.transform.childCount > 0 && (slot4.transform.GetChild(0).CompareTag("40SiHot") || slot4.transform.GetChild(0).CompareTag("40SiCold")))
                        {
                            tiegel2Farbe = 80;
                        }
                        //-------------------Showgraph--------------------
                        if (tiegel2Graph[3] == false)
                        {
                            windowGraphTiegel2.ShowGraph(BB2_Zieltemp[3], 0, tiegel2Farbe);
                            windowGraphTiegel2.ShowGraph(BB2_Zieltemp[2], BB2_Zeit[0], tiegel2Farbe);
                            tiegel2Graph[3] = true;
                        }
                        if (tiegel2Graph[2] == false && BB2_Zieltemp[2] == BB2_Zieltemp[1])
                        {
                            windowGraphTiegel2.ShowGraph(BB2_Zieltemp[0], BB2_Zeit[0] + BB2_Zeit[1] + BB2_Zeit[2], tiegel2Farbe);
                            tiegel2Graph[2] = true;
                        }
                        //------------------------------------------------
                        istTemp[1] -= (BB2_Zieltemp[3] - BB2_Zieltemp[2]) / BB2_Zeit[3];
                    }
                    else if (istTemp[1] > BB2_Zieltemp[2] && istTemp[1] <= BB2_Zieltemp[3] && graphPunkt4 == true && graphPunkt5 == true)
                    {
                        if (slot1.transform.childCount > 0 && (slot1.transform.GetChild(0).CompareTag("40SiHot") || slot1.transform.GetChild(0).CompareTag("40SiCold")))
                        {
                            tiegel2Farbe = 20;
                        }
                        else if (slot2.transform.childCount > 0 && (slot2.transform.GetChild(0).CompareTag("40SiHot") || slot2.transform.GetChild(0).CompareTag("40SiCold")))
                        {
                            tiegel2Farbe = 40;
                        }
                        else if (slot3.transform.childCount > 0 && (slot3.transform.GetChild(0).CompareTag("40SiHot") || slot3.transform.GetChild(0).CompareTag("40SiCold")))
                        {
                            tiegel2Farbe = 60;
                        }
                        else if (slot4.transform.childCount > 0 && (slot4.transform.GetChild(0).CompareTag("40SiHot") || slot4.transform.GetChild(0).CompareTag("40SiCold")))
                        {
                            tiegel2Farbe = 80;
                        }
                        //-------------------Showgraph--------------------
                        if (tiegel2Graph[3] == false)
                        {
                            windowGraphTiegel2.ShowGraph(BB2_Zieltemp[2], BB2_Zeit[0] + BB2_Zeit[1], tiegel2Farbe);
                            tiegel2Graph[3] = true;
                        }
                        //------------------------------------------------
                        istTemp[1] -= (BB2_Zieltemp[3] - BB2_Zieltemp[2]) / BB2_Zeit[3];
                    }
                    //5ter Graph Punkt
                    else if (istTemp[1] > BB2_Zieltemp[4] && graphPunkt4 == true && graphPunkt5 == true)
                    {
                        if (slot1.transform.childCount > 0 && (slot1.transform.GetChild(0).CompareTag("40SiHot") || slot1.transform.GetChild(0).CompareTag("40SiCold")))
                        {
                            tiegel2Farbe = 20;
                        }
                        else if (slot2.transform.childCount > 0 && (slot2.transform.GetChild(0).CompareTag("40SiHot") || slot2.transform.GetChild(0).CompareTag("40SiCold")))
                        {
                            tiegel2Farbe = 40;
                        }
                        else if (slot3.transform.childCount > 0 && (slot3.transform.GetChild(0).CompareTag("40SiHot") || slot3.transform.GetChild(0).CompareTag("40SiCold")))
                        {
                            tiegel2Farbe = 60;
                        }
                        else if (slot4.transform.childCount > 0 && (slot4.transform.GetChild(0).CompareTag("40SiHot") || slot4.transform.GetChild(0).CompareTag("40SiCold")))
                        {
                            tiegel2Farbe = 80;
                        }
                        //-------------------Showgraph--------------------
                        if (tiegel2Graph[4] == false)
                        {
                            windowGraphTiegel2.ShowGraph(BB2_Zieltemp[4], 0, tiegel2Farbe);
                            windowGraphTiegel2.ShowGraph(BB2_Zieltemp[3], BB2_Zeit[0], tiegel2Farbe);
                            tiegel2Graph[4] = true;
                        }
                        if (tiegel2Graph[3] == false && BB2_Zieltemp[3] == BB2_Zieltemp[2])
                        {
                            windowGraphTiegel2.ShowGraph(BB2_Zieltemp[0], BB2_Zeit[0] + BB2_Zeit[1] + BB2_Zeit[2] + BB2_Zeit[3], tiegel2Farbe);
                            tiegel2Graph[3] = true;
                        }
                        //------------------------------------------------
                        istTemp[1] -= (BB2_Zieltemp[4] - BB2_Zieltemp[3]) / BB2_Zeit[4];
                    }
                    else if (istTemp[1] < 25)
                    {
                        istTemp[1] = 25;
                    }
                }
                #endregion
                //70% Cu / 30% Al
                #region 70% Cu / 30% Al
                #region aufheizen
                if (flamme1Bool && (slot1.transform.GetChild(0).CompareTag("60SiCold") || slot1.transform.GetChild(0).CompareTag("60SiHot")) || flamme2Bool && (slot2.transform.GetChild(0).CompareTag("60SiCold") || slot2.transform.GetChild(0).CompareTag("60SiHot")) || flamme3Bool && (slot3.transform.GetChild(0).CompareTag("60SiCold") || slot3.transform.GetChild(0).CompareTag("60SiHot")) || flamme4Bool && (slot4.transform.GetChild(0).CompareTag("60SiCold") || slot4.transform.GetChild(0).CompareTag("60SiHot")))
                {
                    tiegelLocked60 = true;
                    if (tiegel3Heated)
                    {
                        windowGraphTiegel3.DeleteGraph();
                        tiegel3Heated = false;
                        for (int i = 0; i < tiegel3Graph.Length; i++)
                        {
                            tiegel3Graph[i] = false;
                        }
                    }
                    if (istTemp[2] <= BB3_Zieltemp[0])
                    {
                        istTemp[2] += BB3_Zieltemp[0] / BB3_Zeit[0];
                        tiegel3Heated = true;
                    }
                    else if (istTemp[2] <= BB3_Zieltemp[1])
                    {
                        istTemp[2] += (BB3_Zieltemp[1] - BB3_Zieltemp[0]) / BB3_Zeit[1];
                        tiegel3Heated = true;
                    }
                    else if (istTemp[2] < BB3_Zieltemp[2])
                    {
                        istTemp[2] += (BB3_Zieltemp[2] - BB3_Zieltemp[1]) / BB3_Zeit[2];
                        tiegel3Heated = true;
                    }
                    //3ter Punkt im Graphen
                    else if (istTemp[2] >= BB3_Zieltemp[2] && graphPunkt4 == false && graphPunkt5 == false)
                    {
                        if (slot1.transform.childCount > 0 && slot1.transform.GetChild(0).CompareTag("60SiCold"))
                        {
                            tiegel1Heated = true;
                            slot1.transform.GetChild(0).tag = "60SiHot";
                            tiegelAufBB[0].gameObject.GetComponent<Renderer>().material = tiegelMat[0];
                            slot1.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[0];
                            slot1.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[0];
                        }
                        else if (slot2.transform.childCount > 0 && slot2.transform.GetChild(0).CompareTag("60SiCold"))
                        {
                            tiegel1Heated = true;
                            slot2.transform.GetChild(0).tag = "60SiHot";
                            tiegelAufBB[1].gameObject.GetComponent<Renderer>().material = tiegelMat[0];
                            slot2.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[0];
                            slot2.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[0];
                        }
                        else if (slot3.transform.childCount > 0 && slot3.transform.GetChild(0).CompareTag("60SiCold"))
                        {
                            tiegel1Heated = true;
                            slot3.transform.GetChild(0).tag = "60SiHot";
                            tiegelAufBB[2].gameObject.GetComponent<Renderer>().material = tiegelMat[0];
                            slot3.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[0];
                            slot3.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[0];
                        }
                        else if (slot4.transform.childCount > 0 && slot4.transform.GetChild(0).CompareTag("60SiCold"))
                        {
                            tiegel1Heated = true;
                            slot4.transform.GetChild(0).tag = "60SiHot";
                            tiegelAufBB[3].gameObject.GetComponent<Renderer>().material = tiegelMat[0];
                            slot4.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[0];
                            slot4.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[0];
                        }
                        istTemp[2] = BB3_Zieltemp[2];
                    }
                    else if (istTemp[2] < BB3_Zieltemp[3] && graphPunkt4 == true && graphPunkt5 == false)
                    {
                        istTemp[2] += (BB3_Zieltemp[3] - BB3_Zieltemp[2]) / BB3_Zeit[3];
                        tiegelFarbe = 20;
                        tiegel3Heated = true;
                    }
                    //4ter Punkt im Graphen
                    else if (istTemp[2] >= BB3_Zieltemp[3] && graphPunkt4 == true && graphPunkt5 == false)
                    {
                        if (slot1.transform.childCount > 0 && slot1.transform.GetChild(0).CompareTag("60SiCold"))
                        {
                            tiegel3Heated = true;
                            slot1.transform.GetChild(0).tag = "60SiHot";
                            tiegelAufBB[0].gameObject.GetComponent<Renderer>().material = tiegelMat[0];
                            slot1.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[0];
                            slot1.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[0];
                        }
                        else if (slot2.transform.childCount > 0 && slot2.transform.GetChild(0).CompareTag("60SiCold"))
                        {
                            tiegel3Heated = true;
                            slot2.transform.GetChild(0).tag = "60SiHot";
                            tiegelAufBB[1].gameObject.GetComponent<Renderer>().material = tiegelMat[0];
                            slot2.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[0];
                            slot2.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[0];
                        }
                        else if (slot3.transform.childCount > 0 && slot3.transform.GetChild(0).CompareTag("60SiCold"))
                        {
                            tiegel3Heated = true;
                            slot3.transform.GetChild(0).tag = "60SiHot";
                            tiegelAufBB[2].gameObject.GetComponent<Renderer>().material = tiegelMat[0];
                            slot3.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[0];
                            slot3.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[0];
                        }
                        else if (slot4.transform.childCount > 0 && slot4.transform.GetChild(0).CompareTag("60SiCold"))
                        {
                            tiegel3Heated = true;
                            slot4.transform.GetChild(0).tag = "60SiHot";
                            tiegelAufBB[3].gameObject.GetComponent<Renderer>().material = tiegelMat[0];
                            slot4.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[0];
                            slot4.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[0];
                        }
                        istTemp[2] = BB3_Zieltemp[3];
                    }
                    else if (istTemp[2] < BB3_Zieltemp[4] && graphPunkt4 == true && graphPunkt5 == true)
                    {
                        istTemp[2] += (BB3_Zieltemp[4] - BB3_Zieltemp[3]) / BB3_Zeit[4];
                        tiegelFarbe = 20;
                        tiegel3Heated = true;
                    }
                    //5ter Graph Punkt
                    else if (istTemp[2] >= BB3_Zieltemp[4] && graphPunkt4 == true && graphPunkt5 == true)
                    {
                        if (slot1.transform.childCount > 0 && slot1.transform.GetChild(0).CompareTag("60SiCold"))
                        {
                            tiegel3Heated = true;
                            slot1.transform.GetChild(0).tag = "60SiHot";
                            tiegelAufBB[0].gameObject.GetComponent<Renderer>().material = tiegelMat[0];
                            slot1.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[0];
                            slot1.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[0];
                        }
                        else if (slot2.transform.childCount > 0 && slot2.transform.GetChild(0).CompareTag("60SiCold"))
                        {
                            tiegel3Heated = true;
                            slot2.transform.GetChild(0).tag = "60SiHot";
                            tiegelAufBB[1].gameObject.GetComponent<Renderer>().material = tiegelMat[0];
                            slot2.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[0];
                            slot2.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[0];
                        }
                        else if (slot3.transform.childCount > 0 && slot3.transform.GetChild(0).CompareTag("60SiCold"))
                        {
                            tiegel3Heated = true;
                            slot3.transform.GetChild(0).tag = "60SiHot";
                            tiegelAufBB[2].gameObject.GetComponent<Renderer>().material = tiegelMat[0];
                            slot3.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[0];
                            slot3.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[0];
                        }
                        else if (slot4.transform.childCount > 0 && slot4.transform.GetChild(0).CompareTag("60SiCold"))
                        {
                            tiegel3Heated = true;
                            slot4.transform.GetChild(0).tag = "60SiHot";
                            tiegelAufBB[3].gameObject.GetComponent<Renderer>().material = tiegelMat[0];
                            slot4.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[0];
                            slot4.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[0];
                        }
                        istTemp[2] = BB3_Zieltemp[4];
                    }
                }
                #endregion
                else
                {
                    tiegelLocked60 = false;
                    if (istTemp[2] > 25 && istTemp[2] <= BB3_Zieltemp[0])
                    {
                        if (istTemp[2] >= 25 && istTemp[2] < 100)
                        {
                            if (slot1.transform.childCount > 0 && slot1.transform.GetChild(0).CompareTag("60SiHot"))
                            {
                                slot1.transform.GetChild(0).tag = "60SiCold";
                                tiegelAufBB[0].gameObject.GetComponent<Renderer>().material = tiegelMat[1];
                                slot1.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[1];
                                slot1.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[1];
                            }
                            else if (slot2.transform.childCount > 0 && slot2.transform.GetChild(0).CompareTag("60SiHot"))
                            {
                                slot2.transform.GetChild(0).tag = "60SiCold";
                                tiegelAufBB[1].gameObject.GetComponent<Renderer>().material = tiegelMat[1];
                                slot2.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[1];
                                slot2.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[1];
                            }
                            else if (slot3.transform.childCount > 0 && slot3.transform.GetChild(0).CompareTag("60SiHot"))
                            {
                                slot3.transform.GetChild(0).tag = "60SiCold";
                                tiegelAufBB[2].gameObject.GetComponent<Renderer>().material = tiegelMat[1];
                                slot3.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[1];
                                slot3.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[1];
                            }
                            else if (slot4.transform.childCount > 0 && slot4.transform.GetChild(0).CompareTag("60SiHot"))
                            {
                                slot4.transform.GetChild(0).tag = "60SiCold";
                                tiegelAufBB[3].gameObject.GetComponent<Renderer>().material = tiegelMat[1];
                                slot4.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[1];
                                slot4.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[1];
                            }
                        }
                        if (slot1.transform.childCount > 0 && (slot1.transform.GetChild(0).CompareTag("60SiHot") || slot1.transform.GetChild(0).CompareTag("60SiCold")))
                        {
                            tiegel3Farbe = 20;
                        }
                        else if (slot2.transform.childCount > 0 && (slot2.transform.GetChild(0).CompareTag("60SiHot") || slot2.transform.GetChild(0).CompareTag("60SiCold")))
                        {
                            tiegel3Farbe = 40;
                        }
                        else if (slot3.transform.childCount > 0 && (slot3.transform.GetChild(0).CompareTag("60SiHot") || slot3.transform.GetChild(0).CompareTag("60SiCold")))
                        {
                            tiegel3Farbe = 60;
                        }
                        else if (slot4.transform.childCount > 0 && (slot4.transform.GetChild(0).CompareTag("60SiHot") || slot4.transform.GetChild(0).CompareTag("60SiCold")))
                        {
                            tiegel3Farbe = 80;
                        }
                        //-------------------Showgraph--------------------
                        if (tiegel3Graph[0] == false && graphPunkt4 && graphPunkt5)
                        {
                            windowGraphTiegel3.ShowGraph(25, BB3_Zeit[0] + BB3_Zeit[1] + BB3_Zeit[2] + BB3_Zeit[3] + BB3_Zeit[4], tiegel3Farbe);
                            tiegel3Graph[0] = true;
                        }
                        else if (tiegel3Graph[0] == false && graphPunkt4 && graphPunkt5 == false)
                        {
                            windowGraphTiegel3.ShowGraph(25, BB3_Zeit[0] + BB3_Zeit[1] + BB3_Zeit[2] + BB3_Zeit[3], tiegel3Farbe);
                            tiegel3Graph[0] = true;
                        }
                        else if (tiegel3Graph[0] == false && graphPunkt4 == false && graphPunkt5 == false)
                        {
                            windowGraphTiegel3.ShowGraph(25, BB3_Zeit[0] + BB3_Zeit[1] + BB3_Zeit[2], tiegel3Farbe);
                            tiegel3Graph[0] = true;
                        }
                        //------------------------------------------------
                        istTemp[2] -= BB3_Zieltemp[0] / BB3_Zeit[0];
                    }
                    else if (istTemp[2] > BB3_Zieltemp[0] && istTemp[2] <= BB3_Zieltemp[1])
                    {
                        if (slot1.transform.childCount > 0 && (slot1.transform.GetChild(0).CompareTag("60SiHot") || slot1.transform.GetChild(0).CompareTag("60SiCold")))
                        {
                            tiegel3Farbe = 20;
                        }
                        else if (slot2.transform.childCount > 0 && (slot2.transform.GetChild(0).CompareTag("60SiHot") || slot2.transform.GetChild(0).CompareTag("60SiCold")))
                        {
                            tiegel3Farbe = 40;
                        }
                        else if (slot3.transform.childCount > 0 && (slot3.transform.GetChild(0).CompareTag("60SiHot") || slot3.transform.GetChild(0).CompareTag("60SiCold")))
                        {
                            tiegel3Farbe = 60;
                        }
                        else if (slot4.transform.childCount > 0 && (slot4.transform.GetChild(0).CompareTag("60SiHot") || slot4.transform.GetChild(0).CompareTag("60SiCold")))
                        {
                            tiegel3Farbe = 80;
                        }
                        //-------------------Showgraph--------------------
                        if (tiegel3Graph[1] == false && graphPunkt4 && graphPunkt5)
                        {
                            windowGraphTiegel3.ShowGraph(BB3_Zieltemp[0], BB3_Zeit[0] + BB3_Zeit[1] + BB3_Zeit[2] + BB3_Zeit[3], tiegel3Farbe);
                            tiegel3Graph[1] = true;
                        }
                        else if (tiegel3Graph[1] == false && graphPunkt4 && graphPunkt5 == false)
                        {
                            windowGraphTiegel3.ShowGraph(BB3_Zieltemp[0], BB3_Zeit[0] + BB3_Zeit[1] + BB3_Zeit[2], tiegel3Farbe);
                            tiegel3Graph[1] = true;
                        }
                        else if (tiegel3Graph[1] == false && graphPunkt4 == false && graphPunkt5 == false)
                        {
                            windowGraphTiegel3.ShowGraph(BB3_Zieltemp[0], BB3_Zeit[0] + BB3_Zeit[1], tiegel3Farbe);
                            tiegel3Graph[1] = true;
                        }
                        //------------------------------------------------
                        istTemp[2] -= (BB3_Zieltemp[1] - BB3_Zieltemp[0]) / BB3_Zeit[1];
                    }
                    //3ter Graph Punkt
                    else if (istTemp[2] > BB3_Zieltemp[1] && graphPunkt4 == false && graphPunkt5 == false)
                    {
                        if (slot1.transform.childCount > 0 && (slot1.transform.GetChild(0).CompareTag("60SiHot") || slot1.transform.GetChild(0).CompareTag("60SiCold")))
                        {
                            tiegel3Farbe = 20;
                        }
                        else if (slot2.transform.childCount > 0 && (slot2.transform.GetChild(0).CompareTag("60SiHot") || slot2.transform.GetChild(0).CompareTag("60SiCold")))
                        {
                            tiegel3Farbe = 40;
                        }
                        else if (slot3.transform.childCount > 0 && (slot3.transform.GetChild(0).CompareTag("60SiHot") || slot3.transform.GetChild(0).CompareTag("60SiCold")))
                        {
                            tiegel3Farbe = 60;
                        }
                        else if (slot4.transform.childCount > 0 && (slot4.transform.GetChild(0).CompareTag("60SiHot") || slot4.transform.GetChild(0).CompareTag("60SiCold")))
                        {
                            tiegel3Farbe = 80;
                        }
                        //-------------------Showgraph--------------------
                        if (tiegel3Graph[2] == false)
                        {
                            windowGraphTiegel3.ShowGraph(BB3_Zieltemp[2], 0, tiegel3Farbe);
                            windowGraphTiegel3.ShowGraph(BB3_Zieltemp[1], BB3_Zeit[0], tiegel3Farbe);
                            tiegel3Graph[2] = true;
                        }
                        if (tiegel3Graph[1] == false && BB3_Zieltemp[1] == BB3_Zieltemp[0])
                        {
                            windowGraphTiegel3.ShowGraph(BB3_Zieltemp[0], BB3_Zeit[0] + BB3_Zeit[1], tiegel3Farbe);
                            tiegel3Graph[1] = true;
                        }
                        //------------------------------------------------
                        istTemp[2] -= (BB3_Zieltemp[2] - BB3_Zieltemp[1]) / BB3_Zeit[2];
                    }
                    else if (istTemp[2] > BB3_Zieltemp[1] && istTemp[2] <= BB3_Zieltemp[2] && graphPunkt4 == true && graphPunkt5 == false)
                    {
                        if (slot1.transform.childCount > 0 && (slot1.transform.GetChild(0).CompareTag("60SiHot") || slot1.transform.GetChild(0).CompareTag("60SiCold")))
                        {
                            tiegel3Farbe = 20;
                        }
                        else if (slot2.transform.childCount > 0 && (slot2.transform.GetChild(0).CompareTag("60SiHot") || slot2.transform.GetChild(0).CompareTag("60SiCold")))
                        {
                            tiegel3Farbe = 40;
                        }
                        else if (slot3.transform.childCount > 0 && (slot3.transform.GetChild(0).CompareTag("60SiHot") || slot3.transform.GetChild(0).CompareTag("60SiCold")))
                        {
                            tiegel3Farbe = 60;
                        }
                        else if (slot4.transform.childCount > 0 && (slot4.transform.GetChild(0).CompareTag("60SiHot") || slot4.transform.GetChild(0).CompareTag("60SiCold")))
                        {
                            tiegel3Farbe = 80;
                        }
                        //-------------------Showgraph--------------------
                        if (tiegel3Graph[2] == false && graphPunkt4 && graphPunkt5 == false)
                        {
                            windowGraphTiegel3.ShowGraph(BB3_Zieltemp[1], BB3_Zeit[0] + BB3_Zeit[1], tiegel3Farbe);
                            tiegel3Graph[2] = true;
                        }
                        else if (tiegel3Graph[2] == false && graphPunkt4 && graphPunkt5)
                        {
                            windowGraphTiegel3.ShowGraph(BB3_Zieltemp[1], BB3_Zeit[0] + BB3_Zeit[1] + BB3_Zeit[2], tiegel3Farbe);
                            tiegel3Graph[2] = true;
                        }
                        //------------------------------------------------
                        istTemp[2] -= (BB3_Zieltemp[2] - BB3_Zieltemp[1]) / BB3_Zeit[2];
                    }
                    //4ter Graph Punkt
                    else if (istTemp[2] > BB3_Zieltemp[2] && graphPunkt4 == true && graphPunkt5 == false)
                    {
                        if (slot1.transform.childCount > 0 && (slot1.transform.GetChild(0).CompareTag("60SiHot") || slot1.transform.GetChild(0).CompareTag("60SiCold")))
                        {
                            tiegel3Farbe = 20;
                        }
                        else if (slot2.transform.childCount > 0 && (slot2.transform.GetChild(0).CompareTag("60SiHot") || slot2.transform.GetChild(0).CompareTag("60SiCold")))
                        {
                            tiegel3Farbe = 40;
                        }
                        else if (slot3.transform.childCount > 0 && (slot3.transform.GetChild(0).CompareTag("60SiHot") || slot3.transform.GetChild(0).CompareTag("60SiCold")))
                        {
                            tiegel3Farbe = 60;
                        }
                        else if (slot4.transform.childCount > 0 && (slot4.transform.GetChild(0).CompareTag("60SiHot") || slot4.transform.GetChild(0).CompareTag("60SiCold")))
                        {
                            tiegel3Farbe = 80;
                        }
                        //-------------------Showgraph--------------------
                        if (tiegel3Graph[3] == false)
                        {
                            windowGraphTiegel3.ShowGraph(BB3_Zieltemp[3], 0, tiegel3Farbe);
                            windowGraphTiegel3.ShowGraph(BB3_Zieltemp[2], BB3_Zeit[0], tiegel3Farbe);
                            tiegel3Graph[3] = true;
                        }
                        if (tiegel3Graph[2] == false && BB3_Zieltemp[2] == BB3_Zieltemp[1])
                        {
                            windowGraphTiegel3.ShowGraph(BB3_Zieltemp[0], BB3_Zeit[0] + BB3_Zeit[1] + BB3_Zeit[2], tiegel3Farbe);
                            tiegel3Graph[2] = true;
                        }
                        //------------------------------------------------
                        istTemp[2] -= (BB3_Zieltemp[2] - BB3_Zieltemp[1]) / BB3_Zeit[2];
                    }
                    else if (istTemp[2] > BB3_Zieltemp[2] && istTemp[2] <= BB3_Zieltemp[3] && graphPunkt4 == true && graphPunkt5 == true)
                    {
                        if (slot1.transform.childCount > 0 && (slot1.transform.GetChild(0).CompareTag("60SiHot") || slot1.transform.GetChild(0).CompareTag("60SiCold")))
                        {
                            tiegel3Farbe = 20;
                        }
                        else if (slot2.transform.childCount > 0 && (slot2.transform.GetChild(0).CompareTag("60SiHot") || slot2.transform.GetChild(0).CompareTag("60SiCold")))
                        {
                            tiegel3Farbe = 40;
                        }
                        else if (slot3.transform.childCount > 0 && (slot3.transform.GetChild(0).CompareTag("60SiHot") || slot3.transform.GetChild(0).CompareTag("60SiCold")))
                        {
                            tiegel3Farbe = 60;
                        }
                        else if (slot4.transform.childCount > 0 && (slot4.transform.GetChild(0).CompareTag("60SiHot") || slot4.transform.GetChild(0).CompareTag("60SiCold")))
                        {
                            tiegel3Farbe = 80;
                        }
                        //-------------------Showgraph--------------------
                        if (tiegel3Graph[3] == false)
                        {
                            windowGraphTiegel3.ShowGraph(BB3_Zieltemp[2], BB3_Zeit[0] + BB3_Zeit[1], tiegel3Farbe);
                            tiegel3Graph[3] = true;
                        }
                        //------------------------------------------------
                        istTemp[2] -= (BB3_Zieltemp[3] - BB3_Zieltemp[2]) / BB3_Zeit[3];
                    }
                    //5ter Graph Punkt
                    else if (istTemp[2] > BB3_Zieltemp[4] && graphPunkt4 == true && graphPunkt5 == true)
                    {
                        if (slot1.transform.childCount > 0 && (slot1.transform.GetChild(0).CompareTag("60SiHot") || slot1.transform.GetChild(0).CompareTag("60SiCold")))
                        {
                            tiegel3Farbe = 20;
                        }
                        else if (slot2.transform.childCount > 0 && (slot2.transform.GetChild(0).CompareTag("60SiHot") || slot2.transform.GetChild(0).CompareTag("60SiCold")))
                        {
                            tiegel3Farbe = 40;
                        }
                        else if (slot3.transform.childCount > 0 && (slot3.transform.GetChild(0).CompareTag("60SiHot") || slot3.transform.GetChild(0).CompareTag("60SiCold")))
                        {
                            tiegel3Farbe = 60;
                        }
                        else if (slot4.transform.childCount > 0 && (slot4.transform.GetChild(0).CompareTag("60SiHot") || slot4.transform.GetChild(0).CompareTag("60SiCold")))
                        {
                            tiegel3Farbe = 80;
                        }
                        //-------------------Showgraph--------------------
                        if (tiegel3Graph[4] == false)
                        {
                            windowGraphTiegel3.ShowGraph(BB3_Zieltemp[4], 0, tiegel3Farbe);
                            windowGraphTiegel3.ShowGraph(BB3_Zieltemp[3], BB3_Zeit[0], tiegel3Farbe);
                            tiegel3Graph[4] = true;
                        }
                        if (tiegel3Graph[3] == false && BB3_Zieltemp[3] == BB3_Zieltemp[2])
                        {
                            windowGraphTiegel3.ShowGraph(BB3_Zieltemp[0], BB3_Zeit[0] + BB3_Zeit[1] + BB3_Zeit[2] + BB3_Zeit[3], tiegel3Farbe);
                            tiegel3Graph[3] = true;
                        }
                        //------------------------------------------------
                        istTemp[2] -= (BB3_Zieltemp[4] - BB3_Zieltemp[3]) / BB3_Zeit[4];
                    }
                    else if (istTemp[2] < 25)
                    {
                        istTemp[2] = 25;
                    }
                }
                #endregion
                //40% Cu / 60% Al
                #region 40% Cu / 60% Al
                #region aufheizen
                if (flamme1Bool && (slot1.transform.GetChild(0).CompareTag("80SiCold") || slot1.transform.GetChild(0).CompareTag("80SiHot")) || flamme2Bool && (slot2.transform.GetChild(0).CompareTag("80SiCold") || slot2.transform.GetChild(0).CompareTag("80SiHot")) || flamme3Bool && (slot3.transform.GetChild(0).CompareTag("80SiCold") || slot3.transform.GetChild(0).CompareTag("80SiHot")) || flamme4Bool && (slot4.transform.GetChild(0).CompareTag("80SiCold") || slot4.transform.GetChild(0).CompareTag("80SiHot")))
                {
                    tiegelLocked80 = true;
                    if (tiegel4Heated)
                    {
                        windowGraphTiegel4.DeleteGraph();
                        tiegel4Heated = false;
                        for (int i = 0; i < tiegel4Graph.Length; i++)
                        {
                            tiegel4Graph[i] = false;
                        }
                    }
                    if (istTemp[3] <= BB4_Zieltemp[0])
                    {
                        istTemp[3] += BB4_Zieltemp[0] / BB4_Zeit[0];
                        tiegel4Heated = true;
                    }
                    else if (istTemp[3] <= BB4_Zieltemp[1])
                    {
                        istTemp[3] += (BB4_Zieltemp[1] - BB4_Zieltemp[0]) / BB4_Zeit[1];
                        tiegel4Heated = true;
                    }
                    else if (istTemp[3] < BB4_Zieltemp[2])
                    {
                        istTemp[3] += (BB4_Zieltemp[2] - BB4_Zieltemp[1]) / BB4_Zeit[2];
                        tiegel4Heated = true;
                    }
                    //3ter Punkt im Graphen
                    else if (istTemp[3] >= BB4_Zieltemp[2] && graphPunkt4 == false && graphPunkt5 == false)
                    {
                        if (slot1.transform.childCount > 0 && slot1.transform.GetChild(0).CompareTag("80SiCold"))
                        {
                            tiegel4Heated = true;
                            slot1.transform.GetChild(0).tag = "80SiHot";
                            tiegelAufBB[0].gameObject.GetComponent<Renderer>().material = tiegelMat[0];
                            slot1.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[0];
                            slot1.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[0];
                        }
                        else if (slot2.transform.childCount > 0 && slot2.transform.GetChild(0).CompareTag("80SiCold"))
                        {
                            tiegel4Heated = true;
                            slot2.transform.GetChild(0).tag = "80SiHot";
                            tiegelAufBB[1].gameObject.GetComponent<Renderer>().material = tiegelMat[0];
                            slot2.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[0];
                            slot2.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[0];
                        }
                        else if (slot3.transform.childCount > 0 && slot3.transform.GetChild(0).CompareTag("80SiCold"))
                        {
                            tiegel4Heated = true;
                            slot3.transform.GetChild(0).tag = "80SiHot";
                            tiegelAufBB[2].gameObject.GetComponent<Renderer>().material = tiegelMat[0];
                            slot3.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[0];
                            slot3.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[0];
                        }
                        else if (slot4.transform.childCount > 0 && slot4.transform.GetChild(0).CompareTag("80SiCold"))
                        {
                            tiegel4Heated = true;
                            slot4.transform.GetChild(0).tag = "80SiHot";
                            tiegelAufBB[3].gameObject.GetComponent<Renderer>().material = tiegelMat[0];
                            slot4.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[0];
                            slot4.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[0];
                        }
                        istTemp[3] = BB4_Zieltemp[2];
                    }
                    else if (istTemp[3] < BB4_Zieltemp[3] && graphPunkt4 == true && graphPunkt5 == false)
                    {
                        istTemp[3] += (BB4_Zieltemp[3] - BB4_Zieltemp[2]) / BB4_Zeit[3];
                        tiegelFarbe = 20;
                        tiegel4Heated = true;
                    }
                    //4ter Punkt im Graphen
                    else if (istTemp[3] >= BB4_Zieltemp[3] && graphPunkt4 == true && graphPunkt5 == false)
                    {
                        if (slot1.transform.childCount > 0 && slot1.transform.GetChild(0).CompareTag("80SiCold"))
                        {
                            tiegel4Heated = true;
                            slot1.transform.GetChild(0).tag = "80SiHot";
                            tiegelAufBB[0].gameObject.GetComponent<Renderer>().material = tiegelMat[0];
                            slot1.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[0];
                            slot1.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[0];
                        }
                        else if (slot2.transform.childCount > 0 && slot2.transform.GetChild(0).CompareTag("80SiCold"))
                        {
                            tiegel4Heated = true;
                            slot2.transform.GetChild(0).tag = "80SiHot";
                            tiegelAufBB[1].gameObject.GetComponent<Renderer>().material = tiegelMat[0];
                            slot2.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[0];
                            slot2.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[0];
                        }
                        else if (slot3.transform.childCount > 0 && slot3.transform.GetChild(0).CompareTag("80SiCold"))
                        {
                            tiegel4Heated = true;
                            slot3.transform.GetChild(0).tag = "80SiHot";
                            tiegelAufBB[2].gameObject.GetComponent<Renderer>().material = tiegelMat[0];
                            slot3.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[0];
                            slot3.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[0];
                        }
                        else if (slot4.transform.childCount > 0 && slot4.transform.GetChild(0).CompareTag("80SiCold"))
                        {
                            tiegel4Heated = true;
                            slot4.transform.GetChild(0).tag = "80SiHot";
                            tiegelAufBB[3].gameObject.GetComponent<Renderer>().material = tiegelMat[0];
                            slot4.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[0];
                            slot4.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[0];
                        }
                        istTemp[3] = BB4_Zieltemp[3];
                    }
                    else if (istTemp[3] < BB4_Zieltemp[4] && graphPunkt4 == true && graphPunkt5 == true)
                    {
                        istTemp[3] += (BB4_Zieltemp[4] - BB4_Zieltemp[3]) / BB4_Zeit[4];
                        tiegelFarbe = 20;
                        tiegel4Heated = true;
                    }
                    //5ter Graph Punkt
                    else if (istTemp[3] >= BB4_Zieltemp[4] && graphPunkt4 == true && graphPunkt5 == true)
                    {
                        if (slot1.transform.childCount > 0 && slot1.transform.GetChild(0).CompareTag("80SiCold"))
                        {
                            tiegel4Heated = true;
                            slot1.transform.GetChild(0).tag = "80SiHot";
                            tiegelAufBB[0].gameObject.GetComponent<Renderer>().material = tiegelMat[0];
                            slot1.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[0];
                            slot1.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[0];
                        }
                        else if (slot2.transform.childCount > 0 && slot2.transform.GetChild(0).CompareTag("80SiCold"))
                        {
                            tiegel4Heated = true;
                            slot2.transform.GetChild(0).tag = "80SiHot";
                            tiegelAufBB[1].gameObject.GetComponent<Renderer>().material = tiegelMat[0];
                            slot2.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[0];
                            slot2.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[0];
                        }
                        else if (slot3.transform.childCount > 0 && slot3.transform.GetChild(0).CompareTag("80SiCold"))
                        {
                            tiegel4Heated = true;
                            slot3.transform.GetChild(0).tag = "80SiHot";
                            tiegelAufBB[2].gameObject.GetComponent<Renderer>().material = tiegelMat[0];
                            slot3.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[0];
                            slot3.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[0];
                        }
                        else if (slot4.transform.childCount > 0 && slot4.transform.GetChild(0).CompareTag("80SiCold"))
                        {
                            tiegel4Heated = true;
                            slot4.transform.GetChild(0).tag = "80SiHot";
                            tiegelAufBB[3].gameObject.GetComponent<Renderer>().material = tiegelMat[0];
                            slot4.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[0];
                            slot4.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[0];
                        }
                        istTemp[3] = BB4_Zieltemp[4];
                    }
                }
                #endregion
                else
                {
                    tiegelLocked80 = false;
                    if (istTemp[3] > 25 && istTemp[3] <= BB4_Zieltemp[0])
                    {
                        if (istTemp[3] >= 25 && istTemp[3] < 100)
                        {
                            if (slot1.transform.childCount > 0 && slot1.transform.GetChild(0).CompareTag("80SiHot"))
                            {
                                slot1.transform.GetChild(0).tag = "80SiCold";
                                tiegelAufBB[0].gameObject.GetComponent<Renderer>().material = tiegelMat[1];
                                slot1.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[1];
                                slot1.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[1];
                            }
                            else if (slot2.transform.childCount > 0 && slot2.transform.GetChild(0).CompareTag("80SiHot"))
                            {
                                slot2.transform.GetChild(0).tag = "80SiCold";
                                tiegelAufBB[1].gameObject.GetComponent<Renderer>().material = tiegelMat[1];
                                slot2.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[1];
                                slot2.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[1];
                            }
                            else if (slot3.transform.childCount > 0 && slot3.transform.GetChild(0).CompareTag("80SiHot"))
                            {
                                slot3.transform.GetChild(0).tag = "80SiCold";
                                tiegelAufBB[2].gameObject.GetComponent<Renderer>().material = tiegelMat[1];
                                slot3.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[1];
                                slot3.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[1];
                            }
                            else if (slot4.transform.childCount > 0 && slot4.transform.GetChild(0).CompareTag("80SiHot"))
                            {
                                slot4.transform.GetChild(0).tag = "80SiCold";
                                tiegelAufBB[3].gameObject.GetComponent<Renderer>().material = tiegelMat[1];
                                slot4.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = tiegelSprite[1];
                                slot4.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = tiegelSprite[1];
                            }
                        }
                        if (slot1.transform.childCount > 0 && (slot1.transform.GetChild(0).CompareTag("80SiHot") || slot1.transform.GetChild(0).CompareTag("80SiCold")))
                        {
                            tiegel4Farbe = 20;
                        }
                        else if (slot2.transform.childCount > 0 && (slot2.transform.GetChild(0).CompareTag("80SiHot") || slot2.transform.GetChild(0).CompareTag("80SiCold")))
                        {
                            tiegel4Farbe = 40;
                        }
                        else if (slot3.transform.childCount > 0 && (slot3.transform.GetChild(0).CompareTag("80SiHot") || slot3.transform.GetChild(0).CompareTag("80SiCold")))
                        {
                            tiegel4Farbe = 60;
                        }
                        else if (slot4.transform.childCount > 0 && (slot4.transform.GetChild(0).CompareTag("80SiHot") || slot4.transform.GetChild(0).CompareTag("80SiCold")))
                        {
                            tiegel4Farbe = 80;
                        }
                        //-------------------Showgraph--------------------
                        if (tiegel4Graph[0] == false && graphPunkt4 && graphPunkt5)
                        {
                            windowGraphTiegel4.ShowGraph(25, BB4_Zeit[0] + BB4_Zeit[1] + BB4_Zeit[2] + BB4_Zeit[3] + BB4_Zeit[4], tiegel4Farbe);
                            tiegel4Graph[0] = true;
                        }
                        else if (tiegel4Graph[0] == false && graphPunkt4 && graphPunkt5 == false)
                        {
                            windowGraphTiegel4.ShowGraph(25, BB4_Zeit[0] + BB4_Zeit[1] + BB4_Zeit[2] + BB4_Zeit[3], tiegel4Farbe);
                            tiegel4Graph[0] = true;
                        }
                        else if (tiegel4Graph[0] == false && graphPunkt4 == false && graphPunkt5 == false)
                        {
                            windowGraphTiegel4.ShowGraph(25, BB4_Zeit[0] + BB4_Zeit[1] + BB4_Zeit[2], tiegel4Farbe);
                            tiegel4Graph[0] = true;
                        }
                        //------------------------------------------------
                        istTemp[3] -= BB4_Zieltemp[0] / BB4_Zeit[0];
                    }
                    else if (istTemp[3] > BB4_Zieltemp[0] && istTemp[3] <= BB4_Zieltemp[1])
                    {
                        if (slot1.transform.childCount > 0 && (slot1.transform.GetChild(0).CompareTag("80SiHot") || slot1.transform.GetChild(0).CompareTag("80SiCold")))
                        {
                            tiegel4Farbe = 20;
                        }
                        else if (slot2.transform.childCount > 0 && (slot2.transform.GetChild(0).CompareTag("80SiHot") || slot2.transform.GetChild(0).CompareTag("80SiCold")))
                        {
                            tiegel4Farbe = 40;
                        }
                        else if (slot3.transform.childCount > 0 && (slot3.transform.GetChild(0).CompareTag("80SiHot") || slot3.transform.GetChild(0).CompareTag("80SiCold")))
                        {
                            tiegel4Farbe = 60;
                        }
                        else if (slot4.transform.childCount > 0 && (slot4.transform.GetChild(0).CompareTag("80SiHot") || slot4.transform.GetChild(0).CompareTag("80SiCold")))
                        {
                            tiegel4Farbe = 80;
                        }
                        //-------------------Showgraph--------------------
                        if (tiegel4Graph[1] == false && graphPunkt4 && graphPunkt5)
                        {
                            windowGraphTiegel4.ShowGraph(BB4_Zieltemp[0], BB4_Zeit[0] + BB4_Zeit[1] + BB4_Zeit[2] + BB4_Zeit[3], tiegel4Farbe);
                            tiegel4Graph[1] = true;
                        }
                        else if (tiegel4Graph[1] == false && graphPunkt4 && graphPunkt5 == false)
                        {
                            windowGraphTiegel4.ShowGraph(BB4_Zieltemp[0], BB4_Zeit[0] + BB4_Zeit[1] + BB4_Zeit[2], tiegel4Farbe);
                            tiegel4Graph[1] = true;
                        }
                        else if (tiegel4Graph[1] == false && graphPunkt4 == false && graphPunkt5 == false)
                        {
                            windowGraphTiegel4.ShowGraph(BB4_Zieltemp[0], BB4_Zeit[0] + BB4_Zeit[1], tiegel4Farbe);
                            tiegel4Graph[1] = true;
                        }
                        //------------------------------------------------
                        istTemp[3] -= (BB4_Zieltemp[1] - BB4_Zieltemp[0]) / BB4_Zeit[1];
                    }
                    //3ter Graph Punkt
                    else if (istTemp[3] > BB4_Zieltemp[1] && graphPunkt4 == false && graphPunkt5 == false)
                    {
                        if (slot1.transform.childCount > 0 && (slot1.transform.GetChild(0).CompareTag("80SiHot") || slot1.transform.GetChild(0).CompareTag("80SiCold")))
                        {
                            tiegel4Farbe = 20;
                        }
                        else if (slot2.transform.childCount > 0 && (slot2.transform.GetChild(0).CompareTag("80SiHot") || slot2.transform.GetChild(0).CompareTag("80SiCold")))
                        {
                            tiegel4Farbe = 40;
                        }
                        else if (slot3.transform.childCount > 0 && (slot3.transform.GetChild(0).CompareTag("80SiHot") || slot3.transform.GetChild(0).CompareTag("80SiCold")))
                        {
                            tiegel4Farbe = 60;
                        }
                        else if (slot4.transform.childCount > 0 && (slot4.transform.GetChild(0).CompareTag("80SiHot") || slot4.transform.GetChild(0).CompareTag("80SiCold")))
                        {
                            tiegel4Farbe = 80;
                        }
                        //-------------------Showgraph--------------------
                        if (tiegel4Graph[2] == false)
                        {
                            windowGraphTiegel4.ShowGraph(BB4_Zieltemp[2], 0, tiegel4Farbe);
                            windowGraphTiegel4.ShowGraph(BB4_Zieltemp[1], BB4_Zeit[0], tiegel4Farbe);
                            tiegel4Graph[2] = true;
                        }
                        if (tiegel4Graph[1] == false && BB4_Zieltemp[1] == BB4_Zieltemp[0])
                        {
                            windowGraphTiegel4.ShowGraph(BB4_Zieltemp[0], BB4_Zeit[0] + BB4_Zeit[1], tiegel4Farbe);
                            tiegel4Graph[1] = true;
                        }
                        //------------------------------------------------
                        istTemp[3] -= (BB4_Zieltemp[2] - BB4_Zieltemp[1]) / BB4_Zeit[2];
                    }
                    else if (istTemp[3] > BB4_Zieltemp[1] && istTemp[3] <= BB4_Zieltemp[2] && graphPunkt4 == true && graphPunkt5 == false)
                    {
                        if (slot1.transform.childCount > 0 && (slot1.transform.GetChild(0).CompareTag("80SiHot") || slot1.transform.GetChild(0).CompareTag("80SiCold")))
                        {
                            tiegel4Farbe = 20;
                        }
                        else if (slot2.transform.childCount > 0 && (slot2.transform.GetChild(0).CompareTag("80SiHot") || slot2.transform.GetChild(0).CompareTag("80SiCold")))
                        {
                            tiegel4Farbe = 40;
                        }
                        else if (slot3.transform.childCount > 0 && (slot3.transform.GetChild(0).CompareTag("80SiHot") || slot3.transform.GetChild(0).CompareTag("80SiCold")))
                        {
                            tiegel4Farbe = 60;
                        }
                        else if (slot4.transform.childCount > 0 && (slot4.transform.GetChild(0).CompareTag("80SiHot") || slot4.transform.GetChild(0).CompareTag("80SiCold")))
                        {
                            tiegel4Farbe = 80;
                        }
                        //-------------------Showgraph--------------------
                        if (tiegel4Graph[2] == false && graphPunkt4 && graphPunkt5 == false)
                        {
                            windowGraphTiegel4.ShowGraph(BB4_Zieltemp[1], BB4_Zeit[0] + BB4_Zeit[1], tiegel4Farbe);
                            tiegel4Graph[2] = true;
                        }
                        else if (tiegel4Graph[2] == false && graphPunkt4 && graphPunkt5)
                        {
                            windowGraphTiegel4.ShowGraph(BB4_Zieltemp[1], BB4_Zeit[0] + BB4_Zeit[1] + BB4_Zeit[2], tiegel4Farbe);
                            tiegel4Graph[2] = true;
                        }
                        //------------------------------------------------
                        istTemp[3] -= (BB4_Zieltemp[2] - BB4_Zieltemp[1]) / BB4_Zeit[2];
                    }
                    //4ter Graph Punkt
                    else if (istTemp[3] > BB4_Zieltemp[2] && graphPunkt4 == true && graphPunkt5 == false)
                    {
                        if (slot1.transform.childCount > 0 && (slot1.transform.GetChild(0).CompareTag("80SiHot") || slot1.transform.GetChild(0).CompareTag("80SiCold")))
                        {
                            tiegel4Farbe = 20;
                        }
                        else if (slot2.transform.childCount > 0 && (slot2.transform.GetChild(0).CompareTag("80SiHot") || slot2.transform.GetChild(0).CompareTag("80SiCold")))
                        {
                            tiegel4Farbe = 40;
                        }
                        else if (slot3.transform.childCount > 0 && (slot3.transform.GetChild(0).CompareTag("80SiHot") || slot3.transform.GetChild(0).CompareTag("80SiCold")))
                        {
                            tiegel4Farbe = 60;
                        }
                        else if (slot4.transform.childCount > 0 && (slot4.transform.GetChild(0).CompareTag("80SiHot") || slot4.transform.GetChild(0).CompareTag("80SiCold")))
                        {
                            tiegel4Farbe = 80;
                        }
                        //-------------------Showgraph--------------------
                        if (tiegel4Graph[3] == false)
                        {
                            windowGraphTiegel4.ShowGraph(BB4_Zieltemp[3], 0, tiegel4Farbe);
                            windowGraphTiegel4.ShowGraph(BB4_Zieltemp[2], BB4_Zeit[0], tiegel4Farbe);
                            tiegel4Graph[3] = true;
                        }
                        if (tiegel4Graph[2] == false && BB4_Zieltemp[2] == BB4_Zieltemp[1])
                        {
                            windowGraphTiegel4.ShowGraph(BB4_Zieltemp[0], BB4_Zeit[0] + BB4_Zeit[1] + BB4_Zeit[2], tiegel4Farbe);
                            tiegel4Graph[2] = true;
                        }
                        //------------------------------------------------
                        istTemp[3] -= (BB4_Zieltemp[3] - BB4_Zieltemp[2]) / BB4_Zeit[3];
                    }
                    else if (istTemp[3] > BB4_Zieltemp[2] && istTemp[3] <= BB4_Zieltemp[3] && graphPunkt4 == true && graphPunkt5 == true)
                    {
                        if (slot1.transform.childCount > 0 && (slot1.transform.GetChild(0).CompareTag("80SiHot") || slot1.transform.GetChild(0).CompareTag("80SiCold")))
                        {
                            tiegel4Farbe = 20;
                        }
                        else if (slot2.transform.childCount > 0 && (slot2.transform.GetChild(0).CompareTag("80SiHot") || slot2.transform.GetChild(0).CompareTag("80SiCold")))
                        {
                            tiegel4Farbe = 40;
                        }
                        else if (slot3.transform.childCount > 0 && (slot3.transform.GetChild(0).CompareTag("80SiHot") || slot3.transform.GetChild(0).CompareTag("80SiCold")))
                        {
                            tiegel4Farbe = 60;
                        }
                        else if (slot4.transform.childCount > 0 && (slot4.transform.GetChild(0).CompareTag("80SiHot") || slot4.transform.GetChild(0).CompareTag("80SiCold")))
                        {
                            tiegel4Farbe = 80;
                        }
                        //-------------------Showgraph--------------------
                        if (tiegel4Graph[3] == false)
                        {
                            windowGraphTiegel4.ShowGraph(BB4_Zieltemp[2], BB4_Zeit[0] + BB4_Zeit[1], tiegel4Farbe);
                            tiegel4Graph[3] = true;
                        }
                        //------------------------------------------------
                        istTemp[3] -= (BB4_Zieltemp[3] - BB4_Zieltemp[2]) / BB4_Zeit[3];
                    }
                    //5ter Graph Punkt
                    else if (istTemp[3] > BB4_Zieltemp[4] && graphPunkt4 == true && graphPunkt5 == true)
                    {
                        if (slot1.transform.childCount > 0 && (slot1.transform.GetChild(0).CompareTag("80SiHot") || slot1.transform.GetChild(0).CompareTag("80SiCold")))
                        {
                            tiegel4Farbe = 20;
                        }
                        else if (slot2.transform.childCount > 0 && (slot2.transform.GetChild(0).CompareTag("80SiHot") || slot2.transform.GetChild(0).CompareTag("80SiCold")))
                        {
                            tiegel4Farbe = 40;
                        }
                        else if (slot3.transform.childCount > 0 && (slot3.transform.GetChild(0).CompareTag("80SiHot") || slot3.transform.GetChild(0).CompareTag("80SiCold")))
                        {
                            tiegel4Farbe = 60;
                        }
                        else if (slot4.transform.childCount > 0 && (slot4.transform.GetChild(0).CompareTag("80SiHot") || slot4.transform.GetChild(0).CompareTag("80SiCold")))
                        {
                            tiegel4Farbe = 80;
                        }
                        //-------------------Showgraph--------------------
                        if (tiegel4Graph[4] == false)
                        {
                            windowGraphTiegel4.ShowGraph(BB4_Zieltemp[4], 0, tiegel4Farbe);
                            windowGraphTiegel4.ShowGraph(BB4_Zieltemp[3], BB4_Zeit[0], tiegel4Farbe);
                            tiegel4Graph[4] = true;
                        }
                        if (tiegel4Graph[3] == false && BB4_Zieltemp[3] == BB4_Zieltemp[2])
                        {
                            windowGraphTiegel4.ShowGraph(BB4_Zieltemp[0], BB4_Zeit[0] + BB4_Zeit[1] + BB4_Zeit[2] + BB4_Zeit[3], tiegel4Farbe);
                            tiegel4Graph[3] = true;
                        }
                        //------------------------------------------------
                        istTemp[3] -= (BB4_Zieltemp[4] - BB4_Zieltemp[3]) / BB4_Zeit[4];
                    }
                    else if (istTemp[3] < 25)
                    {
                        istTemp[3] = 25;
                    }
                }
                #endregion
                #endregion
                //Sobald alle Tiegel die Zieltemp. erreicht haben, dann beende diese Schleife
                if (istTemp[0] >= 1500 && istTemp[1] >= 1500 && istTemp[2] >= 1500 && istTemp[3] >= 1500)
                {
                    waiting = false;
                    break;
                }
            }
        }
    }
    #region Bunsen Brenner Flammen
    //Zünde Bunsen Brenner ganz links an
    public void Flamme1Button()
    {
        //Wenn entweder das Hauptgas oder das Platzgas noch nicht eingeschaltet ist, dann zeige die Info, dass beides eingeschaltet sein muss
        if ((hauptGasSchalter == false || platzGasSchalter == false) && verbrannt == false)
        {
            infoBBAusgeschaltet.gameObject.SetActive(true);
        }
        //Wenn man sich verbrannt hat, zeige Fenster, dass man zuerst sich verarzten soll, bevor man weiter arbeiten kann
        if(flamme1.activeInHierarchy == false && hauptGasSchalter && platzGasSchalter && verbrannt)
        {
            erstVerarztenFenster.SetActive(true);
        }
        if (flamme1.activeInHierarchy == false && hauptGasSchalter == true && platzGasSchalter && verbrannt == false && slot1.transform.childCount > 0)
        {
            if (slot1.transform.GetChild(0).CompareTag("Falsch") == false)
            {
                if (slot1.transform.GetChild(0).CompareTag("Richtig") == false)
                {
                    flamme1.SetActive(true);
                    flamme1Bool = true;
                    BunsenBrennerFlammen[0].gameObject.SetActive(true);
                    BunsenBrennerFlammen[0].Play();
                }
                else
                {
                    hinweisToolSchmelzen.SetActive(true);
                }
            }
            else
            {
                hinweisToolSchmelzen.SetActive(true);
            }
        }
        else
        {
            flamme1.SetActive(false);
            flamme1Bool = false;
            BunsenBrennerFlammen[0].gameObject.SetActive(false);
        }
    }
    //Zünde Bunsen Brenner links mittig an
    public void Flamme2Button()
    {
        if ((hauptGasSchalter == false || platzGasSchalter == false) && verbrannt == false)
        {
            infoBBAusgeschaltet.gameObject.SetActive(true);
        }
        if (flamme2.activeInHierarchy == false && hauptGasSchalter && platzGasSchalter && verbrannt)
        {
            erstVerarztenFenster.SetActive(true);
        }
        if (flamme2.activeInHierarchy == false && hauptGasSchalter == true && platzGasSchalter && verbrannt == false && slot2.transform.childCount > 0)
        {
            if (slot2.transform.GetChild(0).CompareTag("Falsch") == false)
            {
                if (slot2.transform.GetChild(0).CompareTag("Richtig") == false)
                {
                    flamme2.SetActive(true);
                    flamme2Bool = true;
                    BunsenBrennerFlammen[1].gameObject.SetActive(true);
                    BunsenBrennerFlammen[1].Play();
                }
                else
                {
                    hinweisToolSchmelzen.SetActive(true);
                }
            }
            else
            {
                hinweisToolSchmelzen.SetActive(true);
            }
        }
        else
        {
            flamme2.SetActive(false);
            flamme2Bool = false;
            BunsenBrennerFlammen[1].gameObject.SetActive(false);
        }
    }
    //Zünde Bunsen Brenner rechts mittig an
    public void Flamme3Button()
    {
        if ((hauptGasSchalter == false || platzGasSchalter == false) && verbrannt == false)
        {
            infoBBAusgeschaltet.gameObject.SetActive(true);
        }
        if (flamme3.activeInHierarchy == false && hauptGasSchalter && platzGasSchalter && verbrannt)
        {
            erstVerarztenFenster.SetActive(true);
        }
        if (flamme3.activeInHierarchy == false && hauptGasSchalter == true && platzGasSchalter && verbrannt == false && slot3.transform.childCount > 0)
        {
            if (slot3.transform.GetChild(0).CompareTag("Falsch") == false)
            {
                if (slot3.transform.GetChild(0).CompareTag("Richtig") == false)
                {
                    flamme3.SetActive(true);
                    flamme3Bool = true;
                    BunsenBrennerFlammen[2].gameObject.SetActive(true);
                    BunsenBrennerFlammen[2].Play();
                }
                else
                {
                    hinweisToolSchmelzen.SetActive(true);
                }
            }
            else
            {
                hinweisToolSchmelzen.SetActive(true);
            }
        }
        else
        {
            flamme3.SetActive(false);
            flamme3Bool = false;
            BunsenBrennerFlammen[2].gameObject.SetActive(false);
        }
    }
    //Zünde Bunsen Brenner ganz rechts an
    public void Flamme4Button()
    {
        if ((hauptGasSchalter == false || platzGasSchalter == false) && verbrannt == false)
        {
            infoBBAusgeschaltet.gameObject.SetActive(true);
        }
        if (flamme4.activeInHierarchy == false && hauptGasSchalter && platzGasSchalter && verbrannt)
        {
            erstVerarztenFenster.SetActive(true);
        }
        if (flamme4.activeInHierarchy == false && hauptGasSchalter == true && platzGasSchalter && verbrannt == false && slot4.transform.childCount > 0)
        {
            if (slot4.transform.GetChild(0).CompareTag("Falsch") == false)
            {
                if (slot4.transform.GetChild(0).CompareTag("Richtig") == false)
                {
                    flamme4.SetActive(true);
                    flamme4Bool = true;
                    BunsenBrennerFlammen[3].gameObject.SetActive(true);
                    BunsenBrennerFlammen[3].Play();
                }
                else
                {
                    hinweisToolSchmelzen.SetActive(true);
                }
            }
            else
            {
                hinweisToolSchmelzen.SetActive(true);
            }
        }
        else
        {
            flamme4.SetActive(false);
            flamme4Bool = false;
            BunsenBrennerFlammen[3].gameObject.SetActive(false);
        }
    }
    #endregion

    //Funktion für den schließen Button der Verbrennung
    public void CloseVerbranntWindow()
    {
        verbranntFenster.SetActive(false);
        zeigeVerbrennung = false;
    }

    //Sobald der Verbandskasten nach einer Verbrennung genutzt wurde, setze die Variable verbrannt wieder false
    public void VerbrennungBehandelt()
    {
        verbrannt = false;
    }

    //Funktion für den schließen Button des vebrannt Fensters
    public void CloseErstVerarztenWindow()
    {
        erstVerarztenFenster.SetActive(false);
    }
}
