import json
from dataclasses import dataclass, field
from typing import List, Dict, Optional

class PatternDataStructures:
    def __init__(self):
        self.pattern = None
        self.properties = None
        self.parameters = {}
        self.parameter_order = []

    @dataclass
    class Edge:
        endpoints: List[int] = field(default_factory=list)
        curvature: Optional[List[float]] = None

    @dataclass
    class Panel:
        translation: List[float] = field(default_factory=list)
        edges: List['PatternDataStructures.Edge'] = field(default_factory=list)
        rotation: List[float] = field(default_factory=list)
        vertices: List[List[float]] = field(default_factory=list)

    @dataclass
    class Stitch:
        edge: int
        panel: str

    @dataclass
    class Pattern:
        panels: Dict[str, 'PatternDataStructures.Panel'] = field(default_factory=dict)
        stitches: List[List['PatternDataStructures.Stitch']] = field(default_factory=list)
        panel_order: List[str] = field(default_factory=list)

    @dataclass
    class Properties:
        curvature_coords: str
        normalize_panel_translation: bool
        units_in_meter: int
        normalized_edge_loops: bool

    @dataclass
    class EdgeInfluence:
        direction: str
        id: int
        along: Optional[List[float]] = None

    @dataclass
    class Influence:
        panel: str
        edge_list: List['PatternDataStructures.EdgeInfluence'] = field(default_factory=list)

    @dataclass
    class Parameters:
        type: str
        value: float
        influence: List['PatternDataStructures.Influence'] = field(default_factory=list)
        range: List[float] = field(default_factory=list)

    @staticmethod
    def from_json(file_path: str) -> 'PatternDataStructures':
        """JSON 파일을 읽고 PatternDataStructures 객체로 변환"""
        with open(file_path, 'r', encoding='utf-8') as file:
            data = json.load(file)
        
        # Properties 생성
        properties = PatternDataStructures.Properties(**data['properties'])
        
        # Pattern 생성
        pattern_data = data['pattern']
        panels = {}
        for panel_name, panel_data in pattern_data['panels'].items():
            # Edge 객체들 생성
            edges = [PatternDataStructures.Edge(**edge_data) for edge_data in panel_data['edges']]
            panel_data['edges'] = edges
            panels[panel_name] = PatternDataStructures.Panel(**panel_data)
        
        # Stitch 객체들 생성
        stitches = [[PatternDataStructures.Stitch(**stitch_data) for stitch_data in stitch_group] 
                   for stitch_group in pattern_data['stitches']]
        
        pattern = PatternDataStructures.Pattern(
            panels=panels,
            stitches=stitches,
            panel_order=pattern_data['panel_order']
        )
        
        # Parameters 생성
        parameters = {}
        for param_name, param_data in data['parameters'].items():
            influences = []
            for influence_data in param_data['influence']:
                edge_list = [PatternDataStructures.EdgeInfluence(**edge_data) 
                           for edge_data in influence_data['edge_list']]
                influences.append(PatternDataStructures.Influence(
                    panel=influence_data['panel'],
                    edge_list=edge_list
                ))
            param_data['influence'] = influences
            parameters[param_name] = PatternDataStructures.Parameters(**param_data)
        
        instance = PatternDataStructures()
        instance.pattern = pattern
        instance.properties = properties
        instance.parameters = parameters
        instance.parameter_order = data['parameter_order']
        return instance