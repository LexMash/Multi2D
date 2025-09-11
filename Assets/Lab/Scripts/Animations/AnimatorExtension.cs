using Cysharp.Threading.Tasks;
using UnityEngine;

public static class AnimatorExtensions
{
    public static async UniTask PlayAndWait(this Animator animator, 
        int stateNameHash, int layerIndex = 0)
    {
        // Запускаем анимацию
        animator.Play(stateNameHash, layerIndex);
        
        // Ждем пока анимация начнется
        await UniTask.WaitUntil(() => 
            animator.GetCurrentAnimatorStateInfo(layerIndex).shortNameHash == stateNameHash);
        
        // Ждем пока анимация закончится
        await UniTask.WaitWhile(() => 
            animator.GetCurrentAnimatorStateInfo(layerIndex).normalizedTime < 1f);
    }
    
    public static async UniTask WaitForCurrentAnimation(this Animator animator, 
        int layerIndex = 0)
    {
        // Ждем окончания текущей анимации
        await UniTask.WaitWhile(() => 
            animator.GetCurrentAnimatorStateInfo(layerIndex).normalizedTime < 1f);
    }
}