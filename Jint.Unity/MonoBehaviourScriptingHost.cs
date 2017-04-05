using System;
using Jint.Native;
using UnityEngine;

namespace Jint.Unity
{
    /// <summary>
    /// This object is able to run a JS script as if it were a MonoBehaviour,
    /// calling lifecycle methods.
    /// </summary>
    public class MonoBehaviourScriptingHost : MonoBehaviour
    {
        /// <summary>
        /// A reference to the script to execute.
        /// </summary>
        public ScriptReference Script;

        /// <summary>
        /// Properties that need to be serialized for the script.
        /// </summary>
        public ScriptingPropertyBucket Properties;

        /// <summary>
        /// An engine to run the tests with.
        /// </summary>
        protected readonly UnityScriptingHost _host = new UnityScriptingHost(
            new ResourcesScriptLoader(),
            new NullScriptingDependencyResolver());

        /// <summary>
        /// References to JS functions.
        /// </summary>
        protected ICallable _update;
        protected ICallable _fixedUpdate;
        protected ICallable _lateUpdate;
        protected ICallable _awake;
        protected ICallable _start;
        protected ICallable _onEnable;
        protected ICallable _onDisable;
        protected ICallable _onDestroy;

        /// <summary>
        /// A JS reference to this, passed to every ICallable.
        /// </summary>
        protected JsValue _this;

        /// <summary>
        /// Called by Unity.
        /// </summary>
        protected virtual void Awake()
        {
            try
            {
                _host.Execute(Script.String());
            }
            catch (Exception exception)
            {
                Debug.LogWarning("Could not execute script: " + exception.Message);

                return;
            }

            _this = JsValue.FromObject(_host, this);

            _update = _host.GetFunction("Update");
            _fixedUpdate = _host.GetFunction("FixedUpdate");
            _lateUpdate = _host.GetFunction("LateUpdate");
            _awake = _host.GetFunction("Awake");
            _start = _host.GetFunction("Start");
            _onEnable = _host.GetFunction("OnEnable");
            _onDisable = _host.GetFunction("OnDisable");
            _onDestroy = _host.GetFunction("OnDestroy");

            if (null != _awake)
            {
                _awake.Call(_this, null);
            }
        }

        /// <summary>
        /// Called by Unity.
        /// </summary>
        protected virtual void Update()
        {
            if (null != _update)
            {
                _update.Call(_this, null);
            }
        }

        /// <summary>
        /// Called by Unity.
        /// </summary>
        protected virtual void FixedUpdate()
        {
            if (null != _fixedUpdate)
            {
                _fixedUpdate.Call(_this, null);
            }
        }

        /// <summary>
        /// Called by Unity.
        /// </summary>
        protected virtual void LateUpdate()
        {
            if (null != _lateUpdate)
            {
                _lateUpdate.Call(_this, null);
            }
        }

        /// <summary>
        /// Called by Unity.
        /// </summary>
        protected virtual void Start()
        {
            if (null != _start)
            {
                _start.Call(_this, null);
            }
        }

        /// <summary>
        /// Called by Unity.
        /// </summary>
        protected virtual void OnEnable()
        {
            if (null != _onEnable)
            {
                _onEnable.Call(_this, null);
            }
        }

        /// <summary>
        /// Called by Unity.
        /// </summary>
        protected virtual void OnDisable()
        {
            if (null != _onDisable)
            {
                _onDisable.Call(_this, null);
            }
        }

        /// <summary>
        /// Called by Unity.
        /// </summary>
        protected virtual void OnDestroy()
        {
            if (null != _onDestroy)
            {
                _onDestroy.Call(_this, null);
            }
        }
    }
}