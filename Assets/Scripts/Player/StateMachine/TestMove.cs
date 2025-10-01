using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class TestMove : MonoBehaviour
{
    public Transform cameraTransform;
    private CharacterController cc;
    public float speed = 5f;

    void Awake() => cc = GetComponent<CharacterController>();

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 move = (cameraTransform.forward * v + cameraTransform.right * h);
        move.y = 0;
        move.Normalize();

        cc.Move(move * speed * Time.deltaTime);
    }
}
