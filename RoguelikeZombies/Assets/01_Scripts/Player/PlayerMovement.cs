using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour, IMovable
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float mouseSensitivity = 120f;
    [SerializeField] private float gravity = -9.81f;

    private CharacterController cc;
    private Vector3 velocity;
    private float camRotX;

    void Awake() { cc = GetComponent<CharacterController>(); Cursor.lockState = CursorLockMode.Locked; }

    void Update()
    {
        // Rotación horizontal con mouse
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);

        // Rotación vertical en la cámara hija
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        camRotX = Mathf.Clamp(camRotX - mouseY, -80f, 80f);
        if (Camera.main) Camera.main.transform.localEulerAngles = new Vector3(camRotX, 0, 0);

        // Movimiento WASD
        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");
        var dir = transform.right * x + transform.forward * z;
        Move(dir.normalized, speed);

        // Gravedad simple
        if (cc.isGrounded && velocity.y < 0) velocity.y = -2f;
        velocity.y += gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);
    }
    public void Move(Vector3 direction, float spd) => cc.Move(direction * spd * Time.deltaTime);
}
