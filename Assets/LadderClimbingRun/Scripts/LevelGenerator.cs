using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    private int currentLevelLenght = 0;
    public int chunckBasePoint = 0;
    [SerializeField] private float chunkUnitLenght = 50;
    [SerializeField] private float threshold = 20;
    private List<int> occupiedPoints;
    private List<int> coinPlaces;
    private List<int> obstaclePlaces;
    private List<int> ladderPlaces;
    private List<int> obsLadderPlaces;
    ObjectPooler objectPooler;
    [SerializeField] private GameObject player = null;
    public List<Vector3> spikes;


    // Start is called before the first frame update
    void Start()
    {
        objectPooler = ObjectPooler.Instance;
        occupiedPoints = new List<int>();
        coinPlaces = new List<int>();
        obstaclePlaces = new List<int>();
        ladderPlaces = new List<int>();
        obsLadderPlaces = new List<int>();
        spikes = new List<Vector3>();
       
        
        GenerateChunk();
    }

    // Update is called once per frame
    void Update()
    {
        CreateChunck();
    }
    private void CreateChunck()
    {
        
        if((player.transform.position.y + threshold )>= currentLevelLenght)
        {  
            Vector3 pos = transform.position;
            pos.y += 50;
            transform.position = pos;
            chunckBasePoint += 50;
            GenerateChunk();
        }
        else
        {
            
        }
    }
    private void GenerateChunk()
    {
        currentLevelLenght += 50;
        GenerateLadder();
        GenerateCollectibles();
        GenerateObstacles("TV");
        GenerateObstacles("Toilet");
        GenerateObstacles("Plant");
        GenerateObstacles("Chair");
        GenerateObstacles("Cactus");
        GenerateObstacles("Table");
    }

    private void GenerateLadder()
    {

        for (int x = -2; x < 3; x += 2)
        {
            int a;
            for (int i = 0; i < 5; i++)
            {
                a = Random.Range(chunckBasePoint+4, currentLevelLenght);
                if (!occupiedPoints.Contains(a))
                {
                    occupiedPoints.Add(a);
                                       
                }
            }
            int b;
            for (int i = 0; i < 3; i++)
            {
                b = Random.Range(chunckBasePoint+5, currentLevelLenght);
                if (!ladderPlaces.Contains(b) && !occupiedPoints.Contains(b))
                {
                    ladderPlaces.Add(b);

                }
            }

            for (int i = 0; i < chunkUnitLenght; i++)
            {
                if (occupiedPoints.Contains(i+chunckBasePoint) && !obsLadderPlaces.Contains(i-1+chunckBasePoint))
                {
                    objectPooler.SpawnFromPool("BrokenLadder", new Vector3(x, i+ chunckBasePoint, 0), Quaternion.identity);
                    obsLadderPlaces.Add(i + chunckBasePoint);
                }
                else if (ladderPlaces.Contains(i+ chunckBasePoint) && !obsLadderPlaces.Contains(i - 1 + chunckBasePoint))
                {
                    GameObject obj = objectPooler.SpawnFromPool("SpikeLadder", new Vector3(x, i + chunckBasePoint, 0), Quaternion.identity);

                    spikes.Add(obj.transform.position);
                    obsLadderPlaces.Add(i + chunckBasePoint);
                }
                else if(!occupiedPoints.Contains(i+ chunckBasePoint) && !ladderPlaces.Contains(i+ chunckBasePoint))
                {
                    objectPooler.SpawnFromPool("Ladder", new Vector3(x, i+chunckBasePoint, 0), Quaternion.identity);
                    
                }
                else
                {
                    objectPooler.SpawnFromPool("Ladder", new Vector3(x, i + chunckBasePoint, 0), Quaternion.identity);

                }

            }

            occupiedPoints.Clear();
            ladderPlaces.Clear();

        }
               

    }
    private void GenerateCollectibles()
    {
        for (int x = -2; x < 3; x += 2)
        {
            int a;
            for (int i = 0; i < 15; i++)
            {
                a = Random.Range(chunckBasePoint+3, currentLevelLenght);
                if (!occupiedPoints.Contains(a))
                {
                    occupiedPoints.Add(a);
                    coinPlaces.Add(a);
                }

            }

            for (int i = 0; i < chunkUnitLenght; i++)
            {
                if (occupiedPoints.Contains(i+ chunckBasePoint) && !obsLadderPlaces.Contains(i+chunckBasePoint))
                {
                    objectPooler.SpawnFromPool("Coin", new Vector3(x, i+ chunckBasePoint, -0.5f), Quaternion.identity);
                }           
            }
            occupiedPoints.Clear();

        }
    }
    private void GenerateObstacles(string tag)
    {
        for (int x = -2; x < 3; x += 2)
        {
            int a;
            for (int i = 0; i < 1; i++)
            {
                a = Random.Range(chunckBasePoint+5, currentLevelLenght);
                if (!occupiedPoints.Contains(a))
                {
                    occupiedPoints.Add(a);
                    
                }
            }

            for (int i = 0; i < chunkUnitLenght; i++)
            {
                if (occupiedPoints.Contains(i+ chunckBasePoint) && !coinPlaces.Contains(i+ chunckBasePoint) && !obstaclePlaces.Contains(i+ chunckBasePoint))
                {

                    objectPooler.SpawnFromPool(tag, new Vector3(x, i+1+chunckBasePoint, -0.5f), Quaternion.identity);              
                    obstaclePlaces.Add(i+ chunckBasePoint);
                }
            }
            occupiedPoints.Clear();
        }
    }
}
