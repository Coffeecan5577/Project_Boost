using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float rocketThrust = 15f;


    private bool _spaceKeyPressed;
    private bool _aKeyPressed;
    private bool _dKeyPresssed;
    private Rigidbody _rocketRB;
    private AudioSource _audioSource;

	// Use this for initialization
	void Start ()
	{
	    _rocketRB = GetComponent<Rigidbody>();
	    _audioSource = GetComponent<AudioSource>();
	    _rocketRB.mass = 1;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    ProcessInput();
	}

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
            {
                print("OK");
            }
            break;

            case "Fuel":
            {
                // Future tag
            }
            break;

            default:
            {
                print("dead");
            }
            break;
        }
    }

    private void ProcessInput()
    {
        _spaceKeyPressed = Input.GetKey(KeyCode.Space);
        _aKeyPressed = Input.GetKey(KeyCode.A);
        _dKeyPresssed = Input.GetKey(KeyCode.D);

        Thrust();
        Rotate();
    }

    private void Thrust()
    {
        if (_spaceKeyPressed)
        {
            _rocketRB.AddRelativeForce(Vector3.up * rocketThrust);
            if (!_audioSource.isPlaying) // so it do
            {
                _audioSource.Play();
            }
        }

        else
        {
            _audioSource.Stop();
        }
    }

    private void Rotate()
    {
        _rocketRB.freezeRotation = true; // take manual control of rotation.
        float rotationThisFrame = Time.deltaTime * rcsThrust;

        if (_aKeyPressed)
        {
            transform.Rotate(new Vector3(0, 0, 2 * rotationThisFrame), Space.World);
        }
        else if (_dKeyPresssed)
        {
            transform.Rotate(new Vector3(0, 0, -2 * rotationThisFrame), Space.World);
        }

        _rocketRB.freezeRotation = false; // resume physics' control of rotation.
    }

}
