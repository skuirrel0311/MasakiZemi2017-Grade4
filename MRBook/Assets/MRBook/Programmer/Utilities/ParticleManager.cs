using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KKUtilities;

public class ParticleManager : BaseManager<ParticleManager>
{
    [SerializeField]
    List<GameObject> particleList = new List<GameObject>();

    [SerializeField]
    List<ParticleSystem> particleInstanceList = new List<ParticleSystem>();

    /// <summary>
    /// 自動で消えないやつ
    /// </summary>
    [SerializeField]
    List<GameObject> remainParticleList = new List<GameObject>();

    Dictionary<string, GameObject> remainParticleDictionary = new Dictionary<string, GameObject>();

    protected override void Awake()
    {
        base.Awake();
        //パーティクルのインスタンス作成
        for(int i = 0;i< particleList.Count;i++)
        {
            ParticleSystem particle = Instantiate(particleList[i], transform).transform.GetChild(0).GetComponent<ParticleSystem>();

            particleInstanceList.Add(particle);
        }

        for(int i = 0;i< remainParticleList.Count;i++)
        {
            remainParticleDictionary.Add(remainParticleList[i].name, remainParticleList[i]);
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

    public void Play(string particleName, Vector3 position, Quaternion rotation, float duration)
    {
        GameObject particle;
        if (!remainParticleDictionary.TryGetValue(particleName, out particle)) return;

        particle.transform.parent.position = position;
        particle.transform.parent.rotation = rotation;

        particle.SetActive(true);

        Utilities.Delay(duration, () => particle.SetActive(false), this);
    }

    public void Play(string particleName, Vector3 position, float duration)
    {
        GameObject particle;
        if (!remainParticleDictionary.TryGetValue(particleName, out particle)) return;

        particle.transform.parent.position = position;

        particle.SetActive(true);

        Utilities.Delay(duration, () => particle.SetActive(false), this);
    }

    public void Play(string particleName, float duration)
    {
        GameObject particle;
        if (!remainParticleDictionary.TryGetValue(particleName, out particle)) return;

        particle.SetActive(true);

        Utilities.Delay(duration, () => particle.SetActive(false), this);
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
