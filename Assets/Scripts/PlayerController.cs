using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float walkSpeed = 1;

    bool _isGrounded = true;

    bool _isPlaying_crouch = false;
    bool _isPLaying_hadoken = false;

    Animator animator;

    const uint STATE_IDLE = 0;
    const uint STATE_WALK = 1;
    const uint STATE_CROUCH = 2;
    const uint STATE_JUMP = 3;
    const uint STATE_HADOKEN = 4;

    string _currentDirection = "left";
    uint _currentAnimationState = STATE_IDLE;

    // Use this for initialization
    void Start () {
        animator = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            changeState(STATE_HADOKEN);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) && !_isPLaying_hadoken && !_isPlaying_crouch)
        {
            if(_isGrounded)
            {
                _isGrounded = false;
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 250));
                changeState(STATE_JUMP);
            }
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            changeState(STATE_CROUCH);
        }
        else if (Input.GetKey(KeyCode.RightArrow) && !_isPLaying_hadoken)
        {
            changeDirection("right");
            transform.Translate(Vector3.left * walkSpeed * Time.fixedDeltaTime);

            if (_isGrounded)
            {
                changeState(STATE_WALK);
            }
                
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && !_isPLaying_hadoken)
        {
            changeDirection("left");
            transform.Translate(Vector3.left * walkSpeed * Time.fixedDeltaTime);

            if (_isGrounded)
            {
                changeState(STATE_WALK);
            }
        }
        else if (_isGrounded)
        {
            changeState(STATE_IDLE);
        }

        _isPlaying_crouch = animator.GetCurrentAnimatorStateInfo(0).IsName("ken_crounch");
        _isPLaying_hadoken = animator.GetCurrentAnimatorStateInfo(0).IsName("ken_hadoken");
    }

    void changeState (uint state)
    {
        if (_currentAnimationState == state)
        {
            return;

            animator.SetInteger("state", (int) state);
            _currentAnimationState = state;
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.name == "Floor")
        {
            _isGrounded = true;
            changeState(STATE_IDLE);
        }
        Debug.Log("une collision ...");
    }

    void changeDirection(string direction)
    {
        if(_currentDirection != direction)
        {
            if(direction == "right")
            {
                transform.Rotate(0, 180, 0);
            }
            else if (direction == "left")
            {
                transform.Rotate(0, -180, 0);
            }
            _currentDirection = direction;
        }
    }
}
