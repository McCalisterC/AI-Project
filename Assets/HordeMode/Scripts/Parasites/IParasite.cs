using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IParasite
{
    public void Activate(int level);
    public void Deactivate(int level);
}
