using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHandler : MonoBehaviour
{
    private Animator animator = null;
    public bool inTrigger = false;
	// Use this for initialization
	void Start ()
    {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        // if (Input.GetKeyDown(KeyCode.F) && inTrigger)
        if (Input.GetMouseButtonDown(1) && inTrigger)
        {
            animator.SetBool("isopen", !animator.GetBool("isopen"));
        }
        else if(!inTrigger)
        {
            animator.SetBool("isopen", false);          
        }
    }
}
