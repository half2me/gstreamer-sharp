using System;
using Gst;

public static class GstTypefindTest
{
    private static TypeFindElement typefind;
    
    public static void Main(string [] args)
    {
        Application.Init();
    
        Pipeline pipeline = new Pipeline("pipeline");
        Element source = ElementFactory.Make("filesrc", "source");
        typefind = TypeFindElement.Make("typefind");
        Element sink = ElementFactory.Make("fakesink", "sink");

        source.SetProperty("location", args[0]);
        
        typefind.HaveType += OnHaveType;
        
        pipeline.AddMany(source, typefind, sink);
        source.Link(typefind);
        typefind.Link(sink);

        pipeline.SetState(State.Paused);
        pipeline.SetState(State.Null);

        pipeline.Dispose();
    }
    
    private static void OnHaveType(object o, HaveTypeArgs args) 
    {
        args.Caps.Refcount++;
        Console.WriteLine("MimeType: {0}, {1}", args.Caps, typefind.Caps);            
    }
}

