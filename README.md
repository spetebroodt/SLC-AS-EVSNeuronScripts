# SLC-AS-EVSNeuronScript

## About

This interactive Automation script allows you to update video path configurations.

It supports the following connectors:

- [EVS Neuron NAP - CONVERT](https://catalog.dataminer.services/details/4d4b865c-b80e-476f-b556-0e4c85bf7ee3)

## Usage

![image](https://github.com/user-attachments/assets/5879d7dc-3d5a-4189-b161-a9964b315bb2)

1. In the dropdown box at the top of the *Connection Configuration* window, select the video path you want to update.

1. Update any of the following values:

   **Frame sync**

   - Frame Delay
   - Vertical Delay
   - Horizontal Delay

   **Color corrections**

   - Gain

     - Red
     - Green
     - Blue

   - Black Level

     - Red
     - Green
     - Blue

1. Click *Apply* to save the changes.

1. Close *Close* to close the window.

> [!NOTE]
> If the selected video path has the *Gain* or *Gain State* column option set to "Disabled" or "Off" (depending on the connector), the interactive Automation script will disable the color correction options and show the following message:
`Values cannot be set as the Gain status is not enabled.`
