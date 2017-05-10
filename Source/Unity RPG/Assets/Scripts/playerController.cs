using UnityEngine;

public class playerController : raycaster {
    public collisionInfo collisions;

    /// <summary>
	/// The collision mask for colliding with the ground layer.
    /// </summary>
	public LayerMask collisionMask;

    public override void Start () {
        horizontalRays = 4;
        verticalRays = 4;

        // Must be override to use base start method
        base.Start ();

        // Always start facing up and to the right
        collisions.directionFacing = new Vector2 (1, 1);
    }

    public void Move (Vector2 velocity) {
        updateRaycasts ();
        collisions.reset ();

        // Velocity in the x plane is NOT 0; check for collisions
        if (velocity.x != 0) {
            collisions.directionFacing.x = (int) Mathf.Sign (velocity.x);
            horizontalCollisions (ref velocity);
        }
        
        // Velocity in the y plane is NOT 0; check for collisions
        if (velocity.y != 0) {
            collisions.directionFacing.y = (int) Mathf.Sign (velocity.y);
            verticalCollisions (ref velocity);
        }

        // Move player with updated velocity value
        transform.Translate (velocity);
    }

    // Velocity in the x plane is NOT 0; check for collisions
    void horizontalCollisions (ref Vector2 velocity) {
        float directionX = collisions.directionFacing.x;
        float rayLength = Mathf.Abs (velocity.x) + inset;

        // Extend rays to detect walls while not moving towards them
        if (Mathf.Abs (velocity.x) < inset) {
            rayLength = 2 * inset;
        }

        for (int i = 0; i < horizontalRays; i++) {
            // If directionX is negative (moving left), raycast from the bottom left
            // If directionX is positive (moving right), raycast from the bottom right
            Vector2 rayOrigin = (directionX == -1) ? raycasts.bottomLeft : raycasts.bottomRight;
            // Add horizontal ray spacing to raycast position
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast (rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            Debug.DrawRay (rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if (hit) {
                if (hit.distance == 0) {
                    continue;
                }

                velocity.x = (hit.distance - inset) * directionX;
                rayLength = hit.distance;

                // Set collision flag for collision left or right
                collisions.left = (directionX == -1);
                collisions.right = (directionX == 1);
            }
        }
    }

    void verticalCollisions (ref Vector2 velocity) {
        float directionY = collisions.directionFacing.y;
        float rayLength = Mathf.Abs (velocity.y) + inset;

        for (int i = 0; i < verticalRays; i++) {
            Vector2 rayOrigin = (directionY == -1) ? raycasts.bottomLeft : raycasts.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast (rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            Debug.DrawRay (rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit) {
                velocity.y = (hit.distance - inset) * directionY;
                rayLength = hit.distance;

                // Set collision flag for collision up or down
                collisions.up = (directionY == 1);
                collisions.down = (directionY == -1);
            }
        }
    }

    // Struct for telling which direction you are colliding in
    public struct collisionInfo {
        public bool up;
        public bool down;
        public bool left;
        public bool right;
        public Vector2 directionFacing;

        public void reset () {
            up = false;
            down = false;
            left = false;
            right = false;
        }
    }
}

