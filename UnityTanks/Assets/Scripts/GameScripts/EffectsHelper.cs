using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsHelper : MonoBehaviour
{
    public static EffectsHelper Instance;

    public ParticleSystem explosionEffect;
    public ParticleSystem exploison1Effect;

    void Awake()
    {
        // регистрация синглтона
        if (Instance != null)
        {
            Debug.LogError("several initiations of EffectsHelper!");
        }

        Instance = this;
    }
    public void Explosion1(Vector3 position)
    {
        instantiate(exploison1Effect, position);
    }
    // Создать взрыв в данной точке
    public void Explosion(Vector3 position)
    {
        instantiate(explosionEffect, position);
    }

    // Создание экземпляра системы частиц из префаба
    private ParticleSystem instantiate(ParticleSystem prefab, Vector3 position)
    {
        ParticleSystem newParticleSystem = Instantiate(
          prefab,
          position,
          Quaternion.identity
        ) as ParticleSystem;

        Destroy(
          newParticleSystem.gameObject,
          newParticleSystem.startLifetime
        );

        return newParticleSystem;
    }
}
