using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float thrust = 1000f;
    [SerializeField] float rotationThrust = 2f;
    [SerializeField] AudioClip mainEngine;
    Rigidbody rb;
    AudioSource audioSource;
    bool isAlive;
    [SerializeField] ParticleSystem LeftSideThrusterParticles;
    [SerializeField] ParticleSystem RocketJetParticles;
    [SerializeField] ParticleSystem RightSideThrusterParticles;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    } 
    void ProcessThrust(){
       if (Input.GetKey(KeyCode.Space))
        {
            StartThrusting();
        }
        else
            StopThrusting();
    }
    void StopThrusting()
    {
        audioSource.Stop();
        RocketJetParticles.Stop();
    }

    void StartThrusting()
    {
        rb.AddRelativeForce(Vector3.up * thrust * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        if (!RocketJetParticles.isPlaying)
        {
            RocketJetParticles.Play();
        }
    }

    void ProcessRotation(){
        if (Input.GetKey(KeyCode.A))
        {
            RightThrusting();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            LeftThrusting();
        }
        else
        {
            StopThrustingBoth();
        }

    }

    private void StopThrustingBoth()
    {
        RightSideThrusterParticles.Stop();
        LeftSideThrusterParticles.Stop();
    }

    private void LeftThrusting()
    {
        ApplyRotation(-rotationThrust);
        if (!LeftSideThrusterParticles.isPlaying)
        {
            LeftSideThrusterParticles.Play();
        }
    }

    private void RightThrusting()
    {
        ApplyRotation(rotationThrust);
        if (!RightSideThrusterParticles.isPlaying)
        {
            RightSideThrusterParticles.Play();
        }
    }

    void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true; //freezing rotation to manually rotate
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rb.freezeRotation = false; //unfreezing rotation 
    }
}
