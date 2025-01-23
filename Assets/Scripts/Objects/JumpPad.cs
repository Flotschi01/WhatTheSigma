using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    // Start is called before the first frame update
    MeshCollider mCollider;
    void Start()
    {
        mCollider = GetComponent<MeshCollider>();
        mCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        throw new NotImplementedException();
    }
}
