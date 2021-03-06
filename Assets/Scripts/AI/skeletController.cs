﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class skeletController : MonoBehaviour {

    private Animator anim;
    private Rigidbody2D body;
    private weaponSheath weapon_sheath;
    private bool isMoving = false; //start off in idle state
    private Stats _stats;

    //AI vars
    private float moveDirect; //an angle measurement, in radians
    private float idleCounter = 0; //how long the skelet will stay idle
    private float moveCounter = 0; //how long the skelet will wander for

    void Start() {
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        weapon_sheath = GetComponent<weaponSheath>();
        _stats = GetComponent<Stats>();
    }

    void Update() {

        //update counters
        if (idleCounter <= 0 && !isMoving) { // if no longer idle, decide new direction to walk in
            moveDirect = (float)(Random.Range(0, 360) * (Math.PI / 180));
            moveCounter = Random.Range(2,4); //decide random wander time
            isMoving = true;
        }
        if (moveCounter <= 0 && isMoving) {
            idleCounter = Random.Range(1, 5); //HARDCODED FOR NOW
            isMoving = false;
        }

        idleCounter -= Time.deltaTime;
        moveCounter -= Time.deltaTime;

        body.velocity = new Vector2(0, 0);
        //Check to see if there is actually input
        if (isMoving) {
            body.velocity = new Vector2((float)(_stats.move_speed*Math.Cos(moveDirect)), (float)(_stats.move_speed*Math.Sin(moveDirect)));
        }

        weapon_sheath.weapon_angle = moveDirect; //point weapon in direction of movement for now

        float facing = Math.Cos(moveDirect) < 0 ? -1 : 1;
        anim.SetFloat("Facing", facing); //update anim
        anim.SetBool("isMoving", isMoving);
    }
}
