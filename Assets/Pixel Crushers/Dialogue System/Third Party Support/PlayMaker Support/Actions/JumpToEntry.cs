using UnityEngine;
using HutongGames.PlayMaker;

namespace PixelCrushers.DialogueSystem.PlayMaker
{

    [ActionCategory("Dialogue System")]
    [HutongGames.PlayMaker.TooltipAttribute("Jumps the active conversation to a specific entry.")]
    public class JumpToEntry : FsmStateAction
    {

        [HutongGames.PlayMaker.TooltipAttribute("Conversation containing the dialogue entry. Leave blank to use active conversation. Must select conversation if you want to specify entry by Title from a dropdown menu. (This is the conversation originally started; not linked conversation if it crossed links.)")]
        public FsmString conversationTitle;

        [HutongGames.PlayMaker.TooltipAttribute("The dialogue entry ID to jump to.")]
        public FsmInt entryID = new FsmInt();

        [HutongGames.PlayMaker.TooltipAttribute("The dialogue entry Title to jump to. If set, takes precedence over Entry ID.")]
        public FsmString entryTitle = new FsmString();

        public override void Reset()
        {
            conversationTitle = new FsmString();
            entryID = new FsmInt();
            entryID.Value = -1;
            entryTitle = new FsmString();
            entryTitle.Value = string.Empty;
        }

        public override void OnEnter()
        {
            if (!DialogueManager.isConversationActive)
            {
                LogError("Can't jump to entry " + entryID.Value + ". No conversation is active.");
            }
            else
            {
                var id = !string.IsNullOrEmpty(entryTitle.Value) ? DialogueSystemPlayMakerTools.GetEntryIDFromTitle(conversationTitle.Value, entryTitle.Value)
                    : (entryID != null) ? entryID.Value : -1;
                var conversation = (conversationTitle != null && !string.IsNullOrEmpty(conversationTitle.Value))
                    ? DialogueManager.masterDatabase.GetConversation(conversationTitle.Value)
                    : DialogueManager.masterDatabase.GetConversation(DialogueManager.lastConversationID);
                var entry = (conversation != null) ? conversation.GetDialogueEntry(id) : null;
                if (entry == null)
                {
                    LogError("Can't find entry  " + entryID.Value + " to jump to it.");
                }
                else
                {
                    var state = DialogueManager.conversationModel.GetState(entry);
                    DialogueManager.conversationController.GotoState(state);
                }
            }
            Finish();
        }

    }

}