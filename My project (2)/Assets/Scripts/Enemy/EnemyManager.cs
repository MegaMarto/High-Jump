using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
  [SerializeField] private List<EnemyAI> enemyList = new List<EnemyAI>();

    private void Awake()
    {
        // Find all EnemyAI objects in the scene and add them to the enemyList
        enemyList.AddRange(FindObjectsOfType<EnemyAI>());
    }

}
