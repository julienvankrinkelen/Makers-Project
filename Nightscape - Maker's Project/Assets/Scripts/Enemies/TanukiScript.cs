using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TanukiScript : MonoBehaviour
{
    [SerializeField] private MobsState mobsState;
    [SerializeField] private GameObject[] waypoints;
    private int currentWaypointIndex = 0;
    [SerializeField] private Collider2D playercollider;

    [SerializeField] private float speed = 1.5f;
    private bool isFacingTheRight;
    private bool IsDead;
    private bool MJing;
    [SerializeField] private Animator anim;

    public float maxHealth = 1;
    private float currentHealth;

    [SerializeField] private GameObject scrollDrop;

    [SerializeField] private AudioSource DieSoundEffect;


    private void Start()
    {
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), playercollider);
        IsDead = false;
        currentHealth = maxHealth;
        scrollDrop.SetActive(false);
    }
    private void Update()
    {
        if (Vector2.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < 0.1)
        {
            currentWaypointIndex++;
            
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, Time.deltaTime * speed);

        Flip();

        scrollDrop.transform.position = transform.position;
    }
    
    private void Flip()
    {
        if (Vector2.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < 0.1  && isFacingTheRight && !MJing)
        {
            Vector3 localScale = transform.localScale;
            localScale.x = 1f;
            transform.localScale = localScale;
            isFacingTheRight = false;
            StartCoroutine(StopMJ());
        }
        else if (Vector2.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < 0.1 && !isFacingTheRight && !MJing)
        {
            Vector3 localScale = transform.localScale;
            localScale.x = -1f;
            transform.localScale = localScale;
            isFacingTheRight = true;
            StartCoroutine(StopMJ());
        }
    }
    public void TakeDamage(float damage)
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
        mobsState.dieTanuki(gameObject);
        GetComponent<TanukiScript>().enabled = false;
        anim.SetBool("IsDead", true);
        DieSoundEffect.Play();
        // Die animation
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
        scrollDrop.SetActive(true);
    }

    private IEnumerator StopMJ()
    {
        MJing = true;
        yield return new WaitForSeconds(1);
        MJing = false;
    }
}
