# Autonomous car simulation in unity 3d - Machine Learning using Neural Network & Genetic Algorithm

[![N|Solid](https://i.ytimg.com/vi/BJOwxY_Ig1s/maxresdefault.jpg)](https://www.youtube.com/watch?v=BJOwxY_Ig1s)

This autonomous car simulation is done using unity3d game engine written in C#.

  - Machine-Learning, Genetic Algorithm
  - Using neural network as a data-function-set and apllying random selection to maximise fitness of agents
  - Magic

The demo features a self-taught, machine learned, simulated car using Neural Networks & Genetic Algorithm. 
The cars does not pre-programmed to know how to drive or what are the steps to learn how to drive. In fact, it does not know anything about it's world except the distance from collision objects, which are the track borders and other cars in the simulation. 
The car has 36 ray cast distances, represented as the red lines for the selected car, to use as inputs for the Neural Network. The blue line are the maximum distance for a raycast. The Neural Network has 1 hidden layer with 36 nodes. There are two output nodes, one for the direction of the wheels and one for the speed of the car. Those output then reconnected as 2 more input nodes for the Neural Network. 

Each car initiated with a random set of weights for it's Neural Network. 
If the activation of the network produce output that make the car collide with the track or with some other car, the car crashes and begin again from it's spawning point, using a new Neural Net from the Genetic-Algorithm agents pool. 
If the car does not crash, it likely to collect food points, represented as purple dots and scattered across the road area. The more food it collect the more likely it to pass on it's genes. Each time the population stack is empty a cross-breed process start to create new population from the strongest fitness agents.

link to see it in action: https://www.youtube.com/watch?v=BJOwxY_Ig1s
This video is long, feel free to run it at X2 speed using youtube's settings.


### Todos

 - Show the neural network graph with all of it's wieghts represented graphically as GUI
 - Implement more machine-learning methods such as deep Q learning.

License
----

MIT


[//]: # (These are reference links used in the body of this note and get stripped out when the markdown processor does its job. There is no need to format nicely because it shouldn't be seen. Thanks SO - http://stackoverflow.com/questions/4823468/store-comments-in-markdown-syntax)


   [dill]: <https://github.com/joemccann/dillinger>
   [git-repo-url]: <https://github.com/joemccann/dillinger.git>
   [john gruber]: <http://daringfireball.net>
   [df1]: <http://daringfireball.net/projects/markdown/>
   [markdown-it]: <https://github.com/markdown-it/markdown-it>
   [Ace Editor]: <http://ace.ajax.org>
   [node.js]: <http://nodejs.org>
   [Twitter Bootstrap]: <http://twitter.github.com/bootstrap/>
   [jQuery]: <http://jquery.com>
   [@tjholowaychuk]: <http://twitter.com/tjholowaychuk>
   [express]: <http://expressjs.com>
   [AngularJS]: <http://angularjs.org>
   [Gulp]: <http://gulpjs.com>

   [PlDb]: <https://github.com/joemccann/dillinger/tree/master/plugins/dropbox/README.md>
   [PlGh]: <https://github.com/joemccann/dillinger/tree/master/plugins/github/README.md>
   [PlGd]: <https://github.com/joemccann/dillinger/tree/master/plugins/googledrive/README.md>
   [PlOd]: <https://github.com/joemccann/dillinger/tree/master/plugins/onedrive/README.md>
   [PlMe]: <https://github.com/joemccann/dillinger/tree/master/plugins/medium/README.md>
   [PlGa]: <https://github.com/RahulHP/dillinger/blob/master/plugins/googleanalytics/README.md>
