using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorExitEvent : MonoBehaviour
{
    public void DoorExit()
    {
        transform.parent.GetComponent<Door>().ChangeCurrentRoom();
    }
}
