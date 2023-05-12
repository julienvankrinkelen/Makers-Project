using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TanukiScript : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    private int currentWaypointIndex = 0;
    public Collider2D playercollider;

    [SerializeField] private float speed = 1.5f;
    private bool isFacingTheRight;
    private bool IsDead;
    public Animator anim;

    public int maxHealth = 1;
    int currentHealth;

    // public GameObject coin;


    private void Start()
    {
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), playercollider);
        // coin.SetActive(false);
        IsDead = false;
        currentHealth = maxHealth;
    }
    private void Update()
    {
        if (Vector2.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < .1f)
        {
            currentWaypointIndex++;
            
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, Time.deltaTime * speed);

        Flip();
    }

    private void Flip()
    {
        if (Vector2.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < .1f  && isFacingTheRight)
        {
            Vector3 localScale = transform.localScale;
            localScale.x = 1f;
            transform.localScale = localScale;
            isFacingTheRight = false;
        }
        else if (Vector2.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < .1f && !isFacingTheRight)
        {
            Vector3 localScale = transform.localScale;
            localScale.x = -1f;
            transform.localScale = localScale;
            isFacingTheRight = true;
        }
    }
    public void TakeDamage(int damage)
    {
        if (!IsDead)
        {
            currentHealth -= damage;
        }
        if (currentHealth <= 0)
        {
            Disappear();
        }
    }
    void Disappear()
    {
            Debug.Log("Tanuki flew!");
            // Disable the enemy
            IsDead = true;
            StartCoroutine(Bunshin());
    }

    private IEnumerator Bunshin()
    {
        GetComponent<TanukiScript>().enabled = false;
        anim.SetBool("IsDead", true);
        // Die animation
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
        //coin.SetActive(true);
    }
}
