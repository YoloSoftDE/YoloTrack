using System;
using System.Collections.Generic;
using System.Reflection;

namespace YoloTrack.MVC.Controller
{
    interface IBindable<T>
    {
        void Bind(T instance);
    }
    interface IObserver<T>
    {
        void Observe(T instance);
    }

    struct DependencyAction
    {
        //public Action<object, object> Action { get; set; }
        public MethodInfo Method { get; set; }
        public bool Satisfied { get; set; }
    }

    class DependencyManager
    {
        Dictionary<object, Dictionary<object, List<DependencyAction>>> m_dependencies;

        public DependencyManager()
        {
            m_dependencies = new Dictionary<object, Dictionary<object, List<DependencyAction>>>();
        }

        public void AddDependency<TDpet, TDpcy>(TDpet Dependent, TDpcy Dependency, Action<TDpet, TDpcy> Action)
        {
            if (!m_dependencies.ContainsKey(Dependent))
            {
                m_dependencies.Add(Dependent, new Dictionary<object, List<DependencyAction>>());
            }
            if (!m_dependencies[Dependent].ContainsKey(Dependency))
            {
                m_dependencies[Dependent].Add(Dependency, new List<DependencyAction>());
            }
            m_dependencies[Dependent][Dependency].Add(new DependencyAction()
            {
                Method = Action.Method,
                Satisfied = false
            });
        }

        public void AddDependencyStaticBind<TDept, TDpcy>(TDept Dependent, TDpcy Dependency)
            where TDept : IBindable<TDpcy>
        {
            AddDependency<TDept, TDpcy>(
                Dependent,
                Dependency,
                new Action<TDept, TDpcy>(delegate(TDept dependent, TDpcy dependency)
                {
                    dependent.Bind(dependency);
                })
            );
        }

        public void AddDependencyObserve<TDept, TDpcy>(TDept Dependent, TDpcy Dependency)
            where TDept : IObserver<TDpcy>
        {
            AddDependency<TDept, TDpcy>(
                Dependent,
                Dependency,
                new Action<TDept, TDpcy>(delegate(TDept dependent, TDpcy dependency)
                {
                    dependent.Observe(dependency);
                })
            );
        }

        public bool TryHandle(object Dependent)
        {
            _fix_dependencies(Dependent);
            _fix_dependents(Dependent);

            return true;
        }

        private bool _fix_dependencies(object Dependent)
        {
            if (!m_dependencies.ContainsKey(Dependent))
            {
                return false;
            }

            foreach (KeyValuePair<object, List<DependencyAction>> dependency in m_dependencies[Dependent])
            {
                _fix_list(Dependent, dependency.Key, dependency.Value);
            }

            return true;
        }

        private bool _fix_dependents(object Dependency)
        {
            foreach (KeyValuePair<object, Dictionary<object, List<DependencyAction>>> dependent in m_dependencies)
            {
                if (dependent.Value.ContainsKey(Dependency))
                {
                    _fix_list(dependent, Dependency, dependent.Value[Dependency]);
                }
            }

            return true;
        }

        private bool _fix_list(object Dependent, object Dependency, List<DependencyAction> DependencyActions)
        {
            for (int i = 0; i < DependencyActions.Count; i++)
            {
                DependencyAction actual_dependency = DependencyActions[i];

                if (!actual_dependency.Satisfied)
                {
                    actual_dependency.Method.Invoke(this, new object[2] {Dependent, Dependency});
                    actual_dependency.Satisfied = true;
                }
            }

            return true;
        }
    }
}
