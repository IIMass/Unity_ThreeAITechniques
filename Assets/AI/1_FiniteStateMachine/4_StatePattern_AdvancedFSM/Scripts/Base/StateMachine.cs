﻿using System.Collections;
using UnityEngine;

namespace StatePattern_FSM
{
    [DisallowMultipleComponent]
    public abstract class StateMachine : MonoBehaviour
    {
        [Header("State Machine Initial State")]
        [SerializeField] protected State initialState;

        [Header("State Machine Current Status (DEBUG)")]
        [SerializeField] protected SubStateMachine currentSubStateMachine;
        [SerializeField] protected State currentState;
        protected bool active => fsmCoroutine != null;

        [Header("State Machine Update Coroutine")]
        protected Coroutine fsmCoroutine;


        #region State Machine Methods
        #region Initialization / Exit Methods
        // Start up FSM, setting it with an initial state
        public virtual void InitializeFSM(bool reset)
        {
            if (fsmCoroutine != null)
            {
                Debug.LogError($"FSM is already running! Can't initialize again.");
                return;
            }

            if (initialState == null)
            {
                Debug.LogError($"No Initial State has been set! Can't initialize.");
                return;
            }

            if (reset) ResetValueFSM();

            SetState(initialState);
            fsmCoroutine = StartCoroutine(FSMUpdate());
        }

        // Start up FSM, setting it with a custom state
        public virtual void InitializeCustomStateFSM(State customState, bool reset)
        {
            if (fsmCoroutine != null)
            {
                Debug.LogError($"FSM is already running! Can't initialize again.");
                return;
            }

            if (customState == null)
            {
                Debug.LogError($"No Custom State detected! Can't initialize.");
                return;
            }

            if (reset) ResetValueFSM();

            SetState(customState);
            fsmCoroutine = StartCoroutine(FSMUpdate());
        }


        // Clean / Reset variable values if needed
        public virtual void ResetValueFSM() { }


        // Exit FSM, exit current state and stop Update Coroutine
        public virtual void ExitFSM()
        {
            if (fsmCoroutine == null)
            {
                Debug.LogError($"FSM is not running! Can't exit if it's not active.");
                return;
            }

            currentState?.Exit();

            currentSubStateMachine = null;
            currentState = null;

            StopCoroutine(fsmCoroutine);
            fsmCoroutine = null;
        }
        #endregion

        #region Update Coroutine
        // Called once per frame. Can change tick time
        protected IEnumerator FSMUpdate()
        {
            while (true)
            {
                if (SubStateMachineTransitionChecker()) continue;
                if (StateTransitionChecker()) continue;

                yield return null;
            }
        }

        protected bool SubStateMachineTransitionChecker()
        {
            State subStateMachineToTransition = currentSubStateMachine?.CheckTransitions();
            if (subStateMachineToTransition != null)
            {
                SetState(subStateMachineToTransition);
                return true;
            }
            else
            {
                currentSubStateMachine?.Execute();
                return false;
            }
        }
        protected bool StateTransitionChecker()
        {
            State stateToTransition = currentState?.CheckTransitions();
            if (stateToTransition != null)
            {
                SetState(stateToTransition);
                return true;
            }
            else
            {
                currentState?.Execute();
                return false;
            }
        }

        protected void NullStateChecker()
        {
            if (currentState == null)
            {
                Debug.LogError("No Current State active! Exiting FSM.");
                ExitFSM();
            }
        }
        #endregion


        #region Get / Set State Methods
        public State GetState()
        {
            return currentState;
        }
        protected virtual void SetState(State newState)
        {
            if (newState != null)
            {
                if (newState is SubStateMachine)
                {
                    SetSubStateMachine(newState as SubStateMachine, true);
                }
                else
                {
                    currentState?.Exit();

                    if (currentSubStateMachine != newState.GetParentSubStateMachine())
                    {
                        SetSubStateMachine(newState.GetParentSubStateMachine(), false);
                    }

                    currentState = newState;
                    currentState?.Enter();
                }
            }
            else
            {
                Debug.LogError($"New State is null! Returning to Initial State.");

                SetState(initialState);
            }
        }

        public SubStateMachine GetSubStateMachine()
        {
            return currentSubStateMachine;
        }
        protected virtual void SetSubStateMachine(SubStateMachine ssm, bool entryToStateMachine)
        {
            currentSubStateMachine?.Exit();
            currentSubStateMachine = ssm;
            currentSubStateMachine?.Enter();

            if (entryToStateMachine) SetState(currentSubStateMachine.entryState);
        }
        #endregion
        #endregion
    }
}