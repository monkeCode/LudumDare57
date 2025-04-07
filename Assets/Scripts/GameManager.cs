using System.Collections.Generic;
using Dungeon;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GeneratorStage[] stages;

    [SerializeField] private DungeonViewGenerator dungeonViewGenerator;

    [SerializeField] private EntityPoint point;
    [SerializeField] List<DungeonEntities> dungeonEntities;

    public GeneratorStage[] Stages => stages;
    [field: SerializeField] public FightStage[] FightStages { get; private set; }

    public static GameManager Instance { get; private set; }

    public Stage CurrentStage => Timer.instance.CurrentStage;
    public int CountStage { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(gameObject);
    }

    void Start()
    {
        var places = dungeonViewGenerator.GenerateDungeon();
        for(int s = 0; s < places.Count; s++)
        {
            foreach(var p in places[s])
            {
                var o = Instantiate(point, p.transform.position, Quaternion.identity);
                o.SetEntities(dungeonEntities[s]);
            }
        }
    }

    void IncrementStage(Stage stage)
    {
        if (stage == Stage.Clill)
        {
            CountStage += 1;
        }
    }

    void OnEnable()
    {
        Timer.instance.StageChanged += IncrementStage;
    }

    void OnDisable()
    {
        Timer.instance.StageChanged -= IncrementStage;
    }

}
