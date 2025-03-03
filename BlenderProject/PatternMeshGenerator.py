import bpy
import json
import bmesh
import os
import sys
from mathutils import Vector
from PatternDataStructures import *
class PatternMeshGenerator:
    def __init__(self, pattern_data: PatternDataStructures):
        self.pattern_data = pattern_data
        self.mesh = None
        self.bm = None
        self.panel_meshes = {}
    
    def CreateMesh(self):
        # 새로운 bmesh 객체 생성
        self.bm = bmesh.new()
        
        # 각 패널별로 메시 생성
        for panel_name in self.pattern_data.pattern.panel_order:
            panel = self.pattern_data.pattern.panels[panel_name]
            
            # 패널의 정점들을 추가
            vertices = []
            for vertex in panel.vertices:
                # Vector를 사용하여 3D 좌표 생성
                v = self.bm.verts.new((vertex[0], vertex[1], 0))  # z 좌표는 0으로 설정
                vertices.append(v)
            
            # bmesh 업데이트
            self.bm.verts.ensure_lookup_table()
            
            # 패널의 엣지들을 추가
            edges = []
            for edge in panel.edges:
                # 엣지의 시작점과 끝점 인덱스로 새 엣지 생성
                e = self.bm.edges.new((vertices[edge.endpoints[0]], vertices[edge.endpoints[1]]))
                edges.append(e)
            
            # 패널 정보 저장
            self.panel_meshes[panel_name] = {
                'vertices': vertices,
                'edges': edges,
                'translation': panel.translation,
                'rotation': panel.rotation
            }
        
        # 스티치 정보에 따라 면 생성
        for stitch_group in self.pattern_data.pattern.stitches:
            stitch_verts = []
            for stitch in stitch_group:
                panel = self.pattern_data.pattern.panels[stitch.panel]
                edge = panel.edges[stitch.edge]
                # 스티치의 정점들 추가
                for vertex_idx in edge.endpoints:
                    vertex = self.panel_meshes[stitch.panel]['vertices'][vertex_idx]
                    if vertex not in stitch_verts:
                        stitch_verts.append(vertex)
            
            # 면이 만들어질 수 있는 경우에만 면 생성
            if len(stitch_verts) >= 3:
                self.bm.faces.new(stitch_verts)
        
        # 새로운 메시 데이터 생성
        self.mesh = bpy.data.meshes.new(name="PatternMesh")
        
        # bmesh를 메시 데이터에 적용
        self.bm.to_mesh(self.mesh)
        self.bm.free()
        
        return self.mesh

# 실행 코드
def main():
    # JSON 파일에서 패턴 데이터 로드
    pattern_data = PatternDataStructures.from_json("SamplePatterns/specification_0.json")
    
    # 메시 생성기 인스턴스 생성
    generator = PatternMeshGenerator(pattern_data)
    
    # 메시 생성
    mesh = generator.CreateMesh()
    
    # 새 오브젝트 생성 및 메시 할당
    obj = bpy.data.objects.new("PatternObject", mesh)
    
    # 씬에 오브젝트 추가
    bpy.context.collection.objects.link(obj)

if __name__ == "__main__":
    main()

