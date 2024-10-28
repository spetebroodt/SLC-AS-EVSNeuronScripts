# SLC-AS-EVSNeuronScript

This Interactive Automation Script (IAS) allows the user to update Video Path configurations at once. This IAS supports the following connectors: 
- [EVS Neuron NAP - CONVERT](https://catalog.dataminer.services/details/4d4b865c-b80e-476f-b556-0e4c85bf7ee3)

The user selects the video path to edit on the dropdown at the top and it shows the actual values. The user would apply/discard the changes using the "Apply" and "Cancel" buttons. The values allowed to edit/update are:

Frame Sync: 
- Frame Delay
- Vertical Delay
- Horizontal Delay

Color Corrections:
- Gain
  - Red
  - Green
  - Blue
- Black Level
  - Red
  - Green
  - Blue

In case the selected Video Path has the "Gain" or "Gain State" column option Disabled/Off (depending on the connector), the IAS will disable the Color Correction options and show the following message: "Values cannot be set as the Gain status is not enabled."

This is how the IAS is displayed on an App:

![image](https://github.com/user-attachments/assets/5879d7dc-3d5a-4189-b161-a9964b315bb2)
