using UnityEngine;

public static class TransformExtension
{
    public static void AllignWithUp(this Transform rotator, Vector3 targetDir, float maxAngleDist = 0.1f)
    {

        float angle = Vector3.SignedAngle(targetDir, rotator.up.normalized, Vector3.forward);
        float sign = Mathf.Sign(angle);

        if (Mathf.Abs(angle) < maxAngleDist) return;
        rotator.Rotate(new Vector3(0, 0, -Mathf.Abs(angle) * sign));

    }

	public static void AllignRightWith(this Transform rotator, Vector3 targetDir, float maxAngleDist = 0.1f)
	{

		float angle = Vector3.SignedAngle(targetDir, rotator.right.normalized, Vector3.forward);
		float sign = Mathf.Sign(angle);

		if (Mathf.Abs(angle) < maxAngleDist) return;
		rotator.Rotate(new Vector3(0, 0, -Mathf.Abs(angle) * sign));

	}

	public static void RotateRightTowards(this Transform rotator, Vector3 targetDir, float singleStep = 1.0f)
	{
		Vector3 next = Vector3.RotateTowards(rotator.right, targetDir, singleStep, 0.0f);
		AllignRightWith(rotator, next);
	}

}
