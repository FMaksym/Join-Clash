using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetable
{
    void MoveToTarget(Transform target);
    void FightWithTarget(Transform target);
}
