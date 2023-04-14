using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasicMovement : MonoBehaviour
{
    [Header("References")]
    public Animator anim;

    [Header("Values")]
    public float speed;
    public float stopDist;
    public float stunTime = .2f;
    public float knockTime = .5f;
    public float knockSpeed = 3;
    public float recoverTime = 1;

    Rigidbody rb;
    bool knocking;
    bool stun;
    bool move;
    bool added;
    [HideInInspector] public bool isDead;
    float playerDist;
    float knockCountdown;
    float lookDir;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        knockCountdown = knockTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;

        lookDir = GameManager.player.position.x > transform.position.x ? 1 : -1;

        if (!stun && !knocking)
        {
            transform.localRotation = Quaternion.Euler(new Vector3(0, 90 * lookDir, 0));

            playerDist = Vector3.Distance(transform.position, GameManager.player.position);
            if (playerDist > stopDist)
            {
                if (added) { GameManager.nearPlayer--; added = false; }

                if (GameManager.nearPlayer < GameManager.maxCrowd) move = true;
            }
            else
            {
                move = false;
                if (!added) { GameManager.nearPlayer++; added = true; }
            }
        }else
        {
            if (stun) move = false;
        }
    }

    private void FixedUpdate()
    {
        if (move)
        {
            Vector3 move = Vector3.MoveTowards(transform.position, GameManager.player.position, Time.fixedDeltaTime * speed);
            move.y = transform.position.y;
            rb.MovePosition(move);
        }else if (knocking)
        {
            rb.MovePosition(transform.position + new Vector3(-lookDir, 0, 0) * knockSpeed * Time.deltaTime);
        }
    }

    public void Stun() { StartCoroutine(m_Stun()); }
    public void Knock(bool die) { StartCoroutine(m_Knock(die)); }

    IEnumerator m_Stun()
    {
        stun = true;

        yield return new WaitForSeconds(stunTime);

        stun = false;
    }

    IEnumerator m_Knock(bool die)
    {
        knocking = true;
        stun = true;        

        yield return new WaitForSeconds(knockTime);

        knocking = false;

        if (!die)
        {
            yield return new WaitForSeconds(recoverTime);

            stun = false;
        }else
        {
            isDead = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stopDist);
    }
}
