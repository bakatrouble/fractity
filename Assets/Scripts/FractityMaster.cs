using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class FractityMaster : MonoBehaviour {
    public ComputeShader MandelbrotShader;
    public float ScaleFactor = 1;
    public Vector2 Offset = Vector2.zero;
    public Gradient PaletteGradient;
    public float Power = 2;

    private const int MAX_ITERATIONS = 256;
    private RenderTexture _target;
    private readonly Vector3[] _palette = new Vector3[MAX_ITERATIONS + 1];
    private ComputeBuffer _paletteBuffer;
    private bool _parametersNeedUpdate = true;

    private void OnEnable() {
        MandelbrotShader.SetInt("_Type", 0);
        MandelbrotShader.SetInt("_Power", 2);
    }

    private void OnDisable() {
        _paletteBuffer?.Release();
    }

    private void InitRenderTexture() {
        if (_target == null || _target.width != Screen.width || _target.height != Screen.height) {
            if (_target != null) {
                _target.Release();
            }

            
            _target = new RenderTexture(Screen.width, Screen.height, 0,
                RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
            _target.enableRandomWrite = true;
            _target.Create();
        }
    }

    private void Render(RenderTexture dest) {
        InitRenderTexture();
        if (_parametersNeedUpdate)
            SetShaderParameters();
        
        MandelbrotShader.SetTexture(0, "Result", _target);
        var threadGroupsX = Mathf.CeilToInt(Screen.width / 32f);
        var threadGroupsY = Mathf.CeilToInt(Screen.height / 32f);
        MandelbrotShader.Dispatch(0, threadGroupsX, threadGroupsY, 1);
        
        Graphics.Blit(_target, dest);
    }

    private void SetShaderParameters() {
        MandelbrotShader.SetFloat("_ScaleFactor", ScaleFactor);
        MandelbrotShader.SetVector("_Offset", Offset);
        MandelbrotShader.SetFloat("_Power", Power);
        _parametersNeedUpdate = false;
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest) {
        Render(dest);
    }

    private void InitPalette() {
        _paletteBuffer ??= new ComputeBuffer(MAX_ITERATIONS + 1, 3 * 4);
        for (var i = 0; i <= MAX_ITERATIONS; i++) {
            var color = PaletteGradient.Evaluate((float) i / MAX_ITERATIONS);
            _palette[i] = new Vector3(color.r, color.g, color.b);
        }
        _paletteBuffer.SetData(_palette);
        MandelbrotShader.SetBuffer(0, "_Palette", _paletteBuffer);
    }

    public void UpdatePalette(Gradient gradient) {
        PaletteGradient = gradient;
        InitPalette();
    }

    public void ChangeType(int type) {
        MandelbrotShader.SetInt("_Type", type);
    }

    public void MarkParametersUpdated() {
        _parametersNeedUpdate = true;
    }

    private void OnValidate() {
        MarkParametersUpdated();
    }
}
