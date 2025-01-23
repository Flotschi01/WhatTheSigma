using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    // Start is called before the first frame update
    PlayerCharControler m_CharControler;
    void Start()
    {
        m_CharControler = GetComponent<PlayerCharControler>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnControllerColliderHit(ControllerColliderHit col)
    {
        if (col.gameObject.CompareTag("JumpPad"))
        {
            m_CharControler.m_ExternalVelocity += Vector3.up;
            Debug.Log(col.gameObject.name);
        }
    }
}
