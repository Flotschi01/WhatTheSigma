using System;
using System.Collections;
using System.Collections.Generic;
using Objects;
using UnityEngine;
using Objects.Abstracts;

public class BuildingPlaceable : MonoBehaviour
{
    // Start is called before the first frame update
    public List<ResourceNode> ResourceNodes = new List<ResourceNode>();
    public List<GameObject> buildingOutputNodes = new List<GameObject>();
    public static Color yes = new Color(0.0f, 0.3f, 1f, 0.5f);//FF1300
    public static Color no = new Color(1f, 0.2f, 0f, 0.65f);//FF1300
    private PlacePreview place;
    //doesnt really work because of ridgig body
    void Start()
    { 
        place = GameObject.FindWithTag("Player").GetComponent<PlacePreview>();
        place.PreviewMaterial.color = yes;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        place.PreviewMaterial.color = no;
    }

    private void OnTriggerExit(Collider other)
    {
        place.PreviewMaterial.color = yes;
    }
}
