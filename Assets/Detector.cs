using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Detector : MonoBehaviour
{
    public float SignalSpeed { get; private set; }
    
    [SerializeField] private List<Vector3> history;
    private Transmitter _transmitter;
    private List<Receiver> _allReceivers;
    private int _minItems;

    private void Start()
    {
        _allReceivers = FindObjectsOfType<Receiver>()
            .Select(x => x.gameObject.GetComponent<Receiver>())
            .ToList();
        if (_allReceivers.Count != 3) throw new ArgumentException("Number of receivers must me equal 3"); 
        _transmitter = FindObjectOfType<Transmitter>();
        SignalSpeed = _transmitter.WaveSpeed;
    }

    private void Update()
    {
        _minItems = _allReceivers.Select(x => x.recievedSignals.Count).ToList().Min();
        //if in history of transmitter's locations less points than in recievers, calculate new history
        //in other worlds, new point of transmitter
        if (history.Count >= _minItems) return;
        var vectors = new[]
        {
            GetClosestPointToTransmitter(_allReceivers[0], _allReceivers[1]),
            GetClosestPointToTransmitter(_allReceivers[1], _allReceivers[2]),
            GetClosestPointToTransmitter(_allReceivers[2], _allReceivers[0])
        };
        foreach (var vector3 in vectors)
        {
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = vector3;
            Destroy(cube, 1f);
        }

        var average = vectors.Aggregate(Vector3.zero, (acc, v) => acc + v) / vectors.Length;
        history.Add(average);
        var item = GameObject.CreatePrimitive(PrimitiveType.Cube);
        item.transform.position = average;
        Destroy(item, 3f);
    }

    private Vector3 GetClosestPointToTransmitter(Receiver one, Receiver two)
    {
        var distance = Vector3.Distance(one.transform.position, two.transform.position);
        var r1 = one.recievedSignals.Last() * SignalSpeed / 2;
        var r2 = two.recievedSignals.Last() * SignalSpeed / 2;
        var r1Square = r1 * r1;
        var r2Square = r2 * r2;
        var distanceSquare = distance * distance;
        var radical = (Math.Pow(distance + r1, 2) - r2Square) * (r2Square - Math.Pow(distance - r1, 2));

        if (radical >= 0)
        {
            var transform1 = one.transform.position;
            var transform2 = two.transform.position;

            var basis = transform1 + new Vector3(transform2.x - transform1.x, transform2.y - transform1.y, 0f) *
                (distance * distance + r1Square - r2Square) / (2 * distanceSquare);
            var addition = new Vector3(transform1.y - transform2.y, transform2.x - transform1.x, 0f)
                * (float) Math.Sqrt(radical) / (2 * distanceSquare);

            var firstPoint = basis + addition;
            var secondPoint = basis - addition;

            var thirdReceiver = _allReceivers.Last(x => x != one && x != two);
            var thirdRecPos = thirdReceiver.transform.position;
            var dist1 = Vector3.Distance(firstPoint, thirdRecPos);
            var dist2 = Vector3.Distance(secondPoint, thirdRecPos);

            var r3 = thirdReceiver.recievedSignals.Last() * SignalSpeed / 2;

            return Math.Abs(dist1 - r3) > Math.Abs(dist2 - r3) ? secondPoint : firstPoint;
        }

        var point = one.transform.position * ((r1 - r2 + distance) / (2 * distance))
                    + two.transform.position * ((r2 - r1 + distance) / (2 * distance));

        return point;
    }
}