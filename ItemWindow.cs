using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemWindow : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,
    IPointerEnterHandler, IPointerExitHandler
{
    Transform canvas;
    Transform previousPatent;
    RectTransform rect;
    CanvasGroup canvasGroup;

    GameObject InfoObj;
    Text[] MoreInfo = new Text[2];
    public int code;

    void Awake()
    {
        canvas      = FindObjectOfType<Canvas>().transform;
        rect        = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        InfoObj = transform.GetChild(0).gameObject;
        InfoObj.SetActive(false);
        MoreInfo[0] = InfoObj.transform.GetChild(0).GetComponent<Text>();
        MoreInfo[1] = InfoObj.transform.GetChild(1).GetComponent<Text>();
    }
    // 현재 오브젝트를 드래그하기 시작할 때 1회 호출
    public void OnBeginDrag(PointerEventData eventDate)
    {
        // 드래그 직전에 소속되어 있던 부모 Transform 정보 지정
        previousPatent = transform.parent;
        // 드래그하면 전에있던 부모 숫자의 아이템값 변경
        if (previousPatent.parent.name.Equals("Panel Inventory"))
        {
            Weapon.item[int.Parse(transform.parent.name)].isnum = false;
            Weapon.item[int.Parse(transform.parent.name)].name = "Empty";
        }
        else if(previousPatent.parent.name.Equals("Numberpanel"))
        {
            Weapon.use[int.Parse(transform.parent.name)].isnum = false;
            Weapon.use[int.Parse(transform.parent.name)].name = "Empty";
        }

        // 현재 드래그중인 UI가 화면의 최상단에 출력되도록 하기 위해
        transform.SetParent(canvas);        // 부모 오브젝트를 Canvas로 설정
        transform.SetAsLastSibling();       // 가장 앞에 보이도록 마지막 자식으로 설정

        // 드래그 가능한 오브젝트가 하나가 아닌 자식들을 가지고 있을 수도 있기떄문에 CanvasGroup으로 통제
        // 알파값을 0.6으로 설정하고, 광선 충돌처리가 되지 않도록 한다.
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    // 현재 오브젝트를 드래그 중일 때 매 프레임 호출
    public void OnDrag(PointerEventData eventData)
    {
        // 현재 스크린상의 마우스 위치를 UI 위치로 설정(Ui가 마우스를 쫒아다니는 상태)
        rect.position = eventData.position;
    }

    // 현재 오브젝트의 드래그를 종료할 때 1회 호출
    public void OnEndDrag(PointerEventData eventData)
    {
        // 드래그를 시작하면 부모가 canvas로 설정되기 때문에
        // 드래그를 종료할 때 부모가 canvas이면 아이템 슬롯이 아닌 엉뚱한 곳에
        // 드롭을 했다는 뜻이기 때문에 드래그 직전에 소속되어 있던 아이템 슬롯으로 아이템 이동
        if (transform.parent.Equals(canvas))
        {
            #region 캔버스에 떨어졌을때
            transform.SetParent(previousPatent);
            transform.SetAsFirstSibling();
            rect.position = previousPatent.GetComponent<RectTransform>().position;

            if (previousPatent.GetChild(0).name != "Empty" && previousPatent.name.Equals("Panel Inventory"))
            {
                Weapon.item[int.Parse(previousPatent.name)].isnum = true;
                Weapon.item[int.Parse(previousPatent.name)].name = previousPatent.GetChild(0).name;                
            }
            else if (previousPatent.GetChild(0).name != "Empty" && previousPatent.name.Equals("Numberpanel"))
            {
                Weapon.use[int.Parse(previousPatent.name)].isnum = true;
                Weapon.use[int.Parse(previousPatent.name)].name = previousPatent.GetChild(0).name;
            }
            #endregion
        }

        // 쓰레기통으로 들어가면 아이템 삭제
        if (transform.parent.gameObject.name.Equals("Thresh"))
        {
            Destroy(this.gameObject);
            return;
        }
        // 알파값을 1로 설정하고, 광선 충돌처리가 되도록 한다.
        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;
    }       

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (transform.parent.parent.name.Equals("Panel Inventory")) // 인벤토리 안에서만 정보 표시
        {
            int min = 0;
            int max = 0;
            string txt = "";
            MoreInfo[0].text = transform.name.Substring(0, transform.name.IndexOf('_'));

            // _Image 제외 시키고 이름을 Text 값으로 표시
            switch (transform.name.Substring(0, transform.name.IndexOf('_')))
            {
                case "유리조각":
                    min = 20;
                    max = 25;
                    break;
                case "삼각자":
                    min = 48;
                    max = 53;
                    break;
                case "커터칼":
                    min = 30;
                    max = 45;
                    break;
                case "빗자루":
                    min = 45;
                    max = 55;
                    break;
                case "리코더":
                    min = 30;
                    max = 30;
                    break;
                case "식칼":
                    min = 70;
                    max = 85;
                    break;
                case "대걸래":
                    min = 90;
                    max = 150;
                    break;
                case "체육관키":
                    txt = "체육관을 열수있는 키";
                    break;
                case "열쇠":
                    switch(code)
                    {
                        case 30001:
                            txt = "1학년1반 키";
                            break;
                        case 30002:
                            txt = "1학년3반 키";
                            break;
                        case 30003:
                            txt = "1학년4반 키";
                            break;
                        case 30004:
                            txt = "1학년5반 키";
                            break;
                        case 30005:
                            txt = "1학년6반 키";
                            break;
                        case 30006:
                            txt = "교무실 키";
                            break;
                        case 30007:
                            txt = "정문 키";
                            break;
                    }                    
                    break;
            }

            if (transform.CompareTag("Weapon"))
            {
                MoreInfo[1].text = min + " ~ " + max;
            }
            else if(transform.CompareTag("UseItem"))
            {
                MoreInfo[1].text = txt;
            }
            InfoObj.SetActive(true);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        InfoObj.SetActive(false);
    }
}