using UnityEngine;

public class TestAnim : MonoBehaviour
{
    private Animator animator;

    private Rigidbody2D rb;
    private BoxCollider2D col;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            animator.SetTrigger("scaleTrigger");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            animator.SetBool("isRotation", true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            animator.SetBool("isRotation", false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            animator.SetBool("isScale", true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            animator.SetBool("isScale", false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            animator.Rebind();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            //animator.enabled = false;
            animator.Update(100000f);
        }

    }
}
