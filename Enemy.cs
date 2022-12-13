using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    float maxHP;
    public static float Enemydamage;
    bool islive;
    public float currentHP;
    float distance;
    public GameObject objHP;
    public GameObject player;
    public Animator animator;
    public Collider AttackCollider;
    public int code;
    public int defence;
    float speed = 1f;
    public GameObject damageText;
    Vector3 dameTxtMove;
    public GameObject centerObj;
    public GameObject knifeEffect;
    public GameObject bluntEffect;

    GameObject middleBossTrigger;
    GameObject bossTrigger;
    public SoundManager Effect;
    float attackDistance;
    float searchDistance;

    private void Awake()
    {
        // 몬스터 죽고 세이브시 젠 안됨
        for (int i = 0; i < DataController.instance.nowPlayer.KillEnemyCode.Count; i++)
        {
            if (DataController.instance.nowPlayer.KillEnemyCode[i].Equals(code))
            {
                Destroy(this.gameObject);
                return;
            }
        }
    }
    void Start()
    {
        #region 몬스터 종류 
        switch (this.gameObject.name)
        {
            case "Zombi":
                maxHP = 100.0f;
                Enemydamage = 30;
                defence = 5;
                attackDistance = 0.7f;
                searchDistance = 5.0f;
                speed = 1.0f;
                break;
            case "Zombi2":
                maxHP = 500.0f;
                Enemydamage = 30;
                defence = 5;
                attackDistance = 0.7f;
                searchDistance = 5.0f;
                speed = 1.0f;
                break;
            case "Boss":
                maxHP = 1000.0f;
                Enemydamage = 35;
                defence = 10;
                attackDistance = 1.5f;
                searchDistance = 10.0f;
                speed = 3.5f;
                break;
        }
        #endregion

        // 공통 세팅
        Effect = transform.GetComponent<SoundManager>();
        Effect.bgmPlayer = GameObject.Find("Zombi Effect Sound").GetComponent<AudioSource>();

        AttackCollider.enabled = false;
        player = ButtonManager.Player;
        animator = GetComponent<Animator>();
        currentHP = maxHP;
        islive = true;
        damageText.SetActive(false);

        if(code.Equals(20008))
        { 
            middleBossTrigger = GameObject.Find("죽이기전못나감");
            middleBossTrigger.GetComponent<BoxCollider>().enabled = false;
        }
        if(code.Equals(20100))
        {
            middleBossTrigger = GameObject.Find("BossKill");
            bossTrigger = GameObject.Find("체육관문");
            middleBossTrigger.GetComponent<BoxCollider>().enabled = false;
            bossTrigger.GetComponent<BoxCollider>().enabled = true;
        }
    }

    void Update()
    {
        #region 생존시 이동 & 죽음
        if (islive)
        {
            centerObj.transform.LookAt(ButtonManager.Cam.transform);
            damageText.transform.Translate(dameTxtMove * Time.deltaTime);
            EnemyMove();
        }
        else if(!islive)
        {
            StartCoroutine(EnemyDeath());
        }
        #endregion
        SetHp();

    }
    void EnemyMove() //이동 및 공격
    {
        distance = Vector3.Distance(transform.position, player.transform.position);
        Vector3 lookplayer = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);

        #region 플레이어 거리 간격별 이동 및 공격
        // 플레이어와의 거리 0.7이하 일때 공격
        if (PlayerMove.currentHp == 0)
        {
            if (distance > 0.05f && distance < 1.0f) // 물어 뜯기를 위한 이동
            {
                transform.LookAt(lookplayer);
                Vector3 playerfollow = new Vector3(0,0,0);
                if (this.gameObject.name != "Boss")
                {
                    playerfollow = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
                }
                else if (this.gameObject.name.Equals("Boss"))
                {
                    playerfollow = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
                }
                transform.position = Vector3.MoveTowards(transform.position, playerfollow, speed * Time.deltaTime);
                animator.SetBool("Move", true);
                animator.SetBool("Attack", false);
            }
            else if(distance <= 0.05f)  // 물어뜯기
            {
                animator.SetBool("Attack", false);
                animator.SetBool("Move", false);
                animator.SetBool("Bit", true);
                if (Effect.ConditionMusic("좀비 Eat").Equals(false))
                {
                    Effect.ZombiOnce(5);
                }
            }            
        }
        else if (distance <= attackDistance)  // 공격
        {
            transform.LookAt(lookplayer);
            animator.SetBool("Attack", true);
            animator.SetBool("Move", false);
        }
        // 0.7초과 10 미만일때 이동, 어택 애니메이션이 아닐때
        else if (distance > attackDistance && distance < searchDistance && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if (code.Equals(20008))
            {
                if (middleBossTrigger.GetComponent<BoxCollider>().enabled.Equals(false))
                {   //중보 조우 음악
                    //ButtonManager.Cam.GetComponent<ButtonManager>().soundManager.BGMSoundPlay(3);
                    middleBossTrigger.GetComponent<BoxCollider>().enabled = true;
                }
            }
            if(code.Equals(20100))
            {
                if (bossTrigger.GetComponent<BoxCollider>().enabled.Equals(true))
                {
                    //ButtonManager.Cam.GetComponent<ButtonManager>().soundManager.BGMSoundPlay(3);
                    bossTrigger.GetComponent<Animator>().SetBool("Door", false);
                    bossTrigger.GetComponent<BoxCollider>().enabled = false;
                }
            }
            transform.LookAt(lookplayer);
            Vector3 playerfollow = new Vector3(0, 0, 0);
            if (this.gameObject.name != "Boss")
            {
                playerfollow = new Vector3(player.transform.position.x, 0.15f, player.transform.position.z);
            }
            else if (this.gameObject.name.Equals("Boss"))
            {
                playerfollow = new Vector3(player.transform.position.x, -2.88f, player.transform.position.z);
            }
            transform.position = Vector3.MoveTowards(transform.position, playerfollow, speed * Time.deltaTime);
            if (!animator.GetBool("Move"))
            {
                Effect.ZombiOnce(0);
            }
            animator.SetBool("Attack", false);
            animator.SetBool("Move", true);
        }
        else  // 아무것도 안할때
        {
            animator.SetBool("Attack", false);
            animator.SetBool("Move", false);
        }
        #endregion
    }
    void SetHp() //HP / 죽음 및 생존여부
    {
        objHP.transform.localScale = new Vector3(currentHP / maxHP, 1, 1);
        if (currentHP <= 0f)
        {
            currentHP = 0f;
            islive = false;
        }
        else if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
    }
    private void OnTriggerEnter(Collider other) //공격에 맞았을때 , 무기로 맞았을때, Player한테 맞았을때
    {
        if (other.gameObject.CompareTag("Attack"))  // 플레이어한테 피격
        {
            currentHP -=  Weapon.Damage - defence;
            if(islive)
            {
                StartCoroutine(GetHitDamgeText());
                if (other.gameObject.name.Equals("PunchHitBox"))
                {
                    GameObject blunt = Instantiate(bluntEffect);
                    blunt.transform.SetParent(this.gameObject.transform);
                    blunt.transform.localPosition = new Vector3(0.0f, 1.4f, 0.0f);
                }
                else if( other.gameObject.name.Equals("대걸래") || other.gameObject.name.Equals("빗자루") || other.gameObject.name.Equals("리코더"))
                {
                    GameObject blunt = Instantiate(bluntEffect);
                    blunt.transform.SetParent(this.gameObject.transform);
                    blunt.transform.localPosition = new Vector3(0.0f, 1.4f, 0.0f);
                }
                else if (other.gameObject.name != "대걸래" && other.gameObject.name != "PunchHitBox" && other.gameObject.name != "리코더")
                {
                    GameObject knife = Instantiate(knifeEffect);
                    knife.transform.SetParent(this.gameObject.transform);
                    knife.transform.localPosition = new Vector3(0.0f, 1.4f, 0.0f);
                }
            }
        }
    }
    public void AttackOn()
    {
        Effect.ZombiRandomAttack(1, 4);
        AttackCollider.enabled = !AttackCollider.enabled;        
    }
    IEnumerator EnemyDeath()
    {
        if(!animator.GetBool("Death"))
        { 
            Effect.ZombiOnce(4);
        }
        AttackCollider.enabled = false;
        animator.SetBool("Death", true);
        yield return new WaitForSeconds(7.0f);
        DataController.instance.nowPlayer.KillEnemyCode.Add(code);
        if(code.Equals(20008))
        {
            middleBossTrigger.GetComponent<BoxCollider>().enabled = false;
        }
        if(code.Equals(20100))
        {
            bossTrigger.GetComponent<BoxCollider>().enabled = true;
            middleBossTrigger.GetComponent<BoxCollider>().enabled = true;
        }
        Destroy(this.gameObject);
    }
    IEnumerator GetHitDamgeText()
    {
        damageText.transform.localPosition = Vector3.zero;
        dameTxtMove = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), -1f).normalized;
        damageText.GetComponent<TextMesh>().text = ((int)Weapon.Damage - (int)defence).ToString();
        damageText.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        damageText.SetActive(false);
    }
}