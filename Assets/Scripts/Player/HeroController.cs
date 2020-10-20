using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class HeroController : MonoBehaviour
{
    [Tooltip("Character settings")]
    public float MoveSpeed = 10f, JumpForce = 10f, Sensitivity = 120f;
    public bool HideCursor = true;
    public bool AndroidControl;
    public VariableJoystick JoystickMove, JoystickView;
    [Tooltip("if interface enabled, swipe is work")]
    public GameObject MainInterface;
    public Transform CamEmpty;

    bool isGround = false;
    float xmouse, ymouse, gravityVector = -1f;
    CharacterController character;
    Vector3 moveVector;
    Vector2 swipeStartPos;
    float yRotation;
    MenuManager menu;
    Touch tch;

    void Start()
    {
        Debug.Log("screen width " + Screen.width);
        character = GetComponent<CharacterController>();
        menu = FindObjectOfType<MenuManager>();

        if (HideCursor)
        {
            Cursor.lockState = CursorLockMode.Locked; // freeze cursor on screen centre
            Cursor.visible = false; // invisible cursor
        }
    }

    void Update()
    {
        #region imnput
        if (AndroidControl)
        {
            if (menu.ViewDrop.value == 3 && MainInterface.activeSelf) // swipe enabled
            {
                if (Input.touchCount == 1 && Input.GetTouch(0).rawPosition.x > Screen.width / 2)
                {
                    tch = Input.GetTouch(0);
                }
                else if (Input.touchCount == 2)
                {
                    if (Input.GetTouch(0).rawPosition.x > Screen.width / 2) tch = Input.GetTouch(0);
                    else if (Input.GetTouch(1).rawPosition.x > Screen.width / 2) tch = Input.GetTouch(1);
                }

                if (tch.phase == TouchPhase.Began) swipeStartPos = tch.rawPosition;
                else if (tch.phase == TouchPhase.Moved)
                {
                    xmouse = tch.position.x - swipeStartPos.x;
                    ymouse = tch.position.y - swipeStartPos.y;

                    xmouse *= Time.deltaTime * Sensitivity / 10;
                    ymouse *= Time.deltaTime * Sensitivity / 10;

                    swipeStartPos = tch.position;
                }
                else
                {
                    xmouse = 0f;
                    ymouse = 0f;
                }
            }
            else
            {
                xmouse = JoystickView.Horizontal * Time.deltaTime * Sensitivity;
                ymouse = JoystickView.Vertical * Time.deltaTime * Sensitivity;
            }
        }
        else
        {
            xmouse = Input.GetAxis("Mouse X") * Time.deltaTime * Sensitivity;
            ymouse = Input.GetAxis("Mouse Y") * Time.deltaTime * Sensitivity;
        }
        // TextDebug.TextComponent.text ="touchs: " + Input.touchCount.ToString();
        #endregion

        #region rotation
        transform.Rotate(Vector3.up * xmouse);
        yRotation -= ymouse;
        yRotation = Mathf.Clamp(yRotation, -85f, 60f);
        CamEmpty.transform.localRotation = Quaternion.Euler(yRotation, 0, 0);
        #endregion

        #region jump
        if (Physics.CheckSphere(transform.position - transform.up * 1.3f, .2f) && gravityVector < 0f) isGround = true;
        if (gravityVector > -8f) gravityVector -= Time.deltaTime * 10;
        if (Input.GetButtonDown("Jump") && isGround)
        {
            gravityVector = JumpForce;
            isGround = false;
        }
        #endregion

        #region moving result
        if (AndroidControl)
        {
            moveVector = transform.forward * JoystickMove.Vertical * MoveSpeed +
            transform.right * JoystickMove.Horizontal * MoveSpeed +
            transform.up * gravityVector;
            character.Move(moveVector * Time.deltaTime);
        }
        else
        {
            moveVector = transform.forward * MoveSpeed * Input.GetAxis("Vertical") +
            transform.right * MoveSpeed * Input.GetAxis("Horizontal") +
            transform.up * gravityVector;
            character.Move(moveVector * Time.deltaTime);
        }
        #endregion
    }

    public void AndroidJump()
    {
        if (AndroidControl && isGround)
        {
            gravityVector = JumpForce;
            isGround = false;
        }
    }
}
