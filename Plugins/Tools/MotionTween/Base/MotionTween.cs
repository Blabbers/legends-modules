using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

// CADA SO vai ter sua lista encadeada de motion.... ? Se for assim nao da pra pegar TARGET D=...
// talvez da pra fazer que no "Player:MonoBehaviour" SE houver um target-transform setado, o evento de target pode ser habilitado. Se nao tiver, simplesmente pula ele?
// Agora sobre os eventos, vao ficar dentro dos "Player:MonoBehaviour" mesmo...
// - OnShow, (no lugar do OnEnable, ideal é chamar essa funcao aqui, MAS tambem vou chamar ela assim que rodar o OnEnable mesmo)
// - OnHide,  (ideal é chamar isso ao invez de dar "SetActive(false)")
// - OnFirstTimeShown (pra casos que só acontece 1x)

namespace Blabbers.Game00
{
	public class MotionTween : ScriptableObject
	{
		[SerializeField] private List<TweenBehaviour> behaviours = new List<TweenBehaviour>();
		public void PlaySequence(MotionTweenPlayer tweenPlayer)
		{
			tweenPlayer.OnAnimationStart?.Invoke();

			tweenPlayer.StartCoroutine(Routine());
			IEnumerator Routine()
			{
				foreach (var behaviour in behaviours)
				{
					behaviour.Play(tweenPlayer);
					if (behaviour is WaitTweenBehaviour)
					{
						if (behaviour.duration > 0)
						{
							yield return new WaitForSecondsRealtime(behaviour.duration);
						}
					}
				}

				yield return null;
				tweenPlayer.OnAnimationFinished?.Invoke();

				if (tweenPlayer.isLoop)
				{
					tweenPlayer.PlayTween();
				}
			}
		}

		public float GetTotalDuration()
		{
			// sums ALL lists WAIT durations ONLY and returns it.
			// this is usefull for returning this value when calling feedback/warning that closes after X duration inside other separate code
			var sum = 0f;
			for (int i = 0; i < behaviours.Count; i++)
			{
				var item = behaviours[i];
				if (item is WaitTweenBehaviour)
				{
					sum += item.duration;
				}
			}
			return sum;
		}


#if UNITY_EDITOR
		[CustomEditor(typeof(MotionTween))]
		public class MotionTweenEditor : Editor
		{
			private ReorderableList list;
			private MotionTween self;

			private void OnEnable()
			{
				self = target as MotionTween;

				list = new ReorderableList(serializedObject,
						serializedObject.FindProperty(nameof(behaviours)),
						true, true, false, false);
				list.onRemoveCallback += OnRemove;
				//list.onChangedCallback += OnChangedCallback;


				list.drawElementCallback = DrawElements; // Delegate to draw the elements on the list
				list.drawHeaderCallback = DrawHeader; // Skip this line if you set displayHeader to 'false' in your ReorderableList constructor.

				list.elementHeightCallback = (index) =>
				{
					SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index); //The element in the list

					var typeName = element.objectReferenceValue.GetType().Name;
					var behaviourName = ObjectNames.NicifyVariableName(typeName.Replace("TweenBehaviour", ""));

					var height = EditorGUIUtility.singleLineHeight * 1.4f;
					//if (list.index == index && !behaviourName.Equals("Wait"))
					if (!behaviourName.Equals("Wait"))
					{
						Repaint();
						height = EditorGUIUtility.singleLineHeight * 5.1f;
					}

					return height;
				};
			}

			private void OnChangedCallback(ReorderableList list)
			{
				// CHAMA ISSO QUANDO Mudou ordem da lista aqui
			}

			public static void DrawUILine(Rect rect, Color color, int thickness = 2, int padding = 10)
			{
				//Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));


				rect.x -= 17f;
				rect.width += 17f;

				rect.height = thickness;
				rect.y += padding / 2;
				rect.x -= 2;
				rect.width += 6;
				EditorGUI.DrawRect(rect, color);
			}

			float heightMultiplier = 1.4f;

			// Draws the elements on the list
			void DrawElements(Rect rect, int index, bool isActive, bool isFocused)
			{
				SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index); //The element in the list

				var startRectHeight = rect.height;
				var y = 0f;

				var typeName = element.objectReferenceValue.GetType().Name;
				var behaviourName = ObjectNames.NicifyVariableName(typeName.Replace("TweenBehaviour", ""));

				if(index == 0)
				{
					var firstLineRect = rect;
					firstLineRect.y -= 8.5f;
					DrawUILine(firstLineRect, new Color(0.5f, 0.5f, 0.5f));
				}

				if (behaviourName.Equals("Wait"))
				{

					var oldY = rect.y;
					rect.y -= 5f;
					DrawUILine(rect, new Color(0.0f, 0.2f, 0.3f, 0.2f), 20);
					rect.y = oldY;

					EditorGUI.LabelField(new Rect(rect.x, rect.y + y, 150, EditorGUIUtility.singleLineHeight), behaviourName, EditorStyles.boldLabel);
					y += EditorGUIUtility.singleLineHeight + 5f;
				}
				else
				{
					EditorGUI.LabelField(new Rect(rect.x, rect.y + y, 150, EditorGUIUtility.singleLineHeight), "► " + behaviourName);
					y += EditorGUIUtility.singleLineHeight + 5f;
				}

				SerializedObject serObj = new SerializedObject(element.objectReferenceValue);
				SerializedProperty prop = serObj.GetIterator();
				prop.NextVisible(true);
				var propCount = 0f;
				while (prop.NextVisible(false))
				{
					if (behaviourName.Equals("Wait"))
					{
						// The "Wait" behaviour has a special drawn mode
						var height = EditorGUIUtility.singleLineHeight;
						// The property field for level. Since we do not need so much space in an int, width is set to 20, height of a single line.

						if (prop.name.Equals("duration"))
						{
							EditorGUI.PropertyField(
								new Rect(rect.x + 40, rect.y, 40, height),
								prop,
								GUIContent.none
							);
						}
					}
					else
					{
						//if (list.index == index)
						{
							// All other behaviours are just fully drawn
							var height = EditorGUIUtility.singleLineHeight;
							EditorGUI.LabelField(new Rect(rect.x + 10 + 10, rect.y + y, 150, height),
								ObjectNames.NicifyVariableName(prop.name));
							// The property field for level. Since we do not need so much space in an int, width is set to 20, height of a single line.
							EditorGUI.PropertyField(
								new Rect(rect.x + 140, rect.y + y, 200, height),
								prop,
								GUIContent.none
							);

							y += EditorGUIUtility.singleLineHeight + 5f;
						}
					}
					propCount += rect.height;
				}

				rect.y += y - 8f;
				DrawUILine(rect, new Color(0.5f, 0.5f, 0.5f));

				heightMultiplier = startRectHeight - rect.height;

				//EditorGUI.ObjectField(rect, element, new GUIContent(serializedObject.targetObject.name));
				//editor.DrawDefaultInspector();

				serObj.ApplyModifiedProperties();
				EditorUtility.SetDirty(serObj.targetObject);

			}


			//Draws the header
			void DrawHeader(Rect rect)
			{
				string name = "Motion Tween Sequence";
				EditorGUI.LabelField(rect, name);
			}

			private void OnRemove(ReorderableList list)
			{
				var isLastItem = list.index == self.behaviours.Count - 1;
				if (list.index >= 0)
				{
					var id = list.index;
					var obj = self.behaviours[id];
					obj.DestroyBehaviour();
					self.behaviours.RemoveAt(id);
				}
				if (isLastItem && self.behaviours.Count >= 0)
				{
					list.index = self.behaviours.Count - 1;
				}
			}

			public override void OnInspectorGUI()
			{
				// Update the array property's representation in the inspector
				serializedObject.Update();
				list.DoLayoutList();

				// We need to call this so that changes on the Inspector are saved by Unity.
				serializedObject.ApplyModifiedProperties();

				if (GUILayout.Button("Add"))
				{
					//int selected = 0;
					//string[] options = new string[]
					//{
					//	"Option1", "Option2", "Option3",
					//};
					//selected = EditorGUILayout.Popup("Label", selected, options);

					ProcessContextMenu();
				}

				if (GUILayout.Button("Remove"))
				{
					OnRemove(list);

				}

				//base.OnInspectorGUI();
			}

			private void ProcessContextMenu()
			{
				// Create the menu and add items to it
				GenericMenu menu = new GenericMenu();

				// Da pra generalizar isso aqui de forma linda
				// O que esse cara aqui fez: https://forum.unity.com/threads/custom-add-component-like-button.439730/
				// É bem mais interessante, pra poder ter Search entre todos os nodes existentes.
				// Seria um menu do mesmo tipo do "AddComponent"

				// Adds wait first?
				PopulateMenu(typeof(WaitTweenBehaviour), menu);

				// Populate TweenBehaviour menu
				FindAllDerivedTypes<TweenBehaviour>().ForEach(delegate (Type type)
				{
					PopulateMenu(type, menu, typeof(WaitTweenBehaviour));
				});

				// Display the menu
				menu.ShowAsContext();
			}

			void PopulateMenu(Type type, GenericMenu menu, Type ignoredType = null)
			{
				if (type != ignoredType)
				{
					var name = type.Name.ToString();

					name = ObjectNames.NicifyVariableName(name.Replace("TweenBehaviour", ""));

					menu.AddItem(new GUIContent(name), false, () =>
					{
						//OnClickAddNode(type, mousePosition, nodeType);
						CreateNode(type);
					});
				}
			}

			public TweenBehaviour CreateNode(Type type)
			{
				// PARA CRIAR NDOES OU PARAMETROS O IDEAL SERIA USAR A LISTA COM SEARCH
				// Como que esse cara aqui fez: https://forum.unity.com/threads/custom-add-component-like-button.439730/
				// É bem mais interessante, pra poder buscar entre todos os nodes existentes quando houver muitos nodes.

				//var child = Instantiate((Node)Activator.CreateInstance(type));
				var child = (TweenBehaviour)CreateInstance(type);
				//var child = ScriptableObject.CreateInstance<T>();

				// Pre-configure some data
				//child.parent = ability;
				child.name = "zzz_[" + child.GetType().Name + "]";
				//child.FillOutputs();

				// Adds as a child of an ability scriptable
				AssetDatabase.AddObjectToAsset(child, self);
				//child.hideFlags = HideFlags.HideInHierarchy;

				// Saves as scriptable asset
				var path = AssetDatabase.GetAssetPath(self);
				if (AssetDatabase.Contains(self))
				{
					//AssetDatabase.SaveAssets();
					AssetDatabase.ImportAsset(path);
				}
				else
				{
					AssetDatabase.CreateAsset(self, path);
				}
				AssetDatabase.ImportAsset(path);
				//AssetDatabase.SaveAssets();
				GUI.changed = true;

				if (list.index == -1)
				{
					list.index = self.behaviours.Count - 1;
				}

				list.index = Mathf.Max(0, list.index);


				try
				{
					self.behaviours.Insert(list.index + 1, child);
				}
				catch
				{
					self.behaviours.Add(child);
				}
				// Pre-configs the inserted behaviour
				list.index = list.index + 1;

				return child;
			}

			public static List<Type> FindAllDerivedTypes<T>()
			{
				return FindAllDerivedTypes<T>(Assembly.GetAssembly(typeof(T)));
			}

			private static List<Type> FindAllDerivedTypes<T>(Assembly assembly)
			{
				var derivedType = typeof(T);
				return assembly
					.GetTypes()
					.Where(t =>
						t != derivedType &&
						derivedType.IsAssignableFrom(t)
						).ToList();
			}
		}
#endif


	}
}