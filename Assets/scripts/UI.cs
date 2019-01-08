using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    public Transform RadialMenue;
    public Transform MainMenue;
    public Transform Inventory;
    public Transform Album;
    public Transform Map;
    public Transform CloseupBack;
    public Transform InventoryCollision;
    public Transform MachineWindow, BunsenBrennerWindow;
    public Text zieltemp;               //Textfeld für die Zieltemperatur
    public InputField inputZieltemp;    //Das Eingabefeld für die Zieltemperatur
    bool inputZieltempBool = false;     //Boolean für Überprüfung ob das Zieltemperatur Eingabefeld ausgewählt ist
    public InputField inputDauer;       //Eingabefeld für die Dauer
    public Text rateTemp;               //Textfeld für die Rate in °C/h
    bool inputRateBool = false;         //Boolean für Überprüfung ob das Rate °C/h Eingabefeld ausgewählt ist
    float laufzeitSek, laufzeitMin, LaufzeitStu;    //Speicher für die Laufzeit des Ofens in Sekunden, Minuten und Stunden
    public Text laufzeitText;           //Ausgabe Textfeld für die aktuelle Laufzeit
    public Text aktuelleTempText;       //Ausgabe Textfeld für die aktuelle Temperatur
    bool laufzeitBool = false;          //Boolean zum überprüfen ob der Ofen auch anlaufen kann
    float aktuelleTemp = 25, zielTempSpeicher, dauerSpeicher;       //Speicher für die aktuelle, zu erreichende und Rate (°C/h) Temperatur
    bool canStart = false, waiting = false;              //Boolean zur Überprüfung ob der Ofen gestartet werden kann
    float[,] funktionen = new float[8, 3];                //Array zum abspeichern von den Funktionen (maximal 8), [X,0] Dauer // [X,1] StartTemperatur // [X,2] ZielTemperatur
    public int arrayPosX, arrayTextPos;           //Aktuelle Position im Array
    public Text[] tabelleText;          //Textfelder der Tabelle für die Ausgabe der eingegebenen Werte
    bool abgekuehlt = false;            //Array zum überprüfen, ob der Ofen noch aufgeheizt ist wenn keine weitere Eingabe vorhanden sind
    public static bool tutorialinventory; // Button für das Tutorial 

    public void Start()
    {
        SetStartTemperatur();
    }

    public void Update()
    {
        InputFieldUpdate();
        //Nur wenn der Ofen die richtigen Einstellungen hat, kann dieser gestartet werden
        if (laufzeitBool)
        {
            Laufzeit_Ofen();
            StartCoroutine(TemperaturRechner());
        }
    }

    #region UI Buttons
    public void Button_OpenMenue()
    {
        if (RadialMenue.gameObject.activeInHierarchy == false)
        {
            RadialMenue.gameObject.SetActive(true);
        }
        else
        {
            if (tutorialinventory)
            {
                Tutorial.step4Done = true; 
            }

            RadialMenue.gameObject.SetActive(false);
            MainMenue.gameObject.SetActive(false);
            Inventory.gameObject.SetActive(false);
            Album.gameObject.SetActive(false);
            Map.gameObject.SetActive(false);
        }
    }

    public void Button_Screenshot()
    {
        Debug.Log("A screenshot was taken.");
    }

    public void Button_MainMenue()
    {
        Debug.Log("The MainMenueButton was clicked.");

        if (MainMenue.gameObject.activeInHierarchy == false)
        {
            MainMenue.gameObject.SetActive(true);
            Inventory.gameObject.SetActive(false);
            Album.gameObject.SetActive(false);
        }
        else
        {
            MainMenue.gameObject.SetActive(false);
        }
    }

    public void Button_Inventory()
    {
        Debug.Log("The InventoryButton was clicked.");

        if (Inventory.gameObject.activeInHierarchy == false)
        {
            MainMenue.gameObject.SetActive(false);
            Inventory.gameObject.SetActive(true);
            Album.gameObject.SetActive(false);
            InventoryCollision.gameObject.SetActive(true);
        }
        else
        {
            Inventory.gameObject.SetActive(false);
            InventoryCollision.gameObject.SetActive(false);
        }
    }

    public void Button_Album()
    {
        Debug.Log("The AlbumButton was clicked.");

        if (Album.gameObject.activeInHierarchy == false)
        {
            MainMenue.gameObject.SetActive(false);
            Inventory.gameObject.SetActive(false);
            Album.gameObject.SetActive(true);
        }
        else
        {
            Album.gameObject.SetActive(false);
        }
    }

    public void Button_Map()
    {
        Debug.Log("The MapButton was clicked.");
        Map.gameObject.SetActive(true);
    }

    public void Button_CloseMap()
    {
        Debug.Log("The Map was closed.");
        Map.gameObject.SetActive(false);
    }

    public void Button_Closeup()
    {
        Debug.Log("The Closeup was closed.");

        if (CameraFollow.instance.closeupInteraction == true)
        {
            CloseupBack.gameObject.SetActive(false);
            MachineWindow.gameObject.SetActive(false);
            BunsenBrennerWindow.gameObject.SetActive(false);
            CameraFollow.instance.closeupInteraction = false;
            CameraFollow.instance.playerToFollow.GetComponent<SkinnedMeshRenderer>().enabled = true;
			//CameraFollow.instance.playerToFollow.Find("clothes_green").GetComponent<MeshRenderer>().enabled = true;
            Book.instance.GetComponent<BoxCollider>().enabled = false;
        }
    }

    public void InputFieldUpdate()
    {
        if(inputZieltemp.isFocused == true)
        {
            inputZieltempBool = true;
            inputRateBool = false;
        }
        else if (inputDauer.isFocused == true)
        {
            inputZieltempBool = false;
            inputRateBool = true;
        }
    }
    #endregion

    #region Numpad
    //Numpad Button 1 fügt, je nach ausgewähltem Eingabefeld, eine 1 der Zahl hinzu. Gleiches gilt für Numapad 2 - 0
    public void Button_Numpad1()
    {
        if(inputZieltempBool)
        {
            inputZieltemp.text += "1";
        }
        else if (inputRateBool)
        {
            inputDauer.text += "1";
        }
    }

	public void Button_Numpad2()
	{
        if (inputZieltempBool)
        {
            inputZieltemp.text += "2";
        }
        else if (inputRateBool)
        {
            inputDauer.text += "2";
        }
    }

	public void Button_Numpad3()
	{
        if (inputZieltempBool)
        {
            inputZieltemp.text += "3";
        }
        else if (inputRateBool)
        {
            inputDauer.text += "3";
        }
    }

	public void Button_Numpad4()
	{
        if (inputZieltempBool)
        {
            inputZieltemp.text += "4";
        }
        else if (inputRateBool)
        {
            inputDauer.text += "4";
        }
    }

	public void Button_Numpad5()
	{
        if (inputZieltempBool)
        {
            inputZieltemp.text += "5";
        }
        else if (inputRateBool)
        {
            inputDauer.text += "5";
        }
    }

	public void Button_Numpad6()
	{
        if (inputZieltempBool)
        {
            inputZieltemp.text += "6";
        }
        else if (inputRateBool)
        {
            inputDauer.text += "6";
        }
    }

    public void Button_Numpad7()
    {
        if (inputZieltempBool)
        {
            inputZieltemp.text += "7";
        }
        else if (inputRateBool)
        {
            inputDauer.text += "7";
        }
    }

	public void Button_Numpad8()
	{
        if (inputZieltempBool)
        {
            inputZieltemp.text += "8";
        }
        else if (inputRateBool)
        {
            inputDauer.text += "8";
        }
    }

	public void Button_Numpad9()
	{
        if (inputZieltempBool)
        {
            inputZieltemp.text += "9";
        }
        else if (inputRateBool)
        {
            inputDauer.text += "9";
        }
    }

	public void Button_Numpad0()
	{
        if (inputZieltempBool)
        {
            inputZieltemp.text += "0";
        }
        else if (inputRateBool)
        {
            inputDauer.text += "0";
        }
    }
    //Resettet das ausgewählte Eingabefeld
	public void Button_NumpadC()
	{
        if (inputZieltempBool)
        {
            inputZieltemp.text = "";
        }
        else if (inputRateBool)
        {
            inputDauer.text = "";
        }
    }
    //Übergibt die eingegebenen Zahlen an die Schmelze
	public void Button_NumpadOK()
	{
        if(inputZieltemp.text != "" && inputDauer.text != "" && funktionen[0,0] != 0 && funktionen[0,2] != 0)
        {
            if (funktionen[1,1] >= 25 && funktionen[1,0] > 0)
            {
                //GUI.Box(new Rect(0, 0, Screen.width / 2, Screen.height / 2), "Daten erfolgreich übergeben");
                canStart = true;
            }
            else
            {
                //GUI.Box(new Rect(0, 0, Screen.width / 2, Screen.height / 2), "Falsche Eingabe im Eingabefeld!");
            }
        }
        else
        {
        }
    }

    public void Button_Start()
    {
        if (canStart && funktionen[0, 0] != 0 && funktionen[0,2] != 0)
        {
            //GUI.Box(new Rect(0, 0, Screen.width / 2, Screen.height / 2), "Ofen wurde gestartet");
            
            arrayPosX = 0;
            laufzeitBool = true;
            canStart = false;
        }
        else
        {
            //GUI.Box(new Rect(0, 0, Screen.width / 2, Screen.height / 2), "Nicht genug Daten eingegeben!");
            Debug.Log("Bitte genug Daten eingeben und mit OK bestätigen.");
        }
    }
    #endregion

    public void Laufzeit_Ofen()
    {
        //Immer wenn eine Minute rum ist, erhöhe Minute um 1 und setze Sekunden wieder auf 0 und wenn 1 Stunde rum ist, erhöhe Stunde um 1 und setze Minute auf 0
        laufzeitSek += Time.deltaTime;
        if(laufzeitSek >= 60f)
        {
            laufzeitMin++;
            laufzeitSek = 0;
        }
        if(laufzeitMin >= 60f)
        {
            LaufzeitStu++;
            laufzeitMin = 0;
        }
        //Die Korrekte Anzeige der Zeit in 00:00:00 Format
        #region Laufzeit Anzeige
        if(LaufzeitStu < 10 && laufzeitMin < 10 && laufzeitSek < 10)
            laufzeitText.text = "Laufzeit: 0" + LaufzeitStu + ":0" + laufzeitMin + ":0" + Mathf.Round(laufzeitSek);
        else if (LaufzeitStu < 10 && laufzeitMin < 10)
            laufzeitText.text = "Laufzeit: 0" + LaufzeitStu + ":0" + laufzeitMin + ":" + Mathf.Round(laufzeitSek);
        else if (LaufzeitStu < 10  && laufzeitSek < 10)
            laufzeitText.text = "Laufzeit: 0" + LaufzeitStu + ":" + laufzeitMin + ":0" + Mathf.Round(laufzeitSek);
        else if (laufzeitMin < 10 && laufzeitSek < 10)
            laufzeitText.text = "Laufzeit: " + LaufzeitStu + ":0" + laufzeitMin + ":0" + Mathf.Round(laufzeitSek);
        else if (laufzeitSek < 10)
            laufzeitText.text = "Laufzeit: " + LaufzeitStu + ":" + laufzeitMin + ":0" + Mathf.Round(laufzeitSek);
        else if (laufzeitMin < 10)
            laufzeitText.text = "Laufzeit: " + LaufzeitStu + ":0" + laufzeitMin + ":" + Mathf.Round(laufzeitSek);
        else if (LaufzeitStu < 10)
            laufzeitText.text = "Laufzeit: 0" + LaufzeitStu + ":" + laufzeitMin + ":" + Mathf.Round(laufzeitSek);
        else
            laufzeitText.text = "Laufzeit: " + LaufzeitStu + ":" + laufzeitMin + ":" + Mathf.Round(laufzeitSek);
        #endregion
    }

    public IEnumerator TemperaturRechner()
    {
        if (arrayPosX < funktionen.Length && waiting == false) //Nur wenn die letzte Ofenfunktion noch nicht erreicht wurde und 1 Sekunde noch nicht abgelaufen ist
        {
            if (aktuelleTemp < funktionen[arrayPosX, 2]) //Solange die aktuelle Temp. kleiner als die zu erreichende Temp. ist
            {
                waiting = true;
                yield return new WaitForSeconds(1);     //Warte 1 Sekunde
                if (funktionen[arrayPosX, 0] > 0 && funktionen[arrayPosX, 1] > 25 && funktionen[arrayPosX, 2] > 25) //Wenn die Zeit größer als 0 ist, die zu ist Temp. größer als 25°C und die zu erreichende Temp. größer als 25°C ist
                {
                    aktuelleTemp += (funktionen[arrayPosX, 2] - funktionen[arrayPosX, 1]) / (funktionen[arrayPosX, 0] * 60);    //Errechne die sekündliche Steigerungsrate der Temperatur und addiere sie zur aktuellen Temperatur hinzu
                }
                //Sonst stoppe den Ofen und setze die Temp. auf 25°C
                else
                {
                    laufzeitBool = false;
                    aktuelleTemp = 25;
                }
                aktuelleTempText.text = "Aktuelle Temp.: " + Mathf.Round(aktuelleTemp) + "°C";
                waiting = false;
            }
            //Wenn die aktuelle Temp. größer als die zu erreichende Temp ist
            else if(aktuelleTemp > funktionen[arrayPosX, 2])
            {
                waiting = true;
                yield return new WaitForSeconds(1);
                if(aktuelleTemp <= 25)
                {
                    abgekuehlt = true;
                }
                if (funktionen[arrayPosX, 0] > 0 && funktionen[arrayPosX, 1] > 25 && funktionen[arrayPosX, 2] > 0 || abgekuehlt == false)
                {
                    aktuelleTemp -= (funktionen[arrayPosX, 1] - funktionen[arrayPosX, 2]) / (funktionen[arrayPosX, 0] * 60);    //Errechne die sekündliche Steigerungsrate der Temperatur und addiere sie zur aktuellen Temperatur hinzu
                }
                else
                {
                    laufzeitBool = false;
                    aktuelleTemp = 25;
                }
                aktuelleTempText.text = "Aktuelle Temp.: " + Mathf.Round(aktuelleTemp) + "°C";
                waiting = false;
            }
            else
            {
                arrayPosX++;
            }
        }
        else if (arrayPosX >= funktionen.Length)
        {
            laufzeitBool = false;
            aktuelleTemp = 25;
        }
    }
    //Fügt eine weitere Funktion zum Ofenprogramm hinzu
    public void Button_AddFunction()
    {
        if(inputZieltemp.text != "" && inputDauer.text != "" && arrayPosX <= 8) //Nur wenn etwas in beiden Eingabefeldern drinnen steht und die Maximale Funktionen Anzahl noch nicht erreicht ist
        {
            if(float.TryParse(inputZieltemp.text, out zielTempSpeicher) && float.TryParse(inputDauer.text, out dauerSpeicher)) //Überprüfe ob im Eingabefeld nur Kommazahlen sind
            {
                zielTempSpeicher = float.Parse(inputZieltemp.text);
                dauerSpeicher = float.Parse(inputDauer.text);
                if (zielTempSpeicher > 24 && dauerSpeicher > 0 && zielTempSpeicher < 1901) //Nur wenn die Eingegebene Temp. größer 25°C, die Dauer zum erreichen mehr als 0 min., aber weniger als 1900°C beträgt
                {
                    //Überprüfe, ob die maximale Temperatur Rate pro Stunde überschritten wurde
                    if ((zielTempSpeicher - funktionen[arrayPosX, 2]) / (dauerSpeicher * 60) < 0.22222222222222222222222222222 && (zielTempSpeicher - funktionen[arrayPosX, 2]) / (dauerSpeicher * 60) > -0.22222222222222222222222222222)
                    {
                        //Wenn die Position im Array die erste ist, befülle nur die neue temp. und zeit
                        if (arrayPosX == 0)
                        {
                            funktionen[arrayPosX, 0] = dauerSpeicher;
                            funktionen[arrayPosX, 2] = zielTempSpeicher;
                            tabelleText[arrayTextPos].text = "" + dauerSpeicher;
                            tabelleText[arrayTextPos + 1].text = "" + funktionen[0, 1];
                            tabelleText[arrayTextPos + 2].text = "" + zielTempSpeicher;
                        }
                        //Sonst befülle neue temp., zeit und alte temp.
                        else
                        {
                            funktionen[arrayPosX, 0] = dauerSpeicher;
                            funktionen[arrayPosX, 1] = funktionen[arrayPosX - 1, 2];
                            funktionen[arrayPosX, 2] = zielTempSpeicher;
                            tabelleText[arrayTextPos].text = "" + dauerSpeicher;
                            tabelleText[arrayTextPos + 1].text = "" + funktionen[arrayPosX - 1, 2];
                            tabelleText[arrayTextPos + 2].text = "" + zielTempSpeicher;
                        }
                    }
                    else
                    {
                        Debug.Log("Zu große Temperaturrate!");
                    }
                }
                else
                {
                    Debug.Log("Negative Zahlen sind nicht möglich");
                }
            }
            else
            {
                Debug.Log("Invalide Eingabe");
            }
            arrayPosX++;
            arrayTextPos += 3; //Wegen 1 Dimensionalem Array += 3, da dort die nächste Reihe gespeichert wird
        }
        else
        {
            Debug.Log("Bitte in beide Eingabefelder eine valide Zahl eingeben.");
        }
    }
    //Setzt die Funktionen des Ofens wieder auf Standard zurück
    public void Button_ResetFunction()
    {
        arrayPosX = 0;
        arrayTextPos = 0;
        funktionen = new float[8,3];
        SetStartTemperatur();
    }
    //Wenn das Spiel startet, wird der Array des Ofens befüllt, oder wenn man den reset Button klickt
    public void SetStartTemperatur()
    {
        funktionen[0, 0] = 0;
        funktionen[0, 1] = 25;
        funktionen[0, 2] = 0;
        tabelleText[arrayTextPos].text = "" + funktionen[0, 0];
        tabelleText[arrayTextPos + 1].text = "" + funktionen[0, 1];
        tabelleText[arrayTextPos + 2].text = "" + funktionen[0, 2];
        arrayTextPos += 3;
        for (int i = 1; i < 8; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                funktionen[i, j] = 0;
            }
            tabelleText[arrayTextPos].text = "" + funktionen[i, 0];
            tabelleText[arrayTextPos + 1].text = "" + funktionen[i, 1];
            tabelleText[arrayTextPos + 2].text = "" + funktionen[i, 2];
            arrayTextPos += 3;
        }
        arrayTextPos = 0;
        aktuelleTempText.text = "Aktuelle Temp.: " + Mathf.Round(aktuelleTemp) + "°C";
    }
}
