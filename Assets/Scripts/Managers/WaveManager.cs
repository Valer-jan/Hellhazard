using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public Wave[] Waves;
    [Space]
    public SphereCollider StartTrigger;
    public Transform[] Spowners;
    [Space]
    public int LevelNumber;

    public static int EnemiesLeft = 0;
    bool StartWaves = false, VictoryFlag = false;
    int PresentWave = 0;
    HeroController hero;

    private void Start()
    {
        hero = FindObjectOfType<HeroController>();
    }

    private void Update()
    {
        if (!StartWaves && Vector3.Distance(StartTrigger.transform.position, hero.transform.position) < StartTrigger.radius)
        {
            StartWaves = true;
            Destroy(StartTrigger.gameObject);
        }

        if (StartWaves && EnemiesLeft == 0)
        {
            if (PresentWave < Waves.Length)
            {
                // next wave
                PresentWave++;
                StartCoroutine(StartNextWave(PresentWave));
            }
            else if (!VictoryFlag)
            {
                // victory
                VictoryFlag = true;
                Debug.Log("Level # " + LevelNumber + " victory!");
                ProgressManager.SaveProgress();
            }
        }
    }

    IEnumerator StartNextWave(int WaveNumber)
    {
        Debug.Log("Wave # " + WaveNumber + " started");
        WaveNumber--;
        for (int i = 0; i < Waves[WaveNumber].Enemy.Length; i++)
        {
            for (int j = 0; j < Waves[WaveNumber].Count[i]; j++)
            {
                Instantiate(Waves[WaveNumber].Enemy[i], Spowners[Random.Range(0, Spowners.Length-1)].position, Quaternion.identity);
                yield return new WaitForSeconds(1f);
            }
        }
    }
}