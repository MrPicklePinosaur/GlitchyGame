﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponController : MonoBehaviour {

    [Range(0f, 200f)] public float melee_damage; //damage dealt when weapon hits entity
    public GameObject projectile;

    public GameObject owner;
    public bool isEnemy;

    [Header("Graphics")]
    [Range(-3f, 3f)] public float orbit_radius;
    [Range(-1f, 1f)] public float follow_offset_x;
    [Range(-1f, 1f)] public float follow_offset_y;
    [Range(0f, 360f)] public float angle_offset; //in degrees
    [Range(0f, 300f)] public float mass;

    private GameObject pickup;
    private Rigidbody2D body;

    void Awake() {
        body = GetComponent<Rigidbody2D>();
        //owner
        //this is giving error for some reason
        //Physics.IgnoreCollision(owner.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
        orbit_radius = 0.8f;

        if (owner != null) { set_pickup_mode(owner);
        } else if(!isEnemy){ set_drop_mode(); }
        
        
    }

    void Update() {

        if (owner != null) {
            

            float weapon_angle = owner.GetComponent<weaponSheath>().weapon_angle; //change this to be more generic so it works with enemies too

            //rotate  weapon depending on where mouse is
            //float zrot = (float) Mathf.Lerp(transform.rotation.z,(float)(weapon_angle * 180 / Math.PI + angle_offset),mass);
            transform.rotation = Quaternion.Euler((transform.rotation.x * 180 / Mathf.PI), (transform.rotation.y * 180 / Mathf.PI), (weapon_angle * 180 / Mathf.PI + angle_offset));

            //weapon 'orbits' owner
            //HARDCODED ORBIT RAIDUS AS 1 FOR NOW
            float new_x = (float)(owner.transform.position.x + 1 * Mathf.Cos(weapon_angle) + follow_offset_x);
            float new_y = (float)(owner.transform.position.y + 1 * Mathf.Sin(weapon_angle) + follow_offset_y);
            transform.position = new Vector3(new_x, new_y, owner.transform.position.z);


        }
        else if(isEnemy)
        {
            
            
        }
        else { //if weapon does not have an owner, display it as a dropped item
            transform.Rotate(0, 4f, 0); //cheesy rotating effect
        }
        
        

    }

    public void shootProjectile() { //create a new projectile object
        projectileController p = Instantiate(projectile, transform.position, transform.rotation).GetComponent<projectileController>();
        p.angle = owner.GetComponent<weaponSheath>().weapon_angle; //set the angle the bullet travels at
    }

    public void set_pickup_mode(GameObject new_owner) { //if the weapon is being used
        owner = new_owner;

        //set equipped item as this weapon
        new_owner.GetComponent<weaponSheath>().equiped_weapon = this.gameObject;

        gameObject.layer = LayerMask.NameToLayer("Weapons");
        //body.isKinematic = true;
        body.freezeRotation = false;

        Destroy(pickup); //remove pickup radius thingy
    }

    public void set_drop_mode() { //if the weapon is no longer being used
        owner = null;

        //dropped items are rendered differently and arent subject to physics
        gameObject.layer = LayerMask.NameToLayer("Dropped Items");
        //body.isKinematic = false; //just make sure there is no force when the item is dropped
        body.freezeRotation = true;
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0f);

        //create an object that detects if an entity is within the pickup range
        pickup = Instantiate(Resources.Load("Detectors/weapon_pickup_detector") as GameObject, transform.position, Quaternion.identity);
        pickup.transform.parent = transform; //make pickup detector as child

    }


    private void OnCollisionEnter2D(Collision2D other) { //if the weapon hits an object
        

        Stats hp_comp = other.gameObject.GetComponent<Stats>();
        if (hp_comp != null) { //if the object the projectile hit has a health bar
            hp_comp.modHp(-1 * melee_damage);
        }
    }
}