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

    public float timeIntial = 2f;

    private IEnumerator coroutine;

    public float leftTime { get; private set; } = 0;

    public float elapsedTime;

    public int lenStages = 3;

    public float getLeftTime()
    {
        return leftTime;
    }

    public event Action<Stage> StageChanged;

    public Stage CurrentStage { get; private set; } = Stage.Fight;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance == this)
        {
            Destroy(gameObject);
        }

        //DontDestroyOnLoad(gameObject);

        coroutine = StartTimer(timeForLooting, timeForFighting);
        StartCoroutine(coroutine);
    }

    private IEnumerator StartTimer(float timeChill, float timeFight)
    {
        leftTime = timeIntial;
        elapsedTime = 0;
        int numberStages = 0;
        while (true)
        {
            yield return null;
            leftTime -= Time.deltaTime;
            elapsedTime += Time.deltaTime;
            if (leftTime < 0)
            {
                CurrentStage = CurrentStage switch
                {
                    Stage.Clill => Stage.Fight,
                    Stage.Fight => Stage.Clill,
                    _ => throw new NotImplementedException()
                };

                StageChanged?.Invoke(CurrentStage);

                if (CurrentStage == Stage.Fight)
                {
                    numberStages += 1;
                    if (numberStages >= lenStages)
                    {
                        leftTime = 0;
                        break;
                    }
                }

                leftTime = CurrentStage == Stage.Fight ? timeFight : timeChill;
            }

        }
    }
}
