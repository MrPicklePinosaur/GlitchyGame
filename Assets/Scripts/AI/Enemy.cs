﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool detected = false;
    public bool attack = false;
    public float attack_range;
    public float sight;
    
    
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        float dist=Mathf.Sqrt(gameObject.transform.position.sqrMagnitude+player.transform.position.sqrMagnitude);
        if (dist < sight)
        {
            detected = true;
        }
        else
        {
            detected = false;
            //just in case sight is greater than attack for some stupid reason
            attack = false;
        }

        if (dist < attack_range)
        {
            attack = true;
            
            


        }
        else
        {
            attack = false;
        }


    }
}
