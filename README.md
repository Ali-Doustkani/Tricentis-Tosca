<p align="center"><img src="/screenshots/tricentis-logo.png" width="40%" alt="Tricentis Logo" /></p>

# NeoLoad Add-on for Tricentis Tosca

## Overview

C# extension to integrate [Tricentis Tosca](https://www.tricentis.com/) with [NeoLoad](https://www.neotys.com/neoload/overview) for SAP GUI and Web Script maintenance.
It allows you to interact with the NeoLoad [Design API](https://www.neotys.com/documents/doc/neoload/latest/en/html/#11265.htm) to convert a Tricentis Tosca SAP GUI or Web script to a NeoLoad User Path or update an existing User Path.



| Property | Value |
| ----------------    | ----------------   |
| Maturity | Stable |
| Author | Neotys |
| License           | [BSD 2-Clause "Simplified"](https://github.com/Neotys-Labs/Tricentis-Tosca/blob/master/LICENSE) |
| NeoLoad Licensing | License FREE edition, or Enterprise edition, or Professional with Integration & Advanced Usage|
| Tested versions | Tricentis Tosca versions: <ul><li>12.x (12.2, 12.3)</li><li>13.x (13.0, 13.1, 13.2, 13.3, 13.4)</li><li>14.x (14.1, 14.2)</li><li>15.x (15.0)</li></ul>[NeoLoad versions](https://www.neotys.com/support/download-neoload) from 6.6 to 8.0.
| Download Binaries | See the [latest release](https://github.com/Neotys-Labs/Tricentis-Tosca/releases/latest)|

## Setting up the NeoLoad Tricentis Tosca Add-on

1. Download and unzip the [latest release](https://github.com/Neotys-Labs/Tricentis-Tosca/releases/latest).

2. Execute the `Install-NeoLoadToscaAddon.bat` file to launch the installation wizard.

3. For more installation options, refer to the `README.txt` file you will find inside your downloaded release.

**Warning**: For SAP test case, you might need to launch both Tosca and SAP Logon as administrator in order to convert the Tosca Script to NeoLoad.

## Global Configuration

Go to **PROJECT** > **Options**, to define the NeoLoad Add-on settings:

<p align="center"><img src="/screenshots/options.png" alt="Options" /></p>

Parameters: 
* **NeoLoadApiPort**: The port of the NeoLoad API, by default it is 7400. 
* **NeoLoadApiKey**: The API Key specified in the NeoLoad General settings or in Project settings REST API. If no identification is required both in Project settings and in General settings, this parameter can be left blank.
* **NeoLoadApiHostname**: The hostname of the machine that contains NeoLoad, by default it is localhost. It should be localhost for SAP GUI test case.
* **CreateTransactionBySapTCode**: Enable/Disable the creation of transaction in NeoLoad for each SAP TCode.
* **Http2**: Enable/Disable the recording of HTTP/2.

To access these values, go to the NeoLoad **Preferences**, then the **Project settings** tab, then select the **REST API** category.
<p align="center"><img src="/screenshots/designapi.png" alt="Design API" /></p>

### When enterprise proxy is set
During any transfer from Tosca to NeoLoad, the enterprise proxy settings must be disabled so that NeoLoad can manage the system proxy temporarily to record the network traffic generated by the Tosca test.
Before the transfer, go to windows proxy settings. Set "Automatically detect settings" and "Use setup script" to Off as shown below.
After the transfer complete, reset the settings at their initial value.
<p align="center"><img src="/screenshots/disable-enterprise-proxy.png" alt="Windows proxy seetings"/></p>

## How to convert a Tricentis Tosca SAP script to a NeoLoad User Path

The NeoLoad Controller used to convert the Tosca SAP script must be 32 bits and launched in process mode (service mode is not supported). More information about NeoLoad Prerequisites for SAP GUI on [NeoLoad documentation](https://www.neotys.com/documents/doc/neoload/latest/en/html/#27113.htm).

In Tricentis Tosca, right click on an execution of a Test Case and then **NeoLoad Add-on > Transfer SAP test case to NeoLoad**
NeoLoad starts the **SAP recording** at the first step named "SAP" or "SAP Login", and stops it at the end of the test case.

<p align="center"><img src="/screenshots/transfertSAPtoNeoload.png" alt="transfer" /></p>

<p align="center"><img src="/screenshots/userpath.png" alt="user path" /></p>

## How to convert a Tricentis Tosca Web or API script to a NeoLoad User Path

In Tricentis Tosca, right click on an execution of a Test Case and then **NeoLoad Add-on > Transfer Web test case to NeoLoad**
NeoLoad starts the **Web recording** at the beginning of the test case, and stops it at the end.

<p align="center"><img src="/screenshots/transfertWEBtoNeoload.png" alt="transfer" /></p>

## User Path Update

During the execution of the Tricentis Tosca test case, if the NeoLoad User Path does not exist, it will be created. Otherwise, the existing User Path will be updated thanks to the User Path Update feature.
The User Path Update feature merges the original User Path with a newer recording, copying variable extractors and variables. Below the SAP GUI User Path in NeoLoad.

## Measure the end user experience in NeoLoad leveraging Tricentis Tosca (Beta)

_(Tosca EUX is currently in beta and subject to change)_

This integration allows Tosca to communicate with NeoLoads DataExchangeApi to send Tosca's TestStep and TestCase duration timings to NeoLoad during execution.
This helps when analysis of the [End User Experience](https://www.neotys.com/blog/why-end-user-experience-is-important-2/) (EUX) in NeoLoad is required.

**Setup**

1. In Tosca set the following [test configuration parameters](https://support.tricentis.com/community/manuals_detail.do?lang=en&url=tosca_commander/tcp_creation.htm) on ExecutionLists or ExecutionEntries which should be executed as part of your EUX test

	Name | Value | Description
	------------ | ------------- | -------------
    SendEndUserExperienceToNeoLoad | True/False | Whether the NeoLoad DataExchangeApi should be used to send End User Experience metrics to NeoLoad.
	NeoLoadApiHost |  Default: localhost | (Optional) The hostname of the NeoLoad DataExchangeApi
	NeoLoadApiPort |  Default: 7400 | (Optional) The port of the NeoLoad DataExchangeApi
	NeoLoadApiKey |  Default: empty | (Optional) E.g. abcb6dcd-ea95-4a6a-9c64-80ff55ff778d

	Alternatively this can also be set as part of your TCShell script as shown in [examples/RunToscaExecution.tcs](./examples/RunToscaExecution.tcs)
	
	_Hint: Set the repetition property on the ExecutionEntry to let one execution run repeatedly. This will increase the TPS as the workspace does not have to be reopened by the UserPath constantly._	

2. Prepare your Tosca execution environment to allow executions via the command line
  * Using a [TCShell Script](https://support.tricentis.com/community/manuals_detail.do?lang=en&url=tosca_commander/script_mode.htm) (for local execution when Tosca is installed on the same machine as the NeoLoad LoadGenerator)\
    Example script: [examples/RunToscaExecution.tcs](./examples/RunToscaExecution.tcs)
  * Or using the [Tosca CI Integration](https://support.tricentis.com/community/manuals_detail.do?lang=en&url=continuous_integration/concept.htm) (for remote execution when Tosca is not installed on the same machine as the NeoLoad LoadGenerator)

4. Create separate EUX User Path in Neoload and add an [Executable Test Script](https://www.neotys.com/documents/doc/neoload/latest/en/html/#8677.htm) Action to it which will trigger the execution of an Tosca Execution List or Entry 
  * Using the `TCShell.exe` Example: \
	`TCShell -executionmode -workspace "C:\Tosca_Projects\Tosca_Workspaces\PathToYour\Workspace.tws" "${NL-CustomResources}\RunToscaExecution.tcs"`
  * Or using the `ToscaCIClient.exe`
  <p align="center"><img src="/screenshots/Tosca-EUX-NeoLoad.png" alt="Tosca EUX User Path" /></p>

5. Trigger the load test in combination with your EUX User Path in NeoLoad. Only one instance of Tosca should be run per LoadGenerator.\
	
	_Hint: It is recommended to run the UEX UserPath by defining a iteration number or to set the Population Parameter Stop Policy to Indeterminate. This will allow last iteration to close Tosca workspace without locking it._

_The above example focuses on the execution via command line, Tosca test cases can alternatively be remotely executed via the [Tosca Rest API](https://support.tricentis.com/community/manuals_detail.do?url=restapi/prerequisites.htm&tcapi=tcrsapi)_

Aditionally to the TestStep and TestCase durations timings the integration will collect browser performance timings similar to the [NeoLoad Selenium EUX integration](https://www.neotys.com/documents/doc/neoload/latest/en/html/#23676.htm) when automating web applications.

<p align="center"><img src="/screenshots/Tosca-EUX-NeoLoad-Metrics.png" alt="Tosca EUX Performance Metrics" /></p>


## Troubleshooting
From Tosca version 12.2, if execution errors are not displayed in Tosca Commander (Loginfo column displays "Stack empty."), check file **neoload-add-on-error.txt** located in your user profile directory (for example C:\Users\<username>\neoload-add-on-error.txt).

### Unable to connect to the remote server (No connection can be established to 127.0.0.1:7400)
Make sure NeoLoad is running, on same host specified by IP address (127.0.0.1 for localhost), and make sure NeoLoad design API is listening on port specified (by default 7400). 

### NL-RECORDING-NOT-LICENSED
You need a valid license with Integration & Advanced Usage. 

### NL-DESIGN-CANNOT-GET-CONTAINS-USER-PATH (No project is opened.)
NeoLoad is running, but there is no project open. Make sure to wait until project is fully loaded in NeoLoad before converting Tosca script.

### NL-DESIGN-CANNOT-START-SAP-RECORDING (There is no active SAP connection)
The API call sent from the NeoLoad add-on to the NeoLoad Controller Design API was performed when there was no active SAP connection. 
Make sure there is an active SAP connection when the conversion starts. If needed, adjust name of test case actions to allow a delayed start. The add-on will trigger the "Start SAP recording" either on action pre-execution or post-execution, as coded [here](https://github.com/Neotys-Labs/Tricentis-Tosca/blob/master/NeoloadTBoxProxy/listener/TestActionListener.cs).

### NL-DESIGN-ILLEGAL-STATE-FOR-OPERATION
Different causes that can produce an NL-DESIGN-ILLEGAL-STATE-FOR-OPERATION:
* A test is running.
* NeoLoad is busy.
* A recording is already in progress.
* The post-recording wizard is still open or running.
* No workbench instance found.
* A recording is already stopping.
* Recording is already paused.
* Recording is not paused.
* Not currently recording.

## ChangeLog

* Version 2.6.0 (December 15, 2021):
   * Support SAP Gui and Web at once test case
   * Improve created transaction in NeoLoad when there is no folders in Tosca.

* Version 2.5.1 (October 22, 2021):
   * Fixed Web Transfer when the system proxy uses the "AutoConfigURL".

* Version 2.5.0 (August 3, 2021): 
   * Basic Support for end user experience testing using Tosca.

* Version 2.4.0 (June 19, 2021): 
   * Create a new transaction in NeoLoad from Tosca test step or folder for both Web and SAP GUI.
   * Tosca version below 12.2 is not supported anymore.

* Version 2.3.0 (October 13, 2020): Add option to enable/disable HTTP2 during NeoLoad recording

* Version 2.2.0 (July 15, 2020): API test recording.
   * Support API test case recording
   * Support of Tosca version 13.3

* Version 2.1.0 (May 08, 2020): Make transactions when recording web test case.
   * Support of Tosca version 13.1 and 13.2

* Version 2.0.0 (April 27, 2020): Stabilization.
   * Support of Tosca version 13.0
   * Support Web test case recording
   * Create a new transaction in NeoLoad at each SAP TCode encountered during the recoding. This feature is available since NeoLoad 7.3
   * Make installation procedure easier

* Version 1.0.0 (November 30, 2018): Initial release.
   * July 24, 2019: Support of Tosca version 12.2
