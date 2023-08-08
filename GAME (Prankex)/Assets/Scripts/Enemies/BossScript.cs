using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BossScript : MonoBehaviour
{

    [Header("Pathfinding")]
    public Transform target;
    public float activateDistance = 50f;
    public float pathUpdateSeconds = 0.2f;

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

    public float maxHealth = 40f;
    public float currentHealth;
    private bool attackok = false;
    private bool Phaseone = false;
    private bool Phasetwo = false;
    private bool dancing = false;
    private bool dancetwodone = false;
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
    private bool dash = false;

    [SerializeField] private AudioSource dieSoundEffect;
    [SerializeField] private AudioSource transiSoundEffect;
    [SerializeField] private AudioSource incantSoundEffect;
    [SerializeField] private AudioSource thunderSoundEffect;
    [SerializeField] private AudioSource attackSoundEffect;

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;

        InvokeRepeating(nameof(UpdatePath), 0f, pathUpdateSeconds);

    }
    
    private void FixedUpdate()
    {
        if (Vector2.Distance(playerTransform.position, transform.position) < 25 && anim.GetBool("IsDead") == false && playerCombat.CurrentHealth > 0 && !Phaseone && !Phasetwo && !dancing)
        {
            StartCoroutine(StartFight());
        }

        if (TargetInDistance() && followEnabled && Phaseone && !Phasetwo)
        {
            PathFollow1();
        }
        if (Phasetwo && followEnabled && TargetInDistance())
        {
            PathFollow2();
        }

        if (currentHealth <= 24 && !Phasetwo && !dancing)
        {
            StartCoroutine(Dance1());
        }

        if (currentHealth <= 12 && !dancing && !dancetwodone)
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
        if ((Vector2.Distance(playerTransform.position, transform.position) <= 50) && Time.time >= nextAttackTime && playerCombat.CurrentHealth > 0  && !dancing && attackok)
        {
            dash = true;
            Attack1();
            Debug.Log("Dashnow");
            nextAttackTime = Time.time + 1f / attackRate;
        }
        else
        {
            dash = false;
        }


        if (dash == true)
        {
            Debug.Log("Dashing");
            rb.AddForce(force * Vector2.right, ForceMode2D.Impulse);
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
        
        float targetSpeedx = direction.x * speed;
        float speedDifx = targetSpeedx - rb.velocity.x;
        float accelRatex = (Mathf.Abs(targetSpeedx) > 0.01f) ? acceleration : deceleration;
        float forcex = Mathf.Pow(Mathf.Abs(speedDifx) * accelRatex, velPower) * Mathf.Sign(speedDifx);

        float targetSpeedy = direction.y * speed;
        float speedDify = targetSpeedy - rb.velocity.y;
        float accelRatey = (Mathf.Abs(targetSpeedy) > 0.01f) ? acceleration : deceleration;
        float forcey = Mathf.Pow(Mathf.Abs(speedDify) * accelRatey, velPower) * Mathf.Sign(speedDify);

        Vector2 force = new Vector2(forcex, forcey);
        // Movement 
        rb.AddForce(force * Vector2.one, ForceMode2D.Force);

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

        if ((Vector2.Distance(playerTransform.position, transform.position) < 100) && Time.time >= nextAttackTime && playerCombat.CurrentHealth > 0  && Phasetwo && !dancing)
        {
            dash = true;
            Debug.Log("Dashnow2");
            Attack2();
            nextAttackTime = Time.time + 1f / attackRate;
        }
        else
        {
            dash = false;
        }

        if (dash == true)
        {
            Debug.Log("Dashing2");
            rb.AddForce(force * Vector2.one * 1.25f, ForceMode2D.Impulse);
        }
        
        // Next Waypoint
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
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

    private IEnumerator ActivateThenDeactivate1(float waittime)
    {
        Zone1.SetActive(true);
        yield return new WaitForSecondsRealtime(waittime);
        Zone1.SetActive(false);
    }

    private IEnumerator ActivateThenDeactivate2(float waittime)
    {
        Zone2.SetActive(true);
        yield return new WaitForSecondsRealtime(waittime);
        Zone2.SetActive(false);
    }

    private IEnumerator StartFight()
    {
        // Blabla -> You there ? It never ends
        yield return new WaitForSecondsRealtime(2);
        StartCoroutine(ActivateThenDeactivate2(2));
        Phaseone = true;
        followEnabled = true;
        // You are just a mere pile of seconds
        yield return new WaitForSecondsRealtime(3);
        rb.bodyType = RigidbodyType2D.Dynamic;
        attackok = true;
    }

    private IEnumerator Dance1()
    {
        dancing = true;
        transform.position = new Vector3 (-478, 116, 0);
        rb.bodyType = RigidbodyType2D.Static;
        anim.SetBool("Dance1", true);
        yield return new WaitForSecondsRealtime(2);
        StartCoroutine(ActivateThenDeactivate1(2));
        yield return new WaitForSecondsRealtime(2);
        StartCoroutine(ActivateThenDeactivate2(2));
        yield return new WaitForSecondsRealtime(2);
        StartCoroutine(ActivateThenDeactivate1(2));
        yield return new WaitForSecondsRealtime(2);
        anim.SetBool("Dance1", false);

        // time for transition animation
        anim.SetBool("Transition", true);
        yield return new WaitForSecondsRealtime(1);
        rb.bodyType = RigidbodyType2D.Dynamic;
        anim.SetBool("Transition", false);
        anim.SetBool("Phasetwo", true);
        yield return new WaitForSecondsRealtime(1);
        rb.gravityScale = 0.5f;
        Phasetwo = true;
        dancing = false;
    }

    private IEnumerator Dance2()
    {
        dancing = true;
        // position a bit higher maybe
        transform.position = new Vector3(-478, 120, 0);
        rb.bodyType = RigidbodyType2D.Static;
        anim.SetBool("Dance2", true);
        yield return new WaitForSecondsRealtime(2);
        StartCoroutine(ActivateThenDeactivate1(2));
        yield return new WaitForSecondsRealtime(2);
        StartCoroutine(ActivateThenDeactivate2(2));
        yield return new WaitForSecondsRealtime(2);
        StartCoroutine(ActivateThenDeactivate1(2));
        yield return new WaitForSecondsRealtime(2);
        StartCoroutine(ActivateThenDeactivate2(2));
        yield return new WaitForSecondsRealtime(2);
        StartCoroutine(ActivateThenDeactivate1(2));
        yield return new WaitForSecondsRealtime(2);
        anim.SetBool("Dance2", false);
        rb.bodyType = RigidbodyType2D.Dynamic;
        dancetwodone = true;
        dancing = false;
    }


        public void TakeDamage(float damage)
    {
        if (!isDead && !dancing)
        {
            currentHealth -= damage;
            Debug.Log("Enemy health : " + currentHealth);
        }
        if (currentHealth <= 0)
        {
            StartCoroutine(FightEnd());
        }
    }



    private IEnumerator FightEnd()
    {
        followEnabled = false;
        transform.position = new Vector3(-478, 116, 0);
        rb.bodyType = RigidbodyType2D.Static;
        anim.SetBool("IsDead", true);
        anim.SetBool("Phasetwo", false);
        anim.SetBool("Transition", true);
        isDead = true;
        // Die animation
        yield return new WaitForSeconds(3);
        anim.SetBool("Transition", false);
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
