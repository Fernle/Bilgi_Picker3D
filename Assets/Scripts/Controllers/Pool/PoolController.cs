using System;
using System.Collections.Generic;
using Data.UnityObjects;
using Data.ValueObjects;
using DG.Tweening;
using Extensions;
using Signals;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Controllers.Pool
{
    public class PoolController : MonoSingleton<PoolController>
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private List<DOTweenAnimation> tweens = new List<DOTweenAnimation>();
        [SerializeField] private TextMeshPro poolText;
        [SerializeField] private byte stageID;
        [SerializeField] private new Renderer renderer;

        #endregion

        #region Private Variables

        [ShowInInspector] private PoolData _data;
        [ShowInInspector] private byte _collectedCount;
        private int collectedCount = 0;

        #endregion

        #endregion

        private void Awake()
        {
            _data = GetPoolData();
            PlayerPrefs.SetInt("speedValue", 0);
        }

        private PoolData GetPoolData()
        {
            return Resources.Load<CD_Level>("Data/CD_Level")
                .Levels[(int) CoreGameSignals.Instance.onGetLevelValue?.Invoke()]
                .PoolList[stageID];
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onStageAreaSuccessful += OnActivateTweens;
            CoreGameSignals.Instance.onStageAreaSuccessful += OnChangeThePoolColor;
            CoreGameSignals.Instance.onStageAreaSuccessful += TextUpdate;
        }

        private void TextUpdate(int stageValue)
        {
            if (stageValue == stageID)
            {
                //collectedCount = collectedCount + (_collectedCount - _data.RequiredObjectCount);
                //collectedCount = collectedCount + (_collectedCount - _data.RequiredObjectCount);
                PlayerPrefs.SetInt("speedValue", PlayerPrefs.GetInt("speedValue", 0) + (_collectedCount - _data.RequiredObjectCount));
                LevelPanelController.Instance.SpeedText.text = $"x{PlayerPrefs.GetInt("speedValue", 0)}";
            }
        }

        private void OnActivateTweens(int stageValue)
        {
            if (stageValue != stageID) return;
            foreach (var tween in tweens)
            {
                tween.DOPlay();
            }
        }

        private void OnChangeThePoolColor(int stageValue)
        {
            if (stageValue == stageID)
                renderer.material.DOColor(new Color(0.1607842f, 0.6039216f, 0.1766218f), 1).SetEase(Ease.Linear);
        }

        private void UnSubscribeEvents()
        {
            CoreGameSignals.Instance.onStageAreaSuccessful -= OnActivateTweens;
            CoreGameSignals.Instance.onStageAreaSuccessful -= OnChangeThePoolColor;
            CoreGameSignals.Instance.onStageAreaSuccessful -= TextUpdate;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        private void Start()
        {
            SetRequiredAmountToText();
        }

        public bool TakeStageResult(byte stageValue)
        {
            if (stageValue == stageID)
            {
                
                return _collectedCount >= _data.RequiredObjectCount;
            }

            return false;
        }

        private void SetRequiredAmountToText()
        {
            poolText.text = $"0/{_data.RequiredObjectCount}";
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Collectable")) return;
            IncreaseCollectedCount();
            SetCollectedCountToText();
        }

        private void SetCollectedCountToText()
        {
            poolText.text = $"{_collectedCount}/{_data.RequiredObjectCount}";
        }

        private void IncreaseCollectedCount()
        {
            _collectedCount++;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Collectable")) return;
            DecreaseTheCollectedCount();
            SetCollectedCountToText();
        }

        private void DecreaseTheCollectedCount()
        {
            _collectedCount--;
        }
    }
}