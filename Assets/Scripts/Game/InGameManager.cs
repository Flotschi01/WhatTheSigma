
using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Objects.Abstracts;
using Player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject pauseMenu;
    public GameObject buildMenu;
    public GameObject inventory;
    private PlayerCharControler m_playerChar;
    private InventoryManager m_InvManager;
    
    [CanBeNull] private GameObject openMenu = null;//maby create a menu class OOP or enum
    void Start()
    {
        m_playerChar = GameObject.FindWithTag("Player").GetComponent<PlayerCharControler>();
        m_InvManager = GameObject.FindWithTag("Player").GetComponent<InventoryManager>();
    }

    public void SwitchScenes(int index)
    {
        if (openMenu != null)
        {
            ToggleMenu(openMenu);
        }
        SceneManager.LoadScene(index);
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    public void CloseOpenMenu()
    {
        if (openMenu != null)
        {
            ToggleMenu(openMenu);
        }
    }
    public void ToggleMenu(GameObject menu)
    {
        menu.SetActive(!menu.activeSelf);//toggle gameObject
        Cursor.lockState = menu.activeSelf ? CursorLockMode.None : CursorLockMode.Locked; // unlock / lock cursor
        //(CursorLockMode)Convert.ToInt32(!menu.activeSelf);mit casting hah afunny
        Time.timeScale = menu.activeSelf ? 0 : 1;//stop/start time
        m_playerChar.enabled = !menu.activeSelf;//stop/start the movement script
        openMenu = menu.activeSelf ? menu : null; // set yourself as active/open menu
        
    }

    [CanBeNull]

    void Update()
    {
        //TODO move Input.GetKeyDown(Keys.KBuildMenu) to input manager and then disable inputmanager when esc is open
        if (Input.GetKeyDown(Keys.KEscMenu))
        {
            if (openMenu)//if there is another menu open
            {
                ToggleMenu(openMenu);//close the menu
                openMenu = null;
            }
            else
            {
                ToggleMenu(pauseMenu);//toggle the ecs menu
            }
        }
        else if (Input.GetKeyDown(Keys.KBuildMenu))
        {
            ToggleMenu(buildMenu);
        }
        else if (Input.GetKeyDown(Keys.KInventory))
        {
            ToggleMenu(inventory);
            m_InvManager.enabled = !m_InvManager.enabled;
            
            m_InvManager.OnInvOpen();
        }

    }

    

    
    
}
