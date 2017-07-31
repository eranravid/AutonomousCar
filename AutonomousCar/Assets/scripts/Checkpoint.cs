using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    public static uint count = 0;

    public uint id = 0;

	// Use this for initialization
	void Start ()
	{
	    
	    id = count;
	    count++;
    }

}
