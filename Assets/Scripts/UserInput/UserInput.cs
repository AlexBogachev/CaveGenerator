using System;
using UniRx;
using UnityEngine;

public class UserInput
{
    public static IObservable<Vector3> OnClick = Observable.EveryUpdate()
        .Where(x => Input.GetMouseButtonUp(0))
        .Select(x => Input.mousePosition);

}
