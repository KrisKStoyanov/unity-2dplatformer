using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParallax : MonoBehaviour {

    public Transform[] backgrounds;
    public float parallaxScale;
    public float parallaxReductionFactor;
    public float smoothing;

    private Vector3 _lastPosition;

	// Use this for initialization
	void Start () {
        _lastPosition = transform.position;
        StartCoroutine(ParallaxRunner());
        //InvokeRepeating("ParallaxRun", 0, 0.1f);
	}
	

    private IEnumerator ParallaxRunner()
    {
        while (true)
        {
            //yield return new WaitForEndOfFrame();
            var parallax = (_lastPosition.x - transform.position.x) * parallaxScale;
            for (var i = 0; i < backgrounds.Length; i++)
            {
                var backgroundTargetPosition = backgrounds[i].position.x + parallax * (i * parallaxReductionFactor + 1);
                backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, new Vector3(backgroundTargetPosition, backgrounds[i].position.y, backgrounds[i].position.z), smoothing * Time.deltaTime);
            }
            //yield return new WaitForEndOfFrame();
            _lastPosition = transform.position;
            yield return null;
        }
    }
}
