using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FollowPath : MonoBehaviour
{
    Transform goal;
    public float speed = 5.0f;
    public float accuracy = 1.0f;
    public float rotSpeed = 2.0f;
    public GameObject wpManager;
    GameObject[] wps;
    GameObject currentNode;
    int currentWP = 0;
    Graph g;
    public GameObject wpHeli, wpRuin, wpFactory; 
    void Start()
    {
        wps = wpManager.GetComponent<WpManager>().waypoints;
        g = wpManager.GetComponent<WpManager>().graph;
        currentNode = wps[0];
    }
    public void GoToHeli()
    {
        g.AStar(currentNode, wpHeli);
        currentWP = 0;
    }
    public void GoToRuin()
    {
        g.AStar(currentNode, wpRuin);
        currentWP = 0;
    }
    public void GoToFactory()
    {
        g.AStar(currentNode, wpFactory);
        currentWP = 0;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (g.getPathLength() == 0 || currentWP == g.getPathLength())
            return;//Se a lista de waypoint estiver vazia ou for o utimo, ele finaliza o m�todo. 

        //O n� que estar� mais pr�ximo neste momento
        currentNode = g.getPathPoint(currentWP);

        //se estivermos mais pr�ximo do n� o tanque se mover� para o pr�ximo
        if (Vector3.Distance(
        g.getPathPoint(currentWP).transform.position,
        transform.position) < accuracy)
        {
            currentWP++;
        }
        //Se n�o estiver no �ltimo waypoint, ele continua a movimenta��o
        if (currentWP < g.getPathLength())
        {
            goal = g.getPathPoint(currentWP).transform;
            //� a posi��o do waypoint onde est� indo, ignorando a altura(y). 
            Vector3 lookAtGoal = new Vector3(goal.position.x,
            this.transform.position.y,
            goal.position.z); 
            //Vetor apontando deste objeto ao lookAtGoal.
            Vector3 direction = lookAtGoal - this.transform.position;
            //Ele est� rotacionando o objeto para apontar pra lookAtGoal.
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
            Quaternion.LookRotation(direction),
            Time.deltaTime * rotSpeed);
            this.transform.Translate(Vector3.forward * speed);//Move o objeto no seu eixo frontal.
        }
    }
}
