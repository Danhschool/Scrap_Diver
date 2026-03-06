using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RobotPassiveData", menuName = "Data/Robot Passive Data")]
public class RobotPassiveData : ScriptableObject
{
    public List<PassiveModule> passiveModules;
}