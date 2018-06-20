using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    SimMaster is the simulation runner.
 */
public class SimMaster : MonoBehaviour
{
    public static SimMaster instance;
    public GameObject carPrefab;
    public GGA gga;
    public List<GameObject> testSubjects = new  List<GameObject>();
    
    public GameObject[] SPs;

    public Car selectedCar;
    public bool lockBestFitness = false;

    public void OnGUI()
    {
        selectedCar.GetComponent<Car>().selectThisCar();
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }
    }


    // Use this for initialization
    void Start ()
    {
        // initialize Genetic Algorithm
        gga = new GGA();
        GGA.bestFitness = 0;

        // initialize test subjects
        SPs = GameObject.FindGameObjectsWithTag("SpawnPoint");
        Transform t;
        foreach (GameObject c in SPs)
        {
            // create car in place
            t = c.gameObject.GetComponent<Transform>();
            GameObject car = (GameObject)Instantiate(carPrefab,t.transform); ;
            // initiate car component
            Car carComp = car.GetComponent<Car>();
            carComp.master = this;
            carComp.id = gga.liveGenomes.Count;
            testSubjects.Add(car);
            // initiate as test subject by gga
            NextTestSubject(carComp);
            selectedCar = carComp;
        }        

    }
	
	// Update is called once per frame
	void Update ()
	{
	    int currBestFitness = 0;
	    gga.CalculateAvarageFitness();
        for (int i=0; i< testSubjects.Count; i++)
        {
            // if test subject failed start a new one
            Car testAgent = testSubjects[i].GetComponent<Car>();
            if (testAgent.hasFailed)
            {                
                gga.killGenome(testAgent.genome);
                testAgent.ClearFailure(); // reset subject
                NextTestSubject(testAgent); // start new subject
            }

            // best fitness
            if (testAgent.fitness > GGA.bestFitness)
            {
                GGA.bestFitness = testAgent.fitness;                
            }
            if (testAgent.fitness > currBestFitness)
            {
                currBestFitness = testAgent.fitness;
                if (lockBestFitness)
                {
                    testAgent.selectThisCar();
                }
            }
        }
       
    }

    public void NextTestSubject(Car testAgent)
    {
        GGenome gen = gga.getNextGenome();
        if (gen != null)
        {
            testAgent.setGenome(gen);
        }
    }
}
