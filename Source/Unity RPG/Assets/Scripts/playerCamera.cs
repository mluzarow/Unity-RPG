using UnityEngine;
using System.Collections;

public class playerCamera : MonoBehaviour {
    public playerController target;

    private Vector2 focusAreaSize = new Vector2 (1.0f, 1.0f);
    private Vector2 lookAheadDist = new Vector2 (1.0f, 1.0f);
    private Vector2 lookSmoothTime = new Vector2 (0.2f, 0.2f);

    private Vector2 currentLookAhead;
    private Vector2 targetLookAhead;
    private Vector2 lookAheadDir;
    private Vector2 smoothLookVelocity;
    private Vector2 smoothVelocity;

    FocusArea focusArea;
    
    void Start () {
        focusArea = new FocusArea (target.GetComponent <BoxCollider2D> ().bounds, focusAreaSize);
    }

    void LateUpdate () {
        // Focus area collision
        focusArea.Update (target.GetComponent <BoxCollider2D> ().bounds);

        // Camera movement
        Vector2 focusPosition = focusArea.center;

        // If focus area is being pushed, find the direction
        if (focusArea.velocity.x != 0) {
            lookAheadDir.x = Mathf.Sign (focusArea.velocity.x);
        }
        if (focusArea.velocity.y != 0) {
            lookAheadDir.y = Mathf.Sign (focusArea.velocity.y);
        }

        // Find direction and distance of look target
        targetLookAhead.x = lookAheadDir.x * lookAheadDist.x;
        targetLookAhead.y = lookAheadDir.y * lookAheadDist.y;

        // Damp moving of camera to target
        currentLookAhead.x = Mathf.SmoothDamp (currentLookAhead.x, targetLookAhead.x, ref smoothLookVelocity.x, lookSmoothTime.x);
        currentLookAhead.y = Mathf.SmoothDamp (currentLookAhead.y, targetLookAhead.y, ref smoothLookVelocity.y, lookSmoothTime.y);

        // Calc camera movement
        focusPosition += currentLookAhead;
        // Move camera while keeping it -10 units away from the level on the z axis
        transform.position = (Vector3) focusPosition + Vector3.forward * -10;
    }

    void OnDrawGizmos () {
        Gizmos.color = new Color (1, 0, 0, 0.5f);
        Gizmos.DrawCube (focusArea.center, focusAreaSize);
    }

    struct FocusArea {
        public Vector2 center;
        public Vector2 velocity;
        float left;
        float right;
        float top;
        float bottom;

        /// <summary>
        /// Focus Area constructor; finds the 4 sides of the target collider box.
        /// </summary>
        public FocusArea (Bounds targetBounds, Vector2 size) {
            // size is the size of the Focus Area
            // targetBounds is the collision box bounds of the target

            // Create the bounds of the focus areea by extending the target collision box boundaries

            // The left-side wall
            left = targetBounds.center.x - size.x / 2;
            // The right-side wall
            right = targetBounds.center.x + size.x / 2;
            // The bottom wall
            bottom = targetBounds.center.y - size.y / 2;
            // The top wall
            top = targetBounds.center.y + size.y / 2;

            // Camera velocity starts at 0.0f
            velocity = Vector2.zero;

            // Find center of focus area
            center = new Vector2 ((left + right) / 2, (top + bottom) / 2);
        }

        public void Update (Bounds targetBounds) {
            float shiftX = 0;

            if (targetBounds.min.x < left) {
                shiftX = targetBounds.min.x - left;
            } else if (targetBounds.max.x > right) {
                shiftX = targetBounds.max.x - right;
            }
            
            left += shiftX;
            right += shiftX;
            
            float shiftY = 0;
            
            if (targetBounds.min.y < bottom) {
                shiftY = targetBounds.min.y - bottom;
            } else if (targetBounds.max.y > top) {
                shiftY = targetBounds.max.y - top;
            }

            top += shiftY;
            bottom += shiftY;
            
            center = new Vector2 ((left + right) / 2, (top + bottom) / 2);
            velocity = new Vector2 (shiftX, shiftY);
        }
    }
}
