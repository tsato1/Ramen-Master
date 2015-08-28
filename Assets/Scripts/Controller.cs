using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class Controller : MonoBehaviour 
{
    /*** variables dynamically assigned ***/
    private static GameObject hoverObject;
    
    private static Slot from, to;

    private float fieldWidth, fieldHeight;

    private RectTransform fieldRect;

    private int numEmptySlots;

    private Canvas canvas;

    /*** varialbes to be set on start ***/
    private List<GameObject> allSlots;
    
    private float slotSize, slotPaddingLeft, slotPaddingTop;

    private int numSlots, numRows;

    private GameObject slotPrefab;

    private float hoverYOffset;

    private GameObject donburi;

    private GameObject blackCircle;

    private RectTransform blackCircleRect;

    private List<Item> items;

    private GameObject scoreBoard;

    public GameObject explanation;

    public Text explanationText;

    private Object[] allToppings;

    private bool readyToStart;

    private GameObject noren;

    public GameObject slots;

    public GameObject dragHere;

    private bool pickFlag;

    public GameObject fieldBackground;

    //

    private AudioSource audioPick;
    private AudioSource audioDrop;
    private AudioSource audioGameover;
    private AudioSource audioBackground;
    private AudioSource audioMasterSatan;
    private AudioSource audioMasterHmmmm;
    private AudioSource audioMasterNotbad;
    private AudioSource audioMasterVomit;
    private AudioSource audioMasterHiThere;

    // 

    private bool gameoverFlag;

    private float timer;

    public GameObject timerObj;

    public GameObject GameoverObj;

	void Start () 
    {
        Initialize();
        CreateLayoutTopping();
        CreateLayoutDonburi();
        AddToppings();
	}

    void Initialize()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        slotPaddingLeft = 0.3f;
        slotPaddingTop = 0.3f;
        numSlots = 3;
        numEmptySlots = numSlots;
        readyToStart = false;
        numRows = 1;
        slotSize = 70.0f;
        hoverYOffset = slotSize * 0.01f;
        slotPrefab = Resources.Load<GameObject>("Prefabs/Slot");
        items = new List<Item>();
        allToppings = Resources.LoadAll("Prefabs/Toppings", typeof(GameObject));
        dragHere.GetComponent<CanvasRenderer>().SetAlpha(0);
        dragHere.transform.FindChild("ClickText").GetComponent<CanvasRenderer>().SetAlpha(0);
        pickFlag = false;
        AudioSource[] audioSources = GetComponents<AudioSource>();
        audioPick = audioSources[0];
        audioDrop = audioSources[1];
        audioBackground = audioSources[2];
        audioMasterSatan = audioSources[3];
        audioGameover = audioSources[4];
        audioMasterHmmmm = audioSources[5];
        audioMasterNotbad = audioSources[6];
        audioMasterVomit = audioSources[7];
        audioMasterHiThere = audioSources[8];
        timer = 0;
        gameoverFlag = true;

        scoreBoard = Instantiate(Resources.Load("Prefabs/ScoreBoard")) as GameObject;
        scoreBoard.transform.SetParent(canvas.transform);
        scoreBoard.transform.position = new Vector3(Screen.width / 2, Screen.height / 2);
        scoreBoard.AddComponent<CanvasGroup>();
        scoreBoard.GetComponent<CanvasGroup>().alpha = 0;
        RectTransform scoreBoardRect = scoreBoard.GetComponent<RectTransform>();
        scoreBoardRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width);
        scoreBoardRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height);
        GameoverObj.GetComponent<CanvasRenderer>().SetAlpha(0);
        GameoverObj.transform.FindChild("GameoverScreenText").GetComponent<CanvasRenderer>().SetAlpha(0);

        //explanation = Instantiate(Resources.Load("Prefabs/Explanation")) as GameObject;
        //explanation.transform.SetParent(canvas.transform);
        //explanation.transform.position = new Vector3(Screen.width / 2, Screen.height / 2);
        //explanation.transform.localScale = new Vector3(1, 1, 1);
        //RectTransform explanationRect = explanation.GetComponent<RectTransform>();
        //explanationRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 200.0f);
        //explanationRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100.0f);

        //explanationText = GameObject.Find("ExplanationText").GetComponent<Text>();
        //RectTransform explanationTextRect = explanationText.GetComponent<RectTransform>();
        //explanationTextRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, explanationRect.sizeDelta.x);
        //explanationTextRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, explanationRect.sizeDelta.y);

        //noren = Instantiate(Resources.Load("Prefabs/Noren")) as GameObject;
        //noren.transform.SetParent(canvas.transform);
        //noren.transform.position = new Vector3(Screen.width / 2 - 10.0f, Screen.height - 70.0f);
        //RectTransform norenRect = noren.GetComponent<RectTransform>();
        //norenRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 100.0f + norenRect.sizeDelta.x);
        //norenRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, norenRect.sizeDelta.y);
    }

    void CreateLayoutTopping()
    {
        allSlots = new List<GameObject>();

        fieldWidth = (numSlots / numRows) * (slotSize + slotPaddingLeft) + slotPaddingLeft;
        fieldHeight = numRows * (slotSize + slotPaddingTop) + slotPaddingTop;

        fieldRect = GetComponent<RectTransform>();
        fieldRect.localScale = new Vector3(1, 1, 1);
        fieldRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, fieldWidth);
        fieldRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, fieldHeight);
        //fieldRect.transform.position = new Vector2(Screen.width / 2, Screen.height / 2);

        int numColumns = numSlots / numRows;
        
        RectTransform fieldBackgroundRect = fieldBackground.GetComponent<RectTransform>();
        fieldBackgroundRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 498.5f);
        fieldBackgroundRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 286.0f);

        for (int y = 0; y < numRows; y++)
        {
            for (int x = 0; x < numColumns; x++)
            {
                GameObject newSlot = Instantiate(slotPrefab) as GameObject;
                newSlot.name = "Slot";
                newSlot.transform.SetParent(canvas.transform);
                newSlot.transform.localScale = new Vector3(1, 1, 1);
                newSlot.transform.SetParent(slots.transform);

                RectTransform slotRect = newSlot.GetComponent<RectTransform>();
                slotRect.localPosition = fieldRect.localPosition + new Vector3(-fieldWidth / 2, 200.0f) + new Vector3(slotPaddingLeft * (x + 1) + (slotSize * x), -slotPaddingTop * (y + 1) - (slotSize * y));
                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize);
                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize);

                allSlots.Add(newSlot);
            }
        }
    }

    void CreateLayoutDonburi()
    {
        donburi = Instantiate(Resources.Load("Prefabs/Donburi")) as GameObject;
        donburi.transform.SetParent(GameObject.Find("Canvas").transform);
        donburi.transform.localScale = new Vector3(3, 3, 1);
        RectTransform donburiRect = donburi.GetComponent<RectTransform>();
        donburiRect.localPosition = fieldRect.localPosition + new Vector3(0.0f, -150.0f);
        donburiRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, donburiRect.sizeDelta.x);
        donburiRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, donburiRect.sizeDelta.y);

        blackCircle = Instantiate(Resources.Load("Prefabs/BlackCircle")) as GameObject;
        blackCircle.transform.SetParent(GameObject.Find("Canvas").transform);
        blackCircleRect = blackCircle.GetComponent<RectTransform>();
        blackCircleRect.localScale = new Vector3(1, 1, 1);
        blackCircleRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, blackCircleRect.sizeDelta.x);
        blackCircleRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, blackCircleRect.sizeDelta.y);
        blackCircleRect.transform.position = donburiRect.transform.position;
    }

    void AddToppings()
    {
        int counter = 0;

        ShuffleToppings();

        while (!readyToStart)
        {
            for (int i = 0; i < numSlots; i++)
            {
                if (((GameObject)allToppings[i]).GetComponent<Item>().score == 33) // check if toppings include at least three toppings that has a score of 33
                {
                    counter++;

                    //Debug.Log("Item for clear = " + ((GameObject)allToppings[i]).GetComponent<Item>().itemName);
                    if (counter >= 3)
                    {
                        readyToStart = true;
                    }
                }
            }

            if (!readyToStart)
            {
                ShuffleToppings();
            }
        }

        /*** Set ***/
        for (int i = 0; i < numSlots; i++)
        {
            Item item = ((GameObject)allToppings[i]).GetComponent<Item>();
            AddItem(item);
        }
    }

    void ShuffleToppings()
    {
        /*** Shuffle all the toppings ***/
        int n = allToppings.Length;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n);

            Object tmp = allToppings[k];
            allToppings[k] = allToppings[n];
            allToppings[n] = tmp;
        }
    }

    bool AddItem(Item item)
    {
        if (numEmptySlots > 0)
        {
            foreach (GameObject slot in allSlots)
            {
                Slot tmp = slot.GetComponent<Slot>();

                if (tmp.IsEmpty)
                {
                    tmp.AddItem(item);
                    numEmptySlots--;
                    return true;
                }
            }
        }

        return false;
    }

    void Update() 
    {
        MouseOverDetection();
        HandleMouseInput();
        Timer();
	}

    void Timer()
    {
        timer += Time.deltaTime;

        if (timer < 1.0f)
        {
            timerObj.GetComponent<Text>().text = "5";
        }
        else if (timer < 2.0f)
        {
            timerObj.GetComponent<Text>().text = "4";
        }
        else if (timer < 3.0f)
        {
            timerObj.GetComponent<Text>().text = "3";
        }
        else if (timer < 4.0f)
        {
            timerObj.GetComponent<Text>().text = "2";
        }
        else if (timer < 5.0f)
        {
            timerObj.GetComponent<Text>().text = "1";
        }
        else if (timer < 6.0f && gameoverFlag)
        {
            audioBackground.Stop();
            audioGameover.PlayOneShot(audioGameover.clip);

            GameoverObj.GetComponent<CanvasRenderer>().SetAlpha(1);
            GameoverObj.transform.FindChild("GameoverScreenText").GetComponent<CanvasRenderer>().SetAlpha(1);

            /*** making all objects invisible ***/
            donburi.GetComponent<CanvasRenderer>().SetAlpha(0);
            blackCircle.SetActive(false);
            if (hoverObject != null) hoverObject.GetComponent<CanvasRenderer>().SetAlpha(0);
            slots.transform.position = new Vector3(1000.0f, 1000.0f, 1000.0f);
        }
    }


    void MouseOverDetection()
    {
        for (int i = 0; i < allSlots.Count; i++)
        {
            float x = allSlots[i].transform.position.x;
            float y = allSlots[i].transform.position.y;

            if (x - slotSize / 2 < Input.mousePosition.x && Input.mousePosition.x < x + slotSize / 2 &&
                y - slotSize / 2 < Input.mousePosition.y && Input.mousePosition.y < y + slotSize / 2)
            {
                if (!allSlots[i].GetComponent<Slot>().IsEmpty)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        explanationText.text = allSlots[i].GetComponent<Slot>().CurrentItem.description;

                        MoveItem(allSlots[i]);

                        dragHere.GetComponent<CanvasRenderer>().SetAlpha(1);
                        dragHere.transform.FindChild("ClickText").GetComponent<CanvasRenderer>().SetAlpha(1);

                        pickFlag = !pickFlag;
                        if (pickFlag)
                        {
                            audioPick.PlayOneShot(audioPick.clip);
                        }
                        else
                        {
                            audioDrop.PlayOneShot(audioDrop.clip);
                        }
                    }
                }
            }
        }

        if (Vector2.Distance(new Vector2(Input.mousePosition.x, Input.mousePosition.y), blackCircle.transform.position) < blackCircleRect.sizeDelta.x / 2)
        {
            blackCircle.GetComponent<Image>().color = Color.red;
        }
        else
        {
            blackCircle.GetComponent<Image>().color = Color.white;
        }
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (Vector2.Distance(new Vector2(Input.mousePosition.x, Input.mousePosition.y), blackCircle.transform.position) < blackCircleRect.sizeDelta.x / 2)
            {
                if (from != null)
                {
                    items.Add(from.GetComponent<Slot>().CurrentItem);
                    from.GetComponent<Image>().color = Color.white;
                    from.ClearSlot();
                    Destroy(GameObject.Find("Hover"));
                    to = null;
                    from = null;
                    hoverObject = null;

                    pickFlag = !pickFlag;
                    if (!pickFlag)
                    {
                        audioDrop.PlayOneShot(audioDrop.clip);
                    }
                }

                if (items.Count == 1)
                {
                    FinishGame();
                    ShowScore();
                }
            }
        }

        if (hoverObject != null) // gets in if cursor has hover object around it
        {
            /*** make the item hover along to the cursor ***/
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out position);
            position.Set(position.x, position.y - hoverYOffset);
            hoverObject.transform.position = canvas.transform.TransformPoint(position);
        }
    }

    void FinishGame()
    {
        audioBackground.Stop();
        scoreBoard.GetComponent<CanvasGroup>().alpha = 1;
        explanation.GetComponent<CanvasRenderer>().SetAlpha(0);
        explanationText.GetComponent<CanvasRenderer>().SetAlpha(0);
        donburi.GetComponent<CanvasRenderer>().SetAlpha(0);
        blackCircle.GetComponent<CanvasGroup>().alpha = 0;
        slots.transform.position = new Vector3(1000.0f, 1000.0f, 1000.0f);
        gameoverFlag = false;
    }

    void ShowScore()
    {
        int totalScore = items[0].score;// +items[1].score + items[2].score;
        
        if (totalScore >= 30)
        {
            Debug.Log("good");
            scoreBoard.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Master/Master_Approving");
            audioMasterNotbad.PlayOneShot(audioMasterNotbad.clip);
        }
        else if (totalScore == 1)
        {
            Debug.Log("A girl chosen");
            scoreBoard.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Master/Master_HiThere");
            audioMasterHiThere.PlayOneShot(audioMasterHiThere.clip);
        }
        else if (totalScore == 2)
        {
            Debug.Log("toilet paper chosen");
            scoreBoard.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Master/Master_Vomit");
            audioMasterVomit.PlayOneShot(audioMasterVomit.clip);
        }
        else if (totalScore <= 0)
        {
            Debug.Log("less than 60");
            scoreBoard.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Master/Master_Satan");
            audioMasterSatan.PlayOneShot(audioMasterSatan.clip);
        }
        
    }

    public void MoveItem(GameObject clicked)
    {
        if (from == null)
        {
            if (!clicked.GetComponent<Slot>().IsEmpty)
            {
                from = clicked.GetComponent<Slot>();
                from.GetComponent<Image>().color = Color.grey;

                hoverObject = Instantiate(Resources.Load("Prefabs/HoverObject")) as GameObject;
                hoverObject.GetComponent<Image>().sprite = clicked.GetComponent<Image>().sprite;
                hoverObject.name = "Hover";

                RectTransform hoverTransform = hoverObject.GetComponent<RectTransform>();
                RectTransform clickedTransform = clicked.GetComponent<RectTransform>();

                hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, clickedTransform.sizeDelta.x);
                hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, clickedTransform.sizeDelta.y);

                hoverObject.transform.SetParent(GameObject.Find("Canvas").transform, true);
            }
        }
        else if (to == null)
        {
            to = clicked.GetComponent<Slot>();
            Destroy(GameObject.Find("Hover"));
        }

        if (to != null && from != null)
        {
            Stack<Item> tmpTo = new Stack<Item>(to.Items);

            to.AddItems(from.Items);

            if (tmpTo.Count == 0)
            {
                from.ClearSlot();
            }
            else
            {
                from.AddItems(tmpTo);
            }

            from.GetComponent<Image>().color = Color.white;

            to = null;
            from = null;
            hoverObject = null;
        }
    }
}
