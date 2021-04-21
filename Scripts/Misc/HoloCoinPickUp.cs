using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HoloCoinPickUp : MonoBehaviour
{
    public float time = 0.5f;
    public int score;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag.Equals("Player"))
        {
            //Use ScoreCounter script and add 10 points
            ScoreCounter.scoreValue += score;

            //Destroy gameobject after colliding with player
            Destroy(gameObject);
        }
    }
}
