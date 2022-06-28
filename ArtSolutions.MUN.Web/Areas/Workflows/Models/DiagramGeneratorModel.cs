using ArtSolutions.MUN.Web.Areas.Workflows.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArtSolutions.MUN.Web.Areas.Workflows.Models
{
    public class DiagramGeneratorModel
    {
        public DiagramViewModel Get(WorkflowViewModel model)
        {
            DiagramViewModel viewModel = new DiagramViewModel();
            string startNode = "start_" + model.ID.ToString();
            viewModel.Nodes.Add(new NodesViewModel
            {
                Name = startNode,
                OffsetY = 60,
                OffsetX = 500,
                Width = 150,
                Height = 60,
                Labels = new List<LabelViewModel> {
                    new LabelViewModel{ Text = "Start" }
                },
                Shape = "terminator",
                FillColor = "#16466A",
                BorderColor = "#16466A",
            });
            var stausSequence = new WorkflowStatusSequenceModel().GetByPaging(model.ID, model.Version);
            double offsetY = 180;
            int j = 120;
            if (stausSequence != null && stausSequence.Any())
            {
                int i = 0;
                foreach (var sequence in stausSequence)
                {
                    if (i > 0)
                        offsetY = offsetY + j;

                    string statusNode = startNode + "_s_" + sequence.WorkflowStatusID;

                    viewModel.Nodes.Add(new NodesViewModel
                    {
                        Name = statusNode,
                        OffsetY = offsetY,
                        OffsetX = 500,
                        Width = 150,
                        Height = 60,
                        Labels = new List<LabelViewModel> {
                            new LabelViewModel{
                                Text = sequence.FromStatus,
                                IsSequence = true,
                                Groups = string.IsNullOrWhiteSpace(sequence.Sequence)?string.Empty: string.Join("&",(sequence.Sequence.Split("^".ToCharArray())[5]).Split("&".ToCharArray()))
                            }
                        },
                        Shape = "process",
                        FillColor = "#256FA9",
                        BorderColor = "#256FA9",
                        StatusID = sequence.WorkflowStatusID,
                    });

                    if (i == 0)
                    {
                        viewModel.Connectors.Add(new ConnectorViewModel
                        {
                            Name = "c_" + statusNode,
                            SourceNode = startNode,
                            TargetNode = statusNode
                        });
                    }
                    i++;
                }
            }

            if (viewModel.Nodes != null && viewModel.Nodes.Any())
            {
                int k = 0;
                foreach (var item in viewModel.Nodes)
                {
                    if (item.StatusID > 0)
                    {
                        var sequences = stausSequence.FirstOrDefault(a => a.WorkflowStatusID == item.StatusID);
                        if (sequences != null)
                        {
                            if (!string.IsNullOrWhiteSpace(sequences.Sequence))
                            {
                                var sequence = sequences.Sequence.Split(",".ToCharArray());
                                int m = 1;
                                foreach (var itemSeq in sequence)
                                {
                                    double length = 0;
                                    bool isLeft = true;

                                    if (m % 2 == 0)
                                        isLeft = false;

                                    var targetNode = viewModel.Nodes.FirstOrDefault(a => a.StatusID == Convert.ToInt32(itemSeq.Split("^".ToCharArray())[2]));

                                    var conectorModel = new ConnectorViewModel
                                    {
                                        Name = "c_" + item.Name + "_" + k.ToString(),
                                        SourceNode = item.Name,
                                        TargetNode = targetNode.Name,
                                        Labels = new List<LabelViewModel> {
                                            new LabelViewModel{
                                                Text = itemSeq.Split("^".ToCharArray())[1],
                                                FontColor = "black",
                                                FontSize = "11",
                                                FillColor = "white"
                                            }
                                        }
                                    };

                                    if (targetNode.OffsetY == (item.OffsetY + j))
                                    {
                                        conectorModel.Segments = new List<SegmentViewModel> {
                                            new SegmentViewModel {
                                                Direction = "bottom",
                                                Type = "orthogonal"
                                            }
                                        };
                                    }
                                    else if (isLeft)
                                    {
                                        length = ((targetNode.OffsetY - viewModel.Nodes.FirstOrDefault().OffsetY) / j) * 25;
                                        conectorModel.Segments = new List<SegmentViewModel> {
                                                new SegmentViewModel {
                                                    Direction = "left",
                                                    Type = "orthogonal",
                                                    Length = length,
                                                }
                                            };
                                    }
                                    else
                                    {
                                        length = ((targetNode.OffsetY - viewModel.Nodes.FirstOrDefault().OffsetY) / j) * 25;
                                        conectorModel.Segments = new List<SegmentViewModel> {
                                                new SegmentViewModel {
                                                    Direction = "right",
                                                    Type = "orthogonal",
                                                    Length = length
                                                }
                                            };
                                    }
                                    viewModel.Connectors.Add(conectorModel);
                                    k++;
                                    m++;
                                }
                            }
                        }
                    }

                }
            }

            string endNode = "end_" + model.ID.ToString();
            viewModel.Nodes.Add(new NodesViewModel
            {
                Name = endNode,
                OffsetY = (offsetY + j),
                OffsetX = 500,
                Width = 150,
                Height = 60,
                Labels = new List<LabelViewModel> {
                    new LabelViewModel{ Text = "End" }
},
                Shape = "terminator",
                FillColor = "#16466A",
                BorderColor = "#16466A",
            });
            viewModel.Connectors.Add(new ConnectorViewModel
            {
                Name = "c_" + endNode,
                SourceNode = viewModel.Nodes.ElementAt(viewModel.Nodes.Count - 2).Name,
                TargetNode = endNode
            });
            return viewModel;
        }
    }
}