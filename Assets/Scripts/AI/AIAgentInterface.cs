using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface AIAgentInterface
{
    public void NoticePlayer();
    public bool GetNoticePlayer();
    public bool HasDied();
    public void Die(Vector3 direction);
    public void Die(Vector3 grenadePos, float grenadeRad);
}
