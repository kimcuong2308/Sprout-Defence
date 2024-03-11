using Godot;
using System;

public partial class CustomSignalSingleton : Node
{
	
	[Signal] public delegate void getDamgeByPlayerEventHandler(float dmg);
}
