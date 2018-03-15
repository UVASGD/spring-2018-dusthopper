using UnityEngine;

[System.Serializable]
class SerialVec3
{
	float x;
	float y;
	float z;

	public SerialVec3(float p_x, float p_y, float p_z)
	{
		this.x = p_x;
		this.y = p_y;
		this.z = p_z;
	}

	public static Vector3 convFrom(SerialVec3 that)
	{
		return new Vector3(that.x, that.y, that.z);
	}

	public static SerialVec3 convTo(Vector3 that)
	{
		return new SerialVec3(that.x, that.y, that.z);
	}
}