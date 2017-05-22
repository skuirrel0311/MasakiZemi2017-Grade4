using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : BaseManager<ParticleManager>
{
    [SerializeField]
    List<GameObject> particleList = new List<GameObject>();

    [SerializeField]
    List<ParticleSystem> particleInstanceList = new List<ParticleSystem>();

    protected override void Awake()
    {
        base.Awake();
        //パーティクルのインスタンス作成
        for(int i = 0;i< particleList.Count;i++)
        {
            ParticleSystem particle = Instantiate(particleList[i], transform).transform.GetChild(0).GetComponent<ParticleSystem>();

            particleInstanceList.Add(particle);
        }
    }
    
    public void Play(string particleName, Vector3 position, Quaternion rotation)
    {
        ParticleSystem particle = GetParticle(particleName);
        if (particle == null) return;

        particle.transform.parent.position = position;
        particle.transform.parent.rotation = rotation;

        particle.Play(true);
    }

    public void Play(string particleName, Vector3 position)
    {
        Play(particleName, position, Quaternion.identity);
    }

    ParticleSystem GetParticle(string particleName)
    {
        ParticleSystem particle = particleInstanceList.Find(n => n.name == particleName);

        if (particle != null) return particle;

        GameObject particlePrefab = Resources.Load<GameObject>(particleName);

        if (particlePrefab != null)
        {
            particle = Instantiate(particlePrefab, transform).transform.GetChild(0).GetComponent<ParticleSystem>();
            particle.name = particleName;
            particleInstanceList.Add(particle);
            return particle;
        }

        particlePrefab = Resources.Load<GameObject>("Particles/" + particleName);

        if (particlePrefab != null)
        {
            particle = Instantiate(particlePrefab, transform).transform.GetChild(0).GetComponent<ParticleSystem>();
            particle.name = particleName;
            particleInstanceList.Add(particle);
            return particle;
        }

        return null;
    }
}
