# MultiTeams <img align="right" src="MultiTeams.png" style="width: 64px" />

Tired of switching between tenants with your MS Teams? Annoyed, that only one tenant is starting upon reboot?

Boy have we a product for you: Here comes "MultiTeams". A small footprint, installer free, easy to use system tray application, taking off your burden of dealing with secondary profiles.

The application acts as a small launcher capable of managing multiple MS Teams instances side by side and autostart them upon reboot.

## Usage

In order to use the tool, no administrative privileges are needed. Simply run it to try it out. Thus, using the tool is fairly straight forward:

1. Download or build the executable.
2. Run it (it will directly sneak into your system tray, without bothering you any further).
3. Use it by either adding/managing your profiles or launching your different instances and setting them up according to your needs.

All functionality of the tool is available through the tray icon's context menu. The application also automatically detects direct edits of its configuration file and reloads the configuration upon every change.

## Installation procedures

### Installation

If you like the tool you can set it up to run automatically upon startup, by going to the `Settings` menu and tick `Autostart`. This will automatically copy the application and its configuration to the `%APPDATA%` folder in your system and set it up for autostart.

### Controlling autostart behavior

You can set/remove the autostart feature any time by again selecting `Autostart` in the `Settings` menu. While turning autostart off, it won't touch any of your other settings, let alone resource occupation.

### Deinstallation

In case you don't want the tool anymore, select `Uninstall` from the `Settings` menu. This will remove the tool and all configuration settings from your system upon next reboot.

!!! caution MS Teams profiles are *not* removed!
    Even if all the tool's files are removed upon reboot, the MS Teams profiles created while using the tool remain untouched and need to be removed manually. 

    Unless you've changed anything in the configuration file, you find the side-by-side-profiles in your MS Teams-folder:
    `%LOCALAPPDATA%\\Microsoft\\Teams\\CustomProfiles\\` 

## Technical background / Kudos

The application is based on the batch-File based approach as published in [the Microsoft Tech Community](https://techcommunity.microsoft.com/t5/microsoft-teams/multiple-instances-of-microsoft-teams-application/m-p/1306051) by [Satish2805](https://techcommunity.microsoft.com/t5/user/viewprofilepage/user-id/259632): Prior starting the MS Teams instance, the user's environment variable `USERPROFILE` is modified such that it points to a separate profile folder for each instance created.
