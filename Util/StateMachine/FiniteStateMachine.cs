using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;



    /// <summary>
    /// generic finite state machine
    /// inherit from this to make a statemachine.
    /// states that belong to the statemachine should inherit from the inner class
    /// all states belonging to this statemachine are automatically instantiated in the constructor
    /// via reflection (StateInstancesFromTypes) and added to a dictionary.
    /// typeof() the states are then used to find the correct state instance from the dictionary
    /// </summary>
    /// <typeparam name="Derived"></typeparam>
    public abstract class FiniteStateMachine<Derived> where Derived : FiniteStateMachine<Derived>
    {
        public abstract class State
        {
            public Derived stateMachine;

            public State(Derived stateMachine)
            {
                this.stateMachine = stateMachine;
            }

            public abstract void Prepare();
            public abstract void Execute();
            public abstract void End();
        }

        private Dictionary<Type, State> allStates = null;
        private State currentState = null;
        private Type currentStateKey = null;


        private void StateInstancesFromTypes(Dictionary<Type, State> dic)
        {


            Type baseState = typeof(FiniteStateMachine<Derived>.State);
            IEnumerable<Type> stateTypes = Assembly.GetAssembly(baseState).GetTypes()
                .Where((myType) => 
                {
                    return !myType.IsAbstract && myType.IsSubclassOf(baseState);
                });
            
            foreach (var state in stateTypes)
            {
                dic.Add(state, (State)Activator.CreateInstance(state, (Derived)this));
            }
        }

        public FiniteStateMachine(Type startingState)
        {            
            allStates = new Dictionary<Type, State>();
            StateInstancesFromTypes(allStates);
            ChangeState(startingState);
        }

        public State GetState(Type stateKey)
        {
            return allStates[stateKey];
        }

        public void ChangeState(Type key)
        {
            if (currentState != null)
            {
                currentState.End();
            }
            
            currentState = allStates[key];
            currentStateKey = key;
            currentState.Prepare();
        }

        public void Execute()
        {
            currentState.Execute();
        }
        public Type KeyOfCurrentState()
        {
            return currentStateKey;
        }

        public bool IsInState(Type key)
        {
            return key == currentStateKey;
        }
    }

