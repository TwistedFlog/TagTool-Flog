using System.IO;
using TagTool.Cache.HaloOnline;
using TagTool.Cache;
using System.Collections.Generic;
using TagTool.Common;
using TagTool.Shaders;
using TagTool.Tags.Definitions;

namespace TagTool.MtnDewIt.Shaders.RenderMethodDefinitions.Shaders
{
    public class ParticleDefinition : RenderMethodData
    {
        public ParticleDefinition(GameCache cache, GameCacheHaloOnline cacheContext, Stream stream) : base
        (
            cache,
            cacheContext,
            stream
        )
        {
            Cache = cache;
            CacheContext = cacheContext;
            Stream = stream;
            RenderMethod();
        }

        public override void RenderMethod()
        {
            var tag = GenerateTag<RenderMethodDefinition>($@"shaders\particle");
            var rmdf = CacheContext.Deserialize<RenderMethodDefinition>(Stream, tag);
            rmdf.GlobalOptions = GenerateTag<RenderMethodOption>($@"shaders\particle_options\global_particle_options");
            rmdf.Categories = new List<RenderMethodDefinition.CategoryBlock>
            {
                new RenderMethodDefinition.CategoryBlock
                {
                    Name = CacheContext.StringTable.GetOrAddString($@"albedo"),
                    ShaderOptions = new List<RenderMethodDefinition.CategoryBlock.ShaderOption>
                    {
                        new RenderMethodDefinition.CategoryBlock.ShaderOption
                        {
                            Name = CacheContext.StringTable.GetOrAddString($@"diffuse_only"),
                            Option = GenerateTag<RenderMethodOption>($@"shaders\particle_options\albedo_diffuse_only"),
                            VertexFunction = StringId.Invalid,
                            PixelFunction = StringId.Invalid,
                        },
                        new RenderMethodDefinition.CategoryBlock.ShaderOption
                        {
                            Name = CacheContext.StringTable.GetOrAddString($@"diffuse_plus_billboard_alpha"),
                            Option = GenerateTag<RenderMethodOption>($@"shaders\particle_options\albedo_diffuse_plus_billboard_alpha"),
                            VertexFunction = StringId.Invalid,
                            PixelFunction = StringId.Invalid,
                        },
                        new RenderMethodDefinition.CategoryBlock.ShaderOption
                        {
                            Name = CacheContext.StringTable.GetOrAddString($@"palettized"),
                            Option = GenerateTag<RenderMethodOption>($@"shaders\particle_options\albedo_palettized"),
                            VertexFunction = StringId.Invalid,
                            PixelFunction = StringId.Invalid,
                        },
                        new RenderMethodDefinition.CategoryBlock.ShaderOption
                        {
                            Name = CacheContext.StringTable.GetOrAddString($@"palettized_plus_billboard_alpha"),
                            Option = GenerateTag<RenderMethodOption>($@"shaders\particle_options\albedo_palettized_plus_billboard_alpha"),
                            VertexFunction = StringId.Invalid,
                            PixelFunction = StringId.Invalid,
                        },
                        new RenderMethodDefinition.CategoryBlock.ShaderOption
                        {
                            Name = CacheContext.StringTable.GetOrAddString($@"diffuse_plus_sprite_alpha"),
                            Option = GenerateTag<RenderMethodOption>($@"shaders\particle_options\albedo_diffuse_plus_sprite_alpha"),
                            VertexFunction = StringId.Invalid,
                            PixelFunction = StringId.Invalid,
                        },
                        new RenderMethodDefinition.CategoryBlock.ShaderOption
                        {
                            Name = CacheContext.StringTable.GetOrAddString($@"palettized_plus_sprite_alpha"),
                            Option = GenerateTag<RenderMethodOption>($@"shaders\particle_options\albedo_palettized_plus_sprite_alpha"),
                            VertexFunction = StringId.Invalid,
                            PixelFunction = StringId.Invalid,
                        },
                        new RenderMethodDefinition.CategoryBlock.ShaderOption
                        {
                            Name = CacheContext.StringTable.GetOrAddString($@"diffuse_modulated"),
                            Option = GenerateTag<RenderMethodOption>($@"shaders\particle_options\albedo_diffuse_modulated"),
                            VertexFunction = StringId.Invalid,
                            PixelFunction = StringId.Invalid,
                        },
                        new RenderMethodDefinition.CategoryBlock.ShaderOption
                        {
                            Name = CacheContext.StringTable.GetOrAddString($@"palettized_glow"),
                            Option = GenerateTag<RenderMethodOption>($@"shaders\particle_options\albedo_palettized_glow"),
                            VertexFunction = StringId.Invalid,
                            PixelFunction = StringId.Invalid,
                        },
                        new RenderMethodDefinition.CategoryBlock.ShaderOption
                        {
                            Name = CacheContext.StringTable.GetOrAddString($@"palettized_plasma"),
                            Option = GenerateTag<RenderMethodOption>($@"shaders\particle_options\albedo_palettized_plasma"),
                            VertexFunction = StringId.Invalid,
                            PixelFunction = StringId.Invalid,
                        },
                        new RenderMethodDefinition.CategoryBlock.ShaderOption
                        {
                            Name = CacheContext.StringTable.GetOrAddString($@"palettized_2d_plasma"),
                            Option = GenerateTag<RenderMethodOption>($@"shaders\particle_options\albedo_palettized_plasma"),
                            VertexFunction = StringId.Invalid,
                            PixelFunction = StringId.Invalid,
                        },
                    },
                    VertexFunction = StringId.Invalid,
                    PixelFunction = StringId.Invalid,
                },
                new RenderMethodDefinition.CategoryBlock
                {
                    Name = CacheContext.StringTable.GetOrAddString($@"blend_mode"),
                    ShaderOptions = new List<RenderMethodDefinition.CategoryBlock.ShaderOption>
                    {
                        new RenderMethodDefinition.CategoryBlock.ShaderOption
                        {
                            Name = CacheContext.StringTable.GetOrAddString($@"opaque"),
                            Option = null,
                            VertexFunction = StringId.Invalid,
                            PixelFunction = StringId.Invalid,
                        },
                        new RenderMethodDefinition.CategoryBlock.ShaderOption
                        {
                            Name = CacheContext.StringTable.GetOrAddString($@"additive"),
                            Option = null,
                            VertexFunction = StringId.Invalid,
                            PixelFunction = StringId.Invalid,
                        },
                        new RenderMethodDefinition.CategoryBlock.ShaderOption
                        {
                            Name = CacheContext.StringTable.GetOrAddString($@"multiply"),
                            Option = null,
                            VertexFunction = StringId.Invalid,
                            PixelFunction = StringId.Invalid,
                        },
                        new RenderMethodDefinition.CategoryBlock.ShaderOption
                        {
                            Name = CacheContext.StringTable.GetOrAddString($@"alpha_blend"),
                            Option = null,
                            VertexFunction = StringId.Invalid,
                            PixelFunction = StringId.Invalid,
                        },
                        new RenderMethodDefinition.CategoryBlock.ShaderOption
                        {
                            Name = CacheContext.StringTable.GetOrAddString($@"double_multiply"),
                            Option = null,
                            VertexFunction = StringId.Invalid,
                            PixelFunction = StringId.Invalid,
                        },
                        new RenderMethodDefinition.CategoryBlock.ShaderOption
                        {
                            Name = CacheContext.StringTable.GetOrAddString($@"maximum"),
                            Option = null,
                            VertexFunction = StringId.Invalid,
                            PixelFunction = StringId.Invalid,
                        },
                        new RenderMethodDefinition.CategoryBlock.ShaderOption
                        {
                            Name = CacheContext.StringTable.GetOrAddString($@"multiply_add"),
                            Option = null,
                            VertexFunction = StringId.Invalid,
                            PixelFunction = StringId.Invalid,
                        },
                        new RenderMethodDefinition.CategoryBlock.ShaderOption
                        {
                            Name = CacheContext.StringTable.GetOrAddString($@"add_src_times_dstalpha"),
                            Option = null,
                            VertexFunction = StringId.Invalid,
                            PixelFunction = StringId.Invalid,
                        },
                        new RenderMethodDefinition.CategoryBlock.ShaderOption
                        {
                            Name = CacheContext.StringTable.GetOrAddString($@"add_src_times_srcalpha"),
                            Option = null,
                            VertexFunction = StringId.Invalid,
                            PixelFunction = StringId.Invalid,
                        },
                        new RenderMethodDefinition.CategoryBlock.ShaderOption
                        {
                            Name = CacheContext.StringTable.GetOrAddString($@"inv_alpha_blend"),
                            Option = null,
                            VertexFunction = StringId.Invalid,
                            PixelFunction = StringId.Invalid,
                        },
                        new RenderMethodDefinition.CategoryBlock.ShaderOption
                        {
                            Name = CacheContext.StringTable.GetOrAddString($@"pre_multiplied_alpha"),
                            Option = null,
                            VertexFunction = StringId.Invalid,
                            PixelFunction = StringId.Invalid,
                        },
                    },
                    VertexFunction = StringId.Invalid,
                    PixelFunction = StringId.Invalid,
                },
                new RenderMethodDefinition.CategoryBlock
                {
                    Name = CacheContext.StringTable.GetOrAddString($@"specialized_rendering"),
                    ShaderOptions = new List<RenderMethodDefinition.CategoryBlock.ShaderOption>
                    {
                        new RenderMethodDefinition.CategoryBlock.ShaderOption
                        {
                            Name = CacheContext.StringTable.GetOrAddString($@"none"),
                            Option = null,
                            VertexFunction = StringId.Invalid,
                            PixelFunction = StringId.Invalid,
                        },
                        new RenderMethodDefinition.CategoryBlock.ShaderOption
                        {
                            Name = CacheContext.StringTable.GetOrAddString($@"distortion"),
                            Option = GenerateTag<RenderMethodOption>($@"shaders\particle_options\specialized_rendering_distortion"),
                            VertexFunction = StringId.Invalid,
                            PixelFunction = StringId.Invalid,
                        },
                        new RenderMethodDefinition.CategoryBlock.ShaderOption
                        {
                            Name = CacheContext.StringTable.GetOrAddString($@"distortion_expensive"),
                            Option = GenerateTag<RenderMethodOption>($@"shaders\particle_options\specialized_rendering_distortion"),
                            VertexFunction = StringId.Invalid,
                            PixelFunction = StringId.Invalid,
                        },
                        new RenderMethodDefinition.CategoryBlock.ShaderOption
                        {
                            Name = CacheContext.StringTable.GetOrAddString($@"distortion_diffuse"),
                            Option = GenerateTag<RenderMethodOption>($@"shaders\particle_options\specialized_rendering_distortion"),
                            VertexFunction = StringId.Invalid,
                            PixelFunction = StringId.Invalid,
                        },
                        new RenderMethodDefinition.CategoryBlock.ShaderOption
                        {
                            Name = CacheContext.StringTable.GetOrAddString($@"distortion_expensive_diffuse"),
                            Option = GenerateTag<RenderMethodOption>($@"shaders\particle_options\specialized_rendering_distortion"),
                            VertexFunction = StringId.Invalid,
                            PixelFunction = StringId.Invalid,
                        },
                    },
                    VertexFunction = StringId.Invalid,
                    PixelFunction = StringId.Invalid,
                },
                new RenderMethodDefinition.CategoryBlock
                {
                    Name = CacheContext.StringTable.GetOrAddString($@"lighting"),
                    ShaderOptions = new List<RenderMethodDefinition.CategoryBlock.ShaderOption>
                    {
                        new RenderMethodDefinition.CategoryBlock.ShaderOption
                        {
                            Name = CacheContext.StringTable.GetOrAddString($@"none"),
                            Option = null,
                            VertexFunction = StringId.Invalid,
                            PixelFunction = StringId.Invalid,
                        },
                        new RenderMethodDefinition.CategoryBlock.ShaderOption
                        {
                            Name = CacheContext.StringTable.GetOrAddString($@"per_pixel_ravi_order_3"),
                            Option = null,
                            VertexFunction = StringId.Invalid,
                            PixelFunction = StringId.Invalid,
                        },
                        new RenderMethodDefinition.CategoryBlock.ShaderOption
                        {
                            Name = CacheContext.StringTable.GetOrAddString($@"per_vertex_ravi_order_0"),
                            Option = null,
                            VertexFunction = StringId.Invalid,
                            PixelFunction = StringId.Invalid,
                        },
                    },
                    VertexFunction = StringId.Invalid,
                    PixelFunction = StringId.Invalid,
                },
                new RenderMethodDefinition.CategoryBlock
                {
                    Name = CacheContext.StringTable.GetOrAddString($@"render_targets"),
                    ShaderOptions = new List<RenderMethodDefinition.CategoryBlock.ShaderOption>
                    {
                        new RenderMethodDefinition.CategoryBlock.ShaderOption
                        {
                            Name = CacheContext.StringTable.GetOrAddString($@"ldr_and_hdr"),
                            Option = null,
                            VertexFunction = StringId.Invalid,
                            PixelFunction = StringId.Invalid,
                        },
                        new RenderMethodDefinition.CategoryBlock.ShaderOption
                        {
                            Name = CacheContext.StringTable.GetOrAddString($@"ldr_only"),
                            Option = null,
                            VertexFunction = StringId.Invalid,
                            PixelFunction = StringId.Invalid,
                        },
                    },
                    VertexFunction = StringId.Invalid,
                    PixelFunction = StringId.Invalid,
                },
                new RenderMethodDefinition.CategoryBlock
                {
                    Name = CacheContext.StringTable.GetOrAddString($@"depth_fade"),
                    ShaderOptions = new List<RenderMethodDefinition.CategoryBlock.ShaderOption>
                    {
                        new RenderMethodDefinition.CategoryBlock.ShaderOption
                        {
                            Name = CacheContext.StringTable.GetOrAddString($@"off"),
                            Option = null,
                            VertexFunction = StringId.Invalid,
                            PixelFunction = StringId.Invalid,
                        },
                        new RenderMethodDefinition.CategoryBlock.ShaderOption
                        {
                            Name = CacheContext.StringTable.GetOrAddString($@"on"),
                            Option = GenerateTag<RenderMethodOption>($@"shaders\particle_options\depth_fade_on"),
                            VertexFunction = StringId.Invalid,
                            PixelFunction = StringId.Invalid,
                        },
                        new RenderMethodDefinition.CategoryBlock.ShaderOption
                        {
                            Name = CacheContext.StringTable.GetOrAddString($@"palette_shift"),
                            Option = GenerateTag<RenderMethodOption>($@"shaders\particle_options\depth_fade_palette_shift"),
                            VertexFunction = StringId.Invalid,
                            PixelFunction = StringId.Invalid,
                        },
                    },
                    VertexFunction = StringId.Invalid,
                    PixelFunction = StringId.Invalid,
                },
                new RenderMethodDefinition.CategoryBlock
                {
                    Name = CacheContext.StringTable.GetOrAddString($@"black_point"),
                    ShaderOptions = new List<RenderMethodDefinition.CategoryBlock.ShaderOption>
                    {
                        new RenderMethodDefinition.CategoryBlock.ShaderOption
                        {
                            Name = CacheContext.StringTable.GetOrAddString($@"off"),
                            Option = null,
                            VertexFunction = StringId.Invalid,
                            PixelFunction = StringId.Invalid,
                        },
                        new RenderMethodDefinition.CategoryBlock.ShaderOption
                        {
                            Name = CacheContext.StringTable.GetOrAddString($@"on"),
                            Option = null,
                            VertexFunction = StringId.Invalid,
                            PixelFunction = StringId.Invalid,
                        },
                    },
                    VertexFunction = StringId.Invalid,
                    PixelFunction = StringId.Invalid,
                },
                new RenderMethodDefinition.CategoryBlock
                {
                    Name = CacheContext.StringTable.GetOrAddString($@"fog"),
                    ShaderOptions = new List<RenderMethodDefinition.CategoryBlock.ShaderOption>
                    {
                        new RenderMethodDefinition.CategoryBlock.ShaderOption
                        {
                            Name = CacheContext.StringTable.GetOrAddString($@"off"),
                            Option = null,
                            VertexFunction = StringId.Invalid,
                            PixelFunction = StringId.Invalid,
                        },
                        new RenderMethodDefinition.CategoryBlock.ShaderOption
                        {
                            Name = CacheContext.StringTable.GetOrAddString($@"on"),
                            Option = null,
                            VertexFunction = StringId.Invalid,
                            PixelFunction = StringId.Invalid,
                        },
                    },
                    VertexFunction = StringId.Invalid,
                    PixelFunction = StringId.Invalid,
                },
                new RenderMethodDefinition.CategoryBlock
                {
                    Name = CacheContext.StringTable.GetOrAddString($@"frame_blend"),
                    ShaderOptions = new List<RenderMethodDefinition.CategoryBlock.ShaderOption>
                    {
                        new RenderMethodDefinition.CategoryBlock.ShaderOption
                        {
                            Name = CacheContext.StringTable.GetOrAddString($@"off"),
                            Option = null,
                            VertexFunction = StringId.Invalid,
                            PixelFunction = StringId.Invalid,
                        },
                        new RenderMethodDefinition.CategoryBlock.ShaderOption
                        {
                            Name = CacheContext.StringTable.GetOrAddString($@"on"),
                            Option = GenerateTag<RenderMethodOption>($@"shaders\particle_options\frame_blend_on"),
                            VertexFunction = StringId.Invalid,
                            PixelFunction = StringId.Invalid,
                        },
                    },
                    VertexFunction = StringId.Invalid,
                    PixelFunction = StringId.Invalid,
                },
                new RenderMethodDefinition.CategoryBlock
                {
                    Name = CacheContext.StringTable.GetOrAddString($@"self_illumination"),
                    ShaderOptions = new List<RenderMethodDefinition.CategoryBlock.ShaderOption>
                    {
                        new RenderMethodDefinition.CategoryBlock.ShaderOption
                        {
                            Name = CacheContext.StringTable.GetOrAddString($@"none"),
                            Option = null,
                            VertexFunction = StringId.Invalid,
                            PixelFunction = StringId.Invalid,
                        },
                        new RenderMethodDefinition.CategoryBlock.ShaderOption
                        {
                            Name = CacheContext.StringTable.GetOrAddString($@"constant_color"),
                            Option = GenerateTag<RenderMethodOption>($@"shaders\particle_options\self_illumination_constant_color"),
                            VertexFunction = StringId.Invalid,
                            PixelFunction = StringId.Invalid,
                        },
                    },
                    VertexFunction = StringId.Invalid,
                    PixelFunction = StringId.Invalid,
                },
            };
            rmdf.EntryPoints = new List<RenderMethodDefinition.EntryPointBlock>
            {
                new RenderMethodDefinition.EntryPointBlock
                {
                    EntryPoint = EntryPoint_32.Default,
                    Passes = new List<RenderMethodDefinition.EntryPointBlock.PassBlock>
                    {
                        new RenderMethodDefinition.EntryPointBlock.PassBlock
                        {
                            Flags = RenderMethodDefinition.EntryPointBlock.PassBlock.PassFlags.None,
                            CategoryDependencies = null,
                        },
                    },
                },
            };
            rmdf.VertexTypes = new List<RenderMethodDefinition.VertexBlock>
            {
                new RenderMethodDefinition.VertexBlock
                {
                    VertexType = RenderMethodDefinition.VertexBlock.VertexTypeValue.Particle,
                    Dependencies = null,
                },
                new RenderMethodDefinition.VertexBlock
                {
                    VertexType = RenderMethodDefinition.VertexBlock.VertexTypeValue.ParticleModel,
                    Dependencies = null,
                },
            };
            rmdf.GlobalPixelShader = GenerateTag<GlobalPixelShader>($@"shaders\particle_shared_pixel_shaders");
            rmdf.GlobalVertexShader = GenerateTag<GlobalVertexShader>($@"shaders\particle_shared_vertex_shaders");
            rmdf.Flags = RenderMethodDefinition.RenderMethodDefinitionFlags.UseAutomaticMacros;
            rmdf.Version = 0;

            CacheContext.Serialize(Stream, tag, rmdf);
        }
    }
}