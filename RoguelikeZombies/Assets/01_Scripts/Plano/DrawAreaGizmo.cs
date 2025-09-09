using UnityEngine;

[ExecuteAlways]
public class DrawAreaGizmo : MonoBehaviour
{
    public Vector2 areaSize = new Vector2(100f, 100f);

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 c = transform.position;
        Vector3 size = new Vector3(areaSize.x, 0.1f, areaSize.y);
        Gizmos.DrawWireCube(c, size);
    }
}
