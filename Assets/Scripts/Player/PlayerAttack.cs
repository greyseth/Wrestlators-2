using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public PlayerMovement movement;
    public float maxDelay = .2f;
    public int maxAtk = 5;
    public float freezeTime = .1f;
    public float damage = 20;

    [Header("Grab settings")]
    public float grabTime = .5f;
    public float throwTime = .5f;

    [Header("Hitbox settings")]
    public DisplayOptions hitboxDisplay;
    public Vector3 hitboxPos;
    public Vector3 hitboxSize = new Vector3(1, 1, 1);

    public enum DisplayOptions { None, Wired, Full}
    bool airAttacking;
    bool isFrozen;
    int atkCount;
    float atkTime;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (movement.isGrounded)
        {
            airAttacking = false;

            if (Input.GetButtonDown("Attack"))
            {
                if (atkCount >= maxAtk) { atkCount = 0; atkTime = 0; movement.anim.SetBool("IsAttacking", false); return; }

                movement.anim.SetBool("IsAttacking", true);
                atkCount++;
                atkTime = Time.time;
            }else if (Input.GetButtonDown("Grab"))
            {
                atkCount = 0;
                atkTime = 0;
                movement.anim.SetBool("IsAttacking", false);
                movement.anim.SetBool("IsGrabbing", true);

                movement.anim.SetTrigger("Grab");
            }
        }else
        {
            if (Input.GetButtonDown("Attack")) { movement.anim.SetTrigger("Attack"); airAttacking = true; }

            if (airAttacking) { Attack(); airAttacking = false; }
        }

        if (Time.time >= atkTime + maxDelay) { atkCount = 0; atkTime = 0; movement.anim.SetBool("IsAttacking", false); }

        movement.anim.SetInteger("AtkCount", atkCount);
    }

    public void Attack()
    {
        Collider[] hit = Physics.OverlapBox(transform.position + hitboxPos, hitboxSize);
        foreach(Collider obj in hit)
        {
            if (obj.tag == "Player") continue;

            EnemyHealth enemy = obj.GetComponent<EnemyHealth>();
            if (enemy != null) enemy.TakeDamage(damage, atkCount >= (maxAtk - 1) ? true : false);

            if (!isFrozen && atkCount >= (maxAtk - 1)) StartCoroutine(ScreenFreeze());
        }
    }

    IEnumerator Grab()
    {
        bool hasTarget = false;

        Collider[] hit = Physics.OverlapBox(transform.position + hitboxPos, hitboxSize);
        foreach(Collider obj in hit)
        {
            if (obj.tag == "Player") continue;

            EnemyHealth enemy = obj.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                Debug.Log("Grabbed an enemy");
                hasTarget = true;
                break;
            }
        }

        yield return new WaitForSeconds(grabTime);

        if (hasTarget) movement.anim.SetTrigger("GrabSuccess");

        yield return new WaitForSeconds(grabTime);

        movement.anim.SetBool("IsGrabbing", false);
    }

    IEnumerator ScreenFreeze()
    {
        isFrozen = true;
        Time.timeScale = 0;

        yield return new WaitForSecondsRealtime(freezeTime);

        Time.timeScale = 1;
        isFrozen = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        switch(hitboxDisplay)
        {
            case DisplayOptions.None:
                break;
            case DisplayOptions.Wired:
                Gizmos.DrawWireCube(transform.position + hitboxPos, hitboxSize);
                break;
            case DisplayOptions.Full:
                Gizmos.DrawCube(transform.position + hitboxPos, hitboxSize);
                break;
        }
    }
}
