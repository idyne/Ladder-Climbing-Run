using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames;

public abstract class Collectible : MonoBehaviour
{
    protected static LadderClimbingRunLevel levelManager;
    [SerializeField] private Transform mesh = null;
    [SerializeField] private GameObject effectPrefab = null;

    void Update()
    {
        Animate();
    }
    private void Awake()
    {
        levelManager = (LadderClimbingRunLevel)LevelManager.Instance;
    }
    public abstract void GetCollected();
    protected void Animate()
    {
        transform.Rotate(Vector3.up, Time.deltaTime * 30);
    }
    protected void DestroyMesh()
    {
        mesh.LeanScale(Vector3.zero, 0.2f).setOnComplete(() => 
        { LeanTween.delayedCall(5f, () => { mesh.LeanScale(Vector3.one, 1f); }); });
        
        
        

    }
    protected void DestroySelf()
    {
        Destroy(GetComponent<Collider>());
        mesh.LeanScale(Vector3.zero, 0.2f).setOnComplete(() => { Destroy(gameObject); });
    }
    protected void ActivateEffect()
    {
        Destroy(Instantiate(effectPrefab, transform.position, effectPrefab.transform.rotation), 5);
    }
}
