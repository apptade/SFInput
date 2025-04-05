using UnityEngine.InputSystem;
using System.Collections.Generic;

namespace SFInput.Screen {
public sealed class TouchMovementInputAdder : MovementInputAdder
{
    private int ControllerCount => 10;

    protected override void Awake()
    {
        AddableManager.DataManager.AddData(0, ControllerCount);
        base.Awake();
    }

    protected override IReadOnlyDictionary<int, MovementInputController> GetControllers()
    {
        var primaryDeltaInput = new InputAction(type: InputActionType.Value, binding:"<Touchscreen>/primaryTouch/delta");
        var primaryPositionInput = new InputAction(type: InputActionType.Value, binding: "<Touchscreen>/primaryTouch/position");

        var dictionary = new Dictionary<int, MovementInputController>(ControllerCount)
        {
            { 0, new(primaryDeltaInput, primaryPositionInput, AddableManager.DataManager.Data[0]) }
        };

        for (int i = 1; i < ControllerCount; i++)
        {
            var deltaInput = new InputAction(type: InputActionType.Value, binding: $"<Touchscreen>/touch{i}/delta");
            var positionInput = new InputAction(type: InputActionType.Value, binding: $"<Touchscreen>/touch{i}/position");

            dictionary.Add(i, new(deltaInput, positionInput, AddableManager.DataManager.Data[i]));
        }

        return dictionary;
    }
}}