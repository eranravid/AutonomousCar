﻿using System.Collections;
using System.Collections.Generic;
using FANNCSharp.Float;
using UnityEngine;

public class Car : MonoBehaviour
{

    public int id = 0;

    public bool hasFailed = false;

    public float MAX_ROTATION; //max rotate speed
    public float MAX_SPEED;
    public float headingAngle; //Degrees
    public float currentSpeed; // speed
    public float rawAngle = 0.0f;
    public float rawSpeed = 0.0f;

    public SimMaster master;
    //public NNet neuralnet;
    public FANNCSharp.Float.NeuralNet neuralnet;
    public GGenome genome;
    public RayCast raycast;
    public Motor motor;
    public hit hit;    

    public int fitness = 0;
    public int lastFitness = 0; // for killing in not overcome threshold
    public IEnumerator killFitnessRoutine;
    public int killFitnessTime = 20; // in seconds
    public int killFitnessThresh = 1; // how much fitness must do before time interval ends
    public int genomeIndx = 0;

    public Vector3 defaultpos;
    public Quaternion defaultrot;

    public Camera cam;

    IEnumerator killTimerByFitness()
    {
        
        yield return new WaitForSeconds(killFitnessTime);

        if (fitness - lastFitness <= killFitnessThresh)
        {
            Kill();
        }
        lastFitness = fitness;

        killFitnessRoutine = killTimerByFitness();
        StartCoroutine(killFitnessRoutine);

    }

    // Use this for initialization
    void Start () {
        hasFailed = false;

        killFitnessRoutine = killTimerByFitness();
        StartCoroutine(killTimerByFitness());

        //neuralnet = gameObject.GetComponent<NNet>();
        //neuralnet.CreateNet(1, 9, 8, 2);
        //neuralnet = genome.brain;
        raycast = gameObject.GetComponent<RayCast>();
        motor = gameObject.GetComponent<Motor>();
        hit = gameObject.GetComponent<hit>();

        defaultpos = transform.position;
        defaultrot = transform.rotation;

        cam.enabled = false;
    }
	
	// Update is called once per frame
	void Update ()
	{

	    if (genome == null) return;

        fitness = hit.checkpoints;
	    
        hasFailed = hit.crash;

        if (!hasFailed)
        {

             genome.fitness = fitness;// record fitness

            /*List<float> inputs = new List<float>();
            for (int i = 0; i < raycast.rays.Length; i++)
            {
                float dist = Normalise(raycast.rays[i]);
                inputs.Add(dist);
            }*/

            float[] inputs = new float[RayCast.raysNumber];
            for (int i = 0; i < raycast.rays.Length; i++)
            {
                float dist = MapInterval(raycast.rays[i], 0, RayCast.RayCast_Length,-1.0f, 1.0f);
                inputs[i] = dist;
            }

            float[] result = neuralnet.Run(inputs);
            //neuralnet.SetInput(inputs);
            //neuralnet.refresh();

            //float speed = neuralnet.GetOutput(0);
            //float angle = neuralnet.GetOutput(1);

            rawSpeed = result[0];
            rawAngle = result[1];
            currentSpeed = MapInterval(rawSpeed, 0.0f, 1.0f, -MAX_SPEED/10, MAX_SPEED);
            headingAngle = MapInterval(rawAngle, 0.0f, 1.0f, -MAX_ROTATION, MAX_ROTATION);

            //Debug.Log("Car raw output: speed " + speed + " angle " + angle);
            //Debug.Log("Car serialized output: speed " + currentSpeed + " angle " + headingAngle);
        }
        
    }

    void OnMouseDown()
    {
        selectThisCar();
    }

    public void selectThisCar()
    {
        master.selectedCar = this;
        cam.enabled = true;

        foreach (GameObject carObj in SimMaster.instance.testSubjects)
        {
            Car car = carObj.GetComponent<Car>();
            if (car != this)
                car.disSelectThisCar();
        }
    }

    public void disSelectThisCar()
    {
        cam.enabled = false;
    }

    public void Kill()
    {
        GetComponent<hit>().crash = true;
    }

    public void setGenome(GGenome gen)
    {
        genome = null;
        neuralnet = null;
        genome = gen;
        neuralnet = gen.brain;
    }

    public void ClearFailure()
    {
        genome = null;
        neuralnet = null;
        hasFailed = false;
        
        fitness = 0;
        lastFitness = 0;
        if (killFitnessRoutine != null)
        {
            StopCoroutine(killFitnessRoutine);
            killFitnessRoutine = killTimerByFitness();
            StartCoroutine(killFitnessRoutine);
        }

        transform.position = defaultpos;
        transform.rotation = defaultrot;

        if (hit != null)
        {
            hit.reset();
        }

    }

    public float Clamp(float val, float min, float max)
    {
        if (val < min)
        {
            return min;
        }
        if (val > max)
        {
            return max;
        }
        return val;
    }

    private float MapInterval(float val, float srcMin, float srcMax, float dstMin, float dstMax)
    {
        if (val >= srcMax) return dstMax;
        if (val <= srcMin) return dstMin;
        return dstMin + (val - srcMin) / (srcMax - srcMin) * (dstMax - dstMin);
    }
}
