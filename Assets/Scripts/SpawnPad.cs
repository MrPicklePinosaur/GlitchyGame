﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPad : MonoBehaviour {

    public GameObject[] spawn_pool; //list of possible enemies that could be spawned here

    void Start() {
        //choose a random enemy to spawn+
        GameObject new_enemy = spawn_pool[Random.Range(0, spawn_pool.Length)];
        Instantiate(new_enemy, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    void Update() { }
}
