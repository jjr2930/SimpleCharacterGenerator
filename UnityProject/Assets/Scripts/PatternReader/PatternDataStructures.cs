using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Android;


public class PatternDataStructures
{
    public class Pattern
    {
        public class Panel
        {
            public class Edge
            {
                public List<int> endpoints { get; set; }
                public List<float> curvature { get; set; }
            }

            public List<float> translation { get; set; }
            public List<Edge> edges { get; set; }
            public List<float> rotation { get; set; }
            public List<List<double>> vertices { get; set; }
        }

        public class Stitch
        {
            public int edge { get; set; }
            public string panel { get; set; }
        }



        public Dictionary<string, Panel> panels { get; set; }
        public List<List<Stitch>> stitches { get; set; }
        public List<string> panel_order { get; set; }
    }
    public class Properties
    {
        public string curvature_coords { get; set; }
        public bool normalize_panel_translation { get; set; }
        public int units_in_meter { get; set; }
        public bool normalized_edge_loops { get; set; }
    }
    public class Parameters
    {
        public class Influence
        {
            public class Edge
            {
                public string direction { get; set; }
                public int id { get; set; }
            }
            public string panel { get; set; }
            public List<Edge> edge_list { get; set; }
        }

        public List<Influence> influence { get; set; }
        public List<float> range { get; set; }
        public string type { get; set; }
        public double value { get; set; }
    }
    
    public Pattern pattern { get; set; }
    public Properties properties { get; set; }
    public Dictionary<string, Parameters> parameters { get; set; }
    public List<string> parameters_order { get; set; }
}
