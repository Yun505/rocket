using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float delayTime = 2f;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip crash;
    bool isTransitioning = false;
    bool isCollisionDisabled = false;
    AudioSource audioSource;

    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;
    void Start(){
        audioSource = GetComponent<AudioSource>();
    }
    void Update(){
        RespondToDebugKeys();
    }
    void RespondToDebugKeys(){
        if (Input.GetKeyDown(KeyCode.L)){
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C)){
            isCollisionDisabled = !isCollisionDisabled;
        }
    }
    void OnCollisionEnter(Collision other) {
        if (isTransitioning || isCollisionDisabled){return;}
        switch(other.gameObject.tag){
            case "Friendly":
                Debug.Log("Friendly");
                break;
            case "Finished":
                StartSucessSequence();
                isTransitioning = true;
                break;
            default:
                StartCrashSequence();
                isTransitioning = true;
                break;
        }
    }
    void StartCrashSequence(){
        audioSource.Stop();
        if (isTransitioning != true){
            crashParticles.Play();
            audioSource.PlayOneShot(crash);
        }
        gameObject.GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", delayTime);
    }
    void ReloadLevel(){
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    void LoadNextLevel(){
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings){
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
    void StartSucessSequence(){
        audioSource.Stop();
        if (isTransitioning != true){
            successParticles.Play();
            audioSource.PlayOneShot(success);
        }
        gameObject.GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", delayTime);
    }
}
