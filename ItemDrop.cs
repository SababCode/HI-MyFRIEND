using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDrop : MonoBehaviour, IPointerEnterHandler, IDropHandler, IPointerExitHandler
{
    Image image;
    RectTransform rect;
    private void Awake()
    {
        image   = GetComponent<Image>();
        rect    = GetComponent<RectTransform>();
    }

    //마우스 포인터가 현재 아이템 슬롯 영역 내부로 들어갈 때 1회 호출
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 아이템 슬롯의 색상을 노란색으로 변경
        image.color = Color.yellow;
        if(image.gameObject.name.Equals("Thresh"))
        {
            image.transform.parent.GetComponent<Image>().color = Color.yellow;
        }
    }

    //마우스 포인터가 현재 아이템 슬롯 영역을 빠져나갈 때 1회 호출
    public void OnPointerExit(PointerEventData eventData)
    {
        //아이템 슬롯의 색상을 하얀색으로 변경
        image.color = Color.white;
        if (image.gameObject.name.Equals("Thresh"))
        {
            image.transform.parent.GetComponent<Image>().color = Color.white;
        }
    }

    //현재 아이템 슬롯 영역 내부에서 드롭을 했을 때 1회 호출
    public void OnDrop(PointerEventData eventData)
    {
        // PointerDrag는 현재 드래그하고 있는 대상 (=아이템)
        if (eventData.pointerDrag != null)
        {
            if (transform.name.Equals("Thresh"))
            {
                SetEventData(eventData);
            }
            else if (transform.GetChild(0).name.Equals("Empty") && transform.name != "Thresh")
            {
                // 넣는 곳이 빈칸일때
                // 드래그하고 있는 대상의 부모를 현재 오브젝트로 설정하고, 위치를 현재 오브젝트 위치와 동일하게 설정
                SetEventData(eventData);

                // 인벤토리 슬롯에 들어가면
                if (transform.parent.name.Equals("Panel Inventory"))
                {
                    Weapon.item[int.Parse(transform.name)].isnum = true;
                    Weapon.item[int.Parse(transform.name)].name = transform.GetChild(0).name;
                }
                // 사용 슬롯에 들어가면
                else if (transform.parent.name.Equals("Numberpanel"))
                {
                    Weapon.use[int.Parse(transform.name)].isnum = true;
                    Weapon.use[int.Parse(transform.name)].name = transform.GetChild(0).name;
                }
            }

        }
    }

    void SetEventData(PointerEventData eventData)
    {
        eventData.pointerDrag.transform.SetParent(transform);
        eventData.pointerDrag.transform.SetAsFirstSibling();
        eventData.pointerDrag.GetComponent<RectTransform>().position = rect.position;
    }
}