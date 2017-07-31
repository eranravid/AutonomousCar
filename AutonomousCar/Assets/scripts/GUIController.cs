using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GUIController : MonoBehaviour {

    public List<Text> texts = new List<Text>();
    private List<String> varTexts = new List<String>();
    public Text carID;
    public Text carFitness;
    public Image carWheel;
    public Image speedDial;
    public Text steerText;
    public Text speedText;
    public Button killButton;


    // Use this for initialization
    void Start () {
        killButton.onClick.AddListener(onButtonKillClicked);
    }
	
	// Update is called once per frame
	void Update ()
	{
        // car
	    Car car = SimMaster.instance.selectedCar;
        carID.text = car.id.ToString();
	    carFitness.text = car.fitness.ToString();

        var rotationVector = carWheel.transform.rotation.eulerAngles;
	    rotationVector.z = MapInterval(car.rawAngle, 0.0f, 1.0f, -90, 90); 
        carWheel.transform.rotation = Quaternion.Euler(-rotationVector);
	    steerText.text = car.rawAngle.ToString();

        rotationVector.z = MapInterval(car.rawSpeed,0.0f, 1.0f, 170, -60);
	    speedDial.transform.rotation = Quaternion.Euler(rotationVector);
	    speedText.text = car.rawSpeed.ToString();        


        rotationVector.z = 0;
	    transform.rotation = Quaternion.Euler(rotationVector);

        // texts / configs
        varTexts.Add(Time.time.ToString("00.00"));
	    varTexts.Add(SimMaster.instance.gga.generation.ToString());
	    varTexts.Add(GGA.avarageFitness.ToString("00.00"));
        varTexts.Add(GGA.bestFitness.ToString());
	    varTexts.Add(SimMaster.instance.gga.fitnessGoal.ToString());
        varTexts.Add(SimMaster.instance.gga.populationNumber.ToString());
	    varTexts.Add(SimMaster.instance.gga.liveGenomes.Count.ToString());
	    varTexts.Add(SimMaster.instance.gga.deadGenomes.Count.ToString());	    
        varTexts.Add(SimMaster.instance.gga.randomMargin.ToString());
	    varTexts.Add(SimMaster.instance.gga.mutateRate.ToString());

	    for (int i = 0; i < texts.Count; i++)
	    {
	        texts[i].text = varTexts[i];
	    }

	    varTexts.Clear();

    }

    void onButtonKillClicked()
    {
        SimMaster.instance.selectedCar.Kill();
    }

    private float MapInterval(float val, float srcMin, float srcMax, float dstMin, float dstMax)
    {
        if (val >= srcMax) return dstMax;
        if (val <= srcMin) return dstMin;
        return dstMin + (val - srcMin) / (srcMax - srcMin) * (dstMax - dstMin);
    }

}
