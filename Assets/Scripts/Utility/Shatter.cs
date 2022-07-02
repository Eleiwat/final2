using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shatter : MonoBehaviour
{
    public GameObject destroyedVersion;

    public GameObject MyGreatObject;


    void OnTriggerEnter(Collider other)
    {
       if (other.CompareTag("Player"))
       {
          StartCoroutine(InstantiateAfterDelay(7f));
       }   
    }
        IEnumerator InstantiateAfterDelay(float delay)
    {
       yield return new WaitForSeconds(delay);
       Instantiate (destroyedVersion, transform.position, transform.rotation);
       Destroy (MyGreatObject);
    }
}



