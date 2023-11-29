using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoMono
{
    public string Message;
    
    public void FakeUpdate()
    {
        Debug.Log(Message);
    }
}
