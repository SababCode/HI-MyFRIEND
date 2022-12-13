using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjStory : MonoBehaviour
{
    public GameObject infoText;
    string[] sty;
    int storynum;
    void Awake()
    {
        switch(this.gameObject.name)
        {
            case "시작":
                storynum = 50001;
                sty = new string[3];
                sty[0] = "너무 어두워..";
                sty[1] = "손전등를 켜보자..";
                sty[2] = "ESC를 눌러 핸드폰을 꺼낼수있어";
            break;
            case "탐색":
                storynum = 50002;
                sty = new string[2];
                sty[0] = "아무도 없나?";
                sty[1] = "주위를 둘러보자";
            break;
            case "핏자국":          
                storynum = 50003;
                sty = new string[3];
                sty[0] = "\"으악! 깜짝이야!!!\"";
                sty[1] = "놀래라..케첩인가?";
                sty[2] = "뭔가 비린내가 나는거 같기도 하고?";
            break;
            case "조우":
            storynum = 50004;
                sty = new string[4];
                sty[0] = "어 사람이다!";
                sty[1] = "지금 시간이면 경비아저씨겠지?";
                sty[2] = "\"경비아저씨!!!!\"";
                sty[3] = "\"저 아직 학교 못나갔어요!\"";
            break;
            case "무기장착":
                storynum = 50005;
                sty = new string[3];
                sty[0] = "모든 사용 아이템은";
                sty[1] = "넘버 슬롯에 올려놓은뒤";
                sty[2] = "사용이 가능해";
            break;
            case "시체들":
                storynum = 50006;
                sty = new string[4];
                sty[0] = "\"깜짝이야!\"";
                sty[1] = "움직이는건 아니겠지..?";
                sty[2] = "제발...";
                sty[3] = "나가고싶어..";
                break;
            case "중간보스":
                storynum = 50007;
                sty = new string[4];
                sty[0] = "뭐야 저건..";
                sty[1] = "어.. 음... \"실례했습니다..\"";
                sty[2] = "라고 하기에는 저기에 열쇠가 보이는데.. 하..";
                sty[3] = "\"들어와라 악당아!\"";
                break;
            case "지나가요":
                storynum = 50008;
                sty = new string[5];
                sty[0] = "무슨 소리지?";
                sty[1] = "위..인가?";
                sty[2] = "어? \"이런..!\"";
                sty[3] = "\"무너진다!!\"";
                sty[4] = "\"이곳을 나가야해!\"";
            break;
        }
        for(int i = 0; i < DataController.instance.nowPlayer.StoryCode.Count; i++)
        {
            if(DataController.instance.nowPlayer.StoryCode[i].Equals(storynum))
            {
                transform.GetComponent<BoxCollider>().enabled = false;
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            infoText = other.transform.GetComponent<Weapon>().infoText;
            StartCoroutine(StoryText(sty));
            DataController.instance.nowPlayer.StoryCode.Add(storynum);
        }
    }
    IEnumerator StoryText(string[] text)
    {
        int count = 0;
        Time.timeScale = 0;
        ButtonManager.isReadStroy = true;
        while(count < text.Length)
        {
            infoText.transform.GetChild(0).GetComponent<TextMesh>().text = text[count];
            infoText.SetActive(true);
            yield return new WaitForSecondsRealtime(2.0f);        
            infoText.SetActive(false);
            count++;
        }
        Time.timeScale = 1;
        transform.GetComponent<BoxCollider>().enabled = false;        
        ButtonManager.isReadStroy = false;    
    }
}
