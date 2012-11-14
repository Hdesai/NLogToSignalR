# NLogToSignalR
**NLogToSignalR** is a NLog target for delivering NLog notification via SignalR framework in form of notification. It supports both PersistentConnection and Hub to deliver notification. 

## Current Version
Alpha.


## Code
Code can be found at https://github.com/Hdesai/NLogToSignalR

## Quick start

###4 Easy Steps


1. Put NLog.Targets.SignalR in the same folder as your App.Config/ Web.Config file.

2. Change NLog Configuration.
  e.g. <targets>
          <target name="a1" type="NLogToSignalR"
                  uri="http://localhost/SignalRAspNetServer"
                  HubName="messenger"
                  GroupName="sourcing"
                  CallViaPersistentConnection="false"
		  MethodToCall="broadCastMessage"
                  layout="$(message)$(exception:tostring}"/>        
       </targets>

3. Deploy the Server (NLog.Targets.SignalR.AspNetServer) on local IIS.

4. Browse the website after deployment and start logging!

        // Display an info toast with no title
        toastr.info('Are you the 6 fingered man?')

## Authors

**Himanshu Desai**

+ http://twitter.com/h_desai

## Credits
Special thanks to three open source projects.
NLog,SignalR and toastr


## Copyright

Copyright © 2012 [Himanshu Desai](http://twitter.com/h_desai) 

## License 

NLogToSignalR is under MIT license - http://www.opensource.org/licenses/mit-license.php