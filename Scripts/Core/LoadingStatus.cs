using UnityEngine;
using System.Collections;
using System;

public class LoadingStatus : MonoBehaviour
{
    public bool isLoading;
    public string loadingText;
    public int currentAmount;
    public int finishAmount;
    public event Action loadingFinished;

    public string FormattedLoadingText
    {
        get { return string.Format("{0} : {1} / {2}", loadingText, currentAmount, finishAmount); }
    }

    public void Increment()
    {
        currentAmount++;
        if (currentAmount == finishAmount && loadingFinished != null)
        {
            loadingFinished();
            loadingFinished = null;
        }
    }

}
