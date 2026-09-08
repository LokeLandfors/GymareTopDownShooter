using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] KeyCode moveUp;
    [SerializeField] KeyCode moveDown;
    [SerializeField] KeyCode moveRight;
    [SerializeField] KeyCode moveLeft;
    Vector3 moveDir => new Vector3(0,0,0);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
