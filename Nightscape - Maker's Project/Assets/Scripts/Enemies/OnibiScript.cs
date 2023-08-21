using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class OnibiScript : MonoBehaviour
{
    #region Declarations
    [Header("Pathfinding")]
    public Transform target;
    private Path path;
    Seeker seeker;
    public float activateDistance = 50f;
    public float pathUpdateSeconds = 0.5f;
    private int currentWaypoint = 0;

    [Header("Physics")]
    public float speed = 500f;
    public float nextWaypointDistance = 3f;

    [Header("Custom Behavior")]
    public bool followEnabled = false;
    public bool directionLookEnabled = true;
    Rigidbody2D rb;
    public Animator anim;

    [Header("Combat")]
    public float maxHealth = 5f;
    public float currentHealth;
    private bool isDead = false;
    bool rush = false;

    public LayerMask playerLayer;
    public Transform attackPoint;

    public GameObject indicator;
    public Collider2D hitbox;
    public float attackRate = 0.25f;
    float nextAttackTime = 0f;

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
            // rb.AddForce((Vector2.up * (speed / 50)) + (Vector2.right * (speed / 50)), ForceMode2D.Impulse);
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
        // anim.SetBool("IsDead", true);
        // Die animation
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
        //coin.SetActive(true);
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
