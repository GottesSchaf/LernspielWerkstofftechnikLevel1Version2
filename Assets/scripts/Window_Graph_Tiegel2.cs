using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Window_Graph_Tiegel2 : MonoBehaviour {
    [SerializeField] Sprite circleSprite;
    [SerializeField] private RectTransform graphContainer;
    private int i = 0;
    GameObject lastCircleGameObject;
    int tiegelColor;
    //BunsenBrenner bunsenBrenner = new BunsenBrenner();
    [SerializeField] GameObject PanelTiegel;
    bool changedPos;
    [SerializeField] GameObject[] BBSlot;

    public void DeleteGraph()
    {
        if (graphContainer.transform.childCount > 0)
        {
            foreach (Transform child in graphContainer)
            {
                Destroy(child.gameObject);
            }
            i = 0;
        }
    }

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

    public void ShowGraph(float value, int sekunden, int tiegelFarbe) //Vorher: ShowGraph(List<int> valueList)
    {
        
        graphContainer = this.gameObject.GetComponentsInChildren<RectTransform>(true)[1];
        tiegelColor = tiegelFarbe;
        float graphHeight = graphContainer.sizeDelta.y; //Größe des Graphen
        float yMaximum = 2000; //Maximale Größe des Graphen
        float xSize = sekunden; //Abstand zwischen X Positionen (sekunden)
        //GameObject lastCircleGameObject = null; //Letzter Punkt, der erstellt wurde
        //Vorher: if(i < valueList.Count)
        float xPosition = i * xSize;
        float yPosition = (value / yMaximum) * graphHeight;
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
        gameObject.transform.SetParent(graphContainer, false);
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
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        rectTransform.anchorMin = new Vector2(0, 0); //Ankerpunkt unten links
        rectTransform.anchorMax = new Vector2(0, 0); //Ankerpunkt unten links
        rectTransform.sizeDelta = new Vector2(distance, 3f);
        rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(dir));
    }

    public static float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }
}
