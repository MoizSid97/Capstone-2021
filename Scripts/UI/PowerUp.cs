using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUp : MonoBehaviour
{
    public GameObject shieldImage;
    //public GameObject pickUpEffect;

    public float time = 5f;
    public AudioSource sfx;

    // Start is called before the first frame update
    void Start()
    {
        shieldImage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //Add Audio
            sfx.Play();

            //Enable UI
            shieldImage.SetActive(true);

            //Do Powerup stuff
            StartCoroutine (PickUp(other));
        }
    }

    IEnumerator PickUp(Collider player)
    {
        //Add Particle System
        //Instantiate(pickUpEffect, transform.position, transform.rotation);

        //Adding powerups
        //Access this script
        CyberShieldAbility ability = player.GetComponent<CyberShieldAbility>();
        ability.enabled = true;

        //Disable mesh and collision
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;

        //Wait set amount of time
        yield return new WaitForSeconds(time);

        //Disable powerup
        ability.enabled = false;

        //Destroy gameobject after colliding with player
        Destroy(gameObject);
    }
    
}
