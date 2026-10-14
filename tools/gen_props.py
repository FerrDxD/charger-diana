import pathlib

arch_dir = pathlib.Path(r"C:\Users\Hype GLK\charger-diana\assets\models\arch")
out_dir = pathlib.Path(r"C:\Users\Hype GLK\charger-diana\scenes\props\arch")
out_dir.mkdir(parents=True, exist_ok=True)

collisions = {
    "arch_wall_straight_2m": (2, 3.2, 0.2),
    "arch_wall_straight_4m": (4, 3.2, 0.2),
    "arch_wall_corner": (2, 3.2, 2),
    "arch_wall_t_2m": (2, 3.2, 2),
    "arch_wall_cross_2m": (2, 3.2, 2),
    "arch_wall_door_frame_2m": (2, 3.2, 0.2),
    "arch_wall_window_frame_2m": (2, 3.2, 0.2),
    "arch_wall_half_1m": (2, 1.6, 0.2),
    "arch_wall_solid_1m": (1, 3.2, 0.2),
    "arch_floor_2m": (2, 0.1, 2),
    "arch_floor_tile_1m": (1, 0.1, 1),
    "arch_ceiling_2m": (2, 0.1, 2),
    "arch_roof_flat": (2, 0.15, 2),
    "arch_pillar_02m": (0.2, 3.2, 0.2),
    "arch_pillar_04m": (0.4, 3.2, 0.4),
    "arch_door_leaf_2m": (1, 2.0, 0.05),
    "arch_window_leaf_1m": (1, 1.2, 0.05),
    "arch_stairs_2m": (2, 1.0, 2),
    "arch_railing_2m": (2, 1.0, 0.05),
    "arch_fence_2m": (2, 1.2, 0.05),
    "arch_gate_main_4m": (4, 2.2, 0.15),
    "arch_cornice_2m": (2, 0.2, 0.25),
    "arch_baseboard_2m": (2, 0.15, 0.22),
}

for glb in sorted(arch_dir.glob("*.glb")):
    name = glb.stem
    rel_glb = f"res://assets/models/arch/{glb.name}"
    mat_path = "res://assets/materials/mat_arch_atlas.tres"
    sx, sy, sz = collisions.get(name, (2, 3.2, 0.2))
    col_size = f"Vector3({sx:g}, {sy:g}, {sz:g})"
    col_pos = f"Vector3({sx/2:g}, {sy/2:g}, {sz/2:g})"
    if name == "arch_cornice_2m":
        col_pos = f"Vector3({sx/2:g}, 3.1, {sz/2:g})"
    if name == "arch_baseboard_2m":
        col_pos = f"Vector3({sx/2:g}, 0.075, {sz/2:g})"

    tscn = (
        "[gd_scene load_steps=3 format=3]\n"
        f"[ext_resource type=\"PackedScene\" path=\"{rel_glb}\" id=\"1_glb\"]\n"
        f"[ext_resource type=\"Material\" path=\"{mat_path}\" id=\"2_mat\"]\n"
        "\n"
        "[sub_resource type=\"BoxShape3D\" id=\"BoxShape_1\"]\n"
        f"size = {col_size}\n"
        "\n"
        f"[node name=\"{name}\" type=\"Node3D\"]\n"
        "\n"
        "[node name=\"Visual\" type=\"Node3D\" parent=\".\"]\n"
        "\n"
        f"[node name=\"{name}\" parent=\"Visual\" instance=ExtResource(\"1_glb\")]\n"
        "material_override = ExtResource(\"2_mat\")\n"
        "\n"
        "[node name=\"Collision\" type=\"StaticBody3D\" parent=\".\"]\n"
        "\n"
        "[node name=\"CollisionShape3D\" type=\"CollisionShape3D\" parent=\"Collision\"]\n"
        f"transform = Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, {col_pos[8:-1]})\n"
        "shape = SubResource(\"BoxShape_1\")\n"
    )
    out = out_dir / f"{name}.tscn"
    out.write_text(tscn, encoding="utf-8")
    print(f"OK {name} -> {out}")

print("done", len(list(arch_dir.glob("*.glb"))))
