using UnityEngine;
using System.Collections;

public class DragScript : MonoBehaviour {
    private bool isDrag = false;

    enum STEP
    {
        NONE = -1,

        IDLE = 0,
        DRAGGING,
    }

    private STEP step = STEP.IDLE;
    private STEP stepNext = STEP.NONE;

    private Vector3 screenPoint;
    private Vector3 offset;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        // state change

        switch (step)
        {
            case STEP.IDLE:
                if (this.isDrag)
                {
                    this.stepNext = STEP.DRAGGING;
                }
                break;

            case STEP.DRAGGING:
                if (!this.isDrag)
                {
                    this.stepNext = STEP.IDLE;
                }
                break;
        }

        // state transition

        while (this.stepNext != STEP.NONE)
        {
            this.step = this.stepNext;
            this.stepNext = STEP.NONE;

            switch (this.step)
            {
                case STEP.DRAGGING:
                    print("start drag" + this.name);
                    this.beginDragging();
                    break;

                case STEP.IDLE:
                    print("end drag" + this.name);
                    break;
            }
        }

        // execute each state job

        switch (this.step)
        {
            case STEP.DRAGGING:
                this.doDragging();
                break;
        }

    }

    void beginDragging()
    {
        screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        float x = Input.mousePosition.x;
        float y = Input.mousePosition.y;
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(x, y, screenPoint.z));
    }

    void doDragging()
    {
        float x = Input.mousePosition.x;
        float y = Input.mousePosition.y;
        Vector3 currentScreenPoint = new Vector3(x, y, screenPoint.z);
        Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenPoint) + offset;

        transform.position = currentPosition;
    }

    void OnMouseDown()
    {
        this.isDrag = true;
    }

    void OnMouseUp()
    {
        this.isDrag = false;
    }
}