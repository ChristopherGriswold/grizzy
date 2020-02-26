using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public GameObject actionButton;
    public float aimPointRayDistance;
    public GameObject itemDatabase;

    private Ray aimPointRay;
    private Camera thisCamera;

    private LayerMask npcLayerMask;

    void Start()
    {
        npcLayerMask = 1 << 16;
        thisCamera = gameObject.GetComponent<Camera>();
      //  StartCoroutine(ShootAimRay());
    }

    void LateUpdate()
    {
        transform.localRotation = itemDatabase.transform.localRotation;
    }

    IEnumerator ShootAimRay()
    {
        while (true)
        {
            aimPointRay = thisCamera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
            RaycastHit aimPointHit;
            if (Physics.Raycast(aimPointRay, out aimPointHit, aimPointRayDistance, npcLayerMask))
            {
                aimPointHit.collider.gameObject.transform.GetChild(1).gameObject.SetActive(true);
               // actionButton.SetActive(true);
               // actionButtonTextText.text = aimPointHit.transform.name;
            }
            else
            {
                // actionButton.SetActive(false);
                // actionButtonTextText.text = "";
            }
            yield return new WaitForSeconds(.2f);
        }
        
    }
}
