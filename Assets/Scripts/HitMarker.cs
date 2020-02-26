using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HitMarker : MonoBehaviour
{
    public float xForce;
    public float yForce;
    public float zForce;
    public TextMeshPro textMesh;
    public MeshRenderer meshRenderer;

    private GameObject player;
    private float xSpeed;
    private float ySpeed;
    private float zSpeed;
    private bool startMoving;

    private GameObject killerCam;

    private void Start()
    {
          xSpeed = (Random.Range(0, 2) * 2 - 1) * Random.value * xForce;
          ySpeed = (Random.Range(0, 2) * 2 - 1) * Random.value * yForce;
          zSpeed = (Random.Range(0, 2) * 2 - 1) * Random.value * zForce;
    }
    public void Popup(GameObject lookAtTarget, Vector3 origin, int damage)
    {
        if (damage != 0)
        {
            textMesh.text = damage.ToString();
            textMesh.color = Color.green;
            textMesh.fontSize = 10;
            meshRenderer.sortingOrder = 2;
        }
        else
        {
            textMesh.text = "Critical Hit";
            textMesh.color = Color.red;
            textMesh.fontSize = 5;
            meshRenderer.sortingOrder = 1;
        }
        // For static X misses
        /*
        transform.position = origin.position + new Vector3((Random.Range(0, 2) * 2 - 1) * Random.Range(.5f, 2f), (Random.Range(0, 2) * 2 - 1) * Random.Range(.5f, 2f), 0f);
        transform.LookAt(lookAtTarget);
        transform.Rotate(0, 180, 0);
        Destroy(this.gameObject, 2f);
        */
        // For moving "miss" misses
        transform.position = origin;
        player = lookAtTarget;
        killerCam = player.GetComponentInChildren<Camera>().gameObject;
        startMoving = true;
        Destroy(this.gameObject, 5f);

    }

    void Update()
    {
        Vector3 lookPos = transform.position - killerCam.transform.position;
      //  lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = rotation;
        if (startMoving)
        {
            transform.Translate(xSpeed, ySpeed, zSpeed); //Z SHOULD BE ZERO!!!
        }
    }
}
