
using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Objects.Abstracts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public float maxInteractDistance;
    public GameObject pauseMenu;
    public GameObject buildMenu;
    public GameObject inventory;
    private PlayerCharControler m_playerChar;
    private List<Item> m_Inventory = new List<Item>();
    [CanBeNull] private GameObject openMenu = null;//maby create a menu class OOP or enum
    void Start()
    {
        m_playerChar = GameObject.FindWithTag("Player").GetComponent<PlayerCharControler>();
    }

    public void SwitchScenes(int index)
    {
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
    Component GetInvComponent()
    {
        RaycastHit hit;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out hit, maxInteractDistance, LayerMask.GetMask("Buildings")))
        {
            var obj = hit.collider.gameObject.GetComponents(typeof(IInvInteractable));
            if (obj.Length > 0)
            {
                return obj[0];
            }
        }
        return null;
    }
    void Update()
    {
        //TODO move Input.GetKeyDown(Keys.KBuildMenu) to input manager and then disable inputmanager when esc is open
        if (Input.GetKeyDown(Keys.KEscMenu))
        {
            if (openMenu!=null)//if there is another menu open
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
            if (GetInvComponent() is IInvInteractable externalInv)
            {
                OpenInv(externalInv.GetItems());
            }
            else
            {
                OpenInv(m_Inventory);
            }
        }

    }

    void OpenInv(List<Item> items)
    {
        Debug.Log("Opening Inv");
    }

    
    
}
