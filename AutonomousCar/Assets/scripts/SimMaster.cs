﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FANNCSharp.Float;
using UnityEngine;
using Random = UnityEngine.Random;

public class SimMaster : MonoBehaviour {

    public GameObject carPrefab;
    //public GA ga;
    public GGA gga;
    public List<GameObject> testSubjects = new List<GameObject>();
    
    public GameObject[] SPs;

    public void OnGUI()
    {
        int x = 100;
        int y = 100;
        GUI.Label(new Rect(x, y, 500, 20), "Time: " + Time.time); 
        GUI.Label(new Rect(x, y + 20, 500, 20), "Population: " + gga.populationNumber + " Best Fitness: " + GGA.bestFitness + " Fitness Goal: " + gga.fitnessGoal);
        GUI.Label(new Rect(x, y + 40, 500, 20), "Generation: " + gga.generation + " Genomes: " + gga.genomes.Count + " Live Genomes: " + gga.liveGenomes.Count + " Dead Genomes: " + gga.deadGenomes.Count);
        GUI.Label(new Rect(x, y + 60, 500, 20), "Random Margin: " + gga.randomMargin + " Mutate Rate: " + gga.mutateRate);
        //GUI.Label(new Rect(x + 200, y + 20, 200, 20), "BestFitness: " + bestFitness);
        /* GUI.Label(new Rect(x + 200, y, 200, 20), "CurrentFitness: " + bestFitness);
         GUI.Label(new Rect(x + 300, y, 200, 20), "out1: " + (testAgent.leftTheta / 2 - 6));
         GUI.Label(new Rect(x + 300, y + 20, 200, 20), "out2: " + testAgent.rightTheta);*/

    }


    // Use this for initialization
    void Start () {

        gga = new GGA();

        // initialize Genetic Algorithm
        //ga = new GA();
        //int totalWeights = 9 * 8 + 8 * 2 + 8 + 2;
        //ga.GenerateNewPopulation(15, totalWeights);
        GGA.bestFitness = 0;

        // initialize test subjects
        SPs = GameObject.FindGameObjectsWithTag("SpawnPoint");
        Transform t;
        foreach (GameObject c in SPs)
        {
            t = c.gameObject.GetComponent<Transform>();
            GameObject car = (GameObject)Instantiate(carPrefab,t.transform); ;
            car.GetComponent<Car>().master = this;
            testSubjects.Add(car);
            NextTestSubject(car.GetComponent<Car>());
        }
        
    }
	
	// Update is called once per frame
	void Update ()
	{
	    
        for (int i=0; i< testSubjects.Count; i++)
        {
            // if test subject falied start a new one
            Car testAgent = testSubjects[i].GetComponent<Car>();
            if (testAgent.hasFailed)
            {
                //int gindx = ga.GetCurrentGenomeIndex();
                // Genome genome = ga.GetGenome(gindx);
                //genome.live = false;
                //testAgent.genomeIndx = gindx;
                gga.killGenome(testAgent.genome);
                testAgent.ClearFailure(); // reset subject
                NextTestSubject(testAgent); // start new subject
            }

            // best fitness
            if (testAgent.fitness > GGA.bestFitness)
            {
                GGA.bestFitness = testAgent.fitness;
            }
        }
       
    }

    public void NextTestSubject(Car testAgent)
    {

        //Genome genome = ga.GetNextGenome();
        //genome.live = true;
        //NNet net = testAgent.GetComponent<NNet>();
        //net.FromGenome(genome, 9, 8, 2);  

        GGenome gen = gga.getNextGenome();
        if (gen != null)
        {
            testAgent.setGenome(gen);
        }

        //testAgent.setGenome(new GGenome(9, new uint[] { 8, 8 }, 4));
        
    }

    
}


public class GGA
{
    public int generation = 0;
    public int populationNumber = 20;
    public float randomMargin = 0.5f; // how much random range a mutate has
    public float mutateRate = 0.55f; // the chane for each gene to take random mutate
    public int fitnessGoal = 2000; // amount of fitness to stop generating genomes. also randomAmplfier of crossbreed for mutateRate & randomMargin is redundent by it.
    public static int bestFitness = 0;
    public List<GGenome> genomes = new List<GGenome>();
    public List<GGenome> liveGenomes = new List<GGenome>();
    public List<GGenome> deadGenomes = new List<GGenome>();

    public GGA()
    {
        for (int i = 0; i < populationNumber; i++)
        {
            uint rn = Convert.ToUInt32(RayCast.raysNumber);
            genomes.Add(new GGenome(rn, new uint[]{ rn },2));
        }
    }

    public GGenome getNextGenome()
    {
        if (bestFitness >= fitnessGoal)
        {
            return genomes[0];
        }
        int passGenomes = liveGenomes.Count + deadGenomes.Count;
        if (passGenomes >= populationNumber)
        {
            generation++;
            BreedPopulation();
            passGenomes = liveGenomes.Count + deadGenomes.Count;
        }
        GGenome g = genomes[passGenomes];
        liveGenomes.Add(g);
        return g;
    }

    public void killGenome(GGenome gen)
    {
        liveGenomes.Remove(gen);
        deadGenomes.Add(gen);
    }

    public void BreedPopulation()
    {
        // find best genomes
        GGenome[] parents = findBestGenes(4);

        // remove dead genomes from genomes list
        for (int i = 0; i < deadGenomes.Count; i++)
        {
            genomes.Remove(deadGenomes[i]);
        }
        deadGenomes = new List<GGenome>();

        // fill up randomed breeded genomes
        GGenome[] newGenomes = crossBreed(parents, populationNumber - genomes.Count, true);
        for (var j = 0; j < newGenomes.Length; j++)
        {
            genomes.Add(newGenomes[j]);
        }

    }

    public GGenome[] findBestGenes(int num)
    {
        GGenome[] parents = new GGenome[num];

        genomes = genomes.OrderByDescending(o => o.fitness).ToList();

        for (var j = 0; j < parents.Length; j++)
        {
            if (genomes[j] != null)
            {
                parents[j] = genomes[j];
            }
        }
        return parents;
    }

    public GGenome[] crossBreed(GGenome[] parents, int numToGen, bool keepFather)
    {
        GGenome[] newGenomes = new GGenome[numToGen];

        Connection[] parCons1;
        Connection[] parCons2;

        for (int i = 0; i < numToGen; i++)
        {
            float randomAmplfier = ((i / numToGen) - (bestFitness / fitnessGoal*2) + 1);

            newGenomes[i] = new GGenome(parents[0]._inputLayers, parents[0]._hiddenLayers, parents[0]._outputLayers);
            //int parentIndex = Random.Range(0, parents.Length); // more than 2 parents

            float crossParentRandom = Random.Range(0.0f, 1.0f);
            int parIndex1 = 0;//(i < crossParentRandom * 0.5f) ? 0 : ((i < crossParentRandom * 0.75f)) ? 1 : 2;
            int parIndex2 = 1;//(i < crossParentRandom * 0.3f) ? 1 : (i < crossParentRandom * 0.75f) ? 2 : 3;
            parCons1 = parents[parIndex1].getConnections();
            parCons2 = parents[parIndex2].getConnections();
            
            int randomCrossRange = Random.Range(0, parCons1.Length / 2);
            int randomCrossRange2 = Random.Range(randomCrossRange, parCons1.Length);
            int strongParent = Random.Range(0, 2);

            bool switched = (strongParent <= 1) ? true : false;

            Connection con;

            for (int j = 0; j < parCons1.Length; j++)
            {
                
                float newWeight;
                switched = (Random.Range(0, 10) <= 1) ? !switched : switched;
                if (switched)//strongParent <= 1 )&& j <= randomCrossRange || j > randomCrossRange2)
                {
                    con = parCons1[j];
                }
                else
                {
                    con = parCons2[j];
                }

                newWeight = con.Weight;
                // randomize wieght and set to new genome
                if (Random.Range(0.0f, 1.0f) <= mutateRate * randomAmplfier)
                {
                    newWeight = Clamp(con.Weight + Random.Range(-randomMargin * randomAmplfier, randomMargin * randomAmplfier), -1.0f, 1.0f);
                }
                newGenomes[i].brain.SetWeight(con.FromNeuron, con.ToNeuron, newWeight);
            }
        }
        if (keepFather)
        {
            newGenomes[newGenomes.Length - 1] = parents[0];
        }
        return newGenomes;
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
}

public class GGenome
{
    public FANNCSharp.Float.NeuralNet brain;
    public int fitness = 0;
    public uint _inputLayers;
    public uint[] _hiddenLayers;
    public uint _outputLayers;

    public GGenome(uint inputLayers, uint[] hiddenLayers, uint outputLayers)
    {
        _inputLayers = inputLayers;
        _hiddenLayers = hiddenLayers;
        _outputLayers = outputLayers;

        // create a new neural network
        ICollection < uint > layers = new List<uint>();
        layers.Add(inputLayers);
        for (uint i = 0; i < hiddenLayers.Length; i++)
        {
            layers.Add(hiddenLayers[i]);
        }
        layers.Add(outputLayers);
        brain = new FANNCSharp.Float.NeuralNet(FANNCSharp.NetworkType.LAYER, layers);
        brain.DisableSeedRand();
        brain.RandomizeWeights(-1.0f,1.0f);

    }

    public Connection[] getConnections()
    {
        return brain.ConnectionArray;
    }

    public void setConnections(Connection[] cons)
    {
        foreach (var connection in cons)
        {
            brain.SetWeight(connection.FromNeuron, connection.ToNeuron, connection.Weight);
        }
    }
}