////////////////////////////////////////////////////////////////
//
//		First-Person-Shooter Mod
//			for Grand Theft Auto: IV
//			and Episodes From Liberty City (untested!!)
//
//		By thaCURSEDpie
//
//		Version 0.1.1.0 BETA
//
//			as always, read the license!
//
////////////////////////////////////////////////////////////////


//Description
/////////////
This mod spices GTA up a bit! It brings kill-notifications (like in first-person-shooters) and kill sounds ("HEADSHOT!", "MULTIKILL!"), which get triggered dynamically.


As mentioned in the header, this mod has not been tested under the EFLC (because I dont have them...), therefore the behaviour might be unexpected!


//Installation
//////////////
Extract the archive to your main game directory (where "LaunchGTAIV.exe" or "LaunchEFLC.exe" is).

You will need an ASI-loader   (http://www.gtaforums.com/index.php?showtopic=380830)
And HazardX's .Net scripthook (http://www.gtaforums.com/index.php?showtopic=392325)

For the .Net scripthook to work, you also need:
.Net framework 4 (http://www.microsoft.com/downloads/details.aspx?FamilyID=9cfb2d51-5ff4-4491-b0e5-b386f32c0992&displaylang=en)
And Visual C++ library 2010 (http://www.microsoft.com/downloads/details.aspx?FamilyID=a7b7a05e-6de6-4d3a-a423-37bf0912db84&displaylang=en%C2%A0)

(you'd need those later on anyway ;)  )


//Usage
///////
You can use the following console-commands (open the console by pressing the "~" key, next to the "1" key):

fpsmod_off				disables FPSmod
fpsmod_on				enables FPSmod
fpsmod_mute				mutes the mod (no kill sounds)
fpsmod_unmute			unmutes the mod (kill sounds on)
fpsmod_reloadsettings	reload the .ini file
fpsmod_hideicons		hide the kill icons
fpsmod_showicons		show the kill icons


//Advanced
//////////
You can make some adjustments in the provided .ini file. You can also use your own sounds and images.
This can be done by replacing the images/sounds by other images/sounds with the same name.

NOTE: images should be in the PNG format, with dimensions of 256x128. (see also the "example.png" file)


//Known issues
//////////////
- sometimes the headshot glow is visible, but not the weapon icon
- sometimes no weapon icon is visible
- sometimes the vehicle_explosion icon is visible, while no vehicle explosion has occured
- sometimes the kill icons dont align properly (new icons show up above older icons)


//Changelog
///////////

v0.1.1.0:
- (hopefully) fixed p90 ("assault smg") kills not showing upt
- added debug text which might help me to solve any issues. it prints text to the console in the following format:
"[FPSmod-dbg]: <type of kill> <current stat>* <killstart>**"
	*: 			the number of kills the game itself has counted, with this specific weapon
	**:			the number of kills the mod has counted, with this specific weapon. Should always equal (currentstat - 1)

- added new setting to enable/disable debug texts. 
	The corresponding console commands are:
		"fpsmod_hidedebugtexts"	-	to hide the debug texts
		"fpsmod_showdebugtexts"	-	to show the debug texts
	The corresponding variable in the .ini file is "showDbgTexts", which defaults to true
		

//Contact
/////////
PM me @gtaforums.com. Nickname: "thaCURSEDpie".


//License
/////////
This software is provided 'as-is', without any express or implied
warranty.  In no event will the author be held liable for any damages
arising from the use of this software.

Permission is granted to anyone to use redistribute this software 
freely, subject to the following restriction:

The origin of this software must not be misrepresented; you must not
claim that you wrote the original software. If you use this software
in a product, an acknowledgment in the product documentation would be
appreciated but is not required.  