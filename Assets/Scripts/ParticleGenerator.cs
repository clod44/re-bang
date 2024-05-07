using System;
using UnityEngine;

[Serializable]
public class ParticleData
{
    public string name;
    public ParticleSystem ps;
}

public class ParticleGenerator : MonoBehaviour
{
    public ParticleData[] particleSystems = new ParticleData[] { };

    public void Emit(string name, Vector3 pos, Quaternion rot)
    {
        // Find the ParticleData with the specified name
        ParticleData pData = Array.Find(particleSystems, pData => pData.name == name);

        // Check if ParticleData was found
        if (pData == null)
        {
            Debug.LogWarning("ParticleGenerator: emitting ps with the name : " + name + " : not found. Are you sure it's referenced with this name?");
            return;
        }

        // Instantiate the particle system
        ParticleSystem instantiatedPS = Instantiate(pData.ps, pos, rot);

        // Set its "emitting" true
        //this has to be done manually. you can not tick the "emit on awake" in the particlesystem. they will emit "in your face"
        instantiatedPS.Play();

        // Automatically destroy itself after the emission is complete
        Destroy(instantiatedPS.gameObject, instantiatedPS.main.duration / instantiatedPS.main.simulationSpeed + 1f);
    }
}
