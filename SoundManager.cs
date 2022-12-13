using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Sound
{
    public string soundName;
    public AudioClip clip;
}
public class SoundManager : MonoBehaviour
{
    [Header("사운드 등록")]
    [SerializeField] Sound[] EffectSound;

    [Header("브금 플레이어")]
    [SerializeField]public AudioSource bgmPlayer;

    public void ZombiRandomAttack(int min, int max)
    {
        int random = Random.Range(min, max);
        bgmPlayer.clip = EffectSound[random].clip;
        bgmPlayer.PlayOneShot(bgmPlayer.clip);
    }
    public void ZombiOnce(int i)
    {
        bgmPlayer.clip = EffectSound[i].clip;
        bgmPlayer.PlayOneShot(bgmPlayer.clip);
    }
    public void PlayerRandomAttack(int min, int max)
    {
        int random = Random.Range(min, max);
        bgmPlayer.clip = EffectSound[random].clip;
        bgmPlayer.Play();
    }
    public void PlayerOnce(int i)
    {
        bgmPlayer.clip = EffectSound[i].clip;
        bgmPlayer.Play();
    }
    public void BGMSoundPlay(int i)
    {
        bgmPlayer.clip = EffectSound[i].clip;
        bgmPlayer.Play();
    }
    public void PlayerOnceStop()
    {
        bgmPlayer.Stop();
    }
    public bool ConditionMusic(string name)
    {
        bool ischeck = false;
        if(bgmPlayer.clip.name.Equals(name))
        {
            ischeck = true;
        }
        else if(bgmPlayer.clip.name != name)
        {
            ischeck = false;
        }
        return ischeck;
    }
}
