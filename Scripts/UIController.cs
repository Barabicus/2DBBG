using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    public Image image;
    public Text UILabel;

    private float _lastTime;
    private UIState _uiState;
    private float _timeOut;

    enum UIState
    {
        Persistent,
        Timer
    }

    public static UIController Instance
    {
        get;
        private set;
    }


    public void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.LogError("UI Controller Instance already exists");
            Destroy(this);
            return;
        }
    }

    public void CloseUI()
    {
        image.gameObject.SetActive(false);
    }

    public void SetText(string text, float timeout)
    {
        image.gameObject.SetActive(true);

        UILabel.text = text;
        if (timeout > 0)
        {
            _uiState = UIState.Timer;
            _lastTime = Time.time;
            _timeOut = timeout;
        }
        else
            _uiState = UIState.Persistent;
    }

    public void Update()
    {
        switch (_uiState)
        {
            case UIState.Timer:
                if (Time.time - _lastTime > _timeOut)
                {
                    image.gameObject.SetActive(false);
               //     image.color = Color.Lerp(image.color, Color.clear, Time.deltaTime);
                //    UILabel.color = Color.Lerp(image.color, Color.clear, Time.deltaTime);
                }
                break;
        }
    }

    IEnumerator HideUI()
    {
        yield return new WaitForSeconds(1);
        if (Time.time - _lastTime > _timeOut)
            image.gameObject.SetActive(false);
    }

}
