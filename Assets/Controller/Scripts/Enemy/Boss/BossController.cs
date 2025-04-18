using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    public Wheel wheel;
    public CentralEye centralEye;
    public EnemyHealth selfHealth;

    [Header("Health Thresholds")]
    public float phase2Threshold = 75f;
    public float phase3Threshold = 50f;
    public float phase4Threshold = 25f;

    [Header("Attack Settings")]
    public float timeBetweenAttacks = 5f;

    private enum BossPhase { Phase1, Phase2, Phase3, Phase4 }
    private BossPhase currentPhase = BossPhase.Phase1;
    private bool isExecutingBehaviour = false;

    private bool isBossFightActive = false;
    public BoxCollider2D triggerArea;
    public GameObject BombEnemyPrefab;
    public Transform BoltPoint1;
    public Transform BoltPoint2;
    public Image bossHealthBar;

    void Start()
    {
        
    }

    void Update()
    {
        if (isBossFightActive)
        {
            UpdatePhase();
            bossHealthBar.fillAmount = Mathf.Clamp(selfHealth.health / selfHealth.maxHealth, 0, 1);
        }
    }

    public void StartBossFight()
    {
        if (isBossFightActive) return; // Prevent multiple starts
        isBossFightActive = true;

        // Start the behavior loop
        StartCoroutine(BehaviourLoop());

        Debug.Log("Boss fight started!");

        if (triggerArea != null) triggerArea.enabled = false;
    }

    private void UpdatePhase()
    {
        float healthPercentage = (selfHealth.health / selfHealth.maxHealth) * 100f;

        if (healthPercentage <= phase4Threshold) {currentPhase = BossPhase.Phase4;
                                                  wheel.StartChasing();}
        else if (healthPercentage <= phase3Threshold) currentPhase = BossPhase.Phase3;
        else if (healthPercentage <= phase2Threshold) currentPhase = BossPhase.Phase2;
        else currentPhase = BossPhase.Phase1;

        if (selfHealth.health <= 0)
        {
            var config = PlayerConfigManager.Instance.Config;
            config.currentRunExperience += 500;
            selfHealth.health = 0;
            isBossFightActive = false;
            UnityEngine.SceneManagement.SceneManager.LoadScene("EndScreen");
        }
    }

    private IEnumerator BehaviourLoop()
    {
        while (true)
        {
            if (wheel.IsFinalSequenceActive())
            {
                StopAllCoroutines();
                yield break;
            }

            if (!isExecutingBehaviour)
            {
                isExecutingBehaviour = true;

                switch (currentPhase)
                {
                    case BossPhase.Phase1:
                        yield return StartCoroutine(ExecuteRandomPhase1Behaviour());
                        break;
                    case BossPhase.Phase2:
                        yield return StartCoroutine(ExecuteRandomPhase2Behaviour());
                        break;
                    case BossPhase.Phase3:
                        yield return StartCoroutine(ExecuteRandomPhase3Behaviour());
                        break;
                    case BossPhase.Phase4:
                        yield return StartCoroutine(ExecuteRandomPhase4Behaviour());
                        break;
                }
                yield return new WaitForSeconds(timeBetweenAttacks);
            }
            yield return null;
        }
    }

    //PHASE 1 BEHAVIOURS
    private IEnumerator ExecuteRandomPhase1Behaviour()
    {
        int behaviour = Random.Range(0, 3);
        switch (behaviour)
        {
            case 0:
                yield return StartCoroutine(Phase1Behaviour1());
                break;
            case 1:
                yield return StartCoroutine(Phase1Behaviour2());
                break;
            case 2:
                yield return StartCoroutine(Phase1Behaviour3());
                break;
        }
    }

    private IEnumerator Phase1Behaviour1()
    {
        wheel.SetRotationSpeed(30f);
        wheel.SetAllEyesSpawnRate(0.5f);
        wheel.SetAllEyesFiring(Eye.FiringMode.Straight);
        yield return new WaitForSeconds(2f);
        wheel.StopAllEyesFiring();
        wheel.SetAllEyesFiring(Eye.FiringMode.Straight);
        wheel.SetAllEyesSpawnRate(0.25f);
        wheel.StartPlayerTracking();
        yield return new WaitForSeconds(8f);
        wheel.StopPlayerTracking();
        wheel.StopAllEyesFiring();
        wheel.SetAllEyesSpawnRate(2f);
        isExecutingBehaviour = false;
    }

    private IEnumerator Phase1Behaviour2()
    {
        wheel.SetRotationSpeed(60f);
        wheel.SetAllEyesSpawnRate(0.5f);
        wheel.SetAllEyesFiring(Eye.FiringMode.Straight);
        wheel.StartPlayerTracking();
        yield return new WaitForSeconds(10f);
        wheel.StopPlayerTracking();
        wheel.StopAllEyesFiring();
        isExecutingBehaviour = false;
    }

    private IEnumerator Phase1Behaviour3()
    {
        wheel.SetAllEyesFiring(Eye.FiringMode.Straight);
        wheel.SetAllEyesSpawnRate(0.5f);
        yield return new WaitForSeconds(5f);
        wheel.StopAllEyesFiring();
        wheel.SetAllEyesSpawnRate(2f);
        isExecutingBehaviour = false;
    }

    //PHASE 2 BEHAVIOURS
    private IEnumerator ExecuteRandomPhase2Behaviour()
    {
        int behaviour = Random.Range(0, 3);
        switch (behaviour)
        {
            case 0:
                yield return StartCoroutine(Phase2Behaviour1());
                break;
            case 1:
                yield return StartCoroutine(Phase2Behaviour2());
                break;
            case 2:
                yield return StartCoroutine(Phase2Behaviour3());
                break;
        }
    }
    private IEnumerator Phase2Behaviour1()
    {
        wheel.SetRotationSpeed(10f);
        wheel.SetAllEyesSpawnRate(0.25f);
        wheel.SetAllEyesFiring(Eye.FiringMode.Straight);
        wheel.StartPlayerTracking();
        centralEye.LaserAttack();
        yield return new WaitForSeconds(3f);
        wheel.StopPlayerTracking();
        wheel.StopAllEyesFiring();
        wheel.SetAllEyesSpawnRate(2f);
        isExecutingBehaviour = false;
    }

    private IEnumerator Phase2Behaviour2()
    {
        wheel.FireAllEyeLasers();
        wheel.StartPlayerTracking();
        yield return new WaitForSeconds(3f);
        wheel.SetAllEyesSpawnRate(0.25f);
        wheel.SetAllEyesFiring(Eye.FiringMode.Straight);
        yield return new WaitForSeconds(2f);
        wheel.StopAllEyesFiring();
        wheel.SetAllEyesSpawnRate(2f);
        isExecutingBehaviour = false;
    }
    private IEnumerator Phase2Behaviour3()
    {
        centralEye.BoltAttack(10);
        yield return new WaitForSeconds(2f);
        wheel.SetRotationSpeed(20f);
        wheel.SetAllEyesSpawnRate(1f);
        wheel.SetAllEyesFiring(Eye.FiringMode.Straight);
        wheel.StartPlayerTracking();
        yield return new WaitForSeconds(5f);
        wheel.StopPlayerTracking();
        wheel.StopAllEyesFiring();
        wheel.SetAllEyesSpawnRate(2f);
        isExecutingBehaviour = false;
    }

    //PHASE 3 BEHAVIOURS
    private IEnumerator ExecuteRandomPhase3Behaviour()
    {
        int behaviour = Random.Range(0, 3);
        switch (behaviour)
        {
            case 0:
                yield return StartCoroutine(Phase3Behaviour1());
                break;
            case 1:
                yield return StartCoroutine(Phase3Behaviour2());
                break;
            case 2:
                yield return StartCoroutine(Phase3Behaviour3());
                break;
        }
    }
    private IEnumerator Phase3Behaviour1()
    {
        wheel.SetRotationSpeed(30f);
        wheel.StartPlayerTracking();
        wheel.FireAllEyeLasers();
        centralEye.LaserAttack();
        yield return new WaitForSeconds(3f);
        wheel.StopAllEyesFiring();
        wheel.StopPlayerTracking();
        isExecutingBehaviour = false;
    }
    private IEnumerator Phase3Behaviour2()
    {
        wheel.StopPlayerTracking(); 
        wheel.FireAllEyeLasers();
        centralEye.BoltAttack(16);
        yield return new WaitForSeconds(3f); 
        isExecutingBehaviour = false;
    }
    private IEnumerator Phase3Behaviour3()
    {
        wheel.StopAllEyesFiring();
        wheel.SetRotationSpeed(50f);
        wheel.SetAllEyesSpawnRate(0.1f);
        wheel.SetAllEyesFiring(Eye.FiringMode.Straight);
        wheel.StartPlayerTracking();
        yield return new WaitForSeconds(3f);
        wheel.SetRotationSpeed(-50f);
        yield return new WaitForSeconds(3f);
        wheel.SetRotationSpeed(50f);
        yield return new WaitForSeconds(3f);
        isExecutingBehaviour = false;
    }

    // PHASE 4 BEHAVIOURS
    private IEnumerator ExecuteRandomPhase4Behaviour()
    {
        int behaviour = Random.Range(0, 3);
        switch (behaviour)
        {
            case 0:
                yield return StartCoroutine(Phase4Behaviour1());
                break;
            case 1:
                yield return StartCoroutine(Phase4Behaviour2());
                break;
            case 2:
                yield return StartCoroutine(Phase4Behaviour3());
                break;
        }
    }

    private IEnumerator Phase4Behaviour1()
    {
        wheel.SetRotationSpeed(10f);
        wheel.FireAllEyeLasers();
        wheel.StartChasing();
        yield return new WaitForSeconds(3f);
        wheel.StopPlayerTracking();
        isExecutingBehaviour = false;
    }

    private IEnumerator Phase4Behaviour2()
    {
        centralEye.BoltAttack(16);
        yield return new WaitForSeconds(2f);
        wheel.SetAllEyesSpawnRate(0.25f);
        wheel.SetAllEyesFiring(Eye.FiringMode.Straight);
        wheel.StartPlayerTracking();
        yield return new WaitForSeconds(5f);
        wheel.StopPlayerTracking();
        wheel.StopAllEyesFiring();
        isExecutingBehaviour = false;
    }
    private IEnumerator Phase4Behaviour3()
    {
        wheel.FireAllEyeLasers();
        centralEye.LaserAttack();
        yield return new WaitForSeconds(3f);
        isExecutingBehaviour = false;
    }

}
