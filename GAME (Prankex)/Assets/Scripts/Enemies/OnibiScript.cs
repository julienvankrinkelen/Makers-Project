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
    public float maxHealth = 2;
    float currentHealth;
    private bool isDead = false;
    bool rush = false;

    public LayerMask playerLayer;
    public Transform attackPoint;

    public float attackRange = 1f;
    public float attackDamage = 1;
    public float attackRate = 5f;
    float nextAttackTime = 0f;

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
            Rush();
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

    public void Rush()
    {
        
        // check for player in range in the layer assigned
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("You have been hit");
            enemy.GetComponent<PlayerCombat>().TakeDamage(attackDamage);
        }

    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void TakeDamage(float damage)
    {
        if (!isDead)
        {
            currentHealth -= damage;
            Debug.Log("Enemy health : " + currentHealth);

            // Hurt animation
            anim.SetTrigger("Hurt");
            rb.AddForce((Vector2.up * (speed / 50)) + (Vector2.right * (speed / 50)), ForceMode2D.Impulse);
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
            Debug.Log("Rushing");
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
