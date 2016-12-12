using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HandleController : MonoBehaviour {

    public GameObject handle;

    public GameObject tl;

    public GameObject tr;

    public GameObject br;

    public GameObject bl;

    private int currentQuadrant = 1;    // The handle starts at the first quadrant

    private Ray ray;

    private RaycastHit raycastHit;

    private Dictionary<int, bool> sequence;

    private int sequencesCompleted = 0;

    public int SequencesTarget = 1;

    void Start()
    {
        var width = Screen.width / 2;
        var height = Screen.height / 2;

        sequence = new Dictionary<int, bool>()
        {
            { 1, false },
            { 2, false },
            { 3, false },
            { 4, false }
        };

        // TODO Refactor to each quadrant
        tl.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        tl.transform.localPosition = new Vector3((width / 2) * -1, height / 2, 1);
        tl.GetComponent<BoxCollider>().size = new Vector3(width, height, 1);

        tr.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        tr.transform.localPosition = new Vector3((width / 2), height / 2, 1);
        tr.GetComponent<BoxCollider>().size = new Vector3(width, height, 1);

        br.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        br.transform.localPosition = new Vector3((width / 2), (height / 2) * -1, 1);
        br.GetComponent<BoxCollider>().size = new Vector3(width, height, 1);

        bl.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        bl.transform.localPosition = new Vector3((width / 2) * -1, (height / 2) * -1, 1);
        bl.GetComponent<BoxCollider>().size = new Vector3(width, height, 1);
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out raycastHit))
            {
                var quadrant = raycastHit.transform.GetComponent<HandleQuadrant>();

                if (quadrant.order == currentQuadrant)
                {
                    return;
                }

                if (quadrant.order != GetNextInSequence())
                {
                    Debug.Log("Wrong sequence");
                    sequence[currentQuadrant] = false;
                    currentQuadrant = quadrant.order;
                }
                else
                {
                    Debug.Log(quadrant.order);
                    currentQuadrant = quadrant.order;
                    sequence[currentQuadrant] = true;
                }

                if (HasCompletedCircle())
                {
                    sequence[1] = false;
                    sequence[2] = false;
                    sequence[3] = false;
                    sequence[4] = false;

                    sequencesCompleted++;

                    Debug.Log("Sequence " + sequencesCompleted + " completed");
                }

                if (sequencesCompleted >= SequencesTarget)
                {
                    //Debug.Log("Handle mini game completed");
                }
            }
        }
        else
        {
            // Reset the handle
        }
    }

    private bool HasCompletedCircle()
    {
        return sequence.All(item => item.Value);
    }

    private int GetNextInSequence()
    {
        var next = currentQuadrant + 1;

        if (next > sequence.Count)
        {
            next = 1;
        }

        return next;
    }
}
