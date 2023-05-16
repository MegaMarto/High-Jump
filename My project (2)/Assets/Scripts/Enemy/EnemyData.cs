using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="EnemyData", menuName = "ScriptableObjects/Enemy Data" , order =1)]
public class EnemyData : ScriptableObject
{
    public float detectionRadius=10f;
    public float stopDistance=2f;
    public float avoidDistance=8f;
    public float lowHealthThreshold=30f;
    public float minDistanceFromOtherEnemies=2f;
}
