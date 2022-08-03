using Types.Classes;
using UnityEngine;

namespace Systems.General.EntitiesAnimator.Player
{
    public class AttackBehaviour : StateMachineBehaviour
    {
        private Types.Classes.Player m_player;
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            m_player ??= animator.gameObject.GetComponent<Types.Classes.Player>();
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            m_player.SetTrigger(PlayerStates.None);
        }

        /*public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
        }

        public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
        }

        public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
        }*/
    }
}