using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{
    public static bool otherSlot;

    public GameObject item
    {
        get
        {
            if (transform.childCount > 0)
            {
                return transform.GetChild(0).gameObject;
            }
            return null;
        }
    }

    #region IDropHandler implementation
    public void OnDrop(PointerEventData eventData)
    {
        if (!item && DragHandeler.cantTransform == false)
        {
            DragHandeler.itemBeingDragged.transform.SetParent(transform);
            otherSlot = true;
            ExecuteEvents.ExecuteHierarchy<IHasChanged>(gameObject, null, (x, y) => x.HasChanged());
            if (transform.parent.transform.parent.transform.parent.transform.name.Contains("InventoryMen"))
            {
                CollisionDetection.itemInInventory = true;
            }
            else
            {
                CollisionDetection.itemInInventory = false;
            }
            //DragHandeler.itemBeingDragged = null;
        }
        else
        {
            Debug.Log("Hier ist schon ein Item im Slot " + this.gameObject.name);
            otherSlot = false;
        }
    }
    #endregion
}