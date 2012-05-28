This the readme file for the NAppUpdate FeedBuilder.


=== NAppUpdate CONCEPTS and USAGE =============================================

The NAppUpdater (NAU) provides a method for describing actions to perform, 
related to updating your application. It uses a special XML file (NauXML) which
contains the description of the updates, what logic to use to determine if
updates are needed, and what actions are necessary to perform the updates.

Here's how to include the NAU in your project.
1. Add references to NAppUpdate.Framework.dll.
2. Add a class to your project (or add logic to an existing class) to check
   for an update. The NAU project contains a Windows Forms example and a WPF
   example for how you can do this.
3. You'll want to modify the "source" variable so it points to your particular 
   updates repository.
4. Add a post-build event command line to your project, so it calls the 
   FeedBuilder (refer to command line options and examples below). The first 
   time you build the project, select the appropriate FeedBuilder options and 
   save the configuration.
5. Create an installer that includes whatever files your project needs for an
   initial install. At minimum (we could call this a web installer), you need 
   the NAppUpdate.Framework.dll and an executable that checks your repository 
   for updates.

Here's how to release an update.
1. Build your project
2. Run the FeedBuilder and perform the Build so that it outputs the update 
   items to a folder. If you've added the FeedBuilder as a post-build event,
   this should happen automatically.
3. Open the folder where the feed outputs were generated.
4. Upload the feed outputs to your updates repository.

Here's how NAppUpdate works in general.
1. Your application defines a source and gets an instance of the UpdateManager.
   For example, if you use the SimpleWebSource, you simply provide it with the 
   URL to the remote folder of your repository.
2. When your application determines it is time to check for an update, it 
   calls one of the NAU CheckForUpdates methods.
3. NAU downloads the NauXML file and compares it to the running application
   to determine whether an update is available.
4. Your application uses the NAU response to initiate the preparation of the 
   update (e.g. downloading files to a temporary folder) using one of NAU's
   PrepareUpdates methods.
5. NAU extracts an updater executable and places in a temporary folder.
6. When it is ready, your application calls NAU's ApplyUpdates method. 
   This will...
   a) terminate your application (if a cold update is needed), 
   b) perform the update actions (e.g. update files), and then 
   c) restart your application (if it was terminated).


=== COMMAND LINE OPTIONS ======================================================

The FeedBuilder recognizes the following command line options. 
The order of the options does not matter.
All options are not case sensitive.

(path and filename to a FeedBuilder config file)
        Open the file for processing. If the config file doesn't exist, the GUI 
		will be displayed so you can configure the feed and save the config 
		file for subsequent command line invocation.

Build
        Build the feed. This is the same as clicking on the Build button on the
        GUI, except the outputs folder will only be shown if the OpenOutputs
		option is present.

ShowGui
       Show the GUI

OpenOutputs
       After building, open the outputs folder. The outputs folder is always
	   displayed if the GUI is shown.


=== EXAMPLES ==================================================================

Here's a typical scenario of a Visual Studio project, where the 
FeedBuilder.config file is placed in the same folder as the .sln file. This may
be appropriate for a debug project, where you might not want to upload files 
every time the project is built (so the output folder is not opened after the
build). You would place this command in your Visual Studio project's Post Build 
event command line:

   "C:\PathTo\FeedBuilder.exe" "$(SolutionDir)FeedBuilder_Debug.config" -Build


Here's a typical scenario for the same project as above, except applied to the 
Release build, where it might make sense to open the outputs folder so you can 
upload them after they are processed:

   "C:\PathTo\FeedBuilder.exe" "$(SolutionDir)FeedBuilder_Release.config" -Build -OpenOutputs
