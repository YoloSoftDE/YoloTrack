using System;
using System.Collections.Generic;

namespace YoloTrack.MVC.Controller
{
    struct DependencyAction
    {
        public Action<object, object> Action { get; set; }
        public bool Satisfied { get; set; }
    }

    class DependencyManager
    {
        Dictionary<object, Dictionary<object, List<DependencyAction>>> m_dependencies;

        public DependencyManager()
        {
            m_dependencies = new Dictionary<object, Dictionary<object, List<DependencyAction>>>();
        }

        public void AddDependency(object Dependent, object Dependency, Action<object, object> Action)
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
               Action = Action,
               Satisfied = false
            });
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

            foreach (object dependency in m_dependencies[Dependent])
            {
                _fix_list(Dependent, dependency, m_dependencies[Dependent][dependency]);
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
                    actual_dependency.Action(Dependent, Dependency);
                    actual_dependency.Satisfied = true;
                }
            }

            return true;
        }
    }
}
