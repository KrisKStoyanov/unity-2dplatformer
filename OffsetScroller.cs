using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetScroller : MonoBehaviour {

    public float scrollSpeed;
    private Vector2 savedOffset;
    private Renderer renderer;

	// Use this for initialization
	void Start () {
        renderer = GetComponent<Renderer>();
        savedOffset = renderer.sharedMaterial.GetTextureOffset("_MainTex");
        StartCoroutine(ScrollingBackground());
	}

    private void OnDisable()
    {
        renderer.sharedMaterial.SetTextureOffset("_MainTex", savedOffset);
    }

    private IEnumerator ScrollingBackground()
    {
        while (true)
        {
            float x = Mathf.Repeat(Time.time * scrollSpeed, 1);
            Vector2 offset = new Vector2(x, savedOffset.y);
            renderer.sharedMaterial.SetTextureOffset("_MainTex", offset);
            yield return null;
        }
    }
}
