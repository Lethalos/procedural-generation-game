using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputManager : SingletonBehaviour<InputManager>
{
    public ButtonAnimation buttonAnimation;
    private Coroutine activeCoroutine;
    private PoolManager poolManager;
    private List<GameObject> objectsToToggle = new List<GameObject>(); // Create a separate list
    private CameraManager cameraManager; // Reference to the CameraManager script
    private Vector3 spawnPosition;
    private GameObject clickedObject;

    public float rotationSpeed = 5f; // Speed of camera rotation

    private Vector3 fp; // first touch/mouse pos
    private Vector3 lp; // last touch/mouse pos
    private float dragDistance; // minimum dist for a swipe

    private MountainTurn mountainTurn;
    public Transform mountain;
    private int layerMask = 1 << 6;

    public float lastInputTime;
    public float idleTimeout = 30f; // Timeout duration for idle state in seconds


    private void Start()
    {
        cameraManager = FindObjectOfType<CameraManager>(); // Find and assign the CameraManager component
        mountainTurn = FindObjectOfType<MountainTurn>();
        dragDistance = Screen.height * 15f / 100f;
        poolManager = PoolManager.Instance;
        if(buttonAnimation != null)
        {
            buttonAnimation = buttonAnimation.GetComponent<ButtonAnimation>();
        }
        
    }

    private void Update()
    {
        if (GameManager.Instance.currentGameState == GameState.Active)
        {
           
            OnClick();
            HandleTouchInput();
            HandleMouseInput();
        }

        CheckUserInput();
    }

    private void OnClick()
    {   
        

        //if (GameManager.Instance.currentGameState == GameState.Idle)
        //{   
        //    if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began))
        //    {
        //        // Change the game state to active without zooming in
        //        GameManager.Instance.ChangeGameState(GameState.Active);
        //        return;
        //    }
        //}

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                clickedObject = hit.transform.gameObject;
                HandleObjectClicked(clickedObject);
                poolManager.DeactivateAllPoolObjects();
            }
            else
            {
                // Zoom out and activate the first objects of each type
                // cameraManager.ZoomOut();
                WeatherManager.Instance.ClearWeather();
                //poolManager.DeactivateAllPoolObjects();
                UIManager.Instance.HideAllPopUps();
                //poolManager.ActivateFirstObjects();
               
            }
        }
        else if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                clickedObject = hit.transform.gameObject;
                HandleObjectClicked(clickedObject);
                poolManager.DeactivateAllPoolObjects();
            }
            else
            {
                // Zoom out and activate the first objects of each type
                //cameraManager.ZoomOut();
                WeatherManager.Instance.ClearWeather();
                //poolManager.DeactivateAllPoolObjects();
                UIManager.Instance.HideAllPopUps();
                //poolManager.ActivateFirstObjects();
                
            }
        }
    }

    private void HandleObjectClicked(GameObject clickedObject)
    {
        // Check if the object is already clicked
        bool isObjectClickedBefore = clickedObject.GetComponent<ClickableObject>().IsClicked;

        if (!isObjectClickedBefore)
        {
            //poolManager.DeactivatePoolObjectsExcept(clickedObject);
           // poolManager.ActivateSameTypeObjects(clickedObject);

            // Zoom in on the first click
            //cameraManager.ZoomIn(clickedObject);
        }
        else
        {
            //poolManager.DeactivateSecondAndThirdObjects(clickedObject);
            // Deactivate all objects in the pool
            //poolManager.DeactivateAllPoolObjects();
            UIManager.Instance.HideAllPopUps();
            // Activate the first objects of each type
            //poolManager.ActivateFirstObjects();
            WeatherManager.Instance.ClearWeather();
            // Zoom out
           // cameraManager.ZoomOut();
           
        }

        // Update the clicked state of the object
        clickedObject.GetComponent<ClickableObject>().IsClicked = !isObjectClickedBefore;
        PoolObjectType objectType = poolManager.GetObjectTypeFromGameObject(clickedObject);
        if (objectType != PoolObjectType.None)
        {
            // Do something with the pool object type (e.g., pass it to the WeatherManager)
            WeatherManager.Instance.ChangeWeather(clickedObject);
        }
        
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                fp = touch.position;
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                lp = touch.position;

                // Calculate the horizontal swipe distance
                float swipeDistanceX = lp.x - fp.x;

                if (swipeDistanceX > 0)
                {
                    RightSwipe();
                }
                else if (swipeDistanceX < 0)
                {
                    LeftSwipe();
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                lp = touch.position;
                if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
                {
                    if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
                    {
                        if (lp.x > fp.x)
                        {
                            RightSwipe();
                        }
                        else
                        {
                            LeftSwipe();
                        }
                    }
                }
            }
        }
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            fp = Input.mousePosition;
            lp = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            lp = Input.mousePosition;

            // Calculate the horizontal swipe distance
            float swipeDistanceX = lp.x - fp.x;

            if (swipeDistanceX > 0)
            {
                RightSwipe();
                mountainTurn.RightSwipe();
            }
            else if (swipeDistanceX < 0)
            {
                LeftSwipe();
                mountainTurn.LeftSwipe();
            }
        }
    }

    private void RightSwipe()
    {
        // Rotate the camera around the target to the right
        Camera.main.transform.RotateAround(mountain.position, Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void LeftSwipe()
    {
        // Rotate the camera around the target to the left
        Camera.main.transform.RotateAround(mountain.position, Vector3.down, rotationSpeed * Time.deltaTime);
    }

    private void CheckUserInput()
    {
        if (GameManager.Instance.currentGameState == GameState.Active)
        {
            UIManager.Instance.infoPage.gameObject.SetActive(false);
            //UIManager.Instance.infoPanel.gameObject.SetActive(true);
            // Reset the idle timer if the user interacts with the screen
            if ((Input.GetMouseButtonDown(0) || Input.touchCount > 0) && IsButtonClicked() == false)
            {
                lastInputTime = Time.time;
                
            }
            else if (Time.time - lastInputTime >= idleTimeout)
            {
                // If the user hasn't interacted with the screen for the specified idle timeout period,
                // change the game state to idle
                GameManager.Instance.ChangeGameState(GameState.Idle);
                Debug.Log("No interaction to screen. Game state changed to the idle state.");
                UIManager.Instance.idleObjectParent.gameObject.SetActive(true);
                UIManager.Instance.infoPanel.gameObject.SetActive(true);
                UIManager.Instance.IdleUIInfoPanel();
                PoolManager.Instance.ActivateFirstObjects();
                UIManager.Instance.HideAllPopUps();
                UIManager.Instance.infoPage.gameObject.SetActive(false);
                buttonAnimation.StartScaleAnimation();
            }
        }
        else if (GameManager.Instance.currentGameState == GameState.Idle)
        {
            // Check for user input to change the game state
            if ((Input.GetMouseButtonDown(0) || Input.touchCount > 0) && IsButtonClicked() == false)
            {   
                UIManager.Instance.ApplyFadeOutEffectIdle();
                StartCoroutine(WaitCoroutine(2f));
                
                lastInputTime = Time.time;
            }
        }
        else if (GameManager.Instance.currentGameState == GameState.Info)
        {
            
            // Check for user input to change the game state
            if (Input.GetMouseButtonDown(0))
            {
                GameManager.Instance.ChangeGameState(GameState.Active);
                lastInputTime = Time.time;
                
                UIManager.Instance.infoGrayBack.gameObject.SetActive(false);
                UIManager.Instance.infoGrayCircle.gameObject.SetActive(false);
                UIManager.Instance.idleObjectParent.gameObject.SetActive(false);
                UIManager.Instance.HideInfoPage();
                UIManager.Instance.infoPage.gameObject.SetActive(false);
                buttonAnimation.StartScaleAnimation();
                UIManager.Instance.HideAllPopUps();
                poolManager.DeactivateAllPoolObjects();
            }
            else if (Time.time - lastInputTime >= idleTimeout)
            {
                // If the user hasn't interacted with the screen for the specified idle timeout period,
                // change the game state to idle
                GameManager.Instance.ChangeGameState(GameState.Idle);
                Debug.Log("No interaction to screen. Game state changed to the idle state.");
                UIManager.Instance.idleObjectParent.gameObject.SetActive(true);
                UIManager.Instance.IdleUIInfoPanel();
                PoolManager.Instance.ActivateFirstObjects();
                UIManager.Instance.HideAllPopUps();
                UIManager.Instance.infoPage.gameObject.SetActive(false);
            }
        }
    }

    IEnumerator WaitCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameManager.Instance.ChangeGameState(GameState.Active);
    }


    public bool IsButtonClicked()
    {
        
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            Button button = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
            if (button != null)
            {
                return true;
            }

            Debug.Log("Button: "+ button.gameObject.name);
        }
        
         return false;
    }

    

}
