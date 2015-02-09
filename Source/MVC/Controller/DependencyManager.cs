using System;
using System.Collections.Generic;
using System.Reflection;

namespace YoloTrack.MVC.Controller
{
    public interface IBindable<T>
    {
        void Bind(T instance);
    }
    public interface IObserver<T>
    {
        void Observe(T instance);
    }

    struct DependencyAction
    {
        //public Action<object, object> Action { get; set; }
        public MethodInfo Method { get; set; }
        public bool Satisfied { get; set; }
    }

    struct FinalizerMethod
    {
        public Action Method { get; set; }
        public bool Executed { get; set; }
    }

    public class Dependent
    {
        private Func<bool> m_initializer;
        private Dictionary<object, List<DependencyAction>> m_dependencies;
        private List<FinalizerMethod> m_finalizers;
        private Func<object> m_accessor;
        private List<Func<object>> m_prerequisites;

        public Dependent(Func<object> Accessor)
        {
            m_accessor = Accessor;
            m_dependencies = new Dictionary<object, List<DependencyAction>>();
            m_finalizers = new List<FinalizerMethod>();
            m_prerequisites = new List<Func<object>>();
        }

        public void Method<TDependency>(Func<TDependency> DependencyAccessor, Action<object, TDependency> Action)
        {
            if (!m_dependencies.ContainsKey(DependencyAccessor))
            {
                m_dependencies.Add(DependencyAccessor, new List<DependencyAction>());
            }
            m_dependencies[DependencyAccessor].Add(new DependencyAction()
            {
                Method = Action.Method,
                Satisfied = false
            });
        }

        public void Bind<TDependency>(Func<TDependency> DependencyAccessor)
        {
            Method(DependencyAccessor, delegate(object DependentInstance, TDependency DependencyInstance)
            {
                ((IBindable<TDependency>)DependentInstance).Bind(DependencyInstance);
            });
        }

        public void Observe<TDependency>(Func<TDependency> DependencyAccessor)
        {
            Method(DependencyAccessor, delegate(object DependentInstance, TDependency DependencyInstance)
            {
                ((IObserver<TDependency>)DependentInstance).Observe(DependencyInstance);
            });
        }

        public void After<TPrerequisite>(Func<TPrerequisite> Pre)
        {
            m_prerequisites.Add(() => (object)Pre());
        }

        public void Initialize(Func<bool> InitializeAction)
        {
            m_initializer = InitializeAction;
        }

        public void Finalize(Action FinalAction)
        {
            m_finalizers.Add(new FinalizerMethod()
            {
                Method = FinalAction,
                Executed = false
            });
        }

        public bool Wraps(object Compare)
        {
            return Compare == m_accessor();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>true if an instance could be </returns>
        public bool Fix()
        {
            if (m_accessor() == null && m_initializer != null)
            {
                if (!m_initializer())
                {
                    return false;
                }
            }

            // Don't initialize until the prerequisites have been fulfilled
            foreach (Func<object> pre in m_prerequisites)
            {
                if (pre() == null)
                {
                    return false;
                }
            }

            foreach (KeyValuePair<object, List<DependencyAction>> dependency in m_dependencies)
            {
                Func<object> depdency_accessor = (Func<object>)dependency.Key;
                if (depdency_accessor() != null)
                {
                    _fix_list(m_accessor(), depdency_accessor(), dependency.Value);
                }
            }

            return true;
        }

        public void RunFinalizers()
        {
            for (int i = 0; i < m_finalizers.Count; i++)
            {
                FinalizerMethod action = m_finalizers[i];

                if (!action.Executed)
                {
                    action.Method();
                    action.Executed = true;
                    m_finalizers[i] = action;
                }
            }
        }

        [Obsolete]
        public bool FixFor(object Dependency)
        {
            foreach (KeyValuePair<object, List<DependencyAction>> dependency in m_dependencies)
            {
                Func<object> depdency_accessor = (Func<object>)dependency.Key;
                if (depdency_accessor() == Dependency)
                {
                    _fix_list(m_accessor(), depdency_accessor(), dependency.Value);
                }
            }

            return true;
        }

        public bool Completed()
        {
            if (m_accessor() == null)
            {
                return false;
            }

            foreach (KeyValuePair<object, List<DependencyAction>> dependency in m_dependencies)
            {
                foreach (DependencyAction action in dependency.Value)
                {
                    if (!action.Satisfied)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void _fix_list(object Dependent, object Dependency, List<DependencyAction> DependencyActions)
        {
            for (int i = 0; i < DependencyActions.Count; i++)
            {
                DependencyAction actual_dependency = DependencyActions[i];

                if (!actual_dependency.Satisfied)
                {
                    actual_dependency.Method.Invoke(this, new object[2] { Dependent, Dependency });
                    actual_dependency.Satisfied = true;
                    DependencyActions[i] = actual_dependency;
                }
            }
        }
    }

    class DependencyManager
    {
        private List<Dependent> m_dependents;

        public DependencyManager()
        {
            m_dependents = new List<Dependent>();
        }

        public void AddDependent(Func<object> Accessor, Action<Dependent> SetupRoutine)
        {
            Dependent dependent = new Dependent(Accessor);
            SetupRoutine(dependent);
            m_dependents.Add(dependent);
        }

        public void FixAll()
        {
            bool completed = true;
            for (int i = 0; i < 2; i++)
            {
                foreach (Dependent dependent in m_dependents)
                {
                    dependent.Fix();
                    if (dependent.Completed())
                    {
                        dependent.RunFinalizers();
                    }
                    else
                    {
                        completed = false;
                    }
                }

                if (completed)
                {
                    break;
                }
            }
        }

        public bool FixOne(object Dependent)
        {
            _fix_dependencies(Dependent);
            _fix_dependents(Dependent);

            return true;
        }

        private bool _fix_dependencies(object Dependent)
        {
            Dependent depedent = m_dependents.Find(d => d.Wraps(Dependent));
            if (depedent == null)
            {
                return false;
            }

            depedent.Fix();
            return true;
        }

        private bool _fix_dependents(object Dependency)
        {
            /*
            foreach (KeyValuePair<object, Dictionary<object, List<DependencyAction>>> dependent in m_dependencies)
            {
                if (dependent.Value.ContainsKey(Dependency))
                {
                    _fix_list(dependent, Dependency, dependent.Value[Dependency]);
                }
            }
             */

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
