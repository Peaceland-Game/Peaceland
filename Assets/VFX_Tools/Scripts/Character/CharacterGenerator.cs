using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGenerator : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] GameObject gooberCharacter;
    [SerializeField] int characterCount;
    [SerializeField] Vector2 spacing;
    [Header("Overall GoopSettings")]
    [SerializeField] Vector2 timeScaleRange;
    [SerializeField] Vector2 dirA;
    [SerializeField] Vector2 dirB;
    [Header("SimpleNoise")]
    [SerializeField] Vector2 SimpleNoiseScaleRange;
    [SerializeField] Vector2 SimpleGradientThresholdARange;
    [SerializeField] Vector2 SimpleGradientThresholdBRange;
    [SerializeField] Vector2 SimpleCenterXRange;
    [SerializeField] Vector2 SimpleCenterYRange;
    [SerializeField] Vector2 SimpleSizeXRange;
    [SerializeField] Vector2 SimpleSizeYRange;
    [Header("Voronoi")]
    [SerializeField] Vector2 RescaleRange;
    [SerializeField] Vector2 AngleOffsetRange;
    [SerializeField] Vector2 CellDensityRange;
    [SerializeField] Vector2 VoronoiGradientThresholdARange;
    [SerializeField] Vector2 VoronoiGradientThresholdBRange;
    [SerializeField] Vector2 VoronoiCenterXRange;
    [SerializeField] Vector2 VoronoiCenterYRange;
    [SerializeField] Vector2 VoronoiSizeXRange;
    [SerializeField] Vector2 VoronoiSizeYRange;
    [Header("Color")]
    [SerializeField] Gradient PrimaryColorRange;
    [SerializeField] Gradient SecondaryColorRange;
    [SerializeField] Gradient HighlightColorRange;
    [SerializeField] Vector2 SaturationRange;
    [SerializeField] Vector2 PrimaryMetallicRange;
    [SerializeField] Vector2 PrimarySmoothnessRange;
    [SerializeField] Vector2 SecondaryMetallicRange;
    [SerializeField] Vector2 SecondarySmoothnessRange;
    [Header("VertexDisplacement")]
    [SerializeField] Vector2 DisScaleRange;
    [SerializeField] Vector2 DisAngleOffsetRange;
    [SerializeField] Vector2 DisCellDensityRange;
    [SerializeField] Vector2 DisTimeScaleRange;

    private void Start()
    {
        for (int i = 0; i < characterCount; i++)
        {
            SpawnCharacters(i);
        }
    }

    private void SpawnCharacters(int index)
    {
        GooberMaterial gooberMat = new GooberMaterial(
            RandomRange(timeScaleRange),
            new Vector2(UnityEngine.Random.Range(dirA.x, dirB.x), UnityEngine.Random.Range(dirA.y, dirB.y)),

            RandomRange(SimpleNoiseScaleRange),
            RandomRange(SimpleGradientThresholdARange),
            RandomRange(SimpleGradientThresholdBRange),
            RandomRange(SimpleCenterXRange, SimpleCenterYRange),
            RandomRange(SimpleSizeXRange, SimpleSizeYRange),

            RandomRange(RescaleRange),
            RandomRange(AngleOffsetRange),
            RandomRange(CellDensityRange),
            RandomRange(VoronoiGradientThresholdARange),
            RandomRange(VoronoiGradientThresholdBRange),
            RandomRange(VoronoiCenterXRange, VoronoiCenterYRange),
            RandomRange(VoronoiSizeXRange, VoronoiSizeYRange),

            PrimaryColorRange.Evaluate(UnityEngine.Random.Range(0.0f, 1.0f)),
            SecondaryColorRange.Evaluate(UnityEngine.Random.Range(0.0f, 1.0f)),
            HighlightColorRange.Evaluate(UnityEngine.Random.Range(0.0f, 1.0f)),
            RandomRange(SaturationRange),
            RandomRange(PrimaryMetallicRange),
            RandomRange(SecondaryMetallicRange),
            RandomRange(PrimarySmoothnessRange),
            RandomRange(SecondarySmoothnessRange),

            RandomRange(DisScaleRange),
            RandomRange(DisAngleOffsetRange),
            RandomRange(DisCellDensityRange),
            RandomRange(DisTimeScaleRange)
            );

        GameObject goober = Instantiate(gooberCharacter, this.transform.position + new Vector3(this.transform.position.x + index * spacing.x, 0.0f, this.transform.position.z), Quaternion.LookRotation(-Vector3.forward));
        SetCharacter(goober.GetComponentInChildren<Renderer>(), gooberMat);
    }

    private float RandomRange(Vector2 range)
    {
        return UnityEngine.Random.Range(range.x, range.y);  
    }

    private Vector2 RandomRange(Vector2 rangeX, Vector2 rangeY)
    {
        return new Vector2(RandomRange(rangeX), RandomRange(rangeY));
    }

    private void SetCharacter(Renderer renderer, GooberMaterial goober)
    {
        Material[] materials = renderer.materials;
        foreach(Material mat in materials)
        {
            mat.SetFloat("_TimeScale", goober.TimeScaleRange);
            mat.SetVector("_PatternDirection", goober.Direction);
            mat.SetFloat("_SimpleNoiseScale", goober.SimpleNoiseScale);
            mat.SetFloat("_SimpleGradientThresholdA", goober.SimpleGradientThresholdA);
            mat.SetFloat("_SimpleGradientThresholdB", goober.SimpleGradientThresholdB);
            mat.SetVector("_SimpleCenter", goober.SimpleCenter);
            mat.SetVector("_SimpleSize", goober.SimpleSize);
            mat.SetFloat("_Rescale", goober.Rescale);
            mat.SetFloat("_AngleOffset", goober.AngleOffset);
            mat.SetFloat("_CellDensity", goober.CellDensity);
            mat.SetFloat("_VoronoiGradientThresholdA", goober.VoronoiGradientThresholdA);
            mat.SetFloat("_VoronoiGradientThresholdB", goober.VoronoiGradientThresholdB);
            mat.SetVector("_VoronoiCenter", goober.VoronoiCenter);
            mat.SetVector("_VoronoiSize", goober.VoronoiSize);
            mat.SetColor("_Primary", goober.Primary);
            mat.SetColor("_Secondary", goober.Secondary);
            mat.SetColor("_Highlight", goober.Highlight);
            mat.SetFloat("_Saturation", goober.Saturation);
            mat.SetFloat("_PrimaryMetallic", goober.PrimaryMetallic);
            mat.SetFloat("_PrimarySmoothness", goober.PrimarySmoothness);
            mat.SetFloat("_SecondaryMetallic", goober.SecondaryMetallic);
            mat.SetFloat("_SecondarySmoothness", goober.SecondarySmoothness);
            mat.SetFloat("_DisplacementScale", goober.DisScale);
            mat.SetFloat("_VertexAngleOffset", goober.DisAngleOffset);
            mat.SetFloat("_VertexCellDensity", goober.DisCellDensity);
            mat.SetFloat("_DisTimeScale", goober.DisTimeScale); 
        }
    }

    
    private class GooberMaterial
    {
        // Pattern Settings 
        public float TimeScaleRange;
        public Vector2 Direction;

        // SimpleNoise
        public float SimpleNoiseScale;
        public float SimpleGradientThresholdA;
        public float SimpleGradientThresholdB;
        public Vector2 SimpleCenter;
        public Vector2 SimpleSize;

        // Voronoi
        public float Rescale;
        public float AngleOffset;
        public float CellDensity;
        public float VoronoiGradientThresholdA;
        public float VoronoiGradientThresholdB;
        public Vector2 VoronoiCenter;
        public Vector2 VoronoiSize;

        // Color
        public Color Primary;
        public Color Secondary;
        public Color Highlight;
        public float Saturation;
        public float PrimaryMetallic;
        public float SecondaryMetallic;
        public float PrimarySmoothness;
        public float SecondarySmoothness;

        // VertexDisplacement
        public float DisScale;
        public float DisAngleOffset;
        public float DisCellDensity;
        public float DisTimeScale;

        public GooberMaterial(
            float timeScaleRange,
            Vector2 direction,
            float SimpleNoiseScale,
            float SimpleGradientThresholdA,
            float SimpleGradientThresholdB,
            Vector2 SimpleCenter,
            Vector2 SimpleSize,
            float Rescale,
            float AngleOffset,
            float CellDensity,
            float VoronoiGradientThresholdA,
            float VoronoiGradientThresholdB,
            Vector2 VoronoiCenter,
            Vector2 VoronoiSize,
            Color PrimaryColor,
            Color SecondaryColor,
            Color HighlightColor,
            float Saturation,
            float PrimaryMetallic,
            float SecondaryMetallic,
            float PrimarySmoothness,
            float SecondarySmoothness,
            float DisScale,
            float DisAngleOffset,
            float DisCellDensity,
            float DisTimeScale)
        {
            this.TimeScaleRange = timeScaleRange;
            this.Direction = direction;
            this.SimpleNoiseScale = SimpleNoiseScale;
            this.SimpleGradientThresholdA = SimpleGradientThresholdA;
            this.SimpleGradientThresholdB = SimpleGradientThresholdB;
            this.SimpleCenter = SimpleCenter;
            this.SimpleSize = SimpleSize;
            this.Rescale = Rescale;
            this.AngleOffset = AngleOffset;
            this.CellDensity = CellDensity;
            this.VoronoiGradientThresholdA = VoronoiGradientThresholdA;
            this.VoronoiGradientThresholdB = VoronoiGradientThresholdB;
            this.VoronoiCenter = VoronoiCenter;
            this.VoronoiSize = VoronoiSize;  
            this.Primary = PrimaryColor;
            this.Secondary = SecondaryColor;
            this.Highlight = HighlightColor;
            this.Saturation = Saturation;
            this.PrimaryMetallic = PrimaryMetallic;
            this.PrimarySmoothness = PrimarySmoothness;
            this.SecondaryMetallic = SecondaryMetallic;
            this.SecondarySmoothness = SecondarySmoothness;
            this.DisScale = DisScale;
            this.DisAngleOffset = DisAngleOffset;
            this.DisCellDensity = DisCellDensity;
            this.DisTimeScale = DisTimeScale;
        }
    }
}
