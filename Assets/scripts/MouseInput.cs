using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MouseInput : MonoBehaviour
{

    NavMeshAgent playerAgent;
    Ray ray;
    RaycastHit hit;
    public string RayHitsThis;
    [SerializeField] GameObject laptop, bunsenBrenner, gussformWrench, gussformPleuel, gussformZahnrad, datenblatt, laborkittelError, tiegelBeschriftung, verbrannt, ofen, tiegelInfo;

    void Start()
    {
        playerAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            RayHitsThis = hit.collider.name;
        }

        if (Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() && CameraFollow.instance.closeupInteraction == false)
        {
            GetInput();
        }
        else if (Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() && CameraFollow.instance.closeupInteraction == true && RayHitsThis == "Book")
        {
            GetInput();
        }
    }

    void GetInput()
    {
        Ray interactionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit interactionInfo;
        if (Physics.Raycast(interactionRay, out interactionInfo, Mathf.Infinity))
        {
            if (laptop.activeInHierarchy == false && bunsenBrenner.activeInHierarchy == false && gussformPleuel.activeInHierarchy == false && gussformWrench.activeInHierarchy == false && gussformZahnrad.activeInHierarchy == false && datenblatt.activeInHierarchy == false && laborkittelError.activeInHierarchy == false && tiegelBeschriftung.activeInHierarchy == false && verbrannt.activeInHierarchy == false && ofen.activeInHierarchy == false && tiegelInfo.activeInHierarchy == false) {
                GameObject interactiveObject = interactionInfo.collider.gameObject;
                if (interactiveObject.tag == "Interactive")
                {
                    interactiveObject.GetComponent<Interactive>().MoveToInteraction(playerAgent);
                }
                if (interactiveObject.tag == "Collectible")
                {
                    interactiveObject.GetComponent<Collectible>().MoveToCollectible(playerAgent);
                }
                else
                {
                    playerAgent.destination = interactionInfo.point;
                    this.gameObject.transform.LookAt(new Vector3(interactionInfo.point.x, transform.position.y, interactionInfo.point.z));
                }
            }
        }

    }
}