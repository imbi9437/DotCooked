using System.Collections;
using System.Collections.Generic;
using Interface;
using UnityEngine;

public interface IGrabAble
{
    public IGrabber Grabber { get; set; }
    public void Grab(IGrabber grabber);
    public void Release(IGrabber grabber);
}
