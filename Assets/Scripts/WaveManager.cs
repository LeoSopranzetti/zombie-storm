using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

    public int currentWave = 1;

    public int zombiesPerWave;
    public int zombiesRemaining = 0;
    public int maxZombiePerWave = 25;
    private float increaseZombieLife = 0;
    private float increaseZombieDamage = 0;
    private bool zombieWave = true;

    public int bossPerWave;
    public int bossRemaining = 0;
    public int maxBossPerWave = 5;
    private float increaseBossLife = 0;
    private float increaseBossDamage = 0;
    private bool bossWave = false;

    private float nextSpawnTime = 0f;
    private bool canSpawn = true;

    private WaveManager myWaveManager;
    public static WaveManager instance;

    public GameObject zombieSpawner;
    private GeradorZumbis scriptZombieSpawner;

    public GameObject bossSpawner;
    private GeradorChefe scriptBossSpawner;

    private ControlaInterface scriptControlaInterface;
    public AudioClip somDeGeracao;


    private void Awake()
    {
        myWaveManager = GetComponent<WaveManager>();
        scriptControlaInterface = GameObject.FindObjectOfType(typeof(ControlaInterface)) as ControlaInterface;
        instance = myWaveManager;
    }

    void Start()
    {
        scriptZombieSpawner = zombieSpawner.GetComponent<GeradorZumbis>();
        scriptBossSpawner = bossSpawner.GetComponent<GeradorChefe>();
        StartNextWave();
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (zombieWave)
        {
            string isZombie = "ZOMBIE";
            checkSpawnRules(zombiesPerWave, canSpawn, zombiesRemaining, isZombie);
        }


        if (bossWave)
        {
            string isBoss = "BOSS";
            checkSpawnRules(bossPerWave, canSpawn, bossRemaining, isBoss);
        }
    }

    void StartNextWave()
    {
        currentWave++;

        if (currentWave % 5 != 0)
        {
            zombieWave = true;
            bossWave = false;
        }

        if (currentWave % 5 == 0)
        {
            bossWave = true;
            zombieWave = false;
        }


        zombiesPerWave = maxZombiePerWave;
        zombiesRemaining = zombiesPerWave;

        bossPerWave = maxBossPerWave;
        bossRemaining = bossPerWave;

        if (zombieWave)
        {
            scriptControlaInterface.waveNumberTextOnScreen(currentWave, zombiesRemaining);
        } else if (bossWave)
        {
            scriptControlaInterface.waveNumberTextOnScreen(currentWave, bossRemaining);
        }


        CalculateNextSpawnTime();
    }

    void CalculateNextSpawnTime()
    {
        nextSpawnTime = Time.time + 0.2f;
    }

    public void ZombieDefeated()
    {
        zombiesRemaining--;
        scriptControlaInterface.waveNumberTextOnScreen(currentWave, zombiesRemaining);

        if (zombiesRemaining <= 0)
        {
            StartCoroutine(waitForOpenUpgradeScreen());
            increaseDifficultyNormalWave();
        }
    }

    public void bossDefeated()
    {
        bossRemaining--;

        scriptControlaInterface.waveNumberTextOnScreen(currentWave, bossRemaining);

        if (bossRemaining <= 0)
        {
            StartCoroutine(waitForOpenUpgradeScreen());
            increaseDifficultyBossWave();
        }
    }

    private IEnumerator waitForOpenUpgradeScreen()
    {
        // Aguarde 3 segundos.
        yield return new WaitForSeconds(1.5f);

        // Após a espera de 3 segundos, execute o pausamento do jogo.
        Time.timeScale = 0;
        scriptControlaInterface.optionsForUpgrade();
        canSpawn = true;
        StartNextWave();
    }



    private void checkSpawnRules(int numberPerWave, bool canSpawn, int numberRemaining, string monsterType)
    {
        if (numberPerWave <= 0)
        {
            canSpawn = false;
        }

        if (Time.time >= nextSpawnTime && numberRemaining > 0 && canSpawn)
        {
            if (monsterType == "ZOMBIE")
            {
                scriptZombieSpawner.spawnZombie();
                --zombiesPerWave;
            }

            if (monsterType == "BOSS")
            {
                if (bossPerWave == maxBossPerWave)
                {
                    ControlaAudio.instancia.PlayOneShot(somDeGeracao);
                }
                scriptBossSpawner.spawnBoss();
                --bossPerWave;
            }

            CalculateNextSpawnTime();

        }
    }

    private void increaseDifficultyNormalWave()
    {
        increaseZombieLife += 0.5f;
        increaseZombieDamage += 1;
        maxZombiePerWave += 5;
        scriptZombieSpawner.updateZombieStatus(increaseZombieLife, increaseZombieDamage);
    }

    private void increaseDifficultyBossWave()
    {
        increaseBossLife += 20f;
        increaseBossDamage += 10f;
        maxBossPerWave += 5;
        scriptBossSpawner.updaateBossStatus(increaseBossLife, increaseBossDamage);
    }


}
