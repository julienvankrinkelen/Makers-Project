using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BossScript : MonoBehaviour
{

    [Header("Pathfinding")]
    public Transform target;
    public float activateDistance = 50f;
    public float pathUpdateSeconds = 0.5f;

    [Header("Physics")]
    public float speed = 8f;
    [SerializeField] private float acceleration = 13;
    [SerializeField] private float deceleration = 16;
    [SerializeField] private float velPower = 0.96f;
    public float nextWaypointDistance = 3f;
    public float jumpNodeHeightRequirement = 0.8f;
    public float jumpModifier = 0.5f;
    public float jumpCheckOffset = 0.1f;

    [Header("Custom Behavior")]
    public bool followEnabled = false;
    public bool jumpEnabled = false;
    public bool directionLookEnabled = true;

    private Path path;
    private int currentWaypoint = 0;
    bool isGrounded = false;
    Seeker seeker;
    Rigidbody2D rb;
    Animator anim;

    public float maxHealth = 2f;
    public float currentHealth;
    private bool Phaseone = false;
    private bool Phasetwo = false;
    private bool isDead = false;

    public LayerMask playerLayer;
    public Transform attackPoint;
    public Transform playerTransform;
    public PlayerCombat playerCombat;
    public Collider2D playercollider;

    public float attackRange = 0.5f;
    public int attackDamage = 2;
    public float attackRate = 0.5f;
    float nextAttackTime = 0f;
    [SerializeField] private float DamageForce = 13;
    private bool dash = false;

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;

        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);

    }

    private void FixedUpdate()
    {
        if (TargetInDistance() && followEnabled & Phaseone)
        {
            PathFollow1();
        }
        else if (Phaseone & !followEnabled)
        {
            anim.SetBool("Run", false);
        }
        else if (Phasetwo & followEnabled & TargetInDistance())
        {
            PathFollow2();
        }
    }

    private void Update()
    {
        if (Vector2.Distance(playerTransform.position, attackPoint.position) < 10 && anim.GetBool("IsDead") == false && playerCombat.CurrentHealth > 0)
        {
            followEnabled = true;
            anim.SetBool("CombatMode", true);

        }
        else if (Vector2.Distance(playerTransform.position, attackPoint.position) > 50 && anim.GetBool("IsDead") == false)
        {
            followEnabled = false;
            anim.SetBool("CombatMode", false);
        }


        if ((Vector2.Distance(playerTransform.position, attackPoint.position) < 6) && Time.time >= nextAttackTime && playerCombat.CurrentHealth > 0 && Phaseone)
        {
            Attack1();
            dash = true;
            nextAttackTime = Time.time + 4f / attackRate;
        }
        else
        {
            dash = false;
        }

    }

    private void UpdatePath()
    {
        if (followEnabled && TargetInDistance() && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);

        }
    }

    private void PathFollow1()
    {
        if (path == null)
        {
            return;
        }

        // Reached end of path
        if (currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }

        // See if colliding with anything
        isGrounded = Physics2D.Raycast(transform.position, -Vector3.up, GetComponent<Collider2D>().bounds.extents.y + jumpCheckOffset);

        // Direction calculation
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        // Vector2 force = direction * speed * Time.deltaTime;
        float targetSpeed = direction.x * speed;
        float speedDif = targetSpeed - rb.velocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
        float force = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);



        // Jump
        if (jumpEnabled == true && isGrounded)
        {
            if (direction.y > jumpNodeHeightRequirement)
            {
                rb.AddForce(Vector2.up * force * jumpModifier, ForceMode2D.Impulse);
            }
        }

        // Movement
        rb.AddForce(force * Vector2.right, ForceMode2D.Force);

        // Next Waypoint
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if (directionLookEnabled == true)
        {
            if (direction.x > 0f)
            {
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                anim.SetBool("Run", true);
            }
            else if (direction.x < -0f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                anim.SetBool("Run", true);
            }
        }

        if (dash == true)
        {
            Debug.Log("Rushing");
            rb.AddForce(force * Vector2.right * 5, ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(force * Vector2.right , ForceMode2D.Force);
        }

    }

    private void PathFollow2()
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

    public void Attack1()
    {

        anim.SetTrigger("Attack1");

    }
    public void OnTriggerEnter2D(Collider2D Collider2D)
    {
        if (Collider2D.tag == "Player")
        {
            Collider2D.GetComponent<PlayerCombat>().TakeDamage(attackDamage);
        }

    }

    public void TakeDamage(float damage)
    {
        if (!isDead)
        {
            currentHealth -= damage;
            Debug.Log("Enemy health : " + currentHealth);
            // Hurt animation
            anim.SetTrigger("Hurt");
            rb.AddForce((Vector2.up * DamageForce) + (Vector2.right * DamageForce), ForceMode2D.Impulse);
        }
        if (currentHealth <= 0)
        {
            StartCoroutine(Bunshin());
        }
    }



    private IEnumerator Bunshin()
    {
        GetComponent<EnemyScript>().enabled = false;
        // anim.SetBool("IsDead", true);
        // Die animation
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
        //coin.SetActive(true);
    }


}
