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
        private const int VideoPathColorCorrectionTableId = 2400;
        private const int DefaultWidth = 200;
        private const int ValueWidth = 150;

        private const string NeuronCompress = "EVS Neuron NAP - COMPRESS";
        private const string NeuronConvert = "EVS Neuron NAP - CONVERT";

        private Dictionary<string, List<VideoPathData>> videoPathData = new Dictionary<string, List<VideoPathData>>();
        private bool isNeuronConvert;
        private bool GainStatusOff;

        public PopUpDialog(IEngine engine, string elementId) : base(engine)
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
            AddWidget(ErrorMessageLabel, ++layoutRowPos, 0, 1, 2);

            AddWidget(GainLabel, ++layoutRowPos, 0);
            AddWidget(GainRedLabel, ++layoutRowPos, 0);
            AddWidget(GainRedSlider, layoutRowPos, 1);
            AddWidget(PercentageLabel, layoutRowPos, 2);
            AddWidget(GainGreenLabel, ++layoutRowPos, 0);
            AddWidget(GainGreenSlider, layoutRowPos, 1);
            AddWidget(Percentage2Label, layoutRowPos, 2);
            AddWidget(GainBlueLabel, ++layoutRowPos, 0);
            AddWidget(GainBlueSlider, layoutRowPos, 1);
            AddWidget(Percentage3Label, layoutRowPos, 2);
            AddWidget(BlacklevelLabel, ++layoutRowPos, 0);
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
            InitializeControls(engine, elementId);
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

        private readonly Label DelayLabel = new Label { Style = TextStyle.Bold, Text = "Frame Sync" };

        private readonly Label FrameDelayLabel = new Label { Text = "Frame Delay:" };

        private readonly Label VerticalDelayLabel = new Label { Text = "Vertical Delay:" };

        private readonly Label HorizontalDelayLabel = new Label { Text = "Horizontal Delay:" };

        private readonly Label ColorCorrectionsLabel = new Label { Style = TextStyle.Bold, Text = "Color Corrections" };

        private readonly Label GainLabel = new Label { Style = TextStyle.Heading, Text = "Gain" };

        private readonly Label GainRedLabel = new Label { Text = "Red:" };

        private readonly Label GainGreenLabel = new Label { Text = "Green:" };

        private readonly Label GainBlueLabel = new Label { Text = "Blue:" };

        private readonly Label BlacklevelLabel = new Label { Style = TextStyle.Heading, Text = "Black Level" };

        private readonly Label BlacklevelRedLabel = new Label { Text = "Red:" };

        private readonly Label BlacklevelGreenLabel = new Label { Text = "Green:" };

        private readonly Label BlacklevelBlueLabel = new Label { Text = "Blue:" };

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

        internal void InitializeControls(IEngine engine, string elementData)
        {
            GetTableData(engine, elementData);

            ErrorMessageLabel.IsVisible = false;

            if (videoPathData.Any())
            {
                var rows = isNeuronConvert ? videoPathData[NeuronConvert] : videoPathData[NeuronCompress];

                if (!rows.Any())
                {
                    SetDataNotAvailable();
                    return;
                }

                var defaultRow = rows[0];
                VideoPathDropDown.Options = rows.Select(x => x.Key).ToList();
                VideoPathDropDown.Selected = defaultRow.Key;
                FrameDelaySlider.Value = defaultRow.FrameDelay;
                VerticalDelaySlider.Value = defaultRow.VerticalDelay;
                HorizontalDelaySlider.Value = defaultRow.HorizontalDelay;
                GainRedSlider.Value = defaultRow.GainRed;
                GainGreenSlider.Value = defaultRow.GainGreen;
                GainBlueSlider.Value = defaultRow.GainBlue;
                BlacklevelRedSlider.Value = defaultRow.BlackLevelRed;
                BlacklevelGreenSlider.Value = defaultRow.BlackLevelGreen;
                BlacklevelBlueSlider.Value = defaultRow.BlackLevelBlue;

                ValidateVideoPathStatus(defaultRow);
            }
            else
            {
                SetDataNotAvailable();
            }
        }

        internal void UpdateDialogData()
        {
            var matchedRow = isNeuronConvert ?
                videoPathData[NeuronConvert].First(x => x.Key.Equals(VideoPathDropDown.Selected)) :
                videoPathData[NeuronCompress].First(x => x.Key.Equals(VideoPathDropDown.Selected));

            FrameDelaySlider.Value = matchedRow.FrameDelay;
            VerticalDelaySlider.Value = matchedRow.VerticalDelay;
            HorizontalDelaySlider.Value = matchedRow.HorizontalDelay;
            GainRedSlider.Value = matchedRow.GainRed;
            GainGreenSlider.Value = matchedRow.GainGreen;
            GainBlueSlider.Value = matchedRow.GainBlue;
            BlacklevelRedSlider.Value = matchedRow.BlackLevelRed;
            BlacklevelGreenSlider.Value = matchedRow.BlackLevelGreen;
            BlacklevelBlueSlider.Value = matchedRow.BlackLevelBlue;

            ValidateVideoPathStatus(matchedRow);
        }

        internal void ProcessSelectedData(IEngine engine, string elementData)
        {
            var splittedElement = elementData.Split('/');
            var dmaId = Convert.ToInt32(splittedElement[0]);
            var elementId = Convert.ToInt32(splittedElement[1]);

            var element = engine.FindElement(dmaId, elementId);
            var selectedVideo = VideoPathDropDown.Selected;

            // Set values on table cells
            if (isNeuronConvert)
            {
                element.SetParameterByPrimaryKey(2357, selectedVideo, FrameDelaySlider.Value);
                element.SetParameterByPrimaryKey(2358, selectedVideo, VerticalDelaySlider.Value);
                element.SetParameterByPrimaryKey(2359, selectedVideo, HorizontalDelaySlider.Value);

                if (!GainStatusOff)
                {
                    element.SetParameterByPrimaryKey(2360, selectedVideo, GainRedSlider.Value);
                    element.SetParameterByPrimaryKey(2361, selectedVideo, GainGreenSlider.Value);
                    element.SetParameterByPrimaryKey(2362, selectedVideo, GainBlueSlider.Value);
                    element.SetParameterByPrimaryKey(2363, selectedVideo, BlacklevelRedSlider.Value);
                    element.SetParameterByPrimaryKey(2364, selectedVideo, BlacklevelGreenSlider.Value);
                    element.SetParameterByPrimaryKey(2365, selectedVideo, BlacklevelBlueSlider.Value);
                }
            }
            else
            {
                element.SetParameterByPrimaryKey(2357, selectedVideo, FrameDelaySlider.Value);
                element.SetParameterByPrimaryKey(2358, selectedVideo, VerticalDelaySlider.Value);
                element.SetParameterByPrimaryKey(2359, selectedVideo, HorizontalDelaySlider.Value);
                if (!GainStatusOff)
                {
                    element.SetParameterByPrimaryKey(2454, selectedVideo, GainRedSlider.Value);
                    element.SetParameterByPrimaryKey(2455, selectedVideo, GainGreenSlider.Value);
                    element.SetParameterByPrimaryKey(2456, selectedVideo, GainBlueSlider.Value);
                    element.SetParameterByPrimaryKey(2460, selectedVideo, BlacklevelRedSlider.Value);
                    element.SetParameterByPrimaryKey(2461, selectedVideo, BlacklevelGreenSlider.Value);
                    element.SetParameterByPrimaryKey(2462, selectedVideo, BlacklevelBlueSlider.Value);
                }
            }
        }

        internal void SetDefaultData()
        {
            FrameDelaySlider.Value = 0;
            VerticalDelaySlider.Value = 0;
            HorizontalDelaySlider.Value = 0;

            if (!GainStatusOff)
            {
                GainRedSlider.Value = 100;
                GainGreenSlider.Value = 100;
                GainBlueSlider.Value = 100;

                BlacklevelBlueSlider.Value = 0;
                BlacklevelGreenSlider.Value = 0;
                BlacklevelBlueSlider.Value = 0;
            }
        }

        private void SetDataNotAvailable()
        {
            VideoPathDropDown.Options = new List<string> { "Data Not Available" };
            VideoPathDropDown.Selected = "Data Not Available";

            VideoPathDropDown.IsEnabled = false;
            EnableDisableWriteProperties(false);
        }

        private void GetTableData(IEngine engine, string elementData)
        {
            var splittedElement = elementData.Split('/');
            var dmaId = Convert.ToInt32(splittedElement[0]);
            var elementId = Convert.ToInt32(splittedElement[1]);

            var dms = engine.GetDms();
            var dmsElement = dms.GetElement(new DmsElementId(dmaId, elementId));
            isNeuronConvert = dmsElement.Protocol.Name.Equals(NeuronConvert);
            if (isNeuronConvert)
            {
                videoPathData.Add(NeuronConvert, new List<VideoPathData>());
                var tableData = dmsElement.GetTable(VideoPathTableId).GetData();
                foreach (var row in tableData.Values)
                {
                    videoPathData[NeuronConvert].Add(new VideoPathData
                    {
                        Key = Convert.ToString(row[0]),
                        FrameDelay = Convert.ToInt32(row[6]),
                        VerticalDelay = Convert.ToInt32(row[7]),
                        HorizontalDelay = Convert.ToInt32(row[8]),
                        GainRed = Convert.ToInt32(row[9]),
                        GainGreen = Convert.ToInt32(row[10]),
                        GainBlue = Convert.ToInt32(row[11]),
                        BlackLevelRed = Convert.ToInt32(row[12]),
                        BlackLevelGreen = Convert.ToInt32(row[13]),
                        BlackLevelBlue = Convert.ToInt32(row[14]),
                        Status = Convert.ToInt32(row[21]),
                    });
                }
            }
            else
            {
                videoPathData.Add(NeuronCompress, new List<VideoPathData>());
                var tableData = dmsElement.GetTable(VideoPathTableId).GetData();

                var list = new List<VideoPathData>();
                foreach (var row in tableData.Values)
                {
                    list.Add(new VideoPathData
                    {
                        Key = Convert.ToString(row[0]),
                        FrameDelay = Convert.ToInt32(row[5]),
                        VerticalDelay = Convert.ToInt32(row[6]),
                        HorizontalDelay = Convert.ToInt32(row[7]),
                    });
                }

                tableData = dmsElement.GetTable(VideoPathColorCorrectionTableId).GetData();
                foreach (var row in tableData.Values)
                {
                    var key = Convert.ToString(row[0]);
                    if (list.Exists(x => x.Key.Equals(key)))
                    {
                        var matchedRow = list.Find(x => x.Key.Equals(key));
                        matchedRow.GainRed = Convert.ToInt32(row[3]);
                        matchedRow.GainGreen = Convert.ToInt32(row[4]);
                        matchedRow.GainBlue = Convert.ToInt32(row[5]);
                        matchedRow.BlackLevelRed = Convert.ToInt32(row[9]);
                        matchedRow.BlackLevelGreen = Convert.ToInt32(row[10]);
                        matchedRow.BlackLevelBlue = Convert.ToInt32(row[11]);
                        matchedRow.Status = Convert.ToInt32(row[2]);
                    }
                }

                videoPathData[NeuronCompress].AddRange(list);
            }
        }

        private void ValidateVideoPathStatus(VideoPathData row)
        {
            GainStatusOff = row.Status == (int)Status.Off;
            if (GainStatusOff)
            {
                EnableDisableWriteProperties(false);
            }
            else
            {
                EnableDisableWriteProperties(true);
            }
        }

        private void EnableDisableWriteProperties(bool value)
        {
            GainRedSlider.IsEnabled = value;
            GainGreenSlider.IsEnabled = value;
            GainBlueSlider.IsEnabled = value;
            BlacklevelRedSlider.IsEnabled = value;
            BlacklevelGreenSlider.IsEnabled = value;
            BlacklevelBlueSlider.IsEnabled = value;

            ErrorMessageLabel.IsVisible = !value;
        }
    }

    public class VideoPathData
    {
        public string Key { get; set; }

        public int FrameDelay { get; set; }

        public int VerticalDelay { get; set; }

        public int HorizontalDelay { get; set; }

        public int GainRed { get; set; }

        public int GainGreen { get; set; }

        public int GainBlue { get; set; }

        public int BlackLevelRed { get; set; }

        public int BlackLevelGreen { get; set; }

        public int BlackLevelBlue { get; set; }

        public int Status { get; set; }
    }
}
