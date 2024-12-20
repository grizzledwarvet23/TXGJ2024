using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSwitch : MonoBehaviour
{
    public GameObject activatedDoor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ActivateDoor()
    {
        activatedDoor.SetActive(false);
    }



}
