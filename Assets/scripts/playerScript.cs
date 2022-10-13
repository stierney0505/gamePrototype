using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class playerScript : MonoBehaviour
{   
    
    runeSelector RuneSelect;
    private float speed = 5.0f;
    private int health;
    private Animator animator;
    
    
    // Start is called before the first frame update
    void Start()
    {
        RuneSelect = GetComponent<runeSelector>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        bool up = false, down = false, left = false, right = false;

        if (Input.GetKeyDown(KeyCode.E))
            Charge();

        

            if (Input.GetKey(KeyCode.W))
                up = true;
            if (Input.GetKey(KeyCode.S))
                down = true;
            if (Input.GetKey(KeyCode.A))
                left = true;
            if (Input.GetKey(KeyCode.D))
                right = true;
            Move(up, down, left, right);
        
        if (!(up || down || left || right))
            animator.SetTrigger("Player Still");
        
        
        

    }

    public void Move(bool up, bool down, bool left, bool right)
    {
            bool moveUp = (up && !down);
            bool moveDown = (down && !up);
            bool moveLeft = (left && !right);
            bool moveRight = (right && !left);

            if (moveUp)
            {
                animator.SetTrigger("Player Move");
                Vector3 movement = new Vector2(0, 1.0f);
                transform.Translate(movement * speed * Time.deltaTime);
            }
            if (moveDown)
            {
                animator.SetTrigger("Player Move");
                Vector3 movement = new Vector2(0, -1.0f);
                transform.Translate(movement * speed * Time.deltaTime);
            }
            if (moveLeft)
            {
                animator.SetTrigger("Player Move");
                gameObject.transform.localScale = new Vector3(-10, 10, 10);
                Vector3 movement = new Vector2(-1.0f, 0);
                transform.Translate(movement * speed * Time.deltaTime);
            }
            if (moveRight)
            {
                animator.SetTrigger("Player Move");
                gameObject.transform.localScale = new Vector3(10, 10, 10);
                Vector3 movement = new Vector2(1.0f, 0);
                transform.Translate(movement * speed * Time.deltaTime);
            }
        



    }

    public void Charge()
    {
        animator.SetTrigger("Player Charge");
        
    }
}
