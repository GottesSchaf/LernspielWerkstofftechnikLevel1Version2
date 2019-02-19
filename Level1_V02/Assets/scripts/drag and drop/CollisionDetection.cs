using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour {

    bool isCollidingBlue, isCollidingRed;
    GameObject UICanvas;
    public static bool itemInInventory = false;

    private void Start()
    {
        UICanvas = GameObject.Find("Canvas");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        //if (collision.CompareTag("redCube") && DragHandeler.itemBeingDragged.name.Contains("blue"))
        //{
        //    Debug.Log("Item blau über Item rot");
        //    isCollidingRed = true;
        //}
        //else if (collision.CompareTag("blueCube") && DragHandeler.itemBeingDragged.name.Contains("red"))
        //{
        //    Debug.Log("Item rot über Item blau");
        //    isCollidingBlue = true;
        //}
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Inventory"))
        {
            if (DragHandeler.draggingItem)
            {
                DragHandeler.Inventory.SetActive(false);
                DragHandeler.itemBeingDragged.transform.SetParent(UICanvas.transform);
                DragHandeler.itemBeingDragged.transform.SetAsLastSibling();
                itemInInventory = true;
            }
        }
    }
}
