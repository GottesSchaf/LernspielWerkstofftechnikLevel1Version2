using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Window_Graph : MonoBehaviour {
    [SerializeField] Sprite circleSprite;
    [SerializeField] private RectTransform graphContainer;
    private int i = 0;
    GameObject lastCircleGameObject;
    int tiegelColor;
    [SerializeField] GameObject PanelTiegel;
    bool changedPos;
    [SerializeField] GameObject[] BBSlot;

    private GameObject CreatCircle(Vector2 anchoredPosition)
    {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(0, 0); //Größe des Punktes
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return gameObject;
    }

    public void DeleteGraph()
    {
        if (graphContainer.transform.childCount > 0)
        {
            foreach(Transform child in graphContainer)
            {
                Destroy(child.gameObject);
            }
            i = 0;
        }
    }
    public void ShowGraph(float value, int sekunden, int tiegelFarbe) //Vorher: ShowGraph(List<int> valueList)
    {
        //if (changedPos == false)
        //{
        //    if (BBSlot[0].GetComponentInChildren<GameObject>().CompareTag("20SiCold"))
        //    {
        //        PanelTiegel.transform.position = new Vector2(-42, -101);
        //        PanelTiegel.layer = 0;
        //    }
        //    else if (BBSlot[1].GetComponentInChildren<GameObject>().CompareTag("20SiCold"))
        //    {
        //        PanelTiegel.transform.position = new Vector2(-42, -231);
        //        PanelTiegel.layer = 0;
        //    }
        //    else if (BBSlot[2].GetComponentInChildren<GameObject>().CompareTag("20SiCold"))
        //    {
        //        PanelTiegel.transform.position = new Vector2(-42, -387);
        //        PanelTiegel.layer = 0;
        //    }
        //    else if (BBSlot[3].GetComponentInChildren<GameObject>().CompareTag("20SiCold"))
        //    {
        //        PanelTiegel.transform.position = new Vector2(-42, -541);
        //        PanelTiegel.layer = 0;
        //    }
        //    else
        //    {
        //        PanelTiegel.transform.position = new Vector2(-42, -101);
        //        PanelTiegel.layer = 1;
        //        Debug.Log("Konnte kein Tiegel finden mit dem Tag '20SiCold'");
        //    }
        //    changedPos = true;
        //}
        graphContainer = this.gameObject.GetComponentsInChildren<RectTransform>(true)[1];
        tiegelColor = tiegelFarbe;

        float graphHeight = graphContainer.sizeDelta.y; //Größe des Graphen
        float yMaximum = 2000; //Maximale Größe des Graphen
        float xSize = sekunden; //Abstand zwischen X Positionen (sekunden)
        float xPosition = i * xSize; //Berechne die X Position auf dem Graphen
        float yPosition = (value / yMaximum) * graphHeight; //Berechne die Y Position auf dem Graphen
        float yPos = (float)yPosition;
        GameObject circleGameObject = CreatCircle(new Vector2(xPosition, yPos));
        //Falls ein vorheriger Punkt vorhanden, erstelle eine Verbindung
        if (lastCircleGameObject != null)
        {
            CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
        }
        lastCircleGameObject = circleGameObject; //setze den letzten Punkt zum aktuellen
        i++;
    }

    private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB)
    {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);  //Setze den Parent des neuen Objektes, auch wenn der Parent nicht aktiv ist in der Hierarchy
        if (tiegelColor == 20)
        {
            gameObject.GetComponent<Image>().color = new Color(1, 1, 1, .75f); //R, G, B, Transparenz
        }
        else if (tiegelColor == 40)
        {
            gameObject.GetComponent<Image>().color = new Color(1, 0, 0, .75f); //R, G, B, Transparenz
        }
        else if (tiegelColor == 60)
        {
            gameObject.GetComponent<Image>().color = new Color(0, 1, 0, .75f); //R, G, B, Transparenz
        }
        else if (tiegelColor == 80)
        {
            gameObject.GetComponent<Image>().color = new Color(0, 106, 214, .75f); //R, G, B, Transparenz
        }
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized;     //Setze die Länge des Vektors auf 1
        float distance = Vector2.Distance(dotPositionA, dotPositionB); //Errechne die Distanz zwischen zwei Punkten
        rectTransform.anchorMin = new Vector2(0, 0); //Ankerpunkt unten links
        rectTransform.anchorMax = new Vector2(0, 0); //Ankerpunkt unten links
        rectTransform.sizeDelta = new Vector2(distance, 3f);
        rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(dir));   //Stelle den Winkel der Geraden zwischen den Punkten richtig ein
    }

    public static float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }
}
