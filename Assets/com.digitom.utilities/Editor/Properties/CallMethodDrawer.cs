using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Reflection;
namespace DigitomUtilities
{

    [CustomPropertyDrawer(typeof(CallMethod), true)]
    public class CallMethodDrawer : NgnProperyDrawer
    {
        protected Dictionary<int, WrapperValue<bool>> isReturns = new Dictionary<int, WrapperValue<bool>>();
        protected Dictionary<int, WrapperValue<Object>> lastObjs = new Dictionary<int, WrapperValue<Object>>();
        protected Dictionary<int, WrapperValue<int>> lastComps = new Dictionary<int, WrapperValue<int>>();
        protected Dictionary<int, WrapperValue<int>> lastMethods = new Dictionary<int, WrapperValue<int>>();
        protected Dictionary<int, WrapperValue<MethodInfo[]>> methodsContainer = new Dictionary<int, WrapperValue<MethodInfo[]>>();
        protected Dictionary<int, WrapperValue<ParameterInfo[]>> methodParamsContainer = new Dictionary<int, WrapperValue<ParameterInfo[]>>();
        protected Dictionary<int, WrapperValue<string[]>> methodNamesContainer = new Dictionary<int, WrapperValue<string[]>>();

        protected override void Initialize(SerializedProperty property, int index)
        {
            var targetObject = property.FindPropertyRelative("targetObject");
            var methodInd = property.FindPropertyRelative("methodInd");
            var componentInd = property.FindPropertyRelative("componentInd");
            var lastObj = lastObjs.GetOrAddValue(index);
            var lastComp = lastComps.GetOrAddValue(index);
            var lastMethod = lastMethods.GetOrAddValue(index);
            lastObj.Value = targetObject.objectReferenceValue;
            lastComp.Value = componentInd.intValue;
            lastMethod.Value = methodInd.intValue;

        }

        protected override float SetPropertyHeight(SerializedProperty property, GUIContent label, int index)
        {
            //set height
            propertyHeight = lineHeight;
            var targetObject = property.FindPropertyRelative("targetObject");
            if (targetObject.objectReferenceValue)
            {
                if (property.isExpanded)
                {
                    propertyHeight += space * 2;
                    var parameters = property.FindPropertyRelative("parameters");
                    if (parameters.arraySize > 0)
                    {
                        //params adjust
                        propertyHeight += space;
                        for (int i = 0; i < parameters.arraySize; i++)
                        {
                            var ele = parameters.GetArrayElementAtIndex(i);
                            propertyHeight += EditorGUI.GetPropertyHeight(ele, true) + spacing;
                        }

                        
                    }
                    //return adjust
                    var isReturn = isReturns.GetOrAddValue(index);
                    if (isReturn.Value)
                    {
                        propertyHeight += space;
                        var rv = property.FindPropertyRelative("methodReturnValue");
                        propertyHeight += EditorGUI.GetPropertyHeight(rv, true);
                    }
                        
                }
            }
            return propertyHeight;
        }

        protected override void SetOnGUI(Rect position, SerializedProperty property, GUIContent label, int index)
        {
            var targetObject = property.FindPropertyRelative("targetObject");
            var methodInd = property.FindPropertyRelative("methodInd");
            var componentInd = property.FindPropertyRelative("componentInd");
            var parameters = property.FindPropertyRelative("parameters");

            var lastObj = lastObjs.GetOrAddValue(index);
            var lastComp = lastComps.GetOrAddValue(index);
            var lastMethod = lastMethods.GetOrAddValue(index);
            var methods = methodsContainer.GetOrAddValue(index);
            var methodNames = methodNamesContainer.GetOrAddValue(index);
            

            var methodParams = methodParamsContainer.GetOrAddValue(index);

            var pos = new Rect(position.x, position.y, position.width, lineHeight);
            EditorGUI.PropertyField(pos, targetObject, new GUIContent { text = property.displayName});

            if (targetObject.objectReferenceValue)
            {
                if (lastObj.Value != targetObject.objectReferenceValue)
                {
                    lastComp.Value = -1;
                    lastMethod.Value = -1;
                    lastObj.Value = targetObject.objectReferenceValue;
                    Debug.Log("object changed");
                }
                var style = EditorStyles.foldout;
                var dropRect = new Rect(position.x - 13 + (EditorGUI.indentLevel * 15), position.y, EditorGUIUtility.labelWidth, lineHeight);
                property.isExpanded = EditorGUI.Toggle(dropRect, property.isExpanded, style);

                if (property.isExpanded)
                {
                    EditorGUI.indentLevel++;

                    var serObj = new SerializedObject(targetObject.objectReferenceValue);
                    var go = serObj.targetObject as Component;

                    //cancel if not gameobject
                    if (go == null)
                    {
                        targetObject.objectReferenceValue = null;
                        return;
                    }

                    //component index
                    var components = go.GetComponents<Component>();
                    var types = new System.Type[components.Length + 1];
                    types[0] = typeof(GameObject);
                    for (int i = 0; i < components.Length; i++)
                    {
                        types[i + 1] = components[i].GetType();
                    }
                    var typesNames = types.Select(x => x.Name).ToArray();

                    //component field
                    pos.y += space;
                    componentInd.intValue = EditorGUI.Popup(pos, "Component", componentInd.intValue, typesNames);
                    componentInd.intValue = Mathf.Clamp(componentInd.intValue, 0, typesNames.Length - 1);

                    //redo methods name lists if component selections have changed
                    if (lastComp.Value != componentInd.intValue || methodNames.Value == null)
                    {
                        
                        //method index
                        methods.Value = types[componentInd.intValue].GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                            .OrderBy(x => x.Name).ToArray();
                        methodNames.Value = new string[methods.Value.Length];
                        for (int i = 0; i < methods.Value.Length; i++)
                        {
                            methodNames.Value[i] = methods.Value[i].Name;
                            var mParams = methods.Value[i].GetParameters();
                            for (int j = 0; j < mParams.Length; j++)
                            {
                                if (j == 0)
                                    methodNames.Value[i] += " (";
                                methodNames.Value[i] += mParams[j].ParameterType.Name;
                                if (j < mParams.Length - 1)
                                    methodNames.Value[i] += ",";
                                else
                                    methodNames.Value[i] += ")";
                            }
                        }

                        if (lastComp.Value != componentInd.intValue)
                        {
                            Debug.Log("component changed");
                            lastMethod.Value = -1;
                            lastComp.Value = componentInd.intValue;
                        }  
                    }
                    
                    //method field
                    pos.y += space;
                    methodInd.intValue = EditorGUI.Popup(pos, "Method", methodInd.intValue, methodNames.Value);
                    methodInd.intValue = Mathf.Clamp(methodInd.intValue, 0, methodNames.Value.Length - 1);
                    var method = methods.Value[methodInd.intValue];

                    //redo parameters lists if method selections have changed
                    if (lastMethod.Value != methodInd.intValue || methodParams.Value == null)
                    {
                        

                        //clear property array
                        if(lastMethod.Value != methodInd.intValue)
                        {
                            Debug.Log("method changed");
                            parameters.ClearArray();
                            lastMethod.Value = methodInd.intValue;
                        }
                        

                        //populate parameter fields
                        methodParams.Value = methods.Value[methodInd.intValue].GetParameters();
                        if (parameters.arraySize < 1 && methodParams.Value.Length > 0)
                        {
                            for (int i = 0; i < methodParams.Value.Length; i++)
                            {
                                var type = methodParams.Value[i].ParameterType;
                                parameters.InsertArrayElementAtIndex(parameters.arraySize);
                                var ele = parameters.GetArrayElementAtIndex(parameters.arraySize - 1);
                                ////Create for serialize reference when it works... :/
                                //var obj = FactoryReferenceValue.CreateReferenceValueObject(type);
                                //Debug.Log(obj);
                                //ele.managedReferenceValue = obj;
                                //serializedTarget.ApplyModifiedPropertiesWithoutUndo();
                                var name = ele.FindPropertyRelative("name");
                                var valueType = ele.FindPropertyRelative("valueType");
                                name.stringValue = methodParams.Value[i].Name;
                                valueType.stringValue = type.AssemblyQualifiedName;
                            }
                        }

                        //return property
                        if (method.ReturnType != typeof(void) && !method.ReturnType.IsGenericParameter)
                        {
                            ////Create for serialize reference when it works... :/
                            //var rv = property.FindPropertyRelative("methodReturnValue");
                            //rv.managedReferenceValue = FactoryReferenceValue.CreateReferenceValueObject(method.ReturnType);
                            //serializedTarget.ApplyModifiedPropertiesWithoutUndo();
                        }
                        serializedTarget.ApplyModifiedProperties();
                    }

                    if (methodParams.Value.Length < 1) return;
                    pos.y += space;
                    //parameters array field
                    if (parameters.arraySize > 0)
                    {
                        EditorGUI.LabelField(pos, "Parameters:");
                        EditorGUI.indentLevel++;
                        pos.y += space;
                        for (int i = 0; i < parameters.arraySize; i++)
                        {
                            var ele = parameters.GetArrayElementAtIndex(i);
                            EditorGUI.PropertyField(pos, ele, new GUIContent { text = methodParams.Value[i].Name }, true);
                            pos.y += EditorGUI.GetPropertyHeight(ele, true) + spacing;
                        }
                        EditorGUI.indentLevel--;
                    }



                    //return Type field
                    var isReturn = isReturns.GetOrAddValue(index);
                    isReturn.Value = method.ReturnType != typeof(void) && !method.ReturnType.IsGenericParameter;
                    if (isReturn.Value)
                    {
                        
                        EditorGUI.LabelField(pos, "Return Value:");
                        pos.y += space;

                        EditorGUI.indentLevel++;
                        var methodReturnValue = property.FindPropertyRelative("methodReturnValue");
                        var name = methodReturnValue.FindPropertyRelative("name");
                        var valueType = methodReturnValue.FindPropertyRelative("valueType");
                        name.stringValue = method.ReturnType.Name;
                        valueType.stringValue = method.ReturnType.AssemblyQualifiedName;
                        
                        EditorGUI.PropertyField(pos, methodReturnValue, true);
                        pos.y += EditorGUI.GetPropertyHeight(methodReturnValue, true);

                        EditorGUI.indentLevel--;
                    }
                    
                    EditorGUI.indentLevel--;
                }
                
            }
            
        }

    }

}

