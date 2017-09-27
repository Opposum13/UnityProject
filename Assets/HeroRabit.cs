﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroRabit : MonoBehaviour {

    public float speed = 1;

    Rigidbody2D myBody = null;
    bool isGrounded = false;
    bool JumpActive = false;
    float JumpTime = 0f;
    public float MaxJumpTime = 2f;
    public float JumpSpeed = 2f;

    Transform heroParent = null;

    // Use this for initialization
    void Start() {
        LevelController.current.setStartPosition(transform.position);
        myBody = this.GetComponent<Rigidbody2D>();
        this.heroParent = this.transform.parent;
    }

    // Update is called once per frame
    void FixedUpdate() {
        //[-1, 1]
        float value = Input.GetAxis("Horizontal");
        Animator animator = GetComponent<Animator>();

        if (Mathf.Abs(value) > 0)
        {
            Vector2 vel = myBody.velocity;
            vel.x = value * speed;
            myBody.velocity = vel;
            if (this.isGrounded)
            {
                animator.SetBool("jump", false);
                animator.SetBool("run", true);
                animator.SetBool("idle", false);
            }
            else
            {
                animator.SetBool("jump", true);
                animator.SetBool("run", false);
                animator.SetBool("idle", false);
            }
        }
        else
        {
            
            if (this.isGrounded)
            {
                animator.SetBool("jump", false);
                animator.SetBool("run", false);
                animator.SetBool("idle", true);
            }
            else
            {
                animator.SetBool("jump", true);
                animator.SetBool("run", false);
                animator.SetBool("idle", false);
            }
        }


        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (value < 0)
        {
            sr.flipX = true;
            if (this.isGrounded)
            {
                animator.SetBool("jump", false);
                animator.SetBool("run", true);
                animator.SetBool("idle", false);
            }
            else
            {
                animator.SetBool("jump", true);
                animator.SetBool("run", false);
                animator.SetBool("idle", false);
            }

        }
        else if (value > 0)
        {
            sr.flipX = false;
            if (this.isGrounded)
            {
                animator.SetBool("jump", false);
                animator.SetBool("run", true);
                animator.SetBool("idle", false);
            }
            else
            {
                animator.SetBool("jump", true);
                animator.SetBool("run", false);
                animator.SetBool("idle", false);
            }
        }

        Vector3 from = transform.position + Vector3.up * 0.3f;
        Vector3 to = transform.position + Vector3.down * 0.1f;
        int layer_id = 1 << LayerMask.NameToLayer("Ground");
        //Перевіряємо чи проходить лінія через Collider з шаром Ground
        RaycastHit2D hit = Physics2D.Linecast(from, to, layer_id);
        if (hit)
        {
            isGrounded = true;

            //Перевіряємо чи ми опинились на платформі
            if (hit.transform != null
            && hit.transform.parent != null)
            {
                if (hit.transform.GetComponentInParent<MovingPlatform>() != null)
                {
                    //Приліпаємо до платформи
                    SetNewParent(this.transform, hit.transform.parent);
                }
            } 
        }
        else
        {
            isGrounded = false;
            SetNewParent(this.transform, this.heroParent);
        }
        //Намалювати лінію (для розробника)
        Debug.DrawLine(from, to, Color.red);


        //Якщо кнопка тільки що натислась
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            this.JumpActive = true;
        }
        if (this.JumpActive)
        {
            //Якщо кнопку ще тримають
            if (Input.GetButton("Jump"))
            {
                this.JumpTime += Time.deltaTime;
                if (this.JumpTime < this.MaxJumpTime)
                {
                    Vector2 vel = myBody.velocity;
                    vel.y = JumpSpeed * (1.0f - JumpTime / MaxJumpTime);
                    myBody.velocity = vel;
                }
            }
            else
            {
                this.JumpActive = false;
                this.JumpTime = 0;
            }
        }

        

    }
   


    // Update is called once per frame
    void Update () {
		
	}

    static void SetNewParent(Transform obj, Transform new_parent)
    {
        if (obj.transform.parent != new_parent)
        {
            //Засікаємо позицію у Глобальних координатах
            Vector3 pos = obj.transform.position;
            //Встановлюємо нового батька
            obj.transform.parent = new_parent;
            //Після зміни батька координати кролика зміняться
            //Оскільки вони тепер відносно іншого об’єкта
            //повертаємо кролика в ті самі глобальні координати
            obj.transform.position = pos;
        }
    }
}
