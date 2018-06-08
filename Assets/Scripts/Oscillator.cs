using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent] // Attribute
public class Oscillator : MonoBehaviour
{
    [SerializeField] private Vector3 _movementVector;

    // TODO remove from inspector later.
    [Range(0, 1)]
    [SerializeField] //Attribute
    private float _movementFactor; // 0 for not moved, 1 for fully moved.

    private Vector3 _startingPosition;

	// Use this for initialization
	void Start ()
	{
	    _startingPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    Vector3 offset = _movementFactor * _movementVector;
	    transform.position = _startingPosition + offset;
	}
}
