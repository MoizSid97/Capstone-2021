using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableBossFight : MonoBehaviour
{
    public GameObject barrier;

    // Start is called before the first frame update
    void Start()
    {
        barrier.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            barrier.SetActive(true);
        }
    }
}
