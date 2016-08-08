﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public struct Date
{
    private int year;
    public int Year
    {
        get;
        set;
    }
    private int month;
    public int Month
    {
        get
        {
            return month;
        }

        set
        {
            month = value;
            if(month >= 13)
            {
                year += 1;
                month = 1;
            }
        }
    }
    private int day;
    public int Day
    {
        get
        {
            return day;
        }

        set
        {
            day = value;

            int lastDay = 0;
            switch(month)
            {
                case 1:
                    lastDay = 31;
                    break;
                case 2:
                    lastDay = 29;
                    break;
                case 3:
                    lastDay = 31;
                    break;
                case 4:
                    lastDay = 30;
                    break;
                case 5:
                    lastDay = 31;
                    break;
                case 6:
                    lastDay = 30;
                    break;
                case 7:
                    lastDay = 31;
                    break;
                case 8:
                    lastDay = 31;
                    break;
                case 9:
                    lastDay = 30;
                    break;
                case 10:
                    lastDay = 31;
                    break;
                case 11:
                    lastDay = 30;
                    break;
                case 12:
                    lastDay = 31;
                    break;
                default:
                    Debug.LogError("Invalid Month", GameManager.Instance);
                    break;
            }

            if(day >= lastDay)
            {
                day = 1;
                Month += 1;
            }
        }
    }
}

public enum SkinType
{
    Desk = 1,
    Bed,
    Wall,
    Floor,
    Costume
}

public enum ScheduleButtonType
{
    Schedule,
    Test,
    Interview,
    End
}

public class GameManager : Manager<GameManager>
{

    private bool pause = false;
    public bool IsPause
    {
        get
        {
            return pause;
        }
    }                    // 누적 시간

    private int stress;
    public int Stress
    {
        get
        {
            return stress;
        }

        set
        {
            if(value <= 100 && value >= 0)
            {
                stress = value;
            }
            else
            {
                Debug.LogWarning("Stress.value invalid value");
            }
        }
    }

    private Date gameDate;
    public Date GameDate
    {
        get
        {
            return gameDate;
        }
        set
        {
            gameDate = value;
        }
    }

    [SerializeField]
    private GameObject prePlayer = null;
    private GameObject player = null;
    public GameObject Player
    {
        get
        {
            if (curLevel != UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex)
            {
                curLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
                world = GameObject.Find("World");
                

                if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
                == SceneManager.Instance.GetLevel("GameScene"))
                {
                    InitGameScene();
                }
            }

            if(player == null)
            {
                Debug.Log("Player is NULL");

                player = GameObject.Find("Player(Clone)");
            }

            return player;
        }
    }
    private Vector2 playerPos;
    public Vector2 PlayerPos
    {
        get
        {
            if (playerPos == null)
                playerPos = Player.transform.localPosition;

            return playerPos;
        }
    }

    private Dictionary<ScheduleType, int> schedulesDic;
    public Dictionary<ScheduleType, int> SchedulesDic
    {
        get
        {
            return schedulesDic;
        }
    }
    private Dictionary<string, float> parameters;
    private Dictionary<string, int> parameterLimit;

    static private bool executeSchedule = false;       // 스케쥴 실행이 예약되어 있는가
    [SerializeField]
    private GameObject prePausePopup = null;
    private GameObject pausePopup = null;

    private Dictionary<string, List<Animator>> animationLayer;
    private Dictionary<string, Sprite>[] preSkinSpriteDic;
    private int curCostumeCode;
    public int CostumeCode
    {
        get
        {
            return curCostumeCode;
        }
        set
        {
            curCostumeCode = value;
        }
    }
    private string[] curSkinNames;
    public string[] CurSkinNames
    {
        get
        {
            return curSkinNames;
        }
    }

    [SerializeField]
    private GameObject preStatPopup = null;
    private GameObject statPopup = null;

    int money = 0;
    public int Money
    {
        get
        {
            return money;
        }
        set
        {
            if (value < 0)
                money = 0;
            else
                money = value;
        }
    }

    private GameObject world;
    public GameObject World
    {
        get
        {
            if(curLevel != UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex)
            {
                curLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
                world = GameObject.Find("World");
                

                if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
            == SceneManager.Instance.GetLevel("GameScene"))
                {
                    InitGameScene();
                }
            }

            return world;
        }
    }

    static int curLevel = -1;

    [SerializeField]
    private GameObject preScheduleButton;
    private Button scheduleButton;

    public ScheduleButtonType scheduleButtonType = ScheduleButtonType.Schedule;

    private int latestScore;
    public int LatestScore
    {
        get
        {
            return latestScore;
        }
        set
        {
            testCount += 1;
            SetParameter("TotalTestScore",
                GetParameter("TotalTestScore") + value);

            latestScore = value;
        }
    }

    private int testCount = 0;
    public int TestCount
    {
        get
        {
            return testCount;
        }
    }

    [SerializeField]
    private GameObject preCredit;
    public GameObject credit;
    public GameObject Credit
    {
        get
        {
            return credit;
        }
    }

    public override void Init()
    {
        if (!inited)
            Start();

        base.Init();
    }

    // Use this for initialization
    void Start()
    {
        base.Init();

        stress = 0;
        schedulesDic = new Dictionary<ScheduleType, int>();

        schedulesDic.Add(ScheduleType.TakeARest, -1);
        schedulesDic.Add(ScheduleType.BasicMath, 4);
        schedulesDic.Add(ScheduleType.English, 4);
        schedulesDic.Add(ScheduleType.Korean, 4);
        schedulesDic.Add(ScheduleType.Volunteer, 4);
        schedulesDic.Add(ScheduleType.Art, 4);
        schedulesDic.Add(ScheduleType.Music, 4);
        schedulesDic.Add(ScheduleType.Parttime, 4);
        schedulesDic.Add(ScheduleType.Science, 4);
        schedulesDic.Add(ScheduleType.WorldHistory, 4);

        parameters = new Dictionary<string, float>();

        parameters.Add("Stress", 0);
        parameters.Add("Math", 0);
        parameters.Add("English", 0);
        parameters.Add("Korean", 0);
        parameters.Add("Science", 0);
        parameters.Add("Social", 0);
        parameters.Add("Art", 0);
        parameters.Add("Music", 0);
        parameters.Add("Volunteer", 0);

        parameters.Add("InterviewScore", 0);
        parameters.Add("TotalTestScore", 0);
        parameters.Add("Course", 1);

        parameterLimit = new Dictionary<string, int>();

        parameterLimit.Add("Stress", 100);
        parameterLimit.Add("Volunteer", 100);

        animationLayer = new Dictionary<string, List<Animator>>();
        preSkinSpriteDic = new Dictionary<string, Sprite>[4];
        curSkinNames = new string[4];
        curCostumeCode = 0;

        for(int i = 1; i <= 4; i += 1)
        {
            Dictionary<string, Sprite> preSprite = new Dictionary<string, Sprite>();
            Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/Skin/" + ((SkinType)i).ToString() + "/");
            for (int j = 0; j < sprites.Length; j += 1)
            {
                preSprite.Add(sprites[j].name, sprites[j]);
            }

            preSkinSpriteDic[i - 1] = preSprite;
        }

        curSkinNames[(int)SkinType.Bed - 1] = "Map_Bed";
        curSkinNames[(int)SkinType.Desk - 1] = "Map_Desk";
        curSkinNames[(int)SkinType.Floor - 1] = "normal_floor";
        curSkinNames[(int)SkinType.Wall - 1] = "normal_wallpaper";

        gameDate.Year = 1;
        gameDate.Month = 3;
        gameDate.Day = 2;
       
        if(prePausePopup == null || preStatPopup == null ||
            preScheduleButton == null || prePlayer == null ||
            preCredit == null)
        {
            Debug.LogWarning("The Prefab NOT PREPARED");
        }

        if (curLevel != UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex)
        {
            curLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
            world = GameObject.Find("World");
            

            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
            == SceneManager.Instance.GetLevel("GameScene"))
            {
                InitGameScene();
            }
        }
    }

    void Update()
    {
        if (!pause)
        {
            SchedulingManager.Instance.update();
            EventManager.Instance.update();
            UIManager.Instance.update();
            MovementManager.Instance.update();

            if(curLevel == SceneManager.Instance.GetLevel("GameScene"))
                playerPos = Player.transform.localPosition;
        }
    }

    public override void OnLevelWasLoaded(int level)
    {
        base.OnLevelWasLoaded(level);
        scheduleButton = null;

        if (curLevel != UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex)
        {
            curLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
            world = GameObject.Find("World");

            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
            == SceneManager.Instance.GetLevel("GameScene"))
            {
                InitGameScene();
            }
        }

        animationLayer = new Dictionary<string, List<Animator>>();
    }

    public void InitGameScene()
    {
        player = Instantiate(prePlayer);
        player.transform.SetParent(world.transform);
        player.transform.localPosition = Vector3.zero;
        player.transform.localScale = Vector3.one;


        if (executeSchedule)
        {
            executeSchedule = false;

            SchedulingManager.Instance.OpenSchedulePopup();
            EventManager.Instance.InitEvents();

            SchedulingManager.Instance.Progressing = true;
        }

        InitScheduleButton();
        
    }

    public void InitScheduleButton()
    {
        if(scheduleButton != null)
        {
            Destroy(scheduleButton.gameObject);
        }

        GameObject obj = Instantiate(preScheduleButton);
        obj.transform.SetParent(UIManager.Instance.Canvas.transform);
        obj.transform.localPosition = new Vector2(165, -303);
        obj.transform.localScale = Vector3.one;
        scheduleButton = obj.GetComponent<Button>();

        switch (scheduleButtonType)
        {
            case ScheduleButtonType.Test:
                scheduleButton.image.sprite =
                    Resources.Load<Sprite>("Sprites/UI/UI_Button_Test_Normal");
                SpriteState state = scheduleButton.spriteState;
                state.pressedSprite = Resources.Load<Sprite>("Sprites/UI/UI_Button_Test_Pushed");
                scheduleButton.spriteState = state;

                scheduleButton.GetComponent<ScheduleButton>().sceneName = "ExamScene";
                break;
            case ScheduleButtonType.Interview:
                scheduleButton.image.sprite =
                    Resources.Load<Sprite>("Sprites/UI/UI_Button_Interview_Normal");
                state = scheduleButton.spriteState;
                state.pressedSprite = Resources.Load<Sprite>("Sprites/UI/UI_Button_Interview_Pushed");
                scheduleButton.spriteState = state;

                ScheduleButton button = scheduleButton.GetComponent<ScheduleButton>();
                button.sceneName = "Interview";
                button.isConvEvent = true;

                break;
            case ScheduleButtonType.End:
                scheduleButton.image.sprite =
                    Resources.Load<Sprite>("Sprites/UI/UI_Button_Ending_Normal");
                state = scheduleButton.spriteState;
                state.pressedSprite = Resources.Load<Sprite>("Sprites/UI/UI_Button_Ending_Pushed");
                scheduleButton.spriteState = state;

                button = scheduleButton.GetComponent<ScheduleButton>();
                button.sceneName = GetGameOverName();
                button.isConvEvent = true;

                break;
        }
    }
    
    public float GetParameter(string name)
    {
        float val = 0;
        if(parameters.TryGetValue(name, out val))
        {
            return val;
        }
        else
        {
            Debug.LogError("parameters DOESN'T HAVE " + name);
            return -1;
        }
    }

    public bool SetParameter(string name, float value)
    {
        if(!parameters.ContainsKey(name))
        {
            Debug.LogError("parameters DOESN'T HAVE " + name);
            return false;
        }

        if (value < 0)
        {
            value = 0;
            Debug.LogWarning("Parameter " + name + " LowerLimit Bound");
        }

        int limit;
        if(parameterLimit.TryGetValue(name, out limit))
        {
            if (value > limit)
            {
                value = limit;
                Debug.LogWarning("Parameter " + name + " UpperLimit Bound");
            }
        }

        parameters.Remove(name);
        parameters.Add(name, value);

        return true;
    }

    public void ScheduleExecute()
    {
        executeSchedule = true;
        SceneManager.Instance.ChangeScene("GameScene");
    }

    public void PauseGame()
    {
        UIManager.Instance.SetEnableTouchLayer("Main", false);
        pause = true;

        pausePopup = Instantiate(prePausePopup);
        pausePopup.transform.parent = UIManager.Instance.Canvas.transform;
        pausePopup.transform.localPosition = Vector3.zero;
        pausePopup.transform.localScale = Vector3.one;

        SetPlayAnimator("Player", false);

        if (SchedulingManager.Instance.Progressing)
            SetPlayAnimator("CutScene", false);
    }

    public void ResumeGame()
    {
        UIManager.Instance.SetEnableTouchLayer("Main", true);
        pause = false;

        Destroy(pausePopup);
        pausePopup = null;

        SetPlayAnimator("Player", true);

        if (SchedulingManager.Instance.Progressing)
            SetPlayAnimator("CutScene", true);
    }

    public void AddAnimator(string layerName, Animator animator)
    {
        if(animationLayer.ContainsKey(layerName))
        {
            List<Animator> list;
            animationLayer.TryGetValue(layerName, out list);
            list.Add(animator);
            animationLayer.Remove(layerName);
            animationLayer.Add(layerName, list);
        }
        else
        {
            List<Animator> list = new List<Animator>();
            list.Add(animator);
            animationLayer.Add(layerName, list);
        }
    }

    public bool SetPlayAnimator(string layerName, bool play)
    {
        if(animationLayer.ContainsKey(layerName))
        {
            List<Animator> list;
            animationLayer.TryGetValue(layerName, out list);
            foreach(var iter in list)
            {
                iter.enabled = play;
            }

            return true;
        }
        else
        {
            Debug.LogWarning(layerName + " layer DOESN'T EXIST");
            return false;
        }
    }

    public void SetSkinName(SkinType type, string name)
    {
        curSkinNames[(int)type - 1] = name;
    }

    public Sprite GetAppropriateSkin(SkinType type)
    {
        Sprite sprite;
        if(preSkinSpriteDic[(int)type - 1]
            .TryGetValue(curSkinNames[(int)type - 1], out sprite))
        {
            return sprite;
        }
        else
        {
            Debug.LogError("Coundn't FIND Sprite" + curSkinNames[(int)type - 1]);
            return null;
        }
    } 

    public void OpenStatPopup()
    {
        statPopup = Instantiate(preStatPopup);
        statPopup.transform.SetParent(UIManager.Instance.Canvas.transform);
        statPopup.transform.localScale = Vector3.one;
        statPopup.transform.localPosition = Vector3.zero;
    }

    public void CloseStatPopup()
    {
        Destroy(statPopup);
        statPopup = null;
    }

    public bool GameOverCheck()
    {
        if (gameDate.Year == 3 && gameDate.Month == 12 && gameDate.Day == 1)
            return true;
        else
            return false;
    }

    public string GetGameOverName()
    {
        return "Military Ending";
    }

    public void AddParameter(string name)
    {
        if(!parameters.ContainsKey(name))
        {
            parameters.Add(name, 0);
        }
        else
        {
            Debug.LogError("The Parameter is ALREADY EXIST");
            return;
        }
    }

    public void OpenCredit()
    {
        UIManager.Instance.SetEnableTouchLayer("PausePopup", false);

        credit = Instantiate(preCredit);
        credit.transform.SetParent(UIManager.Instance.Canvas.transform);
        credit.transform.localPosition = Vector2.zero;
        credit.transform.localScale = Vector3.one;
    }

    public void CloseCredit()
    {
        if(credit != null)
        {
            UIManager.Instance.SetEnableTouchLayer("PausePopup", true);

            Destroy(credit);
            credit = null;
        }
    }
}
