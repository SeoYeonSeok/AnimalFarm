using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameMgr : MonoBehaviour
{
    [Header("UI")]
    public GameObject[] panels; // 0 - EggBG, 1 - Timer, 2 - InField
    public GameObject sideUIOnBtn;
    public GameObject sideUIOffBtn;

    [Header("Main Camera")]
    public Camera mainCam;
    public Transform[] mainCamPos;
    private int mainCamStatus = 0; // 0 - Egg, 1 - Farm, 2 - Field

    [Header("Eggs Elements")]
    private int currAnimal;
    public GameObject egg;
    public GameObject[] animals;
    public TextMeshProUGUI ment;
    public GameObject animal;

    [Header("Eggs & Animal's Animator Controller (Can Replaced to FEEL or Something Maybe?)")]
    public Animator animator_Eggs; // 0 - Idle, 1 - Shake
    public Animator[] animator_Animals;

    [Header("FarmMgr")]
    public FarmMgr farmMgr;
    public GameObject movableArea;

    private bool timeFlag = false;

    [Header("Feedbacks")]
    public MMFeedbacks hatchParticle;
    public MMFeedbacks hatchRotate;
    public MMFeedbacks fieldScale;
    public MMFeedbacks ui_SideFeedback_On;
    public MMFeedbacks ui_SideFeedback_Off;
    public MMFeedbacks ui_MissionFeedback_On;
    public MMFeedbacks ui_MissionFeedback_Off;
    public MMFeedbacks ui_ProfileFeedback_On;
    public MMFeedbacks ui_ProfileFeedback_Off;

    [Header("Focus Mode")]
    public Image focusColorImg;
    public TextMeshProUGUI focusModeText;
    public int currFocusMode = 0;

    [Header("Coin")]
    private int focusTimeMin = 10;
    public int currentCoin;
    public TextMeshProUGUI currentCoinText;

    private void Start()
    {
        egg.SetActive(true);
    }

    public void Debug_ChngTime()
    {
        if (timeFlag) Time.timeScale = 1;
        else Time.timeScale = 100;

        timeFlag = !timeFlag;
    }

    public void MoveMainCamera(int flag)
    {        
        DisableAllUI();        
        ResetCamTrns(flag);
        TurnUI(flag);

        RemadeEgg();
        ment.text = "Let's Hatch Some New Friends!";
    }

    public void ChngMainCamStatus(int status)
    {
        mainCamStatus = status;
    }

    public int GetMainCamStatus()
    {
        return mainCamStatus;
    }

    private void ResetCamTrns(int flag)
    {        
        mainCam.transform.position = mainCamPos[flag].position; // Move Camera's Position
        mainCam.transform.SetParent(mainCamPos[flag]);

        if (flag == 0)
        {
            mainCam.transform.rotation = Quaternion.Euler(0, 0, 0);
            farmMgr.ClearMountain();
            farmMgr.ClearField();
            SetCameraForEgg();
            ChngMainCamStatus(0);
            DisableAllCenter();
        }
        else if (flag == 1)
        {
            mainCam.transform.rotation = Quaternion.Euler(55f, 0, 0);
            farmMgr.AnimalFieldSpawn();            
            ChngMainCamStatus(1);
        }
        else if (flag == 2)
        {
            mainCam.transform.localRotation = Quaternion.identity;
            SetCameraForMountain();
            farmMgr.AnimalMountainSpawn();
            ChngMainCamStatus(2);
        }
    }

    private void DisableAllUI()
    {
        //for (int i = 0; i < panels.Length; i++) panels[i].SetActive(false);
        for (int i = 0; i < 4; i++) panels[i].SetActive(false);
    }

    private void TurnUI(int flag)
    {
        if (flag == 0) // Move 2 EggBG
        {
            panels[0].SetActive(true);
            panels[1].SetActive(true);
        }
        else if (flag == 1)
        {
            panels[2].SetActive(true);
        }
        else if (flag == 2)
        {
            panels[3].SetActive(true);
        }
    }

    public void MakeNewAnimal(int animalNum)
    {
        currAnimal = animalNum;
        EggChng(animalNum);                
    }

    public void EggChng(int animalNum)
    {
        egg.SetActive(false);

        if (animalNum == 0)
        {
            animals[0].SetActive(true);
        }
        else
        {
            animals[animalNum].SetActive(true);
            CatchNewAnimals();
        }
    }

    public void RemadeEgg()
    {
        animal.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        animal.transform.localRotation = Quaternion.Euler(0f, -80f, 0f);

        if (animals[currAnimal].activeSelf == true)
        {
            animals[currAnimal].SetActive(false);
            egg.SetActive(true);
        }
        hatchRotate.StopFeedbacks();
    }

    public void DoIdleEggs(bool status)
    {
        egg.transform.rotation = Quaternion.identity;
        animator_Eggs.SetBool("Idle", status);
    }

    public void DoShakeEggs(bool status)
    {
        animator_Eggs.SetBool("Shake", status);
    }

    public void CatchNewAnimals()
    {
        Debug.Log("Catch " + currAnimal);
        Time.timeScale = 1;

        farmMgr.CatchAnimal(currAnimal);
        hatchParticle.PlayFeedbacks();
        hatchRotate.PlayFeedbacks();

        UpdateCurrentCoin(focusTimeMin / 2, true);
    }

    public void DisableAllCenter()
    {
        int centerCount = movableArea.transform.childCount;

        for (int i = 0; i < centerCount; i++)
        {
            movableArea.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void SetCameraForMountain()
    {
        mainCam.orthographic = true;
        mainCam.orthographicSize = 10;
        mainCam.clearFlags = CameraClearFlags.Skybox;
    }

    private void SetCameraForEgg()
    {
        mainCam.orthographic = false;
        mainCam.fieldOfView = 60;
        mainCam.clearFlags = CameraClearFlags.SolidColor;
        mainCam.backgroundColor = new Color(0xEE / 255f, 0xF2 / 255f, 0xCE / 255f); // EEF2CE 색상 적용
    }

    public void FieldPosScaling()
    {
        fieldScale.PlayFeedbacks();
    }

    public void ChngFocusMode(int newMode)
    {
        currFocusMode = newMode;

        if (newMode == 0) {
            SetNewColor("#FFFFFF");
            focusModeText.text = "Study";
        }
        else if (newMode == 1) {
            SetNewColor("#FF4545");
            focusModeText.text = "Entertain";
        }
        else if (newMode == 2) {
            SetNewColor("#5A7CD6");
            focusModeText.text = "Sports";
        }
        else if (newMode == 3) {
            SetNewColor("#5EF561");
            focusModeText.text = "Sleep";
        }
        else if (newMode == 4) {
            SetNewColor("#D4BC29");
            focusModeText.text = "Whatever";
        }
    }

    private void SetNewColor(string colorCode)
    {
        Color newColor;
        if (ColorUtility.TryParseHtmlString(colorCode, out newColor)) focusColorImg.color = newColor;
    }

    public void SaveMinutes(int minTime)
    {
        focusTimeMin = minTime;
    }

    public void UpdateCurrentCoin(int value, bool plus)
    {
        if (plus == true) currentCoin += value;
        else currentCoin -= value;

        currentCoinText.text = currentCoin.ToString();
    }

    public void OnOffSideUI(bool flag)
    {
        if (flag == true)
        {
            ui_SideFeedback_On.PlayFeedbacks();
            sideUIOffBtn.SetActive(true);
        }
        else 
        { 
            ui_SideFeedback_Off.PlayFeedbacks();
            sideUIOffBtn.SetActive(false);
        }
    }

    public void OnOffMissionUI(bool flag)
    {
        if (flag == true)
        {
            ui_MissionFeedback_On.PlayFeedbacks();
            OnOffSideUI(false);
            sideUIOnBtn.SetActive(false);
        }
        else
        {
            ui_MissionFeedback_Off.PlayFeedbacks();
            sideUIOnBtn.SetActive(true);
            sideUIOffBtn.SetActive(false);
        }
    }

    public void OnOffProfileUI(bool flag)
    {
        if (flag == true)
        {
            ui_ProfileFeedback_On.PlayFeedbacks();
            OnOffSideUI(false);
            sideUIOnBtn.SetActive(false);
        }
        else
        {
            ui_ProfileFeedback_Off.PlayFeedbacks();
            sideUIOnBtn.SetActive(true);
            sideUIOffBtn.SetActive(false);
        }
    }
}
