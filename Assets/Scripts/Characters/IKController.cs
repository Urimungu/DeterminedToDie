using UnityEngine;

public class IKController : MonoBehaviour{

    private Animator _animator;
    private CharacterFunctions _character;

    private void Start(){
        _character = transform.parent.GetComponent<CharacterFunctions>();
        _animator = GetComponent<Animator>();
    }

    void OnAnimatorIK(int layerIndex){
        if (_character.WeaponHandle != null){
            //Right Hand
            _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
            _animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);

            _animator.SetIKPosition(AvatarIKGoal.RightHand, _character.WeaponHandle.position);
            _animator.SetIKRotation(AvatarIKGoal.RightHand, _character.WeaponHandle.rotation);


            //Head and body
            _animator.SetLookAtWeight(0.2f, 1, 1);
            _animator.SetLookAtPosition(_character.WeaponHandle.position);
        }else {
            _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
            _animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
            _animator.SetLookAtWeight(0, 0, 0);
        }
    }
}
