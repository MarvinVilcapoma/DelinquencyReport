using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ArtSolutions.MUN.Web.Areas.Workflows.ViewModel
{
    [DataContract]
    public class DiagramViewModel
    {
        [DataMember]
        public List<NodesViewModel> Nodes { get; set; } = new List<NodesViewModel>();
        [DataMember]
        public List<ConnectorViewModel> Connectors { get; set; } = new List<ConnectorViewModel>();
    }
    public class NodesViewModel : DiagramCommonViewModel
    {
        [JsonProperty(PropertyName = "offsetY")]
        public double OffsetY { get; set; }
        [JsonProperty(PropertyName = "offsetX")]
        public double OffsetX { get; set; }
        [JsonProperty(PropertyName = "shape")]
        public string Shape { get; set; }
    }
    public class LabelViewModel
    {
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }
        [JsonProperty(PropertyName = "readOnly")]
        public bool ReadOnly { get; set; } = true;
        [JsonProperty(PropertyName = "groups")]
        public string Groups { get; set; }
        [JsonProperty(PropertyName = "isSequence")]
        public bool IsSequence { get; set; } = false;
        [JsonProperty(PropertyName = "fontColor")]
        public string FontColor { get; set; } = "white";
        [JsonProperty(PropertyName = "fontSize")]
        public string FontSize { get; set; } = "11";
        [JsonProperty(PropertyName = "fontFamily")]
        public string FontFamily { get; set; } = "Segoe UI";
        [JsonProperty(PropertyName = "fillColor")]
        public string FillColor { get; set; }
    }
    public class ConnectorViewModel : DiagramCommonViewModel
    {
        [JsonProperty(PropertyName = "sourceNode")]
        public string SourceNode { get; set; }
        [JsonProperty(PropertyName = "targetNode")]
        public string TargetNode { get; set; }
        [JsonProperty(PropertyName = "segments")]
        public List<SegmentViewModel> Segments { get; set; } = new List<SegmentViewModel>();
    }
    public class DiagramCommonViewModel
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "labels")]
        public List<LabelViewModel> Labels { get; set; } = new List<LabelViewModel>();
        [JsonProperty(PropertyName = "borderColor")]
        public string BorderColor { get; set; }
        [JsonProperty(PropertyName = "fillColor")]
        public string FillColor { get; set; }
        [JsonProperty(PropertyName = "width")]
        public double Width { get; set; }
        [JsonProperty(PropertyName = "height")]
        public double Height { get; set; }
        [JsonProperty(PropertyName = "statusID")]
        public int StatusID { get; set; } = 0;
    }
    public class SegmentViewModel
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [JsonProperty(PropertyName = "direction")]
        public string Direction { get; set; }
        [JsonProperty(PropertyName = "length")]
        public double Length { get; set; }
    }
}