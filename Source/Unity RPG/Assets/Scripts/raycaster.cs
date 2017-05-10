using UnityEngine;

[RequireComponent (typeof (BoxCollider2D))]
public class raycaster : MonoBehaviour {
    [HideInInspector] public BoxCollider2D collider;
    [HideInInspector] public raycastOrigins raycasts;

    protected const float inset = 0.015f;
    protected int horizontalRays = 8;
    protected int verticalRays = 8;
    protected float horizontalRaySpacing;
    protected float verticalRaySpacing;

    public virtual void Start () { // Must be virtual in order to be used scipts that extend this one
        collider = GetComponent<BoxCollider2D> ();
        calcRaySpacing ();
    }

    public void updateRaycasts () {
        Bounds bounds = collider.bounds;
        bounds.Expand (inset * -2);

        raycasts.bottomLeft = new Vector2 (bounds.min.x, bounds.min.y);
        raycasts.bottomRight = new Vector2 (bounds.max.x, bounds.min.y);
        raycasts.topLeft = new Vector2 (bounds.min.x, bounds.max.y);
        raycasts.topRight = new Vector2 (bounds.max.x, bounds.max.y);
    }

    public void calcRaySpacing () {
        Bounds bounds = collider.bounds;
        bounds.Expand (inset * -2);

        // Minimum amount of rays is 2 (for editor)
        horizontalRays = Mathf.Clamp (horizontalRays, 2, int.MaxValue);
        verticalRays = Mathf.Clamp (verticalRays, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRays - 1);
        verticalRaySpacing = bounds.size.x / (verticalRays - 1);
    }

    // struct for the bounds of an object from which rays will be cast
    public struct raycastOrigins {
        public Vector2 topLeft;
        public Vector2 topRight;
        public Vector2 bottomLeft;
        public Vector2 bottomRight;
    }
}
