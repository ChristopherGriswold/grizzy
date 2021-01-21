using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapCameraController : MonoBehaviour, IDragHandler
{
    public GameObject mapCameraObject;
    public float cameraLookSpeed;

    private Camera mapCamera;
    private Vector2 touchDeltaPosition;
    public float orthoZoomSpeed = 0.02f;

    
    void Update()
    {
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
            mapCamera.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;
            mapCamera.orthographicSize = Mathf.Clamp(mapCamera.orthographicSize, 25f, 500f);
        }
    }


public void OnDrag(PointerEventData eventData)
    {
        if(Input.touchCount > 1)
        {
            return;
        }
        touchDeltaPosition = eventData.delta;
        mapCamera.transform.position += new Vector3(-touchDeltaPosition.x * cameraLookSpeed * (mapCamera.orthographicSize / 500),
            0f, -touchDeltaPosition.y * cameraLookSpeed * (mapCamera.orthographicSize / 500));
        mapCamera.transform.position = new Vector3(Mathf.Clamp(mapCamera.transform.position.x, 0, 1000), 200f, Mathf.Clamp(mapCamera.transform.position.z, 0, 1000));
    }

    void Start ()
	{
        
        mapCamera = mapCameraObject.GetComponent<Camera>();
	}

    public void ZoomCamera(float z)
    {
        mapCamera.orthographicSize = (z * 500) + 25;
    }
    public void ResetCamera()
    {
        mapCamera.orthographicSize = 25;
        mapCameraObject.transform.localPosition = new Vector3(0f, 200f, 0f);
    }
}
