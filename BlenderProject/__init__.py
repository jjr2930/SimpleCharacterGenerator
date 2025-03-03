bl_info = {
    "name": "Pattern Mesh Generator",
    "author": "Your Name",
    "version": (1, 0),
    "blender": (2, 80, 0),
    "location": "View3D > Sidebar > Pattern",
    "description": "Generate meshes from pattern data",
    "category": "Object",
}

import bpy
from . import PatternDataStructures
from . import PatternMeshGenerator

def register():
    # 여기에 등록 코드 추가
    pass

def unregister():
    # 여기에 해제 코드 추가
    pass

if __name__ == "__main__":
    register() 