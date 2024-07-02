using System;
using System.Collections.Generic;
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
                        colorHelper.color = (UnityEngine.Color)shader.GetPropertyDefaultVectorValue(i);
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

    /// <summary>
    /// Resets all the ranges to mimic the blueprint and adds any missing ranges if needed. Will remove any
    /// created ranges so use with caution
    /// </summary>
    public void OverrideRanges()
    {
        // Create ranges object 
        propertiesRanges = new RangeProperties(propertiesBP);
    }

    /// <summary>
    /// Go through each property range and choose a random value between those
    /// given values 
    /// </summary>
    public void GenerateRandomProperties()
    {
        if (propertiesRanges == null)
            return;

        generatedProperties = new List<ShaderProperties>();
        
        if(propertiesBP == null)
            LoadProperties();

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

    /// Used to to help shader property edits logic in different scripts 
    #region LogicHelpers

    /// <summary>
    /// If a properties was created outside this script use this
    /// function to finalize it 
    /// </summary>
    /// <param name="properties"></param>
    public static void GeneratePropertyHelpers(ShaderProperties properties, Shader shader)
    {
        Debug.LogWarning("Using deprecated GeneratePropertyHelpers function. Try calling directly from the properties instead");

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
            string name = shader.GetPropertyName(i);
            ShaderPropertyType type = shader.GetPropertyType(i);

            Tuple<string, ShaderPropertyType> nameAndType =
                                new Tuple<string, ShaderPropertyType>(name, type);

            properties.nameAndType.Add(nameAndType);
            properties.nameToType.Add(name, type);

            // Keep track of property indicies in their
            // respective lists
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

    /// <summary>
    /// Interpolates between two shader properties. Resulting shader properties
    /// will only include properties included in both parameters. Make sure that
    /// property helpers have been generated before calling this function 
    /// </summary>
    /// <param name="propertiesA"></param>
    /// <param name="propertiesB"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    public static ShaderProperties InterpolateProperties(ShaderProperties propertiesA, ShaderProperties propertiesB, float t)
    {
        ShaderProperties properties = new ShaderProperties();

        for(int i = 0; i < propertiesA.nameAndType.Count; i++)
        {
            Tuple<string, ShaderPropertyType> pair = propertiesA.nameAndType[i];

            string name = pair.Item1;
            int indexA, indexB;

            indexA = propertiesA.nameToIndex[name];
            indexB = propertiesB.nameToIndex[name];

            // Check if both indicies exist. Following will NOT
            // work if both properties contain the same name.

            // TODO: Fix bug if two variables have same name but
            //       different types 
            

            // Find correct type and interpolate 
            switch (pair.Item2)
            {
                case ShaderPropertyType.Color:

                    UnityEngine.Color colorA = propertiesA.colors[indexA].color;
                    UnityEngine.Color colorB = propertiesB.colors[indexB].color;

                    ColorHelper cHelper = new ColorHelper(name);
                    cHelper.color = UnityEngine.Color.Lerp(colorA, colorB, t);
                    properties.colors.Add(cHelper);

                    break;
                case ShaderPropertyType.Vector:

                    Vector4 vecA = propertiesA.vectors[indexA].vector;
                    Vector4 vecB = propertiesB.vectors[indexB].vector;

                    VectorHelper vHelper = new VectorHelper(name);
                    vHelper.vector = Vector4.Lerp(vecA, vecB, t);
                    properties.vectors.Add(vHelper);

                    break;
                case ShaderPropertyType.Float:

                    float fA = propertiesA.floats[indexA].value;
                    float fB = propertiesB.floats[indexB].value;

                    FloatHelper fHelper = new FloatHelper(name);
                    fHelper.value = Mathf.Lerp(fA, fB, t);
                    properties.floats.Add(fHelper);

                    break;
                case ShaderPropertyType.Range:
                    SFloatRange rA = propertiesA.ranges[indexA].range;
                    SFloatRange rB = propertiesB.ranges[indexB].range;

                    // Maximize range using smallest min and largest max
                    // Average the value 

                    RangeHelper rHelper = new RangeHelper(name);
                    SFloatRange range = new SFloatRange(
                        Mathf.Lerp(rA.GetValue(), rB.GetValue(), t),
                        rA.GetRange().x < rB.GetRange().x ? rA.GetRange().x : rB.GetRange().x, // Get lower min 
                        rA.GetRange().y > rB.GetRange().y ? rA.GetRange().y : rB.GetRange().y); // Get higher max 

                    rHelper.range = range;
                    properties.ranges.Add(rHelper);

                    break;
                case ShaderPropertyType.Texture:

                    // Choose texture based on which part t is closer to
                    float value = Mathf.Round(Mathf.Clamp01(t));

                    if((int)value == 0)
                    {
                        
                    }
                    else
                    {

                    }

                    Debug.LogError("Texture interpolating not yet implemented");

                    break;
                case ShaderPropertyType.Int:

                    int iA = propertiesA.ints[indexA].value;
                    int iB = propertiesB.ints[indexB].value;

                    IntHelper iHelper = new IntHelper(name);
                    iHelper.value = (int)Mathf.Lerp((float)iA, (float)iB, t);
                    properties.ints.Add(iHelper);

                    break;
                default:
                    Debug.LogWarning("Invalid ShaderPropertyType: " + pair.Item2);
                    break;
            }
        }

        return properties;
    }

    #endregion

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

        /// <summary>
        /// Generates the proper helper variables using current helper
        /// variables that are attached to this object 
        /// </summary>
        public void GeneratePropertyHelpers()
        {
            nameAndType = new List<Tuple<string, ShaderPropertyType>>();
            nameToType = new Dictionary<string, ShaderPropertyType>();
            nameToIndex = new Dictionary<string, int>();

            // Add colors 
            for (int i = 0; i < colors.Count; i++)
            {
                nameAndType.Add(new Tuple<string, ShaderPropertyType>(
                    colors[i].name,
                    ShaderPropertyType.Color));
                if (nameToType.ContainsKey(colors[i].name))
                    print(colors[i].name);
                nameToType.Add(colors[i].name, ShaderPropertyType.Color);

                
                nameToIndex[colors[i].name] = i;
            }

            // Add vectors 
            for (int i = 0; i < vectors.Count; i++)
            {
                nameAndType.Add(new Tuple<string, ShaderPropertyType>(
                    vectors[i].name,
                    ShaderPropertyType.Vector));
                nameToType.Add(vectors[i].name, ShaderPropertyType.Vector);

                nameToIndex[vectors[i].name] = i;
            }

            // Add floats 
            for (int i = 0; i < floats.Count; i++)
            {
                nameAndType.Add(new Tuple<string, ShaderPropertyType>(
                    floats[i].name,
                    ShaderPropertyType.Float));
                nameToType.Add(floats[i].name, ShaderPropertyType.Float);

                nameToIndex[floats[i].name] = i;
            }

            // Add ranges 
            for (int i = 0; i < ranges.Count; i++)
            {
                nameAndType.Add(new Tuple<string, ShaderPropertyType>(
                    ranges[i].name,
                    ShaderPropertyType.Range));
                nameToType.Add(ranges[i].name, ShaderPropertyType.Range);

                nameToIndex[ranges[i].name] = i;
            }

            // Add textures 
            for (int i = 0; i < texs.Count; i++)
            {
                nameAndType.Add(new Tuple<string, ShaderPropertyType>(
                    texs[i].name,
                    ShaderPropertyType.Texture));
                nameToType.Add(texs[i].name, ShaderPropertyType.Texture);

                nameToIndex[texs[i].name] = i;
            }

            // Add ints 
            for (int i = 0; i < ints.Count; i++)
            {
                nameAndType.Add(new Tuple<string, ShaderPropertyType>(
                    ints[i].name,
                    ShaderPropertyType.Int));
                nameToType.Add(ints[i].name, ShaderPropertyType.Int);

                nameToIndex[floats[i].name] = i;
            }
        }
    }

    [Serializable]
    public class VariableHelper
    {
        [SerializeField] public string name;
    }

    [Serializable]
    public class ColorHelper : VariableHelper
    {
        [SerializeField] public UnityEngine.Color color = new UnityEngine.Color();

        public ColorHelper(string name)
        {
            this.name = name;
        }
    }

    [Serializable]
    public class VectorHelper : VariableHelper
    {
        [SerializeField] public Vector3 vector;

        public VectorHelper(string name)
        {
            this.name = name;
        }
    }

    [Serializable]
    public class FloatHelper : VariableHelper
    {
        [SerializeField] public float value;

        public FloatHelper(string name)
        {
            this.name = name;
        }
    }

    [Serializable]
    public class RangeHelper : VariableHelper // :3 
    {
        [SerializeField] public SFloatRange range;

        public RangeHelper(string name)
        {
            this.name = name;
        }
    }

    [Serializable]
    public class IntHelper : VariableHelper
    {
        [SerializeField] public int value;

        public IntHelper(string name)
        {
            this.name = name;
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
