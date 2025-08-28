using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class AITargets{
    public string name;
    public Transform position;
    public bool state;

    AITargets(string name, Transform position,bool state){
        this.name = name;
        this.position = position;
        this.state = state;
    }
}
public class AI : MonoBehaviour
{
    [SerializeField] AITargets[] AITargets = new AITargets[2];
    // [SerializeField] Transform _destination;
    // [SerializeField] Transform _nextDestination;
    [SerializeField] float turnSpeed = 5f; // Add turn speed variable
    [SerializeField] public bool start = false;
    Animator animator;
    NavMeshAgent _navMeshAgent;
    [SerializeField] OVRCameraRig ovrCameraRig; // Reference to the OVRCameraRig
    AITargets startTarget;
    AITargets endTarget;
    void Start()
    {
        animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();

        if (_navMeshAgent == null){
            Debug.LogError("Nav Mesh not attached to " + gameObject.name);
        }        
        // startTarget = GetTargetByName("Start Target");
        // endTarget = GetTargetByName("End Target");
        // startTarget.state = true;
        // endTarget.state = false;
        // gameObject.SetActive(false);
    }

    private void SetDestination(Transform destination)
    {
        if (destination != null)
        {
            // Debug.Log("next" + destination.name + "");
            animator.SetBool("IsWalking", true);
            // _destination = destination;
            _navMeshAgent.SetDestination(destination.position);
        }
    }

    protected void RotateTowards(Vector3 to)
    {
        Quaternion lookRotation = Quaternion.LookRotation((to - transform.position).normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }

   void Update()
    {
        if(!start){ 
            return;
        }
        // SetDestination(startTarget.position);

        if(_navMeshAgent != null){
            animator.SetBool("IsSitting", true);
        }   

        return;
        
        // Check if the destination is set and the NavMeshAgent is valid
        if (startTarget.state && _navMeshAgent != null){
            // Update rotation towards the destination
            RotateTowards(startTarget.position.position);
            // Check if the agent has reached the destination
            if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance){
                // If the agent has reached the destination, stop moving and turn
                _navMeshAgent.isStopped = true;
                animator.SetBool("IsWalking", false);

                // Rotate 180 degrees
                Vector3 reverseDirection = transform.position - startTarget.position.position;
                Quaternion lookRotation = Quaternion.LookRotation(reverseDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 1f); // 1f means instantly rotate
                animator.SetBool("IsSitting", true);
                // Reset destination
                // _destination = null;
                // startTarget.state = false;
                // endTarget.state = true;
                // start = false;
            }
        }
    }
    public void StartAnimation(){
        gameObject.SetActive(true);
        start = true;
    }

    public void IsSitting(){
        _navMeshAgent.isStopped = false;
        animator.SetBool("IsSitting", true);
        animator.SetBool("IsSitTalking", false);

    }
    public void IsSitTalking(){
        _navMeshAgent.isStopped = false;
        animator.SetBool("IsSitTalking", true);

    }
    public void StopAnimation(){
        _navMeshAgent.isStopped = false;
        // SetDestination(endTarget.position);
        // _nextDestination = null; // Reset next destination
        start = true;

        // startTarget.state = true;
        // endTarget.state = false;
    }

    private AITargets GetTargetByName(string name)
    {
        foreach (AITargets target in AITargets)
        {
            if (target.name == name)
            {
                return target;
            }
        }
        return null;
    }


    // Public method to set the next destination
    public void SetNextDestination(Transform destination)
    {
        // _nextDestination = destination;
    }
}
