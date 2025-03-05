using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AnimalType
{
    Chicken = 0,
    Goat = 1,
    Lion = 2,
    Pig = 3,
    Rabbit = 4,
    Cow = 5
}

[System.Serializable]
public struct Animals
{
    public AnimalType type;
    public string name;

    public Animals(AnimalType type, string name)
    {
        this.type = type;
        this.name = name;
    }
}

public class FarmMgr : MonoBehaviour
{
    [Header("Animals (Need to Change to Local Data)")]
    public GameObject[] animalPrefabs;
    public Animals[] animalsGot = new Animals[12]; // 최대 동물 소유 개수는 10개    
    public int animalCount = 0;    

    [Header("Mountain Elements")]
    public Transform[] animalSpawnPos_mt;
    private HashSet<int> uniqueNumbers_mt = new HashSet<int>(); // 중복 방지용 HashSet
    public int[] spawnFieldArr_mt;

    [Header("Field Elements")]
    public Transform[] animalSpawnPos_f;
    private HashSet<int> uniqueNumbers_f = new HashSet<int>(); // 중복 방지용 HashSet
    public int[] spawnFieldArr_f;


    private void Start()
    {
        spawnFieldArr_mt = new int[animalSpawnPos_mt.Length];
        spawnFieldArr_f = new int[animalSpawnPos_f.Length];
    }

    public void CatchAnimal(int animalNum)
    {
        Debug.Log("Catch Animal : " + animalNum);

        if (animalCount < animalsGot.Length)
        {
            Animals randAnimal = new Animals();

            if (animalNum == 1) randAnimal = new Animals(AnimalType.Chicken, "Cuckoo");
            else if (animalNum == 2) randAnimal = new Animals(AnimalType.Goat, "siu");
            else if (animalNum == 3) randAnimal = new Animals(AnimalType.Lion, "Mupasa");
            else if (animalNum == 4) randAnimal = new Animals(AnimalType.Pig, "pig");
            else if (animalNum == 5) randAnimal = new Animals(AnimalType.Rabbit, "rab");
            else if (animalNum == 6) randAnimal = new Animals(AnimalType.Cow, "Ben");

            animalsGot[animalCount] = randAnimal;
            animalCount++;
        }
        else Debug.Log("Farm is full now");
    }

    public void KillAnimal()
    {
        Debug.Log("number is : " + uniqueNumbers_mt.Count + " / " + animalSpawnPos_mt.Length);
    }

    public void AnimalMountainSpawn()
    {
        if (animalCount > 0)
        {
            Debug.Log("Animal Count is over 0_Mountain");

            int randomFieldNum = 0;
            for (int i = 0; i < animalCount; i++)
            {
                while (uniqueNumbers_mt.Count < animalSpawnPos_mt.Length)
                {
                    randomFieldNum = Random.Range(0, animalSpawnPos_mt.Length);
                    uniqueNumbers_mt.Add(randomFieldNum);
                }

                uniqueNumbers_mt.CopyTo(spawnFieldArr_mt);

                GameObject go = Instantiate(animalPrefabs[(int)animalsGot[i].type],
                    animalSpawnPos_mt[spawnFieldArr_mt[i]].position,
                    Quaternion.identity);
                
                go.transform.SetParent(animalSpawnPos_mt[spawnFieldArr_mt[i]]);
            }
        }        
    }

    public void ClearMountain()
    {
        if (animalCount > 0)
        {
            for (int i = 0; i < spawnFieldArr_mt.Length; i++)
            {
                if (animalSpawnPos_mt[i].childCount > 0) Destroy(animalSpawnPos_mt[i].GetChild(0).gameObject);
            }

            uniqueNumbers_mt.Clear();
        }
    }

    public void AnimalFieldSpawn()
    {
        if (animalCount > 0)
        {
            Debug.Log("Animal Count is over 0_Field");

            int randomFieldNum = 0;
            for (int i = 0; i < animalCount; i++)
            {
                while (uniqueNumbers_f.Count < animalSpawnPos_f.Length)
                {
                    randomFieldNum = Random.Range(0, animalSpawnPos_f.Length);
                    uniqueNumbers_f.Add(randomFieldNum);
                }

                uniqueNumbers_f.CopyTo(spawnFieldArr_f);

                GameObject go = Instantiate(animalPrefabs[(int)animalsGot[i].type],
                    animalSpawnPos_f[spawnFieldArr_f[i]].position,
                    Quaternion.Euler(0, 135, 0));

                go.transform.SetParent(animalSpawnPos_f[spawnFieldArr_f[i]]);
                go.transform.GetComponent<NavMeshAgent>().enabled = false;
                go.transform.GetComponent<RandomMovement>().enabled = false;

                GetComponent<GameMgr>().FieldPosScaling();
            }
        }
    }

    public void ClearField()
    {
        if (animalCount > 0)
        {
            for (int i = 0; i < animalSpawnPos_f.Length; i++)
            {
                if (animalSpawnPos_f[i].childCount > 0) 
                {
                    Debug.Log(animalSpawnPos_f[i].name);                   
                    Destroy(animalSpawnPos_f[i].GetChild(0).gameObject);                    
                }                
            }

            uniqueNumbers_f.Clear();

            /*
            for (int i = 0; i < animalSpawnPos_f.Length; i++)
            {
                Animator anim = animalSpawnPos_f[i].GetChild(0).GetComponent<Animator>();
                if (anim.enabled == true) anim.enabled = false;

                animalSpawnPos_f[i].localScale = new Vector3(0.1f, 0.1f, 0.1f);
                Debug.Log(i);
            }
            */
        }
    }
}
