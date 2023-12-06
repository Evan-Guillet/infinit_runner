using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipControl : MonoBehaviour
{
    [SerializeField]
    private float translationSpeed;
    private const float MIN_X = -9;
    private const float MAX_X = 9;

    [SerializeField]
    private float moveSpeed;
    private float currentSpeed = 0;

    private ParticleSystem FlameLeft;
    private ParticleSystem FlameRight;

    private bool accelerate;

    void Start()
    {
        transform.position = new Vector3(0, 0.2f, 0);

        FlameLeft = GameObject.Find("FlameLeft").GetComponent<ParticleSystem>();
        FlameRight = GameObject.Find("FlameRight").GetComponent<ParticleSystem>();

        accelerate = false;
    }

    void ChangFlame(ParticleSystem system, bool accelerate){
        if(!accelerate){
            var main = system.main;
            main.startSpeed = 0;
            main.startSize = 0;

        } else {
            var main = system.main;
            main.startSpeed = 2;
            main.startSize = 1.25f;
        }
    }

    void Update()
    {
        // VERTICAL AXIS
        float verticalMove = Input.GetAxis("Vertical") * translationSpeed * Time.deltaTime;
        transform.position += transform.up * verticalMove;

        if(transform.position.y > 2)
            transform.position = new Vector3(transform.position.x, 2f, transform.position.z);
        if(transform.position.y < 0.2)
            transform.position = new Vector3(transform.position.x, 0.2f, transform.position.z);

        // HORIZONTAL AXIS
        float horizontalMove = Input.GetAxis("Horizontal") * translationSpeed * Time.deltaTime;
        transform.position += transform.right * horizontalMove;

        if(transform.position.x < MIN_X)
            transform.position = new Vector3(MIN_X, transform.position.y, transform.position.z);
        if(transform.position.x > MAX_X)
            transform.position = new Vector3(MAX_X, transform.position.y, transform.position.z);
        
        // FORWARD AXIS
        if(Input.GetButton("Fire2")){
            currentSpeed += 0.1f * Time.deltaTime;

            if(currentSpeed > moveSpeed)
                currentSpeed = moveSpeed;

            if(!accelerate){
                accelerate = true;

                ChangFlame(FlameLeft, accelerate);
                ChangFlame(FlameRight, accelerate);
            }

        } else {
            if(currentSpeed > 0){
                currentSpeed -= 0.1f * Time.deltaTime;

                if(currentSpeed < 0)
                    currentSpeed = 0;
            }

            if(accelerate){
                accelerate = false;

                ChangFlame(FlameLeft, accelerate);
                ChangFlame(FlameRight, accelerate);
            }
        }

        transform.position += transform.forward * currentSpeed;
    }

    private void OnTriggerEnter(Collider other){
        Debug.Log("Collision!");
        GameController.Instance.Health -= 10;
    }
}
