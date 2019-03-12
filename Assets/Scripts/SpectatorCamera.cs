using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectatorCamera : MonoBehaviour {

    public float height = 5;
    public float radius = 5;
    public float speed = 10; // Degrees per second

    private float currentDegrees = 0;

	private void Start () {
        transform.position = GetPositionOnCircle(0);
	}
	
	private void Update () {
        transform.position = GetPositionOnCircle(currentDegrees);
        transform.LookAt(Vector3.zero, Vector3.up);
        currentDegrees = (currentDegrees + speed * Time.deltaTime) % 360;
	}

    private Vector3 GetPositionOnCircle(float degrees) {
        float radians = degrees * Mathf.PI / 180;
        float x = radius * Mathf.Cos(radians);
        float z = radius * Mathf.Sin(radians);
        return new Vector3(x, height, z);
    }
}
