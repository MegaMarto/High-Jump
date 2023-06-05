using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRespawn : MonoBehaviour
{
    [SerializeField]private Transform respawnPoint;
    [SerializeField]private Transform character;
    public float respawnHeight;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Respawn();
        
    }
    private void Respawn()
        {
        if (character.position.y <= respawnHeight)
        {
            character.position=respawnPoint.position;
        }
        }
}
