using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    [Header("Character Prefabs")]
    public GameObject malePrefab;
    public GameObject femalePrefab;

    [Header("Spawn Points")]
    public Transform maleSpawnPoint;
    public Transform femaleSpawnPoint;

    private GameObject currentCharacter;

    void Start()
    {
        SpawnFemale(); // default on start
    }

    public void SpawnMale()
    {
        DestroyCurrentCharacter();

        if (malePrefab != null && maleSpawnPoint != null)
        {
            currentCharacter = Instantiate(malePrefab, maleSpawnPoint.position, maleSpawnPoint.rotation);
        }
        else
        {
            Debug.LogWarning("Male prefab or spawn point is missing!");
        }
    }

    public void SpawnFemale()
    {
        DestroyCurrentCharacter();

        if (femalePrefab != null && femaleSpawnPoint != null)
        {
            currentCharacter = Instantiate(femalePrefab, femaleSpawnPoint.position, femaleSpawnPoint.rotation);
        }
        else
        {
            Debug.LogWarning("Female prefab or spawn point is missing!");
        }
    }

    private void DestroyCurrentCharacter()
    {
        if (currentCharacter != null)
        {
            Destroy(currentCharacter);
        }
    }
}
