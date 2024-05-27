using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class UnityMainThread : MonoBehaviour
{
    internal static UnityMainThread wkr;
    public static Queue<Action> jobs = new Queue<Action>();

    void Awake()
    {
        wkr = this;
    }

    void Update()
    {
        if (jobs.Count > 0)
        {
            //Debug.Log("Running " +jobs.Count);
            jobs.Dequeue().Invoke();
        }
    }

    internal void AddJob(Action newJob)
    {

        jobs.Enqueue(newJob);
    }
}
