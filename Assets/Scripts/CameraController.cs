using UnityEngine;

[ExecuteInEditMode]
public class CameraController : MonoBehaviour
{
    public Camera cameraShowFrustumAlways;
    private void OnDrawGizmos()
    {
        if (cameraShowFrustumAlways) {
            Gizmos.matrix = cameraShowFrustumAlways.transform.localToWorldMatrix;
            Gizmos.DrawFrustum(new Vector3(0, 0, cameraShowFrustumAlways.nearClipPlane),
                cameraShowFrustumAlways.fieldOfView,
                cameraShowFrustumAlways.farClipPlane,
                cameraShowFrustumAlways.nearClipPlane,
                cameraShowFrustumAlways.aspect);
        }
    }
}
