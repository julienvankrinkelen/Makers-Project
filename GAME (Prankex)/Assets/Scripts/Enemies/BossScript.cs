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
    public float jumpCheckOffset = 0.1f;

    [Header("Custom Behavior")]
    public bool followEnabled = false;
    public bool directionLookEnabled = true;

    private Path path;
    private int currentWaypoint = 0;
    bool isGrounded = false;
    Seeker seeker;
    Rigidbody2D rb;
    public Animator anim;

    public float maxHealth = 20f;
    public float currentHealth;
    private bool Phaseone = false;
    private bool Phasetwo = false;
    private bool dancing = false;
    private bool isDead = false;

    public LayerMask playerLayer;
    public Transform playerTransform;
    public PlayerCombat playerCombat;
    public Collider2D playercollider;
    public GameObject Zone1;
    public GameObject Zone2;

    public float attackRange = 0.5f;
    public int attackDamage = 2;
    public float attackRate = 0.5f;
    float nextAttackTime = 0f;
    [SerializeField] private float DamageForce = 3;
    private bool dash = false;

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;

        InvokeRepeating(nameof(UpdatePath), 0f, pathUpdateSeconds);

    }

    private void FixedUpdate()
    {
        if (TargetInDistance() && followEnabled & Phaseone)
        {
            PathFollow1();
        }
        else if (Phasetwo & followEnabled & TargetInDistance())
        {
            PathFollow2();
        }
    }

    private void Update()
    {
        if (Vector2.Distance(playerTransform.position, transform.position) < 25 && anim.GetBool("IsDead") == false && playerCombat.CurrentHealth > 0 && !Phasetwo && !dancing)
        {
            Phaseone = true;

        }
        if (Vector2.Distance(playerTransform.position, transform.position) < 25 && anim.GetBool("IsDead") == false && playerCombat.CurrentHealth > 0)
        {
            followEnabled = true;

        }
        else if (Vector2.Distance(playerTransform.position, transform.position) > 50 && anim.GetBool("IsDead") == false)
        {
            followEnabled = false;
        }


        if ((Vector2.Distance(playerTransform.position, transform.position) < 25) && Time.time >= nextAttackTime && playerCombat.CurrentHealth > 0 && Phaseone && !Phasetwo && !dancing)
        {
            Attack1();
            dash = true;
            nextAttackTime = Time.time + 3f / attackRate;
        }
        else
        {
            dash = false;
        }

        if ((Vector2.Distance(playerTransform.position, transform.position) < 6) && Time.time >= nextAttackTime && playerCombat.CurrentHealth > 0 && !Phaseone && Phasetwo && !dancing)
        {
            dash = true;
            Attack2();
            nextAttackTime = Time.time + 4f / attackRate;
        }
        else
        {
            dash = false;
        }

        if ( currentHealth == 13)
        {
            StartCoroutine(Dance1());
        }

        if (currentHealth == 6)
        {
            StartCoroutine(Dance2());
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


        // Movement
        // rb.AddForce(force * Vector2.right, ForceMode2D.Force);

        if (directionLookEnabled == true)
        {
            if (direction.x < 0f)
            {
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (direction.x > -0f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }

        if (dash == true)
        {
            Debug.Log("Dashing");
            rb.AddForce(5 * force * Vector2.right, ForceMode2D.Impulse);
        }

        // Next Waypoint
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
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

        if (directionLookEnabled == true)
        {
            if (direction.x > 0f)
            {
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (direction.x < -0f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }

        if (dash == true)
        {
            Debug.Log("Dashing");
            rb.AddForce(force * Vector2.one * 5, ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(force * Vector2.one, ForceMode2D.Force);
        }
    }


    public void Attack1()
    {
        anim.SetTrigger("Attack1");
    }

    public void Attack2()
    {
        anim.SetTrigger("Attack2");
    }

    private IEnumerator Dance1()
    {
        dancing = true;
        transform.position = new Vector3 (-478, 116, 0);
        anim.SetBool("Dance1", true);
        yield return new WaitForSeconds(2);
        Zone1.SetActive(true);
        yield return new WaitForSeconds(3);
        Zone1.SetActive(false);
        yield return new WaitForSeconds(1);
        Zone2.SetActive(true);
        yield return new WaitForSeconds(3);
        Zone2.SetActive(false);
        yield return new WaitForSeconds(1);
        Zone1.SetActive(true);
        yield return new WaitForSeconds(3);
        Zone1.SetActive(false);
        yield return new WaitForSeconds(1);
        anim.SetBool("Dance1", false);

        // time for transition animation
        anim.SetBool("Transition", true);
        yield return new WaitForSeconds(3);
        anim.SetBool("Phasetwo", true);
        dancing = false;

    }

    private IEnumerator Dance2()
    {
        dancing = true;
        // transform.position == //middle of arena;
        anim.SetBool("Dance2", true);
        yield return new WaitForSeconds(2);
        Zone1.SetActive(true);
        yield return new WaitForSeconds(3);
        Zone1.SetActive(false);
        yield return new WaitForSeconds(1);
        Zone2.SetActive(true);
        yield return new WaitForSeconds(3);
        Zone2.SetActive(false);
        yield return new WaitForSeconds(1);
        Zone1.SetActive(true);
        yield return new WaitForSeconds(3);
        Zone1.SetActive(false);
        yield return new WaitForSeconds(1);
        anim.SetBool("Dance2", false);
        dancing = false;
    }


    public void TakeDamage(float damage)
    {
        if (!isDead || !dancing)
        {
            currentHealth -= damage;
            Debug.Log("Enemy health : " + currentHealth);
            // Hurt animation
            anim.SetTrigger("Hurt");
            rb.AddForce((Vector2.up * DamageForce) + (Vector2.right * DamageForce), ForceMode2D.Impulse);
        }
        if (currentHealth <= 0)
        {
            StartCoroutine(FightEnd());
        }
    }



    private IEnumerator FightEnd()
    {
        followEnabled = false;
        anim.SetBool("IsDead", true);
        isDead = true;
        // Die animation
        yield return new WaitForSeconds(1);
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
