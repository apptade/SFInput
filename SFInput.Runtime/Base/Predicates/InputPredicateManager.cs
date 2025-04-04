using System.Collections.Generic;

namespace SFInput {
public sealed class InputPredicateManager : IInputPredicateManager
{
    private readonly ICollection<IInputPredicateManager> _managers;
    private readonly ICollection<IInputPredicate> _predicates;

    public InputPredicateManager(ICollection<IInputPredicateManager> managers = null, ICollection<IInputPredicate> predicates = null)
    {
        _managers = managers ?? new List<IInputPredicateManager>();
        _predicates = predicates ?? new List<IInputPredicate>();
    }

    public void AddManager(IInputPredicateManager manager)
    {
        if (manager is null) return;
        _managers.Add(manager);
    }

    public void AddPredicate(IInputPredicate predicate)
    {
        if (predicate is null) return;
        _predicates.Add(predicate);
    }

    public void RemoveManager(IInputPredicateManager manager)
    {
        if (manager is null) return;
        _managers.Remove(manager);
    }

    public void RemovePredicate(IInputPredicate predicate)
    {
        if (predicate is null) return;
        _predicates.Remove(predicate);
    }

    public bool Result()
    {
        foreach (var manager in _managers)
        {
            if (manager.Result() is false)
                return false;
        }

        foreach (var predicate in _predicates)
        {
            if (predicate.Result() is false)
                return false;
        }

        return true;
    }
}}