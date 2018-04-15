using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    public GameObject platform;
    public float moveSpeed;
    private Transform currentPoint;
    public Transform[] points;
    public int pointSelection;

	// Use this for initialization
	void Start () {
        currentPoint = points[pointSelection];
        if (platform.transform.position == currentPoint.position)
        {
            pointSelection++;

            if (pointSelection == points.Length)
            {
                pointSelection = 0;
            }

            currentPoint = points[pointSelection];
        }
        StartCoroutine(MovePlatform());
	}


    private IEnumerator MovePlatform()
    {
        while (true)
        {
            platform.transform.position = Vector3.MoveTowards(platform.transform.position, currentPoint.position, Time.deltaTime * moveSpeed);
            currentPoint = points[pointSelection];
            if (platform.transform.position == currentPoint.position)
            {
                pointSelection++;

                if (pointSelection == points.Length)
                {
                    pointSelection = 0;
                }

                currentPoint = points[pointSelection];
            }
            yield return null;
        }
    }
}
