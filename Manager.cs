using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System;

public class Manager : MonoBehaviour
{
    public GameObject mainButton;
    public GameObject Scroll;

    public GameObject loadSlot;
    public GameObject checkLoad;
    public Text[] slotText;

    int datanumber;
    bool[] savefile = new bool[3];
    void Start()
    {
        // 슬롯별로 저장된 데이터가 존재하는지 판단
        for (int i = 0; i < 3; i++)
        {
            if (File.Exists(DataController.instance.path + $"{i}"))
            {
                // 데이터가 있다면 슬롯에 표시
                savefile[i] = true;
                DataController.instance.nowSlot = i;// 불러올 데이터 넘버
                DataController.instance.LoadData(); // 데이터 로드
                // 텍스트 설정
                slotText[i].text = "현재 층 : " + DataController.instance.nowPlayer.clearFloor.ToString() + "층"
                    + System.Environment.NewLine + DateTime.Now.ToString(("yyyy-MM-dd-HH:mm:ss"));
            }
            else
            {
                slotText[i].text = "비어있음";
            }
        }
        DataController.instance.DataClear();
    }
    public void Playbtn()
    {
        loadSlot.gameObject.SetActive(!loadSlot.activeSelf);
        checkLoad.gameObject.SetActive(false);
    }
    public void Settingbtn()
    {
        Scroll.SetActive(!Scroll.activeSelf);
        mainButton.SetActive(!mainButton.activeSelf);
    }
    public void CheckLoadBtn()      // 선택된 슬롯 로드 및 생성
    {
        DataController.instance.nowSlot = datanumber; 
        // 저장된 데이터가 있을때
        if (savefile[datanumber])
        {
            DataController.instance.LoadData();
        }
        // 저장된 데이터가 없을때
        else if (!savefile[datanumber])
        {
            slotText[datanumber].text = "현재 층 : 1층" + System.Environment.NewLine + DateTime.Now.ToString(("yyyy-MM-dd-HH:mm:ss"));
            DataController.instance.SaveData();
        }
        GoGameScene();
    }
    public void OnSlotBtn(int num)  // 선택된 슬롯 넘버 및 실행여부 창 띄우기
    {
        if(savefile[num])
        {
            checkLoad.transform.GetChild(0).GetComponent<Text>().text = "선택 슬롯의 저장파일을 불러오기";
        }
        else if(!savefile[num])
        {
            checkLoad.transform.GetChild(0).GetComponent<Text>().text = "선택 슬롯에 새로운 게임 생성";
        }
        datanumber = num;
        checkLoad.gameObject.SetActive(true);
    }
    public void OffSlotBtn()        // 선택된 슬롯 넘버 및 실행여부 창 끄기
    {
        datanumber = -1;
        checkLoad.gameObject.SetActive(false);
    }
    public void GoGameScene()
    {
        SceneManager.LoadScene(DataController.instance.nowPlayer.Scenenum);
    }
    public void EixtGameBtn()
    {
        Application.Quit();
    }
}