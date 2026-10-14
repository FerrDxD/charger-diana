import pathlib
OUT = pathlib.Path(r"C:\Users\Hype GLK\charger-diana\scenes\props\prolog")
OUT.mkdir(parents=True, exist_ok=True)
SRC = pathlib.Path(r"C:\Users\Hype GLK\charger-diana\assets\models\prolog")
MAT = "res://assets/materials/mat_diana_room_atlas.tres"

# size + center for collision boxes (box size, center pos relative to pivot corner)
cols={
    "prolog_wall_2m":((2,2.8,0.12),(1,1.4,0.06)),
    "prolog_wall_door_2m":((2.2,2.8,0.12),(1.1,1.4,0.06)),
    "prolog_wall_window_2m":((2.2,2.8,0.12),(1.1,1.4,0.06)),
    "prolog_floor_2m":((2,0.08,2),(1,0.04,1)),
    "prolog_ceiling_2m":((2,0.08,2),(1,0.04,1)),
    "prolog_door_leaf":((0.9,2.05,0.05),(0.45,1.025,0.025)),
    "prolog_window_glass":((1.28,1.0,0.02),(0.64,0.5,0.01)),
    "prolog_bed_frame":((1.5,0.25,2.1),(0.75,0.125,1.05)),
    "prolog_mattress":((1.38,0.2,1.95),(0.69,0.1,0.975)),
    "prolog_duvet":((1.38,0.12,1.15),(0.69,0.06,0.575)),
    "prolog_pillow":((0.70,0.12,0.42),(0.35,0.06,0.21)),
    "prolog_nightstand":((0.40,0.50,0.42),(0.20,0.25,0.21)),
    "prolog_desk_top":((1.25,0.06,0.65),(0.625,0.03,0.325)),
    "prolog_desk_leg":((0.04,0.68,0.04),(0.02,0.34,0.02)),
    "prolog_chair_seat":((0.46,0.05,0.46),(0.23,0.025,0.23)),
    "prolog_wardrobe":((0.55,2.0,1.1),(0.275,1.0,0.55)),
    "prolog_shelf_110":((1.05,0.03,0.22),(0.525,0.015,0.11)),
    "prolog_rug":((2.0,0.02,1.7),(1.0,0.01,0.85)),
    "prolog_charger":((0.08,0.06,0.14),(0.04,0.03,0.07)),
    "prolog_phone":((0.07,0.02,0.14),(0.035,0.01,0.07)),
    "prolog_book_stack":((0.22,0.14,0.16),(0.11,0.07,0.08)),
    "prolog_bag":((0.45,0.38,0.22),(0.225,0.19,0.11)),
}

for glb in sorted(SRC.glob("*.glb")):
    name=glb.stem
    rel=f"res://assets/models/prolog/{glb.name}"
    sx,sy,sz = cols.get(name, (1,1,1))[0]
    cx,cy,cz = cols.get(name, (1,1,1))[1]
    tscn = (
        "[gd_scene load_steps=3 format=3]\n"
        f"[ext_resource type=\"PackedScene\" path=\"{rel}\" id=\"1_glb\"]\n"
        f"[ext_resource type=\"Material\" path=\"{MAT}\" id=\"2_mat\"]\n"
        "\n"
        "[sub_resource type=\"BoxShape3D\" id=\"BoxShape_1\"]\n"
        f"size = Vector3({sx:g}, {sy:g}, {sz:g})\n"
        "\n"
        f"[node name=\"{name}\" type=\"Node3D\"]\n"
        "\n"
        "[node name=\"Visual\" type=\"Node3D\" parent=\".\"]\n"
        f"[node name=\"{name}\" parent=\"Visual\" instance=ExtResource(\"1_glb\")]\n"
        "material_override = ExtResource(\"2_mat\")\n"
        "\n"
        "[node name=\"Collision\" type=\"StaticBody3D\" parent=\".\"]\n"
        "\n"
        "[node name=\"CollisionShape3D\" type=\"CollisionShape3D\" parent=\"Collision\"]\n"
        f"transform = Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, {cx:g}, {cy:g}, {cz:g})\n"
        "shape = SubResource(\"BoxShape_1\")\n"
    )
    (OUT/f"{name}.tscn").write_text(tscn,encoding="utf-8")
    print("OK",name)

print("done",len(list(SRC.glob("*.glb"))))
