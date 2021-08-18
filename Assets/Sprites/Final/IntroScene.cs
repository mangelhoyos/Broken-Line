using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScene : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SceneManager.LoadScene("MainMenu");
    }

}
