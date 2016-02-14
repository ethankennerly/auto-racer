using UnityEngine;

/**
 * Sharing code between projects:
 * Git repo or submodule.
 * Symlink on mac.
 * Copy code.
 * http://www.rivellomultimediaconsulting.com/symlinks-for-unity-game-development/
 */
public class ToyView
{
	/**
	 * Call animator.Play instead of animator.SetTrigger, in case the animator is in transition.
	 * Test case:  2015-11-15 Enter "SAT".  Type "RAT".  Expect R selected.  Got "R" resets to unselected.
	 * http://answers.unity3d.com/questions/801875/mecanim-trigger-getting-stuck-in-true-state.html
	 *
	 * Do not call until initialized.  Test case:  2015-11-15 Got warning "Animator has not been initialized"
	 * http://answers.unity3d.com/questions/878896/animator-has-not-been-initialized-1.html
	 *
	 * In editor, deleted and recreated animator state transition.  Test case:  2015-11-15 Got error "Transition '' in state 'selcted' uses parameter 'none' which is not compatible with condition type"
	 * http://answers.unity3d.com/questions/1070010/transition-x-in-state-y-uses-parameter-z-which-is.html
	 */
	public static void SetState(GameObject gameObject, string state, bool isRestart = true)
	{
		Animator animator = gameObject.GetComponent<Animator>();
		if (null != animator && animator.isInitialized)
		{
			// Debug.Log("ToyView.SetState: " + gameObject + ": " + state);
			if (isRestart)
			{
				animator.Play(state);
			}
			else
			{
				animator.Play(state, -1, 0f);
			}
		}
	}

	public static void SetPositionXZ(Transform transform, float x, float z)
	{
		Vector3 position = transform.position;
		position.x = x;
		position.z = z;
		transform.position = position;
	}

	public static void SetText(TextMesh mesh, string text)
	{
		mesh.text = text;
	}

	/**
	 * Test case:  2015-12-20 Level 8.  Blobo expects to feel challenged.  Felt overwhelmed (+zenmumbler, +Muel).
	 *	Tune difficulty and test in slow motion.
	 * Also update fixed delta time but not delta time.
	 * http://docs.unity3d.com/ScriptReference/Time-timeScale.html
	 */
	public static float UpdateCheatTimeScale()
	{
		float scale = 1.0f;
		float factor = Mathf.Pow(2.0f, 0.5f);
		if (Input.GetKeyDown("2"))
		{
			scale /= factor;
		}
		else if (Input.GetKeyDown("3"))
		{
			scale *= factor;
		}
		if (1.0f != scale)
		{
			Time.timeScale *= scale;
			Time.fixedDeltaTime *= scale;
			Debug.Log("ToyView.UpdateCheatTimeScale: to " + Time.timeScale);
		}
		return scale;
	}

	public static void UpdateCheat(Model model)
	{
		if (Input.GetKeyDown("page up"))
		{
			model.CheatLevelUp(1);
		}
		else if (Input.GetKeyDown("page down"))
		{
			model.CheatLevelUp(-1);
		}
		else if (Input.GetKeyDown("1"))
		{
			model.ToggleIsPerfectMode();
		}
		ToyView.UpdateCheatTimeScale();
	}
}
