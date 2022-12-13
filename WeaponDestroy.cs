using UnityEngine;

public class WeaponDestroy : MonoBehaviour
{
    public int code;
    private void Awake()
    {
        // 아이템을 줍고 세이브시 젠 안됌
        for(int i = 0; i < DataController.instance.nowPlayer.GetItemCode.Count; i++)
        {
            if (DataController.instance.nowPlayer.GetItemCode[i].Equals(code))
            {
                Destroy(this.gameObject);
                return;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("DestroyObj"))
        {
            this.gameObject.GetComponent<BoxCollider>().isTrigger = false;
            Destroy(this.gameObject, 0.05f);
        }        
    }
}