using Dungeon;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GeneratorStage[] stages;

    public GeneratorStage[] Stages => stages; 

    public static GameManager Instance{get; private set; }

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        Destroy(gameObject);
    }

    void Update()
    {
        
    }
}
