using System.Collections.Generic;
using UnityEngine;
using System.IO;

// 저장하는법
// 1. 저장할 데이터가 존재
// 2. 데이터를 제이슨으로 변환
// 3. 제이슨을 외부에 저장 -> using System.IO = Input output

// 불러오는법
// 1. 외부에 저장된 제이슨을 가져옴
// 2. 제이슨을 데이터형태로 변환
// 3. 불러온 데이터를 사용
public class PlayerData
{
    // 저장 할 데이터
    // 플레이어 대미지, 플레이어 위치, 저장이 된 씬, 인벤토리, 슬롯에 있는 아이템
    public int clearFloor = 1;                          // 클리어 층수
    public Vector3 PlayerPos = new Vector3(-13.0f, -2.87f, 14.0f);     // 1라운드 초기 플레이어 위치
    public int Scenenum = 1;                                // 씬 넘버
    public int LastGetKeyCode = 30000;
    public string[] inventoryItem = new string[8]{ "Empty", "Empty", "Empty", "Empty", "Empty", "Empty", "Empty", "Empty" };   // 인벤토리 슬롯
    public string[] useSlotItem = new string[5] { "Empty", "Empty", "Empty", "Empty", "Empty" };                                        //   사용   슬롯
    public int[] diaryslot = new int[9];
    public int bandageCount = 0;
    public List<int> GetItemCode = new List<int>();
    public List<int> KillEnemyCode = new List<int>();
    public List<int> StoryCode = new List<int>();
}
public class DataController : MonoBehaviour
{
    // 싱글 톤
    public static DataController instance;
    public PlayerData nowPlayer = new PlayerData();

    public string path;
    public int nowSlot;

    void Awake()
    {
        #region 싱글톤
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(instance.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        #endregion

        path = Application.persistentDataPath + "/killHighSchool"; // 파일 경로 "/"는 이름을 붙이기위함
    }

    public void SaveData()
    {
        string data = JsonUtility.ToJson(nowPlayer); // 파일변환
        File.WriteAllText(path + nowSlot.ToString(), data);// 경로, 컨텐츠(어떤것)
    }
    public void LoadData()
    {
       string data = File.ReadAllText(path + nowSlot.ToString()); // 파일 불러오기 (경로)
        nowPlayer = JsonUtility.FromJson<PlayerData>(data); // 파일변환 데이터값
    }
    public void DataClear()
    {
        nowSlot = -1;
        nowPlayer = new PlayerData();
    }
}
