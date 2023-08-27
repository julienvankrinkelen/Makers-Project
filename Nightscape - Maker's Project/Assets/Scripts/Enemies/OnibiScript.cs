using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class OnibiScript : MonoBehaviour
{
    #region Declarations
    [Header("Pathfinding")]
    [SerializeField] private Transform target;
    private Path path;
    Seeker seeker;
    private float activateDistance = 50f;
    private float pathUpdateSeconds = 0.5f;
    private int currentWaypoint = 0;

    [Header("Physics")]
    private float speed = 500f;
    private float nextWaypointDistance = 3f;

    [Header("Custom Behavior")]
    private bool followEnabled = false;
    private bool directionLookEnabled = true;
    Rigidbody2D rb;
    [SerializeField] private Animator anim;

    [Header("Combat")]
    public float maxHealth = 5f;
    public float currentHealth;
    private bool isDead = false;
    bool rush = false;

    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Transform attackPoint;

    [SerializeField] private GameObject indicator;
    [SerializeField] private Collider2D hitbox;
    private float attackRate = 0.25f;
    private float nextAttackTime = 0f;

    [SerializeField] private AudioSource dieSoundEffect;
    [SerializeField] private AudioSource hurtSoundEffect;
    #endregion

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);

        currentHealth = maxHealth;

    }

    private void Update()
    {
        if (Vector2.Distance(target.position, attackPoint.position) < 10 && anim.GetBool("IsDead") == false && !isDead)
        {
            followEnabled = true;
        }

        if ((Vector2.Distance(target.position, attackPoint.position) < 5) && Time.time >= nextAttackTime)
        {
            rush = true;
            StartCoroutine(Rush());
            nextAttackTime = Time.time + 1f / attackRate;
        }

        else
        {
            rush = false;
        }

        if (followEnabled)
        {
            PathFollow();

        }
    }

    private IEnumerator Rush()
    {
        hitbox.enabled = true;
        indicator.SetActive(true);
        yield return new WaitForSeconds(1);
        hitbox.enabled = false;
        indicator.SetActive(false);
    }


    public void TakeDamage(float damage)
    {
        if (!isDead)
        {
            currentHealth -= damage;
            Debug.Log("Enemy health : " + currentHealth);

            // Hurt animation
            anim.SetTrigger("Hurt");
            hurtSoundEffect.Play();
        }
        if (currentHealth <= 0)
        {
            Die();

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Traps"))
        {
            TakeDamage(1);
        }
        if (collision.gameObject.CompareTag("Juice"))
        {
            TakeDamage(2);
        }
    }

    void Die()
    {
        // Die animation
        anim.SetBool("IsDead", true);
        dieSoundEffect.Play();
        // Disable the enemy
        isDead = true;

        Debug.Log("Onibi died!");
        StartCoroutine(Bunshin());

    }

    private IEnumerator Bunshin()
    {
        GetComponent<OnibiScript>().enabled = false;
        // Die animation
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }


    private void UpdatePath()
    {
        if (followEnabled && TargetInDistance() && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    private void PathFollow()
    {
        if (path == null)
        {
            return;
        }

        // Checking if currentWaypoint is within the range
        if (currentWaypoint < 0 || currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }

        // Reached end of path
        if (currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }

        // Direction calculation
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        if (rush == true)
        {
            rb.AddForce(force * 5, ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(force, ForceMode2D.Force);
        }

        // Next Waypoint
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

    }

    private bool TargetInDistance()
    {
        return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
}
