using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxRegulator : MonoBehaviour {

    public GameObject startFieldOne;
    public GameObject startFieldTwo;
    public GameObject startFieldThree;

    public GameObject SetOne;
    public GameObject SetTwo;
    public GameObject SetThree;

    public Transform[] ForwardSet;
    public Transform[] MiddleSet;
    public Transform[] BackwardSet;

	// Use this for initialization
	void Start () {
        ForwardSet = new Transform[SetOne.transform.childCount];
        MiddleSet = new Transform[SetTwo.transform.childCount];
        BackwardSet = new Transform[SetThree.transform.childCount];

        for(int i = 0; i < ForwardSet.Length; i++)
        {
            ForwardSet[i] = SetOne.transform.GetChild(i);
        }
        for (int i = 0; i < MiddleSet.Length; i++)
        {
            MiddleSet[i] = SetTwo.transform.GetChild(i);
        }
        for (int i = 0; i < BackwardSet.Length; i++)
        {
            BackwardSet[i] = SetThree.transform.GetChild(i);
        }

        ForwardSet[0].transform.position = new Vector2(startFieldOne.transform.position.x + ForwardSet[0].transform.position.x, startFieldOne.transform.position.y);


    }
}
