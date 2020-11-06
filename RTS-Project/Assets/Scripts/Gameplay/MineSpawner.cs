using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSpawner : MonoBehaviour
{
    public int currentUnexploredMines;
    public int maxMines;
    public int maxUnexploredMines;
    public float minTime;
    public float maxTime;
    public List<GameObject> mines = new List<GameObject>();

    private Grid grid;
    private float finalTime;
    private float spawnTimer;
    // Start is called before the first frame update
    void Start()
    {
        grid = Grid.Get();
        SetNewTimer();
        MarkBehaviour.OnMarkDone += SubstractUnexploredMines;
        MiningAction.OnDestroyMine += DeleteMine;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentUnexploredMines < maxUnexploredMines && mines.Count < maxMines)
        {
            spawnTimer += Time.deltaTime;

            if(spawnTimer >= finalTime)
            {
                SpawnMine();
                SetNewTimer();
            }
        }
    }

    private void SetNewTimer()
    {
        float randomTime = Random.Range(minTime, maxTime);
        finalTime = randomTime;
        spawnTimer = 0;
    }

    private void DeleteMine(GameObject mine)
    {
        if(mines.Contains(mine))
        {
            OreMine oreMine = mine.GetComponent<OreMine>();
            oreMine.isMarked = false;
            oreMine.enabled = false;
            Destroy(oreMine.currentFlag);
            mines.Remove(mine);
        }
    }

    private void SubstractUnexploredMines(GameObject mine)
    {
        currentUnexploredMines--;
    }

    private void SpawnMine()
    {
        if(grid)
        {
            List<Node> allNormalNodes = new List<Node>();

            foreach (Node n in grid.nodes)
            {
                OreMine o = n.GetComponent<OreMine>();

                if (!o.enabled && n.isWalkable)
                    allNormalNodes.Add(n);
            }

            int randomIndex = Random.Range(0, allNormalNodes.Count);
            allNormalNodes[randomIndex].GetComponent<OreMine>().enabled = true;
            mines.Add(allNormalNodes[randomIndex].gameObject);
            currentUnexploredMines++;

        }
    }

    private void OnDestroy()
    {
        MarkBehaviour.OnMarkDone -= SubstractUnexploredMines;
        MiningAction.OnDestroyMine -= DeleteMine;
    }
}
