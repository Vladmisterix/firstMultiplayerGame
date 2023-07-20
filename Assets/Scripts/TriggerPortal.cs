using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPortal : MonoBehaviour
{
    public static bool onStay = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(onStay == false)
            {
                onStay = true;
                other.gameObject.transform.SetPositionAndRotation(new Vector3(56f, 2.16f, -5f), Quaternion.Euler(-90f, 0f, 0f));
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        onStay = false;
    }
}
