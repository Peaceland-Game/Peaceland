/*
Mostly copied from https://github.com/JimmyCushnie/Noisy-Nodes/tree/master,
Voronoi3D contributed by @fdervaux.

Also read through this explanation to better understand Voronoi:
https://thebookofshaders.com/12/ by Patricio Gonzales Vivo

William Duprey
6/18/24
Voronoi3D Noise
*/

/// <summary>
/// Deterministic random function (always produces same output given same input).
/// Inspired from 2D deterministic function here: https://thebookofshaders.com/10/.
/// All values used in calculations here are arbitrary.
/// </summary>
/// <param name="seed">Vector3 used to seed the random function.</param>
inline float3 random_vector3(float3 UV, float offset)
{
	// 3x3 matrix of arbitrary values
	float3x3 m = float3x3(
		15.27, 47.63, 99.41,
		89.98, 95.07, 38.39,
		33.83, 51.06, 60.77
	);
	
	// Matrix multiplication with m (resulting back in a vector3),
	// sined, and fractional component taken from that.
	// Essentially, arbitrarily random steps
	UV = frac(sin(mul(UV, m)) * 46839.32);

	// Construct a new vector3 and scale it by the angle offset
	return float3(
		sin(UV.y * +offset) * 0.5 + 0.5,
		cos(UV.x * offset) * 0.5 + 0.5,
		sin(UV.z * offset) * 0.5 + 0.5);
}

/// <summary>
/// Does Voronoi stuff. To be frank, most of this is copied from the Noisy-Nodes
/// code. I just changed some of the formatting and added comments.
/// I generally understand what's going on, but some stuff I don't get.
/// 
/// Also, something fun I learned is that the Graph Settings Precision determines
/// what this function should be named. When set to Single precision, if this function
/// doesn't have "_float" at the end, it won't work. The Unity generated shader code
/// assumes the function is named the name of the Custom Function node, 
/// with "_float" at the end.
/// </summary>
void Voronoi3D_float(float3 UV, float AngleOffset, float CellDensity, 
	out float Out, out float Cells)
{
	// Tile the space based on the given CellDensity
	float3 g = floor(UV * CellDensity);
	float3 f = frac(UV * CellDensity);

	// Vector3 used for storing minimum distances while calculating, and for
	// reporting the final calculations.
	// res.x for regular output, res.y for ???, res.z for Cells output
	float3 res = float3(8.0, 8.0, 8.0);

	// Loop through each neighboring tile
	for (int y = -1; y <= 1; y++) {
		for (int x = -1; x <= 1; x++) {
			for (int z = -1; z <= 1; z++)
			{
				float3 lattice = float3(x, y, z);
				float3 offset = random_vector3(g + lattice, AngleOffset);
				float3 v = lattice + offset - f;
				
				// Dot product of a vector with itself is equal to its magnitude
				float d = dot(v, v);

				// TODO: Wrap my head around this
				if (d < res.x)
				{
					res.y = res.x;
					res.x = d;
					res.z = offset.x;
				}
				else if (d < res.y)
				{
					res.y = d;
				}
			}
		}
	}

	Out = res.x;
	Cells = res.z;
}