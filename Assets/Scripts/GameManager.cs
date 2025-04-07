using System.Collections.Generic;
using Dungeon;
using NavMeshPlus.Components;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GeneratorStage[] stages;

    [SerializeField] private DungeonViewGenerator dungeonViewGenerator;
    [SerializeField] private NavMeshSurface _surface;
    [SerializeField] private EntityPoint point;
    [SerializeField] List<DungeonEntities> dungeonEntities;

    private List<Vector2> _stagePoints;
    public IReadOnlyList<Vector2> StagePoints => _stagePoints;

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
        _stagePoints = dungeonViewGenerator.starts;
        foreach(var v in _stagePoints)
        {
            Debug.Log(v);
        }
        _surface.BuildNavMesh();
        for (int s = 0; s < places.Count; s++)
        {
            foreach (var p in places[s])
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
