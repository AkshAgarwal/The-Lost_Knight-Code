using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyGfx : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    Animator a;
    [SerializeField] private float jump = 7f;
    [SerializeField] private GameObject enemy;
    [SerializeField] private int enemyhealth, maxhealth = 100;
    [SerializeField] private RaycastHit2D hit;
    [SerializeField] private GameObject raypoint;
    [SerializeField] private float distance;
    [SerializeField] private LayerMask layermask;
    [SerializeField] private LayerMask GroundLayer;
    public bool mustPatrol;
    public float movespeed;
    [SerializeField] private Transform groundcheckpos;
    public Collider2D bodycollider;
    [SerializeField] private bool mustflip;
    public static EnemyGfx eg;
    private void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        a = this.GetComponent<Animator>();
        enemyhealth = maxhealth;
        distance = 1.2f;
        mustPatrol = true;
        movespeed = 100f;
        if(eg==null)
        {
            eg = this;
        }
    }
    // Start is called before the first frame update
    private void Update()
    {
        if (mustPatrol)
        {
            Patrol();

        }
        hit = Physics2D.Raycast(raypoint.transform.position, raypoint.transform.forward, distance, layermask);
        Debug.DrawRay(raypoint.transform.position, raypoint.transform.forward * distance, Color.yellow);
        if (hit)
        {
            Debug.Log(hit.collider.tag);
            if (hit.collider.name == "Player")
            {
                mustPatrol = false;
                Debug.Log(hit.collider.name);
                a.SetTrigger("isAttacking");

            }
            if (hit.collider.tag == "Obstacle")
            {
                flip();
            }
        }



    }
    private void FixedUpdate()
    {
        if (mustPatrol)
        {
            mustflip = !Physics2D.OverlapCircle(groundcheckpos.position, 0.1f, GroundLayer);
            // Debug.Log(mustflip);
        }
    }

    public void takedamage()
    {
        enemyhealth -= 5;

        if (enemyhealth < 0)
        {
            enemyhealth = 0;
            Destroy(this.gameObject);
        }
    }
    void Patrol()
    {
        if (mustflip)
        {
            flip();
        }
        else
        {
            rb.velocity = new Vector2(-movespeed * Time.fixedDeltaTime, rb.velocity.y);
            a.SetFloat("move", Mathf.Abs(movespeed));
        }

        //Debug.Log("Moving");
    }
    public void flip()
    {
        mustPatrol = false;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        movespeed *= -1;
        mustPatrol = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {

            mustPatrol = false;
            flip();
            mustPatrol = true;
        }
    }
}
