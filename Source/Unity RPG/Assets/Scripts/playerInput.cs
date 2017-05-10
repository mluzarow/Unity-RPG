using UnityEngine;

[RequireComponent (typeof (playerController))]

public class playerInput : MonoBehaviour {
    // Movement
    float moveSpeed = 3.0f;  // Movement force
    Vector2 velocity;       // Player velocity

    public static Vector2 input;

    playerController controller;

    // Use this for initialization
    void Start () {
        controller = GetComponent <playerController> ();
    }

    // Update is called once per frame
    void Update () {
        // Get raw direction - Keyboard W for up S for down
        input = new Vector2 ((Input.GetKey (KeyCode.A) ? -1 : 0) + (Input.GetKey (KeyCode.D) ? 1 : 0), (Input.GetKey (KeyCode.W) ? 1 : 0) + (Input.GetKey (KeyCode.S) ? -1 : 0));

        // Check for collisions and update velocity
        if (controller.collisions.up || controller.collisions.down) {
            velocity.y = 0f;
        }
        if (controller.collisions.left || controller.collisions.right) {
            velocity.x = 0f;
        }

        velocity = input * moveSpeed;
        controller.Move (velocity * Time.deltaTime);
    }
}

