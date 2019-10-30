The MidiFileReader package is a collection of scripts that extend the MidiSharp library by Stephen Toub available for download from his github
page at https://github.com/stephentoub/MidiSharp

For in depth examples of the scripts working, see example project.

By adding a Midi event listener component to a gameobject, functions from other scripts can be triggered based on the
events of an assigned midi file and the parameters defined within the midi event listener.

Midi file path - Firstly, the correct file path must be entered, this can be done by right clicking the desired midi file in the file browser and choosing
copy file path. This can then be pasted into the midi file path field of the event listener.

Track number in file - The track number is defaulted to 1 as would contain only meta information. This is useful if you have a midi file with mulitple tracks,
for example, a number of different instruments in one file. Changing this number will allow you read through different tracks.

Event type to listen for - If none of these are selected then no events will be listened for.

Filter notes - Instead of looking for specific notes, you might want to listen for notes below or above a certain threshold. Modifying these 
allows you to filter out and listen for notes in that desired threshold.

Listen for specific notes - Selecting this will allow you to listen for specific notes. In order to do this successfully, the list,
note values to listen for, should be modified accordingly. Give it a size, and then type in the specific notes you are looking for.

NOTE: Note filters will still apply, even when looking for specific note.

Note on and off events - These will allow you to define what will happen on each event you are listening for. Add a new function with the 
+ button and drag a gameobject into the empty field. This will give you access to all of the scripts on that gameobject and there
public functions. If you wish to pass the data of each event to this function, you must define a NoteEventData parameter for the function,
a class found in the MidiFileReader namespace. An example can be found below

public NoteOnEvent(NoteEventData data)
{
	//Do ting
}

The value, timing, and velocity for each event can then be accessed through the data parameter.

Master clock - In order for midi event listeners to iterate through their assigned track, the master clock component must be 
added on am object within your scene. This component is responsible for updating the timers of each midi event listener
and without it, no events will be registered.