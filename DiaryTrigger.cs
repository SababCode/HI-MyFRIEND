using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiaryTrigger : MonoBehaviour
{
    public int dirayNum = 0;
    void Awake()
    {
        switch(this.name)
        {
            case "일기장1":
            dirayNum = 1;
            break; 
            case "일기장2":
            dirayNum = 2;
            break;
            case "일기장3":
            dirayNum = 3;
            break;
            case "일기장4":
            dirayNum = 4;
            break;
            case "일기장5":
            dirayNum = 5;
            break;
            case "일기장6":
            dirayNum = 6;
            break;
            case "일기장7":
            dirayNum = 7;
            break;
            case "일기장8":
            dirayNum = 8;
            break;
            case "일기장9":
            dirayNum = 9;
            break;
        }
    }
}
