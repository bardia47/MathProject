using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{

    public static UIManager manager;
    public  TMP_Text numberOne, numberTwo,numberThree,operation;
    public TMP_InputField input;
    [Header("Score")]
    public TMP_Text scoreText;
    [SerializeField]
    private int score;
    [Header("Timer")]
    public float maxTimer;
    public float timerLeft;
    private bool isEnded = false;
    public float secondsLostForWrongAnswer;
    public TMP_Text timerText;

    [Header("HighScores")]
    public CanvasGroup highScoreCg;
    public HighScoreDisplay[] highScoreDisplays;

    public int startingTimer = 3;
    public TMP_Text startingTimerText;
    public int Score
    {
        get { return score; }
        set { score = value;
            UpdateScoreDisplay();
                }
    }

    private void UpdateScoreDisplay()
    {
        scoreText.transform.DOPunchScale(Vector3.one * 0.5f,0.2f);
        scoreText.DOColor(Color.green, 0.1f).OnComplete(() => {
            scoreText.DOColor(Color.white, 0.5f);
        });
        scoreText.text = score.ToString();
    }

    private void Awake()
    {
        if (manager != null && manager != this)
        {
            Destroy(gameObject);
            return;
        }
        manager = this;
    }

    public void UpdateEquationDisplay()
    {
        numberOne.text = EquationManager.manager.numberOne.ToString();
        numberTwo.text = EquationManager.manager.numberTwo.ToString();
        numberThree.text = EquationManager.manager.numberThree.ToString();
        operation.text = GetOperationType();
        int maxRange = EquationManager.manager.opType.Equals(Operations.DIV)? 2 : 3;
        int r = Random.Range(0, maxRange);
        switch (r)
        {
            case 0:
                EquationManager.manager.selectedNum = EquationManager.manager.numberOne;
                numberOne.text = "__";
                break;
            case 1:
                EquationManager.manager.selectedNum = EquationManager.manager.numberTwo;
                numberTwo.text = "__";
                break;
            case 2:
                EquationManager.manager.selectedNum = EquationManager.manager.numberThree;
                numberThree.text = "__";
                break;

        }
    }

    private string GetOperationType()
    {
        switch (EquationManager.manager.opType)
        {
            case Operations.ADD:
                return "+";
            case Operations.SUB:
                return "-";
            case Operations.MUL:
                return "*";
            case Operations.DIV:
                return "/";
            default:
                return "";
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        timerLeft = maxTimer;
        StartCoroutine(InitializeStartingCountDown());

    }

 

    // Update is called once per frame
    void Update()
    {
        if ((timerLeft <= 0 && isEnded) || startingTimer > 0)
            return;
        timerLeft -= Time.deltaTime;
        if (timerLeft < 0f) {
            isEnded = true;
            timerLeft = 0;
            HighScoreManager.manager.SaveScore(Score);
        
        }

        UpdateTimerDisplay();
    }

    private void UpdateTimerDisplay(bool modify  = false, bool correct= false)
    {
        if (modify)
        {
            timerText.transform.DOPunchScale(Vector3.one * 0.5f, 0.2f);
            timerText.DOColor(correct? Color.green : Color.red, 0.1f).OnComplete(() => {
                timerText.DOColor(Color.white, 0.5f);
            });
        }
        timerText.text = timerLeft.ToString((timerLeft < 10f) ? "F1" : "F0" ) ;
    }

    public void OnSumbitAnswer(string ss) {

        if ((timerLeft <= 0 && isEnded) || startingTimer > 0)
            return;

        if (!string.IsNullOrEmpty(ss))
        {
            float num = 0;
            float.TryParse(ss, out num);
            if (num.Equals(EquationManager.manager.selectedNum))
            {
                EquationManager.manager.GenerateNewOperation();
                Score++;
              //  UpdateTimerDisplay(true, true);

            }
            else
            {
                timerLeft -= secondsLostForWrongAnswer;
                UpdateTimerDisplay(true, false);
            }
        }
        input.text = "";
        input.ActivateInputField();

    }

    public void UpdateHighScores() {
        for (int i = 0; i < highScoreDisplays.Length; i++)
        {
            if (i < HighScoreManager.manager.scores.Count)
            {
                highScoreDisplays[i].setScore(HighScoreManager.manager.scores[i]);

            }
            else
                highScoreDisplays[i].scoreText.text = "";
        }
    }

    public void ToggleHighScoreTable() {
        UpdateHighScores();
        highScoreCg.blocksRaycasts = false;
        highScoreCg.interactable = false;
        highScoreCg.DOFade(highScoreCg.alpha==1f? 0f : 1f,0.2f).OnComplete(() => {

            highScoreCg.blocksRaycasts = highScoreCg.alpha == 1f;
            highScoreCg.interactable = highScoreCg.alpha == 1f;

        });
    }

    public void OnPlayAgin() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnQuitGame()
    {
        Application.Quit();
    }

    private void StartGame() {
        input.interactable = true;
        input.ActivateInputField();
        UpdateTimerDisplay();
        UpdateScoreDisplay();
    }
    private IEnumerator InitializeStartingCountDown()
    {
        for (int i = startingTimer; i >0 ; i--)
        {
            startingTimerText.transform.localScale = Vector3.zero;
            startingTimerText.text = i.ToString();
            startingTimerText.transform.DOScale(1f, 1f);
            yield return new WaitForSeconds(1);
        }
        startingTimer = 0;
        startingTimerText.gameObject.SetActive(false);
        StartGame();
    }
}
