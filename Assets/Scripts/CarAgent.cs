using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class CarAgent : Agent
{
    [SerializeField] private Transform targetTransform;

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * 1f; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Goal>(out Goal goal))
        {
            SetReward(1f);
        }
        if (other.TryGetComponent<Crash>(out Crash crash))
        {
            SetReward(-1f);
        }
        EndEpisode();
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(70, 0, -70);
        transform.localRotation = new Quaternion(0, 0, 0, 0);
    }
}
