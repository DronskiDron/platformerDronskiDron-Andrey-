﻿using UnityEditor;
using Utils.Editor;

namespace General.Components.Dialogs.Editor
{
    [CustomEditor(typeof(ShowDialogComponent))]
    public class ShowDialogComponentEditor : UnityEditor.Editor
    {
        private SerializedProperty _modeProperty;
        private SerializedProperty _onCompleteProperty;
        private SerializedProperty _oneSentenceModProperty;


        private void OnEnable()
        {
            _modeProperty = serializedObject.FindProperty("_mode");
            _onCompleteProperty = serializedObject.FindProperty("_onComplete");
            _oneSentenceModProperty = serializedObject.FindProperty("_oneSentenceMod");
        }


        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(_modeProperty);

            if (_modeProperty.GetEnum(out ShowDialogComponent.Mode mode))
            {
                switch (mode)
                {
                    case ShowDialogComponent.Mode.Bound:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_bound"));
                        break;
                    case ShowDialogComponent.Mode.External:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_external"));
                        break;
                }
            }

            EditorGUILayout.PropertyField(_onCompleteProperty);
            EditorGUILayout.PropertyField(_oneSentenceModProperty);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
