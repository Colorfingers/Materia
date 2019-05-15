﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Materia.MathHelpers;

namespace Materia.Nodes.MathNodes
{
    public class FloorNode : MathNode
    {
        NodeInput input;
        NodeOutput output;

        public FloorNode(int w, int h, GraphPixelType p = GraphPixelType.RGBA)
        {
            //we ignore w,h,p

            CanPreview = false;

            Name = "Floor";
            Id = Guid.NewGuid().ToString();
            shaderId = "S" + Id.Split('-')[0];

            input = new NodeInput(NodeType.Float | NodeType.Float2 | NodeType.Float3 | NodeType.Float4, this, "Float Input");
            output = new NodeOutput(NodeType.Float | NodeType.Float2 | NodeType.Float3 | NodeType.Float4, this);

            Inputs = new List<NodeInput>();
            Inputs.Add(input);

            input.OnInputAdded += Input_OnInputAdded;
            input.OnInputChanged += Input_OnInputChanged;

            Outputs = new List<NodeOutput>();
            Outputs.Add(output);
        }

        private void Input_OnInputChanged(NodeInput n)
        {
            TryAndProcess();
        }

        private void Input_OnInputAdded(NodeInput n)
        {
            Updated();
        }

        public override void TryAndProcess()
        {
            if (input.HasInput)
            {
                Process();
            }
        }

        public override string GetShaderPart()
        {
            if (!input.HasInput) return "";
            var s = shaderId + "0";
            var n1id = (input.Input.Node as MathNode).ShaderId;

            var index = input.Input.Node.Outputs.IndexOf(input.Input);

            n1id += index;

            Console.WriteLine("floor prev input: " + n1id);

            if (input.Input.Type == NodeType.Float4)
            {
                Console.WriteLine("floor Float4");
                output.Type = NodeType.Float4;
                return "vec4 " + s + " = floor(" + n1id + ");\r\n";
            }
            else if (input.Input.Type == NodeType.Float3)
            {
                Console.WriteLine("floor Float3");
                output.Type = NodeType.Float3;
                return "vec3 " + s + " = floor(" + n1id + ");\r\n";
            }
            else if (input.Input.Type == NodeType.Float2)
            {
                Console.WriteLine("floor Float2");
                output.Type = NodeType.Float2;
                return "vec2 " + s + " = floor(" + n1id + ");\r\n";
            }
            else if (input.Input.Type == NodeType.Float)
            {
                Console.WriteLine("floor Float");
                output.Type = NodeType.Float;
                return "float " + s + " = floor(" + n1id + ");\r\n";
            }

            Console.WriteLine("floor nothing");

            return "";
        }

        void Process()
        {
            if (input.Input.Data == null) return;

            object o = input.Input.Data;

            if (o is float || o is int)
            {
                float v = (float)o;
                output.Data = (float)Math.Floor(v);
                output.Changed();
            }
            else if (o is MVector)
            {
                MVector v = (MVector)o;
                MVector d = new MVector();
                d.X = (float)Math.Floor(v.X);
                d.Y = (float)Math.Floor(v.Y);
                d.Z = (float)Math.Floor(v.Z);
                d.W = (float)Math.Floor(v.W);

                output.Data = d;
                output.Changed();
            }
            else
            {
                output.Data = 0;
                output.Changed();
            }

            if (ParentGraph != null)
            {
                FunctionGraph g = (FunctionGraph)ParentGraph;

                if (g != null && g.OutputNode == this)
                {
                    g.Result = output.Data;
                }
            }
        }
    }
}
