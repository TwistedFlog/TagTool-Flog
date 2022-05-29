﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Shaders.ShaderMatching;
using TagTool.Tags.Definitions;
using ShaderGen2 = TagTool.Tags.Definitions.Gen2.Shader;
using HaloShaderGenerator.Shader;

namespace TagTool.Commands.Porting.Gen2
{
    partial class PortTagGen2Command : Command
    {
        public Shader ConvertShader(ShaderGen2 gen2Shader, ShaderGen2 gen2ShaderH2, Stream stream)
        {
            string shader_template = gen2ShaderH2.Template.Name;
            shader_template = shader_template.Split('\\').Last();

            var shader = new Shader()
            {
                Material = gen2Shader.MaterialName,
                ShaderProperties = new List<Shader.RenderMethodPostprocessBlock>(),
                Options = new List<Shader.RenderMethodOptionIndex>()
            };

            byte[] shaderCategories = new byte[11];

            // Declare string lists that contain the order and contents for each of these tagblocks
            var h2_bitmap_order = new List<string> {
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "\0"
            };
            var h2_pixel_constants = new List<string> {
                " ",
                " ",
                " ",
                " ",
                " ",
                " ",
                " ",
                " ",
                " ",
                " ",
                " ",
                " ",
                " ",
                " ",
                " ",
                "\0"
            };
            var h2_vertex_constants = new List<string>
            {
                " ",
                " ",
                " ",
                " ",
                " ",
                " ",
                " ",
                " ",
                " ",
                " ",
                " ",
                " ",
                " ",
                " ",
                " ",
                "\0"
            };

            // MODIFIERS FOR VERTEX CONSTANTS
            // ! = Flip the orientation of the constant values
            // 
            var modifiers = new List<char>
            {
                '!'
            };


            // Change the contents of lists depending on the h2 template used
            switch (shader_template)
            {
                case "tex_bump":
                case "tex_bump_active_camo":
                case "tex_bump_shiny":
                    {
                        shaderCategories[(int)ShaderMethods.Bump_Mapping] = (byte)Bump_Mapping.Standard;
                        shaderCategories[(int)ShaderMethods.Specular_Mask] = (byte)Specular_Mask.Specular_Mask_From_Diffuse;
                        shaderCategories[(int)ShaderMethods.Material_Model] = (byte)Material_Model.Two_Lobe_Phong;

                        h2_vertex_constants[0] = "bump_map";
                        h2_vertex_constants[1] = "base_map";
                        h2_vertex_constants[2] = "detail_map";
                        h2_vertex_constants[3] = "\0";


                        h2_pixel_constants[0] = "normal_specular_tint";
                        h2_pixel_constants[1] = "glancing_specular_tint";
                        h2_pixel_constants[2] = "\0";

                        h2_bitmap_order[0] = "bump_map";
                        h2_bitmap_order[1] = "alpha_test_map";
                        h2_bitmap_order[2] = "base_map";
                        h2_bitmap_order[3] = "detail_map";
                        h2_bitmap_order[4] = "\0";
                    }
                    break;
                case "tex_bump_env":
                    {
                        shaderCategories[(int)ShaderMethods.Bump_Mapping] = (byte)Bump_Mapping.Standard;
                        shaderCategories[(int)ShaderMethods.Specular_Mask] = (byte)Specular_Mask.Specular_Mask_From_Diffuse;
                        shaderCategories[(int)ShaderMethods.Material_Model] = (byte)Material_Model.Two_Lobe_Phong;
                        shaderCategories[(int)ShaderMethods.Environment_Mapping] = (byte)Environment_Mapping.Custom_Map;

                        if (Gen2Cache.Version == CacheVersion.Halo2Vista)
                        {
                            h2_vertex_constants[0] = "bump_map";
                            h2_vertex_constants[1] = "base_map";
                            h2_vertex_constants[2] = "detail_map";
                            h2_vertex_constants[3] = "\0";
                        }
                        else
                        {
                            h2_vertex_constants[0] = "detail_map";
                            h2_vertex_constants[1] = "base_map";
                            h2_vertex_constants[2] = "bump_map";
                            h2_vertex_constants[3] = "\0";
                        }

                        if (Gen2Cache.Version == CacheVersion.Halo2Vista)
                        {
                            h2_pixel_constants[0] = "normal_specular_tint";
                            h2_pixel_constants[1] = "glancing_specular_tint";
                            h2_pixel_constants[2] = "environment_map_specular_contribution";
                            h2_pixel_constants[4] = "env_tint_color";
                            h2_pixel_constants[5] = "\0";
                        }
                        else
                        {
                            h2_pixel_constants[1] = "normal_specular_tint";
                            h2_pixel_constants[2] = "glancing_specular_tint";
                            h2_pixel_constants[3] = "environment_map_specular_contribution";
                            h2_pixel_constants[5] = "env_tint_color";
                            h2_pixel_constants[6] = "\0";
                        }

                        h2_bitmap_order[0] = "bump_map";
                        h2_bitmap_order[1] = "alpha_test_map";
                        h2_bitmap_order[2] = "base_map";
                        h2_bitmap_order[3] = "detail_map";
                        h2_bitmap_order[4] = "environment_map";
                        h2_bitmap_order[5] = "\0";
                        break;
                    }
                case "tex_bump_env_illum_3_channel":
                    {
                        shaderCategories[(int)ShaderMethods.Bump_Mapping] = (byte)Bump_Mapping.Standard;
                        shaderCategories[(int)ShaderMethods.Specular_Mask] = (byte)Specular_Mask.Specular_Mask_From_Diffuse;
                        shaderCategories[(int)ShaderMethods.Material_Model] = (byte)Material_Model.Two_Lobe_Phong;
                        shaderCategories[(int)ShaderMethods.Environment_Mapping] = (byte)Environment_Mapping.Custom_Map;
                        shaderCategories[(int)ShaderMethods.Self_Illumination] = (byte)Self_Illumination._3_Channel_Self_Illum;

                        h2_vertex_constants[0] = "bump_map";
                        h2_vertex_constants[1] = "base_map";
                        h2_vertex_constants[2] = "detail_map";
                        h2_vertex_constants[9] = "environment_map_specular_contribution";
                        h2_vertex_constants[11] = "self_illum_map";
                        h2_vertex_constants[12] = "\0";

                        h2_pixel_constants[0] = "normal_specular_tint";
                        h2_pixel_constants[1] = "glancing_specular_tint";
                        h2_pixel_constants[2] = "emissive_color";
                        h2_pixel_constants[4] = "\0";

                        h2_bitmap_order[0] = "bump_map";
                        h2_bitmap_order[1] = "base_map";
                        h2_bitmap_order[2] = "detail_map";
                        h2_bitmap_order[3] = "environment_map";
                        h2_bitmap_order[4] = "self_illum_map";
                        h2_bitmap_order[5] = "\0";
                        break;
                    }
                case "tex_bump_env_alpha_test":
                case "tex_bump_env_alpha_test_indexed":
                    {
                        shaderCategories[(int)ShaderMethods.Bump_Mapping] = (byte)Bump_Mapping.Standard;
                        shaderCategories[(int)ShaderMethods.Alpha_Test] = (byte)Alpha_Test.Simple;
                        shaderCategories[(int)ShaderMethods.Specular_Mask] = (byte)Specular_Mask.Specular_Mask_From_Diffuse;
                        shaderCategories[(int)ShaderMethods.Material_Model] = (byte)Material_Model.Two_Lobe_Phong;
                        shaderCategories[(int)ShaderMethods.Environment_Mapping] = (byte)Environment_Mapping.Custom_Map;

                        h2_vertex_constants[0] = "bump_map";
                        h2_vertex_constants[1] = "alpha_test_map";
                        h2_vertex_constants[2] = "base_map";
                        h2_vertex_constants[3] = "detail_map";
                        h2_vertex_constants[4] = "\0";

                        h2_pixel_constants[0] = "env_tint_color";
                        h2_pixel_constants[2] = "environment_map_specular_contribution";
                        h2_pixel_constants[4] = "normal_specular_tint";
                        h2_pixel_constants[5] = "glancing_specular_tint";
                        h2_pixel_constants[6] = "\0";

                        h2_bitmap_order[0] = "bump_map";
                        h2_bitmap_order[1] = "alpha_test_map";
                        h2_bitmap_order[2] = "base_map";
                        h2_bitmap_order[3] = "detail_map";
                        h2_bitmap_order[4] = "environment_map";
                        h2_bitmap_order[5] = "\0";
                        break;
                    }
                case "tex_bump_env_two_change_color_indexed":
                    {
                        shaderCategories[(int)ShaderMethods.Albedo] = (byte)Albedo.Two_Change_Color;
                        shaderCategories[(int)ShaderMethods.Bump_Mapping] = (byte)Bump_Mapping.Standard;
                        shaderCategories[(int)ShaderMethods.Specular_Mask] = (byte)Specular_Mask.Specular_Mask_From_Diffuse;
                        shaderCategories[(int)ShaderMethods.Material_Model] = (byte)Material_Model.Two_Lobe_Phong;
                        shaderCategories[(int)ShaderMethods.Environment_Mapping] = (byte)Environment_Mapping.Custom_Map;

                        if (Gen2Cache.Version == CacheVersion.Halo2Vista)
                        {
                            h2_vertex_constants[0] = "bump_map";
                            h2_vertex_constants[1] = "base_map";
                            h2_vertex_constants[2] = "detail_map";
                            h2_vertex_constants[3] = "change_color_map";
                            h2_vertex_constants[4] = "\0";
                        }
                        else
                        {
                            h2_vertex_constants[0] = "detail_map";
                            h2_vertex_constants[1] = "base_map";
                            h2_vertex_constants[2] = "!bump_map";
                            h2_vertex_constants[3] = "change_color_map";
                            h2_vertex_constants[4] = "\0";
                        }

                        h2_pixel_constants[0] = "normal_specular_tint";
                        h2_pixel_constants[1] = "glancing_specular_tint";
                        h2_pixel_constants[2] = "";
                        h2_pixel_constants[3] = "";
                        h2_pixel_constants[4] = "";
                        h2_pixel_constants[5] = "env_tint_color";
                        h2_pixel_constants[6] = "environment_map_specular_contribution";
                        h2_pixel_constants[7] = "\0";

                        h2_bitmap_order[0] = "bump_map";
                        h2_bitmap_order[1] = "base_map";
                        h2_bitmap_order[2] = "detail_map";
                        h2_bitmap_order[3] = "change_color_map";
                        h2_bitmap_order[4] = "environment_map";
                        h2_bitmap_order[5] = "\0";
                        break;
                    }
                case "one_add_illum":
                    {
                        shaderCategories[(int)ShaderMethods.Albedo] = (byte)Albedo.Constant_Color;
                        shaderCategories[(int)ShaderMethods.Self_Illumination] = (byte)Self_Illumination.Simple;
                        shaderCategories[(int)ShaderMethods.Blend_Mode] = (byte)Blend_Mode.Additive;

                        h2_vertex_constants[2] = "self_illum_color";
                        h2_vertex_constants[3] = "self_illum_map";
                        h2_vertex_constants[4] = "self_illum_intensity";
                        h2_vertex_constants[5] = "\0";

                        h2_bitmap_order[0] = "self_illum_map";
                        h2_bitmap_order[1] = "\0";
                        break;
                    }
                default:
                    new TagToolWarning($"Shader template '{shader_template}' not yet supported!");
                    return null;
            }
            string rmt2TagName = @"shaders\shader_templates\_" + string.Join("_", shaderCategories);

            ShaderMatcherNew.Rmt2Descriptor rmt2Desc = new ShaderMatcherNew.Rmt2Descriptor("shader", shaderCategories);

            CachedTag rmt2Tag;
            RenderMethodTemplate rmt2Definition;
            if (!Cache.TagCacheGenHO.TryGetTag(rmt2TagName + ".rmt2", out rmt2Tag))
            {
                // Generate the template

                var generator = rmt2Desc.GetGenerator(true);

                GlobalPixelShader glps;
                GlobalVertexShader glvs;
                RenderMethodDefinition rmdf;
                CachedTag rmdfTag;
                if (!Cache.TagCache.TryGetTag($"shaders\\{rmt2Desc.Type}.rmdf", out rmdfTag))
                {
                    Console.WriteLine($"Generating rmdf for \"{rmt2Desc.Type}\"");
                    rmdf = TagTool.Shaders.ShaderGenerator.RenderMethodDefinitionGenerator.GenerateRenderMethodDefinition(Cache, stream, generator, rmt2Desc.Type, out glps, out glvs);
                    rmdfTag = Cache.TagCache.AllocateTag<RenderMethodDefinition>($"shaders\\{rmt2Desc.Type}");
                    Cache.Serialize(stream, rmdfTag, rmdf);
                    Cache.SaveTagNames();
                }
                else
                {
                    rmdf = Cache.Deserialize<RenderMethodDefinition>(stream, rmdfTag);
                    glps = Cache.Deserialize<GlobalPixelShader>(stream, rmdf.GlobalPixelShader);
                    glvs = Cache.Deserialize<GlobalVertexShader>(stream, rmdf.GlobalVertexShader);
                }

                rmt2Definition = TagTool.Shaders.ShaderGenerator.ShaderGenerator.GenerateRenderMethodTemplate(Cache, stream, rmdf, glps, glvs, generator, rmt2TagName, out PixelShader pixl, out VertexShader vtsh);
                rmt2Tag = Cache.TagCache.AllocateTag<RenderMethodTemplate>(rmt2TagName);

                Cache.Serialize(stream, rmt2Tag, rmt2Definition);
            }
            else
            {
                rmt2Definition = Cache.Deserialize<RenderMethodTemplate>(stream, rmt2Tag);
            }

            shader.BaseRenderMethod = Cache.TagCacheGenHO.GetTag<RenderMethodDefinition>(rmt2Desc.GetRmdfName());

            // Add postprocessblock
            RenderMethod.RenderMethodPostprocessBlock newPostprocessBlock = new RenderMethod.RenderMethodPostprocessBlock
            {
                Template = rmt2Tag,
                TextureConstants = new List<RenderMethod.RenderMethodPostprocessBlock.TextureConstant>(),
                RealConstants = new List<RenderMethod.RenderMethodPostprocessBlock.RealConstant>(),
                RoutingInfo = new List<RenderMethod.RenderMethodPostprocessBlock.RenderMethodRoutingInfoBlock>(),
                Functions = new List<RenderMethod.RenderMethodAnimatedParameterBlock>()
            };
            shader.ShaderProperties.Add(newPostprocessBlock);

            // Populate options block
            shader.Options = new List<RenderMethod.RenderMethodOptionIndex>();
            foreach (var category in rmt2Desc.Options)
            {
                RenderMethod.RenderMethodOptionIndex option = new RenderMethod.RenderMethodOptionIndex
                {
                    OptionIndex = category
                };
                shader.Options.Add(option);
            }

            // Add all the texture maps
            foreach (var shadermap in rmt2Definition.TextureParameterNames)
            {
                var h2_postprocess = gen2Shader.PostprocessDefinition[0];
                var h2_texture_reference = h2_postprocess.Bitmaps;
                string current_type = Cache.StringTable[((int)shadermap.Name.Value)];   // Gets the current type of bitmap in the template
                bool found = false;

                // If the string in the bitmap order list matches the current_type in the rmt2,
                // Then set current_type to the bitmap path
                for (var i = 0; h2_bitmap_order[i] != "\0"; i++)
                {
                    if (h2_bitmap_order[i] == current_type)
                    {
                        current_type = h2_texture_reference[i].Bitmap.ToString();
                        found = true;
                        break;
                    }
                }

                // If bitmap type is not found in list just give it a default bitmap
                if (found == false) current_type = "shaders\\default_bitmaps\\bitmaps\\alpha_grey50.bitmap";

                RenderMethod.RenderMethodPostprocessBlock.TextureConstant newTextureConstant = new RenderMethod.RenderMethodPostprocessBlock.TextureConstant
                {
                    Bitmap = Cache.TagCacheGenHO.GetTag(current_type)
                };
                newPostprocessBlock.TextureConstants.Add(newTextureConstant);
            }

            // Declare Real Parameters and "try" to convert the similar counterparts in h2 to h3 equivalents
            foreach (var floatconstant in rmt2Definition.RealParameterNames)
            {
                bool found = false;
                var h2resource = gen2Shader.PostprocessDefinition;
                var h2pixel_constant = h2resource[0].PixelConstants;
                var h2vertex_constant = h2resource[0].VertexConstants;

                string current_type = Cache.StringTable[((int)floatconstant.Name.Value)];   // Gets the current type of bitmap in the template

                // Set albedo_color to 1 as default for all shaders
                // Is changed to h2 equivalent when a shader that actually changes this color is used
                if (current_type == "albedo_color")
                {
                    found = true;
                    RenderMethod.RenderMethodPostprocessBlock.RealConstant newfloatconstant = new RenderMethod.RenderMethodPostprocessBlock.RealConstant
                    {
                        Arg0 = 1,
                        Arg1 = 1,
                        Arg2 = 1,
                        Arg3 = 1
                    };
                    newPostprocessBlock.RealConstants.Add(newfloatconstant);
                }

                // Adjust specular values
                if (current_type == "normal_specular_power" || current_type == "glancing_specular_power" || current_type == "fresnel_curve_steepness")
                {
                    found = true;
                    RenderMethod.RenderMethodPostprocessBlock.RealConstant newfloatconstant = new RenderMethod.RenderMethodPostprocessBlock.RealConstant
                    {
                        Arg0 = (float)gen2Shader.LightmapSpecularBrightness * (float)10,
                        Arg1 = (float)gen2Shader.LightmapSpecularBrightness * (float)10,
                        Arg2 = (float)gen2Shader.LightmapSpecularBrightness * (float)10,
                        Arg3 = (float)gen2Shader.LightmapSpecularBrightness * (float)10
                    };
                    newPostprocessBlock.RealConstants.Add(newfloatconstant);

                }

                // Set diffuse_coefficient to 1 default for all shaders
                else if (current_type == "diffuse_coefficient")
                {
                    found = true;
                    RenderMethod.RenderMethodPostprocessBlock.RealConstant newfloatconstant = new RenderMethod.RenderMethodPostprocessBlock.RealConstant
                    {
                        Arg0 = 1,
                        Arg1 = 1,
                        Arg2 = 1,
                        Arg3 = 1
                    };
                    newPostprocessBlock.RealConstants.Add(newfloatconstant);
                }

                // Set specular_coefficient to specular value for h2 divided by 10
                else if (current_type == "specular_coefficient")
                {
                    found = true;
                    RenderMethod.RenderMethodPostprocessBlock.RealConstant newfloatconstant = new RenderMethod.RenderMethodPostprocessBlock.RealConstant
                    {
                        Arg0 = (float)gen2Shader.LightmapSpecularBrightness / (float)10,
                        Arg1 = (float)gen2Shader.LightmapSpecularBrightness / (float)10,
                        Arg2 = (float)gen2Shader.LightmapSpecularBrightness / (float)10,
                        Arg3 = (float)gen2Shader.LightmapSpecularBrightness / (float)10
                    };
                    newPostprocessBlock.RealConstants.Add(newfloatconstant);
                }

                // Set area_specular_contribution and analytical_specular_contribution to default values
                else if (current_type == "area_specular_contribution" || current_type == "analytical_specular_contribution")
                {
                    found = true;
                    RenderMethod.RenderMethodPostprocessBlock.RealConstant newfloatconstant = new RenderMethod.RenderMethodPostprocessBlock.RealConstant
                    {
                        Arg0 = (float)0.5,
                        Arg1 = (float)0.5,
                        Arg2 = (float)0.5,
                        Arg3 = (float)0.5
                    };
                    newPostprocessBlock.RealConstants.Add(newfloatconstant);
                }

                // If no float constant specific edits are needed 
                else
                {
                    // Convert Pixel Constants to Float Constants if matches are found
                    for (var i = 0; h2_pixel_constants[i] != "\0"; i++)
                    {
                        if (h2_pixel_constants[i] == current_type)
                        {
                            found = true;
                            float r = -1, g = -1, b = -1, a = -1;

                            // Convert values in h2 pixel constant block to the layout and values it expects in h3
                            // (For numeric values in shader)
                            // Set (rgba) to value of (a) in h3 shader if only (a) has a value and (rgb) dosent in h2
                            if
                            (h2pixel_constant[i].Color.Red == 0 && h2pixel_constant[i].Color.Green == 0 && h2pixel_constant[i].Color.Blue == 0 && h2pixel_constant[i].Color.Alpha > 0)
                            {
                                r = (float)h2pixel_constant[i].Color.Alpha / (float)255;
                                g = (float)h2pixel_constant[i].Color.Alpha / (float)255;
                                b = (float)h2pixel_constant[i].Color.Alpha / (float)255;
                                a = (float)h2pixel_constant[i].Color.Alpha / (float)255;
                            }

                            // (For colours in shader)
                            // Set (a) to value of 1 if any (rgb) values are above 0 and (a) is 0
                            else if
                            (h2pixel_constant[i].Color.Red > 0 || h2pixel_constant[i].Color.Green > 0 || h2pixel_constant[i].Color.Blue > 0 && h2pixel_constant[i].Color.Alpha == 0)
                            {
                                r = (float)h2pixel_constant[i].Color.Red / (float)255;
                                g = (float)h2pixel_constant[i].Color.Green / (float)255;
                                b = (float)h2pixel_constant[i].Color.Blue / (float)255;
                                a = 1;
                            }

                            RenderMethod.RenderMethodPostprocessBlock.RealConstant newfloatconstant = new RenderMethod.RenderMethodPostprocessBlock.RealConstant
                            {
                                Arg0 = r,
                                Arg1 = g,
                                Arg2 = b,
                                Arg3 = a
                            };
                            newPostprocessBlock.RealConstants.Add(newfloatconstant);
                        }
                    }

                    // If any Pixel Constants with matching types aren't found
                    // try to find and convert Vertex Constants to Float Constants
                    if (found == false)
                    {
                        for (var i = 0; h2_vertex_constants[i] != "\0"; i++)
                        {
                            char[] symbol = h2_vertex_constants[i].Substring(0, 1).ToCharArray();
                            foreach (var modifier in modifiers)
                            {
                                if (symbol[0] == modifier) h2_vertex_constants[i].Remove(0);
                            }

                            if (h2_vertex_constants[i] == current_type)
                            {
                                RenderMethod.RenderMethodPostprocessBlock.RealConstant newfloatconstant = new RenderMethod.RenderMethodPostprocessBlock.RealConstant();
                                found = true;
                                switch (symbol[0])
                                {
                                    case '!':
                                        {
                                            newfloatconstant.Arg0 = h2vertex_constant[i].W;
                                            newfloatconstant.Arg1 = h2vertex_constant[i].Vector3.K;
                                            newfloatconstant.Arg2 = h2vertex_constant[i].Vector3.J;
                                            newfloatconstant.Arg3 = h2vertex_constant[i].Vector3.I;
                                        };
                                        break;
                                    default:
                                        newfloatconstant.Arg0 = h2vertex_constant[i].Vector3.I;
                                        newfloatconstant.Arg1 = h2vertex_constant[i].Vector3.J;
                                        newfloatconstant.Arg2 = h2vertex_constant[i].Vector3.K;
                                        newfloatconstant.Arg3 = h2vertex_constant[i].W;
                                        break;
                                }
                                newPostprocessBlock.RealConstants.Add(newfloatconstant);
                            }
                        }
                    }

                    // Set all to 0 if no matches are found
                    if (found == false)
                    {
                        RenderMethod.RenderMethodPostprocessBlock.RealConstant newfloatconstant = new RenderMethod.RenderMethodPostprocessBlock.RealConstant
                        {
                            Arg0 = 0,
                            Arg1 = 0,
                            Arg2 = 0,
                            Arg3 = 0
                        };
                        newPostprocessBlock.RealConstants.Add(newfloatconstant);
                    }
                }
            }
            return shader;
        }
    }
}