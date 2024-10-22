/*
****************************************************************************
*  Copyright (c) 2024,  Skyline Communications NV  All Rights Reserved.    *
****************************************************************************

By using this script, you expressly agree with the usage terms and
conditions set out below.
This script and all related materials are protected by copyrights and
other intellectual property rights that exclusively belong
to Skyline Communications.

A user license granted for this script is strictly for personal use only.
This script may not be used in any way by anyone without the prior
written consent of Skyline Communications. Any sublicensing of this
script is forbidden.

Any modifications to this script by the user are only allowed for
personal use and within the intended purpose of the script,
and will remain the sole responsibility of the user.
Skyline Communications will not be responsible for any damages or
malfunctions whatsoever of the script resulting from a modification
or adaptation by the user.

The content of this script is confidential information.
The user hereby agrees to keep this confidential information strictly
secret and confidential and not to disclose or reveal it, in whole
or in part, directly or indirectly to any person, entity, organization
or administration without the prior written consent of
Skyline Communications.

Any inquiries can be addressed to:

	Skyline Communications NV
	Ambachtenstraat 33
	B-8870 Izegem
	Belgium
	Tel.	: +32 51 31 35 69
	Fax.	: +32 51 31 01 29
	E-mail	: info@skyline.be
	Web		: www.skyline.be
	Contact	: Ben Vandenberghe

****************************************************************************
Revision History:

DATE		VERSION		AUTHOR			COMMENTS

24/07/2024	1.0.0.1		GBS, Skyline	Initial version
****************************************************************************
*/

namespace SLC_AS_EVSNeuronScript_1
{
	using System;
	using System.Collections.Generic;

	using Newtonsoft.Json;

	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.Utils.InteractiveAutomationScript;

	/// <summary>
	/// Represents a DataMiner Automation script.
	/// </summary>
	public class Script
	{
		/// <summary>
		/// The script entry point.
		/// </summary>
		/// <param name="engine">Link with SLAutomation process.</param>
		public void Run(IEngine engine)
		{
			engine.Timeout = TimeSpan.FromHours(1);

			// DO NOT REMOVE THIS COMMENTED-OUT CODE OR THE SCRIPT WON'T RUN!
			// DataMiner evaluates if the script needs to launch in interactive mode.
			// This is determined by a simple string search looking for "engine.ShowUI" in the source code.
			// However, because of the toolkit NuGet package, this string cannot be found here.
			// So this comment is here as a workaround.
			//// engine.ShowUI();

			try
			{
				RunSafe(engine);
			}
			catch (ScriptAbortException)
			{
				// Catch normal abort exceptions (engine.ExitFail or engine.ExitSuccess)
				throw; // Comment if it should be treated as a normal exit of the script.
			}
			catch (ScriptForceAbortException)
			{
				// Catch forced abort exceptions, caused via external maintenance messages.
				throw;
			}
			catch (ScriptTimeoutException)
			{
				// Catch timeout exceptions for when a script has been running for too long.
				throw;
			}
			catch (InteractiveUserDetachedException)
			{
				// Catch a user detaching from the interactive script by closing the window.
				// Only applicable for interactive scripts, can be removed for non-interactive scripts.
				throw;
			}
			catch (Exception e)
			{
				engine.ExitFail("Run|Something went wrong: " + e);
			}
		}

		private void RunSafe(IEngine engine)
		{
			var elementId = GetOneDeserializedValue(engine.GetScriptParam("Element ID").Value);
			var videoPathId = GetOneDeserializedValue(engine.GetScriptParam("Video Path ID").Value);

			var controller = new InteractiveController(engine);
			var dialog = new PopUpDialog(engine, elementId, videoPathId);

			dialog.VideoPathDropDown.Changed += (sender, args) => dialog.UpdateDialogData();
			dialog.ApplyButton.Pressed += (sender, args) => dialog.ProcessSelectedData(engine, elementId);
			dialog.DefaultSettingsButton.Pressed += (sender, args) => dialog.SetDefaultData();
			dialog.CloseButton.Pressed += (sender, args) => controller.Stop();

			controller.ShowDialog(dialog);
		}

		private string GetOneDeserializedValue(string scriptParam)
		{
			if (scriptParam.Contains("[") && scriptParam.Contains("]"))
			{
				return JsonConvert.DeserializeObject<List<string>>(scriptParam)[0];
			}
			else
			{
				return scriptParam;
			}
		}
	}
}
