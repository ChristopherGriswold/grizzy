using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomTouchPad : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler
{
    public GameObject fpsCam;
    public GameObject itemDatabase;
    public GameObject player;
    public GameObject weaponSlot;
    public GameObject objectHit;
    public GameObject targetCanvas;
    public GameObject dialoguePanel;
    public GameObject menuButton;
    public GameObject miniMapButton;
    public GameObject weaponButton;
    public float verticalLookSpeed;
    public float horizontalLookSpeed;
    public float rayLength;

    private Ray ray;
    private Camera cam;
    private RaycastHit hit;
    private RaycastHit lineHit;
    private Slot weaponSlotSlot;
    public FireWeapon fireWeapon;
    private TargetHandler targetHandler;
    private RawImage menuButtonImage;
    private float tiltAngle;
    private Vector3 screenPoint;
    private bool onScreen;
    private bool clickedOnce;
    private float doubleClickTimeLimit = 0.25f;
    public int taps;


    private Vector2 touchDeltaPosition;
    private LayerMask enemyLayerMask;
    private LayerMask npcLayerMask;
    private LayerMask interactableLayerMask;
    private int targetRange;
    private bool isFiring;
    private bool doubleTapStillActive;
    private bool singleTapStillActive;
    
    private void Start()
    {
        cam = fpsCam.GetComponent<Camera>();
        targetHandler = targetCanvas.GetComponent<TargetHandler>();
        weaponSlotSlot = weaponSlot.GetComponent<Slot>();
        enemyLayerMask = 1 << 15;
        npcLayerMask = 1 << 16;
        interactableLayerMask = 1 << 10;
        menuButtonImage = menuButton.GetComponent<RawImage>();


        int defaultValue = EventSystem.current.pixelDragThreshold;
        EventSystem.current.pixelDragThreshold = Mathf.Max(defaultValue, (int)(defaultValue * Screen.dpi / 160f));
    }

    public void RegisterWeapon(GameObject weapon)
    {

        fireWeapon = weapon.GetComponent<FireWeapon>();
        fireWeapon.RegisterTouchPad(this);
    }

    public static bool IsDoubleTap()
    {
        bool result = false;
        float MaxTimeWait = .25f;
        float VariancePosition = .25f;

        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began);
        {
            float DeltaTime = Input.GetTouch(0).deltaTime;
            float DeltaPositionLenght = Input.GetTouch(0).deltaPosition.magnitude;

            if (DeltaTime > 0 && DeltaTime < MaxTimeWait && DeltaPositionLenght < VariancePosition)
                result = true;
        }
        return result;
    }

    private IEnumerator ClickEvent()
    {
        //pause a frame so you don't pick up the same mouse down event.
        yield return new WaitForEndOfFrame();

        float count = 0f;
        int taps = 1;
        while (count < doubleClickTimeLimit)
        {
            if (Input.touchCount > 1)
            {
                DoubleClick();
                yield break;
            }
            count += Time.deltaTime;// increment counter by change in time between frames
            yield return null; // wait for the next frame
        }
        SingleClick();
    }


    private void SingleClick()
    {

    }

    private void DoubleClick()
    {

        if(fireWeapon != null)
        {
            isFiring = true;
            doubleTapStillActive = true;
            fireWeapon.DrawBow();
        }
    }

    private void ThirdClick()
    {
        if (fireWeapon != null && fireWeapon.bowDrawn)
        {
            isFiring = false;
            fireWeapon.CancelDrawBow();
            doubleTapStillActive = false;
        }
    }

    IEnumerator TimeToDoubleTap()
    {
        singleTapStillActive = true;
        yield return new WaitForSeconds(doubleClickTimeLimit);
        singleTapStillActive = false;
        taps = 0;
    }

public void OnDrag(PointerEventData eventData)
    {
        eventData.useDragThreshold = true;
        if (targetHandler.isActiveAndEnabled)
        {
            targetHandler.followTarget = false;
        }
        touchDeltaPosition = eventData.delta;
        player.transform.Rotate(0, touchDeltaPosition.x * horizontalLookSpeed, 0);
        tiltAngle += -touchDeltaPosition.y * verticalLookSpeed;
        tiltAngle = Mathf.Clamp(tiltAngle, -80, 80);
        itemDatabase.transform.localRotation = Quaternion.Euler(tiltAngle, 0, 0);
    }



    public void OnPointerDown(PointerEventData eventData)
    {
        taps++;
        if(taps == 1)
        {
            StartCoroutine(TimeToDoubleTap());
        }
        if (doubleTapStillActive)
        {
            ThirdClick();
            return;
        }
        if (taps > 1 && singleTapStillActive)
        {
            DoubleClick();
            return;
        }

     //   StartCoroutine(ClickEvent());
        ray = cam.ScreenPointToRay(eventData.position);
        if (Physics.Raycast(ray, out hit, rayLength, enemyLayerMask))
        {
            if(objectHit == hit.collider.gameObject)
            {
          //      return;
            }
            objectHit = hit.collider.gameObject;
            targetRange = objectHit.GetComponent<EnemyController>().targetRange;
            if((Vector3.Distance(cam.transform.position, objectHit.transform.GetChild(0).position) <= targetRange))
            {
                AcquireTarget(objectHit);
            }
            else
            {
                objectHit = null;
            }
        }
        else if (Physics.Raycast(ray, out hit, rayLength, npcLayerMask))
        {
            if (objectHit == hit.collider.gameObject)
            {
             //   objectHit.GetComponent<NpcController>().Talk();
           //     ClearTarget();
                return;
            }
            objectHit = hit.collider.gameObject;
            targetRange = objectHit.GetComponent<NpcController>().targetRange;
            if ((Vector3.Distance(cam.transform.position, objectHit.transform.GetChild(0).position) <= targetRange))
            {
                //  targetCanvas.SetActive(true);
                //  targetHandler.AcquireTargetNPC(objectHit);
                menuButtonImage.raycastTarget = false;
                miniMapButton.GetComponent<Button>().interactable = false;
                dialoguePanel.SetActive(true);
                objectHit.GetComponent<DialogueManager>().LinkPanel(dialoguePanel, this.gameObject);
                objectHit.GetComponent<DialogueManager>().PopulateDialogue(true);
                StartCoroutine("CheckIfTargetLocked");
            }
            else
            {
                menuButtonImage.raycastTarget = true;
                miniMapButton.GetComponent<Button>().interactable = true;
                dialoguePanel.SetActive(false);
                objectHit = null;
            }
        }
        else if (Physics.Raycast(ray, out hit, rayLength, interactableLayerMask))
        {
            if (objectHit == hit.collider.gameObject)
            {
                //   objectHit.GetComponent<NpcController>().Talk();
                //     ClearTarget();
                return;
            }
            objectHit = hit.collider.gameObject;
            targetRange = 5;
            if ((Vector3.Distance(cam.transform.position, objectHit.transform.GetChild(0).position) <= targetRange))
            {
                if (objectHit.tag == "CookingStation" && player.GetComponent<DataHandler>().playerData.rewardsPurchased.Contains(2))
                {
                    transform.root.gameObject.GetComponentInChildren<CookingManager>(true).gameObject.SetActive(true);

                    menuButtonImage.raycastTarget = false;
                    miniMapButton.GetComponent<Button>().interactable = false;
                }
                if(objectHit.tag == "RewardShop")
                {

                    transform.root.gameObject.GetComponentInChildren<RewardShop>(true).gameObject.SetActive(true);

                    menuButtonImage.raycastTarget = false;
                    miniMapButton.GetComponent<Button>().interactable = false;
                }
                if (objectHit.tag == "GeneralStore")
                {

                    transform.root.gameObject.GetComponentInChildren<Store>(true).gameObject.SetActive(true);

                    menuButtonImage.raycastTarget = false;
                    miniMapButton.GetComponent<Button>().interactable = false;
                }
                StartCoroutine("CheckIfTargetLocked");
            }
            else
            {
                menuButtonImage.raycastTarget = true;
                miniMapButton.GetComponent<Button>().interactable = true;
                transform.root.gameObject.GetComponentInChildren<CookingManager>(true).gameObject.SetActive(false);
                transform.root.gameObject.GetComponentInChildren<RewardShop>(true).gameObject.SetActive(false);
                objectHit = null;
            }
        }
    }

    public void AcquireTarget(GameObject hit)
    {
        objectHit = hit;
        targetRange = objectHit.GetComponent<EnemyController>().targetRange;
        targetCanvas.SetActive(false);
        targetCanvas.SetActive(true);
        targetHandler.AcquireTarget(hit);
        StartCoroutine("CheckIfTargetLocked");
    }
   

    IEnumerator CheckIfTargetLocked()
    {
        while (objectHit)
        {
            if (weaponSlotSlot.isFilled && objectHit.layer == 15)
            {
              //  weaponButtonHandler.RegisterTarget(objectHit);    POSSIBLE FIX NEEDED HERE
            }
            TargetLocked();
            yield return new WaitForSeconds(.1f);
        }
    }

    public void TargetLocked()
    {
        screenPoint = cam.WorldToViewportPoint(objectHit.transform.GetChild(0).position);
        onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        if (Physics.Linecast(cam.transform.position, objectHit.transform.GetChild(0).position, out lineHit) && onScreen)
        {
            if ((lineHit.collider.gameObject != objectHit && lineHit.collider.gameObject.layer != 13 && lineHit.collider.gameObject.layer != 2) || Vector3.Distance(cam.transform.position, lineHit.transform.GetChild(0).position) > targetRange)
            {
                ClearTarget();
            } 
        }
        else
        {
            ClearTarget();
        }
    }

    public void ClearTarget()
    {
        transform.root.gameObject.GetComponentInChildren<RewardShop>(true).gameObject.SetActive(false);
        dialoguePanel.SetActive(false);

        miniMapButton.GetComponent<Button>().interactable = true;
        transform.root.gameObject.GetComponentInChildren<CookingManager>(true).gameObject.SetActive(false);
        transform.root.gameObject.GetComponentInChildren<Store>(true).gameObject.SetActive(false);
        menuButtonImage.raycastTarget = true;
        StopCoroutine("CheckIfTargetLocked");
        objectHit = null;
        targetHandler.AcquireTarget(null);
        targetHandler.AcquireTargetNPC(null);
        targetHandler.AcquireTargetOtherPlayer(null);
        targetCanvas.SetActive(false);
      //  weaponButtonHandler.RegisterTarget(null); here too
    }



    public void OnPointerUp(PointerEventData eventData)
    {
        if (isFiring == true)
        {
            if (objectHit != null)
            {

                fireWeapon.FireShot(objectHit);
            }
            else
            {
                fireWeapon.FireShot();
            }

            doubleTapStillActive = false;
            isFiring = false;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        float angle = itemDatabase.transform.localEulerAngles.x;
        angle = (angle > 180) ? angle - 360 : angle;
        tiltAngle = angle;
    }
}
