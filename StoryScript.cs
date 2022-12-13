using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class StoryScript : MonoBehaviour
{
    public GameObject[] storyObj = new GameObject[4];
    public GameObject[] firsttxt;
    public GameObject[] secontxt;
    public GameObject[] thirdtxt;
    public GameObject[] fourthtxt;
    int storynum = 0;
    int detailstory = 0;
    // Start is called before the first frame update
    void Awake()
    {
        for(int i = 0; i < storyObj.Length; i++)
        {            
            int count = storyObj[i].transform.childCount;
            for (int j = 0; j < count; j++)
            {
                switch(i)
                {
                    case 0:
                        firsttxt = new GameObject[count];
                        for (int f = 0; f < count; f++)
                        {
                            firsttxt[f] = storyObj[i].transform.GetChild(f).gameObject;
                            firsttxt[f].SetActive(false);
                        }
                        break;
                    case 1:
                        secontxt = new GameObject[count];
                        for (int f = 0; f < count; f++)
                        {
                            secontxt[f] = storyObj[i].transform.GetChild(f).gameObject;
                            secontxt[f].SetActive(false);
                        }
                        break;
                    case 2:
                        thirdtxt = new GameObject[count];
                        for (int f = 0; f < count; f++)
                        {
                            thirdtxt[f] = storyObj[i].transform.GetChild(f).gameObject;
                            thirdtxt[f].SetActive(false);
                        }
                        break;
                    case 3:
                        fourthtxt = new GameObject[count];
                        for (int f = 0; f < count; f++)
                        {
                            fourthtxt[f] = storyObj[i].transform.GetChild(f).gameObject;
                            fourthtxt[f].SetActive(false);
                        }
                        break;
                }
                storyObj[i].SetActive(false);
            }
        }
    }
    private void Start()
    {
        storyObj[0].SetActive(true);
        firsttxt[0].SetActive(true);
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            detailstory++;
            if(storynum.Equals(0))
            {
                if (detailstory < firsttxt.Length)
                {
                    if (detailstory != 0)
                    {
                        firsttxt[detailstory - 1].SetActive(false);
                    }

                    firsttxt[detailstory].SetActive(true);
                    storyObj[storynum].SetActive(true);
                }
                else if (detailstory >= firsttxt.Length)
                {
                    firsttxt[detailstory-1].SetActive(false);
                    storyObj[storynum].SetActive(false);
                    storynum++;
                    detailstory = 0;
                }
            }
            if (storynum.Equals(1))
            {
                if (detailstory < secontxt.Length)
                {
                    if (detailstory != 0)
                    {
                        secontxt[detailstory - 1].SetActive(false);
                    }
                    secontxt[detailstory].SetActive(true);
                    storyObj[storynum].SetActive(true);
                }
                else if (detailstory >= secontxt.Length)
                {
                    secontxt[detailstory - 1].SetActive(false);
                    storyObj[storynum].SetActive(false);
                    storynum++;
                    detailstory = 0;
                }
            }
            if (storynum.Equals(2))
            {
                if (detailstory < thirdtxt.Length)
                {
                    if (detailstory != 0)
                    {
                        thirdtxt[detailstory - 1].SetActive(false);
                    }
                    thirdtxt[detailstory].SetActive(true);
                    storyObj[storynum].SetActive(true);
                }
                else if (detailstory >= firsttxt.Length)
                {
                    thirdtxt[detailstory - 1].SetActive(false);
                    storyObj[storynum].SetActive(false);
                    storynum++;
                    detailstory = 0;
                }
            }
            if (storynum.Equals(3))
            {
                if (detailstory < fourthtxt.Length)
                {
                    if (detailstory != 0)
                    {
                        fourthtxt[detailstory - 1].SetActive(false);
                    }

                    fourthtxt[detailstory].SetActive(true);
                    storyObj[storynum].SetActive(true);
                }
                else if (detailstory >= firsttxt.Length)
                {
                    fourthtxt[detailstory - 1].SetActive(false);
                    storyObj[storynum].SetActive(false);
                    storynum++;
                    detailstory = 0;
                    if (!Ending.endgame)
                    {
                        DataController.instance.nowPlayer.Scenenum = 2;
                        DataController.instance.SaveData();
                        SceneManager.LoadScene(2);
                    }
                    else if(Ending.endgame)
                    {
                        Ending.endgame = false;
                        SceneManager.LoadScene(0);
                    }
                }
            }
        }
    }
}