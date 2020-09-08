using UnityEngine;

namespace CodingEssentials.Pattern
{
    /// <summary>
    ///     Specialized singleton for <see cref="MonoBehaviour" />s.
    ///     In Awake this class will test if there is already a singleton, if this is the case, the gameobject this instance is
    ///     atached to will be destroyed.
    ///     Therefore you should NOT write an own Awake function. Instead you can override the <see cref="AwakeSingleton" />
    ///     function,
    ///     which is only called if the singleton is valid.
    ///     <para>
    ///         <example>public class MySingleton : MonoSingleton&lt;MySingleton&gt;{ ... }</example>
    ///     </para>
    ///     For normal C# singletons, see <see cref="Singleton{T}" />.
    /// </summary>
    /// <typeparam name="T">Your singleton type.</typeparam>
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T _instance;

        /// <summary>
        ///     Returns the instance of this singleton.
        ///     If the singleton doesn't exist, a new <see cref="GameObject" /> will be created and the singleton will be attached
        ///     to it.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (!_instance)
                    _instance = new GameObject(typeof(T).Name).AddComponent<T>();
                return _instance;
            }
        }

        /// <summary>
        ///     Checks if there is already a singleton and if so, it destroys the gameobject it is attached to.
        /// </summary>
        public void Awake()
        {
            if (_instance && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = (T) this;
                AwakeSingleton();
            }
        }

        /// <summary>
        ///     Awake function which is only called if the singleton is valid.
        /// </summary>
        public virtual void AwakeSingleton()
        {
        }
    }
}