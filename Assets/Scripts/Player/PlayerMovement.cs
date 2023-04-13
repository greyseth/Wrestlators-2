using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform directionModifier;
    public Animator anim;
    public Transform animRoot;

    [Header("Basic movement")]
    public float speed;
    public float dashSpeed;
    public float doubleClickTime = .3f;

    [Header("Jumping")]
    public float jumpHeight;
    public float airSpeed;
    public float gravity = -9.18f;
    public Transform groundCheck;
    public Vector3 checkSize;
    public LayerMask groundLayer;

    [HideInInspector] public CharacterController controller;
    Vector3 airMove;
    Vector3 vel;
    [HideInInspector] public bool isGrounded;
    bool dashing;
    [HideInInspector] public float lastX = 1;

    float clickAmount, t;
    bool DoubleClick(string button)
    {
        if (Input.GetButtonDown(button))
        {
            clickAmount++;
            if (clickAmount == 1) t = Time.time;
        }
        if (clickAmount > 1 && Time.time - t < doubleClickTime)
        {
            clickAmount = 0;
            t = 0;
            return true;
        }
        else if (clickAmount > 2 || Time.time - t > 1) clickAmount = 0;
        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckBox(groundCheck.position, checkSize, Quaternion.identity, groundLayer);

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        Vector3 move = directionModifier.forward * z + directionModifier.right * x;

        if (isGrounded)
        {
            if (move.magnitude > .1f)
            {
                if (move.x != 0) lastX = move.x;
                animRoot.transform.localRotation = Quaternion.Euler(new Vector3(-90, 0, 90 * lastX));
            }

            Vector3 normalMove = move * speed * Time.deltaTime;
            Vector3 dashMove = new Vector3(move.x, 0, 0) * dashSpeed * Time.deltaTime;

            if (!anim.GetBool("IsAttacking")) controller.Move(dashing ? dashMove : normalMove);
            if (!Input.GetButtonDown("Jump")) airMove = Vector3.zero;

            controller.slopeLimit = 45;
            if (vel.y < 0) vel.y = -2;

            if (DoubleClick("Right") || DoubleClick("Left")) dashing = true;
            else if (Input.GetButtonUp("Right") || Input.GetButtonUp("Left")) dashing = false;

            if (Input.GetButtonDown("Jump"))
            {
                airMove = move;
                vel.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
                isGrounded = false;

                //Debug.Log($"{move} -> Move\n{airMove} -> airMove");
            }
        }else
        {
            animRoot.transform.localRotation = Quaternion.Euler(new Vector3(-90, 0, 90 * lastX));
            controller.Move(airMove * airSpeed * Time.deltaTime);

            controller.slopeLimit = 90;
            dashing = false;
        }

        vel.y += gravity * Time.deltaTime;
        controller.Move(vel * Time.deltaTime);

        anim.SetBool("Grounded", isGrounded);
        anim.SetBool("Moving", move.magnitude > .1f ? true : false);
        anim.SetBool("Dashing", dashing);        
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.grey;
            Gizmos.DrawCube(groundCheck.position, checkSize);
        }
    }
}
