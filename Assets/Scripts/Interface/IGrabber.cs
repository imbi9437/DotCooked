using UnityEngine;

namespace Interface
{
    public interface IGrabber
    {
        public Transform GrabPivot { get; set; }
        public IGrabAble GrabAble { get; set; }
        public Vector2 ReleaseVector { get; set; }

        public void GrabObject(IGrabAble grabAble);
        public void ReleaseObject();
    }
}
