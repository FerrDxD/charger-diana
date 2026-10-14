import pathlib, struct, json

OUT = pathlib.Path(r"C:\Users\Hype GLK\charger-diana\assets\models\prolog")
OUT.mkdir(parents=True, exist_ok=True)

def box_vertices(min_xyz, max_xyz):
    mx,my,mz = max_xyz; nx,ny,nz = min_xyz
    faces=[
        ([(mx,ny,nz),(mx,my,nz),(mx,my,mz),(mx,ny,mz)], (1,0,0)),
        ([(nx,ny,mz),(nx,my,mz),(nx,my,nz),(nx,ny,nz)], (-1,0,0)),
        ([(nx,my,nz),(nx,my,mz),(mx,my,mz),(mx,my,nz)], (0,1,0)),
        ([(nx,ny,mz),(mx,ny,mz),(mx,ny,nz),(nx,ny,nz)], (0,-1,0)),
        ([(nx,ny,mz),(nx,my,mz),(mx,my,mz),(mx,ny,mz)], (0,0,1)),
        ([(mx,ny,nz),(mx,my,nz),(nx,my,nz),(nx,ny,nz)], (0,0,-1)),
    ]
    pos,nrm,uv,idx=[],[],[],[]; base=0
    for verts, normal in faces:
        for i,v in enumerate(verts):
            pos.extend(v); nrm.extend(normal)
            if normal==(1,0,0) or normal==(-1,0,0):
                u=(v[2]-nz)/(mz-nz) if mz!=nz else 0; vv=(v[1]-ny)/(my-ny) if my!=ny else 0
            elif normal==(0,1,0) or normal==(0,-1,0):
                u=(v[0]-nx)/(mx-nx) if mx!=nx else 0; vv=(v[2]-nz)/(mz-nz) if mz!=nz else 0
            else:
                u=(v[0]-nx)/(mx-nx) if mx!=nx else 0; vv=(v[1]-ny)/(my-ny) if my!=ny else 0
            uv.extend([u,vv])
        idx.extend([base,base+1,base+2,base,base+2,base+3]); base+=4
    return pos,nrm,uv,idx

def build_glb(boxes, color, path):
    all_p,all_n,all_uv,all_idx=[],[],[],[]; off=0
    for bmin,bmax in boxes:
        p,n,uv,idx=box_vertices(bmin,bmax)
        all_p.extend(p); all_n.extend(n); all_uv.extend(uv)
        all_idx.extend([i+off for i in idx]); off+=len(p)//3
    pb=struct.pack('<'+'f'*len(all_p),*all_p); nb=struct.pack('<'+'f'*len(all_n),*all_n); ub=struct.pack('<'+'f'*len(all_uv),*all_uv); ib=struct.pack('<'+'H'*len(all_idx),*all_idx)
    def pad(b): return b + b'\x00'*((4-len(b)%4)%4)
    pbp,nbp,ubp,ibp=pad(pb),pad(nb),pad(ub),pad(ib)
    buf=pbp+nbp+ubp+ibp
    off2=0; bv=[]
    def add(l,t):
        nonlocal off2
        bv.append({'buffer':0,'byteOffset':off2,'byteLength':l,'target':t})
        i=len(bv)-1; off2+=l+(4-l%4 if l%4 else 0)
        return i
    bp=add(len(pb),34962); bn=add(len(nb),34962); bu=add(len(ub),34962); bi=add(len(ib),34963)
    xs=all_p[0::3]; ys=all_p[1::3]; zs=all_p[2::3]
    acc=[{'bufferView':bp,'componentType':5126,'count':len(all_p)//3,'type':'VEC3','min':[min(xs),min(ys),min(zs)],'max':[max(xs),max(ys),max(zs)]},{'bufferView':bn,'componentType':5126,'count':len(all_p)//3,'type':'VEC3'},{'bufferView':bu,'componentType':5126,'count':len(all_p)//3,'type':'VEC2'},{'bufferView':bi,'componentType':5123,'count':len(all_idx),'type':'SCALAR'}]
    j={'asset':{'version':'2.0','generator':'charger-diana C2 diana_room'},'scene':0,'scenes':[{'name':'Scene','nodes':[0]}],'nodes':[{'name':path.stem,'mesh':0}],'meshes':[{'name':path.stem,'primitives':[{'attributes':{'POSITION':0,'NORMAL':1,'TEXCOORD_0':2},'indices':3,'material':0}]}],'materials':[{'name':'mat','pbrMetallicRoughness':{'baseColorFactor':[color[0],color[1],color[2],1.0],'metallicFactor':0.0,'roughnessFactor':1.0},'doubleSided':False}],'accessors':acc,'bufferViews':bv,'buffers':[{'byteLength':len(buf)}]}
    jb=json.dumps(j,separators=(',',':')).encode(); jb+=b' '*((4-len(jb)%4)%4)
    total=12+8+len(jb)+8+len(buf)
    glb=struct.pack('<4sII',b'glTF',2,total)+struct.pack('<I4s',len(jb),b'JSON')+jb+struct.pack('<I4s',len(buf),b'BIN\x00')+buf
    path.write_bytes(glb)
    return len(all_idx)//3

# Palet kamar Diana ART_BIBLE: krem #CFC6B4, parket #8A6B4A, plafon #D8D3C4, oak #9A7B52, seprai #B9C4B0
PAL={
    "cream":(0xCF/255,0xC6/255,0xB4/255),
    "parquet":(0x8A/255,0x6B/255,0x4A/255),
    "ceiling":(0xD8/255,0xD3/255,0xC4/255),
    "oak":(0x9A/255,0x7B/255,0x52/255),
    "linen":(0xB9/255,0xC4/255,0xB0/255),
    "linen2":(0xE8/255,0xE0/255,0xD0/255),
    "dark":(0x2A/255,0x2A/255,0x2E/255),
    "fabric":(0x86/255,0x84/255,0x80/255),
    "glass":(0x88/255,0x92/255,0x96/255),
    "metal":(0x2A/255,0x2A/255,0x30/255),
}

kit=[
    ("prolog_wall_2m",      [((0,0,0),(2,2.8,0.12))], PAL["cream"]),
    ("prolog_wall_door_2m", [((0,0,0),(0.6,2.8,0.12)), ((1.6,0,0),(2.2,2.8,0.12)), ((0.6,2.1,0),(1.6,2.8,0.12))], PAL["cream"]),
    ("prolog_wall_window_2m",[((0,0,0),(0.4,2.8,0.12)), ((1.8,0,0),(2.2,2.8,0.12)), ((0.4,0,0),(1.8,1.05,0.12)), ((0.4,1.95,0),(1.8,2.8,0.12))], PAL["cream"]),
    ("prolog_floor_2m",     [((0,0,0),(2,0.08,2))], PAL["parquet"]),
    ("prolog_ceiling_2m",   [((0,0,0),(2,0.08,2))], PAL["ceiling"]),
    ("prolog_door_leaf",    [((0,0,0),(0.9,2.05,0.05))], PAL["cream"]),
    ("prolog_window_glass", [((0,0,0),(1.28,1.0,0.02))], PAL["glass"]),
    ("prolog_bed_frame",    [((0,0,0),(1.5,0.25,2.1))], PAL["oak"]),
    ("prolog_mattress",     [((0,0,0),(1.38,0.2,1.95))], PAL["linen2"]),
    ("prolog_duvet",        [((0,0,0),(1.38,0.12,1.15))], PAL["linen"]),
    ("prolog_pillow",       [((0,0,0),(0.70,0.12,0.42))], PAL["linen"]),
    ("prolog_nightstand",   [((0,0,0),(0.40,0.50,0.42))], PAL["oak"]),
    ("prolog_desk_top",     [((0,0,0),(1.25,0.06,0.65))], PAL["oak"]),
    ("prolog_desk_leg",     [((0,0,0),(0.04,0.68,0.04))], PAL["metal"]),
    ("prolog_chair_seat",   [((0,0,0),(0.46,0.05,0.46))], PAL["fabric"]),
    ("prolog_wardrobe",     [((0,0,0),(0.55,2.0,1.1))], PAL["oak"]),
    ("prolog_shelf_110",    [((0,0,0),(1.05,0.03,0.22))], PAL["oak"]),
    ("prolog_rug",          [((0,0,0),(2.0,0.02,1.7))], PAL["fabric"]),
    ("prolog_charger",      [((0,0,0),(0.08,0.04,0.14)), ((0.03,0.04,0.06),(0.05,0.06,0.08))], PAL["dark"]),
    ("prolog_phone",        [((0,0,0),(0.07,0.015,0.14)), ((0.01,0.015,0.02),(0.06,0.018,0.12))], PAL["dark"]),
    ("prolog_book_stack",   [((0,0,0),(0.22,0.06,0.16)), ((0,0.06,0),(0.22,0.10,0.16)), ((0,0.10,0),(0.22,0.14,0.16))], PAL["linen"]),
    ("prolog_bag",          [((0,0,0),(0.45,0.35,0.22)), ((0.05,0.35,0.05),(0.35,0.38,0.15))], PAL["fabric"]),
]

for name, boxes, col in kit:
    tris=build_glb(boxes, col, OUT/f"{name}.glb")
    print(f"OK {name:30s} tris {tris:3d}")

# verify
import json, struct
for p in sorted(OUT.glob("*.glb")):
    d=p.read_bytes(); cl=struct.unpack('<I',d[12:16])[0]; j=json.loads(d[20:20+cl].decode()); tris=j['accessors'][j['meshes'][0]['primitives'][0]['indices']]['count']//3; print(f" ver {p.name:30s} {tris}")
