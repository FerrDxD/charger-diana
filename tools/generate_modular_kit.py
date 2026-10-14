#!/usr/bin/env python3
"""Generate C1 modular kit GLBs - low-poly boxes, corner pivot, PS1 budget"""
import struct, json, pathlib, math

OUT_DIR = pathlib.Path(r"C:\Users\Hype GLK\charger-diana\assets\models\arch")
OUT_DIR.mkdir(parents=True, exist_ok=True)

def box_vertices(min_xyz, max_xyz):
    """Return (positions, normals, uvs, indices) for a box with 24 verts (flat normals)"""
    mx, my, mz = max_xyz
    nx, ny, nz = min_xyz
    # 6 faces, each 4 verts
    faces = [
        # +X right
        ([(mx,ny,nz),(mx,my,nz),(mx,my,mz),(mx,ny,mz)], (1,0,0)),
        # -X left
        ([(nx,ny,mz),(nx,my,mz),(nx,my,nz),(nx,ny,nz)], (-1,0,0)),
        # +Y top
        ([(nx,my,nz),(nx,my,mz),(mx,my,mz),(mx,my,nz)], (0,1,0)),
        # -Y bottom
        ([(nx,ny,mz),(mx,ny,mz),(mx,ny,nz),(nx,ny,nz)], (0,-1,0)),
        # +Z front
        ([(nx,ny,mz),(nx,my,mz),(mx,my,mz),(mx,ny,mz)], (0,0,1)),
        # -Z back
        ([(mx,ny,nz),(mx,my,nz),(nx,my,nz),(nx,ny,nz)], (0,0,-1)),
    ]
    pos, nrm, uv = [], [], []
    idx = []
    base = 0
    for verts, normal in faces:
        for i, v in enumerate(verts):
            pos.extend(v)
            nrm.extend(normal)
            # UV full face 0-1
            u = 1 if i in (2,3) else 0  # rough mapping
            vv = 1 if i in (1,2) else 0
            # flip for some faces to keep winding
            if normal == (1,0,0) or normal == (-1,0,0):
                # map y->v, z->u
                u = (v[2]-nz)/(mz-nz) if mz!=nz else 0
                vv = (v[1]-ny)/(my-ny) if my!=ny else 0
            elif normal == (0,1,0) or normal == (0,-1,0):
                u = (v[0]-nx)/(mx-nx) if mx!=nx else 0
                vv = (v[2]-nz)/(mz-nz) if mz!=nz else 0
            else:
                u = (v[0]-nx)/(mx-nx) if mx!=nx else 0
                vv = (v[1]-ny)/(my-ny) if my!=ny else 0
            uv.extend([u, vv])
        # two triangles per face
        idx.extend([base+0, base+1, base+2, base+0, base+2, base+3])
        base += 4
    return pos, nrm, uv, idx

def build_glb(boxes, color_rgb, filepath):
    """boxes: list of (min,max), color_rgb: (r,g,b) 0-1"""
    all_pos, all_nrm, all_uv = [], [], []
    all_idx = []
    v_offset = 0
    for bmin, bmax in boxes:
        p, n, uv, idx = box_vertices(bmin, bmax)
        all_pos.extend(p)
        all_nrm.extend(n)
        all_uv.extend(uv)
        all_idx.extend([i+v_offset for i in idx])
        v_offset += len(p)//3

    # Pack binary
    pos_bytes = struct.pack('<' + 'f'*len(all_pos), *all_pos)
    nrm_bytes = struct.pack('<' + 'f'*len(all_nrm), *all_nrm)
    uv_bytes = struct.pack('<' + 'f'*len(all_uv), *all_uv)
    # indices as uint16
    idx_bytes = struct.pack('<' + 'H'*len(all_idx), *all_idx)
    # align to 4
    def pad(b):
        r = len(b) % 4
        return b + b'\x00'*(4-r) if r else b
    pos_bytes_p = pad(pos_bytes)
    nrm_bytes_p = pad(nrm_bytes)
    uv_bytes_p = pad(uv_bytes)
    idx_bytes_p = pad(idx_bytes)

    buffer_bytes = pos_bytes_p + nrm_bytes_p + uv_bytes_p + idx_bytes_p
    # bufferViews
    offset = 0
    bv = []
    def add_view(length, target):
        nonlocal offset
        bv.append({"buffer":0, "byteOffset": offset, "byteLength": length, "target": target})
        idx = len(bv)-1
        # advance offset with padded length
        padded = length + (4 - length%4 if length%4 else 0)
        offset += padded
        return idx
    bv_pos = add_view(len(pos_bytes), 34962)
    bv_nrm = add_view(len(nrm_bytes), 34962)
    bv_uv = add_view(len(uv_bytes), 34962)
    bv_idx = add_view(len(idx_bytes), 34963)

    vert_count = len(all_pos)//3
    # compute min/max
    xs = all_pos[0::3]; ys = all_pos[1::3]; zs = all_pos[2::3]
    p_min = [min(xs), min(ys), min(zs)]
    p_max = [max(xs), max(ys), max(zs)]

    accessors = [
        {"bufferView": bv_pos, "componentType": 5126, "count": vert_count, "type":"VEC3", "min": p_min, "max": p_max},
        {"bufferView": bv_nrm, "componentType": 5126, "count": vert_count, "type":"VEC3"},
        {"bufferView": bv_uv, "componentType": 5126, "count": vert_count, "type":"VEC2"},
        {"bufferView": bv_idx, "componentType": 5123, "count": len(all_idx), "type":"SCALAR"},
    ]

    tris = len(all_idx)//3
    # Estimate bytes
    json_dict = {
        "asset": {"version":"2.0", "generator":"charger-diana C1 kit"},
        "scene": 0,
        "scenes": [{"name":"Scene", "nodes":[0]}],
        "nodes": [{"name": filepath.stem, "mesh":0}],
        "meshes": [{"name": filepath.stem, "primitives": [{"attributes":{"POSITION":0,"NORMAL":1,"TEXCOORD_0":2}, "indices":3, "material":0}]}],
        "materials": [{"name":"mat_arch","pbrMetallicRoughness":{"baseColorFactor":[color_rgb[0], color_rgb[1], color_rgb[2], 1.0], "metallicFactor":0.0, "roughnessFactor":1.0}, "doubleSided": False}],
        "accessors": accessors,
        "bufferViews": bv,
        "buffers": [{"byteLength": len(buffer_bytes)}],
    }
    json_bytes = json.dumps(json_dict, separators=(',',':')).encode('utf-8')
    # pad json to 4 with spaces
    json_pad = (4 - len(json_bytes)%4) % 4
    json_bytes_padded = json_bytes + b' ' * json_pad

    # GLB header
    total_len = 12 + 8 + len(json_bytes_padded) + 8 + len(buffer_bytes)
    header = struct.pack('<4sII', b'glTF', 2, total_len)
    chunk_json = struct.pack('<I4s', len(json_bytes_padded), b'JSON') + json_bytes_padded
    chunk_bin = struct.pack('<I4s', len(buffer_bytes), b'BIN\x00') + buffer_bytes
    glb = header + chunk_json + chunk_bin
    filepath.write_bytes(glb)
    return tris, vert_count

# Palet ART_BIBLE
PAL = {
    "wall": (0x5E/255, 0x6E/255, 0x5E/255),
    "wall_top": (0xB8/255, 0xB4/255, 0xA6/255),
    "floor": (0x7A/255, 0x76/255, 0x6C/255),
    "ceiling": (0xA8/255, 0xA4/255, 0x9A/255),
    "door": (0x6E/255, 0x52/255, 0x38/255),
    "pillar": (0x5A/255, 0x5A/255, 0x58/255),
    "gate": (0x23/255, 0x23/255, 0x26/255),
}

# Define kit - each entry: filename, boxes list, color
# Grid 2m, wall thickness 0.2, height 3.2, floor thickness 0.1
W = 0.2
H = 3.2
D = 0.2

kit = [
    # floor 2x2
    ("arch_floor_2m", [((0,0,0),(2,0.1,2))], PAL["floor"]),
    ("arch_ceiling_2m", [((0,0,0),(2,0.1,2))], PAL["ceiling"]),
    ("arch_wall_straight_4m", [((0,0,0),(4,H,W))], PAL["wall"]),
    # door frame: wall 2m wide with 1x2 opening centered (two jambs + lintel)
    ("arch_wall_door_frame_2m", [((0,0,0),(0.5,H,W)), ((1.5,0,0),(2,H,W)), ((0.5,2.0,0),(1.5, H, W))], PAL["wall"]),
    ("arch_wall_window_frame_2m", [((0,0,0),(0.5,H,W)), ((1.5,0,0),(2,H,W)), ((0.5,0,0),(1.5,1.0,W)), ((0.5,2.2,0),(1.5,H,W))], PAL["wall"]),
    ("arch_pillar_02m", [((0,0,0),(0.2,H,0.2))], PAL["pillar"]),
    ("arch_pillar_04m", [((0,0,0),(0.4,H,0.4))], PAL["pillar"]),
    ("arch_door_leaf_2m", [((0,0,0),(1,2.0,0.05))], PAL["door"]),
    ("arch_window_leaf_1m", [((0,0,0),(1,1.2,0.05))], PAL["wall_top"]),
    ("arch_stairs_2m", [((0,0,0),(2,0.2,0.4)), ((0,0.2,0.4),(2,0.4,0.8)), ((0,0.4,0.8),(2,0.6,1.2)), ((0,0.6,1.2),(2,0.8,1.6)), ((0,0.8,1.6),(2,1.0,2.0))], PAL["floor"]),
    ("arch_railing_2m", [((0,0.8,0),(2,1.0,0.05)), ((0,0,0),(0.05,1.0,0.05)), ((1.95,0,0),(2,1.0,0.05)), ((0,0.4,0),(2,0.45,0.02))], PAL["gate"]),
    ("arch_fence_2m", [((0,0,0),(0.05,1.2,0.05)), ((0.5,0,0),(0.55,1.2,0.05)), ((1.0,0,0),(1.05,1.2,0.05)), ((1.5,0,0),(1.55,1.2,0.05)), ((1.95,0,0),(2,1.2,0.05)), ((0,0.9,0),(2,1.0,0.02)), ((0,0.3,0),(2,0.35,0.02))], PAL["pillar"]),
    ("arch_gate_main_4m", [((0,0,0),(0.15,2.2,0.15)), ((3.85,0,0),(4,2.2,0.15)), ((0,1.8,0),(4,2.0,0.06)), ((0.15,0,0),(0.2,2.0,0.04)), ((3.80,0,0),(3.85,2.0,0.04))], PAL["gate"]),
    ("arch_wall_t_2m", [((0,0,0),(2,H,W)), ((1-W/2,0,-2),(1+W/2,H,0))], PAL["wall"]),
    ("arch_wall_cross_2m", [((0,0,1-W/2),(2,H,1+W/2)), ((1-W/2,0,0),(1+W/2,H,2))], PAL["wall"]),
    ("arch_cornice_2m", [((0,3.0,0),(2,3.2,0.25))], PAL["wall_top"]),
    ("arch_baseboard_2m", [((0,0,0),(2,0.15,0.22))], PAL["pillar"]),
    ("arch_wall_half_1m", [((0,0,0),(2,1.6,W))], PAL["wall"]),
    ("arch_wall_solid_1m", [((0,0,0),(1,H,W))], PAL["wall"]),
    ("arch_floor_tile_1m", [((0,0,0),(1,0.1,1))], PAL["floor"]),
]

existing_names = {p.stem for p in OUT_DIR.glob("*.glb")}

for name, boxes, col in kit:
    path = OUT_DIR / f"{name}.glb"
    if path.stem in existing_names:
        print(f"SKIP existing {name}")
        continue
    tris, verts = build_glb(boxes, col, path)
    size = path.stat().st_size
    print(f"OK {name:30s} tris {tris:3d} verts {verts:3d} size {size:5d} boxes {len(boxes)}")

# verify existing tris
import json as _j, struct as _s
for p in sorted(OUT_DIR.glob("*.glb")):
    data = p.read_bytes()
    clen = _s.unpack('<I', data[12:16])[0]
    j = _j.loads(data[20:20+clen].decode('utf-8'))
    acc_idx = j['accessors'][j['meshes'][0]['primitives'][0]['indices']]
    tris = acc_idx['count']//3
    print(f" VERIFY {p.name:30s} tris {tris:3d}")
