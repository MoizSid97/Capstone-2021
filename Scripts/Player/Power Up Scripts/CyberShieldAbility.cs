using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyberShieldAbility : MonoBehaviour
{
    public Transform spawn;

    public Transform parentObj;

    public GameObject shield;

    private bool used;
    private bool spawned;

    public AudioSource shieldSFX;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !used)
        {
            shieldSFX.Play();

            used = true;

            //Spawn shield
            GameObject _shield = Instantiate(shield, spawn.position, spawn.rotation);

            //Follow players positoin
            _shield.transform.parent = parentObj.transform;

            Destroy(_shield, 5f);
        }
        
    }
}
