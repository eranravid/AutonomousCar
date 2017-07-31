using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class hit : MonoBehaviour {

	public int checkpoints;
	public Material Passed;
	public bool crash;
    public List<uint> checkpointsEaten = new List<uint>();
    public int reviveTime = 30; // in seconds

	// Use this for initialization
	void Start () {
		crash = false;
		checkpoints = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Checkpoint")
		{
		    Checkpoint cp = other.GetComponent<Checkpoint>();
            if (!checkpointsEaten.Contains(cp.id))
                addCheckpoint(cp);		
            
        } else if (other.gameObject.tag != "SpawnPoint") {
			//hit a wall considered fail
			crash = true;
		}
	}

    void addCheckpoint(Checkpoint cp)
    {
        checkpoints++;
        checkpointsEaten.Add(cp.id);
        StartCoroutine(checkpointTimer(cp.id));
    }

    IEnumerator checkpointTimer(uint id)
    {

        yield return new WaitForSeconds(reviveTime);

        checkpointsEaten.Remove(id);

    }

    public void reset()
    {
        checkpointsEaten.Clear();
        checkpoints = 0;
        crash = false;
    }
}
