using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))  //Player를 타격
        {
            PlayerMove.currentHp -= Enemy.Enemydamage - PlayerMove.defense;
        }
    }
}