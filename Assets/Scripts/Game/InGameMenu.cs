using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InGameMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> Menus;
    private int currentMenu;
    public List<GameObject> MenuButtons;
    Color normalColor = Color.clear;
    Color selectedColor = new Color(1f, 0.8f, 0f, 0.8f);
    void Start()
    {
        currentMenu = 0;
        SelectGroup(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectGroup(int index)
    {
        Menus[currentMenu].SetActive(false);
        MenuButtons[currentMenu].GetComponent<UnityEngine.UI.Image>().color = normalColor;
        currentMenu = index;
        Menus[currentMenu].SetActive(true);
        MenuButtons[currentMenu].GetComponent<UnityEngine.UI.Image>().color = selectedColor;
    }
}
