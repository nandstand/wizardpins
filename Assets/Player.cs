using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public enum State {
        START,
        ROLL,
        COLLIDE,
        OUT_OF_BOUNDS
    }

    public const KeyCode ROLL_KEY = KeyCode.Space;
    public const KeyCode RESET_KEY = KeyCode.X;

    [HideInInspector]
    public State state;
    [HideInInspector]
    public Vector3 startPos;

    private const float COLL_CAM_TIMER_LENGTH = 2f;

    [SerializeField]
    private float forwardForce = 10f;
    [SerializeField]
    private float tiltForce = 10f;
    [SerializeField]
    private GameObject bounds;

    private Rigidbody rb;
    private GameController game;
    private HashSet<GameObject> collided;
    private Vector3 rollDirection;
    private Vector3 collisionPos;
    private float collisionCamTimer;

    void Start() {
        startPos = transform.position;
        rb = GetComponent<Rigidbody>();
        game = FindObjectOfType<GameController>();
        collided = new HashSet<GameObject>();
        state = State.START;
        rollDirection = transform.forward;
    }

    void Update() {
        
        if (Input.GetKeyUp(ROLL_KEY)) {
            if (state == State.START) {
                game.decrementBalls();
                game.toggleSpacePrompt();
                state = State.ROLL;
                rb.AddForce(rollDirection * 2000f);
                rb.AddForce((transform.up + transform.forward) * 250f);
            }
        }

        if (collisionCamTimer > 0) {
            collisionCamTimer -= Time.deltaTime;
        }

    }

    void FixedUpdate() {
        if (state == State.ROLL) {
            rb.AddForce(rollDirection * forwardForce);

            float hInput = Input.GetAxis("Horizontal");
            rb.AddForce(transform.right * hInput * tiltForce);
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject == bounds) {
            state = State.OUT_OF_BOUNDS;
            if (game.balls == 0) {
                game.toggleResetPrompt();
            }
            else {
                game.toggleSpacePrompt();
            }
        }
    }

    void OnCollisionEnter(Collision collision) {
        
        if ((collision.rigidbody != null)) {

            tryAddScore(collision);
            collided.Add(collision.gameObject);

            if ( (state != State.COLLIDE) && (collisionCamTimer <= 0) ) {

                state = State.COLLIDE;
                collisionPos = transform.position;
                collisionCamTimer = COLL_CAM_TIMER_LENGTH;
                game.collisionCamStart();

            }
        }
    }
    private void tryAddScore(Collision collision) {
        
        if ( !(collided.Contains(collision.gameObject)) ) {
            game.addScore();
        }

    }


    public Vector3 GetCollisionPos() {
        return collisionPos;
    }
}
