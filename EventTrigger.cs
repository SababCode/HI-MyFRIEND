using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger : MonoBehaviour
{
    public Animator Stone;
    public GameObject Effect;
    SoundManager ea;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Stone.SetBool("joinPlayer",true);
            Effect.SetActive(true);
            ea = transform.GetComponent<SoundManager>();
            ea.bgmPlayer = GameObject.Find("Around Effect Sound").GetComponent<AudioSource>();
            ea.ZombiOnce(0);
            if(this.gameObject.name.Equals("BossKill"))
            {
                this.gameObject.transform.GetComponent<BoxCollider>().enabled = false;
            }
        }
    }
}
