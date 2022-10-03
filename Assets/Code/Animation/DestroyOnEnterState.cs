using System.Security.Cryptography;
using UnityEngine;

namespace Animation
{
    public class DestroyOnEnterState : StateMachineBehaviour 
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Destroy(animator.gameObject);
        }
    }
}