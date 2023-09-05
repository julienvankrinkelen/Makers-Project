using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BossScript : MonoBehaviour
{

    [Header("Pathfinding")]
    [SerializeField] private Transform target;
    private float activateDistance = 50f;
    private float pathUpdateSeconds = 0.2f;

    [Header("Physics")]
    private float speed = 8f;
    [SerializeField] private float acceleration = 13;
    [SerializeField] private float deceleration = 16;
    [SerializeField] private float velPower = 0.96f;
    private float nextWaypointDistance = 3f;
    private float jumpCheckOffset = 0.1f;

    [Header("Custom Behavior")]
    private bool followEnabled = false;
    private bool directionLookEnabled = true;

    private Path path;
    private int currentWaypoint = 0;
    private bool isGrounded = false;
    Seeker seeker;
    Rigidbody2D rb;
    [SerializeField] private Animator anim;

    private float maxHealth = 40f;
    private float currentHealth;
    private bool attackok = false;
    private bool Phaseone = false;
    private bool Phasetwo = false;
    private bool dancing = false;
    private bool dancetwodone = false;
    private bool isDead = false;

    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private PlayerCombat playerCombat;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Collider2D playercollider;
    [SerializeField] private GameObject Zone1;
    [SerializeField] private GameObject Zone2;
    [SerializeField] private GameObject Zone3;
    [SerializeField] private GameObject Wall;

    private float attackRange = 0.5f;
    private int attackDamage = 2;
    private float attackRate = 0.5f;
    private float nextAttackTime = 0f;
    private bool dash = false;

    [SerializeField] private AudioSource dieSoundEffect;
    [SerializeField] private AudioSource transiSoundEffect;
    [SerializeField] private AudioSource incantSoundEffect;
    [SerializeField] private AudioSource thunderSoundEffect;
    [SerializeField] private AudioSource attackSoundEffect;

    [SerializeField] private Material flashingMaterial;
    [SerializeField] private Material originalMaterial;
    private float flashDuration = 0.30f;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private BossRPStart bossRPStart;
    [SerializeField] private BossRPEnd bossRPEnd;
    private bool RpStarted = false;

    [SerializeField] private AudioSource bossTrack;
    [SerializeField] private MusicZoneScript musicZoneScript;
    [SerializeField] private PauseMenu pauseMenu;
    

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        InvokeRepeating(nameof(UpdatePath), 0f, pathUpdateSeconds);
        currentHealth = maxHealth;
        transform.position = new Vector3(-484.18f, 115.86f, 0);
        Wall.SetActive(false);
    }
    
    private void FixedUpdate()
    {
        if (!RpStarted && Vector2.Distance(playerTransform.position, transform.position) < 15 && anim.GetBool("IsDead") == false && playerCombat.CurrentHealth > 0 && !Phaseone && !Phasetwo && !dancing)
        {
            StartRP();
            RpStarted = true;
            playerCombat.InBossCombat = true;
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

    public void Autodestruction()
    {
        Debug.Log("AUTODESTRU ");
        gameObject.SetActive(false);
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
            rb.AddForce(force/1.5f * Vector2.right , ForceMode2D.Impulse) ;
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

        if ((Vector2.Distance(playerTransform.position, transform.position) < 150) && Time.time >= nextAttackTime && playerCombat.CurrentHealth > 0  && Phasetwo && !dancing)
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
        if (dash == true && transform.localScale.x == 1)
        {
            Debug.Log("Dashing2");
            rb.AddForce(direction * force * Vector2.one * 2f, ForceMode2D.Impulse);
        }

        if (dash == true && transform.localScale.x == -1)
        {
            Debug.Log("Dashing2");
            rb.AddForce(direction * force * -Vector2.one * 2f, ForceMode2D.Impulse);
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
        StartCoroutine(SlashDelayed());
    }

    public IEnumerator SlashDelayed()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        attackSoundEffect.Play();
        
    }

    public void Attack2()
    {
        anim.SetTrigger("Attack2");
        StartCoroutine(SlashDelayed());
    }

    private IEnumerator ActivateThenDeactivate1(float waittime)
    {
        StartCoroutine(ThunderSoundEffectPlay());
        Zone1.SetActive(true);
        yield return new WaitForSecondsRealtime(waittime);
        Zone1.SetActive(false);
    }

    private IEnumerator ActivateThenDeactivate2(float waittime)
    {
        StartCoroutine(ThunderSoundEffectPlay());
        Zone2.SetActive(true);
        yield return new WaitForSecondsRealtime(waittime);
        Zone2.SetActive(false);
    }

    private IEnumerator ActivateThenDeactivate3(float waittime)
    {
        StartCoroutine(ThunderSoundEffectPlay());
        Zone3.SetActive(true);
        yield return new WaitForSecondsRealtime(waittime);
        Zone3.SetActive(false);
    }

    private IEnumerator ThunderSoundEffectPlay()
    {
        yield return new WaitForSeconds(1.2f);
        thunderSoundEffect.Play();
    }

    public void StartRP()
    {
        bossRPStart.StartDialog();
        pauseMenu.EnablePauseEchap(false);
    }
    public void StartFight()
    {
        StartCoroutine(StartFightRoutine());
    }

    private IEnumerator StartFightRoutine()
    {
        yield return new WaitForSecondsRealtime(1);
        Wall.SetActive(true);
        StartCoroutine(ActivateThenDeactivate2(2));
        Phaseone = true;
        followEnabled = true;
        yield return new WaitForSecondsRealtime(1);
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
        incantSoundEffect.Play();
        StartCoroutine(ActivateThenDeactivate1(2));
        yield return new WaitForSecondsRealtime(2);
        StartCoroutine(ActivateThenDeactivate2(2));
        yield return new WaitForSecondsRealtime(2);
        StartCoroutine(ActivateThenDeactivate1(2));
        yield return new WaitForSecondsRealtime(2);
        incantSoundEffect.Stop();

        anim.SetBool("Dance1", false);

        // time for transition animation
        anim.SetBool("Transition", true);
        transiSoundEffect.Play();
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
        incantSoundEffect.Play();
        StartCoroutine(ActivateThenDeactivate1(2));
        yield return new WaitForSecondsRealtime(2);
        StartCoroutine(ActivateThenDeactivate2(2));
        yield return new WaitForSecondsRealtime(2);
        StartCoroutine(ActivateThenDeactivate3(2));
        yield return new WaitForSecondsRealtime(2);
        StartCoroutine(ActivateThenDeactivate2(2));
        yield return new WaitForSecondsRealtime(2);
        StartCoroutine(ActivateThenDeactivate1(2));
        
        yield return new WaitForSecondsRealtime(2);
        incantSoundEffect.Stop();
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

            StartCoroutine(FlashCoroutine());
        }
        if (!isDead && currentHealth <= 0)
        {
            isDead = true;
            StartCoroutine(FightEnd());
        }
    }

    private IEnumerator FlashCoroutine()
    {
        spriteRenderer.material = flashingMaterial;

        // Animate transparency for flashing effect
        float timer = 0;
        while (timer < flashDuration)
        {
            flashingMaterial.color = new Color(1f, 0.4f, 0.4f);
            timer += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.material = originalMaterial;
    }


    private IEnumerator FightEnd()
    {
        playerCombat.InBossCombat = false;
        followEnabled = false;
        transform.position = new Vector3(-478, 116, 0);
        rb.bodyType = RigidbodyType2D.Static;
        anim.SetBool("IsDead", true);
        anim.SetBool("Phasetwo", false);
        anim.SetBool("Transition", true);
        // Die animation
        yield return new WaitForSeconds(1);
        dieSoundEffect.Play();
        musicZoneScript.FadeOutTrackSingle(bossTrack);
        yield return new WaitForSeconds(2);
        anim.SetBool("Transition", false);
        Wall.SetActive(false);
        yield return new WaitForSeconds(2);
        bossRPEnd.StartDialog();
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
