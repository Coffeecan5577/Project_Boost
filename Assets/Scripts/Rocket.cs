﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float rocketThrust = 15f;

    private bool _spaceKeyPressed;
    private bool _aKeyPressed;
    private bool _dKeyPresssed;
    private Rigidbody _rocketRB;

	// Use this for initialization
	void Start ()
	{
	    _rocketRB = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	    ProcessInput();
	}

    private void ProcessInput()
    {
        _spaceKeyPressed = Input.GetKey(KeyCode.Space);
        _aKeyPressed = Input.GetKey(KeyCode.A);
        _dKeyPresssed = Input.GetKey(KeyCode.D);

        if (_spaceKeyPressed)
        {
            _rocketRB.AddRelativeForce(new Vector3(0, rocketThrust, 0));
        }

        if (_aKeyPressed)
        {
            transform.Rotate(new Vector3(0, 0, 2), Space.World);
        }
        else if (_dKeyPresssed)
        {
            transform.Rotate(new Vector3(0, 0, -2), Space.World);
        }
    }
}