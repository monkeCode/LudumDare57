using UnityEngine;
using System.Collections;
using System;


public enum Stage
{
    Clill,
    Fight,
}

public class Timer : MonoBehaviour
{
    public static Timer instance = null;
    public bool platformRiding = false;

    public float timeForLooting = 60f;

    public float timeForFighting = 45f;

    private IEnumerator coroutine;

    public float leftTime = 0;

    public event Action<Stage> StageChanged;

    public Stage CurrentStage {get; private set;}

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance == this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        coroutine = StartTimer(timeForLooting, timeForFighting);
        StartCoroutine(coroutine);
    }

    private IEnumerator StartTimer(float timeChill, float timeFight)
    {
        while (true)
        {
            yield return null;
            leftTime -= Time.deltaTime;
            if (leftTime < 0)
            {
                CurrentStage = CurrentStage switch
                {
                    Stage.Clill => Stage.Fight,
                    Stage.Fight => Stage.Clill,
                    _ => throw new NotImplementedException()
                };

                StageChanged.Invoke(CurrentStage);

                leftTime = CurrentStage==Stage.Fight?timeFight:timeChill;
            }

        }
    }
}
