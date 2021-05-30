using System.Collections.Generic;
using UnityEngine;

public interface IShootDirectionProvider
{
    Vector3 ShootDirection { get; }
    List<Vector3> AllShootDirections { get; }
}