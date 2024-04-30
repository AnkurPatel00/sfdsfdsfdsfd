using Game.Utility;
using UnityEngine;

//This is for Orthographic Camera Only
[RequireComponent(typeof(Camera))]
public class CameraManager : MonoBehaviour
{
    [HideInInspector]
    public Camera Camera;

    private float pOrthosize => Camera.orthographicSize;

    private float pWorldScreenHalfWidth => CUtility.RoundingToFloat(pOrthosize * Camera.aspect);

    public float UpY => CUtility.RoundingToFloat(pOrthosize + transform.position.y);

    public float DownY => CUtility.RoundingToFloat(-pOrthosize + transform.position.y);

    public float RightX => CUtility.RoundingToFloat(pWorldScreenHalfWidth + transform.position.x);

    public float LeftX => CUtility.RoundingToFloat(-pWorldScreenHalfWidth + transform.position.x);

    private float width => CUtility.RoundingToFloat(pWorldScreenHalfWidth * 2);

    private float height => CUtility.RoundingToFloat(pOrthosize * 2);

    void Awake()
    {
        Camera = gameObject.GetComponent<Camera>();
    }

    public void DebugAllValues()
    {
        Debug.Log("m_orthosize =>" + pOrthosize);
        Debug.Log("m_Width =>" + width);
        Debug.Log("m_Height =>" + height);
        Debug.Log("m_UpY =>" + UpY);
        Debug.Log("m_DownY =>" + DownY);
        Debug.Log("m_RightX =>" + RightX);
        Debug.Log("m_LeftX =>" + LeftX);
    }

    public void SetOrthoGraphicSize(float size)
    {
        Camera.orthographicSize = size;
       //  DebugAllValues();
    }
}
