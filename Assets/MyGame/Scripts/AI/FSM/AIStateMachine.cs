public class AIStateMachine 
{
    public AIState[] states;
    public AIAgent agent;
    public AIStateID currentState;

    public AIStateMachine(AIAgent agent)
    {
        this.agent = agent;
        int numStates = System.Enum.GetNames(typeof(AIStateID)).Length;
        states = new AIState[numStates];
    }

    public void RegisterState(AIState state)
    {
        int index = (int)state.GetID();
        states[index] = state;
    }

    public AIState GetState(AIStateID stateID)
    {
        int index = (int)stateID;
        return states[index];
    }

    public void Update()
    {
        GetState(currentState)?.Update(agent);
    }

    public void ChangeState(AIStateID newSate)
    {
        GetState(currentState)?.Exit(agent);
        currentState = newSate;
        GetState(currentState)?.Enter(agent);
    }
}
