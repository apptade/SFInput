using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SFInput.Screen {
public sealed class MovementInputController : InputController, IDisposable
{
    private readonly InputAction _deltaInput;
    private readonly InputAction _positionInput;
    private readonly MovementInputData _movementData;

    private Vector2 _lastInputDelta;
    private int _positionInputExecuteCount;

    public MovementInputController(InputAction deltaInput, InputAction positionInput, MovementInputData movementData) 
    {
        _deltaInput = deltaInput;
        _positionInput = positionInput;
        _movementData = movementData;
    }

    public void Dispose()
    {
        _deltaInput.Dispose();
        _positionInput.Dispose();
    }

    protected override void OnEnable()
    {
        _deltaInput.started += OnDeltaInputStarted;
        _deltaInput.canceled += OnDeltaInputCanceled;
        _deltaInput.Enable();

        _positionInput.started += OnPositionInputStarted;
        _positionInput.canceled += OnPositionInputCanceled;
        _positionInput.performed += OnPositionInputPerformed;
        _positionInput.Enable();
    }

    protected override void OnDisable()
    {
        _deltaInput.started -= OnDeltaInputStarted;
        _deltaInput.canceled -= OnDeltaInputCanceled;
        _deltaInput.Disable();

        _positionInput.started -= OnPositionInputStarted;
        _positionInput.canceled -= OnPositionInputCanceled;
        _positionInput.performed -= OnPositionInputPerformed;
        _positionInput.Disable();
    }

    private void OnDeltaInputStarted(InputAction.CallbackContext callback)
    {
        if (PredicateManager.Result())
        {
            _movementData.OnDeltaChanged(callback.ReadValue<Vector2>());
            _lastInputDelta = _movementData.Delta;
        }
    }

    private void OnDeltaInputCanceled(InputAction.CallbackContext callback)
    {
        if (_movementData.Delta != Vector2.zero)
        {
            _movementData.OnDeltaChanged(Vector2.zero);
        }
    }

    private void OnPositionInputStarted(InputAction.CallbackContext callback)
    {
        if (PredicateManager.Result())
        {
            _movementData.OnPositionChanged(callback.ReadValue<Vector2>());
        }

        _positionInputExecuteCount = 1;
    }

    private void OnPositionInputCanceled(InputAction.CallbackContext callback)
    {
        if (_movementData.Position != Vector2.zero)
        {
            _movementData.OnPositionChanged(Vector2.zero);
        }
    }

    private void OnPositionInputPerformed(InputAction.CallbackContext callback)
    {
        if (_positionInputExecuteCount > 1 && PredicateManager.Result())
        {
            _movementData.OnPositionChanged(callback.ReadValue<Vector2>());
            _movementData.OnMovementChanged(_lastInputDelta, _movementData.Position);
        }

        _positionInputExecuteCount += 1;
    }
}}