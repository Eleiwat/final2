using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroy : MonoBehaviour
{
    [SerializeField] GameObject MyObject;


    void Start()
    {
        Destroy (MyObject, 1.5f);
    }

}
