using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class NMAScript : MonoBehaviour
{
    private NavMeshAgent nma = null;
    private Bounds bndFloor;
    private Vector3 moveto;
    private GameObject pole = null;
    private LineRenderer line = null;
    private TextMeshProUGUI tmpro = null;
    private bool flag = false;

    private void Start()
    {
        nma = this.GetComponent<NavMeshAgent>();
        bndFloor = GameObject.Find("floor").GetComponent<Renderer>().bounds;
        pole = GameObject.Find("pole");
        tmpro = GameObject.Find("UIText").GetComponent<TextMeshProUGUI>();

        //instantiate a line object
        line = this.gameObject.AddComponent<LineRenderer>();
        line.material = new Material(Shader.Find("Sprites/Default"));
        line.widthMultiplier = 0.2f;

        SetRandomDestination();
    }
    private void Update()
    {
        tmpro.text = string.Format("path={0}, point on navmesh={1}", nma.hasPath, (nma.pathEndPosition == moveto));

        if (nma.hasPath == false && flag == false)
        {
            flag = true;
            SetRandomDestination();
        }
    }
    private void SetRandomDestination()
    {
        //1. pick a point
        float rx = Random.Range(bndFloor.min.x, bndFloor.max.x);
        float rz = Random.Range(bndFloor.min.z, bndFloor.max.z);
        moveto = new Vector3(rx, this.transform.position.y, rz);
        nma.SetDestination(moveto); //figure out path, starts gameobject moving

        //2. show the destination
        pole.transform.position = new Vector3(moveto.x, pole.transform.position.y, moveto.z);

        Invoke("CheckPointOnPath", 0.2f);

        flag = false;
    }
    private void CheckPointOnPath()
    {
        //3. draw line (#1 tutorial)
        if (nma.path.corners.Length >= 2)
        {
            line.positionCount = nma.path.corners.Length;
            for (int i = 0; i < nma.path.corners.Length; i++)
            {
                line.SetPosition(i, nma.path.corners[i]);
            }
        }

        //4. check
        if (nma.pathEndPosition != moveto)
        {
            //point is not on navmesh!!! tadaaa!
            SetRandomDestination();
        }
    }


}
