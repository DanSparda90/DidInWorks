using UnityEngine;

/// <summary>
/// Use this attribute to put names on each element of an array or list and see them in the editor
/// </summary>
public class NamedArrayAttribute : PropertyAttribute
{
	public readonly string[] names;
	public NamedArrayAttribute(string[] names) { this.names = names; }
}