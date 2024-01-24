using UnityEngine;
using UnityEditor;
using TMPro;
using UnityEngine.UI;

public class InputsNavigationSetter : EditorWindow
{
    [Tooltip("Put the inputs in the same order as you have in your scene")]
    public TMP_InputField[] inputsToNavigate;
    SerializedObject serObj;

    [MenuItem("Futura/Inputs Navigation Setter")]
    public static void ShowWindow(){
        GetWindow(typeof(InputsNavigationSetter));
    }

    private void OnEnable() {
        ScriptableObject target = this;
        serObj = new SerializedObject(target);
    }

    private void OnGUI() {
        serObj.Update();
        GUILayout.Label("Set Input Navigation", EditorStyles.boldLabel);
        SerializedProperty inputsProperty = serObj.FindProperty("inputsToNavigate");
        EditorGUILayout.PropertyField(inputsProperty, true);
        serObj.ApplyModifiedProperties();

        if(GUILayout.Button("Set Navigation"))
            SetInputsNavigation();
    }

    private void SetInputsNavigation() {
        Navigation nav = new Navigation();
        nav.mode = Navigation.Mode.Explicit;

        for(int i = 0; i<inputsToNavigate.Length; i++){
            if(i == inputsToNavigate.Length-1){
                nav.selectOnDown = inputsToNavigate[0];
            } else {
                if(i == 0)
                    nav.selectOnUp = inputsToNavigate[inputsToNavigate.Length-1];
                else
                    nav.selectOnUp = inputsToNavigate[i-1];

                nav.selectOnDown = inputsToNavigate[i+1];
            }

            inputsToNavigate[i].navigation = nav;
        }        
    }
}