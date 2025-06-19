using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Common;
using TagTool.IO;
using TagTool.Tags.Definitions;
using TagTool.Commands.Common;
using TagTool.Geometry;
using TagTool.Tags;
using TagTool.Tags.Resources;
using TagTool.Geometry.Utils;

namespace TagTool.Commands.Scenarios
{
    class ConvertInstancedGeometryCommand : Command
    {
        private GameCacheHaloOnlineBase HoCache { get; }
        private Scenario Scnr { get; }

        public ConvertInstancedGeometryCommand(GameCache cacheContext, Scenario scnr) :
            base(false,
                "ConvertInstancedGeometry",
                "Convert Instanced Geometry in Halo Online maps to forge objects",
                "ConvertInstancedGeometry <BspIndex> [nocenter] [<Instance index or 'all'> [New Tagname]]",
                "Convert Instanced Geometry in Halo Online maps to forge objects")
        {
            HoCache = (GameCacheHaloOnlineBase)cacheContext;
            Scnr = scnr;
        }

        public override object Execute(List<string> args)
        {
            var argStack = new Stack<string>(args.AsEnumerable().Reverse());
            if (argStack.Count < 1)
                return new TagToolError(CommandError.ArgCount, "Expected bsp index!");

            if (!int.TryParse(argStack.Pop(), out int sbspIndex))
                return new TagToolError(CommandError.ArgInvalid, "Invalid bsp index");

            bool centerGeometry = true;
            if (argStack.Count > 0 && argStack.Peek().Equals("nocenter", StringComparison.OrdinalIgnoreCase))
            {
                argStack.Pop();
                centerGeometry = false;
            }

            var desiredInstances = new Dictionary<int, string>();
            var convertedMeshes = new HashSet<short>();

            using (var readStream = HoCache.OpenCacheReadWrite())
            using (var writeStream = HoCache.OpenCacheReadWrite())
            {
                var bspRef = Scnr.StructureBsps[sbspIndex].StructureBsp;
                var hoSbsp = HoCache.Deserialize<ScenarioStructureBsp>(readStream, bspRef);

                if (argStack.Count > 0 && argStack.Peek().Equals("all", StringComparison.OrdinalIgnoreCase))
                {
                    argStack.Pop();
                    for (int i = 0; i < hoSbsp.InstancedGeometryInstances.Count; i++)
                    {
                        short meshIdx = hoSbsp.InstancedGeometryInstances[i].DefinitionIndex;
                        if (convertedMeshes.Add(meshIdx))
                            desiredInstances.Add(i, null);
                    }
                }
                else if (argStack.Count > 0)
                {
                    if (!int.TryParse(argStack.Pop(), out int instIdx))
                        return new TagToolError(CommandError.ArgInvalid, "Invalid instance index");

                    string newName = argStack.Count > 0 ? argStack.Pop() : null;
                    desiredInstances.Add(instIdx, newName);
                }
                else
                {
                    Console.WriteLine(new string('-', 66));
                    Console.WriteLine("Enter each instance with the format <Index> [New tagname]");
                    Console.WriteLine("Enter a blank line to finish.");
                    Console.WriteLine(new string('-', 66));

                    string line;
                    while (!string.IsNullOrWhiteSpace(line = Console.ReadLine()))
                    {
                        var parts = line.Split(' ', 2);
                        if (!int.TryParse(parts[0], out int instIdx))
                            return new TagToolError(CommandError.OperationFailed);
                        string name = parts.Length > 1 ? parts[1] : null;
                        desiredInstances.Add(instIdx, name);
                    }
                }

                if (desiredInstances.Count == 0)
                    return true;

                var converter = new GeometryToObjectConverter(
                    HoCache, readStream,
                    HoCache, writeStream,
                    Scnr, sbspIndex);

                foreach (var kv in desiredInstances)
                {
                    try
                    {
                        converter.ConvertGeometry(
                            kv.Key,          // instance index
                            kv.Value,        // custom tag name
                            false,           // skip immediate saves
                            centerGeometry   // center if requested
                        );
                    }
                    finally
                    {
                        HoCache.SaveStrings();
                        HoCache.SaveTagNames();
                    }
                }
            }

            return true;
        }
    }
}
