using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class SetSettingsValues : MonoBehaviour
{
    // Start is called before the first frame update
    public int Group;
    private SettingsManager menuManager;
    void Awake()
    {
         menuManager = GameObject.Find("Settings").GetComponent<SettingsManager>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ValueChanged(Slider setting)
    {
        //Debug.Log(setting.value);
        //Debug.Log(menuManager);
        menuManager.tempSettings[Group, setting.GameObject().transform.GetSiblingIndex()] = setting.value;
    }
}
