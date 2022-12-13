using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public GameObject threshPanel;

    public static GameObject Player;
    public static GameObject Cam;
    public Weapon weaponScripts;

    public GameObject menu;             // 메인화면
    public GameObject memory;           // 기록
    public GameObject Inventory;        // 가방
    public GameObject setting;          // 설정
    public GameObject flsh;             // 손전등
    public GameObject flshObj;          // 라이트
    public GameObject pushMessage;
    public static bool isReadStroy;     // 스토리 진행중인지 확인
    public GameObject[] dirayObj;       // 일기 오브젝트
    public GameObject dirayImage;       // 일기 디테일
    public Text bandageCount;
    public GameObject soundSetting;

    public Sprite[] flshImage = new Sprite[2];
    GameObject phonePanel;              // 폰
    public SoundManager soundManager;
    public SoundManager buttonSound;

    private void Awake()
    { 
        isReadStroy = false;
        phonePanel = Inventory.transform.parent.gameObject;
        Cam = GameObject.Find("Main Camera");
        Player = GameObject.Find("Player");
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        buttonSound = transform.GetComponent<SoundManager>();
        buttonSound.bgmPlayer = GameObject.Find("Around Effect Sound").GetComponent<AudioSource>();
        flshObj = Player.transform.GetChild(3).GetChild(2).gameObject;
        weaponScripts = Player.GetComponent<Weapon>();
        soundManager.BGMSoundPlay(1);
    }
    private void LateUpdate()
    {
        bandageCount.text = weaponScripts.bandageNum.ToString();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!isReadStroy){
                OffDiary();
                phonePanel.SetActive(!phonePanel.activeSelf);
                memory.SetActive(false);
                Inventory.SetActive(false);
                setting.SetActive(false);
                soundSetting.SetActive(false);

                buttonSound.ZombiOnce(5);
                menu.SetActive(true);
                // 일시정지 기능
                if (Time.timeScale.Equals(1))
                {
                    Time.timeScale = 0;
                }
                // 일시정지 해제
                else if (Time.timeScale.Equals(0))
                {
                    Time.timeScale = 1;
                }
            } 
        }
        if(memory.activeSelf)
        {            
            Color color = new Color(0f, 0f, 0f, 0f);
            for (int i = 0; i < DataController.instance.nowPlayer.diaryslot.Length; i++)
            {
                if (DataController.instance.nowPlayer.diaryslot[i] != 0)
                {
                    color.a = 1.0f;
                    dirayObj[i].transform.GetComponent<Button>().enabled = true;
                    dirayObj[i].transform.GetChild(0).GetComponent<Text>().color = color;
                }
                else if (DataController.instance.nowPlayer.diaryslot[i].Equals(0))
                {
                    color.a = 0.5f;
                    dirayObj[i].transform.GetComponent<Button>().enabled = false;
                    dirayObj[i].transform.GetChild(0).GetComponent<Text>().color = color;
                }
            }
        }
        if (PlayerMove.currentHp <= 0)
        {
            if (soundManager.ConditionMusic("Die").Equals(false))
            {
                soundManager.BGMSoundPlay(2);
            }
        }
    }
    public void SaveGame() // 게임 저장
    {
        for (int i = 0; i < Weapon.item.Length; i++)
        {
            DataController.instance.nowPlayer.inventoryItem[i] = Weapon.item[i].name;
        }
        for (int i = 0; i < Weapon.use.Length; i++)
        {
            DataController.instance.nowPlayer.useSlotItem[i] = Weapon.use[i].name;
        }
        DataController.instance.nowPlayer.PlayerPos = weaponScripts.gameObject.transform.position;
        DataController.instance.nowPlayer.bandageCount = weaponScripts.bandageNum;
        DataController.instance.nowPlayer.Scenenum = SceneManager.GetActiveScene().buildIndex;
        DataController.instance.SaveData();
        StartCoroutine(PushMessage());
    }
    public void SoundSettingToggle()
    {
        memory.SetActive(false);
        Inventory.SetActive(false);
        setting.SetActive(false);
        menu.SetActive(false);

        soundSetting.SetActive(true);
    }
    public void ExitGameBtn()
    {
        setting.SetActive(false);
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    public void MainBtn()
    {
        memory.SetActive(false);
        Inventory.SetActive(false);
        setting.SetActive(false);
        soundSetting.SetActive(false);

        buttonSound.ZombiOnce(0);
        menu.SetActive(true);
    }
    public void SettingBtn()
    {
        menu.SetActive(false);
        memory.SetActive(false);
        Inventory.SetActive(false);
        soundSetting.SetActive(false);

        setting.SetActive(true);
    }
    public void MemoryBtn()
    {
        menu.SetActive(false);
        setting.SetActive(false);
        Inventory.SetActive(false);
        soundSetting.SetActive(false);

        buttonSound.ZombiOnce(4);
        memory.SetActive(true);
    }
    public void IventoryBtn()
    {
        menu.SetActive(false);
        memory.SetActive(false);
        setting.SetActive(false);
        soundSetting.SetActive(false);

        buttonSound.ZombiOnce(3);
        Inventory.SetActive(true);
    }
    public void FlshBtn()
    {
        if (flsh.transform.GetComponent<Image>().sprite.Equals(flshImage[0]))
        {
            flsh.transform.GetComponent<Image>().sprite =  flshImage[1];
            buttonSound.ZombiOnce(1);
            flshObj.SetActive(true);
        }
        else
        {
            flsh.transform.GetComponent<Image>().sprite = flshImage[0];
            buttonSound.ZombiOnce(2);
            flshObj.SetActive(false);
        }
    }
    public void DiaryBtn(int diraynum)
    {
        dirayImage.SetActive(true);
        string day = "";
        string detail = "";
        switch (diraynum)
        {
            case 0:
                day = "2014년 3월 2일";
                detail = "내 이름은 김소연 자존심이라고는..." +
                    "\n 찾아볼수없다. 그래서 일기를 항상 쓴다."+
                    "\n 나는 가난하다 라는 이유로" +
                    "\n 중학교에서 온갖 차별과 비판을 받았고" +
                    "\n 고등학교는 먼곳으로 나왔고 적극적으로" +
                    "\n 말을 걸어서\"강서진\"이라는 친구를 만났다." +
                    "\n 굉장히 아름다웠고 수석입학이라고 한다." +
                    "\n 놓치고 싶지않아서 끈질기게 말을 했다." +
                    "\n 내일도 말을 열심히 걸어야 겠다..";
                break;
            case 1:
                day = "2014년 3월 21일";
                detail = "하루도 빠짐 없이 말을 계속 걸었지만" +
                    "\n 돌아오는것은 무시밖에 없었다." +
                    "\n 정말로 친구가 될수있기는 할까..?" +
                    "\n 점점 그녀가 미워진다.. 나는 단지.." +
                    "\n 친해지고 싶을 뿐인데.. 다른 동급생들은" +
                    "\n 무리를 만들었다..중간에 끼는 일은 힘들다." +
                    "\n 조금 화가나지만 아직 이친구를 더 지켜봐야" +
                    "\n 알수있을거 같다. 그래도 내치지는 않으니..후.";
                break;
            case 2:
                day = "2014년 4월 2일";
                detail = "드디어 말에 대꾸를 해줬다!" +
                    "\n 이제 마음을 조금씩 열었을지도??" +
                    "\n 나는 들뜬 마음으로 더욱 다가갔다." +
                    "\n 강서진의 표정이 매우 당황하듯이" +
                    "\n 보였다.. 조금 무리를 했을지도.." +
                    "\n 기뻐서 그만,, 그래도 오늘은 기념적인" +
                    "\n 날이다. 처음으로 나에게 호전적으로 " +
                    "\n 말을 걸어준 친구가 생겼기에 너무 기쁘다." +
                    "\n 내일도 재밌는 이야기를 많이 준비해야지" +
                    "\n 사물함에 자물쇠를 해야하니 가져가야지";
                break;
            case 3:
                day = "2014년 4월 26일";
                detail = "요즘 친해진줄 알고 이것저것 막 말하고있다." +
                    "\n 근데 반응도 별로이고 그냥 대답해주는게 " +
                    "\n 기분이 매우 나쁘다. 왜 저렇게 부정적인지.." +
                    "\n 참.. 알수가없네.. 좀 다정다감하게" +
                    "\n 대답을 해주면 뭐가 나쁜가?" +
                    "\n 그래도 첫 친구에 배부를수는 없지" +
                    "\n 이야기를 하고 이야기를 들어줄 친구가" +
                    "\n 있다는게..내일은 밖에서 놀아보기로 했다.";
                break;
            case 4:
                day = "2014년 5월 4일";
                detail = "내일은 어린이 날이다" +
                    "\n 서진이랑 꽤 친하게 지내고 있다." +
                    "\n 오늘은 쉬는 날임에도 불구하고" +
                    "\n 쇼핑이랑 오락실을 가기로 했다." +
                    "\n 서진이는 오락실을 모르는 표정이였는데" +
                    "\n 어떻게 사람이 오락실을 모르냐면서 " +
                    "\n 끌고 가기로했다. 정말 어떤 삶을 살아온건지ㅎㅎ" +
                    "\n 오늘 본때를 보여주기로 했다. 내가 가난하지만" +
                    "\n 오락을 관둔적은 단 하루도 없었다!" +
                    "\n 공부와 운동 이외에 내가 이기는 것을 보여줄거다. ";
                break;
            case 5:
                day = "2014년 5월 8일";
                detail = "오늘은 어버이날이다. 어머니와 아버지가 싸운다." +
                    "\n 이유는 돈 문제이다. 아버지가 다니시던 공장에서" +
                    "\n 정리해고를 당했나보다. 그 일로 어머니와 싸우고" +
                    "\n 나는 이어폰을 쓰고 지금 일기를 쓰고 있다." +
                    "\n 지긋지긋하다 이제 돈문제로 벗어나고싶다." +
                    "\n 나도 좋은곳 좋은거 먹으면서 공부하고 싶다." +
                    "\n 서진이를 만나러 갈때도 신경을 굉장히쓴다." +
                    "\n 서진이가 입었던옷 비싸보여서.." +
                    "\n 살짝 뒷조사를 해봤다.. 재벌이였다.." +
                    "\n 나를 도와달라고 할까..? 아니면.." +
                    "\n 나만 원룸같은곳에 살게 해달라고 빌까.." +
                    "\n 고민 많이 했다.. 부럽다 재벌..";
                break;
            case 6:
                day = "2014년 5월 13일";
                detail = "오늘은 너무 힘든 날이다..." +
                    "\n 학교 사람들이 내 중학교 모습과 사정을 알았다." +
                    "\n 더이상 나에게 말도 안걸고 괴롭히기 시작했다.." +
                    "\n 치사하게 서진이가 안볼때만 괴롭힌다. 찍히기는" +
                    "\n 싫은가보다 진짜 서진이랑 친하게 지내고 싶으면" +
                    "\n 자신이 있게 이야기 하던가.. 오늘은 서진이랑" +
                    "\n 만나지 못했다. 옷이 너무 더러워졌기 때문이다." +
                    "\n 내일은 빨리 만나야지";
                break;
            case 7:
                day = "2014년 5월 20일";
                detail = "오늘 서진이가 나한테 돈때문에 만나는거냐고" +
                    "\n 화내면서 물어 봤다..누가 이간질을 했나보다" +
                    "\n 나는 나를 믿지 못하고 나에게 그런것을 물어본" +
                    "\n 서진이가 너무 싫었다 나를 정말 믿어주는 친구는" +
                    "\n 없는걸까 하며 원망하고 저주했다. 도대체 내가 뭘" +
                    "\n 잘못했다고 요구한것도 아니고 뭘 바란것도 아니고" +
                    "\n 단지 그냥 친구가 사귀고 싶었을 뿐인데 왜 나한테만.." +
                    "\n 더이상 사람이 싫다 돈도 싫다 뭐가 이토록 나를 괴롭게" +
                    "\n 하는 건지.. 집에가서 가족과 대판 싸우고 맞았다 그리고" +
                    "\n 집을 나왔다.. 더이상 일기따위 안쓸거야...";
                break;
            case 8:
                day = "    년  월  일";
                detail = "안쓰기로 다짐한지 삼일도 안돼서 나는 다시" +
                    "\n 펜을 잡았다. 집에 들어가지도 씻지도 학교도" +
                    "\n 먹는것도 마시는것도 아무것도 하지않았다." +
                    "\n 나는 지금 다리위에서 일기를 쓰고 있다" +
                    "\n 처음에는 죽고싶어서 왔지만 지나다니는 사람들을" +
                    "\n 보면서 평범하게 바쁘고 힘들고 웃으면서 지내는.." +
                    "\n 모습이 너무 아름다웠다. 그래서 고민을 멈추고 그냥" +
                    "\n 이 마음이 가라 앉기까지 풍경을 보고있다" +
                    "\n 다시 처음부터 시작하면 되는거야 그러면서 마음을 " +
                    "\n 다지고 있다. 그리고 내 처음 사귄 서진이 다시 만날" +
                    "\n 날을 기대하면서 오해도 풀고 이야기도 하고 해볼거다" +
                    "\n 인생은 한참 남았으니깐! 나 화이팅!! 힘내자" +
                    "\n 아! 가족 한테도 ㅅ";
                break;
        }
        dirayImage.transform.GetChild(0).GetComponent<Text>().text = day;
        dirayImage.transform.GetChild(1).GetComponent<Text>().text = detail;
    }
    public void OffDiary()
    {
        dirayImage.SetActive(false);
    }
    public void ReTry()
    {
        DataController.instance.LoadData();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    IEnumerator PushMessage()
    {
        pushMessage.SetActive(true);
        buttonSound.ZombiOnce(6);
        yield return new WaitForSecondsRealtime(0.5f);
        pushMessage.SetActive(false);
    }
}