using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    public Image image;
    public Text UILabel;
    public LoadingStatus loadStatus;

    private float _lastTime;
    private UIState _uiState;
    private float _timeOut;

    enum UIState
    {
        Persistent,
        Timer
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
        if (loadStatus.isLoading)
            SetText(loadStatus.FormattedLoadingText, 0.5f);

        switch (_uiState)
        {
            case UIState.Timer:
                if (Time.time - _lastTime > _timeOut)
                {
                    image.gameObject.SetActive(false);
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
