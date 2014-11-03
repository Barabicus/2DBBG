using UnityEngine;
using System.Collections;

public class LoadingStatus : MonoBehaviour
{
    public bool isLoading;
    public string loadingText;
    public int currentAmount;
    public int finishAmount;

    public string FormattedLoadingText
    {
        get { return string.Format("{0} : {1} / {2}", loadingText, currentAmount, finishAmount); }
    }

    public void Increment()
    {
        currentAmount++;
    }

}
