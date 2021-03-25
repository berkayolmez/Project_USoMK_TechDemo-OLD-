using System.Collections;

namespace project_WAST
{
    public abstract class State
    {
        public virtual IEnumerator Start()
        {
            yield break;
        }

        public virtual IEnumerator Idle()
        {
            yield break;
        }
        public virtual IEnumerator Walk()
        {
            yield break;
        }

        public virtual IEnumerator Run()
        {
            yield break;
        }


    }
}