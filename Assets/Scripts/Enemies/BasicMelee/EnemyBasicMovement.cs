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
    float playerDist;
    float knockCountdown;
    [HideInInspector] public float knockDir;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        knockCountdown = knockTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (!stun && !knocking)
        {
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
            rb.MovePosition(transform.position + new Vector3(knockDir, transform.position.y, transform.position.z) * knockSpeed * Time.deltaTime);
        }
    }

    public void Knock() { StartCoroutine(m_Knock()); }

    IEnumerator m_Knock()
    {
        knocking = true;
        stun = true;

        yield return new WaitForSeconds(knockTime);

        knocking = false;

        yield return new WaitForSeconds(recoverTime);

        stun = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stopDist);
    }
}
