using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPortalTwo : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (TriggerPortal.onStay == false)
            {
                TriggerPortal.onStay = true;
                other.gameObject.transform.SetPositionAndRotation(new Vector3(2.737066f, 2.16f, -5.790509f), Quaternion.Euler(-90f, 0f, 0f));
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        TriggerPortal.onStay = false;
    }
}
