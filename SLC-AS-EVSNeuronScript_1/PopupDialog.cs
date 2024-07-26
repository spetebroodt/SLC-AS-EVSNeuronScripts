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
        private Dictionary<string, object[]> tableData = new Dictionary<string, object[]>();

        public PopUpDialog(IEngine engine, string elementId) : base(engine)
        {
            // Set title
            Title = "Connection Configuration";

            // Init widgets
            VideoPathLabel = new Label("Video Path");
            VideoPathDropDown = new DropDown();
            FrameDelayLabel = new Label("Frame Delay");
            FrameDelayTextBox = new TextBox();
            VerticalDelayLabel = new Label("Vertical Delay");
            VerticalDelayTextBox = new TextBox();
            HorizontalDelayLabel = new Label("Horizontal Delay");
            HorizontalDelayTextBox = new TextBox();
            ApplyButton = new Button("Apply");

            // Define layout
            AddWidget(VideoPathLabel, 0, 0);
            AddWidget(VideoPathDropDown, 1, 0);
            AddWidget(FrameDelayLabel, 2, 0);
            AddWidget(FrameDelayTextBox, 3, 0);
            AddWidget(VerticalDelayLabel, 4, 0);
            AddWidget(VerticalDelayTextBox, 5, 0);
            AddWidget(HorizontalDelayLabel, 6, 0);
            AddWidget(HorizontalDelayTextBox, 7, 0);
            AddWidget(ApplyButton, 8, 0);

            // Adjust width
            VideoPathLabel.Width = 100;
            VideoPathDropDown.Width = 100;
            FrameDelayLabel.Width = 100;
            FrameDelayTextBox.Width = 100;
            VerticalDelayLabel.Width = 100;
            VerticalDelayTextBox.Width = 100;
            HorizontalDelayLabel.Width = 100;
            HorizontalDelayTextBox.Width = 100;
            ApplyButton.Width = 100;

            // Set Default data
            SetDefaultData(engine, elementId);
        }

        public DropDown VideoPathDropDown { get; set; }

        public Button ApplyButton { get; set; }

        private Label VideoPathLabel { get; set; }

        private Label FrameDelayLabel { get; set; }

        private Label VerticalDelayLabel { get; set; }

        private Label HorizontalDelayLabel { get; set; }

        private TextBox FrameDelayTextBox { get; set; }

        private TextBox VerticalDelayTextBox { get; set; }

        private TextBox HorizontalDelayTextBox { get; set; }

        internal void SetDefaultData(IEngine engine, string elementData)
        {
            var splittedElement = elementData.Split('/');
            var dmaId = Convert.ToInt32(splittedElement[0]);
            var elementId = Convert.ToInt32(splittedElement[1]);

            var dms = engine.GetDms();
            var dmsElement = dms.GetElement(new DmsElementId(dmaId, elementId));
            tableData = (Dictionary<string, object[]>)dmsElement.GetTable(2300 /* Video Path */).GetData();

            if (tableData.Any())
            {
                var defaultRow = tableData.First();
                this.VideoPathDropDown.Options = tableData.Keys;
                this.VideoPathDropDown.Selected = defaultRow.Key;
                this.FrameDelayTextBox.Text = Convert.ToString(defaultRow.Value[6]);
                this.VerticalDelayTextBox.Text = Convert.ToString(defaultRow.Value[7]);
                this.HorizontalDelayTextBox.Text = Convert.ToString(defaultRow.Value[8]);

                this.VideoPathDropDown.IsEnabled = true;
                this.FrameDelayTextBox.IsEnabled = true;
                this.VerticalDelayTextBox.IsEnabled = true;
                this.HorizontalDelayTextBox.IsEnabled = true;
            }
            else
            {
                this.VideoPathDropDown.Options = new List<string> { "Data Not Available" };
                this.VideoPathDropDown.Selected = "Data Not Available";
                this.FrameDelayTextBox.Text = "N/A";
                this.VerticalDelayTextBox.Text = "N/A";
                this.HorizontalDelayTextBox.Text = "N/A";

                this.VideoPathDropDown.IsEnabled = false;
                this.FrameDelayTextBox.IsEnabled = false;
                this.VerticalDelayTextBox.IsEnabled = false;
                this.HorizontalDelayTextBox.IsEnabled = false;
            }
        }

        internal void UpdateDialogData()
        {
            var defaultRow = tableData.First();
            this.FrameDelayTextBox.Text = Convert.ToString(defaultRow.Value[6]);
            this.VerticalDelayTextBox.Text = Convert.ToString(defaultRow.Value[7]);
            this.HorizontalDelayTextBox.Text = Convert.ToString(defaultRow.Value[8]);
        }

        internal void ProcessSelectedData(IEngine engine, string elementData)
        {
            var splittedElement = elementData.Split('/');
            var dmaId = Convert.ToInt32(splittedElement[0]);
            var elementId = Convert.ToInt32(splittedElement[1]);

            var element = engine.FindElement(dmaId, elementId);

            // Set values on table cells
            element.SetParameterByPrimaryKey(2307, this.VideoPathDropDown.Selected, this.FrameDelayTextBox.Text);
            element.SetParameterByPrimaryKey(2308, this.VideoPathDropDown.Selected, this.VerticalDelayTextBox.Text);
            element.SetParameterByPrimaryKey(2309, this.VideoPathDropDown.Selected, this.HorizontalDelayTextBox.Text);
        }
    }
}
