using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    Rigidbody playerRigid;                  // 플레이어 물리스크립트
    public float speed;                     // 이동속도
    public Camera mainCam;                  // 메인 카메라
    Animator animePlayer;                   // 플레이어 애니메이션

    //public Weapon weaponScrt;             // 무기 스크립트
    public static bool isEquiped;   // 무기 장착여부

    float colTime = 0f;                     // 쿨타임
    public Image dodgeColtime;

    public Text staminaText;                // 스테미너 텍스트
    public Image staminaFill;               // 스테미너 이미지 오브젝트
    float staminaHealTime = 0f;             // 스태미나 자연치유
    float stamina = 100;                    // 스태미나

    //체력
    float hpHealTime = 0f;                  // 체력 자연치유 시간
    public static float currentHp;          // 현재 체력
    float maxHp;                            // 최대 체력
    public Image hpObj;                     // 체력 이미지 오브젝트
    public Text hpText;                     // 체력 텍스트
    public GameObject DeadPanel;
    
    public static float defense;            // 방어력
    public GameObject hitBox;               // 히트박스
    public GameObject moveSound;            // 움직임 사운드
    bool isDodge;                           // 회피 체크
    bool isDodgeSpeed;                      // 회피 스피드 줄였는지
    public static bool isStory = false;     // 스토리중
    Vector3 moveposi;                       // 움직임

    void Start()
    {
        transform.position = DataController.instance.nowPlayer.PlayerPos;   // 저장된 플레이어 위치 불러오기

        maxHp = 100;
        defense = 1;
        currentHp = maxHp;
        animePlayer = transform.GetComponent<Animator>();
        playerRigid = transform.GetComponent<Rigidbody>();
        isDodgeSpeed = false;
        hitBox.SetActive(false);
        isEquiped = false;
    }
    
    void Update()
    {
        if (!currentHp.Equals(0))
        {
            if(!isStory)
            {
                Move();     // 움직임
                Heal();     // 자연치유
                Dodge();    // 회피
                KeyAttack();
            }
        }
        else if(currentHp <= 0)
        {
            DeadPanel.SetActive(true);
        }
    }
    private void LateUpdate()
    {
        CamMove();      // 카메라 이동
        LimitStatus();  // 체력 및 스테미나 한도 , 데이터 출력
    }

    void Heal() // 자연치유 체력 및 스테미나
    {
        if (stamina < 100)
        {
            staminaHealTime += Time.deltaTime;

            if (staminaHealTime >= 2.0f)
            {
                stamina += 5;
                staminaHealTime = 0;
            }
        }
        if (currentHp < maxHp)
        {
            hpHealTime += Time.deltaTime;

            if (hpHealTime >= 1.5f)
            {
                currentHp += 1f;
                hpHealTime = 0;
            }
        }
    }
    void LimitStatus() // 체력및 스테미너 한도 / hp,stamina UI정의
    {
        staminaText.text = stamina.ToString();
        staminaFill.fillAmount = stamina * 0.01f;

        hpText.text = currentHp.ToString();
        hpObj.fillAmount = currentHp * 0.01f;

        // 체력 한도 설정
        if (currentHp >= 100)
        {
            currentHp = 100;
        }
        else if (currentHp <= 0)
        {
            currentHp = 0;
            animePlayer.SetBool("Death", true);
        }
        // 스테미너 한도 설정
        if (stamina >= 100)
        {
            stamina = 100;
        }
        else if (stamina <= 0)
        {
            stamina = 0;
        }
    }

    void CamMove()  // 카메라 이동
    {
        mainCam.transform.position = new Vector3(transform.position.x - 2.5f, transform.position.y + 2.87f, transform.position.z);
        mainCam.transform.eulerAngles = new Vector3(40.0f, 90.0f, .0f);
    }
    void Move() // 이동
    {
        // !animePlayer.GetBool("GetItem") && !isAttack
        
        if (animePlayer.GetCurrentAnimatorStateInfo(0).IsName("idel") ||
            animePlayer.GetCurrentAnimatorStateInfo(0).IsName("Walking")||
            animePlayer.GetCurrentAnimatorStateInfo(0).IsName("Roll")) // 기본와 걷기, 회피 할때만 움직임가능
        {
            // 움직임 받는곳
            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");

            // 움직임 애니메이션
            if ((x != 0 || z != 0))
            {
                moveSound.SetActive(true);
                animePlayer.SetBool("Move", true);
            }
            else
            {
                moveSound.SetActive(false);
                animePlayer.SetBool("Move", false);
            }
            // 움직이지 않을 시 리턴
            if (x == 0 && z == 0)
            {
                return;
            }
            // 움직임
            moveposi = new Vector3(z, 0, -x).normalized * speed * Time.deltaTime * 2;    // 움직임 값 및 이동속도, 등속 연산
            playerRigid.MovePosition(transform.position + moveposi);                // 움직이게 하기

            Quaternion newRotation = Quaternion.LookRotation(moveposi);             // 회전값을 이동하는 곳을 바라보는 방향으로
            this.playerRigid.rotation = Quaternion.Slerp(this.playerRigid.rotation, newRotation, Time.deltaTime * 8);   // 회전하는 방향 및 속도
        }
    }

    public void KeyAttack()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            animePlayer.SetBool("Attack", true);
        }
        else if(Input.GetKeyUp(KeyCode.Space))
        {
            animePlayer.SetBool("Attack", false);
        }
    }
    void Dodge() // 회피기
    {
        if (isDodge)
        {            
            colTime += Time.deltaTime;
            dodgeColtime.fillAmount = 1.0f - (colTime * 0.25f);

            if (colTime >= 1.3f && !isDodgeSpeed)
            {
                animePlayer.SetBool("Roll", false);
                isDodgeSpeed = true;
                speed /= 1.5f;
            }
            if(colTime >= 4.0f)
            {
                isDodge = false;
                isDodgeSpeed = false;
            }
        }
        else
        {
            colTime = 0f;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDodge && stamina >= 10) // 회피기
        {
            isDodge = true;
            animePlayer.SetBool("Attack", false);
            animePlayer.SetBool("GetItem", false);
            animePlayer.SetBool("Roll", true);
            speed *= 1.5f;
            stamina -= 10;
        }
    }
    
    public IEnumerator BoxinghitBox() // 기본공격시 히트박스 키고 끄기
    {
        GetComponent<Weapon>().Effect.PlayerOnce(0);
        hitBox.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        hitBox.SetActive(false);
    }
}