using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{


    public int balls;

    [SerializeField]
    private float timerLength = 1f;
    [SerializeField]
    private float slowTimeScale = 0.5f;
    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private TextMeshProUGUI spacePromptText;
    [SerializeField]
    private TextMeshProUGUI resetPromptText;

    private float startFixedDeltaTime;
    private float timer;
    private Player player;
    private CameraController camController;

    private int score;

    void Start() {
        player = FindObjectOfType<Player>();
        camController = FindObjectOfType<CameraController>();
        startFixedDeltaTime = Time.fixedDeltaTime;
     
        score = 0;
        balls = 3;
        scoreText.text = "score: " + score.ToString("D24");
        resetPromptText.enabled = false;
    }

    void Update() {

        if ( Input.GetKey(KeyCode.X)) {
            SceneManager.LoadSceneAsync(
                SceneManager.GetActiveScene().buildIndex);
        }

        if (player.state == Player.State.OUT_OF_BOUNDS) {

            if (balls > 0) {
                if ( Input.GetKey(KeyCode.Space) ) {
                    player.transform.position = player.startPos;
                    player.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                    Rigidbody playerRb = player.GetComponent<Rigidbody>();
                    playerRb.velocity = new Vector3(0f, 0f, 0f);
                    playerRb.angularVelocity = new Vector3(0f, 0f, 0f);
                    camController.transform.rotation = Quaternion.Euler(0, 0, 0);
                    player.state = Player.State.START;
                }
            }
 

        }
        else if (player.state == Player.State.COLLIDE) {
            if (timer > 0) {
                timer -= Time.deltaTime;
            }
            else {
                player.state = Player.State.ROLL;
                Time.timeScale = 1f;
                Time.fixedDeltaTime = startFixedDeltaTime;
                camController.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

    public void addScore() {
        score += 500;
        scoreText.text = "score: " + score.ToString("D24");
    }

    public void decrementBalls() {
        balls -= 1;
    }

    public void toggleSpacePrompt() {
        if (spacePromptText.enabled == false) {
            spacePromptText.enabled = true;
        }
        else {
            spacePromptText.enabled = false;
        }
    }
    public void toggleResetPrompt() {
        if (resetPromptText.enabled == false) {
            resetPromptText.enabled = true;
        }
        else {
            resetPromptText.enabled = false;
        }
    }

    public void collisionCamStart() {
        camController.newCamPosition();
        Time.timeScale = slowTimeScale;
        Time.fixedDeltaTime *= slowTimeScale;
        timer = timerLength;
    }
}
