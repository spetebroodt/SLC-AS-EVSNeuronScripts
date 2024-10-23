# SLC-AS-EVSNeuronScript

This Interactive Automation Script (IAS) allows the user to update Video Path configurations at once. This IAS supports the following connectors: 
- EVS Neuron - CONVERT: designed for the monitoring of the convert function of the EVS Neuron
- EVS Neuron - COMPRESS: designed for the monitoring of the convert function of the EVS Neuron

The user selects the video path to edit on the dropdown at the top and it shows the actual values. The user would apply/discard the changes using the "Apply" and "Cancel" buttons. The values allowed to edit/update are:

Delays: 
- Frame Delay
- Vertical Delay
- Horizontal Delay

Color Corrections:
- Gain Red
- Gain Green
- Gain Blue
- Blacklevel Red
- Blacklevel Green
- Blacklevel Blue

In case the selected Video Path has the "Gain" or "Gain State" column (depends on the connector) option Disabled/Off, the IAS will disable all options and show the following message: "Values cannot be set as the Gain status is not enabled."

This is how the IAS is displayed on an App:

![image](https://github.com/user-attachments/assets/e58e5ffc-c6dc-4e73-8b6d-31f834c61cdf)
