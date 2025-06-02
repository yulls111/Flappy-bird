using UnityEngine;
using System.Collections.Generic;
public class Spowner : MonoBehaviour
{
    public GameObject prefab;

    public float spawnRate = 1f;

    public float minHeight = -1f;

    public float maxHeight = 1f;

    private List<GameObject> activePipes = new List<GameObject>();


    private void OnEnable()
    {
        InvokeRepeating(nameof(Spawn), spawnRate, spawnRate);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(Spawn));
    }
    private void Spawn()
    {
        GameObject newPipe = Instantiate(prefab, transform.position, Quaternion.identity);
        newPipe.transform.position += Vector3.up * Random.Range(minHeight, maxHeight);
        activePipes.Add(newPipe);

        // Добавляем автоматическое удаление из списка при уничтожении трубы
        PipeDestructionNotifier destructionNotifier = newPipe.AddComponent<PipeDestructionNotifier>();
        destructionNotifier.Initialize(this);
    }
    public void ClearAllPipes()
    {
        // Удаляем в обратном порядке, чтобы избежать проблем с индексами
        for (int i = activePipes.Count - 1; i >= 0; i--)
        {
            if (activePipes[i] != null)
            {
                Destroy(activePipes[i]);
            }
        }
        activePipes.Clear();
    }
    public void RemovePipeFromList(GameObject pipe)
    {
        activePipes.Remove(pipe);
    }

    public class PipeDestructionNotifier : MonoBehaviour
    {
        private Spowner spawner;

        public void Initialize(Spowner owner)
        {
            spawner = owner;
        }

        private void OnDestroy()
        {
            if (spawner != null)
            {
                spawner.RemovePipeFromList(gameObject);
            }
        }


    }
}
