using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent] // Attribute
public class Oscillator : MonoBehaviour
{
    [SerializeField] private Vector3 _movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] private float period = 2f;

    // TODO remove from inspector later.
    [Range(0, 1)] //Attribute
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
        // Set movement factor automatically
        // TODO protect against the period being 0

	    float cycles = Time.time / period; // grows continually from 0

	    const float tau = Mathf.PI * 2; // tau is 2 * Pi.
	    float rawSineWave = Mathf.Sin(cycles * tau);


	    _movementFactor = rawSineWave / 2f + 0.5f;
	    Vector3 offset = _movementFactor * _movementVector;
	    transform.position = _startingPosition + offset;
	}
}
