using System;
using Tayx.Graphy;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {
    public FractityMaster Master;
    public GraphyManager Graphy;
    public Toggle[] Toggles;
    
    private bool _dragging;
    private Vector2 _lastMousePosition;

    private readonly Gradient[] _palettes = new [] {
        new Gradient {
            colorKeys = new[] {
                new GradientColorKey(new Color(.1f, .2f, .3f, 1), 0),
                new GradientColorKey(new Color(1.536f, 2.304f, .768f, 1), 1),
            }
        },
        new Gradient {
            colorKeys = new[] {
                new GradientColorKey(new Color(0, .027f, .392f, 1), 0),
                new GradientColorKey(new Color(.125f, .42f, .796f, 1), .16f),
                new GradientColorKey(new Color(.929f, 1, 1, 1), .42f),
                new GradientColorKey(new Color(1, .667f, 0, 1), .6425f),
                new GradientColorKey(new Color(0, .008f, 0, 1), .8575f),
            }
        },
        new Gradient {
            colorKeys = new[] {
                new GradientColorKey(new Color(0f, 0f, 0f, 1), 0),
                new GradientColorKey(new Color(0f, 1f, 1f, 1), 1),
            }
        },
        new Gradient {
            colorKeys = new[] {
                new GradientColorKey(new Color(1f, 0f, 0f, 1), 0),
                new GradientColorKey(new Color(1f, 1f, 1f, 1), .1f),
                new GradientColorKey(new Color(0f, 0f, 0f, 1), 1),
            }
        },
    };
    
    private static Vector2 ScreenSize => new Vector2(Screen.width, Screen.height);

    private Vector2 MousePointTo(Vector2 mousePosition) => (mousePosition + Master.Offset - ScreenSize / 2f) / 180f * Master.ScaleFactor;

    private void Awake() {
        foreach (var toggle in Toggles) {
            toggle.SetIsOnWithoutNotify(false);
        }
        
        Master.UpdatePalette(_palettes[0]);
        QualitySettings.vSyncCount = 0;
    }

    private void Update() {
        var isOnSettingsBar = Input.mousePosition.x < 360 && (Screen.height - Input.mousePosition.y) < 200;  
        
        if (!isOnSettingsBar && Input.mouseScrollDelta.y != 0) {
            Magnify(Input.mousePosition, -Input.mouseScrollDelta.y);
        }

        if (!isOnSettingsBar && !_dragging && Input.GetMouseButtonDown(0)) {
            _dragging = true;
            _lastMousePosition = Input.mousePosition;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
        }
        
        if (_dragging && !Input.GetMouseButton(0)) {
            _dragging = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (_dragging) {
            Move(_lastMousePosition - (Vector2) Input.mousePosition);
            _lastMousePosition = Input.mousePosition;
        }
    }

    private void Magnify(Vector2 mousePosition, float scale) {
        var mousePointTo = MousePointTo(mousePosition);
        Master.ScaleFactor = Mathf.Clamp(scale > 0 ? Master.ScaleFactor * 1.01f : Master.ScaleFactor / 1.01f, 2e-7f, 1);
        Move(180 * mousePointTo / Master.ScaleFactor - mousePosition + ScreenSize / 2 - Master.Offset);
    }

    private void Move(Vector2 delta) {
        Master.Offset += delta;
        Master.MarkParametersUpdated();
    }

    public void TogglePerformanceStats(bool state) {
        if (!Application.isPlaying)
            return;
        
        if (state)
            Graphy.Enable();
        else
            Graphy.Disable();
    }

    public void ChangeTargetFPS(int value) {
        Application.targetFrameRate = value switch {
            1 => 60,
            2 => 144,
            3 => 240,
            _ => 0
        };
    }

    public void ChangeGradient(int value) {
        Master.UpdatePalette(_palettes[value]);
    }

    public void SetPower(float value) {
        Master.Power = value;
        Master.MarkParametersUpdated();
    }
}