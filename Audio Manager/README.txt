(The event system used together with this audio manager was not made by me)
The event system is used for calling the audio manager, but is based on a mix of GDC talks and YT videos on scriptable object events.

It is at the moment however advised not to use it, as scriptable obejct events cause a lot of garbage if called too often!
I will perhaps at later point make my own abstract event system that can be used with the audio manager instead, to fix the garbage collection issue currently present.
