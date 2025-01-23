using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public static class Keys
{
    public static KeyCode KJump { get; set; }= KeyCode.Space;//private ?
    public static KeyCode KForward { get; set; }= KeyCode.W;
    public static KeyCode KBackward { get; set; }= KeyCode.S;
    public static KeyCode KLeft { get; set; }= KeyCode.A;
    public static KeyCode KRight { get; set; }= KeyCode.D;
    public static KeyCode KCrouch {get; set; }= KeyCode.LeftControl;
    public static KeyCode KToggleCrouch {get; set; }= KeyCode.C;
    public static KeyCode KSprint {get; set; }= KeyCode.LeftShift;
    public static KeyCode KToggleSprint {get; set; }= KeyCode.RightControl;
    public static KeyCode KEscMenu { get; set; }= KeyCode.Escape;
    public static KeyCode KBuildMenu { get; set; }= KeyCode.Q;
    public static KeyCode KInventory { get; set; }= KeyCode.E;
    public static int BMouseLeft { get; set; }= 0;

}
public class SettingsManager : MonoBehaviour
{
    private int indexLastActive = 0;
    public GameObject[] tabs;
    public float[,] tempSettings { get; set; }= new float[3,1];
    // Start is called before the first frame update
    void Awake()
    {
        if (!PlayerPrefs.HasKey("AutoSaveInterval"))
        {
            PlayerPrefs.SetFloat("AutoSaveInterval", 0f);//0 for off
        }
        if (!PlayerPrefs.HasKey("Volume"))
        {
            PlayerPrefs.SetFloat("Volume", 0.5f);
        }
        if (!PlayerPrefs.HasKey("Sensitivity"))
        {
            PlayerPrefs.SetFloat("Sensitivity", 0.5f);
        }
        tempSettings[0, 0] = PlayerPrefs.GetFloat("AutoSaveInterval");
        tempSettings[1, 0] = PlayerPrefs.GetFloat("Volume");
        tempSettings[2, 0] = PlayerPrefs.GetFloat("Sensitivity");
        LoadGeneral();
        PlayerPrefs.Save();
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    public void LoadGeneral()
    {
        ResetActiveTab(0);
        GameObject.Find("General").GetComponentInChildren<Slider>().value = tempSettings[0, 0];
    }
    public void LoadAudio()
    {
        ResetActiveTab(1);
        GameObject.Find("Audio").GetComponentInChildren<Slider>().value = tempSettings[1,0];
    }
    public void LoadControls()
    {
        ResetActiveTab(2);
        GameObject.Find("Controls").GetComponentInChildren<Slider>().value = tempSettings[2, 0];
        //import the othere settings 
    }

    private void ResetActiveTab(int indexNewActive)
    {
        tabs[indexLastActive].SetActive(false);
        tabs[indexNewActive].SetActive(true);
        indexLastActive = indexNewActive;
    }

    
    public void SaveOptions()
    {
        //Code for saving settings to PlayerPrefs;
        PlayerPrefs.SetFloat("AutoSaveInterval", tempSettings[0,0]);
        PlayerPrefs.SetFloat("Volume", tempSettings[1,0]);
        PlayerPrefs.SetFloat("Sensitivity", tempSettings[2,0]);
        //Debug.Log("Auto Save Interval: " + PlayerPrefs.GetFloat("AutoSaveInterval"));
        //Debug.Log("Volume : " + PlayerPrefs.GetFloat("Volume"));
        //Debug.Log("Sensitivity : " + PlayerPrefs.GetFloat("Sensitivity"));
        PlayerPrefs.Save();
        
    }
}
