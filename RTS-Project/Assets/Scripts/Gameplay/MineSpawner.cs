using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSpawner : MonoBehaviour
{
    public int currentMines;
    public int maxMines;
    public float minTime;
    public float maxTime;

    private Grid grid;
    private float finalTime;
    private float spawnTimer;
    // Start is called before the first frame update
    void Start()
    {
        grid = Grid.Get();
        SetNewTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentMines < maxMines)
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

    private void SpawnMine()
    {
        if(grid)
        {
            List<Node> allNormalNodes = new List<Node>();

            foreach (Node n in grid.nodes)
            {
                OreMine o = n.GetComponent<OreMine>();

                if (!o.enabled)
                    allNormalNodes.Add(n);
            }

            int randomIndex = Random.Range(0, allNormalNodes.Count);
            allNormalNodes[randomIndex].GetComponent<OreMine>().enabled = true;
            currentMines++;

        }
    }
}
