using ui;
using UnityEngine;

namespace behaviours.block {
    [RequireComponent(typeof(BlockSkillDataHolder))]
    public class BlockTooltipHandler : MonoBehaviour {
        private BlockSkillDataHolder _blockSkillDataHolder;
        private string _tooltipMessage;
        private bool _toggleTooltip;

        private void Start() {
            _blockSkillDataHolder = GetComponent<BlockSkillDataHolder>();
            _tooltipMessage = BuildTooltipMessage();
        }

        private void OnMouseDown() {
            if (_toggleTooltip) {
                TooltipUI.Instance.Hide();
            } else {
                TooltipUI.Instance.Show(_tooltipMessage);
            }

            _toggleTooltip = !_toggleTooltip;
        }

        private void OnMouseExit() {
            _toggleTooltip = false;
            TooltipUI.Instance.Hide();
        }

        private string BuildTooltipMessage() {
            //HERE WE CAN FORMAT THE TOOLTIP MESSAGE
            return _blockSkillDataHolder.GetSkillData().ToString();
        }
    }
}