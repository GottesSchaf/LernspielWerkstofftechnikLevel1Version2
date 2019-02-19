using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform playerToFollow;
    public static CameraFollow instance;
    public bool closeupInteraction = false;
    public Transform CloseupBack;
    public GameObject Camera;
    public GameObject player;

    private bool noClipEnabled;
    private bool inElevator;

    void Start()
    {
        instance = this;
    }
    void Update()
    {


        if (closeupInteraction == true)
        {
            CloseupBack.gameObject.SetActive(true);
            player.SetActive(false); //playerToFollow.GetComponent<SkinnedMeshRenderer>().enabled = false
            playerToFollow.Find("clothes_green").GetComponent<MeshRenderer>().enabled = false;
            Camera = GameObject.Find("Main Camera");

            if (Camera.transform.position.z < -6)
            {
                Vector3 newPosition = transform.position;
                newPosition = Camera.transform.position;
                newPosition.z += 1f;
                transform.position = newPosition;
            }
        }
        else if (noClipEnabled)
        {

            Vector3 newPosition = transform.position;
            newPosition.x = playerToFollow.position.x;
            newPosition.y = playerToFollow.position.y + 1f;
            newPosition.z = -7;
            transform.position = newPosition;

        }
        else if (playerToFollow && checkCameraViewportRight() && CheckCameraViewportLeft())
        {
            Vector3 newPosition = transform.position;
            newPosition.x = playerToFollow.position.x;
            newPosition.y = playerToFollow.position.y + 1f;
            newPosition.z = -7;

            if (inElevator)
            {
                newPosition.z = -4;
                newPosition.y = playerToFollow.position.y + 0.5f;
            }
            transform.position = newPosition;
        }
        else if (playerToFollow && !checkCameraViewportRight())
        {
            Vector3 newPosition = transform.position;
            newPosition.x = playerToFollow.position.x;

            if (newPosition.x > transform.position.x)
            {
                newPosition.x = transform.position.x;
            }

            newPosition.y = playerToFollow.position.y + 1f;
            newPosition.z = -7;

            if (inElevator)
            {
                newPosition.z = -4;
                newPosition.y = playerToFollow.position.y + 0.5f; ;
            }

            transform.position = newPosition;
        }
        else if (playerToFollow && !CheckCameraViewportLeft())
        {
            Vector3 newPosition = transform.position;
            newPosition.x = playerToFollow.position.x;

            if (newPosition.x < transform.position.x)
            {
                newPosition.x = transform.position.x;
            }
            newPosition.y = playerToFollow.position.y + 1f;
            newPosition.z = -7;

            if (inElevator)
            {
                newPosition.z = -4;
                newPosition.y = playerToFollow.position.y + 0.5f; ;
            }

            transform.position = newPosition;
        }
    }

    public bool checkCameraViewportRight()
    {
        float camNearClip = Camera.GetComponent<Camera>().nearClipPlane;
        float camFarClip = Camera.GetComponent<Camera>().farClipPlane;

        Camera cam = Camera.GetComponent<Camera>();

        Vector3 camNearClipUpRight = cam.ViewportToWorldPoint(new Vector3(1.0f, 1.0f, camNearClip));
        Vector3 camNearClipMiddleRight = cam.ViewportToWorldPoint(new Vector3(1.0f, 0.5f, camNearClip));
        Vector3 camNearClipDownRight = cam.ViewportToWorldPoint(new Vector3(1.0f, 0.0f, camNearClip));

        Vector3 camFarClipUpRight = cam.ViewportToWorldPoint(new Vector3(1.0f, 1.0f, camFarClip));
        Vector3 camFarClipMiddleRight = cam.ViewportToWorldPoint(new Vector3(1.0f, 0.5f, camFarClip));
        Vector3 camFarClipDownRight = cam.ViewportToWorldPoint(new Vector3(1.0f, 0.0f, camFarClip));


        Debug.DrawLine(cam.transform.position, camNearClipUpRight, Color.blue);
        Debug.DrawLine(cam.transform.position, camNearClipMiddleRight, Color.blue);
        Debug.DrawLine(cam.transform.position, camNearClipDownRight, Color.blue);

        Debug.DrawLine(camNearClipUpRight, camFarClipUpRight, Color.green);
        Debug.DrawLine(camNearClipMiddleRight, camFarClipMiddleRight, Color.green);
        Debug.DrawLine(camNearClipDownRight, camFarClipDownRight, Color.green);



        if (Physics.Linecast(camNearClipUpRight, camFarClipUpRight))
        {

        }
        else if (!Physics.Linecast(camNearClipUpRight, camFarClipUpRight))
        {
            return false;
        }

        if (Physics.Linecast(camNearClipMiddleRight, camFarClipMiddleRight))
        {

        }
        else if (!Physics.Linecast(camNearClipMiddleRight, camFarClipMiddleRight))
        {
            return false;
        }

        if (Physics.Linecast(camNearClipDownRight, camFarClipDownRight))
        {

        }
        else if (!Physics.Linecast(camNearClipDownRight, camFarClipDownRight))
        {
            return false;
        }
        return true;
    }

    private bool CheckCameraViewportLeft()
    {
        {
            float camNearClip = Camera.GetComponent<Camera>().nearClipPlane;
            float camFarClip = Camera.GetComponent<Camera>().farClipPlane;

            Camera cam = Camera.GetComponent<Camera>();

            Vector3 camFarClipUpLeft = cam.ViewportToWorldPoint(new Vector3(0.0f, 1.0f, camFarClip));
            Vector3 camFarClipMiddleLeft = cam.ViewportToWorldPoint(new Vector3(0.0f, 0.5f, camFarClip));
            Vector3 camFarClipDownLeft = cam.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, camFarClip));

            Vector3 camNearClipUpLeft = cam.ViewportToWorldPoint(new Vector3(0.0f, 1.0f, camNearClip));
            Vector3 camNearClipMiddleLeft = cam.ViewportToWorldPoint(new Vector3(0.0f, 0.5f, camNearClip));
            Vector3 camNearClipDownLeft = cam.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, camNearClip));


            Debug.DrawLine(cam.transform.position, camNearClipUpLeft, Color.blue);
            Debug.DrawLine(cam.transform.position, camNearClipMiddleLeft, Color.blue);
            Debug.DrawLine(cam.transform.position, camNearClipDownLeft, Color.blue);

            Debug.DrawLine(camNearClipUpLeft, camFarClipUpLeft, Color.green);
            Debug.DrawLine(camNearClipMiddleLeft, camFarClipMiddleLeft, Color.green);
            Debug.DrawLine(camNearClipDownLeft, camFarClipDownLeft, Color.green);

            if (Physics.Linecast(camNearClipUpLeft, camFarClipUpLeft))
            {

            }
            else if (!Physics.Linecast(camNearClipUpLeft, camFarClipUpLeft))
            {
                return false;
            }

            if (Physics.Linecast(camNearClipMiddleLeft, camFarClipMiddleLeft))
            {

            }
            else if (!Physics.Linecast(camNearClipMiddleLeft, camFarClipMiddleLeft))
            {
                return false;
            }

            if (Physics.Linecast(camNearClipDownLeft, camFarClipDownLeft))
            {

            }
            else if (!Physics.Linecast(camNearClipDownLeft, camFarClipDownLeft))
            {
                return false;
            }
            return true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter No Clip");
        if (other.tag == "NoClip")
        {
            noClipEnabled = true;
        }
        if (other.name == "Elevator")
        {
            inElevator = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Exit No Clip");
        if (other.tag == "NoClip")
        {
            noClipEnabled = false;
        }
        if (other.name == "Elevator")
        {
            inElevator = false;
        }
    }
}