using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("Values")]
    public float maxHealth;
    public float hurtTime = .2f;
    public float knockTime;
    public float knockSpeed;
    public float knockRecoverTime = 3.5f;

    [Header("UI")]
    public Slider bar;

    [Header("Debug")]
    public bool inTesting = false;

    PlayerMovement player;
    [HideInInspector] public bool stun;
    bool knocking;
    bool isDead;
    float health;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerMovement>();
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Debug.Log("You died");

            //if (!isDead) { player.enabled = false; player.GetComponent<PlayerAttack>().enabled = false; isDead = true; }
            player.enabled = false; player.GetComponent<PlayerAttack>().enabled = false;
            isDead = true;
        }

        if (knocking)
        {
            player.controller.Move(new Vector3(-player.lastX, 0, 0) * knockSpeed * Time.deltaTime);
        }

        if (inTesting)
        {
            if (Input.GetKeyDown(KeyCode.T)) TakeDamage(10, false);
            if (Input.GetKeyDown(KeyCode.Y)) TakeDamage(10, true);
        }
    }

    public void TakeDamage(float amount, bool knock)
    {
        if (stun || isDead || knocking) return;

        stun = true;
        health -= amount;
        
        if (!knock)
        {
            player.anim.SetTrigger("Hurt");
            StartCoroutine(DisableComponents(false, hurtTime));
        }else
        {
            player.anim.SetBool("IsKnocked", true);
            StartCoroutine(DisableComponents(false, knockTime));
            knocking = true;
        }
    }

    public void TakeHeal(float amount)
    {
        health += amount;
    }

    IEnumerator DisableComponents(bool indefinite, float time = 1)
    {
        player.enabled = false;
        player.GetComponent<PlayerAttack>().enabled = false;

        yield return new WaitForSeconds(time);
        
        player.anim.SetBool("IsKnocked", false);

        if(knocking)
        {
            knocking = false;
            yield return new WaitForSeconds(knockRecoverTime);
            if (!indefinite) { player.enabled = true; player.GetComponent<PlayerAttack>().enabled = true; }
            stun = false;
        }else {
            if (!indefinite) { player.enabled = true; player.GetComponent<PlayerAttack>().enabled = true; }
            stun = false;
        }
    }
}
