using UnityEngine;

public class MouseWorldPosition : MonoBehaviour
{
    public static MouseWorldPosition instance { get; private set; }

    //==================================================================================================

    private void Awake()
    {
        instance = this;
    }


    public Vector3 getPosition()
    {
        var mouseCameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new(Vector3.up, Vector3.zero);

        if (!plane.Raycast(mouseCameraRay, out var distance)) return default;

        return mouseCameraRay.GetPoint(distance);
    }
}
