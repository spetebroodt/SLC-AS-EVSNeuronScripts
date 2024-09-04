namespace SLC_AS_EVSNeuronScript_1
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.Core.DataMinerSystem.Automation;
	using Skyline.DataMiner.Core.DataMinerSystem.Common;
	using Skyline.DataMiner.Utils.InteractiveAutomationScript;

	public class PopUpDialog : Dialog
    {
        private const int VideoPathTableId = 2300;
        private const int DefaultWidth = 200;
        private const int ValueWidth = 150;

        private IDmsElement dmsElement;

        public PopUpDialog(IEngine engine, string elementId) : base(engine)
        {
            // Set title
            Title = "Connection Configuration";

            // Init widgets
            VideoPathDropDown = new DropDown();
            ApplyButton = new Button("Apply");

            // Define layout
            var layoutRowPos = 0;

            AddWidget(VideoPathLabel, layoutRowPos, 0);
            AddWidget(VideoPathDropDown, layoutRowPos, 1);

            AddWidget(DelayLabel, ++layoutRowPos, 0);
            AddWidget(FrameDelayLabel, ++layoutRowPos, 0);
            AddWidget(FrameDelaySlider, layoutRowPos, 1);
            AddWidget(FramesLabel, layoutRowPos, 2);
            AddWidget(VerticalDelayLabel, ++layoutRowPos, 0);
            AddWidget(VerticalDelaySlider, layoutRowPos, 1);
            AddWidget(LinesLabel, layoutRowPos, 2);
            AddWidget(HorizontalDelayLabel, ++layoutRowPos, 0);
            AddWidget(HorizontalDelaySlider, layoutRowPos, 1);
            AddWidget(PixelLabel, layoutRowPos, 2);

            AddWidget(ColorCorrectionsLabel, ++layoutRowPos, 0);
            AddWidget(GainRedLabel, ++layoutRowPos, 0);
            AddWidget(GainRedSlider, layoutRowPos, 1);
            AddWidget(PercentageLabel, layoutRowPos, 2);
            AddWidget(GainGreenLabel, ++layoutRowPos, 0);
            AddWidget(GainGreenSlider, layoutRowPos, 1);
            AddWidget(Percentage2Label, layoutRowPos, 2);
            AddWidget(GainBlueLabel, ++layoutRowPos, 0);
            AddWidget(GainBlueSlider, layoutRowPos, 1);
            AddWidget(Percentage3Label, layoutRowPos, 2);
            AddWidget(BlacklevelRedLabel, ++layoutRowPos, 0);
            AddWidget(BlacklevelRedSlider, layoutRowPos, 1);
            AddWidget(BitLabel, layoutRowPos, 2);
            AddWidget(BlacklevelGreenLabel, ++layoutRowPos, 0);
            AddWidget(BlacklevelGreenSlider, layoutRowPos, 1);
            AddWidget(Bit2Label, layoutRowPos, 2);
            AddWidget(BlacklevelBlueLabel, ++layoutRowPos, 0);
            AddWidget(BlacklevelBlueSlider, layoutRowPos, 1);
            AddWidget(Bit3Label, layoutRowPos, 2);

            AddWidget(new WhiteSpace(), ++layoutRowPos, 0);
            AddWidget(ApplyButton, ++layoutRowPos, 0);

            // Adjust width
            VideoPathLabel.Width = DefaultWidth;
            VideoPathDropDown.Width = ValueWidth;

            DelayLabel.Width = DefaultWidth;
            FrameDelayLabel.Width = DefaultWidth;
            VerticalDelayLabel.Width = DefaultWidth;
            HorizontalDelayLabel.Width = DefaultWidth;

            ColorCorrectionsLabel.Width = DefaultWidth;
            GainRedLabel.Width = DefaultWidth;
            GainGreenLabel.Width = DefaultWidth;
            GainBlueLabel.Width = DefaultWidth;
            BlacklevelRedLabel.Width = DefaultWidth;
            BlacklevelGreenLabel.Width = DefaultWidth;
            BlacklevelBlueLabel.Width = DefaultWidth;

            ApplyButton.Width = DefaultWidth;

            // Set Default data
            SetDefaultData(engine, elementId);
        }

        #region Properties
        public DropDown VideoPathDropDown { get; set; }

        public Button ApplyButton { get; set; }

        private Label VideoPathLabel = new Label { Text = "Video Path:" };

        private Label FrameDelayLabel = new Label { Text = "Frame Delay:" };

        private Label VerticalDelayLabel = new Label { Text = "Vertical Delay:" };

        private Label HorizontalDelayLabel = new Label { Text = "Horizontal Delay:" };

        private Label DelayLabel = new Label { Style = TextStyle.Bold, Text = "Delay" };

        private Label ColorCorrectionsLabel = new Label { Style = TextStyle.Bold, Text = "Color Corrections" };

        private Label GainRedLabel = new Label { Text = "Gain Red:" };

        private Label GainGreenLabel = new Label { Text = "Gain Green:" };

        private Label GainBlueLabel = new Label { Text = "Gain Blue:" };

        private Label BlacklevelRedLabel = new Label { Text = "Blacklevel Red:" };

        private Label BlacklevelGreenLabel = new Label { Text = "Blacklevel Green:" };

        private Label BlacklevelBlueLabel = new Label { Text = "Blacklevel Blue:" };

        private Label FramesLabel = new Label { Text = "Frames" };

        private Label LinesLabel = new Label { Text = "Lines" };

        private Label PixelLabel = new Label { Text = "px" };

        private Label PercentageLabel = new Label { Text = "%" };

        private Label Percentage2Label = new Label { Text = "%" };

        private Label Percentage3Label = new Label { Text = "%" };

        private Label BitLabel = new Label { Text = "b" };

        private Label Bit2Label = new Label { Text = "b" };

        private Label Bit3Label = new Label { Text = "b" };

        private Numeric FrameDelaySlider = new Numeric { Minimum = 0, Maximum = 128, Width = ValueWidth };

        private Numeric VerticalDelaySlider = new Numeric { Minimum = 0, Maximum = 2160, Width = ValueWidth };

        private Numeric HorizontalDelaySlider = new Numeric { Minimum = 0, Maximum = 4124, Width = ValueWidth };

        private Numeric GainRedSlider = new Numeric { Minimum = 50, Maximum = 150 , Width = ValueWidth };

        private Numeric GainGreenSlider = new Numeric { Minimum = 50, Maximum = 150, Width = ValueWidth };

        private Numeric GainBlueSlider = new Numeric { Minimum = 50, Maximum = 150, Width = ValueWidth };

        private Numeric BlacklevelRedSlider = new Numeric { Minimum = -128, Maximum = 127, Width = ValueWidth };

        private Numeric BlacklevelGreenSlider = new Numeric { Minimum = -128, Maximum = 127, Width = ValueWidth };

        private Numeric BlacklevelBlueSlider = new Numeric { Minimum = -128, Maximum = 127, Width = ValueWidth };
        #endregion

        internal void SetDefaultData(IEngine engine, string elementData)
        {
            var splittedElement = elementData.Split('/');
            var dmaId = Convert.ToInt32(splittedElement[0]);
            var elementId = Convert.ToInt32(splittedElement[1]);

            var dms = engine.GetDms();
            dmsElement = dms.GetElement(new DmsElementId(dmaId, elementId));
            var tableData = (Dictionary<string, object[]>)dmsElement.GetTable(VideoPathTableId).GetData();

            if (tableData.Any())
            {
                var defaultRow = tableData.First();
                VideoPathDropDown.Options = tableData.Keys;
                VideoPathDropDown.Selected = defaultRow.Key;
                FrameDelaySlider.Value = Convert.ToInt32(defaultRow.Value[6]);
                VerticalDelaySlider.Value = Convert.ToInt32(defaultRow.Value[7]);
                HorizontalDelaySlider.Value = Convert.ToInt32(defaultRow.Value[8]);
                GainRedSlider.Value = Convert.ToInt32(defaultRow.Value[9]);
                GainGreenSlider.Value = Convert.ToInt32(defaultRow.Value[10]);
                GainBlueSlider.Value = Convert.ToInt32(defaultRow.Value[11]);
                BlacklevelRedSlider.Value = Convert.ToInt32(defaultRow.Value[12]);
                BlacklevelGreenSlider.Value = Convert.ToInt32(defaultRow.Value[13]);
                BlacklevelBlueSlider.Value = Convert.ToInt32(defaultRow.Value[14]);

                VideoPathDropDown.IsEnabled = true;
                FrameDelaySlider.IsEnabled = true;
                VerticalDelaySlider.IsEnabled = true;
                HorizontalDelaySlider.IsEnabled = true;
                GainRedSlider.IsEnabled = true;
                GainGreenSlider.IsEnabled = true;
                GainBlueSlider.IsEnabled = true;
                BlacklevelRedSlider.IsEnabled = true;
                BlacklevelGreenSlider.IsEnabled = true;
                BlacklevelBlueSlider.IsEnabled = true;
            }
            else
            {
                VideoPathDropDown.Options = new List<string> { "Data Not Available" };
                VideoPathDropDown.Selected = "Data Not Available";

                VideoPathDropDown.IsEnabled = false;
                FrameDelaySlider.IsEnabled = false;
                VerticalDelaySlider.IsEnabled = false;
                HorizontalDelaySlider.IsEnabled = false;
                GainRedSlider.IsEnabled = false;
                GainGreenSlider.IsEnabled = false;
                GainBlueSlider.IsEnabled = false;
                BlacklevelRedSlider.IsEnabled = false;
                BlacklevelGreenSlider.IsEnabled = false;
                BlacklevelBlueSlider.IsEnabled = false;
            }
        }

        internal void UpdateDialogData()
        {
            var tableData = dmsElement.GetTable(VideoPathTableId).GetData();
            var matchedRow = tableData[VideoPathDropDown.Selected];
            FrameDelaySlider.Value = Convert.ToInt32(matchedRow[6]);
            VerticalDelaySlider.Value = Convert.ToInt32(matchedRow[7]);
            HorizontalDelaySlider.Value = Convert.ToInt32(matchedRow[8]);
            GainRedSlider.Value = Convert.ToInt32(matchedRow[9]);
            GainGreenSlider.Value = Convert.ToInt32(matchedRow[10]);
            GainBlueSlider.Value = Convert.ToInt32(matchedRow[11]);
            BlacklevelRedSlider.Value = Convert.ToInt32(matchedRow[12]);
            BlacklevelGreenSlider.Value = Convert.ToInt32(matchedRow[13]);
            BlacklevelBlueSlider.Value = Convert.ToInt32(matchedRow[14]);
        }

        internal void ProcessSelectedData(IEngine engine, string elementData)
        {
            var splittedElement = elementData.Split('/');
            var dmaId = Convert.ToInt32(splittedElement[0]);
            var elementId = Convert.ToInt32(splittedElement[1]);

            var element = engine.FindElement(dmaId, elementId);
            var selectedVideo = VideoPathDropDown.Selected;

            // Set values on table cells
            element.SetParameterByPrimaryKey(2357, selectedVideo, FrameDelaySlider.Value);
            element.SetParameterByPrimaryKey(2358, selectedVideo, VerticalDelaySlider.Value);
            element.SetParameterByPrimaryKey(2359, selectedVideo, HorizontalDelaySlider.Value);
            element.SetParameterByPrimaryKey(2360, selectedVideo, GainRedSlider.Value);
            element.SetParameterByPrimaryKey(2361, selectedVideo, GainGreenSlider.Value);
            element.SetParameterByPrimaryKey(2362, selectedVideo, GainBlueSlider.Value);
            element.SetParameterByPrimaryKey(2363, selectedVideo, BlacklevelRedSlider.Value);
            element.SetParameterByPrimaryKey(2364, selectedVideo, BlacklevelGreenSlider.Value);
            element.SetParameterByPrimaryKey(2365, selectedVideo, BlacklevelBlueSlider.Value);
        }
    }
}
