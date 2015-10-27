﻿using System;
using System.Windows.Forms;
using AldursLab.WurmAssistant3.Core.Areas.Triggers.Modules.TriggersManager;

namespace AldursLab.WurmAssistant3.Core.Areas.Triggers.Views.TriggersManager
{
    public partial class SimpleConditionTriggerBaseConfig : UserControl, ITriggerConfig
    {
        private readonly SimpleConditionTriggerBase _simpleConditionTriggerBase;

        private readonly bool initComplete = false;
        public SimpleConditionTriggerBaseConfig(SimpleConditionTriggerBase simpleConditionTriggerBase)
        {
            _simpleConditionTriggerBase = simpleConditionTriggerBase;
            InitializeComponent();
            ConditionTbox.Text = simpleConditionTriggerBase.Condition;
            DescLabel.Text = simpleConditionTriggerBase.ConditionHelp;
            initComplete = true;
        }

        public UserControl ControlHandle { get { return this; } }

        private void ConditionTbox_TextChanged(object sender, EventArgs e)
        {
            if (initComplete) _simpleConditionTriggerBase.Condition = ConditionTbox.Text;
        }
    }
}