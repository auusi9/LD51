﻿using System;
using Code.Boxes;
using Code.Orders;
using TMPro;
using UnityEngine;

namespace Code.Senders
{
    public class Sender : MonoBehaviour
    {
        [SerializeField] private OrderInterface _orderInterface;
        [SerializeField] private Animator _orderCompleted;
        [SerializeField] private TextMeshProUGUI _orderCompletedText;
        [SerializeField] private Animator _orderUpdated;
        [SerializeField] private Animator _objectSentAnimator;
        [SerializeField] private ScoreSystem _scoreSystem;
        [SerializeField] private AudioSource _senderAudioSource;
        [SerializeField] private AudioClip _badBox;
        [SerializeField] private AudioClip _goodBox;

        private void Start()
        {
            _orderInterface.OrderUpdated += OrderUpdated;
            _orderInterface.InvalidBox += BadBox;
            _orderInterface.MainMenuBoxInvalidSent += BadBox;
            _orderInterface.MainMenuBoxSent += ValidBoxOnSender;
            _scoreSystem.LastBoxCompletedScore += OrderCompleted;
        }

        private void OnDestroy()
        {
            _orderInterface.OrderUpdated -= OrderUpdated;
            _orderInterface.InvalidBox -= BadBox;
            _orderInterface.MainMenuBoxInvalidSent -= BadBox;
            _orderInterface.MainMenuBoxSent -= ValidBoxOnSender;
            _scoreSystem.LastBoxCompletedScore -= OrderCompleted;
        }

        private void OrderCompleted(int score)
        {
            _orderCompletedText.text = score.ToString();
            if (_orderCompleted.gameObject.activeSelf)
            {
                _orderCompleted.Rebind();
                return;
            }
            _senderAudioSource.clip = _goodBox;
            _senderAudioSource.Play();
            _orderCompleted.gameObject.SetActive(true);
        }

        private void OrderUpdated(OrderUpdater order)
        {
            ValidBoxOnSender();
        }

        private void ValidBoxOnSender()
        {
            if (_orderUpdated.gameObject.activeSelf)
            {
                _orderUpdated.Rebind();
                return;
            }
            _senderAudioSource.clip = _goodBox;
            _senderAudioSource.Play();
            _orderUpdated.gameObject.SetActive(true);
        }

        public void Send(MovingBox movingBox)
        {
            //CalculateScore
            _orderInterface.BoxSent(movingBox.Box);
        }

        private void BadBox()
        {
            _senderAudioSource.clip = _badBox;
            _senderAudioSource.Play();

            if (_objectSentAnimator.gameObject.activeSelf)
            {
                _objectSentAnimator.Rebind();
                return;
            }

            _objectSentAnimator.gameObject.SetActive(true);
        }
    }
}