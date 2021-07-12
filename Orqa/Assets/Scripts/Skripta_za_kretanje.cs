using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Skripta_za_kretanje : MonoBehaviour
{
    Rigidbody ourDrone;

    void Awake()
    {
        ourDrone = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        MovementUpDown();
        MovementForward();
        Rotation();
        ClampingSpeedValues();
        Skretanja();

        ourDrone.AddRelativeForce(Vector3.up * upForce);
        ourDrone.rotation = Quaternion.Euler(
            new Vector3(tiltAmoundForward, currentYRotation, tiltAmoundSideways)
        );

    }

    public float nagib;
    public float masa;
    public float snaga;


    public void masaSlider(float novaMasa)
    {
        masa = novaMasa;
    }
    public void snagaSlider(float novaSnaga)
    {
        snaga = novaSnaga;
    }
    public void nagibSlider(float noviNagib)
    {
        nagib = noviNagib;
    }

    private float upForce;
    void settingUpForce()
    {
        upForce = snaga / masa;
    }
    void MovementUpDown()
    {
        if ((Mathf.Abs(Input.GetAxis("Vertical")) > 0.2f || Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f))
        {
            if (Input.GetKey(KeyCode.I) || Input.GetKey(KeyCode.K))
            {
                ourDrone.velocity = ourDrone.velocity;
            }
            if (!Input.GetKey(KeyCode.I) && !Input.GetKey(KeyCode.K) && !Input.GetKey(KeyCode.J) && !Input.GetKey(KeyCode.L))
            {
                ourDrone.velocity = new Vector3(ourDrone.velocity.x, Mathf.Lerp(ourDrone.velocity.y, 0, Time.deltaTime * 5), ourDrone.velocity.z);
                upForce /= 1.6f;
            }
            if (!Input.GetKey(KeyCode.I) && !Input.GetKey(KeyCode.K) && (Input.GetKey(KeyCode.J) || Input.GetKey(KeyCode.L)))
            {
                ourDrone.velocity = new Vector3(ourDrone.velocity.x, Mathf.Lerp(ourDrone.velocity.y, 0, Time.deltaTime * 5), ourDrone.velocity.z);
                upForce /= 4f;
            }
            if (Input.GetKey(KeyCode.J) || Input.GetKey(KeyCode.L))
            {
                upForce /= 1.1f;
            }

        }

        if (Mathf.Abs(Input.GetAxis("Vertical")) < 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
        {
            upForce /= 3.3f;
        }



        if (Input.GetKey(KeyCode.I))
        {
            upForce = snaga / masa;
            if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
            {
                upForce /= 0.9f;
            }
        }
        else if (Input.GetKey(KeyCode.K))
        {
            upForce = snaga / masa;
            upForce /= -2.2f;
        }
        else if (!Input.GetKey(KeyCode.I) && !Input.GetKey(KeyCode.K) && (Mathf.Abs(Input.GetAxis("Vertical")) < .2f && Mathf.Abs(Input.GetAxis("Horizontal")) < 0.2f))
        {
            upForce = -98.1f * masa;
        }


    }
    private float movementForwardSpeed = 500.0f;
    private float tiltAmoundForward = 0;
    private float titltVelocityForward;
    void MovementForward()
    {
        if (Input.GetAxis("Vertical") != 0)
        {
            ourDrone.AddRelativeForce(Vector3.forward * Input.GetAxis("Vertical") * movementForwardSpeed);
            tiltAmoundForward = Mathf.SmoothDamp(tiltAmoundForward, nagib * Input.GetAxis("Vertical"), ref titltVelocityForward, 0.1f);

        }
    }

    private float wantedYRotation;
    [HideInInspector] public float currentYRotation;
    private float rotateAmountByKEys = 2.5f;
    private float rotationYVelocity;
    void Rotation()
    {
        if (Input.GetKey(KeyCode.J))
        {
            wantedYRotation -= rotateAmountByKEys;
        }
        if (Input.GetKey(KeyCode.L))
        {
            wantedYRotation += rotateAmountByKEys;
        }

        currentYRotation = Mathf.SmoothDamp(currentYRotation, wantedYRotation, ref rotationYVelocity, 0.25f);
    }


    private Vector3 velocityUsporavanja;
    void ClampingSpeedValues()
    {
        if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
        {
            ourDrone.velocity = Vector3.ClampMagnitude(ourDrone.velocity, Mathf.Lerp(ourDrone.velocity.magnitude, 10.0f, Time.deltaTime * 5f));
        }
        if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) < 0.2f)
        {
            ourDrone.velocity = Vector3.ClampMagnitude(ourDrone.velocity, Mathf.Lerp(ourDrone.velocity.magnitude, 10.0f, Time.deltaTime * 5f));
        }
        if (Mathf.Abs(Input.GetAxis("Vertical")) < 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
        {
            ourDrone.velocity = Vector3.ClampMagnitude(ourDrone.velocity, Mathf.Lerp(ourDrone.velocity.magnitude, 5.0f, Time.deltaTime * 5f));
        }
        if (Mathf.Abs(Input.GetAxis("Vertical")) < 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) < 0.2f)
        {
            ourDrone.velocity = Vector3.SmoothDamp(ourDrone.velocity, Vector3.zero, ref velocityUsporavanja, 0.95f);
        }
    }



    private float sideMovementAmount = 300.0f;
    private float tiltAmoundSideways;
    private float tiltAmoundVelocity;
    void Skretanja()
    {
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
        {
            ourDrone.AddRelativeForce(Vector3.right * Input.GetAxis("Horizontal") * sideMovementAmount);
            tiltAmoundSideways = Mathf.SmoothDamp(tiltAmoundSideways, -nagib * Input.GetAxis("Horizontal"), ref tiltAmoundVelocity, 0.1f);
        }
        else
        {
            tiltAmoundSideways = Mathf.SmoothDamp(tiltAmoundSideways, 0, ref tiltAmoundVelocity, 0.1f);
        }

    }

}