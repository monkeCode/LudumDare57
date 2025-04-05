using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour
{
    public static Timer instance = null;
    public static bool platformRiding = false;

    public float timeForLooting = 60f;

    public float timeForFighting = 45f;

    private IEnumerator coroutine;

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

        coroutine = Time(timeForLooting, timeForFighting);
        StartCoroutine(coroutine);
    }

    private IEnumerator Time(float timeChill, float timeFight)
    {
        while (true)
        {
            if (platformRiding)
            {
                yield return new WaitForSeconds(timeChill);
                platformRiding = !platformRiding;
            }
            else
            {
                yield return new WaitForSeconds(timeFight);
                platformRiding = !platformRiding;
            }
        }
    }
}
