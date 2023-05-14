using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressUIController : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> states = new List<GameObject>();
    [SerializeField]
    private TrainingDropManager trainingDropManager;
    [SerializeField]
    private Color activeStateColor;
    [SerializeField]
    private Color doneStateColor;
    [SerializeField]
    private Scrollbar scrollbar;

    private int statesCount;
    private int currentStateIndex = 0;
    private bool isStarted = false;
    private int curSkipStatesCount = 0;
    private int skipStatesCount = 1;
    private void Awake() => statesCount = states.Count;

    private void Start()
    {
        if (!trainingDropManager.isActiveAndEnabled)
            gameObject.SetActive(false);

        if (scrollbar)
            StartCoroutine(SetScrollBapPos());
    }

    IEnumerator SetScrollBapPos()
    {
        yield return null;
        scrollbar.value = 1;
    }

    private void OnEnable() => trainingDropManager.OnStateChanged += ChangeState;

    private void ChangeState()
    {
        if (currentStateIndex >= statesCount)
            return;

        if (trainingDropManager.Scenario == Constants.ScenarioOne)
        {
            foreach (Transform item in states[currentStateIndex].transform)
                item.GetComponent<Image>().color = doneStateColor;
            currentStateIndex++;

            if (currentStateIndex >= statesCount)
                return;

            foreach (Transform item in states[currentStateIndex].transform)
                item.GetComponent<Image>().color = activeStateColor;
        }
        else if (trainingDropManager.Scenario == Constants.ScenarioTwo)
        {
            curSkipStatesCount++;
            if (curSkipStatesCount <= skipStatesCount)
                return;
            if (currentStateIndex >= statesCount)
                return;

            var image = states[currentStateIndex].transform.GetChild(0).GetComponent<Image>();
            if (image.color != activeStateColor && image.color != doneStateColor)
            {
                foreach (Transform item in states[currentStateIndex].transform)
                    item.GetComponent<Image>().color = activeStateColor;
            }
            else
            {
                foreach (Transform item in states[currentStateIndex].transform)
                    item.GetComponent<Image>().color = doneStateColor;
                currentStateIndex++;

                if (currentStateIndex >= statesCount)
                    return;

                foreach (Transform item in states[currentStateIndex].transform)
                    item.GetComponent<Image>().color = activeStateColor;

                if (currentStateIndex == statesCount - 1)
                {
                    foreach (Transform item in states[currentStateIndex].transform)
                        item.GetComponent<Image>().color = doneStateColor;
                }
            }
        }
    }
}
