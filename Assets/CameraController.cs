using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //[SerializeField]
    //private float tilt = 0.5f;

    [SerializeField]
    private float maxHorizontalAngle = 20f;
    private float tiltSpeed = 200f;

    private Player player;
    private Vector3 offset;

    private float collideCamX;
    private float collideCamY;
    private float collideCamZ;

    void Start() {
        player = FindObjectOfType<Player>();
        offset = transform.position - player.transform.position;
    }

    void LateUpdate() {
        if ( (player.state == Player.State.ROLL) || (player.state == Player.State.START) ) {
            transform.position = player.transform.position + offset;
            

            float scaledHorizontalTilt = Input.GetAxis("Horizontal") * maxHorizontalAngle;
            Quaternion targetZRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, scaledHorizontalTilt);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetZRotation, tiltSpeed * Time.deltaTime);
        } 
        else if (player.state == Player.State.COLLIDE) {
            transform.position = player.GetCollisionPos() + new Vector3(collideCamX, collideCamY, collideCamZ);
            transform.LookAt(player.transform.position);
        }
        else if (player.state == Player.State.OUT_OF_BOUNDS) {
            transform.LookAt(player.transform.position);
        }
    }

    public void newCamPosition() {
        collideCamX = Random.Range(6f, 8f);
        collideCamY = Random.Range(6f, 8f);
        collideCamZ = Random.Range(6f, 8f);
    }
}
