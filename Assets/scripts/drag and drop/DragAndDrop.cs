using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour {

    public const string DRAGGABLE_TAG = "UIDraggable";
    bool dragging = false;
    Vector2 startPos;
    Transform objectToDrag;
    Image objectToDragImage;
    List<RaycastResult> hitObjects = new List<RaycastResult>();
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0))
        {
            objectToDrag = GetDraggableTransformUnderMouse();

            if(objectToDrag != null)
            {
                dragging = true;
                objectToDrag.SetAsLastSibling();
                startPos = objectToDrag.position;
                objectToDragImage = objectToDrag.GetComponent<Image>();
                objectToDragImage.raycastTarget = false;
            }
        }

        if (dragging)
        {
            objectToDrag.position = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if(objectToDrag != null)
            {
                Transform objectToReplace = GetDraggableTransformUnderMouse();

                if(objectToReplace != null)
                {
                    objectToDrag.position = objectToReplace.position;
                    objectToReplace.position = startPos;
                }
                else
                {
                    objectToDrag.position = startPos;
                }

                objectToDragImage.raycastTarget = true;
                objectToDrag = null;
            }

            dragging = false;
        }
	}

    GameObject GetObjectUnderMouse()
    {
        var pointer = new PointerEventData(EventSystem.current);

        pointer.position = Input.mousePosition;
        EventSystem.current.RaycastAll(pointer, hitObjects);

        if(hitObjects.Count <= 0)
        {
            return null;
        }

        return hitObjects.First().gameObject;
    }

    Transform GetDraggableTransformUnderMouse()
    {
        GameObject clickedObject = GetObjectUnderMouse();

        if(clickedObject != null && clickedObject.tag == DRAGGABLE_TAG)
        {
            return clickedObject.transform;
        }

        return null;
    }
}
