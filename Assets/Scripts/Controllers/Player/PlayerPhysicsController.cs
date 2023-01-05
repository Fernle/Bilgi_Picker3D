using Controllers.Pool;
using DG.Tweening;
using Managers;
using Signals;
using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;

namespace Controllers.Player
{
    public class PlayerPhysicsController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private PlayerManager manager;
        [SerializeField] private new Collider collider;
        [SerializeField] private new Rigidbody rigidbody;

        #endregion

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("StageArea"))
            {
                manager.ForceCommand.Execute();
                CoreGameSignals.Instance.onStageAreaEntered?.Invoke();
                InputSignals.Instance.onDisableInput?.Invoke();
                DOVirtual.DelayedCall(3, () =>
                {
                    var result = other.transform.parent.GetComponentInChildren<PoolController>()
                        .TakeStageResult(manager.StageValue);
                    if (result)
                    {
                        CoreGameSignals.Instance.onStageAreaSuccessful?.Invoke(manager.StageValue);
                        InputSignals.Instance.onEnableInput?.Invoke();
                    }
                    else CoreGameSignals.Instance.onLevelFailed?.Invoke();
                });
                return;
            }

            if (other.CompareTag("Finish"))
            {
                CoreGameSignals.Instance.onFinishAreaEntered?.Invoke();
                InputSignals.Instance.onDisableInput?.Invoke();
                CoreGameSignals.Instance.onLevelSuccessful?.Invoke();
                return;
            }

            if (other.CompareTag("MiniGame"))
            {
                //Write Mini Game Conditions
                StartCoroutine(MiniGameEnd());
            }
        }

        IEnumerator MiniGameEnd()
        {
            //calculated by maks value as 30 and final tour takes 10 sec.
            float value = PlayerPrefs.GetInt("speedValue", 0);

            yield return new WaitForSecondsRealtime(value/3f);

            InputSignals.Instance.onDisableInput?.Invoke();
            CoreGameSignals.Instance.onFinishAreaEntered?.Invoke();

            Debug.Log(gameObject.transform.position.z);

            float valueZ = gameObject.transform.position.z;

            if (valueZ >= 355 && valueZ <= 364)
                PlayerPrefs.SetInt("rewardImage", 0);
            else if (valueZ >= 365 && valueZ <= 374)
                PlayerPrefs.SetInt("rewardImage", 1);
            else if (valueZ >= 375 && valueZ <= 384)
                PlayerPrefs.SetInt("rewardImage", 2);
            else if (valueZ >= 385 && valueZ <= 394)
                PlayerPrefs.SetInt("rewardImage", 3);
            else if (valueZ >= 395 && valueZ <= 404)
                PlayerPrefs.SetInt("rewardImage", 4);
            else if (valueZ >= 405 && valueZ <= 414)
                PlayerPrefs.SetInt("rewardImage", 5);
            else if (valueZ >= 415 && valueZ <= 424)
                PlayerPrefs.SetInt("rewardImage", 6);
            else if (valueZ >= 425 && valueZ <= 434)
                PlayerPrefs.SetInt("rewardImage", 7);
            else if (valueZ >= 435 && valueZ <= 444)
                PlayerPrefs.SetInt("rewardImage", 8);
            else if (valueZ >= 445 && valueZ <= 454)
                PlayerPrefs.SetInt("rewardImage", 9);
            else if(valueZ < 355)
                PlayerPrefs.SetInt("rewardImage", 10);

            yield return new WaitForSecondsRealtime(3f);
            
            CoreGameSignals.Instance.onLevelSuccessful?.Invoke();
            UIEventSubscriber.Instance.RewardImage.sprite = UIEventSubscriber.Instance.RewardSprites[PlayerPrefs.GetInt("rewardImage", 0)];
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            var transform1 = manager.transform;
            var position = transform1.position;
            Gizmos.DrawSphere(new Vector3(position.x, position.y - 1.2f, position.z + 1f), 1.65f);
        }

        internal void OnReset()
        {
        }
    }
}