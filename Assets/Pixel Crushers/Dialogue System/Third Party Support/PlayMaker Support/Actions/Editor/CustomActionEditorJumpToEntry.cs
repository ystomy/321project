using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using HutongGames.PlayMakerEditor;

namespace PixelCrushers.DialogueSystem.PlayMaker
{

    [CustomActionEditor(typeof(JumpToEntry))]
    public class CustomActionEditorJumpToEntry : CustomActionEditor
    {

        private ConversationPicker conversationPicker = null;
        private DialogueEntryPicker entryPicker = null;
        protected string[] conversationTitles = null;

        public override void OnEnable()
        {
            var action = target as JumpToEntry;
            if (action.conversationTitle == null) action.conversationTitle = new HutongGames.PlayMaker.FsmString();
            conversationPicker = new ConversationPicker(EditorTools.FindInitialDatabase(), action.conversationTitle.Value, !action.conversationTitle.UseVariable);
        }

        public override bool OnGUI()
        {
            var isDirty = false;

            var action = target as JumpToEntry;
            if (action == null) return DrawDefaultInspector();

            EditorGUI.BeginChangeCheck();

            if (action.conversationTitle == null) action.conversationTitle = new HutongGames.PlayMaker.FsmString();
            action.conversationTitle.UseVariable = EditorGUILayout.Toggle(new GUIContent("Use Variable", "Specify the conversation title with a PlayMaker variable"), action.conversationTitle.UseVariable);

            if (action.conversationTitle.UseVariable)
            {
                EditField("conversationTitle");
            }
            else
            {
                conversationPicker.Draw(true);
                if (!string.Equals(action.conversationTitle.Value, conversationPicker.currentConversation))
                {
                    action.conversationTitle.Value = conversationPicker.currentConversation;
                    isDirty = true;
                    entryPicker = null;
                }
            }

            if (EditorGUI.EndChangeCheck())
            {
                conversationTitles = null;
            }

            if (action.entryID == null)
            {
                action.entryID = new HutongGames.PlayMaker.FsmInt();
                action.entryID.Value = -1;
            }
            var specifyEntryID = (action.entryID.Value != -1);
            EditorGUI.BeginChangeCheck();
            specifyEntryID = EditorGUILayout.Toggle(new GUIContent("Specify Entry ID", "Specify a dialogue entry to jump to"), specifyEntryID);
            if (EditorGUI.EndChangeCheck())
            {
                action.entryID.Value = specifyEntryID ? 0 : -1;
            }
            if (specifyEntryID)
            {
                action.entryTitle.Value = string.Empty;
                if (entryPicker == null)
                {
                    entryPicker = new DialogueEntryPicker(action.conversationTitle.Value);
                }
                if (entryPicker.isValid)
                {
                    action.entryID.Value = entryPicker.DoLayout("Entry ID", action.entryID.Value);
                }
                else
                {
                    EditField("entryID");
                }
            }

            var specifyEntryTitle = action.entryTitle != null &&
                (action.entryTitle.UseVariable || !string.IsNullOrEmpty(action.entryTitle.Value));
            EditorGUI.BeginChangeCheck();
            specifyEntryTitle = EditorGUILayout.Toggle(new GUIContent("Specify Entry Title", "Specify the Title of a dialogue entry to jump to"), specifyEntryTitle);
            if (EditorGUI.EndChangeCheck())
            {
                if (!specifyEntryTitle) action.entryTitle.Value = string.Empty;
            }

            if (specifyEntryTitle)
            {
                action.entryID.Value = -1;

                if (action.entryTitle == null) action.entryTitle = new HutongGames.PlayMaker.FsmString();
                action.entryTitle.UseVariable = EditorGUILayout.Toggle(new GUIContent("Use Variable", "Specify the dialogue entry title with a PlayMaker variable"), action.entryTitle.UseVariable);

                if (action.entryTitle.UseVariable)
                {
                    EditField("entryTitle");
                }
                else
                {
                    // Draw entry title picker:
                    if (conversationTitles == null)
                    {
                        conversationTitles = GetUniqueTitles(action.conversationTitle.Value);
                    }
                    if (string.IsNullOrEmpty(action.entryTitle.Value))
                    {
                        action.entryTitle = conversationTitles[0];
                    }
                    var titleIndex = GetTitleIndex(conversationTitles, action.entryTitle.Value);
                    if (titleIndex == -1) titleIndex = 0;
                    EditorGUI.BeginChangeCheck();
                    titleIndex = EditorGUILayout.Popup(new GUIContent("entry Title", "Jump to entry with this Title. If set, takes precedence over Entry ID"), titleIndex, conversationTitles);
                    if (EditorGUI.EndChangeCheck())
                    {
                        if (0 <= titleIndex && titleIndex < conversationTitles.Length)
                        {
                            action.entryTitle.Value = conversationTitles[titleIndex];
                        }
                    }
                }
            }

            return isDirty || GUI.changed;
        }

        protected string[] GetUniqueTitles(string conversationTitle)
        {
            var list = new List<string>();
            if (conversationPicker.database != null)
            {
                var conversation = conversationPicker.database.GetConversation(conversationTitle);
                if (conversation != null)
                {
                    foreach (var entry in conversation.dialogueEntries)
                    {
                        var title = entry.Title;
                        if (!list.Contains(title))
                        {
                            list.Add(title);
                        }
                    }
                }
            }
            return list.ToArray();
        }

        protected int GetTitleIndex(string[] titles, string currentTitle)
        {
            if (string.IsNullOrEmpty(currentTitle) || titles == null) return -1;
            for (int i = 0; i < titles.Length; i++)
            {
                if (string.Equals(currentTitle, titles[i]))
                {
                    return i;
                }
            }
            return -1;
        }

        protected int GetEntryIDFromTitle(string conversationTitle, string entryTitle)
        {
            if (conversationPicker.database != null)
            {
                var conversation = conversationPicker.database.GetConversation(conversationTitle);
                if (conversation != null)
                {
                    foreach (var entry in conversation.dialogueEntries)
                    {
                        if (entry.Title == entryTitle) return entry.id;
                    }
                }
            }
            return -1;
        }

    }
}