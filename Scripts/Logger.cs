using Godot;
using System;

public partial class Logger : Node
{
    private static Logger _instance;

    // Mutex to ensure safe access to the log file.
    private static Mutex logMutex = new Mutex();

    // File path for the log file
    private static string logFilePath = "user://log.txt";

    private static FileAccess file;


    private Logger() { }

    public static Logger Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Logger();
                file = FileAccess.Open(logFilePath, FileAccess.ModeFlags.ReadWrite);
            }
            return _instance;
        }
    }

    public void Print(string message)
    {
        GD.Print(message);
    }

    public void Log(string message)
    {
        logMutex.Lock();

        String currentTime = Time.GetDatetimeStringFromSystem();
        string full_message = $"{currentTime} | {message}";
        file.StoreLine(full_message);

        logMutex.Unlock();
    }
}

// Example of usage:
// Logger.Instance.Print("Hello, World!");
// Logger.Instance.Log("This is a log message.");
