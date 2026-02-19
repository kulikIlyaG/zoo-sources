using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Utilities.UnityExtensions.Editor
{
    [CustomPropertyDrawer(typeof(SReferenceView))]
    public sealed class SReferenceViewDrawer : PropertyDrawer
    {
        private const float VerticalSpacing = 2f;

        private static readonly Dictionary<Type, List<Type>> DerivedTypesCache = new();

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = EditorGUIUtility.singleLineHeight;

            if (!property.isExpanded)
            {
                return height;
            }

            height += GetChildrenHeight(property);

            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.ManagedReference)
            {
                EditorGUI.HelpBox(position, $"'{label.text}' must be a [SerializeReference] field.", MessageType.Error);
                return;
            }

            Rect lineRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            Rect foldRect = new Rect(lineRect.x, lineRect.y, lineRect.width - 90f, lineRect.height);
            Rect buttonRect = new Rect(lineRect.xMax - 84f, lineRect.y, 84f, lineRect.height);

            string selectedType = GetShortTypeName(property.managedReferenceFullTypename);
            DrawFoldoutHeader(foldRect, property, label, selectedType);

            if (property.managedReferenceValue == null)
            {
                if (GUI.Button(buttonRect, "Select"))
                {
                    ShowTypeSelector(property);
                }
            }
            else
            {
                if (GUI.Button(buttonRect, "Clear"))
                {
                    property.serializedObject.Update();
                    property.managedReferenceValue = null;
                    property.serializedObject.ApplyModifiedProperties();
                }
            }

            if (!property.isExpanded)
            {
                return;
            }

            float y = lineRect.yMax + VerticalSpacing;
            DrawChildren(position, property, y);
        }

        private static void DrawFoldoutHeader(Rect foldRect, SerializedProperty property, GUIContent label, string selectedType)
        {
            Rect arrowRect = new Rect(foldRect.x, foldRect.y, 14f, foldRect.height);
            property.isExpanded = EditorGUI.Foldout(arrowRect, property.isExpanded, GUIContent.none, true);

            Rect textRect = new Rect(foldRect.x, foldRect.y, foldRect.width - arrowRect.width, foldRect.height);
            string typeLabel = $"({selectedType})";

            Vector2 mainLabelSize = EditorStyles.label.CalcSize(new GUIContent(label.text));
            Rect mainLabelRect = new Rect(textRect.x, textRect.y, mainLabelSize.x, textRect.height);
            EditorGUI.LabelField(mainLabelRect, label, EditorStyles.label);

            Rect typeLabelRect = new Rect(mainLabelRect.xMax + 4f, textRect.y, textRect.width - mainLabelSize.x - 4f, textRect.height);
            EditorGUI.LabelField(typeLabelRect, typeLabel, EditorStyles.miniBoldLabel);
        }

        private static float GetChildrenHeight(SerializedProperty property)
        {
            if (property.managedReferenceValue == null)
            {
                return 0f;
            }

            float height = 0f;
            SerializedProperty iterator = property.Copy();
            SerializedProperty end = iterator.GetEndProperty();

            bool enterChildren = true;
            while (iterator.NextVisible(enterChildren) && !SerializedProperty.EqualContents(iterator, end))
            {
                height += EditorGUI.GetPropertyHeight(iterator, true) + VerticalSpacing;
                enterChildren = false;
            }

            return height;
        }

        private static void DrawChildren(Rect totalRect, SerializedProperty property, float startY)
        {
            if (property.managedReferenceValue == null)
            {
                return;
            }

            SerializedProperty iterator = property.Copy();
            SerializedProperty end = iterator.GetEndProperty();

            EditorGUI.indentLevel++;
            bool enterChildren = true;
            float y = startY;
            while (iterator.NextVisible(enterChildren) && !SerializedProperty.EqualContents(iterator, end))
            {
                float childHeight = EditorGUI.GetPropertyHeight(iterator, true);
                Rect childRect = new Rect(totalRect.x, y, totalRect.width, childHeight);
                EditorGUI.PropertyField(childRect, iterator, true);
                y += childHeight + VerticalSpacing;
                enterChildren = false;
            }
            EditorGUI.indentLevel--;
        }

        private void ShowTypeSelector(SerializedProperty property)
        {
            GenericMenu menu = new GenericMenu();
            List<Type> types = GetAssignableTypes(GetBaseType(property));

            foreach (Type type in types)
            {
                bool isSelected = property.managedReferenceValue?.GetType() == type;
                menu.AddItem(new GUIContent(type.Name), isSelected, () => AssignNewInstance(property, type));
            }

            menu.AddSeparator(string.Empty);
            menu.AddItem(new GUIContent("Null"), property.managedReferenceValue == null, () =>
            {
                property.serializedObject.Update();
                property.managedReferenceValue = null;
                property.serializedObject.ApplyModifiedProperties();
            });

            menu.ShowAsContext();
        }

        private static void AssignNewInstance(SerializedProperty property, Type type)
        {
            property.serializedObject.Update();
            property.managedReferenceValue = Activator.CreateInstance(type);
            property.isExpanded = true;
            property.serializedObject.ApplyModifiedProperties();
            property.serializedObject.Update();
        }

        private List<Type> GetAssignableTypes(Type baseType)
        {
            if (baseType == null)
            {
                return new List<Type>();
            }

            if (DerivedTypesCache.TryGetValue(baseType, out List<Type> cached))
            {
                return cached;
            }

            SReferenceView attribute = (SReferenceView)this.attribute;
            List<Type> types = TypeCache.GetTypesDerivedFrom(baseType)
                .Where(CanBeAssigned)
                .OrderBy(t => t.FullName)
                .ToList();

            if (attribute.IncludeBaseType && CanBeAssigned(baseType))
            {
                types.Insert(0, baseType);
            }

            DerivedTypesCache[baseType] = types;
            return types;
        }

        private static bool CanBeAssigned(Type type)
        {
            if (type == null)
            {
                return false;
            }

            if (type.IsAbstract || type.IsInterface || type.IsGenericTypeDefinition)
            {
                return false;
            }

            if (typeof(UnityEngine.Object).IsAssignableFrom(type))
            {
                return false;
            }

            return type.IsSerializable;
        }

        private Type GetBaseType(SerializedProperty property)
        {
            Type managedReferenceType = ResolveManagedReferenceFieldType(property);
            if (managedReferenceType != null)
            {
                return managedReferenceType;
            }

            return ResolveElementType(fieldInfo.FieldType);
        }

        private static Type ResolveManagedReferenceFieldType(SerializedProperty property)
        {
            string fullTypeName = property.managedReferenceFieldTypename;
            if (string.IsNullOrWhiteSpace(fullTypeName))
            {
                return null;
            }

            string[] parts = fullTypeName.Split(' ');
            if (parts.Length != 2)
            {
                return null;
            }

            string assemblyName = parts[0];
            string typeName = parts[1];
            Type type = Type.GetType($"{typeName}, {assemblyName}");
            return ResolveElementType(type);
        }

        private static Type ResolveElementType(Type type)
        {
            if (type == null)
            {
                return null;
            }

            if (type.IsArray)
            {
                return type.GetElementType();
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                return type.GetGenericArguments()[0];
            }

            return type;
        }

        private static string GetShortTypeName(string fullTypeName)
        {
            if (string.IsNullOrEmpty(fullTypeName))
            {
                return "None";
            }

            string[] parts = fullTypeName.Split(' ');
            if (parts.Length != 2)
            {
                return fullTypeName;
            }

            string fullName = parts[1];
            int dot = fullName.LastIndexOf('.');
            return dot >= 0 ? fullName[(dot + 1)..] : fullName;
        }
    }
}
