using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyStone : MonoBehaviour
{
    public GameObject sto;
    public GameObject block;
    void Awake()
    {
        for(int i = 0; i < DataController.instance.nowPlayer.StoryCode.Count; i++)
        {
            if(DataController.instance.nowPlayer.StoryCode[i].Equals(50010))
            {
                Des();
            }
        }
    }
    public void Des()
    {
        StartCoroutine(des());
    }
    IEnumerator des()
    {
        block.SetActive(true);
        Destroy(sto);
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }
}
