using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Password : MonoBehaviour
{
    public GameObject locker;
    public GameObject clearItem;
    public int[] password;
    public List<int> pressPassword;
    ColorBlock colorblock;
    Button Btn;
    Color selectcolor = new Color(1f, 0f, 0f, 1f);
    Color noncolor = new Color(1f, 1f, 1f, 1f);

    public void PressBtn(int num)
    {
        bool ischeck = false;
        Btn = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        #region 버튼 누르면 숫자를 리스트에 추가
        if (Btn.colors.normalColor != selectcolor)
        {
            if (pressPassword.Count != 0)
            {
                for (int i = 0; i < pressPassword.Count; i++)
                {
                    if (pressPassword[i].Equals(num))
                    {
                        ischeck = true;
                        break;
                    }
                    else
                    {
                        ischeck = false;
                    }
                }
            }
            if (pressPassword.Count.Equals(0) || !ischeck)
            {
                SelectBtn(num);
            }
        }
        #endregion
    }
    public void ClearSelect()
    {
        for (int i = 0; i < pressPassword.Count; i++)
        {
            Button chiBtn = Btn.gameObject.transform.parent.GetChild(pressPassword[i] - 1).transform.GetComponent<Button>();

            colorblock = chiBtn.colors;
            colorblock.normalColor = noncolor;
            colorblock.selectedColor = noncolor;
            chiBtn.colors = colorblock;
        }
        pressPassword.Clear();
    }

    void SelectBtn(int num)
    {
        pressPassword.Add(num);
        colorblock = Btn.colors;
        colorblock.normalColor = selectcolor;
        colorblock.selectedColor = selectcolor;
        Btn.colors = colorblock;
    }
    public void Checkbtn()
    {
        Allbtn(); // 자리상관없이 번호만 맞으면됨
    }
    public void Allbtn() // 버튼 숫자만 맞으면 됨
    {
        if (password.Length.Equals(pressPassword.Count))
        {
            bool[] isLogin = new bool[password.Length];
            for (int i = 0; i < password.Length; i++)
            {
                for (int j = 0; j < pressPassword.Count; j++)
                {
                    if (password[i].Equals(pressPassword[j]))
                    {
                        isLogin[i] = true;
                        break;
                    }
                }
            }
            LastCheck(isLogin);
        }
    }
    public void LastCheck(bool[] isLogin) // 패스워드랑 적은답과 맞는지 확인
    {
        bool result = false;
        for (int i = 0; i < isLogin.Length; i++)
        {
            if (isLogin[i])
            {
                result = true;
            }
            else if (!isLogin[i])
            {
                result = false;
                break;
            }
        }
        if (result)
        {
            transform.parent.gameObject.SetActive(false);
            locker.GetComponent<Animator>().SetBool("lockerOpen", true);
            locker.GetComponent<BoxCollider>().enabled = false;
            clearItem.SetActive(true);
        }
        else if (!result)
        {
            ClearSelect();
        }
    }
}
