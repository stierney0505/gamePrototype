using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class playerScript : MonoBehaviour
{
    private float speed = 5.0f;
    private int health;
    private Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        bool up = false, down = false, left = false, right = false;

        if (Input.GetKeyDown(KeyCode.E)) //The E button currently turns the character into a charge state where they 
            Charge();                    //Load a rune into the rune buffer

        if (Input.GetKey(KeyCode.W)) //these get the directions the character is moving as bools, this is so that if 
            up = true;               //Up and down is pressed the character doesnt move
        if (Input.GetKey(KeyCode.S))
            down = true;
        if (Input.GetKey(KeyCode.A))
            left = true;
        if (Input.GetKey(KeyCode.D))
            right = true;
        Move(up, down, left, right);

        if (!(up || down || left || right)) //This stops the character's move animation the moment they stop
            animator.SetTrigger("Player Still");
    }

    public void Move(bool up, bool down, bool left, bool right) //Movement method, just takes the bools for movement directions
    {                                                           //and moves the character based upon that
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

    public void Charge() { animator.SetTrigger("Player Charge"); }
}
