﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Simple_FSM
{
    public class SimpleFSM_UI : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private SimpleFSM fsm;
        [SerializeField] private Animator spriteAnimator;

        [Space(10)]

        [Header("UI FSM State Text")]
        [SerializeField] private Text stateText;

        [Space(10)]

        [Header("UI Input Components")]
        [SerializeField] private Dropdown distanceDropdown;

        [Space(5)]

        [SerializeField] private InputField idlePauseTimeInputField;
        [SerializeField] private Text remainingIdleTimeTitleText;
        [SerializeField] private Text remainingIdleTimeText;

        [Space(5)]

        [SerializeField] private Toggle reachedPatrolPointToggle;

        [Space(5)]

        [SerializeField] private Toggle canSeePlayerToggle;
        [SerializeField] private Toggle playerIsDeadToggle;

        [Space(10)]

        [Header("Sprite Animator Triggers")]
        [SerializeField] private string idleTrigger;
        [SerializeField] private string patrolTrigger;
        [SerializeField] private string chaseTrigger;
        [SerializeField] private string attackTrigger;

        [Header("Timer Coroutine")]
        private Coroutine timerCoroutine;


        private void OnEnable()
        {
            fsm.OnStateChange += UpdateUI;
        }
        private void OnDisable()
        {
            fsm.OnStateChange -= UpdateUI;
        }


        // Start is called before the first frame update
        void Start()
        {
            UpdateUI();
        }


        private void UpdateUI()
        {
            switch (fsm.GetRobotState())
            {
                case SimpleFSM.RobotStates.Idle:
                    spriteAnimator.SetTrigger(idleTrigger);

                    StartTimer();

                    break;

                case SimpleFSM.RobotStates.Patrol:
                    spriteAnimator.SetTrigger(patrolTrigger);

                    StopTimer();

                    break;

                case SimpleFSM.RobotStates.Chase:
                    spriteAnimator.SetTrigger(chaseTrigger);

                    StopTimer();

                    break;

                case SimpleFSM.RobotStates.Attack:
                    spriteAnimator.SetTrigger(attackTrigger);

                    StopTimer();

                    break;
            }

            stateText.text = fsm.GetRobotState().ToString();

            distanceDropdown.SetValueWithoutNotify((int)fsm.GetRobotToPlayerDistance());

            idlePauseTimeInputField.text = fsm.GetIdlePauseTime().ToString();

            reachedPatrolPointToggle.SetIsOnWithoutNotify(fsm.GetPatrolReachedPoint());

            canSeePlayerToggle.SetIsOnWithoutNotify(fsm.GetPlayerVisibility());
            playerIsDeadToggle.SetIsOnWithoutNotify(fsm.GetPlayerStatus());
        }

        private void StartTimer()
        {
            if (timerCoroutine != null)
            {
                StopCoroutine(timerCoroutine);
            }
            timerCoroutine = StartCoroutine(TimerCountdown());
        }
        private void StopTimer()
        {
            remainingIdleTimeTitleText.enabled = false;
            remainingIdleTimeText.enabled = false;

            if (timerCoroutine != null)
            {
                StopCoroutine(timerCoroutine);
                timerCoroutine = null;
            }
        }
        private IEnumerator TimerCountdown()
        {
            remainingIdleTimeTitleText.enabled = true;
            remainingIdleTimeText.enabled = true;

            while (fsm.GetIdlePauseTime() - fsm.GetIdlePauseTimer() > Mathf.Epsilon)
            {
                remainingIdleTimeText.text = (fsm.GetIdlePauseTime() - fsm.GetIdlePauseTimer()).ToString("0.00");
                yield return null;
            }

            StopTimer();

            yield break;
        }


        public void SetRobotToPlayerDistance(int newDistance)
        {
            fsm.SetRobotToPlayerDistance((SimpleFSM.Distance)newDistance);
        }

        public void SetIdlePauseTime(string newTime)
        {
            if (int.Parse(newTime) < 0f)
            {
                idlePauseTimeInputField.SetTextWithoutNotify(fsm.GetIdlePauseTime().ToString());
                return;
            }

            fsm.SetIdlePauseTime(int.Parse(newTime));
        }

        public void SetPatrolPointCheck(bool reachedPoint)
        {
            fsm.SetPatrolReachedPoint(reachedPatrolPointToggle);
        }

        public void SetPlayerVisibility(bool visible)
        {
            fsm.SetPlayerVisibility(visible);
        }
        public void SetPlayerStatus(bool reachedPoint)
        {
            fsm.SetPlayerStatus(reachedPoint);
        }
    }
}