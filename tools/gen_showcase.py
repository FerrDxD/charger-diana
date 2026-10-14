import pathlib

base = pathlib.Path(r"C:\Users\Hype GLK\charger-diana")
# Build a showcase scene that tiles the modular kit on a 2m grid without gaps.
# Grid layout (top view, y up, origin at corner):
#   Floor 4x2 tiles covering 8x4 meters, walls around, door and window frames included.
#   Also demonstrates corner, T, cross, pillar, stairs, gate, fence.

out = base / "scenes" / "level" / "LevelModularShowcase.tscn"
props = {
    "wall_s2": "res://scenes/props/arch/arch_wall_straight_2m.tscn",
    "wall_s4": "res://scenes/props/arch/arch_wall_straight_4m.tscn",
    "wall_c": "res://scenes/props/arch/arch_wall_corner.tscn",
    "wall_t": "res://scenes/props/arch/arch_wall_t_2m.tscn",
    "wall_x": "res://scenes/props/arch/arch_wall_cross_2m.tscn",
    "wall_door": "res://scenes/props/arch/arch_wall_door_frame_2m.tscn",
    "wall_win": "res://scenes/props/arch/arch_wall_window_frame_2m.tscn",
    "floor": "res://scenes/props/arch/arch_floor_2m.tscn",
    "floor1": "res://scenes/props/arch/arch_floor_tile_1m.tscn",
    "ceil": "res://scenes/props/arch/arch_ceiling_2m.tscn",
    "pillar02": "res://scenes/props/arch/arch_pillar_02m.tscn",
    "pillar04": "res://scenes/props/arch/arch_pillar_04m.tscn",
    "door": "res://scenes/props/arch/arch_door_leaf_2m.tscn",
    "win": "res://scenes/props/arch/arch_window_leaf_1m.tscn",
    "stairs": "res://scenes/props/arch/arch_stairs_2m.tscn",
    "railing": "res://scenes/props/arch/arch_railing_2m.tscn",
    "fence": "res://scenes/props/arch/arch_fence_2m.tscn",
    "gate": "res://scenes/props/arch/arch_gate_main_4m.tscn",
    "cornice": "res://scenes/props/arch/arch_cornice_2m.tscn",
    "baseb": "res://scenes/props/arch/arch_baseboard_2m.tscn",
}

# Assign ext ids
keys = list(props.keys())
ext_lines = []
id_map = {}
for i, k in enumerate(keys, start=1):
    ext_lines.append(f'[ext_resource type="PackedScene" path="{props[k]}" id="{i}_{k}"]')
    id_map[k] = f"{i}_{k}"

# Layout: build a room 8x4 (floor 4x2 tiles), walls perimeter, interior T wall, etc.
# Coordinates: X east, Z south (Godot). Grid 2m.
nodes = []

def inst(key, pos, yaw_deg=0):
    # pos = (x, y, z) corner pivot; yaw around Y
    x, y, z = pos
    idv = id_map[key]
    name = f"{key}_{len(nodes)}"
    if yaw_deg == 0:
        tr = f"Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, {x:g}, {y:g}, {z:g})"
    else:
        import math
        rad = math.radians(yaw_deg)
        c, s = math.cos(rad), math.sin(rad)
        tr = f"Transform3D({c:g}, 0, {s:g}, 0, 1, 0, {-s:g}, 0, {c:g}, {x:g}, {y:g}, {z:g})"
    nodes.append((name, idv, tr))

# Floor 8x4 => 4 floortiles along X, 2 along Z (2m tiles)
for fx in [0, 2, 4, 6]:
    for fz in [0, 2]:
        inst("floor", (fx, 0, fz))

# Ceiling same
for fx in [0, 2, 4, 6]:
    for fz in [0, 2]:
        inst("ceil", (fx, 3.2, fz))

# Perimeter walls
# South wall z=0, facing north: straight 4 + straight 4 (with door at second segment -> use door frame instead of straight)
inst("wall_s4", (0, 0, 0))  # south 0-4
inst("wall_door", (4, 0, 0)) # south 4-6 with door
inst("wall_s2", (6, 0, 0))  # south 6-8
# North wall z=4 (need rotation 180 so thickness inward)
inst("wall_s4", (4, 0, 4), 180)
inst("wall_s2", (2, 0, 4), 180)
inst("wall_win", (0, 0, 4), 180) # window on north-west

# East wall x=8, z 0-4 (rot 270)
inst("wall_s2", (8, 0, 0), 270)
inst("wall_s2", (8, 0, 2), 270)
# West wall x=0 (rot 90)
inst("wall_s2", (0, 0, 2), 90)
inst("wall_s2", (0, 0, 0), 90)

# Interior T and cross demo (center of room)
inst("wall_t", (4, 0, 2), 0)  # T at center
# Pillars at corners
inst("pillar04", (0, 0, 0))
inst("pillar04", (8-0.4, 0, 0))
inst("pillar04", (0, 0, 4-0.4))
inst("pillar04", (8-0.4, 0, 4-0.4))

# Door leaf placed inside the door frame (hinged left, inset)
inst("door", (4.05, 0, 0.075))
# Window leaves
inst("win", (0.5, 1.0, 4-0.05-0.075))
# Fence + gate showcase slightly outside the room
inst("fence", (0, 0, -2), 0)
inst("gate", (2, 0, -2), 0)
inst("fence", (6, 0, -2), 0)
# Stairs demo outside
inst("stairs", (9, 0, 0), 0)
inst("railing", (9, 0, 2), 270)

# Baseboard along south wall (shows snap)
inst("baseb", (0, 0, 0.15), 0)
inst("baseb", (2, 0, 0.15), 0)
# Cornice along south wall top
inst("cornice", (0, 0, 0), 0)
inst("cornice", (2, 0, 0), 0)

# Player start and lighting for testing
header = "[gd_scene load_steps={} format=3]\n\n".format(len(keys)+1)
header += "\n".join(ext_lines) + "\n"
header += "\n[sub_resource type=\"Environment\" id=\"Environment_1\"]\nbackground_mode = 1\nbackground_color = Color(0.02, 0.02, 0.03, 1)\nambient_light_color = Color(0.12, 0.12, 0.14, 1)\nambient_light_energy = 1.2\nfog_enabled = true\nfog_light_color = Color(0.06, 0.06, 0.08, 1)\nfog_density = 0.02\n"
header += "\n[node name=\"LevelModularShowcase\" type=\"Node3D\"]\n"
header += "\n[node name=\"WorldEnvironment\" type=\"WorldEnvironment\" parent=\".\"]\nenvironment = SubResource(\"Environment_1\")\n"
header += "\n[node name=\"ModularKit\" type=\"Node3D\" parent=\".\"]\n"

body = ""
for name, idv, tr in nodes:
    body += f"\n[node name=\"{name}\" parent=\"ModularKit\" instance=ExtResource(\"{idv}\")]\ntransform = {tr}\n"

body += "\n[node name=\"Player\" type=\"Node3D\" parent=\".\"]\n"
body += "[node name=\"Camera3D\" type=\"Camera3D\" parent=\"Player\"]\ntransform = Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, 4, 1.6, 2)\ncurrent = true\nfov = 75.0\n"
body += "\n[node name=\"DirectionalLight3D\" type=\"DirectionalLight3D\" parent=\".\"]\ntransform = Transform3D(0.7, -0.5, 0.5, 0, 0.7, 0.7, -0.7, -0.5, 0.5, 0, 8, 0)\nlight_energy = 0.9\nshadow_enabled = false\n"
body += "\n[node name=\"OmniLight3D\" type=\"OmniLight3D\" parent=\".\"]\ntransform = Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, 4, 2.8, 2)\nomni_range = 12.0\nlight_color = Color(0.9, 0.9, 0.85, 1)\nlight_energy = 1.0\nshadow_enabled = false\n"

out.write_text(header + body, encoding="utf-8")
print(f"Wrote {out} with {len(nodes)} instances")
