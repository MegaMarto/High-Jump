using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamage : MonoBehaviour
{
    public int damage = 10; // The amount of damage this bullet does
     private void OnCollisionEnter(Collision collision)
    {
        CharacterHealth character = collision.gameObject.GetComponent<CharacterHealth>();
        if (character != null)
        {
            character.TakeDamage(damage);
            Destroy(gameObject); // Destroy the bullet after it hits the character
        }
        Destroy(gameObject); 
    }
}
