using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Button = UnityEngine.UIElements.Button;
namespace Unity.Scripts.Game
{
    public class MenuManager : MonoBehaviour
    {
        // Start is called before the first frame update
        public GameObject settings;
        public GameObject GUI;
        void Start()
        { ;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(0);
            }
        }

        public void SwitchScenes(int index)
        {
            SceneManager.LoadScene(index);
        }

        public void GotoSettings()
        {
            
            GUI.SetActive(!GUI.activeSelf);
            settings.SetActive(!settings.activeSelf);

        }
        public void ExitGame()
        {
            Application.Quit();
        }
        public void LoadGameFromFile()
        {
            string path = Application.persistentDataPath;
            //EditorUtility.OpenFilePanel("Open Game", path, "");   
        }
        //settings section:

    }
}
