
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController2D : MonoBehaviour
{
    public static PlayerController2D playercontroller;
    SpriteRenderer sp;
    Animator a;
    Rigidbody2D rb;
    
    
    [SerializeField] private float move = 4f;
    [SerializeField] private float jump = 4f;
    [SerializeField] private float fallmultiplier = 2.5f;
    [SerializeField] private float lowjumpmultiplier = 2f;
    [SerializeField] private BoxCollider2D stand;
    [SerializeField] private BoxCollider2D crouch;
    [SerializeField] private CircleCollider2D circle;
    [SerializeField] private int maxhealth=100;
    [SerializeField] private int currenthealth;
    [SerializeField] private HealthBar healthbar;
    [SerializeField] public Transform attackpoint;
    [SerializeField] private float attackrange = 0.3f;
    [SerializeField] private LayerMask enemylayer;
    [SerializeField] private GameObject menu;
    [SerializeField] private int lifecount;
    [SerializeField] private Text life;
    [SerializeField] private Respawn spawn;


    private float moveinput;
    private bool crouching;
    private bool jumping;
    public static PlayerController2D controller;
    private BoxCollider2D boxcollider2d;
    Vector2 position = new Vector2(13.5f, 0f);
    // Start is called before the first frame update
    void Start()
    {
        sp = this.GetComponent<SpriteRenderer>();
        rb = transform.GetComponent<Rigidbody2D>();
        a = this.GetComponent<Animator>();
        stand.enabled = true;
        crouch.enabled = false;
        circle.enabled = true;
        currenthealth = maxhealth;
        healthbar.setMaxHealth(maxhealth);
        controller = this;
        life.text = "X" + lifecount.ToString();
        menu.SetActive(false);
        if(playercontroller=null)
        {
            playercontroller = this;
        }
      
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        moveinput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector3(moveinput * move, rb.velocity.y);
        a.SetFloat("move", Mathf.Abs(moveinput));
        if (moveinput >0)
        {
            transform.rotation = Quaternion.identity;
            
        }
        else if(moveinput<0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            
        }    
        

        //JUMP
        //||-----------------------------------------------------------------------------------------------------------||
        if (Input.GetKey("space") & Mathf.Abs(rb.velocity.y) < 0.01f)
        {
            a.SetBool("jump", true);
            rb.velocity = new Vector2(rb.velocity.x, jump);
            jumping = true;
        }
        else
        {
            a.SetBool("jump", false);
            jumping = false;
        }
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity * (fallmultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetKey("space"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity * (lowjumpmultiplier - 1) * Time.deltaTime;
            a.SetBool("jump", false);
        }
        //ATTACK
        //||-----------------------------------------------------------------------------------------------------------||
        if (Input.GetKey("z"))
        {
            attack();
        }
        void attack()
        {
            a.SetTrigger("Attack");
           Collider2D[] hitenemies = Physics2D.OverlapCircleAll(attackpoint.position,attackrange,enemylayer);
            foreach(Collider2D enemy in hitenemies)
            {
                
                if(enemy.name=="Bandit")
                {
                    Debug.Log("We hit " + enemy.name);
                    enemy.GetComponent<EnemyGfx>().takedamage();
                }
                  
                
               
            }
        }
        //CROUCH
        //||-----------------------------------------------------------------------------------------------------------||
        if (Input.GetKey("c"))
        {
            a.SetBool("isCrouch", true);
            crouching = true;
        }
        else
        {
            a.SetBool("isCrouch", false);
            crouching = false;
        }
        if(jumping==true)
        {
            stand.enabled = true;
            crouch.enabled = false;
            circle.enabled = false;
        }else
        {
            if(crouching==true)
            {
                stand.enabled = false;
                crouch.enabled = true;
                circle.enabled = true;
            }
            else
            {
                stand.enabled = true;
                crouch.enabled = false;
                circle.enabled = true ;
            }
        }
     
       
    }
    private void Update()
    {
        //Damage Test//
        //||------------------------------------------------------------------------------------------||//
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(5);
            a.SetBool("isHurt", true);

        }
        else
        {
            a.SetBool("isHurt", false);
        }
    }
    
   public void TakeDamage(int damage)
    {
        
        currenthealth -= damage;
        
        healthbar.sethealth(currenthealth);
        if(currenthealth<0)
        {
            currenthealth = 0;
            LifeCounter();
            
        }
    }
   void LifeCounter()
    {
        if(lifecount==0)
        {
            dead();
        }
        else
        {
          
            lifecount--;
            Debug.Log(lifecount) ;
            life.text = "X" + lifecount.ToString();
            currenthealth = maxhealth;
          
        }
    }
  
    void dead()
    {
        a.SetTrigger("isDead");
        Destroy(this.gameObject);
        menu.SetActive(true);
    }
    void healthboost()
    {
        lifecount++;
        life.text = "X" + lifecount.ToString();
    }
    
}






