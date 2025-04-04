using System.Collections.Generic;

namespace SFInput {
public sealed class InputControllerManager<TController> : IInputControllerManager<TController> where TController : IInputController
{
    private readonly Dictionary<int, IInputPredicateManager> _predicateManagers = new();
    private readonly Dictionary<int, IReadOnlyList<TController>> _readControllers = new();
    private readonly Dictionary<int, ICollection<TController>> _writeControllers = new();

    public IReadOnlyDictionary<int, IReadOnlyList<TController>> Controllers { get => _readControllers; }
    public IReadOnlyDictionary<int, IInputPredicateManager> PredicateManagers { get => _predicateManagers; }

    public void AddController(int index, TController controller)
    {
        if (controller is null) return;

        if (_writeControllers.ContainsKey(index) is false)
        {
            var collection = new List<TController>();

            _writeControllers.Add(index, collection);
            _readControllers.Add(index, collection);
        }

        _writeControllers[index].Add(controller);

        if (_predicateManagers.TryGetValue(index, out var manager))
        {
            controller.PredicateManager.AddManager(manager);
        }
    }

    public void AddPredicateManager(int index, IInputPredicateManager manager)
    {
        if (manager is null) return;

        if (_predicateManagers.TryAdd(index, manager))
        {
            this.ForEachController(c => c.PredicateManager.AddManager(manager));
        }
    }

    public void RemoveController(int index, TController controller)
    {
        if (controller is null) return;

        if (_writeControllers.TryGetValue(index, out var collection))
        {
            collection.Remove(controller);

            if (_predicateManagers.TryGetValue(index, out var manager))
            {
                controller.PredicateManager.RemoveManager(manager);
            }
        }
    }

    public void RemovePredicateManager(int index)
    {
        if (_predicateManagers.Remove(index, out var manager))
        {
            this.ForEachController(c => c.PredicateManager.RemoveManager(manager));
        }
    }
}}