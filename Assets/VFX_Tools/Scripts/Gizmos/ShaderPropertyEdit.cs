using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class ShaderPropertyEdit : MonoBehaviour
{
    [Tooltip("Takes the attributes from this shader and builds a blueprint")]
    [SerializeField] Shader shader;
    [Tooltip("Materials that attributes are loaded into")]
    [SerializeField] List<Material> targetMaterials;
    [Space]
    [Tooltip("Defines the ranges that random values can be chosen from")]
    [SerializeField] RangeProperties propertiesRanges;
    [Space]
    [Tooltip("How many property sets would you like to generate")]
    [SerializeField] int numToGenerate;
    [Tooltip("Resulting generated property sets. Top of the list are applied to target materials")]
    [SerializeField] List<ShaderProperties> generatedProperties;
    [Space]

    private ShaderProperties propertiesBP;


    public void LoadProperties()
    {
        propertiesBP = new ShaderProperties();
        
        int propertyCount = shader.GetPropertyCount();
        for (int i = 0; i < propertyCount; i++)
        { 
            // Make sure accessible 
            if ((shader.GetPropertyFlags(i) & UnityEngine.Rendering.ShaderPropertyFlags.HideInInspector) != UnityEngine.Rendering.ShaderPropertyFlags.HideInInspector)
            {
                string name = shader.GetPropertyName(i);
                ShaderPropertyType type = shader.GetPropertyType(i);

                Tuple<string, ShaderPropertyType> nameAndType =
                    new Tuple<string, ShaderPropertyType>(shader.GetPropertyName(i), shader.GetPropertyType(i));

                propertiesBP.nameAndType.Add(nameAndType);
                propertiesBP.nameToType.Add(name, type);

                // Add value to given type list and name to nameToIndex array 
                switch (type)
                {
                    case ShaderPropertyType.Color:
                        propertiesBP.nameToIndex.Add(name, propertiesBP.colors.Count);
                        ColorHelper colorHelper = new ColorHelper(name);
                        colorHelper.color = (Color)shader.GetPropertyDefaultVectorValue(i);
                        propertiesBP.colors.Add(colorHelper);
                        break;
                    case ShaderPropertyType.Vector:
                        propertiesBP.nameToIndex.Add(name, propertiesBP.vectors.Count);
                        VectorHelper vectorHelper = new VectorHelper(name);
                        vectorHelper.vector = shader.GetPropertyDefaultVectorValue(i);
                        propertiesBP.vectors.Add(vectorHelper);
                        break;
                    case ShaderPropertyType.Float:
                        propertiesBP.nameToIndex.Add(name, propertiesBP.floats.Count);
                        FloatHelper floatHelper = new FloatHelper(name);
                        floatHelper.value = shader.GetPropertyDefaultFloatValue(i);
                        propertiesBP.floats.Add(floatHelper);
                        break;
                    case ShaderPropertyType.Range:
                        propertiesBP.nameToIndex.Add(name, propertiesBP.ranges.Count);
                        RangeHelper rangeHelper = new RangeHelper(name);
                        rangeHelper.range = new SFloatRange(shader.GetPropertyDefaultFloatValue(i), shader.GetPropertyRangeLimits(i));
                        propertiesBP.ranges.Add(rangeHelper);
                        break;
                    case ShaderPropertyType.Texture:
                        break;
                    case ShaderPropertyType.Int:
                        propertiesBP.nameToIndex.Add(name, propertiesBP.ints.Count);
                        IntHelper intHelper = new IntHelper(name);
                        intHelper.value = shader.GetPropertyDefaultIntValue(i);
                        propertiesBP.ints.Add(intHelper);
                        break;
                }
            }
        }
    }

    public void OverrideRanges()
    {
        // Create ranges object 
        propertiesRanges = new RangeProperties(propertiesBP);
    }

    public void GenerateRandomProperties()
    {
        if (propertiesRanges == null)
            return;

        generatedProperties = new List<ShaderProperties>();
        
        for (int i = 0; i < numToGenerate; i++)
        {
            ShaderProperties properties = propertiesRanges.GenerateSP(propertiesBP);
            if(properties != null)
                generatedProperties.Add(properties);
        }
    }

    public void LoadIntoTargetMaterials()
    {
        // Take the top materials and apply the equal amount of
        // top properties to thme if possible 
        
        for (int i = 0; i < targetMaterials.Count; i++)
        {
            // Check if within generatedProperties range
            if (i >= generatedProperties.Count)
                break;
            
            ShaderProperties curr = generatedProperties[i];
            LoadIntoMaterial(targetMaterials[i], curr);
        }
    }


    /// <summary>
    /// If a properties was created outside this script use this
    /// function to finalize it 
    /// </summary>
    /// <param name="properties"></param>
    public static void GeneratePropertyHelpers(ShaderProperties properties, Shader shader)
    {
        properties.nameAndType = new List<Tuple<string, ShaderPropertyType>>();
        properties.nameToType = new Dictionary<string, ShaderPropertyType>();

        int propertyCount =
            properties.colors.Count + 
            properties.vectors.Count + 
            properties.floats.Count + 
            properties.ranges.Count + 
            properties.texs.Count + 
            properties.ints.Count;

        int cols = 0;
        int vecs = 0;
        int floats = 0;
        int ranges = 0;
        int texs = 0;
        int ints = 0;

        // Combine all properties into lists 
        for (int i = 0; i < propertyCount; i++)
        {
            Tuple<string, ShaderPropertyType> nameAndType =
                                new Tuple<string, ShaderPropertyType>(shader.GetPropertyName(i), shader.GetPropertyType(i));

            string name = shader.GetPropertyName(i);
            ShaderPropertyType type = shader.GetPropertyType(i);
            properties.nameAndType.Add(nameAndType);
            properties.nameToType.Add(name, type);

            // Keep track of property indecies 
            switch (type)
            {
                case ShaderPropertyType.Color:
                    properties.nameToIndex[name] = cols;
                    cols++;
                    break;
                case ShaderPropertyType.Vector:
                    properties.nameToIndex[name] = vecs;
                    vecs++;
                    break;
                case ShaderPropertyType.Float:
                    properties.nameToIndex[name] = floats;
                    floats++;
                    break;
                case ShaderPropertyType.Range:
                    properties.nameToIndex[name] = ranges;
                    ranges++;
                    break;
                case ShaderPropertyType.Texture:
                    properties.nameToIndex[name] = texs;
                    texs++;
                    break;
                case ShaderPropertyType.Int:
                    properties.nameToIndex[name] = ints;
                    ints++;
                    break;
            }
        }
    }

    /// <summary>
    /// Load a property's attributes into given material 
    /// </summary>
    /// <param name="material"></param>
    /// <param name="properties"></param>
    public static void LoadIntoMaterial(Material material, ShaderProperties properties)
    {

        for (int i = 0; i < properties.nameAndType.Count; i++)
        {
            string name = properties.nameAndType[i].Item1;
            ShaderPropertyType type = properties.nameAndType[i].Item2;
            
            int index = properties.nameToIndex[name];
            
            switch (type)
            {
                case ShaderPropertyType.Color:
                    material.SetColor(name, properties.colors[index].color);
                    break;
                case ShaderPropertyType.Vector:
                    material.SetVector(name, properties.vectors[index].vector);
                    break;
                case ShaderPropertyType.Float:
                    material.SetFloat(name, properties.floats[index].value);
                    break;
                case ShaderPropertyType.Range:
                    material.SetFloat(name, properties.ranges[index].range.GetValue());
                    break;
                case ShaderPropertyType.Texture:
                    material.SetTexture(name, properties.texs[index]);
                    break;
                case ShaderPropertyType.Int:
                    material.SetInt(name, properties.ints[index].value);
                    break;
            }
        }
    }


    #region ShaderProperties

    [Serializable]
    public class ShaderProperties
    {
        public List<Tuple<string, ShaderPropertyType>> nameAndType = new List<Tuple<string, ShaderPropertyType>>();
        public Dictionary<string, ShaderPropertyType> nameToType = new Dictionary<string, ShaderPropertyType>();
        public Dictionary<string, int> nameToIndex = new Dictionary<string, int>();
        
        [SerializeField] public List<ColorHelper> colors = new List<ColorHelper>();
        [SerializeField] public List<VectorHelper> vectors = new List<VectorHelper>();
        [SerializeField] public List<FloatHelper> floats = new List<FloatHelper>();
        [SerializeField] public List<RangeHelper> ranges = new List<RangeHelper>();
        [SerializeField] public List<Texture> texs = new List<Texture>();
        [SerializeField] public List<IntHelper> ints = new List<IntHelper>();

    }

    [Serializable]
    public class ColorHelper
    {
        [SerializeField] public string colorName;
        [SerializeField] public Color color = new Color();

        public ColorHelper(string name)
        {
            colorName = name;
        }
    }

    [Serializable]
    public class VectorHelper
    {
        [SerializeField] public string vectorName;
        [SerializeField] public Vector3 vector;

        public VectorHelper(string name)
        {
            vectorName = name;
        }
    }

    [Serializable]
    public class FloatHelper
    {
        [SerializeField] public string floatName;
        [SerializeField] public float value;

        public FloatHelper(string name)
        {
            floatName = name;
        }
    }

    [Serializable]
    public class RangeHelper // :3 
    {
        [SerializeField] public string rangeName;
        [SerializeField] public SFloatRange range;

        public RangeHelper(string name)
        {
            rangeName = name;
        }
    }

    [Serializable]
    public class IntHelper
    {
        [SerializeField] public string intName;
        [SerializeField] public int value;

        public IntHelper(string name)
        {
            intName = name;
        }
    }


    [Serializable]
    public struct SFloatRange
    {
        [SerializeField] private float min, max;
        [SerializeField] private float value;

        public SFloatRange(float value, float min, float max)
        {
            this.value = value;
            this.min = min;
            this.max = max;
        }

        public SFloatRange(float value, Vector2 range)
        {
            this.value = value;
            this.min = range.x;
            this.max = range.y;
        }

        


        public Vector2 GetRange()
        {
            return new Vector2(min, max);
        }

        public float GetValue()
        {
            return value;
        }

        public void SetValue(float v)
        {
            value = Mathf.Clamp(value, min, max);
        }
    }

    #endregion

    #region ShaderRanges

    [Serializable]
    private class RangeProperties
    {
        [SerializeField] public List<ColorRange> colors = new List<ColorRange>();
        [SerializeField] public List<VectorRange> vectors = new List<VectorRange>();
        [SerializeField] public List<FloatRange> floats = new List<FloatRange>();
        [SerializeField] public List<RangeRange> ranges = new List<RangeRange>();
        [SerializeField] public List<Texture> texs = new List<Texture>();
        [SerializeField] public List<IntRange> ints = new List<IntRange>();

        /// <summary>
        /// Generate the random range structures using the
        /// given properties object as a blueprint 
        /// </summary>
        /// <param name="properties"></param>
        public RangeProperties(ShaderProperties properties)
        {
            print(properties);
            foreach (Tuple<string, ShaderPropertyType> tuple in properties.nameAndType)
            {
                string name = tuple.Item1;
                int index = properties.nameToIndex[name];

                switch (tuple.Item2)
                {
                    case ShaderPropertyType.Color:
                        colors.Add(new ColorRange(name));
                        break;
                    case ShaderPropertyType.Vector:
                        vectors.Add(new VectorRange(name));
                        break;
                    case ShaderPropertyType.Float:
                        floats.Add(new FloatRange(name));
                        break;
                    case ShaderPropertyType.Range:
                        ranges.Add(new RangeRange(name, properties.ranges[index].range.GetRange()));
                        break;
                    case ShaderPropertyType.Texture:
                        // Just copy over the whole list at once later 
                        break;
                    case ShaderPropertyType.Int:
                        ints.Add(new IntRange(name));
                        break;
                    default:
                        Debug.LogWarning("Invalid ShaderPropertyType: " + tuple.Item2);
                        break;
                }
            }
            texs = properties.texs;
        }


        /// <summary>
        /// Generates a ShaderProperties object with random values
        /// based on the ranges in this object 
        /// </summary>
        /// <returns></returns>
        public ShaderProperties GenerateSP(ShaderProperties bluePrint)
        {
            ShaderProperties sp = new ShaderProperties();

            // Safety check 
            if(bluePrint == null)
            {
                Debug.LogError("No Properties Blueprint");
                return null;
            }
            
            
            for(int i = 0; i < bluePrint.nameAndType.Count; i++)
            {
                string name = bluePrint.nameAndType[i].Item1;
                ShaderPropertyType type = bluePrint.nameAndType[i].Item2;
                int index = bluePrint.nameToIndex[name];


                Tuple<string, ShaderPropertyType> nameAndType =
                    new Tuple<string, ShaderPropertyType>(name, type);
                sp.nameAndType.Add(nameAndType);

                switch (type)
                {
                    case ShaderPropertyType.Color:
                        sp.nameToIndex.Add(name, sp.colors.Count);
                        ColorHelper colorHelper = new ColorHelper(name);
                        colorHelper.color = colors[index].gradient.Evaluate(UnityEngine.Random.Range(0.0f, 1.0f));
                        sp.colors.Add(colorHelper);
                        break;
                    case ShaderPropertyType.Vector:
                        sp.nameToIndex.Add(name, sp.vectors.Count);
                        VectorHelper vectorHelper = new VectorHelper(name);
                        vectorHelper.vector = RandVec(vectors[index].minRange, vectors[index].maxRange);
                        sp.vectors.Add(vectorHelper);
                        break;
                    case ShaderPropertyType.Float:
                        sp.nameToIndex.Add(name, sp.floats.Count);
                        FloatHelper floatHelper = new FloatHelper(name);
                        floatHelper.value = UnityEngine.Random.Range(floats[index].range.x, floats[index].range.y);
                        sp.floats.Add(floatHelper);
                        break;
                    case ShaderPropertyType.Range:
                        sp.nameToIndex.Add(name, sp.ranges.Count);
                        RangeHelper rangeHelper = new RangeHelper(name);
                        rangeHelper.range = new SFloatRange(
                                UnityEngine.Random.Range(ranges[index].range.x, ranges[index].range.y),
                                ranges[index].range);
                        sp.ranges.Add(rangeHelper);
                        break;
                    case ShaderPropertyType.Texture:
                        break;
                    case ShaderPropertyType.Int:
                        sp.nameToIndex.Add(name, sp.ints.Count);
                        IntHelper intHelper = new IntHelper(name);
                        intHelper.value = UnityEngine.Random.Range(ints[index].range.x, ints[index].range.y);
                        sp.ints.Add(intHelper);
                        break; 
                }
            }

            sp.texs = texs;

            return sp;
        }
    }

    [Serializable]
    private class ColorRange
    {
        [SerializeField] public string colorName;
        [SerializeField] public Gradient gradient = new Gradient();

        public ColorRange(string name)
        {
            colorName = name;
        }
    }

    [Serializable]
    private class VectorRange
    {
        [SerializeField] public string vectorName;
        [SerializeField] public Vector3 minRange, maxRange;

        public VectorRange(string name)
        {
            vectorName = name;
        }
    }

    [Serializable]
    private class FloatRange
    {
        [SerializeField] public string floatName;
        [SerializeField] public Vector2 range;

        public FloatRange(string name)
        {
            floatName = name;
        }
    }

    [Serializable]
    private class RangeRange // :3 
    {
        [SerializeField] public string rangeName;
        [SerializeField] public Vector2 range;

        public RangeRange(string name, Vector2 range)
        {
            rangeName = name;
            this.range = range;
        }
    }

    [Serializable]
    private class IntRange
    {
        [SerializeField] public string intName;
        [SerializeField] public Vector2Int range;

        public IntRange(string name)
        {
            intName = name;
        }
    }

    #endregion


    private static Vector4 RandVec(Vector4 min, Vector4 max)
    {
        return new Vector4
            (
                UnityEngine.Random.Range(min.x, max.x),
                UnityEngine.Random.Range(min.y, max.y),
                UnityEngine.Random.Range(min.z, max.z),
                UnityEngine.Random.Range(min.w, max.w)
            );
    }
}
