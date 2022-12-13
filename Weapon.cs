using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    public GameObject weaponHand;       // 무기 쥐는손

    public GameObject Inventory;        // 인벤토리
    public GameObject trashCanPanel;    // 쓰레기통 Panel

    public GameObject[] useSlot = new GameObject[5];   // 무기 아이템 슬롯
    public GameObject[] weaponKind;     // 무기 (막대기)
    public GameObject[] weaponImage;    // 인벤토리 무기이미지
    public GameObject touchIteam;       // 닿아있는 아이템
    public GameObject infoText;         // 안내 텍스트
    public GameObject gameDisplayPush;  // 게임화면 알림
    public GameObject lockerpanel;    // 자물쇠 이미지

    public static float Damage;         // 대미지
    public static float minDamage;      // 최소 대미지
    public static float maxDamage;      // 최대 대미지
    public int bandageNum;              // 붕대 갯수

    int slotUseNum;                     // 사용하고있는 슬롯 번호
    int prevSlotUseNum;                 // 이전에 사용하던 슬롯 번호

    int cheackUse;                      // 상호작용 1. 무기줍기 3. 문열/닫기
    bool isTalk;                        // 이야기 하고있는지
    int useItemCode;                    // 사용가능 아이템 코드
    int keyCode = 30000;
    public SoundManager Effect;

    public GameObject GetWeaponImage(int num) // 무기 이미지 가져오기 
    {
        GameObject getWeaponImage = weaponImage[num];
        return getWeaponImage;
    }


    public struct StcItemSlot   //인벤토리 슬롯
    {
        public bool isnum;  // 아이템 있는지
        public string name; // 아이템 이름
    }
    public struct StcUseSlot    // 1~5 슬롯 
    {
        public bool isnum;  // 아이템 있는지
        public string name; // 아이템 이름
    }
    public static StcUseSlot[] use = new StcUseSlot[5];     // 사용 슬롯 갯수만큼 배열선언
    public static StcItemSlot[] item = new StcItemSlot[8];  // 아이템 슬롯 갯수만큼 배열선언

    private void Awake()
    {
        #region 저장파일 불러오기
        // 인벤토리에 아이템 있는지 확인
        keyCode = DataController.instance.nowPlayer.LastGetKeyCode;   
        bandageNum = DataController.instance.nowPlayer.bandageCount;
        for (int i = 0; i < item.Length; i++) // 배열 선언
        {
            // 저장값 혹은 초기값을 대입
            item[i].name = DataController.instance.nowPlayer.inventoryItem[i];
            // 저장값에 아이템이 있으면 생성
            if (item[i].name != "Empty")
            {
                Respawn(i, 1);
            }

            // 인벤토리 번호의 자식오브젝트의 이름
            string itemName = Inventory.transform.GetChild(i).transform.GetChild(0).name;
            if (itemName != "Empty")
            {
                item[i].isnum = true;
                item[i].name = itemName;
            }
            else if(itemName.Equals("Empty"))
            {
                item[i].isnum = false;
                item[i].name = "Empty";
            }
        }
        // 사용 슬롯 1~5 아이템 있는지 확인
        for(int i = 0; i < use.Length; i++)
        {
            
            // 저장값 혹은 초기값을 대입
            use[i].name = DataController.instance.nowPlayer.useSlotItem[i];
            // 저장값에 아이템이 있으면 생성
            if (use[i].name != "Empty")
            {
                Respawn(i, 2);
            }

            string useName = useSlot[i].transform.GetChild(0).name;           
            if (useName != "Empty")
            {
                use[i].isnum = true;
                use[i].name = useName;
            }
            else if (useName.Equals("Empty"))
            {
                use[i].isnum = false;
                use[i].name = "Empty";
            }
        }
        #endregion
    }

    void Start()
    {
        Effect = transform.GetComponent<SoundManager>();
        Effect.bgmPlayer = GameObject.Find("Player Effect Sound").GetComponent<AudioSource>();
        infoText.SetActive(false);      // 안내 텍스트
        minDamage = 15;
        maxDamage = 25;
        slotUseNum = 5;
        prevSlotUseNum = 5;
        cheackUse = -1;
        useItemCode = 0;
        isTalk = false;
    }
    private void LateUpdate()
    {
        if (PlayerMove.currentHp > 0)
        {
            Damage = Random.Range(minDamage, maxDamage);    //랜덤 대미지
            infoText.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 2.0f, this.transform.position.z); // 안내 텍스트 위치
            infoText.transform.LookAt(ButtonManager.Cam.transform);
            EquipedWeapon();
            KeyDownEvent();
        }
    }
    public void EquipedWeapon() // 무기를 장착하고있는지 
    {
        if (slotUseNum != 5)
        {
            if (this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("idel") ||
                this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Walking")) // 무기변경시 고려사항
            {
                if (useSlot[slotUseNum].transform.GetChild(0).gameObject.name.Equals("Empty"))
                {
                    prevSlotUseNum = slotUseNum;
                    if (PlayerMove.isEquiped)
                    {
                        Destroy(weaponHand.transform.GetChild(0).gameObject);   // 오브젝트 삭제
                        this.gameObject.GetComponent<Animator>().SetBool("Weapon", false);
                    }
                    minDamage = 15;
                    maxDamage = 25;
                    PlayerMove.isEquiped = false;
                    return;
                }
                else if (useSlot[slotUseNum].transform.GetChild(0).gameObject.name != "Empty" && !PlayerMove.isEquiped)
                {
                    prevSlotUseNum = slotUseNum;
                    WeaponSet(slotUseNum);
                    if (weaponHand.transform.GetChild(0).tag != "UseItem")
                    {
                        weaponHand.transform.GetChild(0).GetComponent<BoxCollider>().enabled = false;
                    }
                }
                else if (useSlot[slotUseNum].transform.GetChild(0).gameObject.name != "Empty" && PlayerMove.isEquiped && prevSlotUseNum != slotUseNum)
                {
                    prevSlotUseNum = slotUseNum;
                    Destroy(weaponHand.transform.GetChild(0).gameObject);   // 오브젝트 삭제
                    this.gameObject.GetComponent<Animator>().SetBool("Weapon", false);
                    WeaponSet(slotUseNum);
                    if (weaponHand.transform.GetChild(0).tag != "UseItem")
                    {
                        weaponHand.transform.GetChild(0).GetComponent<BoxCollider>().enabled = false;
                    }
                }
            }
        }
    }
    public void KeyDownEvent() // 키에 따른 행동
    {
        switch (Input.inputString)
        {
            case "e": case "E": // 상호작용
                switch (cheackUse)
                {
                    case -1:
                        break;
                    case 1: // 장비
                        if (touchIteam != null)
                        {
                            ItemGetBtn();
                        }
                        break;
                    case 2:// 일기
                        if (touchIteam != null)
                        {
                            StartCoroutine(DestroyDiary());
                        }
                        break;
                    case 3: // 문
                        DoorEvent();
                        break;
                    case 4: // 붕대
                        if (touchIteam != null)
                        {
                            StartCoroutine(BandageGet());
                        }
                        break;
                    case 5: // 사물함
                        lockerpanel.SetActive(true);
                        switch (touchIteam.name)
                        {
                            case "트리거사물함기초":
                                lockerpanel.transform.GetChild(0).gameObject.SetActive(true);
                                lockerpanel.transform.GetChild(1).gameObject.SetActive(false);                                
                                break;
                            case "트리거사물함중간":
                                lockerpanel.transform.GetChild(0).gameObject.SetActive(false);
                                lockerpanel.transform.GetChild(1).gameObject.SetActive(true);
                                break;
                        }
                        break;
                }
                break;
            case "F":case "f":
                if(bandageNum != 0)
                {
                    if (PlayerMove.currentHp < 100)
                    {
                        PlayerMove.currentHp += 50;
                        bandageNum--;
                    }
                    else if (PlayerMove.currentHp >= 100)
                    {
                        StartCoroutine(Push("체력이 최대치 입니다."));
                    }
                }
                else if(bandageNum.Equals(0))
                {
                    StartCoroutine(Push("붕대가 부족합니다"));
                }
                break;
            case "1":
                    slotUseNum = 0;
                break;
            case "2":
                slotUseNum = 1;
                break;
            case "3":
                slotUseNum = 2;
                break;
            case "4":
                slotUseNum = 3;
                break;
            case "5":
                slotUseNum = 4;
                break;
            case "x":case "X":
                slotUseNum = 5;
                if (PlayerMove.isEquiped == true)
                {
                    Destroy(weaponHand.transform.GetChild(0).gameObject);   // 오브젝트 삭제
                    this.gameObject.GetComponent<Animator>().SetBool("Weapon", false);
                }
                minDamage = 15;
                maxDamage = 25;
                PlayerMove.isEquiped = false;
                break;
        }
    }

    void DoorEvent() //E 버튼을 누르면 문 이벤트
    {
        Animator ani = new Animator();
        if (touchIteam != null)
        {
            ani = touchIteam.GetComponent<Animator>();
        }
        switch (touchIteam.name)
        {
            case "유리문":
                DoorToggle(ani);
                break;
            case "정문":
                ConditionDoorToggle(ani, 30007, "정문 열쇠가 필요해..");
                break;
            case "닫힌유리문":
                Effect.PlayerOnce(15);
                StartCoroutine(TalkTime("이곳으로는 안가는게 좋겠어..", 1.0f));
                break;
            case "문 1":case "문004":
                switch(touchIteam.transform.parent.name)
                {
                    case "1학년1반":
                        ConditionDoorToggle(ani,30001,"1학년1반 열쇠가 필요해..");
                        break;
                    case "1학년2반":
                        DoorToggle(ani);
                        break;
                    case "1학년3반":
                        ConditionDoorToggle(ani, 30002, "1학년3반 열쇠가 필요해..");
                        break;
                    case "1학년4반":
                        ConditionDoorToggle(ani, 30003, "1학년4반 열쇠가 필요해..");
                        break;
                    case "1학년5반":
                        ConditionDoorToggle(ani, 30004, "1학년5반 열쇠가 필요해..");
                        break;
                    case "1학년6반":
                        ConditionDoorToggle(ani, 30005, "1학년6반 열쇠가 필요해..");
                        break;
                    case "교무실":
                        ConditionDoorToggle(ani, 30006, "교무실 열쇠가 필요해..");
                        break;
                }
                break;
            case "체육관문":
            case "체육관문2":
                bool isCheakitem= false;

                for (int i = 0; i < 5; i++) // 키가 있는지 확인
                {
                    if (useSlot[i].transform.GetChild(0).gameObject.name.Equals("체육관키_이미지"))
                    {
                        isCheakitem = true;
                    }
                }

                if (ani.GetBool("Door")) // 문이 열려있으면 언제든지 닫을수 있음
                {
                    ani.SetBool("Door", false);
                }
                else if (isCheakitem) //아이템이 있다면 문열기
                {
                    if (!ani.GetBool("Door"))
                    {
                        ani.SetBool("Door", true);
                    }
                }
                else if(!isCheakitem)
                {
                    StartCoroutine(TalkTime("체육관키가 필요해..", 2.0f));
                }
                break;
        }
    }
    void DoorToggle(Animator ani)
    {
        if (ani.GetBool("Door"))
        {
            if(touchIteam.name.Equals("유리문"))
            {
                Effect.PlayerOnce(11);
            }
            else
            {
                Effect.PlayerOnce(9);
            }
            ani.SetBool("Door", false);
        }
        else if (!ani.GetBool("Door"))
        {
            if (touchIteam.name.Equals("유리문"))
            {
                Effect.PlayerOnce(12);
            }
            else
            {
                Effect.PlayerOnce(10);
            }
            ani.SetBool("Door", true);
        }
    }
    void ConditionDoorToggle(Animator ani ,int code, string text)
    {
        bool isCheakitem = false;

        for (int i = 0; i < 5; i++) // 키가 있는지 확인
        {
            if(useSlot[i].transform.GetChild(0).gameObject.name.Equals("Empty"))
            {
                Effect.PlayerOnce(15);
                continue;
            }
            else if (useSlot[i].transform.GetChild(0).gameObject.transform.GetComponent<ItemWindow>().code.Equals(code))
            {
                isCheakitem = true;
            }
        }

        if (ani.GetBool("Door")) // 문이 열려있으면 언제든지 닫을수 있음
        {
            Effect.PlayerOnce(10);
            ani.SetBool("Door", false);
        }
        else if (isCheakitem) //아이템이 있다면 문열기
        {
            if (!ani.GetBool("Door"))
            {
                Effect.PlayerOnce(9);
                ani.SetBool("Door", true);
            }
        }
        else if (!isCheakitem)
        {
            StartCoroutine(TalkTime(text, 2.0f));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon") ||
            other.gameObject.CompareTag("UseItem")) // 아이템에 닿을때
        {
            touchIteam = other.gameObject;
            cheackUse = 1;
            if(other.gameObject.CompareTag("UseItem"))
            {
               useItemCode = touchIteam.transform.parent.GetComponent<WeaponDestroy>().code;
            }
            //infoText.transform.GetComponent<TextMesh>().color = Color.red;
            infoText.SetActive(true);
            TalkTextBox("무언가 있는데 주울까?...");
        }
        if (other.gameObject.CompareTag("Diary"))
        {
            touchIteam = other.gameObject;
            cheackUse = 2;
            infoText.SetActive(true);
            TalkTextBox("무언가 있는데 주울까?...");
        }
        if (other.gameObject.CompareTag("Bandage"))
        {
            touchIteam = other.gameObject;
            cheackUse = 4;
            infoText.SetActive(true);
            TalkTextBox("무언가 있는데 주울까?...");
        }
        if (other.gameObject.CompareTag("TrashCan")) // 쓰레기 통
        {
            trashCanPanel.SetActive(true);
        }
        if(other.gameObject.CompareTag("Locker"))
        {
            touchIteam = other.gameObject;
            cheackUse = 5;
            infoText.SetActive(true);
            if (touchIteam.name.Equals("트리거사물함기초"))
            {
                TalkTextBox("소연이의 사물함이다...");
            }
            else if(touchIteam.name.Equals("트리거사물함중간"))
            {
                TalkTextBox("여긴 누구의 사물함이지?");
            }
        }
        if (other.gameObject.CompareTag("Door"))
        {
            switch(other.gameObject.name)
            {
                case "유리문":
                    StartCoroutine(TalkTime("상호작용은 E를 눌르면 되던가..?", 1.0f));
                    break;
            }
        }
        if(other.gameObject.CompareTag("Stone")) // 돌에 맞으면
        {
            PlayerMove.currentHp = 0;
        }            
    }    
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Door"))    // 문에 접근
        {
            touchIteam = other.gameObject;
            cheackUse = 3;

            infoText.SetActive(true);
            if (other.gameObject.GetComponent<Animator>().GetBool("Door").Equals(false))
            {                
                TalkTextBox("문을 열어볼까?...");
            }
            else if (other.gameObject.GetComponent<Animator>().GetBool("Door").Equals(true))
            {
                TalkTextBox("문을 닫을까?...");
            }

        }
    }
    private void OnTriggerExit(Collider other)
    {
        #region 닿은 물건에서 빠져나올때 버튼제거
        if (other.gameObject.CompareTag("Weapon")) 
        {
            touchIteam = null;
            infoText.SetActive(false);
            cheackUse = -1;
        }
        if (other.gameObject.CompareTag("UseItem"))
        {
            touchIteam = null;
            infoText.SetActive(false);
            cheackUse = -1;
        }
        if (other.gameObject.CompareTag("Door"))
        {
            touchIteam = null;
            infoText.SetActive(false);
            cheackUse = -1;
        }
        if (other.gameObject.CompareTag("Diary"))
        {
            touchIteam = null;
            infoText.SetActive(false);
            cheackUse = -1;
        }
        if (other.gameObject.CompareTag("Bandage"))
        {
            touchIteam = null;
            infoText.SetActive(false);
            cheackUse = -1;
        }
        if (other.gameObject.CompareTag("Locker"))
        {
            lockerpanel.SetActive(false);
            touchIteam = null;
            infoText.SetActive(false);
            cheackUse = -1;
        }
        #endregion
        if (other.gameObject.CompareTag("TrashCan")) // 쓰레기 통
        {
            trashCanPanel.SetActive(false);
        }
    }
    public void ItemGetBtn() // 무기 줍기
    {
        for (int i = 0; i < item.Length; i++) // 아이템 창이 가득 차있는지 확인
        {
            if (!item[i].isnum ) // 인벤토리 자식중에서 가지고있는 아이템이 없는지 확인
            {
                DataController.instance.nowPlayer.GetItemCode.Add(touchIteam.transform.parent.GetComponent<WeaponDestroy>().code);

                StartCoroutine(DestroyObj(i)); // 오브젝트 흭득으로 인한 맵에서 제거 트리거
                return;
            }
            else if(i >= 7 && item[7].isnum)
            {
                StartCoroutine(Push("가방이 가득찼습니다."));
            }
        }
    }
    public void TalkTextBox(string talk)
    {
        if (isTalk)
        {
            return;
        }
        else if (!isTalk)
        {
            infoText.transform.GetChild(0).GetComponent<TextMesh>().text = talk;
        }
    }

    //아이템을 주우면 ItemGetBtn -> DesttoryObj -> GetItem -> SetTrans 순으로 실행
    IEnumerator TalkTime(string talk, float time) // 조건부 생각
    {
        isTalk = true;
        infoText.transform.GetChild(0).GetComponent<TextMesh>().text = talk;
        yield return new WaitForSeconds(time);
        isTalk = false;
    }
    IEnumerator DestroyObj(int i) // 무기 주우면 사라지게하는 오브젝트
    {
        GetComponent<Animator>().SetBool("GetItem", true);  // 애니메이션 true
        infoText.SetActive(false);                          // 안내 텍스트 false
        cheackUse = -1;                                     // 줍기 가능 불가능으로 만들기
        yield return new WaitForSeconds(1.5f);              // 1.5초 후 다시 호출 (줍는 애니메이션일때 아이템 삭제를 위한 타임)
        GetItem(i, 1);                                      // 인벤토리에 아이템 확인및 생성
        Effect.PlayerOnce(13);
        Destroy(touchIteam.transform.parent.gameObject);    // 만지고 있는 오브젝트 삭제
        yield return new WaitForSeconds(1.0f);              // 1초후 다시 호출 (애니메이션을 false 로 만들기위한 타임)
        GetComponent<Animator>().SetBool("GetItem", false); // 애니메이션 flase
        StartCoroutine(Push("가방에 물건이 들어왔습니다."));
        touchIteam = null;                                  // 만지고 있는 오브젝트를 null(없는 상태)로 만들기
    }
    IEnumerator DestroyDiary() // 무기 주우면 사라지게하는 오브젝트
    {
        int num = touchIteam.transform.GetComponent<DiaryTrigger>().dirayNum;
        GetComponent<Animator>().SetBool("GetItem", true);  // 애니메이션 true
        infoText.SetActive(false);                          // 안내 텍스트 false
        cheackUse = -1;                                   // 줍기 가능 불가능으로 만들기
        yield return new WaitForSeconds(1.5f);              // 1.5초 후 다시 호출 (줍는 애니메이션일때 아이템 삭제를 위한 타임)
        DataController.instance.nowPlayer.diaryslot[num - 1] = num;
        Effect.PlayerOnce(13);
        Destroy(touchIteam.transform.gameObject);           // 만지고 있는 오브젝트 삭제
        yield return new WaitForSeconds(1.0f);              // 1초후 다시 호출 (애니메이션을 false 로 만들기위한 타임)
        GetComponent<Animator>().SetBool("GetItem", false); // 애니메이션 flase
        StartCoroutine(Push("일기를 주웠습니다."));
        touchIteam = null;
    }
    IEnumerator BandageGet() // 무기 주우면 사라지게하는 오브젝트
    {
        DataController.instance.nowPlayer.GetItemCode.Add(touchIteam.transform.GetComponent<WeaponDestroy>().code);
        GetComponent<Animator>().SetBool("GetItem", true);  // 애니메이션 true
        infoText.SetActive(false);                          // 안내 텍스트 false
        cheackUse = -1;                                     // 줍기 가능 불가능으로 만들기
        yield return new WaitForSeconds(1.5f);              // 1.5초 후 다시 호출 (줍는 애니메이션일때 아이템 삭제를 위한 타임)
        Effect.PlayerOnce(13);
        Destroy(touchIteam.transform.gameObject);           // 만지고 있는 오브젝트 삭제
        yield return new WaitForSeconds(1.0f);              // 1초후 다시 호출 (애니메이션을 false 로 만들기위한 타임)
        GetComponent<Animator>().SetBool("GetItem", false); // 애니메이션 flase
        bandageNum++;
        StartCoroutine(Push("붕대를 주웠습니다."));
        touchIteam = null;
    }
    IEnumerator Push(string pushtxt)
    {
        gameDisplayPush.transform.GetChild(0).transform.GetComponent<Text>().text = pushtxt;
        gameDisplayPush.SetActive(true);
        Effect.PlayerOnce(14);
        yield return new WaitForSecondsRealtime(1.0f);
        gameDisplayPush.SetActive(false);
        gameDisplayPush.transform.GetChild(0).transform.GetComponent<Text>().text = "";
    }
    void GetItem(int i, int makeNum) // 아이템 주울때 아이템의 종류 확인 및 인벤토리창에 이미지 생성
    {
        GameObject getItem = null;
        switch (touchIteam.name)
        {
            case "유리조각":
                getItem = Instantiate(weaponImage[0]); // 이미지 소환
                break;
            case "삼각자":
                getItem = Instantiate(weaponImage[1]); // 이미지 소환
                break;
            case "커터칼":
                getItem = Instantiate(weaponImage[2]); // 이미지 소환
                break;
            case "빗자루":
                getItem = Instantiate(weaponImage[3]); // 이미지 소환
                break;
            case "리코더":
                getItem = Instantiate(weaponImage[4]); // 이미지 소환
                break;
            case "식칼":
                getItem = Instantiate(weaponImage[5]); // 이미지 소환
                break;
            case "대걸래":
                getItem = Instantiate(weaponImage[6]); // 이미지 소환
                break;
            case "체육관키":
                getItem = Instantiate(weaponImage[7]); // 이미지 소환
                getItem.transform.GetComponent<ItemWindow>().code = useItemCode;
                break;
            case "열쇠":
                getItem = Instantiate(weaponImage[8]); // 이미지 소환
                getItem.transform.GetComponent<ItemWindow>().code = useItemCode;
                if(DataController.instance.nowPlayer.LastGetKeyCode < useItemCode)
                {
                    DataController.instance.nowPlayer.LastGetKeyCode = useItemCode;
                }
                break;
        }
        getItem.name = touchIteam.name + "_이미지";
        SetTrans(getItem, i, makeNum);
    }
    public void WeaponSet(int i) // 1 ~ 5번 선택시 무기 생성
    {
        int weaponNum = -1; // 무기 번호
        // 무기 생성
        switch (useSlot[i].transform.GetChild(0).gameObject.name)
        {
            case "유리조각_이미지": // 막대기 이미지
                minDamage = 20; // 최소대미지
                maxDamage = 25; // 최대 대미지
                weaponNum = 0;  //무기 번호
                break;
            case "삼각자_이미지":
                minDamage = 48;
                maxDamage = 53;
                weaponNum = 1;
                break;
            case "커터칼_이미지":
                minDamage = 30;
                maxDamage = 45;
                weaponNum = 2;
                break;
            case "빗자루_이미지":
                minDamage = 45;
                maxDamage = 55;
                weaponNum = 3;
                break;
            case "리코더_이미지":
                minDamage = 30;
                maxDamage = 30;
                weaponNum = 4;
                break;
            case "식칼_이미지":
                minDamage = 70;
                maxDamage = 85;
                weaponNum = 5;
                break;
            case "대걸래_이미지":
                minDamage = 90;
                maxDamage = 150;
                weaponNum = 6;
                break;
            case "체육관키_이미지":
                weaponNum = 7;
                break;
            case "열쇠_이미지":
                weaponNum = 8;
                break;
        }

        //공통 사항
        if (weaponNum > -1)
        {
            GameObject weapon = Instantiate(weaponKind[weaponNum], new Vector3(0, 0, 0), Quaternion.identity);
            weapon.name = weaponKind[weaponNum].name;
            this.gameObject.GetComponent<Animator>().SetBool("Weapon", true);
            PlayerMove.isEquiped = true;
            weapon.transform.SetParent(weaponHand.transform);
            weapon.transform.localRotation = Quaternion.Euler(0, 0, 0);
            weapon.gameObject.transform.localPosition = new Vector3(0, 0, 0);
            if(weaponNum.Equals(6))
            {
                this.gameObject.GetComponent<Animator>().SetBool("TwoHand", true);
            }
            else
            {
                this.gameObject.GetComponent<Animator>().SetBool("TwoHand", false);
            }

            if (weaponNum.Equals(7)|| weaponNum.Equals(8))
            {
                this.gameObject.GetComponent<Animator>().SetBool("Weapon", false);
                this.gameObject.GetComponent<Animator>().SetBool("TwoHand", false);
            }
        }
    }
    // 세이브 파일 가져올때 실행
    void Respawn(int i, int makeNum)
    {
        GameObject getItem = null;
        string weaponName = "";
        if (makeNum.Equals(1))
        {
            weaponName = item[i].name;
        }
        else if (makeNum.Equals(2))
        {
            weaponName = use[i].name;
        }

        if (weaponName != null)
        {
            switch (weaponName)
            {
                case "유리조각_이미지":
                    getItem = Instantiate(weaponImage[0]); // 이미지 소환
                    break;
                case "삼각자_이미지":
                    getItem = Instantiate(weaponImage[1]); // 이미지 소환
                    break;
                case "커터칼_이미지":
                    getItem = Instantiate(weaponImage[2]); // 이미지 소환
                    break;
                case "빗자루_이미지":
                    getItem = Instantiate(weaponImage[3]); // 이미지 소환
                    break;
                case "리코더_이미지":
                    getItem = Instantiate(weaponImage[4]); // 이미지 소환
                    break;
                case "식칼_이미지":
                    getItem = Instantiate(weaponImage[5]); // 이미지 소환
                    break;
                case "대걸래_이미지":
                    getItem = Instantiate(weaponImage[6]); // 이미지 소환
                    break;
                case "체육관키_이미지":
                    getItem = Instantiate(weaponImage[7]); // 이미지 소환
                    getItem.transform.GetComponent<ItemWindow>().code = keyCode;
                    break;
                case "열쇠_이미지":
                    getItem = Instantiate(weaponImage[8]); // 이미지 소환
                    if(keyCode > 30000)
                    {
                        getItem.transform.GetComponent<ItemWindow>().code = keyCode;
                        keyCode--;
                    }
                    break;
            }
        }

        if (getItem != null)
        {
            getItem.name = weaponName;
            SetTrans(getItem, i, makeNum);
        }
    }
    void SetTrans(GameObject obj, int i, int makeNum) // 주운 아이템 생성 및 부모, 정보초기화 설정
    {
        // 인벤토리에 이미지 생성
        if (makeNum.Equals(1)) 
        {
            obj.transform.SetParent(Inventory.transform.GetChild(i).gameObject.transform); // 생성된 오브젝트 부모설정            
            item[i].isnum = true;   // 오브젝트 있음
            item[i].name = obj.name;// 이름 변경
        }
        // 사용슬롯에 이미지 생성
        else if(makeNum.Equals(2))
        {
            obj.transform.SetParent(useSlot[i].gameObject.transform); // 생성된 오브젝트 부모설정
            use[i].isnum = true;    // 오브젝트 있음으로 변경
            use[i].name = obj.name; // 이름 변경
        }
        //생성된 오브젝트 위치 초기화
        obj.transform.SetAsFirstSibling();
        obj.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        obj.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        obj.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
    }
    public IEnumerator WeaponAttack()
    {
        switch(weaponHand.transform.GetChild(0).name)
        {
            case "커터칼":case "유리조각":
            case "식칼":  case "삼각자":
                Effect.PlayerRandomAttack(2,8);
                break;
            case "대걸래":case "빗자루":
                Effect.PlayerOnce(1);
                break;
            case "리코더":
                //음악소리
                break;
        }
        weaponHand.transform.GetChild(0).GetComponent<BoxCollider>().enabled = true;
        yield return new WaitForSeconds(0.3f);
        weaponHand.transform.GetChild(0).GetComponent<BoxCollider>().enabled = false;
    }
}