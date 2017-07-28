using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarSelectListContent : MonoBehaviour {

    [SerializeField]
    public GameObject carSelectComponent;

    private List<GameObject> carsSubjects;
    private List<GameObject> carsComponents = new List<GameObject>();

    void initCars () {

        for (int i = 0; i < carsSubjects.Count; i++)
	    {
	        GameObject carSC = Instantiate(carSelectComponent) as GameObject;
	        carSC.transform.SetParent(transform, false);
            carsComponents.Add(carSC);

	        CarSelectControllerS csc = carSC.GetComponent<CarSelectControllerS>();
	        if (csc != null) csc.SetID(i);

        }

    }
	
	void Update () {

	    if (carsSubjects == null)
	    {
	        if (SimMaster.instance.testSubjects != null)
	        {
	            carsSubjects = SimMaster.instance.testSubjects;
	            initCars();

	        }
	        else
	        {
	            return;
	        }
	    }

	    for (int i = 0; i < carsComponents.Count; i++)
	    {
	        CarSelectControllerS csc = carsComponents[i].GetComponent<CarSelectControllerS>();
	        if (csc != null)
	        {
	            csc.SetID(i);
	            csc.SetFitness(SimMaster.instance.testSubjects[i].GetComponent<Car>().fitness);
            }
	    }
    }
}
