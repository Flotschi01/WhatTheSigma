using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameMenuButton : MonoBehaviour
{
    // Start is called before the first frame update
    
    PlacePreview m_placePreview;
    InGameManager m_InGameManager;
    void Start()
    {
        m_placePreview = GameObject.FindWithTag("Player").GetComponent<PlacePreview>(); 
        m_InGameManager = GameObject.Find("EventSystem").GetComponent<InGameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
/// <summary>
/// tries to load the prefab corresponding to the string gameObject name
/// </summary>
    public void OnBuildingSelect(string name)
    {
        m_InGameManager.CloseOpenMenu();
        m_placePreview.LoadPlacePrefab(name.ToPlaceType());
    }
}
