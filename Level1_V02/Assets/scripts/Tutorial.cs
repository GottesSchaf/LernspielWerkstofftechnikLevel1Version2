using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Tutorial : MonoBehaviour
{
    //Player
    [SerializeField] private GameObject Player;
    [SerializeField] private NavMeshAgent agent;

    //Sprites
    [SerializeField] private SpriteRenderer spriteToChange;
    [SerializeField] private Sprite[] tutSprites;
    [SerializeField] private GameObject target;

    //Cameras
    [SerializeField] private Camera tutCam;
    [SerializeField] private Camera playerCam;

    //Objects in Tut.Room
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject doorframe;
    [SerializeField] private GameObject cube;

    //Canvas Buttons
    [SerializeField] private GameObject invOpen;
    [SerializeField] private GameObject invMenu;
    [SerializeField] private GameObject[] invSlots;
    [SerializeField] private GameObject Speed1;
    [SerializeField] private GameObject Speed2;
    [SerializeField] private GameObject Questwindow;

    [SerializeField] private GameObject pauseScreen;

    [SerializeField] private GameObject[] Buttons;

    [SerializeField] private NavMeshSurface surface;
    // Playerposition
    public Vector3 destination;

    public int slot = -1;

    private bool step2Done;
    private bool step3Done;
    public static bool step4Done;
    private bool step5Done;
    private bool step6Done;
    private bool step7Done;
    /*
     1. Willkommen Screen 
     2. > Weiter drücken auf Button (schon drinne) 
     3. "Willst du eine Einweisung?" - Screen 
     4. Aufploppen von zwei Buttons -> JA / NEIN 
     4.1. WENN JA: Laufen Screen 
     4.2. WENN NEIN: Starte Spiel 
     5. Laufen Screen -> Roter Kreis (target) aktiviert sich 
     -> Spieler muss reinlaufen 
     6. Wenn Spieler drinne: Pick Up Screen 
     -> Tisch mit Cube taucht auf 
     -> Spieler muss Cube anklicken (aufheben, script ist auch drinne) 
     7. Wenn Cube aufgehoben: Inventar Screen (v.w. das ist dein Inventar)
     8. Drag and Drop Screen (noch nicht drin als Sprite) 
     9. Pausenscreen (wenn P gedrückt wurde, dann weiter zu 10.) 
     10. Das sind deine Buttons (CANVAS BUTTONS) Screen (SPRITE) 
     11. Tür ploppt auf 
     -> SPieler geht raus und beendet Tutorial und "QUESTWINDOW" öffnet sich
    */

    private void Awake()
    {
        step2Done = false; // Walking
        step3Done = false; // Pick Up
        step4Done = false; // Open Inventory
        step5Done = false; // Pausescreen
        step6Done = false; // Buttons
        step7Done = false; // Door
    }

    private void Update()
    {
        if(Player.GetComponent<CheckCollision>().HitTarget && !step2Done) // Laufen Screen 
        {
            spriteToChange.sprite = tutSprites[2];
            target.SetActive(false);
            step2Done = true;
            cube.SetActive(true);
        }

        if(step2Done && !step3Done) // Pick Up Screen
        {
            slot = -1;
            foreach(GameObject x in invSlots)
            { 
                slot++;
                if (x.GetComponentInChildren<DragHandeler>())
                {
                    step3Done = true;
                    spriteToChange.sprite = tutSprites[3];
                    break;
                }
            }
        }

        if(step2Done && step3Done && !step4Done)
        {
            CollisionDetection.itemInInventory = true;
            spriteToChange.sprite = tutSprites[4]; // In der Tasche sind deine Objekte (Bild)
            invOpen.SetActive(true);               // Icon für Inventar taucht auf
            foreach (GameObject x in invSlots)
            {
                if (x.GetComponentInChildren<DragHandeler>())
                {
                    if(x == invSlots[slot])
                    {
                     
                    }
                    else
                    {
                        invMenu.SetActive(false);
                        invOpen.SetActive(true);
                        Destroy(x.GetComponentInChildren<DragHandeler>().gameObject);
                        step4Done = true;
                    }
                    break;
                }
            }
        }
        if (step2Done && step3Done && step4Done && !step5Done)
        {
            CollisionDetection.itemInInventory = false;
            spriteToChange.sprite = tutSprites[5];
            if (pauseScreen.activeSelf)   
            {
                spriteToChange.sprite = tutSprites[6];
                step5Done = true;           
                Speed1.SetActive(true);
                Speed2.SetActive(true);
            }
        }      
        
        if(step2Done && step3Done && step4Done && step5Done && step6Done && !step7Done)
        {
            spriteToChange.sprite = tutSprites[7];
            door.SetActive(true);
            doorframe.SetActive(true);
            Time.timeScale = 1;
            step7Done = true;
        }

    }

    public void Help()
    {
        step6Done = true;
    }

    // Change Sprite
    public void Forward()
    {
        spriteToChange.sprite = tutSprites[0];
        Buttons[0].SetActive(false);
        Buttons[1].SetActive(true);
        Buttons[2].SetActive(true);
    }

    // Button "Ja" pressed
    public void Yes()
    {
        tutCam.enabled = false;
        playerCam.enabled = true;
        spriteToChange.sprite = tutSprites[1];
        Buttons[1].SetActive(false);
        Buttons[2].SetActive(false);
        target.SetActive(true);
    }

    // Button "Nein" pressed
    // Spiel wird gestartet
    public void No()
    {
        tutCam.enabled = false;
        playerCam.enabled = true;
        Buttons[1].SetActive(false);
        Buttons[2].SetActive(false);
        agent.Warp(destination); // teleports player into hallway
        Questwindow.SetActive(true);
    }

    public void Understood()
    {
        invOpen.SetActive(true);
        Speed1.SetActive(true);
        Speed2.SetActive(true);
    }
}
