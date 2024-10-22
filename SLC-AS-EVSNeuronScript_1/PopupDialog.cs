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

        private Dictionary<string, object[]> tableData = new Dictionary<string, object[]>();

        public PopUpDialog(IEngine engine, string elementId, string videoPathId) : base(engine)
        {
            // Set title
            Title = "Connection Configuration";

            // Init widgets
            VideoPathDropDown = new DropDown();
            ApplyButton = new Button("Apply");
            DefaultSettingsButton = new Button("Reset to Default");
            CloseButton = new Button("Close");

            // Define layout
            var layoutRowPos = 0;

            AddWidget(VideoPathLabel, layoutRowPos, 0);
            AddWidget(VideoPathDropDown, layoutRowPos, 1, rowSpan: 1, colSpan: 2);
            AddWidget(DefaultSettingsButton, ++layoutRowPos, 1, rowSpan: 1, colSpan: 2);

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
            AddWidget(ErrorMessageLabel, ++layoutRowPos, 0, 1, 2);

            AddWidget(new WhiteSpace(), ++layoutRowPos, 0);
            AddWidget(ApplyButton, ++layoutRowPos, 0);

            AddWidget(new WhiteSpace(), ++layoutRowPos, 0);
            AddWidget(CloseButton, ++layoutRowPos, 0);

            // Adjust width
            VideoPathLabel.Width = DefaultWidth;

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
            ErrorMessageLabel.Width = 350;

            ApplyButton.Width = DefaultWidth;
            CloseButton.Width = DefaultWidth;

            // Set Default data
            InitializeControls(engine, elementId, videoPathId);
        }

        private enum Status
        {
            Off = 7,
            On = 8,
        }

        #region Properties
        public DropDown VideoPathDropDown { get; set; }

        public Button ApplyButton { get; set; }

        public Button DefaultSettingsButton { get; set; }

        public Button CloseButton { get; set; }

        private readonly Label VideoPathLabel = new Label { Text = "Video Path:" };

        private readonly Label FrameDelayLabel = new Label { Text = "Frame Delay:" };

        private readonly Label VerticalDelayLabel = new Label { Text = "Vertical Delay:" };

        private readonly Label HorizontalDelayLabel = new Label { Text = "Horizontal Delay:" };

        private readonly Label DelayLabel = new Label { Style = TextStyle.Bold, Text = "Delay" };

        private readonly Label ColorCorrectionsLabel = new Label { Style = TextStyle.Bold, Text = "Color Corrections" };

        private readonly Label GainRedLabel = new Label { Text = "Gain Red:" };

        private readonly Label GainGreenLabel = new Label { Text = "Gain Green:" };

        private readonly Label GainBlueLabel = new Label { Text = "Gain Blue:" };

        private readonly Label BlacklevelRedLabel = new Label { Text = "Blacklevel Red:" };

        private readonly Label BlacklevelGreenLabel = new Label { Text = "Blacklevel Green:" };

        private readonly Label BlacklevelBlueLabel = new Label { Text = "Blacklevel Blue:" };

        private readonly Label FramesLabel = new Label { Text = "Frames" };

        private readonly Label LinesLabel = new Label { Text = "Lines" };

        private readonly Label PixelLabel = new Label { Text = "px" };

        private readonly Label PercentageLabel = new Label { Text = "%" };

        private readonly Label Percentage2Label = new Label { Text = "%" };

        private readonly Label Percentage3Label = new Label { Text = "%" };

        private readonly Label BitLabel = new Label { Text = "b" };

        private readonly Label Bit2Label = new Label { Text = "b" };

        private readonly Label Bit3Label = new Label { Text = "b" };

        private readonly Label ErrorMessageLabel = new Label { Style = TextStyle.Heading, Text = "Values cannot be set as the Gain status is not enabled." };

        private readonly Numeric FrameDelaySlider = new Numeric { Minimum = 0, Maximum = 128, Width = ValueWidth };

        private readonly Numeric VerticalDelaySlider = new Numeric { Minimum = 0, Maximum = 2160, Width = ValueWidth };

        private readonly Numeric HorizontalDelaySlider = new Numeric { Minimum = 0, Maximum = 4124, Width = ValueWidth };

        private readonly Numeric GainRedSlider = new Numeric { Minimum = 50, Maximum = 150, Width = ValueWidth };

        private readonly Numeric GainGreenSlider = new Numeric { Minimum = 50, Maximum = 150, Width = ValueWidth };

        private readonly Numeric GainBlueSlider = new Numeric { Minimum = 50, Maximum = 150, Width = ValueWidth };

        private readonly Numeric BlacklevelRedSlider = new Numeric { Minimum = -128, Maximum = 127, Width = ValueWidth };

        private readonly Numeric BlacklevelGreenSlider = new Numeric { Minimum = -128, Maximum = 127, Width = ValueWidth };

        private readonly Numeric BlacklevelBlueSlider = new Numeric { Minimum = -128, Maximum = 127, Width = ValueWidth };
        #endregion

        internal void InitializeControls(IEngine engine, string elementData, string videoPathId)
        {
            var splittedElement = elementData.Split('/');
            var dmaId = Convert.ToInt32(splittedElement[0]);
            var elementId = Convert.ToInt32(splittedElement[1]);

            var dms = engine.GetDms();
            var dmsElement = dms.GetElement(new DmsElementId(dmaId, elementId));
            tableData = (Dictionary<string, object[]>)dmsElement.GetTable(VideoPathTableId).GetData();

            ErrorMessageLabel.IsVisible = false;

            if (tableData.Any())
            {
                var defaultRow = tableData.First(x => x.Key.Equals(videoPathId));
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

                EnableDisableWriteProperties(true);

                if (ValidateRowStatus(defaultRow.Value))
                {
                    ErrorMessageLabel.IsVisible = true;
                    EnableDisableWriteProperties(false);
                    ApplyButton.IsEnabled = false;
                    DefaultSettingsButton.IsEnabled = false;
                }
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

        internal void EnableDisableWriteProperties(bool value)
        {
            VideoPathDropDown.IsEnabled = value;
            FrameDelaySlider.IsEnabled = value;
            VerticalDelaySlider.IsEnabled = value;
            HorizontalDelaySlider.IsEnabled = value;
            GainRedSlider.IsEnabled = value;
            GainGreenSlider.IsEnabled = value;
            GainBlueSlider.IsEnabled = value;
            BlacklevelRedSlider.IsEnabled = value;
            BlacklevelGreenSlider.IsEnabled = value;
            BlacklevelBlueSlider.IsEnabled = value;
        }

        internal void SetDefaultData()
        {
            FrameDelaySlider.Value = 0;
            VerticalDelaySlider.Value = 0;
            HorizontalDelaySlider.Value = 0;

            GainRedSlider.Value = 100;
            GainGreenSlider.Value = 100;
            GainBlueSlider.Value = 100;

            BlacklevelBlueSlider.Value = 0;
            BlacklevelGreenSlider.Value = 0;
            BlacklevelBlueSlider.Value = 0;
        }

        internal void UpdateDialogData()
        {
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

            var matchedRow = tableData.FirstOrDefault(x => x.Key.Equals(selectedVideo)).Value;
            if (ValidateRowStatus(matchedRow))
            {
                ErrorMessageLabel.IsVisible = true;
                return;
            }

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

        internal bool ValidateRowStatus(object[] row)
        {
            return Convert.ToInt32(row[21/*Status*/]) == (int)Status.Off;
        }
    }
}
