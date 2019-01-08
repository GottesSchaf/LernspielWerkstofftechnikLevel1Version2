using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragHandeler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public static GameObject itemBeingDragged;
    public static bool draggingItem;
    [SerializeField] Vector3 startPosition;
    [SerializeField] Transform startParent;
    private Ray updateRay;
    private RaycastHit updateHit;
    public GameObject RadialMenue;
    public GameObject cam;
    public static GameObject Inventory;
    public GameObject InventoryCollision;
    public GameObject UICanvas;
	public GameObject Machine, machineNew, machineNewParent;
    public GameObject iconSprite;
    GameObject[] shatter;
    List<GameObject> shatter1 = new List<GameObject>();
    public Slot MachineSlot;
    public GameObject player;
    public GameObject mesh;
    GameObject gameOverScreen;
    DestroyMachine desMachine;
    GameObject invFix;
    bool transformDone;
    public static bool cantTransform;

    private void Start()
    {
        cantTransform = false;
    }
    #region IBeginDragHandler implementation

    public void OnBeginDrag(PointerEventData eventData)
    {
        itemBeingDragged = gameObject;
        draggingItem = true;
        if (transform.parent != startParent)
        {
            startPosition = transform.position;
        }
        startParent = transform.parent;
        GetComponent<CanvasGroup>().blocksRaycasts = false;

        RadialMenue = GameObject.Find("RadialMenue");
        cam = GameObject.Find("Main Camera");
        Inventory = GameObject.Find("InventoryMenue");
        UICanvas = GameObject.Find("Canvas");
        Machine = GameObject.Find("Machine");
        gameOverScreen = GameObject.Find("Maschine_Kaputt");
        //invFix = GameObject.Find("InventoryFix");
        //invFix.SetActive(true);
        //invFix.SetActive(false);
    }

    #endregion

    #region IDragHandler implementation

    public void OnDrag(PointerEventData eventData)
    {
        if ((this.gameObject.name.Contains("Tiegel_1") && BunsenBrenner.tiegelLocked20 == false) || (this.gameObject.name.Contains("Tiegel_2") && BunsenBrenner.tiegelLocked40 == false) || (this.gameObject.name.Contains("Tiegel_3") && BunsenBrenner.tiegelLocked60 == false) || (this.gameObject.name.Contains("Tiegel_4") && BunsenBrenner.tiegelLocked80 == false))
        {
            cantTransform = false;
            transform.position = Input.mousePosition; //eventData.position
            if (CollisionDetection.itemInInventory == false)
            {
                if (transformDone == false)
                {
                    itemBeingDragged.transform.SetParent(UICanvas.transform, false);
                    itemBeingDragged.transform.SetAsLastSibling();
                    transformDone = true;
                }
            }
        }
        else if (this.gameObject.name.Contains("Tiegel_1") == false && this.gameObject.name.Contains("Tiegel_2") == false && this.gameObject.name.Contains("Tiegel_3") == false && this.gameObject.name.Contains("Tiegel_4") == false)
        {
            cantTransform = false;
            transform.position = Input.mousePosition; //eventData.position
            if (CollisionDetection.itemInInventory == false)
            {
                if (transformDone == false)
                {
                    itemBeingDragged.transform.SetParent(UICanvas.transform, false);
                    itemBeingDragged.transform.SetAsLastSibling();
                    transformDone = true;
                }
            }
        }
        else
        {
            Debug.Log("Error: Kann GameObject nicht bewegen");
            cantTransform = true;
        }
    }

    #endregion

    #region IEndDragHandler implementation

    public void OnEndDrag(PointerEventData eventData)
    {
        //raycast
        RaycastHit hit = new RaycastHit();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.transform.CompareTag("Machine") && itemBeingDragged.transform.tag == "Richtig")
            {
                //Gewonnen
                Machine = GameObject.Find("Crazy_Machine_Shatter");
                iconSprite = GameObject.Find("Gear_Icon");
                Machine.SetActive(false);
                machineNewParent = GameObject.Find("NewMachineParent");
                machineNew = machineNewParent.transform.GetChild(0).gameObject;
                //for (int i = 0; i < Machine.transform.childCount; i++)
                //{
                //    shatter1.Add(Machine.transform.GetChild(i).gameObject);
                //}
                //int childCount = Machine.transform.childCount;
                //for(int i = 0; i < childCount; i++)
                //{
                //    Destroy(Machine.transform.GetChild(0).gameObject);
                //}
                if (machineNew != null)
                {
                    machineNew.SetActive(true);
                    if (iconSprite != null)
                    {
                        iconSprite.SetActive(false);
                    }
                    Invoke("Gewonnen", 2);
                }
                else
                {
                    Debug.Log("Konnte keine machineNew finden!");
                }
            }
            else if(hit.transform.CompareTag("Machine") && itemBeingDragged.transform.tag == "FalschCheat")
            {
                //Text: du kleiner Cheater
                //Game Over
            }
            else if(hit.transform.CompareTag("Machine") && itemBeingDragged.transform.tag == "Falsch")
            {
                //GameOver
                Machine = GameObject.Find("Crazy_Machine_Shatter");
                Machine.SetActive(false);
                machineNewParent = GameObject.Find("NewMachineParent");
                machineNew = machineNewParent.transform.GetChild(0).gameObject;
                if (machineNew != null)
                {
                    iconSprite = GameObject.Find("Gear_Icon");
                    machineNew.SetActive(true);
                    if (iconSprite != null)
                    {
                        iconSprite.SetActive(false);
                    }
                    Invoke("DestroyMachine", 2);
                }
                else
                {
                    Debug.Log("Konnte keine machineNew finden!");
                    Debug.Log(machineNew);
                }
            }
            if (hit.transform.CompareTag("Player") && itemBeingDragged.name.Contains("Labcoat"))
            {
                player = GameObject.Find("Player");
                mesh = player.transform.Find("LabCoat").gameObject;
                mesh.SetActive(true);
                Destroy(itemBeingDragged);
            }
            else if (hit.transform.CompareTag("Player") && itemBeingDragged.name.Contains("Glove"))
            {
                player = GameObject.Find("Player");
                mesh = player.transform.Find("Glove_Left").gameObject;
                mesh.SetActive(true);
                Destroy(itemBeingDragged);
            }
        }

        itemBeingDragged = null;
        draggingItem = false;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        transformDone = false;

        if (Slot.otherSlot == false || transform.parent.name.Equals("Canvas") || transform.parent == startParent) //!Inventory.activeSelf &&
        {
            transform.position = startPosition;
            transform.SetParent(startParent);
        }
        //else if (transform.parent != startParent)
        //{
        //    transform.position = startPosition;
        //    transform.SetParent(transform.parent);
        //}

    }

    void DestroyMachine()
    {
        desMachine = machineNew.GetComponent<DestroyMachine>();
        desMachine.DestroyMe();
    }

    void Gewonnen()
    {
        Debug.Log("void Gewonnen()");
        desMachine = machineNew.GetComponent<DestroyMachine>();
        desMachine.ShowGameWonScreen();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {

        //Debug.Log("Leaving now: Inventory");

    }
    #endregion
}