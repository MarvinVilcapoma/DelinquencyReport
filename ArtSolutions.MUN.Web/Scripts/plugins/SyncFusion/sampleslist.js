window.GroupingList = ["GRIDS", "DATA VISUALIZATION", "LAYOUT", "EDITORS", "NAVIGATION", "NOTIFICATION", "FILE FORMATS","REPORTING", "BUSINESS INTELLIGENCE", "DATA SCIENCE", "INTEGRATION"];
//Controls List
window.SampleControls = [
    { "name": "Chart" }, { "name": "Grid" }, { "name": "Gantt" }, { "name": "TreeGrid" }, { "name": "Schedule" }, { "name": "Diagram" }, { "name": "Maps" }, { "name": "TreeMap" }, { "name": "LinearGauge" }, { "name": "CircularGauge" }, { "name": "DigitalGauge" }, { "name": "ReportViewer" }, { "name": "BulletGraph" }, { "name": "RangeNavigator" }, { "name": "OlapChart" }, { "name": "OlapClient" }, { "name": "PivotGrid" }, { "name": "OlapGauge" }, { "name": "RTE" },{ "name": "FileExplorer" },{ "name": "TreeView" }, { "name": "ColorPicker" }, { "name": "DatePicker" }, { "name": "TimePicker" }, { "name": "DateTimePicker" }, { "name": "Editor" }, { "name": "Autocomplete" }, { "name": "Rotator" }, { "name": "Tab" }, { "name": "Menu" }, { "name": "Pdf" }, { "name": "Barcode" }, { "name": "Accordion" }, { "name": "Button" }, { "name": "Dialog" }, { "name": "ProgressBar" }, { "name": "Rating" }, { "name": "Slider" }, { "name": "TagCloud" }, { "name": "Toolbar" }, { "name": "WaitingPopup" }, { "name": "Upload" }, { "name": "Dropdownlist" }, { "name": "ScrollBar" }, { "name": "Splitter" }, { "name": "DocIO" },{ "name": "Presentation" }, { "name": "XlsIO" }, { "name": "Captcha" }, { "name": "ListBox" }, { "name": "ListView" }, { "name": "NavigationDrawer" },{ "name": "Ribbon" }, { "name": "RadialMenu" }, { "name": "TileView" },{"name":"RadialSlider"}, { "name": "Analytics" }, {"name":"AngularSupport"}, {"name":"KOSupport"}
];
//Samples List
window.SamplesList = [
    {
         "name": "Chart", "id": "Chart", "childcount": "2","Group": "DATA VISUALIZATION", "controller": "Chart","type": "update", "action": "Default", "samples": [
           { "id": "1", "name": "Line ", "controller": "Chart", "action": "Default", "childcount": "0" },
           { "id": "2", "name": "Step Line ", "controller": "Chart", "action": "StepLine", "childcount": "0" },
           { "id": "3", "name": "Area ", "controller": "Chart", "action": "Area", "childcount": "0" },
           { "id": "4", "name": "Step Area ", "controller": "Chart", "action": "StepArea", "childcount": "0" },
           { "id": "5", "name": "Spline Area ", "controller": "Chart", "action": "SplineArea", "childcount": "0" },
           { "id": "6", "name": "Stacked Area ", "controller": "Chart", "action": "StackingArea", "childcount": "0" },
            { "id": "7", "name": "100% Stacked Area ", "controller": "Chart", "action": "StackingArea100", "childcount": "0" },
           { "id": "8", "name": "Range Area ", "controller": "Chart", "action": "RangeArea", "childcount": "0"},
           { "id": "9", "name": "Column ", "controller": "Chart", "action": "Column", "childcount": "0"},
           { "id": "10", "name": "Range Column ", "controller": "Chart", "action": "RangeColumn", "childcount": "0"},
           { "id": "11", "name": "Stacked Column ", "controller": "Chart", "action": "StackingColumn", "childcount": "0" },
            { "id": "12", "name": "100% Stacked Column ", "controller": "Chart", "action": "StackingColumn100", "childcount": "0" },
           { "id": "13", "name": "Bar ", "controller": "Chart", "action": "Bar", "childcount": "0" },
           { "id": "14", "name": "Stacked Bar ", "controller": "Chart", "action": "StackingBar", "childcount": "0" },
            { "id": "15", "name": "100% Stacked Bar ", "controller": "Chart", "action": "StackingBar100", "childcount": "0"},
           { "id": "16", "name": "Spline ", "controller": "Chart", "action": "Spline", "childcount": "0"},
           { "id": "17", "name": "Pie ", "controller": "Chart", "action": "Pie", "childcount": "0"},
           { "id": "18", "name": "Doughnut ", "controller": "Chart", "action": "Doughnut", "childcount": "0" },
           { "id": "19", "name": "Semi Pie and Doughnut", "controller": "Chart", "action": "SemiPie", "childcount": "0" },
           { "id": "20", "name": "Pyramid ", "controller": "Chart", "action": "Pyramid", "childcount": "0" },
           { "id": "21", "name": "Funnel ", "controller": "Chart", "action": "Funnel", "childcount": "0" },
           { "id": "22", "name": "Bubble ", "controller": "Chart", "action": "Bubble", "childcount": "0" },
           { "id": "23", "name": "Scatter ", "controller": "Chart", "action": "Scatter", "childcount": "0" },
           { "id": "24", "name": "Candle ", "controller": "Chart", "action": "Candle", "childcount": "0"},
           { "id": "25", "name": "Hilo ", "controller": "Chart", "action": "Hilo", "childcount": "0" },
           { "id": "26", "name": "Hilo Open Close ", "controller": "Chart", "action": "HiloOpenClose", "childcount": "0" },
		   { "id": "27", "name": "Polar ", "controller": "Chart", "action": "Polar", "childcount": "0" },
		   { "id": "28", "name": "Radar ", "controller": "Chart", "action": "Radar", "childcount": "0" },
		   { "id": "29", "name": "Wind Rose ", "controller": "Chart", "action": "WindRose", "childcount": "0" },
           { "id": "30", "name": "Combination ", "controller": "Chart", "action": "Combination",  "childcount": "0" },
		   { "id": "31", "name": "Vertical Chart ", "controller": "Chart", "action": "VerticalChart", "childcount": "0", "type": "new" },
		   { "id": "32", "name": "Trendlines ", "controller": "Chart", "action": "Trendlines", "childcount": "0"},
           { "id": "33", "name": "Live ", "controller": "Chart", "action": "Live", "childcount": "0"},
		    { "id": "34", "name": "Performance", "controller": "Chart", "action": "Performance", "childcount": "0"},
             { "id": "35", "name": "Export ", "controller": "Chart", "action": "Exports",  "childcount": "0" },
             {
                 "id": "36", "name": "Technical Indicators", "controller": "Chart", "action": "rsi", "childcount": "1", samples: [
                     { "id": "1", "name": "RSI", "controller": "Chart", "action": "rsi",  "childcount": "0" },
					 { "id": "2", "name": "Momentum", "controller": "Chart", "action": "momentum",  "childcount": "0" },
                     { "id": "3", "name": "BollingerBand", "controller": "Chart", "action": "Bollingerband",  "childcount": "0" },
                     { "id": "4", "name": "AccumulationDistribution", "controller": "Chart", "action": "Accumulationdistribution",  "childcount": "0" },
                     { "id": "5", "name": "SMA", "controller": "Chart", "action": "Sma",  "childcount": "0" },
					 { "id": "6", "name": "EMA", "controller": "Chart", "action": "Ema",  "childcount": "0" },
					 { "id": "7", "name": "Stochastic", "controller": "Chart", "action": "Stochastic",  "childcount": "0" },
                     { "id": "8", "name": "ATR", "controller": "Chart", "action": "ATR", "childcount": "0" },
					 { "id": "9", "name": "MACD", "controller": "Chart", "action": "MACD", "childcount": "0" },
					 { "id": "10", "name": "TMA", "controller": "Chart", "action": "Tma", "childcount": "0" }
                 ]
             },
			 {
                  "id": "37", "name": "3D", "controller": "Chart", "action": "column3d", "childcount": "1", samples: [
                  { "id": "1", "name": "Column", "controller": "Chart", "action": "Column3d", "childcount": "0" },
                  { "id": "2", "name": "Bar", "controller": "Chart", "action": "Bar3d", "childcount": "0" },
                   { "id": "3", "name": " Stacked Column", "controller": "Chart", "action": "StackingColumn3d", "childcount": "0" },
                   { "id": "4", "name": " 100% Stacked Column", "controller": "Chart", "action": "StackingColumn1003d", "childcount": "0" },
                  { "id": "5", "name": "Stacked Bar", "controller": "Chart", "action": "StackingBar3d", "childcount": "0" },
                   { "id": "6", "name": " 100% Stacked Bar", "controller": "Chart", "action": "StackingBar1003d", "childcount": "0"},
                  { "id": "7", "name": "Pie", "controller": "Chart", "action": "Pie3d", "childcount": "0" },
                  { "id": "8", "name": "Doughnut", "controller": "Chart", "action": "Doughnut3d", "childcount": "0" }
                  ]
              },
           {
               "id": "38", "name": "Chart Axes", "controller": "Chart", "action": "MultipleAxes", "childcount": "1", samples: [
                     { "id": "1", "name": "Multiple Axes", "controller": "Chart", "action": "MultipleAxes", "childcount": "0" },
                     { "id": "2", "name": "Trim Title", "controller": "Chart", "action": "Trim", "childcount": "0"},
                     { "id": "3", "name": "Strip Lines", "controller": "Chart", "action": "StripLine","childcount": "0" },
                     { "id": "4", "name": "DateTime Axis", "controller": "Chart", "action": "DateTimeAxis", "childcount": "0" },
                     { "id": "5", "name": "Log Axis", "controller": "Chart", "action": "LogAxis", "childcount": "0" },
                     { "id": "6", "name": "Smart Axis Labels", "controller": "Chart", "action": "SmartAxisLabels", "childcount": "0" },
                     { "id": "7", "name": "Flexible Axis Layout", "controller": "Chart", "action": "FlexibleAxisLayout", "childcount": "0" },
                     { "id": "8", "name": "Inversed Axis", "controller": "Chart", "action": "IsInversed", "childcount": "0"},
                     { "id": "9", "name": "Alternate Grid Band", "controller": "Chart", "action": "AlternateGridBand", "childcount": "0" }
               ]
           },
           {
               "id": "39", "name": "Chart Customization", "controller": "Chart", "action": "Symbols",type:"update", "childcount": "1", samples: [
                   { "id": "1", "name": "Symbols", "controller": "Chart", "action": "Symbols", "childcount": "0" },
                   { "id": "2", "name": "Empty Points", "controller": "Chart", "action": "EmptyPoints", "childcount": "0"},
                   { "id": "3", "name": "Tooltip Template", "controller": "Chart", "action": "TooltipTemplate", "childcount": "0"},
                   { "id": "4", "name": "Label Template", "controller": "Chart", "action": "LabelTemplate", "childcount": "0" },
                   { "id": "5", "name": "Smart Labels", "controller": "Chart", "action": "Smartlabels", type:"new", "childcount": "0" },
                   { "id": "6", "name": "Annotations", "controller": "Chart", "action": "Annotations", "childcount": "0" },
                   { "id": "7", "name": "Legend Position", "controller": "Chart", "action": "LegendPosition", "childcount": "0" },
                   { "id": "8", "name": "Localization", "controller": "Chart", "action": "Localization", "childcount": "0"},
                   { "id": "9", "name": "SubTitle", "controller":"Chart","action":"SubTitle", "childcount":"0"},
                   { "id": "10", "name": "Selection", "controller": "Chart", "action": "Selection", "type": "update", "childcount": "0" }
               ]
           },
             {
                 "id": "40", "name": "Data Binding", "controller": "Chart", "action": "LocalData", "childcount": "1", samples: [
                    { "id": "1", "name": "Local Data", "controller": "Chart", "action": "LocalData", "childcount": "0" },
                    { "id": "2", "name": "Remote Data", "controller": "Chart", "action": "RemoteData", "childcount": "0" }

                 ]
             },
             {
                 "id": "41", "name": "User Interaction", "controller": "Chart", "action": "ZoomingandPanning", "childcount": "1", samples: [
                     { "id": "1", "name": "Zooming and Panning", "controller": "Chart", "action": "ZoomingandPanning", "childcount": "0" },
                     { "id": "2", "name": "Crosshair", "controller": "Chart", "action": "CrossHair", "childcount": "0"},
                     { "id": "3", "name": "TrackBall", "controller": "Chart", "action": "TrackBall", "childcount": "0" },
                     { "id": "4", "name": "Events", "controller": "Chart", "action": "Events", "childcount": "0" },
                     { "id": "5", "name": "Drill Down", "controller": "Chart", "action": "DrillDown", "childcount": "0" }
                 ]
             }
              
         ]
     },
         {
             "name": "Grid", "id": "Grid", "childcount": "1","Group": "GRIDS", "controller": "Grid", "action": "Default","type": "update", "samples": [
                    {
                        "id": "1", "name": "Default", "controller": "Grid", "action": "Default", "childcount": "0"
                    },
                    {
                        "id": "2", "name": "Data Binding", "controller": "Grid", "action": "LocalBinding", "childcount": "1", "samples": [
                                 { "id": "1", "name": "List Binding", "controller": "Grid", "action": "LocalBinding", "childcount": "0" },
                                 { "id": "2", "name": "Remote Data", "controller": "Grid", "action": "UrlBinding", "childcount": "0" },
                                 { "id": "3", "name": "Table", "controller": "Grid", "action": "TableBinding", "childcount": "0" },
                                 { "id": "4", "name": "Load at once", "controller": "Grid", "action": "LoadAtOnce", "childcount": "0" },
                                 { "id": "19", "name": "Data Caching", "controller": "Grid", "action": "DataCaching", "childcount": "0", "type": "update" },
					            { "id": "20", "name": "Real Time Binding", "controller": "Grid", "action": "SignalR", "childcount": "0" },
                                 { "id": "21", "name": "Live Update", "controller": "Grid", "action": "LiveUpdate", "childcount": "0" },
                                { "id": "24", "name": "Large Data", "controller": "Grid", "action": "LargeData", "childcount": "0"}
                        ]
                    },
                    {
                        "id": "3", "name": "Columns", "controller": "Grid", "action": "ColumnFormatting", "childcount": "1", "samples": [
                                 { "id": "1", "name": "Column Formatting", "controller": "Grid", "action": "ColumnFormatting", "childcount": "0" },
                                 { "id": "2", "name": "Cell Alignment", "controller": "Grid", "action": "CellAlignment", "childcount": "0" },
                                 { "id": "3", "name": "Reorder", "controller": "Grid", "action": "Reorder", "childcount": "0" },
                                 { "id": "4", "name": "Resize", "controller": "Grid", "action": "Resizing", "childcount": "0" },
								 { "id": "11", "name": "Resize to Fit", "controller": "Grid", "action": "ResizetoFit", "childcount": "0" },
                                 { "id": "5", "name": "Custom Command", "controller": "Grid", "action": "CustomCommand", "childcount": "0" },
                                 { "id": "6", "name": "Column Template", "controller": "Grid", "action": "ColumnTemplate", "childcount": "0" },
                                 { "id": "7", "name": "Header Template", "controller": "Grid", "action": "HeaderTemplate", "childcount": "0" },
                                 { "id": "8", "name": "Show or Hide Column", "controller": "Grid", "action": "ShowHideColumn", "childcount": "0" },
                                 { "id": "9", "name": "Foreign Key Column", "controller": "Grid", "action": "ForeignKeyColumn", "childcount": "0" },
                                 { "id": "10", "name": "Frozen Rows and Columns", "controller": "Grid", "action": "FrozenRowsandColumns", "childcount": "0" },
								 { "id": "13", "name": "AutoWrap Column Cells", "controller": "Grid", "action": "AutoWrap", "childcount": "0" },
								 { "id": "14", "name": "Cell Merging", "controller": "Grid", "action": "CellMerging", "childcount": "0"  },
                                { "id": "16", "name": "Column Chooser", "controller": "Grid", "action": "ColumnChooser", "childcount": "0" },
								{ "id": "17", "name": "Stacked Header", "controller": "Grid", "action": "StackedHeader", "childcount": "0" }
                        ]
                    },
                    {
                        "id": "4", "name": "Rows", "controller": "Grid", "action": "RowTemplate", "childcount": "1", "samples": [
                                 { "id": "1", "name": "Row Template", "controller": "Grid", "action": "RowTemplate", "childcount": "0" },
                                 { "id": "2", "name": "Detail Template", "controller": "Grid", "action": "DetailTemplate", "childcount": "0" },
                                 { "id": "3", "name": "Row Hover", "controller": "Grid", "action": "RowHover", "childcount": "0" }
                        ]
                    },
                    {
                        "id": "5", "name": "Editing", "controller": "Grid", "action": "NormalEditing",  "childcount": "1", "samples": [
                                 { "id": "1", "name": "Inline Editing", "controller": "Grid", "action": "NormalEditing", "childcount": "0" },
                                 { "id": "2", "name": "Dialog Editing", "controller": "Grid", "action": "DialogEditing", "childcount": "0" },
                                 { "id": "3", "name": "Inline form Editing", "controller": "Grid", "action": "InlineFormEditing", "childcount": "0" },
                                 { "id": "4", "name": "External form Editing", "controller": "Grid", "action": "ExternalFormEditing", "childcount": "0" },
                                 { "id": "5", "name": "Cell Edit Type", "controller": "Grid", "action": "CellEditType", "childcount": "0" },
                                 { "id": "6", "name": "Batch Editing", "controller": "Grid", "action": "BatchEditing", "childcount": "0" },
                                 { "id": "7", "name": "Lock Row", "controller": "Grid", "action": "LockRow", "childcount": "0" },
                                 { "id": "8", "name": "Command Column", "controller": "Grid", "action": "CommandColumn", "childcount": "0" },
                                 { "id": "9", "name": "Custom Validation", "controller": "Grid", "action": "CustomValidation", "childcount": "0" },
                                 { "id": "10", "name": "Edit Template", "controller": "Grid", "action": "EditTemplate", "childcount": "0" }

                        ]
                    },
                    {
                        "id": "6", "name": "Sorting", "controller": "Grid", "action": "MultiSorting",  "childcount": "1", "samples": [
                                 { "id": "1", "name": "Multi Sorting", "controller": "Grid", "action": "MultiSorting", "childcount": "0" },
                                 { "id": "2", "name": "Sorting API", "controller": "Grid", "action": "SortingAPI",   "childcount": "0" }
								 
                        ]
                    },
                    {
                        "id": "7", "name": "Filtering", "controller": "Grid", "action": "Filtering", "childcount": "1", "samples": [
                                 { "id": "1", "name": "Default Functionalities", "controller": "Grid", "action": "Filtering", "childcount": "0" },
                                 { "id": "2", "name": "Filtering Menu", "controller": "Grid", "action": "FilteringMenu", "childcount": "0"  },
                                 { "id": "3", "name": "Searching", "controller": "Grid", "action": "Searching", "childcount": "0" }
                        ] 
                    },
                    {
                        "id": "8", "name": "Grouping", "controller": "Grid", "action": "Grouping", "childcount": "1", "samples": [
                                 { "id": "1", "name": "Default Functionalities", "controller": "Grid", "action": "Grouping", "childcount": "0" },
                                 { "id": "2", "name": "Group Button", "controller": "Grid", "action": "GroupButton", "childcount": "0" },
                                 { "id": "3", "name": "Hide Grouped Columns", "controller": "Grid", "action": "HideGroupedColumn", "childcount": "0" },
                                 { "id": "4", "name": "Grouping API", "controller": "Grid", "action": "GroupingAPI", "childcount": "0" }
                        ]
                    },
                    {
                        "id": "9", "name": "Paging", "controller": "Grid",  "action": "Paging", "childcount": "1", "samples": [
                                 { "id": "1", "name": "Default Functionalities", "controller": "Grid", "action": "Paging", "childcount": "0" },
                                 { "id": "2", "name": "Paging API", "controller": "Grid", "action": "PagingAPI", "childcount": "0" },
                                 { "id": "3", "name": "Virtual Paging", "controller": "Grid",  "action": "VirtualPaging", "childcount": "0"  },
                                  { "id": "4", "name": "Pager Templates", "controller": "Grid", "action": "PagerImprovements", "childcount": "0"  }

                        ] 
                    },
                     {
                         "id": "10", "name": "Selection", "controller": "Grid", "action": "Selection", "childcount": "1", "samples": [
                                   { "id": "1", "name": "Basic", "controller": "Grid", "action": "Selection", "childcount": "0"  },
                                   { "id": "2", "name": "Selection API", "controller": "Grid", "action": "SelectionCleanup", "childcount": "0" }
                         ]
                     },
                    {
                        "id": "11", "name": "Summary", "controller": "Grid", "action": "Summary", "childcount": "1",  "samples": [
                                  { "id": "1", "name": "Basic", "controller": "Grid", "action": "Summary", "childcount": "0" },
                                  { "id": "2", "name": "Group Summary", "controller": "Grid", "action": "GroupSummary", "childcount": "0" },
                                  { "id": "3", "name": "Caption Summary", "controller": "Grid", "action": "CaptionSummary", "childcount": "0" },
                                  { "id": "4", "name": "Custom Summary", "controller": "Grid", "action": "CustomSummary", "childcount": "0"  }
                        ]
                    },
                    {
                        "id": "12", "name": "AngularJS Support", "controller": "Grid", "action": "ObservableAngular", "childcount": "1", "samples": [
                                  { "id": "2", "name": "Remote Binding", "controller": "Grid", "action": "ComprehensiveAngular", "childcount": "0"  }
                        ]
                    },
                    {
                        "id": "13", "name": "KnockOut Support", "controller": "Grid", "action": "ObservableKO", "childcount": "1", "samples": [
                                  { "id": "2", "name": "Remote Binding", "controller": "Grid", "action": "KORemoteBinding", "childcount": "0"  }
                        ]
                    },
                    {
                        "id": "14", "name": "Export and Print", "controller": "Grid", "action": "ExportingGrid", "childcount": "1", "samples": [
                                  { "id": "1", "name": "Exporting Grid", "controller": "Grid", "action": "ExportingGrid", "childcount": "0"  },
                                  { "id": "2", "name": "Multiple Exporting", "controller": "Grid", "action": "MultipleExporting", "childcount": "0"  },
                                  { "id": "3", "name": "Print Grid", "controller": "Grid", "action": "PrintGrid", "childcount": "0"  }
                        ]
                    },
                      {
                          "id": "18", "name": "Relational Binding", "controller": "Grid", "action": "ExportingGrid", "childcount": "1", "samples": [
                                   { "id": "1", "name": "Hierarchy Grid", "controller": "Grid", "action": "HierarchyGrid", "childcount": "0" },
                                    { "id": "2", "name": "Master-Details", "controller": "Grid", "action": "MasterDetails", "childcount": "0" }
                          ]
                      },
                    { "id": "16", "name": "Adaptive", "controller": "Grid", "action": "Adaptive",  "childcount": "0" },
					{ "id": "38", "name": "GridLines", "controller": "Grid", "action": "GridLines",  "childcount": "0", "type":"new"},
                    { "id": "22", "name": "Conditional Formatting", "controller": "Grid", "action": "ConditionalFormatting", "childcount": "0" },
                    { "id": "25", "name": "State Maintenance", "controller": "Grid", "action": "StateMaintenance", "childcount": "0" },
                    { "id": "26", "name": "ToolBar Template", "controller": "Grid", "action": "Toolbartemplate", "childcount": "0" },
                    { "id": "27", "name": "Keyboard Interaction", "controller": "Grid", "action": "KeyboardInteraction", "childcount": "0" },
                    { "id": "28", "name": "Scrolling", "controller": "Grid", "action": "Scrolling", "childcount": "0" },
                    { "id": "29", "name": "Events", "controller": "Grid", "action": "Events", "childcount": "0" },
                    { "id": "32", "name": "Context Menu", "controller": "Grid", "action": "ContextMenu", "childcount": "0" },
                     {
                         "id": "30", "name": "Internationalization", "controller": "Grid", "action": "ExportingGrid", "childcount": "1", "samples": [
                                  { "id": "1", "name": "Localization", "controller": "Grid", "action": "Localization",  "childcount": "0" },
                                  { "id": "2", "name": "RTL", "controller": "Grid", "action": "RTL", "childcount": "0" }
                         ]
                     }
             ]
         },
             {
                 "name": "Gantt", "id": "Gantt", "childcount": "1","Group": "DATA VISUALIZATION", "controller": "Gantt", "action": "Default", "samples": [
                     { "id": "1", "name": "Default Functionalities", "controller": "Gantt", "action": "Default", "childcount": "0" },
                     { "id": "3", "name": "Editing", "controller": "Gantt", "action": "GanttEditing", "childcount": "0" },
                     { "id": "4", "name": "Sorting", "controller": "Gantt", "action": "GanttSorting", "childcount": "0" },
                     { "id": "5", "name": "Searching", "controller": "Gantt", "action": "GanttSearching", "childcount": "0" },
                     { "id": "6", "name": "Events", "controller": "Gantt", "action": "GanttEvents", "childcount": "0" },
                     { "id": "7", "name": "Virtualization", "controller": "Gantt", "action": "Virtualization", "childcount": "0" },
                     { "id": "8", "name": "Localization", "controller": "Gantt", "action": "Localization", "childcount": "0" },
                     { "id": "9", "name": "Resource", "controller": "Gantt", "action": "Resource", "childcount": "0" },
                     { "id": "10", "name": "Baselines", "controller": "Gantt", "action": "BaseLine", "childcount": "0" },
                     { "id": "11", "name": "Schedule Modes", "controller": "Gantt", "action": "GanttScheduleModes", "childcount": "0"},
                     { "id": "12", "name": "Time Options", "controller": "Gantt", "action": "TimeOptions", "childcount": "0"},
                     { "id": "13", "name": "Timeline Validation", "controller": "Gantt", "action": "TimelineValidation", "childcount": "0" },
                     { "id": "14", "name": "Column Chooser", "controller": "Gantt", "action": "ColumnChooser", "childcount": "0" }

                 ]
             },
             {
                 "name": "TreeGrid", "id": "TreeGrid","Group": "GRIDS", "childcount": "1", "controller": "TreeGrid", "action": "Default", "type": "update", "samples": [
                     { "id": "1", "name": "Default Functionalities", "controller": "TreeGrid", "action": "Default", "childcount": "0" },
                     { "id": "2", "name": "Local Binding", "controller": "TreeGrid", "action": "TreeGridLocalBinding", "childcount": "0" },
                     { "id": "3", "name": "Editing", "controller": "TreeGrid", "action": "TreeGridEditing", "childcount": "0"},
					 { "id": "4", "name": "Sorting", "controller": "TreeGrid", "action": "TreeGridSorting", "childcount": "0" },
                     { "id": "5", "name": "Events", "controller": "TreeGrid", "action": "TreeGridEvents", "childcount": "0" },
                     { "id": "6", "name": "Column Template", "controller": "TreeGrid", "action": "TreeGridColumnTemplate", "childcount": "0" },
                     { "id": "7", "name": "Details Template", "controller": "TreeGrid", "action": "TreeGridRowDetails", "childcount": "0"},
					 { "id": "8", "name": "Context Menu", "controller": "TreeGrid", "action": "TreeGridContextMenu", "childcount": "0" },
                     { "id": "9", "name": "Column Filtering", "controller": "TreeGrid", "action": "TreeGridColumnFiltering", "childcount": "0"},
                     { "id": "10", "name": "Column Chooser", "controller": "TreeGrid", "action": "TreeGridColumnChooser", "childcount": "0" },
                     { "id": "11", "name": "Row Template", "controller": "TreeGrid", "action": "TreeGridRowTemplate", "childcount": "0" },
                     { "id": "12", "name": "Row Drag And Drop", "controller": "TreeGrid", "action": "TreeGridRowDragAndDrop", "childcount": "0" },
                     { "id": "13", "name": "Summary Row", "controller": "TreeGrid", "action": "TreeGridSummaryRow", "childcount": "0"},
                     { "id": "14", "name": "Exporting", "controller": "TreeGrid", "action": "TreeGridExporting", "childcount": "0", "type": "new" },
                     { "id": "15", "name": "Multiple Exporting", "controller": "TreeGrid", "action": "TreeGridMultipleExporting", "childcount": "0", "type": "new" },
                     {
                         "id": "16", "name": "AngularJS Templates", "controller": "TreeGrid", "childcount": "1", "action": "TreeGridAngularRowTemplate", "samples": [
                               { "id": "1", "name": "Row Template", "controller": "TreeGrid", "action": "TreeGridAngularRowTemplate", "childcount": "0" },
                               { "id": "2", "name": "Column Template", "controller": "TreeGrid", "action": "TreeGridAngularColumnTemplate", "childcount": "0" }
                         ]
                     }
                ]
             },

             {
                 "name": "Schedule", "id": "Schedule", "childcount": "1","Group": "DATA VISUALIZATION", "controller": "Schedule", "action": "Default", type: "update", "samples": [
                     { "id": "1", "name": "Default Functionalities", "controller": "Schedule", "action": "Default", "childcount": "0" },
                     {
                         "id": "2", "name": "Data Binding", "controller": "Schedule", "action": "LocalDataBinding", "childcount": "1", "samples": [
                                  { "id": "1", "name": "Local Data", "controller": "Schedule", "action": "LocalDataBinding", "childcount": "0" },
                                  { "id": "2", "name": "Remote Data", "controller": "Schedule", "action": "RemoteDataBinding", "childcount": "0" },
                                  { "id": "3", "name": "Load On Demand", "controller": "Schedule", "action": "LoadOnDemand", "childcount": "0" }
                         ]
                     },
                     { "id": "3", "name": "Time Mode", "controller": "Schedule", "action": "TimeMode", "childcount": "0" },
                     { "id": "4", "name": "Templates", "controller": "Schedule", "action": "Templates", "childcount": "0" },
                     {
                         "id": "5", "name": "Resources", "controller": "Schedule", "action": "MultipleResource", "childcount": "1", "samples": [
                                  { "id": "1", "name": "Multiple Resources", "controller": "Schedule", "action": "MultipleResource", "childcount": "0" },
                                  { "id": "2", "name": "Resource Grouping", "controller": "Schedule", "action": "ResourceGrouping", "childcount": "0" }
                         ]
                     },
                      {
                          "id": "6", "name": "Horizontal View", "controller": "Schedule", "action": "HorizontalDefault", "childcount": "1", "samples": [
                                  { "id": "1", "name": "Default", "controller": "Schedule", "action": "HorizontalDefault", "childcount": "0" },
                                  { "id": "2", "name": "Multiple Resources", "controller": "Schedule", "action": "HorizontalMultipleResource", "childcount": "0" },
                                  { "id": "3", "name": "Resource Grouping", "controller": "Schedule", "action": "HorizontalResourceGrouping", "childcount": "0" }
                          ]
                      },
                     { "id": "8", "name": "FirstDayOfWeek", "controller": "Schedule", "action": "FirstDayOfWeek", "childcount": "0", "type": "new" },
					 { "id": "9", "name": "Categorize", "controller": "Schedule", "action": "CategorizeOption", "childcount": "0"},
                     { "id": "10", "name": "Localization", "controller": "Schedule", "action": "Localization", "childcount": "0" },
                     { "id": "11", "name": "Custom Views", "controller": "Schedule", "action": "CustomViews", "childcount": "0", "type": "update" },
                     { "id": "12", "name": "Custom Window", "controller": "Schedule", "action": "CustomAppWindow", "childcount": "0" },
                     { "id": "13", "name": "Adaptive", "controller": "Schedule", "action": "Adaptive", "childcount": "0" },
                     { "id": "14", "name": "Context Menu", "controller": "Schedule", "action": "ContextMenu", "childcount": "0" },
                     { "id": "15", "name": "Tooltip", "controller": "Schedule", "action": "Tooltip", "childcount": "0", "type": "new" },
                     { "id": "16", "name": "Reminder", "controller": "Schedule", "action": "Reminder", "childcount": "0" },
                     { "id": "17", "name": "SignalR", "controller": "Schedule", "action": "SignalR", "childcount": "0" },
                     { "id": "18", "name": "Appointment Search", "controller": "Schedule", "action": "AppointmentSearch", "childcount": "0" },
                     { "id": "19", "name": "API's", "controller": "Schedule", "action": "API", "childcount": "0" },
                     { "id": "20", "name": "Events", "controller": "Schedule", "action": "Events", "childcount": "0" },
                     { "id": "21", "name": "RTL", "controller": "Schedule", "action": "RTL", "childcount": "0" },
                     { "id": "22", "name": "Keyboard Interaction", "controller": "Schedule", "action": "KeyboardInteraction", "childcount": "0" },
                     { "id": "23", "name": "Print", "controller": "Schedule", "action": "Print", "childcount": "0" },
                     { "id": "24", "name": "Import & Export", "controller": "Schedule", "action": "ScheduleICSExport", "childcount": "0" }
                    

                 ]
             },
                  {
                      "name": "Diagram", "id": "Diagram", "Group": "DATA VISUALIZATION","childcount": "1", "controller": "Diagram", "action": "Default", "type": "update", "samples": [
                         { "id": "1", "name": "Default Functionalities", "controller": "Diagram", "action": "Default", "childcount": "0" },
                         { "id": "2", "name": "Mindmap", "controller": "Diagram", "action": "MindMap", "childcount": "0" },
                         {
                             "id": "3", "name": "Organizational Chart", "controller": "Diagram", "action": "TeamOrgchart", "childcount": "1", "type": "", samples: [
                                 { "id": "1", "name": "Team", "controller": "Diagram", "action": "TeamOrgchart", "childcount": "0", "type": "" },
                                 { "id": "2", "name": "Business Management", "controller": "Diagram", "action": "BusinessChart", "childcount": "0", "type": "" },
                                 { "id": "3", "name": "Project Management", "controller": "Diagram", "action": "ProjectOrgchart", "childcount": "0", "type": "" },
                                 { "id": "4", "name": "University", "controller": "Diagram", "action": "UniversityChart", "childcount": "0", "type": "" }

                             ]
                         },
                         { "id": "4", "name": "Radial Tree", "controller": "Diagram", "action": "RadialTree", "type": "new", "childcount": "0" },
                         { "id": "5", "name": "UML Diagram", "controller": "Diagram", "action": "UMLDiagram", "type": "", "childcount": "0" },
                          { "id": "6", "name": "Localization", "controller": "Diagram", "action": "Localization", "childcount": "0", "type": "" },
                          { "id": "7", "name": "Swimlane Diagram", "controller": "Diagram", "action": "Swimlane", "childcount": "0", "type": "" },
						   { "id": "8", "name": "Drawing Tools", "controller": "Diagram", "action": "DrawingTools", "childcount": "0", "type": "" },
                          { "id": "9", "name": "Overview", "controller": "Diagram", "action": "Overview", "childcount": "0", "type": "" },
                          {
                              "id": "10", "name": "Data Binding", "controller": "Diagram", "action": "LocalDataBinding", "childcount": "1", "type": "", samples: [
                                  { "id": "1", "name": "Local DataBinding", "controller": "Diagram", "action": "LocalDataBinding", "childcount": "0", "type": "" },
                                  { "id": "2", "name": "Remote DataBinding", "controller": "Diagram", "action": "RemoteData", "childcount": "0", "type": "" },
                                  { "id": "3", "name": "HTML DataBinding", "controller": "Diagram", "action": "HTMLBinding", "childcount": "0", "type": "" }
                              ]
                          },
                          { "id": "11", "name": "Circuit Diagram", "controller": "Diagram", "action": "CircuitDiagram", "childcount": "0", "type": "" }
                      ]
                  },
                        {
                            "name": "Maps", "id": "Maps", "Group": "DATA VISUALIZATION", "childcount": "1", "controller": "Maps", "action": "Default", "samples": [
                                { "id": "1", "name": "Data Markers", "controller": "Maps", "action": "Default", "childcount": "0" },
                                { "id": "2", "name": "Drill Down", "controller": "Maps", "action": "DrillDown", "childcount": "0" },
                                { "id": "3", "name": "Label", "controller": "Maps", "action": "Labels", "childcount": "0" },
                                { "id": "4", "name": "Selection", "controller": "Maps", "action": "Selection", "childcount": "0" },
                                { "id": "5", "name": "Zooming", "controller": "Maps", "action": "Zooming", "childcount": "0" },
                                { "id": "6", "name": "HeatMap", "controller": "Maps", "action": "HeatMap", "childcount": "0" },
                                { "id": "7", "name": "Flight Routes", "controller": "Maps", "action": "FlightRoutes", "childcount": "0" }

                            ]
                        },

                         {
                             "name": "TreeMap", "id": "TreeMap", "Group": "DATA VISUALIZATION", "childcount": "1", "controller": "TreeMap", "action": "Default", "samples": [
                                  { "id": "1", "name": "Flat Collection", "controller": "TreeMap", "action": "Default", "childcount": "0" },
                                  { "id": "2", "name": "Customization", "controller": "TreeMap", "action": "Customization", "childcount": "0" },
                                   { "id": "3", "name": "Hierarchical", "controller": "TreeMap", "action": "Hierarchical", "childcount": "0" },
								   { "id": "4", "name": "DrillDown", "controller": "TreeMap", "action": "drillDown", "childcount": "0" }      

                             ]
                         },
                        {
                            "name": "Radial Gauge", "id": "CircularGauge", "Group": "DATA VISUALIZATION", "childcount": "1", "controller": "CircularGauge", "action": "Default", "samples": [
                                     { "id": "1", "name": "Default Functionalities", "controller": "CircularGauge", "action": "Default", "childcount": "0" },
                                     { "id": "2", "name": "Scale", "controller": "CircularGauge", "action": "Scale", "childcount": "0" },
                                     {
                                         "id": "3", "name": "Pointer", "controller": "CircularGauge", "action": "Pointer", "childcount": "1", "samples": [
                                             { "id": "1", "name": "Needle Pointer", "controller": "CircularGauge", "action": "Pointer", "childcount": "0" },
                                             { "id": "2", "name": "Marker Pointer", "controller": "CircularGauge", "action": "MarkerPointer", "childcount": "0" },
                                             { "id": "3", "name": "Image Pointer", "controller": "CircularGauge", "action": "PointerImage", "childcount": "0" }
                                         ]
                                     },
                                     { "id": "4", "name": "Range", "controller": "CircularGauge", "action": "Range", "childcount": "0" },
                                     { "id": "5", "name": "User Interaction", "controller": "CircularGauge", "action": "UserInteraction", "childcount": "0" },
                                     { "id": "6", "name": "Export Image", "controller": "CircularGauge", "action": "ExportImage", "childcount": "0" },
                                     { "id": "7", "name": "Label Customization", "controller": "CircularGauge", "action": "LabelCustomization", "childcount": "0" },
                                     { "id": "8", "name": "  Custom Label", "controller": "CircularGauge", "action": "CustomLabel", "childcount": "0" },
                                     { "id": "9", "name": "Tooltip", "controller": "CircularGauge", "action": "Tooltip", "childcount": "0" },
                                     { "id": "10", "name": "Half Circular", "controller": "CircularGauge", "action": "Semicircular", "childcount": "0" },
                                     { "id": "11", "name": "Frame and Angles", "controller": "CircularGauge", "action": "Frames", "childcount": "0" },
                                     {
                                         "id": "12", "name": "Use Case", "controller": "CircularGauge", "action": "Clock", "childcount": "1", "samples": [
                                            { "id": "1", "name": "Clock", "controller": "CircularGauge", "action": "Clock", "childcount": "0" },
                                            { "id": "2", "name": "Speedometer", "controller": "CircularGauge", "action": "Speedometer", "childcount": "0" }
                                         ]
                                     }
                            ]
                        },
                       {
                            "name": "Linear Gauge", "id": "LinearGauge","Group": "DATA VISUALIZATION", "childcount": "1", "controller": "LinearGauge", "action": "Default", "samples": [
                                      { "id": "1", "name": "Default Functionalities", "controller": "LinearGauge", "action": "Default", "childcount": "0" },
                                      { "id": "2", "name": "Scale", "controller": "LinearGauge", "action": "Scale", "childcount": "0" },
                                      { "id": "3", "name": "Pointer", "controller": "LinearGauge", "action": "Pointer", "childcount": "0" },
                                      { "id": "4", "name": "Range", "controller": "LinearGauge", "action": "Range", "childcount": "0" },
                                      { "id": "5", "name": "Thermometer", "controller": "LinearGauge", "action": "Thermometer", "childcount": "0" },
                                      { "id": "6", "name": "User Interaction", "controller": "LinearGauge", "action": "UserInteraction", "childcount": "0" },
                                      { "id": "9", "name": "Export Image", "controller": "LinearGauge", "action": "ExportImage", "childcount": "0" },
                                      { "id": "10", "name": "Label Customization", "controller": "LinearGauge", "action": "LabelCustomization", "childcount": "0" },
                                      { "id": "11", "name": "Tooltip", "controller": "LinearGauge", "action": "Tooltip", "childcount": "0" },
                                      { "id": "12", "name": "Custom Label", "controller": "LinearGauge", "action": "CustomLabel", "childcount": "0" },
                                      { "id": "13", "name": "Indicators", "controller": "LinearGauge", "action": "Indicators", "childcount": "0"}

                            ]
                        },
                        {
                            "name": "Digital Gauge", "id": "DigitalGauge", "Group": "DATA VISUALIZATION","childcount": "1", "controller": "DigitalGauge", "action": "Default", "samples": [
                                     { "id": "1", "name": "Default Functionalities", "controller": "DigitalGauge", "action": "Default", "childcount": "0" },
                                     { "id": "2", "name": "Digital Clock", "controller": "DigitalGauge", "action": "DigitalClock", "childcount": "0" },
                                     { "id": "5", "name": "Export Image", "controller": "DigitalGauge", "action": "ExportImage", "childcount": "0" }
                            ]
                        },
                         {
                             "name": "Bullet Graph", "id": "BulletGraph","Group": "DATA VISUALIZATION", "childcount": "1", "controller": "BulletGraph", "action": "Default", "samples": [
                                     { "id": "1", "name": "Default Functionalities", "controller": "BulletGraph", "action": "Default", "childcount": "0" },
                                     { "id": "2", "name": "Data Binding-local data", "controller": "BulletGraph", "action": "LocalDataBinding", "childcount": "0" },
                                     { "id": "3", "name": "DataBinding-Remote", "controller": "BulletGraph", "action": "RemoteDataBinding", "childcount": "0" },
                                     { "id": "4", "name": "API's", "controller": "BulletGraph", "action": "API", "childcount": "0" },
                                     { "id": "7", "name": "Indicator", "controller": "BulletGraph", "action": "Indicator", "childcount": "0" },
                                     { "id": "8", "name": "Label & Tick Positioning", "controller": "BulletGraph", "action": "LabelsAndTickPositioning", "childcount": "0"},
									 { "id": "9", "name": "Title Positioning", "controller": "BulletGraph", "action": "TitlePositioning", "childcount": "0"}
                             ]
                         },
                         {
                             "name": "Range Navigator", "id": "RangeNavigator","Group": "DATA VISUALIZATION", "childcount": "1", "controller": "RangeNavigator", "action": "Default", "samples": [
                             { "id": "1", "name": "Default Functionalities", "controller": "RangeNavigator", "action": "Default", "childcount": "0" },
                             { "id": "2", "name": "Numeric Type", "controller": "RangeNavigator", "action": "RangeNumericType", "childcount": "0" },
                             { "id": "3", "name": "RTL", "controller": "RangeNavigator", "action": "RangeRTL", "childcount": "0" },
                             { "id": "4", "name": "Localization", "controller": "RangeNavigator", "action": "Localization", "childcount": "0" },
                             { "id": "5", "name": "Customization", "controller": "RangeNavigator", "action": "RangeCustom", "childcount": "0" }
                             ]
                         },
                         {
                             "name": "Ribbon", "Group": "NAVIGATION", "id": "Ribbon", "childcount": "1", "controller": "Ribbon", "action": "Default", "type":"update", "samples": [
                             { "id": "1", "name": "Default Functionalities", "controller": "Ribbon", "action": "Default", "childcount": "0" },
                             { "id": "2", "name": "API's", "controller": "Ribbon", "action": "Methods", "childcount": "0", "type": "update" },
							 { "id": "3", "name": "Events", "controller": "Ribbon", "action": "Events", "childcount": "0"},
                             { "id": "4", "name": "Resize", "controller": "Ribbon", "action": "Resize", "childcount": "0" },
                             { "id": "5", "name": "Gallery", "controller": "Ribbon", "action": "Gallery", "childcount": "0" },
                             { "id": "6", "name": "Custom Tool Tip", "controller": "Ribbon", "action": "CustomToolTip", "childcount": "0" },
                             { "id": "8", "name": "Backstage Page", "controller": "Ribbon", "action": "BackStage", "childcount": "0" }
                             ]
                         },
                         {
                             "name": "RadialMenu","Group": "NAVIGATION", "id": "RadialMenu", "childcount": "1", "controller": "RadialMenu", "action": "Default", "samples": [
                             { "id": "1", "name": "Default", "controller": "RadialMenu", "action": "Default", "childcount": "0" },
                             { "id": "2", "name": "Nested Radial Menu", "controller": "RadialMenu", "action": "NestedRadialMenu", "childcount": "0" },
                             { "id": "3", "name": "Methods", "controller": "RadialMenu", "action": "Methods", "childcount": "0", "type": "new" }
                             ]
                         },

                    {
                        "name": "ReportViewer", "id": "ReportViewer","Group": "REPORTING", "childcount": "1", "controller": "ReportViewer", "action": "Default", "samples": [
                            { "id": "1", "name": "SSRS", "controller": "ReportViewer", "action": "Default", "childcount": "0" },
                            {
                                "id": "2", "name": "Data Binding", "controller": "ReportViewer", "action": "DataBindingRemote", "childcount": "1", "samples": [
                                         { "id": "1", "name": "Remote Data", "controller": "ReportViewer", "action": "DataBindingRemote", "childcount": "0" },
                                         { "id": "2", "name": "Local Data", "controller": "ReportViewer", "action": "DataBindingLocal", "childcount": "0" }
                                ]
                            },
                            { "id": "4", "name": "Conditional Formatting", "controller": "ReportViewer", "action": "ConditionalFormatting", "childcount": "0" },
                            { "id": "5", "name": "Master Detail", "controller": "ReportViewer", "action": "MasterDetail", "childcount": "0" },
                            { "id": "6", "name": "Side By Side", "controller": "ReportViewer", "action": "SidebySide", "childcount": "0" },
                            { "id": "7", "name": "Mail Merge", "controller": "ReportViewer", "action": "MailMerge", "childcount": "0" },
                            {
                                "id": "8", "name": "Interactive Reports", "controller": "ReportViewer", "action": "DrillDown", "childcount": "1", "samples": [
                                          { "id": "1", "name": "DrillDown", "controller": "ReportViewer", "action": "DrillDown", "childcount": "0" },
                                          //{ "id": "2", "name": "DocumentMap", "controller": "ReportViewer", "action": "DocumentMap", "childcount": "0" },
                                          { "id": "3", "name": "DrillThrough", "controller": "ReportViewer", "action": "DrillThrough", "childcount": "0" }
                                ]
                            },
                            { "id": "9", "name": "Localization", "controller": "ReportViewer", "action": "Localization", "childcount": "0" },
							{
							    "id": "10", "name": "Usage Scenario", "controller": "ReportViewer", "action": "ProductLineSales", "childcount": "1", "samples": [
                                          { "id": "1", "name": "Product Line Sales", "controller": "ReportViewer", "action": "ProductLineSales", "childcount": "0" },
                                          { "id": "2", "name": "Sales Analysis", "controller": "ReportViewer", "action": "ProductSalesAnalysis", "childcount": "0" },
                                          { "id": "3", "name": "Product List", "controller": "ReportViewer", "action": "ProductList", "childcount": "0" },
                                          { "id": "4", "name": "Company Sales", "controller": "ReportViewer", "action": "SalesPerYear", "childcount": "0" },
                                          { "id": "5", "name": "Sales Dashboard", "controller": "ReportViewer", "action": "SalesDashboard", "childcount": "0" }

							    ]
							}
                        ]
                    },

                     {
                         "name": "OlapClient", "id": "OlapClient","Group": "BUSINESS INTELLIGENCE", "childcount": "1", "controller": "OlapClient", "action": "Default", "type": "update", "samples": [
                             { "id": "1", "name": "Default Functionalities", "controller": "OlapClient", "action": "Default", "childcount": "0" },
                             { "id": "2", "name": "Customization", "controller": "OlapClient", "action": "Customization", "childcount": "0", "type": "" },
                             { "id": "3", "name": "Localization", "controller": "OlapClient", "action": "Localization", "childcount": "0" },
                             { "id": "4", "name": "Exporting Modes", "controller": "OlapClient", "action": "ExportingModes", "childcount": "0", "type": "" },
                             { "id": "5", "name": "Defer Update", "controller": "OlapClient", "action": "DeferUpdate", "childcount": "0", "type": "new" }

                         ]
                     },
                    {
                        "name": "PivotGrid", "id": "PivotGrid","Group": "BUSINESS INTELLIGENCE", "childcount": "1", "controller": "PivotGrid", "action": "Default", "type": "update", "samples": [
                              { "id": "1", "name": "Default Functionalities","controller": "PivotGrid", "action": "Default", "childcount": "0" },
                              { "id": "2", "name": "KPI Report","controller": "PivotGrid", "action": "KPI", "childcount": "0" },
                              { "id": "3", "name": "Drill Types","controller": "PivotGrid", "action": "DrillPosition", "childcount": "0" },
                              { "id": "4", "name": "Cell Context","controller": "PivotGrid", "action": "CellContext", "childcount": "0", "type": " " },
                              { "id": "5", "name": "Hyperlink","controller": "PivotGrid", "action": "Hyperlink", "childcount": "0", "type": " " },
                              { "id": "6", "name": "Grid Layouts","controller": "PivotGrid", "action": "Layout", "childcount": "0", "type": " " },
                              { "id": "7", "name": "Localization","controller": "PivotGrid", "action": "Localization", "childcount": "0", "type": "" },
                              { "id": "8", "name": "RTL","controller": "PivotGrid", "action": "RTL", "childcount": "0", "type": "" },
                              { "id": "9", "name": "Paging", "controller": "PivotGrid", "action": "Paging", "childcount": "0", "type": "" },
                              { "id": "10", "name": "Cell Selection", "controller": "PivotGrid", "action": "CellSelection", "childcount": "0", "type": "" },
                              { "id": "11", "name": "Conditional Formatting", "controller": "PivotGrid", "action": "ConditionalFormatting", "childcount": "0", "type": "" },
                              { "id": "12", "name": "Exporting", "controller": "PivotGrid", "action": "Exporting", "childcount": "0", "type": "update" },
                              {
                                  "id": "15", "name": "Data Binding", "controller": "PivotGrid", "action": "Olap", "childcount": "1", "type": "update", samples: [
                                    { "id": "1", "name": "OLAP Binding", "controller": "PivotGrid", "action": "Olap", "childcount": "0", "type": "update" },
                                    { "id": "2", "name": "Relational Binding", "controller": "PivotGrid", "action": "Pivot", "childcount": "0", "type": "update" }
                            ]

                              },
							  { "id": "16", "name": "Grouping Bar", "controller": "PivotGrid", "action": "GroupingBar", "childcount": "0", "type": "" }

                        ]
                    },
                    {
                        "name": "OlapChart", "id": "OlapChart","Group": "BUSINESS INTELLIGENCE", "childcount": "1", "controller": "OlapChart", "action": "Default", "type": "update", "samples": [
                            { "id": "1", "name": "Default Functionalities", "controller": "OlapChart", "action": "Default", "childcount": "0" },
                            { "id": "2", "name": "Chart Types", "controller": "OlapChart", "action": "ChartTypes", "childcount": "0" },
                            { "id": "3", "name": "Localization", "controller": "OlapChart", "action": "Localization", "childcount": "0" },
                            { "id": "4", "name": "Exporting", "controller": "OlapChart", "action": "Exporting", "childcount": "0", "type": "" },
                            { "id": "5", "name": "Chart3D", "controller": "OlapChart", "action": "Chart3D", "childcount": "0", "type": "new" }

                        ]
                    },

                    {
                        "name": "OlapGauge", "id": "OlapGauge","Group": "BUSINESS INTELLIGENCE", "childcount": "1", "controller": "OlapGauge", "action": "Default","type": "", "samples": [
                            { "id": "1", "name": "Default Functionalities", "controller": "OlapGauge", "action": "Default", "childcount": "0" },
                            { "id": "2", "name": "Events", "controller": "OlapGauge", "action": "Events", "childcount": "0" },
                            { "id": "3", "name": "Layout", "controller": "OlapGauge", "action": "Layout", "childcount": "0" },
                            { "id": "4", "name": "Localization", "controller": "OlapGauge", "action": "Localization", "childcount": "0", "type": "" },
                            { "id": "5", "name": "Pointer", "controller": "OlapGauge", "action": "Pointer", "childcount": "0" },
                            { "id": "6", "name": "Range", "controller": "OlapGauge", "action": "Range", "childcount": "0" },
                            { "id": "7", "name": "Scale", "controller": "OlapGauge", "action": "Scale", "childcount": "0" }
                        ]
                    },
                   {
                       "name": "Presentation", "id": "Presentation", "Group": "FILE FORMATS","childcount": "2", "type": "preview", "controller": "Presentation", "action": "Default", "samples": [

                                { "id": "1", "name": "Default", "controller": "Presentation", "action": "Default", "childcount": "0" },
                                
                                {
                                    "id": "2", "name": "Getting Started", "controller": "Presentation", "action": "HelloWorld", "childcount": "1", "samples": [
                                     { "id": "31", "name": "Hello World", "controller": "Presentation", "action": "HelloWorld", "childcount": "0" }
                                     ]
                                },
                                 {
                                     "id": "3", "name": "Slide Elements", "controller": "Presentation", "action": "Images", "childcount": "1", "samples": [
                                      { "id": "41", "name": "Images", "controller": "Presentation", "action": "Images", "childcount": "0" },
                                      { "id": "42", "name": "Slides", "controller": "Presentation", "action": "Slides", "childcount": "0" },
                                      { "id": "43", "name": "Tables", "controller": "Presentation", "action": "Tables", "childcount": "0" }
                                     ]
                                 },
                                   {
                                       "id": "4", "name": "Conversion", "controller": "Presentation", "action": "PPTXToImage", "childcount": "1", "samples": [
                                        { "id": "51", "name": "PPTX to Image", "controller": "Presentation", "action": "PPTXToImage", "childcount": "0" },
                                        { "id": "52", "name": "PPTX to PDF", "controller": "Presentation", "action": "PPTXToPdf", "childcount": "0" }

                                       ]
                                   },
                                    {
                                        "id": "5", "name": "Clone and Merge", "controller": "Presentation", "action": "CloningSlides", "childcount": "1", "samples": [
                                         { "id": "61", "name": "Cloning Slide", "controller": "Presentation", "action": "CloningSlides", "childcount": "0" },
                                         { "id": "62", "name": "Merging Presentations", "controller": "Presentation", "action": "MergingPresentation", "childcount": "0" }

                                        ]
                                    },
                                    {
                                        "id": "6", "name": "Security", "controller": "Presentation", "action": "EncryptAndDecrypt", "childcount": "1", "samples": [
                                         { "id": "71", "name": "Encryption and Decryption", "controller": "Presentation", "action": "EncryptAndDecrypt", "childcount": "0" }
                                        ]
                                    }
                                   ]                                       
                                   },
                			   {
                			       "name": "XlsIO", "id": "XlsIO", "childcount": "1","Group": "FILE FORMATS", "controller": "XlsIO", "action": "Default", "samples": [
                        { "id": "1", "name": "Default", "controller": "XlsIO", "action": "Default", "childcount": "0" },
                        {
                            "id": "2", "name": "Product Showcase", "controller": "XlsIO", "action": "BudgetPlanner", "childcount": "1", samples: [
                                { "id": "1", "name": "Budget Planner", "controller": "XlsIO", "action": "BudgetPlanner", "childcount": "0" },
                                { "id": "2", "name": "Excel To PDF", "controller": "XlsIO", "action": "ExcelToPDF", "childcount": "0" }
                            ]
                        },
                        {
                            "id": "3", "name": "Getting Started", "controller": "XlsIO", "action": "Create", "childcount": "1", samples: [
                                { "id": "1", "name": "Create", "controller": "XlsIO", "action": "Create", "childcount": "0" },
                                { "id": "2", "name": "Find And Extract", "controller": "XlsIO", "action": "FindAndExtract", "childcount": "0" }
                            ]
                        },
                        {
                            "id": "4", "name": "Formatting", "controller": "XlsIO", "action": "FormatCells", "childcount": "1", samples: [
                                { "id": "1", "name": "Format Cells", "controller": "XlsIO", "action": "FormatCells", "childcount": "0" },
                                { "id": "2", "name": "Styles And Formatting", "controller": "XlsIO", "action": "StylesAndFormatting", "childcount": "0" },
                                { "id": "3", "name": "Conditional Formatting", "controller": "XlsIO", "action": "ConditionalFormatting", "childcount": "0" }
                            ]
                        },
                        {
                            "id": "5", "name": "Charts", "controller": "XlsIO", "action": "ChartWorksheet", "childcount": "1", samples: [
                                { "id": "1", "name": "Chart Worksheet", "controller": "XlsIO", "action": "ChartWorksheet", "childcount": "0" },
                                { "id": "2", "name": "Embedded Chart", "controller": "XlsIO", "action": "EmbeddedChart", "childcount": "0" },
                                { "id": "3", "name": "Sparklines", "controller": "XlsIO", "action": "Sparklines", "childcount": "0" }
                            ]
                        },
                        {
                            "id": "6", "name": "Data Management", "controller": "XlsIO", "action": "RangeManipulation", "childcount": "1", samples: [
                                { "id": "1", "name": "Range Manipulation", "controller": "XlsIO", "action": "RangeManipulation", "childcount": "0" },
                                { "id": "2", "name": "Formulas", "controller": "XlsIO", "action": "Formulas", "childcount": "0" },
                                { "id": "3", "name": "Compute All Formulas", "controller": "XlsIO", "action": "ComputeAllformulas", "childcount": "0" },
                                { "id": "4", "name": "Data Validation", "controller": "XlsIO", "action": "DataValidation", "childcount": "0" },
                                { "id": "5", "name": "Performance", "controller": "XlsIO", "action": "Performance", "childcount": "0" },
                                { "id": "6", "name": "Interactive Features", "controller": "XlsIO", "action": "InteractiveFeatures", "childcount": "0" },
                                { "id": "7", "name": "Form Controls", "controller": "XlsIO", "action": "FormControls", "childcount": "0" },
                                { "id": "8", "name": "Data Sorting", "controller": "XlsIO", "action": "DataSorting", "childcount": "0" }
                            ]

                        },
                        {
                            "id": "7", "name": "Data Binding", "controller": "XlsIO", "action": "ExternalConnection", "childcount": "1", "type": "update", samples: [
                                { "id": "1", "name": "External Connection", "controller": "XlsIO", "action": "ExternalConnection", "childcount": "0" },
                                { "id": "2", "name": "Template Marker", "controller": "XlsIO", "action": "TemplateMarker", "childcount": "0" },
                                { "id": "3", "name": "Business Objects", "controller": "XlsIO", "action": "BusinessObjects", "childcount": "0" },
                                { "id": "4", "name": "Invoice", "controller": "XlsIO", "action": "Invoice", "childcount": "0" }
                            ]
                        },
                        {
                            "id": "8", "name": "Sheet Management", "controller": "XlsIO", "action": "RowColumnManipulation", "childcount": "1", samples: [
                                { "id": "1", "name": "Row-Column Manipulation", "controller": "XlsIO", "action": "RowColumnManipulation", "childcount": "0" },
                                { "id": "2", "name": "Worksheet Management", "controller": "XlsIO", "action": "WorksheetManipulation", "childcount": "0" },
                                { "id": "3", "name": "Worksheet To Image", "controller": "XlsIO", "action": "WorksheetToImage", "childcount": "0" }
                            ]
                        },
                        {
                            "id": "9", "name": "Settings", "controller": "XlsIO", "action": "DocumentationSettings", "childcount": "1", samples: [
                                { "id": "1", "name": "Documentation Settings", "controller": "XlsIO", "action": "DocumentationSettings", "childcount": "0" },
                                { "id": "2", "name": "Worksheet Protection", "controller": "XlsIO", "action": "WorksheetProtection", "childcount": "0" },
                                { "id": "3", "name": "Workbook Protection", "controller": "XlsIO", "action": "WorkbookProtection", "childcount": "0" },
                                { "id": "4", "name": "Encrypt and Decrypt", "controller": "XlsIO", "action": "EncryptAndDecrypt", "childcount": "0" }
                            ]
                        },
                        {
                            "id": "10", "name": "Business Intelligence", "controller": "XlsIO", "action": "Tables", "childcount": "1", samples: [
                                { "id": "1", "name": "Tables", "controller": "XlsIO", "action": "Tables", "childcount": "0" },
                                { "id": "2", "name": "Pivot Table", "controller": "XlsIO", "action": "PivotTable", "childcount": "0" },
                                { "id": "3", "name": "Pivot Chart", "controller": "XlsIO", "action": "PivotChart", "childcount": "0" }
                            ]
                        },
                        {
                            "id": "11", "name": "Shapes", "controller": "XlsIO", "action": "AutoShapes", "childcount": "1", samples: [
                                { "id": "1", "name": "AutoShapes", "controller": "XlsIO", "action": "AutoShapes", "childcount": "0" }
                            ]
                        }
                			       ]
                			   },
                                         {

                                             "name": "PDF", "id": "PDF","Group": "FILE FORMATS", "childcount": "2", "controller": "Pdf", "action": "Default", "samples": [

                                        { "id": "1", "name": "Default", "controller": "Pdf", "action": "Default", "childcount": "0" },

                                        {
                                            "id": "2", "name": "Product Showcase", "controller": "Pdf", "action": "JobApplication", "childcount": "1", "samples": [
                                         { "id": "1", "name": "Job Application", "controller": "Pdf", "action": "JobApplication", "childcount": "0" },
                                         { "id": "2", "name": "Invoice", "controller": "Pdf", "action": "InvoiceSample", "childcount": "0" }
                                            ]
                                        },
                                        {
                                            "id": "3", "name": "Getting Started", "controller": "Pdf", "action": "HelloWorld", "childcount": "1", "samples": [
                                           { "id": "1", "name": "Hello World", "controller": "Pdf", "action": "HelloWorld", "childcount": "0" },
                                           { "id": "2", "name": "PDF Conformance", "controller": "Pdf", "action": "PdfConformance", "childcount": "0" },
                                           { "id": "3", "name": "PDF Compression", "controller": "Pdf", "action": "PdfCompression", "childcount": "0" }
                                            ]
                                        },
                                          {
                                              "id": "4", "name": "Graphics", "controller": "Pdf", "action": "Barcode", "childcount": "1", "samples": [
                                           { "id": "1", "name": "Barcode", "controller": "Pdf", "action": "Barcode", "childcount": "0" },
                                           { "id": "2", "name": "Drawing Shapes", "controller": "Pdf", "action": "DrawingShapes", "childcount": "0" },
                                           { "id": "3", "name": "Graphic Brushes", "controller": "Pdf", "action": "GraphicBrushes", "childcount": "0" },
                                           { "id": "4", "name": "Image Insertion", "controller": "Pdf", "action": "ImageInsertion", "childcount": "0" }
                                              ]
                                          },

                                           {
                                               "id": "5", "name": "Tables", "controller": "Pdf", "action": "NorthwindReport", "childcount": "1", "samples": [
                                          { "id": "1", "name": "Northwind Report", "controller": "Pdf", "action": "NorthwindReport", "childcount": "0" },
                                          { "id": "2", "name": "Table Features", "controller": "Pdf", "action": "TableFeatures", "childcount": "0" }
                                               ]
                                           },
                                            {
                                                "id": "6", "name": "Drawing Text", "controller": "Pdf", "action": "TextFlow", "childcount": "1", "samples": [
                                           { "id": "1", "name": "Text Flow", "controller": "Pdf", "action": "TextFlow", "childcount": "0" },
                                           { "id": "2", "name": "RTL Support", "controller": "Pdf", "action": "RtlSupport", "childcount": "0" },
                                           { "id": "3", "name": "Bullets and Lists", "controller": "Pdf", "action": "BulletsandLists", "childcount": "0" },
                                           { "id": "4", "name": "Multi Column HTML Text", "controller": "Pdf", "action": "MultiColumnHTMLText", "childcount": "0" }
                                                ]
                                            },

                                             {
                                                 "id": "7", "name": "Security", "controller": "Pdf", "action": "Encryption", "childcount": "1", "samples": [
                                         { "id": "1", "name": "Encryption", "controller": "Pdf", "action": "Encryption", "childcount": "0" }
                                                 ]
                                             },

                                              {
                                                  "id": "8", "name": "Settings", "controller": "Pdf", "action": "DocumentSettings", "childcount": "1", "samples": [
                                           { "id": "1", "name": "Document Settings", "controller": "Pdf", "action": "DocumentSettings", "childcount": "0" },
                                           { "id": "2", "name": "Page Settings", "controller": "Pdf", "action": "PageSettings", "childcount": "0" },
                                           { "id": "3", "name": "Headers and Footers", "controller": "Pdf", "action": "HeadersandFooters", "childcount": "0" },
                                           { "id": "4", "name": "Layers", "controller": "Pdf", "action": "Layers", "childcount": "0" }
                                                  ]
                                              },
                                              {
                                                  "id": "9", "name": "User Interaction", "controller": "Pdf", "action": "InteractiveFeatures", "childcount": "1", "samples": [
                                           { "id": "1", "name": "Interactive Features", "controller": "Pdf", "action": "InteractiveFeatures", "childcount": "0" },
                                           { "id": "2", "name": "Form Filling", "controller": "Pdf", "action": "FormFilling", "childcount": "0" },
                                           { "id": "3", "name": "Portfolio", "controller": "Pdf", "action": "Portfolio", "childcount": "0" }
                                                  ]
                                              },
                                                 {
                                                     "id": "10", "name": "Import and Export", "controller": "Pdf", "action": "TextExtraction", "childcount": "1", "samples": [
                                           { "id": "1", "name": "Text Extraction", "controller": "Pdf", "action": "TextExtraction", "childcount": "0" },
                                           { "id": "2", "name": "RTF to PDF", "controller": "Pdf", "action": "RTFtoPDF", "childcount": "0" },
                                           { "id": "3", "name": "Doc to PDF", "controller": "Pdf", "action": "DoctoPDF", "childcount": "0" },
                                           { "id": "4", "name": "HTML to PDF", "controller": "Pdf", "action": "HtmltoPDF", "childcount": "0" },
                                           { "id": "5", "name": "XPS to PDF", "controller": "Pdf", "action": "XPStoPDF", "childcount": "0" }
                                                     ]
                                                 },

                                                       {
                                                           "id": "12", "name": "Modify Documents", "controller": "Pdf", "action": "MergeDocuments", "childcount": "1", "samples": [
                                           { "id": "1", "name": "Merge Documents", "controller": "Pdf", "action": "MergeDocuments", "childcount": "0" },
                                           { "id": "2", "name": "Overlay Documents", "controller": "Pdf", "action": "OverlayDocuments", "childcount": "0" },
                                           { "id": "3", "name": "Booklet", "controller": "Pdf", "action": "Booklet", "childcount": "0" }
                                                           ]
                                                       },

                                                         {
                                                             "id": "13", "name": "OCR", "controller": "Pdf", "action": "PdfOCR", "childcount": "1", "samples": [
                                         { "id": "1", "name": "PDF OCR", "controller": "Pdf", "action": "PdfOCR", "childcount": "0" }
                                                             ]
                                                         }
                                             ]
                                         },
                    {
                        "name": "DocIO", "id": "DocIO", "childcount": "3","Group": "FILE FORMATS", "controller": "DocIO", "action": "Default", "samples": [
                         { "id": "1", "name": "Default", "controller": "DocIO", "action": "Default", "childcount": "0" },
                         {
                             "id": "2", "name": "Product Showcase", "controller": "DocIO", "action": "SalesInvoice", "childcount": "1", "samples": [
                                    { "id": "1", "name": "Sales Invoice", "controller": "DocIO", "action": "SalesInvoice", "childcount": "0" },
                                    { "id": "2", "name": "Update Fields", "controller": "DocIO", "action": "UpdateFields", "childcount": "0" }
                             ]
                         },
                         {
                             "id": "3", "name": "Getting Started", "controller": "DocIO", "action": "HelloWorld", "childcount": "1", "samples": [
                                     { "id": "1", "name": "Hello World", "controller": "DocIO", "action": "HelloWorld", "childcount": "0" }
                             ]
                         },
                         {
                             "id": "4", "name": "Editing", "controller": "DocIO", "action": "AdvancedReplace", "childcount": "1", "samples": [
                                      { "id": "1", "name": "Advanced Replace", "controller": "DocIO", "action": "AdvancedReplace", "childcount": "0" },
                                      { "id": "2", "name": "Bookmark Navigation", "controller": "DocIO", "action": "BookmarkNavigation", "childcount": "0" },
                                      { "id": "3", "name": "Forms", "controller": "DocIO", "action": "Forms", "childcount": "0" }
                             ]
                         },
                         {
                             "id": "5", "name": "Formatting", "controller": "DocIO", "action": "FormatTable", "childcount": "1", "samples": [
                                      { "id": "1", "name": "Format Table", "controller": "DocIO", "action": "FormatTable", "childcount": "0" },
                                      { "id": "2", "name": "Format Text", "controller": "DocIO", "action": "FormatText", "childcount": "0" },
                                      { "id": "3", "name": "RTL Support", "controller": "DocIO", "action": "RTLSupport", "childcount": "0" },
                                      { "id": "4", "name": "Styles", "controller": "DocIO", "action": "Styles", "childcount": "0" },
                                      { "id": "5", "name": "Table Styles", "controller": "DocIO", "action": "TableStyles", "childcount": "0" }
                             ]
                         },
                         {
                             "id": "6", "name": "Insert Content", "controller": "DocIO", "action": "Bookmarks", "childcount": "1", "samples": [
                                      { "id": "1", "name": "Bookmarks", "controller": "DocIO", "action": "Bookmarks", "childcount": "0" },
                                      { "id": "2", "name": "Clone and Merge", "controller": "DocIO", "action": "CloneandMerge", "childcount": "0" },
                                      { "id": "3", "name": "Header and Footer", "controller": "DocIO", "action": "HeaderandFooter", "childcount": "0" },
                                      { "id": "4", "name": "Image Insertion", "controller": "DocIO", "action": "ImageInsertion", "childcount": "0" },
                                      { "id": "5", "name": "Insert OLE Object", "controller": "DocIO", "action": "InsertOLEObject", "childcount": "0" }
                             ]
                         },
                         {
                             "id": "7", "name": "Mail Merge", "controller": "DocIO", "action": "EmployeeReport", "childcount": "1", "samples": [
                                      { "id": "1", "name": "Employee Report", "controller": "DocIO", "action": "EmployeeReport", "childcount": "0" },
                                      { "id": "2", "name": "Mail Merge Event", "controller": "DocIO", "action": "MailMergeEvent", "childcount": "0" },
                                      { "id": "3", "name": "Nested Mail Merge", "controller": "DocIO", "action": "NestedMailMerge", "childcount": "0" }
                             ]
                         },
                         {
                             "id": "8", "name": "Page Layout", "controller": "DocIO", "action": "InsertBreak", "childcount": "1", "samples": [
                                      { "id": "1", "name": "Insert Break", "controller": "DocIO", "action": "InsertBreak", "childcount": "0" },
                                      { "id": "2", "name": "Watermark", "controller": "DocIO", "action": "Watermark", "childcount": "0" }
                             ]
                         },
                          {
                              "id": "9", "name": "View", "controller": "DocIO", "action": "DocumentSettings", "childcount": "1", "samples": [
                                       { "id": "1", "name": "Document Settings", "controller": "DocIO", "action": "DocumentSettings", "childcount": "0" },
                                       { "id": "2", "name": "Macro Preservation", "controller": "DocIO", "action": "MacroPreservation", "childcount": "0" }
                              ]
                          },
                          {
                              "id": "10", "name": "Security", "controller": "DocIO", "action": "DocumentProtection", "childcount": "1", "samples": [
                                       { "id": "1", "name": "Document Protection", "controller": "DocIO", "action": "DocumentProtection", "childcount": "0" },
                                       { "id": "2", "name": "Encrypt and Decrypt", "controller": "DocIO", "action": "EncryptandDecrypt", "childcount": "0" }
                              ]
                          },
                          {
                              "id": "11", "name": "References", "controller": "DocIO", "action": "FootnotesandEndnotes", "childcount": "1", "samples": [
                                       { "id": "1", "name": "Footnotes and Endnotes", "controller": "DocIO", "action": "FootnotesandEndnotes", "childcount": "0" },
                                       { "id": "2", "name": "Table of Content", "controller": "DocIO", "action": "TableofContent", "childcount": "0" }
                              ]
                          },
                          {
                              "id": "12", "name": "Import and Export", "controller": "DocIO", "action": "DOCToEPub", "childcount": "1", "samples": [
                                       { "id": "1", "name": "Word to EPub", "controller": "DocIO", "action": "DOCToEPub", "childcount": "0" },
                                       { "id": "2", "name": "Word to PDF", "controller": "DocIO", "action": "DOCtoPDF", "childcount": "0" },
                                       { "id": "3", "name": "HTML to Word", "controller": "DocIO", "action": "HTMLtoDOC", "childcount": "0" },
                                       { "id": "4", "name": "RTF to Word", "controller": "DocIO", "action": "RTFToDoc", "childcount": "0" },
                                       { "id": "5", "name": "Word to Image", "controller": "DocIO", "action": "WordtoImage", "childcount": "0" }
                              ]
                          },
                           {
                               "id": "13", "name": "Shapes", "controller": "DocIO", "action": "AutoShapes", "childcount": "1", "samples": [
                                        { "id": "1", "name": "AutoShapes", "controller": "DocIO", "action": "AutoShapes", "childcount": "0" }
                               ]
                           },
                           {
                               "id": "14", "name": "Chart", "controller": "DocIO", "action": "PieChart", "childcount": "1", "samples": [
                                        { "id": "1", "name": "Pie Chart", "controller": "DocIO", "action": "PieChart", "childcount": "0" },
                                        { "id": "2", "name": "Bar Chart", "controller": "DocIO", "action": "BarChart", "childcount": "0" }
                               ]
                           }
                        ]
                    },
                    {
                        "name":"Predictive Analytics", "id":"Analytics","Group": "DATA SCIENCE", "childcount":"0","action":"", "controller":"","samples": []
                    },
                    {
                        "name": "RichTextEditor", "id": "RTE", "Group": "EDITORS", "childcount": "1", "controller": "RTE", "type": "update" ,"action": "Default", "samples": [
                        { "id": "1", "name": "Default Functionalities", "controller": "RTE", "action": "Default", "childcount": "0" },
                        { "id": "2", "name": "All Tools", "controller": "RTE", "action": "AllTools", "childcount": "0" },
                       //{ "id": "3", "name": "Custom Tool", "controller": "RTE", "action": "CustomTool", "childcount": "0" },
                        { "id": "4", "name": "Methods", "controller": "RTE", "action": "Methods", "childcount": "0","type": "update" },
                        { "id": "5", "name": "Events", "controller": "RTE", "action": "Events", "childcount": "0" },
                        { "id": "6", "name": "File & Image Browser", "controller": "RTE", "action": "FileAndImageBrowser", "childcount": "0" },
                        { "id": "7", "name": "Localization", "controller": "RTE", "action": "Localization", "childcount": "0" },
                        { "id": "10", "name": "RTL", "controller": "RTE", "action": "Rtl", "childcount": "0" },                        
                        { "id": "11", "name": "Keyboard Navigation", "controller": "RTE", "action": "KeyboardNavigation", "childcount": "0" }
                        ]
                    },
					{
					    "name": "FileExplorer", "id": "FileExplorer", "Group": "NAVIGATION", "childcount": "1", "controller": "FileExplorer", "action": "Default", "samples": [
                        { "id": "1", "name": "Default Functionalities", "controller": "FileExplorer", "action": "Default", "childcount": "0" },
                        { "id": "2", "name": "Custom Tool", "controller": "FileExplorer", "action": "CustomTool", "childcount": "0" },
                        { "id": "3", "name": "Methods", "controller": "FileExplorer", "action": "Methods", "childcount": "0" },
                        { "id": "4", "name": "Events", "controller": "FileExplorer", "action": "Events", "childcount": "0" },
                        { "id": "5", "name": "Localization", "controller": "FileExplorer", "action": "Localization", "childcount": "0" },                        
                        { "id": "6", "name": "RTL", "controller": "FileExplorer", "action": "Rtl", "childcount": "0" },
                        { "id": "7", "name": "Keyboard Navigation", "controller": "FileExplorer", "action": "Keyboard", "childcount": "0" }
                        ]
                    },
                    {
                        "name": "TreeView", "id": "TreeView", "Group": "NAVIGATION", "childcount": "1", "controller": "TreeView", "action": "Default", "samples": [
                            { "id": "1", "name": "Default Functionalities", "controller": "TreeView", "action": "Default", "childcount": "0" },
                            { "id": "2", "name": "Icons", "controller": "TreeView", "action": "Icon", "childcount": "0" },
                            { "id": "3", "name": "Checkboxes", "controller": "TreeView", "action": "Checkbox", "childcount": "0" },
                            { "id": "4", "name": "Node Editing-Cut-Paste", "controller": "TreeView", "action": "NodeEdit", "childcount": "0" },
                            { "id": "5", "name": "Drag and Drop", "controller": "TreeView", "action": "Dragdrop", "childcount": "0" },
                            { "id": "6", "name": "Local Data", "controller": "TreeView", "action": "Localdata", "childcount": "0" },
                            { "id": "7", "name": "Remote Data", "controller": "TreeView", "action": "RemoteData", "childcount": "0" },
                            { "id": "8", "name": "Load On Demand", "controller": "TreeView", "action": "LoadonDemand", "childcount": "0" },
                            { "id": "9", "name": "Template", "controller": "TreeView", "action": "Template","childcount": "0" },
                            { "id": "10", "name": "Context Menu", "controller": "TreeView", "action": "ContextMenu", "childcount": "0" },
                            { "id": "11", "name": "State Maintenance", "controller": "TreeView", "action": "Statemaintenance", "childcount": "0" },
                            { "id": "12", "name": "Methods", "controller": "TreeView", "action": "Methods", "childcount": "0" },
                            { "id": "13", "name": "Events", "controller": "TreeView", "action": "Events", "childcount": "0" },
                            { "id": "16", "name": "RTL", "controller": "TreeView", "action": "Rtl", "childcount": "0" },
                            { "id": "17", "name": "Keyboard Navigation", "controller": "TreeView", "action": "Keyboard", "childcount": "0" }
                        ]
                    },
					{
					    "name": "ColorPicker", "id": "ColorPicker", "Group": "EDITORS","type": "update", "childcount": "1", "controller": "ColorPicker", "action": "Default", "samples": [
                            { "id": "1", "name": "Default Functionalities", "controller": "ColorPicker", "action": "Default", "childcount": "0" },
                            { "id": "2", "name": "Display Inline", "controller": "ColorPicker", "action": "DisplayInline", "childcount": "0" },
                            { "id": "3", "name": "Color Palette", "controller": "ColorPicker", "action": "PaletteModel", "childcount": "0", "type": "update" },
                            { "id": "4", "name": "Presets", "controller": "ColorPicker", "action": "Presets", "childcount": "0" },
                            { "id": "5", "name": "Custom Palette", "controller": "ColorPicker", "action": "CustomPalette", "childcount": "0" },
                            { "id": "8", "name": "Methods", "controller": "ColorPicker", "action": "Methods", "childcount": "0" },
                            { "id": "9", "name": "Events", "controller": "ColorPicker", "action": "Events", "childcount": "0" },
                            { "id": "10", "name": "Keyboard Navigation", "controller": "ColorPicker", "action": "Keyboard", "childcount": "0" }

					    ]
					},
                    {
                        "name": "DatePicker", "id": "DatePicker", "Group": "EDITORS", "childcount": "1", "controller": "DatePicker", "action": "Default", "samples": [
                            { "id": "1", "name": "Default Functionalities", "controller": "DatePicker", "action": "Default", "childcount": "0" },
                            { "id": "2", "name": "Date Format", "controller": "DatePicker", "action": "Format", "childcount": "0" },
                            { "id": "3", "name": "Date Range", "controller": "DatePicker", "action": "DateRange", "childcount": "0" },
                            { "id": "4", "name": "Display Inline", "controller": "DatePicker", "action": "DisplayInline", "childcount": "0" },
                            { "id": "5", "name": "Dates In Other Month", "controller": "DatePicker", "action": "DatesInOtherMonth", "childcount": "0" },
                            { "id": "6", "name": "Localization", "controller": "DatePicker", "action": "Localization", "childcount": "0" },
                            { "id": "7", "name": "Methods", "controller": "DatePicker", "action": "Methods", "childcount": "0" },
                            { "id": "8", "name": "Events", "controller": "DatePicker", "action": "Events", "childcount": "0" },
                            { "id": "11", "name": "RTL", "controller": "DatePicker", "action": "Rtl", "childcount": "0" },
                            { "id": "12", "name": "Keyboard Navigation", "controller": "DatePicker", "action": "Keyboard", "childcount": "0" },
                               { "id": "13", "name": "DatePickerFor", "controller": "DatePicker", "action": "Datepickerfor","childcount": "0" }
                        ]
                    },
                    {
                        "name": "TimePicker", "id": "TimePicker", "Group": "EDITORS", "childcount": "1", "controller": "TimePicker", "action": "Default", "samples": [
                            { "id": "1", "name": "Default Functionalities", "controller": "TimePicker", "action": "Default", "childcount": "0" },
                            { "id": "2", "name": "Localization", "controller": "TimePicker", "action": "Localization", "childcount": "0" },
                            { "id": "3", "name": "Methods", "controller": "TimePicker", "action": "Methods", "childcount": "0" },
                            { "id": "4", "name": "Events", "controller": "TimePicker", "action": "Events", "childcount": "0" },
                            { "id": "7", "name": "RTL", "controller": "TimePicker", "action": "Rtl", "childcount": "0" },
                            { "id": "8", "name": "Keyboard Navigation", "controller": "TimePicker", "action": "Keyboard", "childcount": "0" },
                              { "id": "9", "name": "TimePickerFor", "controller": "TimePicker", "action": "TimePickerFor","childcount": "0" }

                        ]
                    },
                    {
                        "name": "DateTimePicker", "id": "DateTimePicker", "Group": "EDITORS", "childcount": "1", "controller": "DateTimePicker", "action": "Default", "samples": [
                           { "id": "1", "name": "Default Functionalities", "controller": "DateTimePicker", "action": "Default", "childcount": "0" },
                           { "id": "2", "name": "Localization", "controller": "DateTimePicker", "action": "Localization", "childcount": "0" },
                           { "id": "3", "name": "Methods", "controller": "DateTimePicker", "action": "Methods", "childcount": "0" },
                           { "id": "4", "name": "Events", "controller": "DateTimePicker", "action": "Events", "childcount": "0" },
                           { "id": "7", "name": "RTL", "controller": "DateTimePicker", "action": "Rtl", "childcount": "0" },
                           { "id": "8", "name": "Keyboard Navigation", "controller": "DateTimePicker", "action": "Keyboard", "childcount": "0" },
                            { "id": "9", "name": "DateTimePickerFor", "controller": "DateTimePicker", "action": "Datetimepickerfor","childcount": "0" }

                        ]
                    },
                      {
                          "name": "TextBoxes", "id": "Editor", "Group": "EDITORS", "childcount": "1", "controller": "Editor", "action": "Default", "samples": [
                             { "id": "1", "name": "Default Functionalities", "controller": "Editor", "action": "Default", "childcount": "0" },
                             { "id": "2", "name": "Localization", "controller": "Editor", "action": "Localization", "childcount": "0" },
                             { "id": "3", "name": "Methods", "controller": "Editor", "action": "Methods", "childcount": "0" },
                             { "id": "4", "name": "Events", "controller": "Editor", "action": "Events", "childcount": "0" },
                             { "id": "7", "name": "RTL", "controller": "Editor", "action": "Rtl", "childcount": "0" },
                             { "id": "8", "name": "Keyboard Navigation", "controller": "Editor", "action": "Keyboard", "childcount": "0" },
                          { "id": "9", "name": "EditorFor", "controller": "Editor", "action": "EditorFor","childcount": "0" }
                          ]
                      },
                    {
                        "name": "Autocomplete", "id": "Autocomplete", "Group": "EDITORS", "childcount": "1", "controller": "Autocomplete", "action": "Default", "samples": [
                        { "id": "1", "name": "Default Functionalities", "controller": "Autocomplete", "action": "Default", "childcount": "0" },
                        { "id": "2", "name": "Multiple Values", "controller": "Autocomplete", "action": "MultipleValues", "childcount": "0" },
                        { "id": "3", "name": "Grouping", "controller": "Autocomplete", "action": "Grouping", "childcount": "0" },
                        { "id": "4", "name": "Template", "controller": "Autocomplete", "action": "Template", "childcount": "0" },
                        { "id": "5", "name": "Local Data", "controller": "Autocomplete", "action": "DatabindingJson", "childcount": "0" },
                         { "id": "6", "name": "Remote Data", "controller": "Autocomplete", "action": "Databindingremote", "childcount": "0" },
                        { "id": "7", "name": "Auto Fill", "controller": "Autocomplete", "action": "Autofill", "childcount": "0" },
                        { "id": "8", "name": "Methods", "controller": "Autocomplete", "action": "Methods", "childcount": "0" },
                        { "id": "9", "name": "Events", "controller": "Autocomplete", "action": "Events", "childcount": "0" },
                        { "id": "12", "name": "RTL", "controller": "Autocomplete", "action": "Rtl", "childcount": "0" },
                        { "id": "13", "name": "Keyboard Navigation", "controller": "Autocomplete", "action": "Keyboard", "childcount": "0" },
                         { "id": "14", "name": "AutocompleteFor", "controller": "Autocomplete", "action": "AutoCompleteFor","childcount": "0" }
                        ]
                    },
                    {
                        "name": "Rotator", "id": "Rotator", "Group": "NAVIGATION", "childcount": "1", "controller": "Rotator", "action": "Default", "samples": [
                            { "id": "1", "name": "Default Functionalities", "controller": "Rotator", "action": "Default", "childcount": "0" },
                            { "id": "2", "name": "Image With Content", "controller": "Rotator", "action": "ImgWithContent", "childcount": "0" },
                            { "id": "3", "name": "Thumbnail", "controller": "Rotator", "action": "Thumbnail", "childcount": "0" },
                            { "id": "4", "name": "Local Data", "controller": "Rotator", "action": "LocalData", "childcount": "0" },
							{ "id": "5", "name": "Methods", "controller": "Rotator", "action": "Properties", "childcount": "0" },
							{ "id": "6", "name": "Events", "controller": "Rotator", "action": "Events", "childcount": "0" },
							{ "id": "9", "name": "Keyboard Navigation", "controller": "Rotator", "action": "Keyboard", "childcount": "0" }

                        ]
                    },
                    {
                        "name": "Tab", "id": "Tab", "childcount": "1", "Group": "NAVIGATION", "controller": "Tab", "action": "Default", "samples": [
                            { "id": "1", "name": "Default Functionalities", "controller": "Tab", "action": "Default", "childcount": "0" },
                            { "id": "2", "name": "Ajax Content", "controller": "Tab", "action": "AjaxContent", "childcount": "0" },
                            { "id": "3", "name": "Images", "controller": "Tab", "action": "Images", "childcount": "0" },
                            { "id": "4", "name": "Direction", "controller": "Tab", "action": "Direction", "childcount": "0" },
                            { "id": "5", "name": "Close Button", "controller": "Tab", "action": "CloseButton", "childcount": "0" },
                            { "id": "6", "name": "Other Widgets", "controller": "Tab", "action": "OtherWidgets", "childcount": "0" },
                            { "id": "7", "name": "State Maintenance", "controller": "Tab", "action": "StateMaintenance", "childcount": "0" },
                            { "id": "8", "name": "Scrollable Tab", "controller": "Tab", "action": "TabScroll", "childcount": "0" },
                            { "id": "9", "name": "Methods", "controller": "Tab", "action": "Methods", "childcount": "0" },
                            { "id": "10", "name": "Events", "controller": "Tab", "action": "Events", "childcount": "0" },
                            { "id": "11", "name": "RTL", "controller": "Tab", "action": "Rtl", "childcount": "0" },
                            { "id": "12", "name": "Keyboard Navigation", "controller": "Tab", "action": "KeyboardNavigation", "childcount": "0" }
                        ]
                    },
                    {
                        "name": "Menu", "id": "Menu", "Group": "NAVIGATION", "child count": "1", "controller": "Menu", "action": "Default", "samples": [
                        { "id": "1", "name": "Default Functionalities", "controller": "Menu", "action": "Default", "childcount": "0" },
                        { "id": "2", "name": "Local Data", "controller": "Menu", "action": "DatabindingJson", "childcount": "0" },
                        { "id": "3", "name": "Remote Data", "controller": "Menu", "action": "Databindingremote", "childcount": "0" },
                        { "id": "4", "name": "Templates", "controller": "Menu", "action": "Template", "childcount": "0" },
                        { "id": "5", "name": "Open Direction", "controller": "Menu", "action": "OpenDirection", "childcount": "0" },
                        { "id": "6", "name": "Vertical Menu", "controller": "Menu", "action": "Vertical", "childcount": "0" },
                        { "id": "7", "name": "Context Menu", "controller": "Menu", "action": "ContextMenu", "childcount": "0" },
                        { "id": "8", "name": "Center Menu", "controller": "Menu", "action": "CenterMenu", "childcount": "0" },
                        { "id": "9", "name": "Methods", "controller": "Menu", "action": "Methods", "childcount": "0" },
                        { "id": "10", "name": "Events", "controller": "Menu", "action": "events", "childcount": "0" },
                        { "id": "13", "name": "RTL", "controller": "Menu", "action": "Rtl", "childcount": "0" },
                        { "id": "14", "name": "Keyboard Navigation", "controller": "Menu", "action": "keyboard", "childcount": "0" }
                        ]
                    },
                         {
                             "name": "Barcode", "id": "Barcode","Group": "DATA VISUALIZATION", "childcount": "1", "controller": "Barcode", "action": "Default", "samples": [
                              { "id": "1", "name": "QR Barcode", "controller": "Barcode", "action": "Default", "childcount": "0" },
                              { "id": "2", "name": "DataMatrix", "controller": "Barcode", "action": "Datamatrix", "childcount": "0" },
                              { "id": "3", "name": "Code 39", "controller": "Barcode", "action": "Code39", "childcount": "0" },
                              { "id": "4", "name": "Code 39 Extended", "controller": "Barcode", "action": "Code39Extended", "childcount": "0" },
                              { "id": "5", "name": "Code 11", "controller": "Barcode", "action": "Code11", "childcount": "0" },
                              { "id": "6", "name": "Codabar", "controller": "Barcode", "action": "Codabar", "childcount": "0" },
                              { "id": "7", "name": "Code 32", "controller": "Barcode", "action": "Code32", "childcount": "0" },
                              { "id": "8", "name": "Code 93", "controller": "Barcode", "action": "Code93", "childcount": "0" },
                              { "id": "9", "name": "Code 93 Extended", "controller": "Barcode", "action": "Code93Extended", "childcount": "0" },
                              { "id": "10", "name": "Code 128 A", "controller": "Barcode", "action": "Code128A", "childcount": "0" },
                              { "id": "11", "name": "Code 128 B", "controller": "Barcode", "action": "Code128B", "childcount": "0" },
                              { "id": "12", "name": "Code 128 C", "controller": "Barcode", "action": "Code128C", "childcount": "0" }
                             ]
                         },
                  {
                      "name": "Accordion", "id": "Accordion", "Group": "NAVIGATION", "childcount": "1", "controller": "Accordion", "action": "Default", "samples": [
                       { "id": "1", "name": "Default Functionalities", "controller": "Accordion", "action": "Default", "childcount": "0" },
                       //{ "id": "2", "name": "Ajax Content", "controller": "Accordion", "action": "AjaxContent", "childcount": "0" },
                       { "id": "3", "name": "Multiple Open", "controller": "Accordion", "action": "MultipleOpen", "childcount": "0" },
                       { "id": "4", "name": "Open On Hover", "controller": "Accordion", "action": "OpenOnHover", "childcount": "0" },
                       { "id": "5", "name": "Icons", "controller": "Accordion", "action": "Icons", "childcount": "0" },
                       { "id": "6", "name": "NestedAccordion", "controller": "Accordion", "action": "NestedAccordion", "childcount": "0" },
                       { "id": "7", "name": "Methods", "controller": "Accordion", "action": "Methods", "childcount": "0" },
                       { "id": "8", "name": "Events", "controller": "Accordion", "action": "Events", "childcount": "0" },
                       { "id": "9", "name": "RTL", "controller": "Accordion", "action": "Rtl", "childcount": "0" },
                       { "id": "10", "name": "Keyboard Navigation", "controller": "Accordion", "action": "KeyboardNavigation", "childcount": "0" }
                      ]
                  },
                 {
                     "name": "ProgressBar", "id": "ProgressBar","Group": "NOTIFICATION", "childcount": "1", "controller": "ProgressBar", "action": "Default", "samples": [
                     { "id": "1", "name": "Default Functionalities", "controller": "ProgressBar", "action": "Default", "childcount": "0" },
                     { "id": "2", "name": "Events", "controller": "ProgressBar", "action": "Events", "childcount": "0" },
                     { "id": "3", "name": "Methods", "controller": "ProgressBar", "action": "Methods", "childcount": "0" },
                     { "id": "4", "name": "RTL", "controller": "ProgressBar", "action": "Rtl", "childcount": "0" }
                     ]
                 },
                 {
                     "name": "Radial Slider", "id": "RadialSlider","type": "preview", "Group": "EDITORS", "childcount": "1", "controller": "RadialSlider", "action": "Default", "samples": [
                    { "id": "1", "name": "Default Functionalities", "controller": "RadialSlider", "action": "Default", "childcount": "0" },
                    { "id": "2", "name": "API's", "controller": "RadialSlider", "action": "API", "childcount": "0" }
                     ]
                 },
                {
                    "name": "Rating", "id": "Rating","Group": "EDITORS", "childcount": "1", "controller": "Rating", "action": "Default", "samples": [
                    { "id": "1", "name": "Default Functionalities", "controller": "Rating", "action": "Default", "childcount": "0" },
                    { "id": "2", "name": "Precision", "controller": "Rating", "action": "Precision", "childcount": "0" },
                    { "id": "3", "name": "Orientation", "controller": "Rating", "action": "Orientation", "childcount": "0" },
                    { "id": "4", "name": "Methods", "controller": "Rating", "action": "Methods", "childcount": "0" },
                    { "id": "5", "name": "Events", "controller": "Rating", "action": "Events", "childcount": "0" }

                    ]
                },
                {
                    "name": "Dropdownlist", "id": "Dropdownlist","Group": "EDITORS", "childcount": "1","type": "update" , "controller": "Dropdownlist", "action": "Default", "samples": [
                    { "id": "1", "name": "Default Functionalities", "controller": "Dropdownlist", "action": "Default", "childcount": "0" },
                    { "id": "2", "name": "Checkbox", "controller": "Dropdownlist", "action": "Checkbox", "childcount": "0" },
                    { "id": "3", "name": "Icons", "controller": "Dropdownlist", "action": "Icons", "childcount": "0" },
                    { "id": "4", "name": "Cascading", "controller": "Dropdownlist", "action": "Cascading", "childcount": "0" },
                    { "id": "5", "name": "Grouping", "controller": "Dropdownlist", "action": "Grouping", "childcount": "0" },
                    { "id": "6", "name": "Local Data", "controller": "Dropdownlist", "action": "DatabindingLocal", "childcount": "0" },
                    { "id": "7", "name": "Remote Data", "controller": "Dropdownlist", "action": "DatabindingRemote", "childcount": "0" },
                    { "id": "8", "name": "Multi-Select", "controller": "Dropdownlist", "action": "Multiselect", "childcount": "0" },
                    { "id": "9", "name": "Template", "controller": "Dropdownlist", "action": "Template", "childcount": "0" },
                        { "id": "12", "name": "Methods", "controller": "Dropdownlist", "action": "Methods", "childcount": "0", "type" :"update"},
                    { "id": "13", "name": "Events", "controller": "Dropdownlist", "action": "Events", "childcount": "0" },
                        { "id": "14", "name": "RTL", "controller": "Dropdownlist", "action": "Rtl", "childcount": "0" },
                    { "id": "15", "name": "Keyboard Navigation", "controller": "Dropdownlist", "action": "Keyboard", "childcount": "0" },
                     { "id": "16", "name": "DropDownListFor", "controller": "Dropdownlist", "action": "DropdownlistFor", "childcount": "0" },
                    { "id": "17", "name": "IntegrationwithWidgets", "controller": "Dropdownlist", "action": "IntegrationwithWidgets", "childcount": "0" },
                    { "id": "18", "name": "VirtualScrolling", "controller": "Dropdownlist", "action": "VirtualScrolling", "childcount": "0", "type": "new" }
                    ]
                },
                {
                    "name": "ListBox", "id": "ListBox", "childcount": "1","Group": "NAVIGATION", "controller": "ListBox", "action": "Default", "samples": [
                    { "id": "1", "name": "Default Functionalities", "controller": "ListBox", "action": "Default", "childcount": "0" },
                    { "id": "2", "name": "Checkbox", "controller": "ListBox", "action": "Checkbox", "childcount": "0" },
                    { "id": "3", "name": "Icons", "controller": "ListBox", "action": "Icons", "childcount": "0" },
                    { "id": "4", "name": "Cascading", "controller": "ListBox", "action": "Cascading", "childcount": "0" },
                    { "id": "5", "name": "Grouping", "controller": "ListBox", "action": "Grouping", "childcount": "0" },
                    { "id": "6", "name": "Local Data", "controller": "ListBox", "action": "LocalData", "childcount": "0" },
                    { "id": "7", "name": "Remote Data", "controller": "ListBox", "action": "RemoteData", "childcount": "0" },
                    { "id": "8", "name": "Multi-Selection", "controller": "ListBox", "action": "MultiSelect", "childcount": "0" },
                    { "id": "9", "name": "Template", "controller": "ListBox", "action": "Template", "childcount": "0" },
                    { "id": "10", "name": "Drag and Drop", "controller": "ListBox", "action": "DragAndDrop", "childcount": "0" },
                    { "id": "11", "name": "Load On Demand", "controller": "ListBox", "action": "LoadOnDemand", "childcount": "0" },
                    { "id": "12", "name": "Reordering", "controller": "ListBox", "action": "Reordering", "childcount": "0" },
                    //{ "id": "13", "name": "Tool Tip", "controller": "ListBox", "action": "Tooltip", "childcount": "0" },
                    //{ "id": "14", "name": "Virtual Scrolling", "controller": "ListBox", "action": "VirtualScrolling", "childcount": "0" },
                    { "id": "15", "name": "Methods", "controller": "ListBox", "action": "Methods", "childcount": "0" },
                    { "id": "16", "name": "Events", "controller": "ListBox", "action": "Events", "childcount": "0" },
                    { "id": "17", "name": "RTL", "controller": "ListBox", "action": "RTL", "childcount": "0" },
                    { "id": "18", "name": "Keyboard Navigation", "controller": "ListBox", "action": "KeyboardInteraction", "childcount": "0" }



                    ]
                },
                {
                    "name": "Slider", "id": "Slider", "childcount": "1", "Group": "EDITORS", "controller": "Slider", "action": "Default", "samples": [
                    { "id": "1", "name": "Default Functionalities", "controller": "Slider", "action": "Default", "childcount": "0" },
                    { "id": "5", "name": "Range Slider", "controller": "Slider", "action": "RangeSlider", "childcount": "0" },
                    { "id": "6", "name": "Vertical Slider", "controller": "Slider", "action": "VerticalSlider", "childcount": "0" },
                    { "id": "3", "name": "Methods", "controller": "Slider", "action": "Methods", "childcount": "0" },
                    { "id": "2", "name": "Events", "controller": "Slider", "action": "Events", "childcount": "0" },
                    { "id": "9", "name": "RTL", "controller": "Slider", "action": "Rtl", "childcount": "0" },
                    { "id": "4", "name": "Keyboard Navigation", "controller": "Slider", "action": "Keyboard", "childcount": "0" }
                    ]
                },
                                {
                                    "name": "Splitter", "id": "Splitter", "Group": "LAYOUT", "childcount": "1", "controller": "Splitter", "action": "Default", "samples": [
                                     { "id": "1", "name": "Default Functionalities", "controller": "Splitter", "action": "Default", "childcount": "0" },
                                     { "id": "2", "name": "Orientation", "controller": "Splitter", "action": "Orientation", "childcount": "0" },
                                     { "id": "3", "name": "Nested Splitter", "controller": "Splitter", "action": "NestedSplitter", "childcount": "0" },
                                     { "id": "4", "name": "Integration", "controller": "Splitter", "action": "Integaration", "childcount": "0" },
                                     { "id": "5", "name": "Methods", "controller": "Splitter", "action": "Methods", "childcount": "0" },
                                     { "id": "6", "name": "Events", "controller": "Splitter", "action": "Events", "childcount": "0" },
                                     { "id": "7", "name": "RTL", "controller": "Splitter", "action": "Rtl", "childcount": "0" },
                                     { "id": "8", "name": "Keyboard Navigation", "controller": "Splitter", "action": "Keyboard", "childcount": "0" }
                                    ]
                                },
                {
                    "name": "TagCloud", "id": "TagCloud", "childcount": "1", "Group": "DATA VISUALIZATION", "controller": "TagCloud", "action": "Default", "samples": [
                      { "id": "1", "name": "Default Functionalities", "controller": "TagCloud", "action": "Default", "childcount": "0" },
                      { "id": "3", "name": "Remote Data", "controller": "TagCloud", "action": "DatabindingOdata", "childcount": "0" },
                      { "id": "2", "name": "Events", "controller": "TagCloud", "action": "Events", "childcount": "0" },
					  { "id": "6", "name": "RTL", "controller": "TagCloud", "action": "Rtl", "childcount":"0"}
                    ]
                },
                {
                    "name": "Button", "id": "Button", "childcount": "1", "Group": "EDITORS", "controller": "Button", "action": "Default", "samples": [
                    { "id": "1", "name": "Default Functionalities", "controller": "Button", "action": "Default", "childcount": "0" },
                    { "id": "2", "name": "Toggle Buttons", "controller": "Button", "action": "ToggleButtons", "childcount": "0" },
                    { "id": "3", "name": "Split Buttons", "controller": "Button", "action": "SplitButtons", "childcount": "0" },
                    { "id": "4", "name": "Repeat Button", "controller": "Button", "action": "RepeatButton", "childcount": "0" },
                    { "id": "5", "name": "Check Boxes", "controller": "Button", "action": "Checkboxes", "childcount": "0" },
                    { "id": "6", "name": "Radio Buttons", "controller": "Button", "action": "RadioButton", "childcount": "0" },
                    { "id": "7", "name": "Methods", "controller": "Button", "action": "Methods", "childcount": "0" },
                    { "id": "8", "name": "Events", "controller": "Button", "action": "Events", "childcount": "0" },
                    { "id": "9", "name": "RTL", "controller": "Button", "action": "Rtl", "childcount": "0" }
                    ]
                },
                {
                    "name": "Toolbar", "id": "Toolbar", "childcount": "1", "Group": "NAVIGATION", "controller": "Toolbar", "action": "Default", "samples": [
                    { "id": "1", "name": "Default Functionalities", "controller": "Toolbar", "action": "Default", "childcount": "0" },
                    { "id": "2", "name": "Local Data", "controller": "Toolbar", "action": "LocalDataBinding", "childcount": "0" },
                    { "id": "3", "name": "Remote Data", "controller": "Toolbar", "action": "Databindingremote", "childcount": "0" },
                    { "id": "4", "name": "Orientation", "controller": "Toolbar", "action": "Orientation", "childcount": "0" },
                    { "id": "5", "name": "Template", "controller": "Toolbar", "action": "Template", "childcount": "0" },
                    { "id": "8", "name": "Methods", "controller": "Toolbar", "action": "Methods", "childcount": "0" },
                    { "id": "9", "name": "Events", "controller": "Toolbar", "action": "Events", "childcount": "0" },
                    { "id": "10", "name": "RTL", "controller": "Toolbar", "action": "Rtl", "childcount": "0" },
                    { "id": "11", "name": "Keyboard Navigation", "controller": "Toolbar", "action": "Keyboard", "childcount": "0" }
                    ]
                },
               {
                   "name": "WaitingPopup", "id": "WaitingPopup", "Group": "NOTIFICATION", "childcount": "1", "controller": "WaitingPopup", "action": "Default", "samples": [
                      { "id": "1", "name": "Default Functionalities", "controller": "WaitingPopup", "action": "Default", "childcount": "0" },
                      { "id": "2", "name": "Template", "controller": "WaitingPopup", "action": "Template", "childcount": "0" }
                   ]
               },
			   {
			       "name": "Dialog", "id": "Dialog", "Group": "LAYOUT", "childcount": "1", "controller": "Dialog", "action": "Default", "samples": [
                         { "id": "1", "name": "Default Functionalities", "controller": "Dialog", "action": "Default", "childcount": "0" },
                   //{ "id": "2", "name": "Ajax Content", "controller": "Dialog", "action": "AjaxContent", "childcount": "0" },
                         { "id": "3", "name": "Multiple Dialog", "controller": "Dialog", "action": "MultipleDialog", "childcount": "0" },
                         { "id": "4", "name": "Custom Action", "controller": "Dialog", "action": "Icons", "childcount": "0" },
                         { "id": "5", "name": "Modal Dialog", "controller": "Dialog", "action": "ModelDialog", "childcount": "0" },
                         { "id": "6", "name": "Methods", "controller": "Dialog", "action": "Methods", "childcount": "0" },
                         { "id": "7", "name": "Events", "controller": "Dialog", "action": "Events", "childcount": "0" },
                         { "id": "8", "name": "RTL", "controller": "Dialog", "action": "Rtl", "childcount": "0" },
                         { "id": "9", "name": "Keyboard Navigation", "controller": "Dialog", "action": "KeyboardNavigation", "childcount": "0" }
			       ]
			   },
                {
                    "name": "Upload", "id": "Upload", "Group": "EDITORS", "childcount": "1", "controller": "Upload" , "action": "Default", "samples": [
                     { "id": "1", "name": "Default Functionalities", "controller": "Upload", "action": "Default", "childcount": "0" },
                     { "id": "2", "name": "File Input", "controller": "Upload", "action": "FileTypesInput", "childcount": "0" },
                      { "id": "3", "name": "Drag And Drop", "controller": "Upload", "action": "DragAndDrop","childcount": "0" },
                      { "id": "7", "name": "Synchronous Upload", "controller": "Upload", "action": "synchronousUpload","childcount": "0" },
                     { "id": "4", "name": "Methods", "controller": "Upload", "action": "Methods", "childcount": "0" },
                     { "id": "5", "name": "Events", "controller": "Upload", "action": "Events", "childcount": "0" } 
                     //{ "id": "6", "name": "Rtl", "controller": "Upload", "action": "Rtl", "childcount": "0" }
                    ]
                },
                {
                    "name": "ScrollBar", "id": "ScrollBar", "Group": "NAVIGATION", "childcount": "1", "controller": "ScrollBar", "action": "Default", "samples": [
                    { "id": "1", "name": "Default Functionalities", "controller": "ScrollBar", "action": "Default", "childcount": "0" },
                    { "id": "2", "name": "Virtual Scrolling", "controller": "ScrollBar", "action": "VirtualScrolling", "childcount": "0"}
                    ]
                },
                {
                    "name": "Captcha", "id": "Captcha", "Group": "EDITORS", "childcount": "1", "controller": "Captcha", "action": "Default", "samples": [
                    { "id": "1", "name": "Default Functionalities", "controller": "Captcha", "action": "Default", "childcount": "0" },
                    { "id": "2", "name": "Core Features", "controller": "Captcha", "action": "CoreFeatures", "childcount": "0" },
                    { "id": "3", "name": "Sign Up Form", "controller": "Captcha", "action": "SignUpForm", "childcount": "0" },
                    { "id": "4", "name": "RTL", "controller": "Captcha", "action": "RTL", "childcount": "0" }

                    ]
                },
                {
                    "name": "ListView", "id": "ListView", "Group": "LAYOUT", "childcount": "1", "controller": "ListView", "action": "Default", "samples": [
                    { "id": "1", "name": "Default Functionalities", "controller": "ListView", "action": "Default", "childcount": "0" },
                    { "id": "2", "name": "CheckList", "controller": "ListView", "action": "CheckList", "childcount": "0" },
                    { "id": "3", "name": "Local Data", "controller": "ListView", "action": "DataBindingLocal", "childcount": "0" }

                    ]
                },
                {
                    "name": "NavigationDrawer", "id": "NavigationDrawer","Group": "NAVIGATION", "childcount": "1", "controller": "NavigationDrawer", "action": "Default", "samples": [
                    { "id": "1", "name": "Default", "controller": "NavigationDrawer", "action": "Default", "childcount": "0" }
                    ]
                },

                {
                    "name": "Tile View", "id": "TileView", "Group": "LAYOUT", "childcount": "1", "controller": "TileView", "action": "Default", "samples": [
                    { "id": "1", "name": "Default", "controller": "TileView", "action": "Default", "childcount": "0" }
                    ]
                },

				 {
				     "name": "AngularJS", "id": "angularsupport", "Group": "INTEGRATION", "childcount": "1", "controller": "AngularSupport", "action": "AutoComplete", "samples": [
                    { "id": "1", "name": "AutoComplete", "controller": "AngularSupport", "action": "AutoComplete", "childcount": "0" },
					{ "id": "2", "name": "BulletGraph", "controller": "AngularSupport", "action": "BulletGraph", "childcount": "0" },
					{ "id": "3", "name": "Chart", "controller": "AngularSupport", "action": "Chart", "childcount": "0" },
				    { "id": "4", "name": "CircularGauge", "controller": "AngularSupport", "action": "CircularGauge", "childcount": "0" },
                    { "id": "5", "name": "ColorPicker", "controller": "AngularSupport", "action": "ColorPicker", "childcount": "0" },
                    { "id": "6", "name": "DatePicker", "controller": "AngularSupport", "action": "DatePicker", "childcount": "0" },
                    { "id": "7", "name": "DateTimePicker", "controller": "AngularSupport", "action": "DateTimePicker", "childcount": "0" },
				    { "id": "8", "name": "Diagram", "controller": "AngularSupport", "action": "Diagram", "childcount": "0" },
					{ "id": "9", "name": "DigitalGauge", "controller": "AngularSupport", "action": "DigitalGauge", "childcount": "0" },
                    { "id": "10", "name": "DropDownList", "controller": "AngularSupport", "action": "DropDownList", "childcount": "0" },
					{ "id": "11", "name": "Gantt", "controller": "AngularSupport", "action": "Gantt", "childcount": "0" },
					{ "id": "12", "name": "Grid", "controller": "AngularSupport", "action": "Grid", "childcount": "0" },
					{ "id": "13", "name": "LinearGauge", "controller": "AngularSupport", "action": "LinearGauge", "childcount": "0" },
                    { "id": "14", "name": "ListBox", "controller": "AngularSupport", "action": "ListBox", "childcount": "0" },
                    { "id": "15", "name": "ListView", "controller": "AngularSupport", "action": "ListView", "childcount": "0" },
					{ "id": "16", "name": "Maps", "controller": "AngularSupport", "action": "Maps", "childcount": "0" },
                    { "id": "17", "name": "Menu", "controller": "AngularSupport", "action": "Menu", "childcount": "0" },
					{ "id": "18", "name": "OlapChart", "controller": "AngularSupport", "action": "OlapChart", "childcount": "0" },
					{ "id": "19", "name": "OlapClient", "controller": "AngularSupport", "action": "OlapClient", "childcount": "0" },
					{ "id": "20", "name": "OlapGauge", "controller": "AngularSupport", "action": "OlapGauge", "childcount": "0" },
					{ "id": "21", "name": "PivotGrid", "controller": "AngularSupport", "action": "PivotGrid", "childcount": "0" },
                    { "id": "22", "name": "RangeNavigator", "controller": "AngularSupport", "action": "RangeNavigator", "childcount": "0" },
                    { "id": "23", "name": "Rating", "controller": "AngularSupport", "action": "Rating", "childcount": "0" },
					{ "id": "24", "name": "ReportViewer", "controller": "AngularSupport", "action": "ReportViewer", "childcount": "0" },
					{ "id": "25", "name": "Ribbon", "controller": "AngularSupport", "action": "Ribbon", "childcount": "0" },
                    { "id": "26", "name": "RichTextEditor", "controller": "AngularSupport", "action": "RichTextEditor", "childcount": "0" },
                    { "id": "27", "name": "Rotator", "controller": "AngularSupport", "action": "Rotator", "childcount": "0" },
					{ "id": "28", "name": "Schedule", "controller": "AngularSupport", "action": "Schedule", "childcount": "0" },
                    { "id": "29", "name": "Slider", "controller": "AngularSupport", "action": "Slider", "childcount": "0" },
                    { "id": "30", "name": "TagCloud", "controller": "AngularSupport", "action": "TagCloud", "childcount": "0" },
                    { "id": "31", "name": "Editor", "controller": "AngularSupport", "action": "Editor", "childcount": "0" },
                    { "id": "32", "name": "TimePicker", "controller": "AngularSupport", "action": "TimePicker", "childcount": "0" },
                    { "id": "33", "name": "Toolbar", "controller": "AngularSupport", "action": "Toolbar", "childcount": "0" },
					{ "id": "34", "name": "TreeGrid", "controller": "AngularSupport", "action": "TreeGrid", "childcount": "0" },
					{ "id": "35", "name": "TreeMap", "controller": "AngularSupport", "action": "TreeMap", "childcount": "0" },
                    { "id": "36", "name": "TreeView", "controller": "AngularSupport", "action": "TreeView", "childcount": "0" }
                    ]
                },
                {
                    "name": "KnockoutJS", "id": "kosupport", "Group": "INTEGRATION", "childcount": "1", "controller": "KOSupport", "action": "AutoComplete", "samples": [
                    { "id": "1", "name": "AutoComplete", "controller": "KOSupport", "action": "AutoComplete", "childcount": "0" },
					{ "id": "2", "name": "BulletGraph", "controller": "KOSupport", "action": "BulletGraph", "childcount": "0" },
					{ "id": "3", "name": "Chart", "controller": "KOSupport", "action": "Chart", "childcount": "0" },
				    { "id": "4", "name": "CircularGauge", "controller": "KOSupport", "action": "CircularGauge", "childcount": "0" },
                    { "id": "5", "name": "ColorPicker", "controller": "KOSupport", "action": "ColorPicker", "childcount": "0" },
                    { "id": "6", "name": "DatePicker", "controller": "KOSupport", "action": "DatePicker", "childcount": "0" },
                    { "id": "7", "name": "DateTimePicker", "controller": "KOSupport", "action": "DateTimePicker", "childcount": "0" },
				    { "id": "8", "name": "Diagram", "controller": "KOSupport", "action": "Diagram", "childcount": "0" },
					{ "id": "9", "name": "DigitalGauge", "controller": "KOSupport", "action": "DigitalGauge", "childcount": "0" },
                    { "id": "10", "name": "DropDownList", "controller": "KOSupport", "action": "DropDownList", "childcount": "0" },
					{ "id": "11", "name": "Gantt", "controller": "KOSupport", "action": "Gantt", "childcount": "0" },
					{ "id": "12", "name": "Grid", "controller": "KOSupport", "action": "Grid", "childcount": "0" },
					{ "id": "13", "name": "LinearGauge", "controller": "KOSupport", "action": "LinearGauge", "childcount": "0" },
                    { "id": "14", "name": "ListBox", "controller": "KOSupport", "action": "ListBox", "childcount": "0" },
                    { "id": "34", "name": "ListView", "controller": "KOSupport", "action": "ListView", "childcount": "0" },
                    { "id": "15", "name": "Menu", "controller": "KOSupport", "action": "Menu", "childcount": "0" },
					{ "id": "16", "name": "OlapChart", "controller": "KOSupport", "action": "OlapChart", "childcount": "0" },
					{ "id": "17", "name": "OlapClient", "controller": "KOSupport", "action": "OlapClient", "childcount": "0" },
					{ "id": "18", "name": "OlapGauge", "controller": "KOSupport", "action": "OlapGauge", "childcount": "0" },
					{ "id": "19", "name": "PivotGrid", "controller": "KOSupport", "action": "PivotGrid", "childcount": "0" },
                    { "id": "20", "name": "RangeNavigator", "controller": "KOSupport", "action": "RangeNavigator", "childcount": "0" },
                    { "id": "21", "name": "Rating", "controller": "KOSupport", "action": "Rating", "childcount": "0" },
					{ "id": "22", "name": "Ribbon", "controller": "KOSupport", "action": "Ribbon", "childcount": "0", "type": "new" },
					{ "id": "23", "name": "ReportViewer", "controller": "KOSupport", "action": "ReportViewer", "childcount": "0" },
                    { "id": "24", "name": "RichTextEditor", "controller": "KOSupport", "action": "RichTextEditor", "childcount": "0" },
                    { "id": "25", "name": "Rotator", "controller": "KOSupport", "action": "Rotator", "childcount": "0" },
					{ "id": "26", "name": "Schedule", "controller": "KOSupport", "action": "Schedule", "childcount": "0" },
                    { "id": "27", "name": "Slider", "controller": "KOSupport", "action": "Slider", "childcount": "0" },
                    { "id": "28", "name": "TagCloud", "controller": "KOSupport", "action": "TagCloud", "childcount": "0" },
                    { "id": "29", "name": "Editor", "controller": "KOSupport", "action": "Editor", "childcount": "0" },
                    { "id": "30", "name": "TimePicker", "controller": "KOSupport", "action": "TimePicker", "childcount": "0" },
                    { "id": "31", "name": "Toolbar", "controller": "KOSupport", "action": "Toolbar", "childcount": "0" },
					{ "id": "32", "name": "TreeGrid", "controller": "KOSupport", "action": "TreeGrid", "childcount": "0" },
                    { "id": "33", "name": "TreeView", "controller": "KOSupport", "action": "TreeView", "childcount": "0" }
                    ]
                }

];
